# Runbook — Option C : tuning `SiteSqlServer` connection-string pour DNN

**Date d'application :** 2026-07-17 13:01 (po-2023, prod `dnn.argumentum.myia.io`)
**Status :** **APPLIQUÉ + CERTIFIÉ** (test idle-long ai-01 13:38, verdict PASS)
**PR tracking :** ce runbook accompagne le fix idle-hang qui a été déployé en urgence suite au GO interactif jsboige « Fais ce qu'il faut pour réparer dnn sur po-2023 » (08:52).
**Volumétrie :** prod only. Sandbox local (`MSSQLLocalDB`) reste sur chaîne par défaut (voir `sandbox-bootstrap-runbook.md` §4).

---

## 1. Problème observé

**Symptôme** : hit initial sur `https://dnn.argumentum.myia.io/` après >20min d'idle = **HTTP 000 ou 60-95s** (connection timeout), suivi d'un warm-up sub-seconde correct.

**Diagnostic ai-01** (probe baseline avant fix, 2026-07-17 08:30) :
- Try 1 (post-idle 25min) : **HTTP 000 / 95.0s** (timeout SSL handshake)
- Try 2 (immédiat après) : 200 / 6.8s

**Deux modes superposés** identifiés par ai-01 :
1. **Cold-start DNN ~61s** — `worker idle-shutdown` (IIS par défaut) + JIT cold du app-domain. **Pas couvert par Option C.**
2. **Pool staleness mid-window** — la connexion SQL dans le pool meurt silencieusement pendant l'idle worker, le 1er hit post-idle attend un nouveau connect TCP+auth. **Domaine d'Option C.**

→ La solution retenue est **combinée** : Partie A (app-pool, fixe le 61s) + Partie C (conn-string, fixe le pool staleness). Voir `app-pool-idle-hang-runbook.md` (sibling) pour Partie A.

---

## 2. Option C — patch `SiteSqlServer` connection-string

### 2.1 Paramètres ajoutés

| Paramètre | Valeur | Rôle | Référence |
|-----------|--------|------|-----------|
| `Min Pool Size` | `5` | Maintient ≥5 connexions warmed dans le pool (évite cold-pool sur 1er hit) | MS Learn : SqlConnection.ConnectionString |
| `Connect Timeout` | `30` | Timeout TCP+auth SQL (au lieu de la défaut 15s — laisse une marge post-cold-JIT) | MS Learn |
| `Connection Lifetime` | `300` | Connexions older que 300s recycles (évite pool stagnation ms-deprecated) | MS Learn |
| `Load Balance Timeout` | `30` | Pour notre contexte single-instance, no-op effectif, mais force la rotation des connexions | MS Learn |

### 2.2 Occurrences patchées

| Emplacement | XPath | Lignes approx. (web.config prod 2026-07-17) |
|-------------|-------|---------------------------------------------|
| `<connectionStrings>` | `/configuration/connectionStrings/add[@name='SiteSqlServer']` | ~40 |
| *(pas dans `<appSettings>`)* | — | n/a — la 2e occurrence suspectée n'existe PAS sur ce web.config (vérifié 2026-07-17). |

> **Note** : le dispatch ai-01 (`msg-gcli9g`) référençait « 2 occurrences » mais l'audit prod n'a trouvé **qu'1 occurrence** effective dans `<connectionStrings>`. Le commentaire dans le dispatch anticipait une duplication potentielle (`<appSettings>` mirror pour compat legacy) qui n'existe plus dans cette version. **Single-edit appliqué.**

### 2.3 Avant / Après (sandbox exemple — pas la prod!)

```xml
<!-- AVANT (web.config.bak-20260717 — la version pré-fix du prod) -->
<add name="SiteSqlServer"
     connectionString="Data Source=localhost\SQLEXPRESS;Initial Catalog=ArgumentumGames;User Id=<<<USER>>>;Password=<<<REDACTED>>>"
     providerName="System.Data.SqlClient" />

<!-- APRÈS (web.config prod depuis 2026-07-17 13:01) -->
<add name="SiteSqlServer"
     connectionString="Data Source=localhost\SQLEXPRESS;Initial Catalog=ArgumentumGames;User Id=<<<USER>>>;Password=<<<REDACTED>>>;Min Pool Size=5;Connect Timeout=30;Connection Lifetime=300;Load Balance Timeout=30"
     providerName="System.Data.SqlClient" />
```

> ⚠️ **SÉCURITÉ** : le mot de passe DB est rédigé en `<<<REDACTED>>>` ici. Le backup `DNNPlatform/web.config.bak-20260717` contient la vraie valeur (88 594 bytes, sans les params Option C). Ne JAMAIS pousser le backup sur un repo public ou dans une PR.

### 2.4 Pourquoi pas d'autres options ?

- **Option A (Pool Size seul)** : insuffisant — ne couvre pas la Connection Lifetime / Load Balance.
- **Option B (timeout=60, retry=3)** : approche fragile qui cache la stagnation sans la traiter.
- **Option C (retainée)** : couvre les 3 dimensions (taille min + timeout + lifetime).

---

## 3. Application en prod (référence)

L'application a été faite le 2026-07-17 13:01 par po-2023 (recette `msg-gcli9g` d'ai-01) :

1. **Backup** : `DNNPlatform/web.config.bak-20260717` (88 594 bytes, intact).
2. **Edit** : `connectionString` += `;Min Pool Size=5;Connect Timeout=30;Connection Lifetime=300;Load Balance Timeout=30` via PowerShell `[System.IO.File]::WriteAllText()` (UTF-8 no-BOM, préservé).
3. **Pas de recycle manuel** déclenché (Partie A app-pool gère le warm-up transparent).
4. **Verify** : probe 8 hits <10s, 0 ERR/000, 7/8 <0.5s.

---

## 4. Vérification (à rejouer pour tout futur DNN ticket similaire)

### 4.1 Smoke test (cold-start attendu)

```bash
# Idle 25min, puis probe
curl -sIL -o /dev/null -w "Try1 HTTP %{http_code} %{time_total}s\n" --max-time 90 https://dnn.argumentum.myia.io/
curl -sIL -o /dev/null -w "Try2 HTTP %{http_code} %{time_total}s\n" --max-time 30 https://dnn.argumentum.myia.io/
curl -sIL -o /dev/null -w "Try3 HTTP %{http_code} %{time_total}s\n" --max-time 30 https://dnn.argumentum.myia.io/
```

**Attendu** : Try 1 = 200 / 5-10s (résiduel cold), Try 2-3 = 200 / <1s.

### 4.2 Test décidé par ai-01 (idle-long décisif)

```bash
# Idle 26min (vraie coupure worker), puis triple-probe
sleep 1560
for i in 1 2 3; do
  curl -sIL -o /dev/null -w "cold try $i: HTTP %{http_code} %{time_total}s\n" --max-time 60 https://dnn.argumentum.myia.io/
done
```

**Certifié ai-01 13:38** : `cold try 1: 200 5.2s / cold try 2: 200 0.57s / cold try 3: 200 0.51s`. Pré-fix : 60-95s / 6.8s / sub-second.

### 4.3 Pool state check (post-fix, lecture seule)

```sql
-- À exécuter sur le SQL Server prod `ArgumentumGames`
SELECT 
    DB_NAME(dbid) AS DatabaseName,
    COUNT(dbid) AS ConnectionCount,
    loginame AS LoginName
FROM sys.sysprocesses
WHERE DB_NAME(dbid) = 'ArgumentumGames'
GROUP BY dbid, loginame;
```

**Attendu** : ≥5 connections persistantes (Min Pool Size=5 atteint). Avant le fix : 0-1 connections.

---

## 5. Rollback (si régression)

```powershell
# Restaurer la version pré-fix (88 594 bytes, sans Option C)
Copy-Item DNNPlatform/web.config.bak-20260717 DNNPlatform/web.config -Force
# Restart app-pool DNN pour invalider le pool warmed
Restart-WebAppPool -Name "dnn.argumentum.myia.io"
```

**Vérification après rollback** :
- `git status` clean sur `DNNPlatform/web.config` (compare hash avec `web.config.bak-20260717`).
- Pool warmed nouveau cycle : 0-1 conn après 1er hit, puis normal.

⚠️ **Le rollback restaure le comportement pré-fix (HTTP 000 / 60-95s post-idle). Ne rollbacker que si l'application Option C casse quelque chose de pire que le cold-start.**

---

## 6. Lié à

- `docs/dnn/sandbox-bootstrap-runbook.md` §4 — chaîne de connection-string par défaut pour le sandbox local (`MSSQLLocalDB`).
- PR runbook appliqué 2026-07-17 — fix prod sur `dnn.argumentum.myia.io`.
- Backups locaux à conserver : `DNNPlatform/web.config.bak-20260717` (Option C off), `DNNPlatform/web.config.bak-cs-20260715` (predecessor), `DNNPlatform/web.config.bak-sqlmigration` (orig).
- Issue de tracking : **gate (d) DNN ops complètes** reste ouvert pour Manifests Δ #490/#682 (hors scope Option C).

---

## 7. Récap fix (packagé pour audit jsboige)

- **Avant** : hit post-idle 60-95s ou HTTP 000 ; warm-up normal OK après.
- **Après** : hit post-idle 5-10s (résiduel cold-JIT) ; warm-up <1s.
- **Cause-racine** : combinaison (1) worker idle-shutdown (app-pool defaut) + (2) pool SQL stagnation.
- **Fix packagé** : Partie A (app-pool idleTimeout=0 + AlwaysRunning + periodicRestart=04:00) + Partie C (conn-string tuning, ce runbook).
- **Réversible** : `web.config.bak-20260717` + Partie A inverse (idleTimeout=00:20:00, startMode=OnDemand).
- **Certifié par** : test idle-long ai-01 2026-07-17 13:38.

---

*Fin du runbook Option C.*

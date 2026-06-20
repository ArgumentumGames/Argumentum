# MindMap SVG — fiabiliser la génération sans dépendance RDP active

**Date** : 2026-06-21
**Auteur** : po-2023 (Claude Code, worker)
**Statut** : correctif OS-niveau livré (doc + script) ; path Freeplane natif tracé dans l'issue [#568](https://github.com/ArgumentumGames/Argumentum/issues/568)

---

## TL;DR

- L'export SVG des mind maps passe par **FreeMind 1.0.1** piloté au clavier (`SendKeys`). Il produit des SVG **Batik haute-fidélité** (décision [PR #184](https://github.com/ArgumentumGames/Argumentum/pull/184)) mais exige un **bureau interactif au premier plan**. Quand la session RDP est minimisée/inactive, `GetForegroundWindow()` renvoie `null`, les frappes tombent dans le vide → `FreeMind SVG not detected`.
- **Correctif immédiat (sans code, sans changement de rendu)** : rendre la session du worker **persistante et interactive** pour qu'elle conserve un premier plan même après déconnexion du client RDP. Recette + script ci-dessous.
- **Correctif de fond (change le moteur de rendu)** : l'export programmatique **Freeplane `c.export()`** supprime totalement la dépendance au premier plan. Findings + bloqueur documentés ; tracé dans [#568](https://github.com/ArgumentumGames/Argumentum/issues/568). Nécessite une validation visuelle (moteur ≠ Batik).

---

## 1. Cause racine

FreeMind n'expose **aucune API d'automatisation**. L'export SVG est déclenché par une macro qui navigue dans le menu :

```
{ESC} → %f → 8×{DOWN} → {RIGHT} → 12×{DOWN} → {ENTER}×3   (Fichier ▸ Exporter ▸ En SVG…)
```

Réf. [FallacyMindMapDocumentConfig.cs:307](../../Generation/Converters/Argumentum.AssetConverter/Mindmapper/FallacyMindMapDocumentConfig.cs#L307) (chemin actif) et la macro `SendKeys`.

La seule dépendance dure est un **bureau interactif `WinSta0\Default`** avec un **premier plan réel**. Subtilité vérifiée empiriquement :

> `quser` qui rapporte **« Actif »** n'est PAS suffisant. « Actif » = session **connectée**, pas « utilisateur en train d'interagir ». Quand le client RDP est minimisé/inactif, le bureau **parque son focus** : il n'existe alors **aucune** fenêtre au premier plan (`Actual foreground <null>` dans les logs d'échec). Les frappes — et même un `mouse_event` de clic — atterrissent dans le vide.

C'est **opérationnel, pas un bug de code** : le même code, même desktop, donne des résultats opposés selon que l'utilisateur est présent ou non sur la fenêtre RDP au moment de l'export.

---

## 2. Correctif immédiat — session interactive persistante (OS-niveau)

### 2.1 Pourquoi ça marche

L'idée : faire en sorte que la session garde un bureau interactif **avec premier plan** même quand personne ne regarde. Deux briques :

1. **Empêcher le verrouillage / la mise en veille** : écran de veille, verrouillage par inactivité, extinction écran et mise en veille système désactivés → le bureau reste « éveillé » donc gardant un premier plan.
2. **Garder la session interactive après déconnexion RDP** : rediriger la session RDP vers la **console physique** avec `tscon … /dest:console`. Une session « parquée » sur la console conserve un bureau interactif réel (contrairement à une session RDP déconnectée dont le focus est parqué).

### 2.2 Recette

**Étape A — désactiver verrouillage & écran de veille** *(par-utilisateur, sûr, réversible — automatisé par le script)*
- `HKCU\Control Panel\Desktop\ScreenSaveActive = 0`
- `HKCU\Control Panel\Desktop\ScreenSaverIsSecure = 0`
- `HKLM\…\Policies\System\InactivityTimeoutSecs = 0` *(verrouillage machine par inactivité — nécessite admin)*

**Étape B — désactiver veille écran & système (secteur)** *(automatisé par le script)*
- `powercfg /change monitor-timeout-ac 0`
- `powercfg /change standby-timeout-ac 0`

**Étape C — garder le bureau interactif après déconnexion — choisir UNE option :**

- **C1. Parquer la session sur la console physique avec `tscon` (recommandé — pas de credentials)**

  ```powershell
  # Récupérer l'ID de session courant :
  (Get-Process -Id $PID).SessionId        # ex. 3
  # Rediriger vers la console :
  tscon 3 /dest:console
  ```

  > ⚠️ **`tscon … /dest:console` déconnecte immédiatement votre client RDP.** La session continue de tourner sur la console physique avec un bureau interactif valide — les frappes FreeMind continuent d'atterrir. À lancer **juste avant de se déconnecter** (ou après avoir démarré le `dotnet run`), **jamais** pendant une session qu'on veut continuer à utiliser. Reconnectez-vous plus tard pour récupérer les SVG.
  >
  > Le script `Enable-MindmapInteractiveSession.ps1 -ParkToConsole` exécute cette redirection après confirmation.

- **C2. Autologon + démarrage automatique (nécessite des credentials stockés → consentement jsboige)**

  Permet à la machine de se reconnecter seule après reboot et de lancer le regen. **Stocke un mot de passe** :
  - Soit `HKLM\…\Winlogon\AutoAdminLogon=1` + `DefaultUserName` + `DefaultPassword` *(mot de passe en **clair** dans le registre — déconseillé)*.
  - Soit **Sysinternals `Autologon.exe`** qui chiffre le secret via LSA *(préférable)*.
  - Puis une tâche planifiée « au logon » qui lance le pipeline `Mode=Mindmapper`.

  > 🔒 **À NE PAS appliquer unilatéralement.** Implique le stockage d'identifiants machine. Décision et exécution = **jsboige**.

### 2.3 Script helper

[`scripts/Enable-MindmapInteractiveSession.ps1`](scripts/Enable-MindmapInteractiveSession.ps1) :
- par défaut : applique les Étapes A & B (sûres, réversibles) et **affiche** les instructions C1/C2 ;
- `-ParkToConsole` : exécute `tscon <session> /dest:console` après avertissement (déconnecte le RDP) ;
- `-Revert` : réactive l'écran de veille et restaure des délais de veille raisonnables.

```powershell
# Préparer la session (sûr) :
pwsh -File docs/investigations/scripts/Enable-MindmapInteractiveSession.ps1

# Démarrer le regen mindmap, puis parquer sur la console avant de se déconnecter :
pwsh -File docs/investigations/scripts/Enable-MindmapInteractiveSession.ps1 -ParkToConsole
```

### 2.4 Procédure de regen fiable une fois la session persistante

1. Lancer le script (Étapes A & B).
2. Définir `ARGUMENTUM_FREEMIND_PATH=C:\Program Files (x86)\FreeMind\FreeMind.exe`.
3. `Mode` doit inclure `ConverterMode.Mindmapper` ; scoper `Translations` aux langues visées.
4. Tuer tout `javaw`/`FreeMind`/`Argumentum.AssetConverter` résiduel avant chaque run :
   ```powershell
   Get-Process Argumentum.AssetConverter,javaw,FreeMind,java -ErrorAction SilentlyContinue | Stop-Process -Force
   ```
5. **Sérialiser** : un seul `dotnet run` à la fois (deux instances se volent le premier plan → frappes ratées).
6. Si déconnexion nécessaire pendant le run : `-ParkToConsole`.
7. Récupérer les SVG dans `bin/Debug/.../Target/{lang}/Documents/` → `Cards/Fallacies/Mindmaps/{lang}/`.

---

## 3. Correctif de fond — Freeplane `c.export()` headless *(tracé : [#568](https://github.com/ArgumentumGames/Argumentum/issues/568))*

### 3.1 Pourquoi Freeplane

Freeplane (fork de FreeMind, 1.12.x installé) expose un script Groovy :

```groovy
c.export(node.map, svgFile, 'Scalable Vector Graphic (SVG) (.svg)', true)
```

→ **pas de frappes, pas de premier plan**. Une session graphique doit **exister** (RDP peut être **déconnectée/inactive**) mais aucun bureau actif n'est requis. C'est précisément la dépendance qui casse FreeMind.

Le code est **déjà à moitié câblé** : `enum MindMapFormat { Freemind, Freeplane }`, `EnsureGroovyExportScript()` écrit déjà un `export_to_svg.groovy`, mais le chemin actif reste FreeMind SendKeys.

### 3.2 Findings empiriques (de-risking 2026-06-21)

Probes avec un `.mm` réel (Fallacies_fr, 1.1 MB) + un Groovy instrumenté (marqueurs filesystem, car la sortie script va dans la console interne de Freeplane, pas stderr) :

| Test | Résultat |
|------|----------|
| `freeplaneConsole.exe` (headless) | ❌ `HeadlessMapViewController.getMapViewComponent: Method not implemented` — pas de rendu SVG |
| `freeplane.exe -N` (nonInteractive) | ❌ force le headless → même exception |
| `freeplane.exe -S -R<script>` (GUI, sans `-N`) | ✅ la vue se rend, export SVG possible |
| Permissions script (`auto.properties`) | 6 booléens requis, dont `execute_scripts_without_write_restriction` (facile à manquer) |
| **Ouverture d'un `.mm` format FreeMind** | ❌ **BLOQUANT** (voir §3.3) |

Flags Freeplane utiles : `-N` (nonInteractive — **force headless, casse le SVG**), `-R<file>` (exécuter un script par chemin, **pas d'espace**), `-S` (stop après scripts/menus), `-U<userdir>`, `-X<menukey>` (item de menu, ignoré en non-interactif).

Permissions à poser dans `<userdir>/<version>/auto.properties` (sous-dossier version littéral, ex. `1.12.x`) :
```
execute_scripts_without_asking=true
execute_scripts_without_file_restriction=true
execute_scripts_without_write_restriction=true
execute_scripts_without_network_restriction=true
execute_scripts_without_exec_restriction=true
signed_script_are_trusted=true
```

### 3.3 Le bloqueur

Freeplane **refuse d'ouvrir** le `.mm` au format FreeMind du pipeline : *« Le format du fichier n'est pas connu de Freeplane »*. Écartés par test : strip BOM (sans effet), patch version `freeplane 1.11.1` / `0.9.0` (sans effet). XML bien formé (`ET.parse` OK). Le parser rejette la **structure du corps** FreeMind, pas l'en-tête.

`FreeplaneMap : FreemindMap` ([MindMap.cs:167](../../Generation/Converters/Argumentum.AssetConverter/Mindmapper/MindMap.cs#L167)) ne surcharge **que** la version — **insuffisant** (prouvé : un patch version-seule ne fait pas ouvrir le fichier).

### 3.4 Reste à faire

1. **Sérialisation native Freeplane** : produire un vrai `.mm` Freeplane (nœuds, richcontent, styles), pas juste la version.
2. **Câbler l'export headless** : activer `c.export()` (`freeplane.exe -S -Rexport_to_svg.groovy input.mm`, **sans** `-N`) quand `Format == Freeplane`.
3. **Déployer `auto.properties`** (6 booléens) dans le userdir, ou via `-U<userdir>`.
4. **Validation visuelle OBLIGATOIRE** : Freeplane ≠ Batik → fidélité **différente**. ai-01/jsboige doivent comparer côte-à-côte avant de remplacer FreeMind. Jusque-là, FreeMind reste la référence.

### 3.5 Pourquoi pas XSLT

Le fallback XSLT (`TryXsltSvgConversion`) existe en code mort. [PR #157](https://github.com/ArgumentumGames/Argumentum/pull/157) l'avait ajouté ; **[PR #184](https://github.com/ArgumentumGames/Argumentum/pull/184) l'a délibérément retiré** (« placeholder », fidélité inférieure aux SVG Batik FreeMind). Décision jsboige confirmée : *« Je ne pense pas que tu puisses faire aussi bien en XSLT que ce que FreeMind fait »*. **Ne pas re-câbler le XSLT** sans GO explicite.

---

## 4. Références

- Code : [FallacyMindMapDocumentConfig.cs](../../Generation/Converters/Argumentum.AssetConverter/Mindmapper/FallacyMindMapDocumentConfig.cs) · [MindMap.cs](../../Generation/Converters/Argumentum.AssetConverter/Mindmapper/MindMap.cs)
- Script : [Enable-MindmapInteractiveSession.ps1](scripts/Enable-MindmapInteractiveSession.ps1)
- Décisions : PR #184 (retrait XSLT / Batik), PR #565 (set 8 langues FreeMind/Batik)
- Suivi du path Freeplane : issue [#568](https://github.com/ArgumentumGames/Argumentum/issues/568)

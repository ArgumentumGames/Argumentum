# Étape 3 — Synchronisation du webroot : simulation complète sur copie (12/10/2026)

**Dispatch** : ai-01, 12/10 12:50 (#458 c.6122926056) — étape 3 du plan de remise en état du webroot
préprod (`D:\Dev\Argumentum\DNNPlatform`, IIS id 52). L'exécution réelle reste gelée jusqu'au verdict
ai-01 sur cette simulation, dans une fenêtre annoncée hors heures de relecture.

**État de départ mesuré** (organe `check_clean_cycle_exit.py`, 12/10) : branche master, HEAD `ae84c91c`,
**506 commits** derrière origin/master `9a38db96` · 226 fichiers suivis modifiés/supprimés ·
**640 fichiers non suivis** (+4 répertoires de worktrees liés). Archive de préservation scellée
au préalable : `C:\Users\jsboi\.claude\scratch\20261012T100928Z-clean-cycle-argumentum-webroot\`
(15/15 SHA256, couverture 644/644) — c'est le **retour arrière** de toute la procédure.

## 1. Protocole de simulation

Copie du webroot hors-ligne (`C:\Users\jsboi\.claude\scratch\etape3-simcopy\`, 9,98 Go, 20 897 fichiers,
robocopy /E avec exclusions : les 4 worktrees liés, `tmp/`, les 3 `bin/` du convertisseur ≈26 Go).
Fidélité prouvée avant tout test :

- **bin/ octet pour octet** : manifeste SHA-256 des 284 fichiers de `DNNPlatform/bin/`, T0 (original) ≡ T1 (copie).
- **statut git ≡** : 226 suivis modifiés/supprimés, 640 non suivis, diff porcelain limité aux 4 worktrees exclus.
- 4 liens symboliques NTFS (`Generation/CardPen/*`, mode git 120000) recréés par `mklink /D` (robocopy les matérialise en répertoires réels).

## 2. Test naïf — RÉFUTÉ par exécution

`git reset --hard origin/master` sur la copie (HEAD `ae84c91c` → `9a38db96`) :

| Mesure | Avant | Après | Conséquence |
|---|---|---|---|
| `DNNPlatform/bin/` | 284 fichiers | **89** | **195 DLL supprimées** (dé-trackées par #1244a/#1365, trackées à `ae84c91c`) |
| Ensemble D (suivis supprimés localement) | 201 | 0 | suppression locale écrasée |
| Non suivis | 640 | 575 | −65 fichiers bin/ (dé-trackés) |

⇒ Un reset naïf **casse la préprod au prochain recycle** : les assemblages servis vivent dans le
worktree et ne sont plus trackés au master. Ce geste est définitivement écarté pour le webroot réel
sans protection (sections 3-4).

## 3. Cible de projection

`d550d0de` = origin/master `9a38db96` + merge PR **#1869** (resx mentions légales, dépôt ← site) +
merge PR **#1870** (vues #674 réalignées sur les vues servies). À l'exécution réelle, la cible est
simplement `origin/master` une fois #1869/#1870 mergées — la projection n'a servi qu'à rendre la
simulation fidèle à ce que sera le master au moment du geste.

## 4. Test protégé — RÉUSSI

Séquence exécutée sur la copie (pré-état re-vérifié ≡ original avant lancement) :

1. **Backup de `DNNPlatform/bin/`** hors arbre (284 fichiers).
2. Snapshot des non suivis (`git ls-files --others --exclude-standard`, 640 fichiers).
3. `git reset --hard d550d0de`.
4. **Restauration de `bin/`** depuis le backup.
5. Contrôles :

| Contrôle | Attendu | Mesuré |
|---|---|---|
| Manifeste bin/ (SHA-256 × 284) | ≡ pré-reset | **T6 ≡ T1, octet pour octet** |
| HEAD / branche | master projeté | `d550d0de` / master |
| Suivis modifiés/supprimés | 0 | **0** |
| Non suivis intacts | 640 | **575 présents + 65 bin/ avalés par `.gitignore`** — 0 perdus, 0 créés |
| bin/ visible dans le statut | — | **invisible** : `.gitignore:163:/DNNPlatform/bin/` (règle #1244a) |

**⭐ Découverte qui simplifie l'exécution réelle** : le groupe « runtime / à ignorer par .gitignore »
du dispatch est **déjà couvert** — la règle `/DNNPlatform/bin/` existe à master depuis #1244a. Aucune
PR .gitignore n'est nécessaire ; le statut post-sync est **propre** (0 suivi modifié, bin/ ignoré).

**Effet de bord attendu** : le reset restaure `DNNPlatform/Portals/1/NBSTemp/wFvqddGx2k1k` (fichier
tracké à la cible dont la suppression locale n'a jamais été committée). Inoffensif (temporaire DNN) ;
à re-supprimer localement ou laisser.

## 5. Classification des 640 non suivis (3 groupes)

| Groupe | Nombre | Contenu | Destination |
|---|---:|---|---|
| **G1 — runtime ignoré** | 65 (+195 dé-trackés = bin/ entier, 284) | `DNNPlatform/bin/*` | rien à faire — couvert par `.gitignore:163` |
| **G1b — état du site** | 335 | DesktopModules 201 (dont Admin 126), admin 58, controls 22, App_GlobalResources 13, Resources 11, Portals 8, Providers 7, Install 2, + 13 artefacts à la racine DNNPlatform (fallacies_*.html, owl, svg, sonde ACME) | **arbitrage ai-01** : artefacts d'install DNN (packs es-ES/pt-BR du 12/10, modules) et copies servies — ignorer ou committer sélectivement |
| **G2 — contenu à committer** | **4** | 4 PNG de preuve à la racine du dépôt (`k2-arbre-ontologie.png`, `i1-navbar-{fr,en}-1440.png`, `ar-rtl-committed.png`) | déplacer sous `docs/` (dossier de preuve dédié) puis committer |
| **G3 — déchet** | 236 | backup-green-restore-20260901 169+, backup-sharpzip 10, backup-mindmaps 16, `.scratch-*` 28, `.bak`, `.received.png` | à supprimer **après** validation du déploiement (préservés dans l'archive scellée) |

### Scan de secrets (⛔ aucun secret au dépôt)

Motifs balayés sur les 640 : `password=`, `pwd=`, `connectionString`, `apiKey`, `machineKey`,
`BEGIN PRIVATE KEY`, empreinte du mot de passe DB. **8 touchés, aucun committable** :

- 5 resx es-ES/pt-BR (G1b) : **faux positifs** — clés de libellés UI (`OAuthConfigBase_APIKey.Text`),
  0 occurrence de connectionString/machineKey.
- 3 backups `web.config.*` (G3) : **vrais secrets** (machineKey, chaîne de connexion) — classés
  déchet, jamais committés, préservés uniquement dans l'archive scellée locale hors dépôt.
- **G2 (à committer) : 0 touché.**

## 6. Procédure d'exécution réelle (gélée jusqu'au verdict ai-01)

**Prérequis** : #1869 et #1870 mergées ; fenêtre annoncée sur le dashboard, hors heures de relecture
d'Adeline ; archive de préservation vérifiée (15/15).

| # | Geste | Contrôle bloquant |
|---|---|---|
| A | **PowerShell** (jamais Git-Bash) : `robocopy D:\Dev\Argumentum\DNNPlatform\bin <backup> /E` | **284 fichiers copiés, octets > 0 — sinon STOP** (un robocopy lancé depuis Git-Bash interprète `/E` comme un lecteur `E:` et échoue silencieusement dans la chaîne : mesuré 12/10) |
| B | `git ls-files --others --exclude-standard > snapshot` | 640 entrées (référence du contrôle E) |
| C | `git fetch origin && git reset --hard origin/master` | HEAD = origin/master |
| D | `robocopy <backup> DNNPlatform\bin /E` (PowerShell) | 195 fichiers re-copiés, 284 présents |
| E | Manifeste SHA-256 bin/ ≡ backup ; `git status` : **0 suivi modifié** ; non suivis = snapshot (les 65 bin/ deviennent invisibles : ignorés) | tout écart = déclencher le retour arrière |
| F | Contrôle rendu : 7 pages FR + `/en` (ai-01 — po-2023 signale, ne valide jamais) | verdict visuel ai-01 |
| G | Recycle app-domain si comportement douteux (`(Get-Item web.config).LastWriteTime = Get-Date`) ; 1ʳᵉ requête à froid ≈ 33 s ≠ panne | — |

**Retour arrière** (si E ou F échoue) : procédure de l'archive scellée — `git reset --hard ae84c91c`
puis `git apply --binary unstaged.patch`, extraire `untracked.tgz` pour les non suivis, re-déposer
`DNNPlatform/bin/` depuis le backup de l'étape A. Mode opératoire complet dans `NOTES.md` de l'archive.

## 7. Leçons d'instrument (simulation)

- **robocopy `/E` saute les fichiers dont la source est plus ancienne que la destination** — un
  refresh après une mutation git ne restaure pas `.git` (réécrit plus récent) ; forcer `/IS /IT`.
- **Référence lâche vs packed-refs** : `update-ref` écrit un ref lâche qui **prime** sur packed-refs —
  après un reset sur une copie dont le `.git` vient de packed-refs, le ref lâche résiduel ment.
- **`git status --porcelain` collapse les répertoires non suivis** (`?? docs/quality/` = 1 ligne) ;
  comparer des ensembles de fichiers non suivis uniquement via `ls-files --others` (niveau fichier,
  sauf collapse des worktrees liés).

---
*po-2023, session 12/10 soir. Données brutes (manifestes T0-T6, listes par groupe, hits secrets) :
scratchpad de session `…\scratchpad\etape3\` ; copie de simulation `…\.claude\scratch\etape3-simcopy\`
(9,98 Go — à purger après verdict ai-01).*

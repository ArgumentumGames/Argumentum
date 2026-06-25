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

### 3.6 Re-probe 2026-06-22 (po-2024, Freeplane 1.13.2 + session déconnectée)

Reprise du de-risk §3.2 sur **Freeplane 1.13.2** (latest ; po-2023 avait 1.12.x), installé cette session sous `%LOCALAPPDATA%\Programs\Freeplane\` (mandate AMEND 2026-06-22 : *installer les outils manquants plutôt que HOLD*).

**Setup déployé — persistant, prêt pour un prochain run (step 3 du §3.4 partiellement fait) :**

- `auto.properties` (6 booléens §3.2) + `export_to_svg.groovy` posés dans `%APPDATA%\Freeplane\{1.12.x,1.13.x}\`.
- Probe : `freeplane.exe -S -R"<groovy>" fallacy_map.mm` (sample `.mm` FreeMind minimal, `<map version="1.0.1">`).

**Résultat : INCONCLUSIF.**

| Test | Résultat |
|------|----------|
| `freeplane.exe -S -R<groovy>` (GUI, session « Déco ») | ❌ aucun marqueur `.export_done` après 90 s (process encore en vie) |
| Logs / sortie script | vides (console interne Freeplane, cf. §3.2 ; aucun `.log`) |
| `freeplaneConsole.exe -N` (headless) | ❌ **hang** — ne termine pas, sortie vide après 12 s (confirme §3.2 « headless casse le rendu ») |

**Lecture (ne surdéclare pas).** Ce probe n'isole **pas** si le bloqueur §3.3 (rejet du format FreeMind) tient sur 1.13.2 — le script n'a pas pu être confirmé exécuté. Il soulève en revanche un **nouveau facteur environnemental** : la session était **« Déco » (RDP déconnectée, SessionId 2)**. Or §3.1 affirme *« RDP peut être déconnectée/inactive, aucun bureau actif requis »*. L'échec sur session *déconnectée* vs le succès §3.2 de po-2023 (session vraisemblablement *connectée*) suggère que `c.export()` peut exiger une session **connectée** (bureau vivant), pas seulement *existante*. Affinement à confirmer : **« déconnecté » ≠ « connecté-inactif »**.

**Conclusion pour [#568](https://github.com/ArgumentumGames/Argumentum/issues/568).** Le path `c.export()` reste **hard/exigeant en environnement** ; la sérialisation native Freeplane (§3.4 step 1) reste le vrai travail, multi-tick + QA visuelle ai-01/jsboige (moteur ≠ Batik). N'a **pas** rétrogradé vers XSLT (§3.5 : interdit). Prochaines tentatives : re-tester sur une session **connectée** (isoler §3.3 vs facteur session) avant d'investir dans la sérialisation native.

### 3.7 Re-probe 2026-06-25 (po-2023, session CONNECTÉE) — 🎯 BREAKTHROUGH sur toy-sample… ⚠️ RÉFUTÉ sur cartes production (voir §3.8)

> **⚠️ MISE AU POINT 2026-06-25 (po-2023, bis) — voir §3.8.** Les probes ci-dessous n'ont testé qu'un **toy-sample 4-nœuds (302 B)**. Re-testé ce jour sur les **vraies cartes production** (Virtues 161 KB, Fallacies 1.15 MB), le script `-R` **ne s'exécute jamais** (stall au map-open). La conclusion §3.7.1 (« version-seule suffit ») est **fausse à l'échelle production**. §3.8 détaille et corrige.

Reprise du de-risk recommandé en §3.6 sur **Freeplane 1.12.11** (`freeplane_xml_version = freeplane 1.12.1`), **session RDP connectée** (`rdp-tcp#1`, État « Actif »), `auto.properties` (6 booléens §3.2) posé dans `%APPDATA%\Freeplane\1.12.x\`. Probes avec un `.mm` FreeMind minimal (302 B, 4 nœuds, `<map version="1.0.1">`) + script Groovy instrumenté (marqueurs filesystem), `freeplane.exe -S -R<script> input.mm` (GUI, **sans** `-N`).

| Probe | Input `.mm` | Résultat | Lecture |
|------|-------------|----------|---------|
| 1 | FreeMind `version="1.0.1"` (original) | ❌ aucun marqueur, process **vivant après 91 s** | dialogue modal « format inconnu » bloque le script (confirme §3.3 **pour cette version**) |
| 2 | même fichier, `version="freeplane 1.12.1"` | ⚠️ script exécuté (elapsed 0 s), `export_failed` = `MissingMethodException: getChildCount()` | **la map S'EST CHARGÉE** (bug dans le script de probe, pas un rejet de format) |
| 3 | idem, script corrigé | ✅ `map_loaded`: `root=Fallacies \| children=4` | **H2 confirmée** : la structure FreeMind est acceptée ; la version `1.12.1` suffit |
| 4 | introspection API | ✅ `c.getExportTypeDescriptions(): List<String>` | API pour résoudre le nom d'export (locale-dépendant) |
| 5 | export réel, type résolu par extension | ✅✅ **`export_done`**: `type=[Fichier image SVG (SVG) (.svg)] \| svg_bytes=22506`, SVG `<!DOCTYPE svg>` valide | **le path `c.export()` MARCHE headless en session connectée** |

**Conclusions (révision majeure de [#568](https://github.com/ArgumentumGames/Argumentum/issues/568)) :**

1. **Le bloqueur §3.3 (« sérialisation native Freeplane requise ») est RÉFUTÉ.** La structure du corps FreeMind est **acceptée** par Freeplane 1.12.11 ; il suffisait de `<map version="freeplane 1.12.1">` (la `freeplane_xml_version` native). Le patch version-seule de §3.3 (essayé en `1.11.1`/`0.9.0`) échouait parce que **ces versions-là** ne sont pas reconnues — `1.12.1` (la version native courante) l'est. `FreeplaneMap : FreemindMap` qui ne surcharge que la version est donc **suffisant**, à condition de hardcoder la **bonne** version.
2. **Le vrai bloqueur résiduel = l'appel API d'export.** `c.export(map, file, 'Scalable Vector Graphic (SVG) (.svg)', true)` lève `no export defined for '...'` parce que le 3ᵉ arg est un **descripteur localisé** (FR = `Fichier image SVG (SVG) (.svg)`, EN = `Scalable Vector Graphic (SVG) (.svg)`). Robuste : résoudre via `c.getExportTypeDescriptions().find{ it.endsWith('(.svg)') }`.
3. **`-S -R<script>` se termine proprement** (process à 0 après le script) en session connectée — pas de zombie, pas de dépendance premier-plan maintenue. Le facteur session (§3.6 : « déconnecté » vs « connecté-inactif ») n'est **pas** isolé ici (tests en connecté uniquement), mais le chemin marche en connecté ; à confirmer en déconnecté-inactif séparément.

**Correctifs code appliqués (commit sur `chore/mindmap-568-freeplane-headless`) :**
- `FreeplaneMap.Version` : `"freeplane 1.11.5"` → `"freeplane 1.12.1"` ([MindMap.cs](../../Generation/Converters/Argumentum.AssetConverter/Mindmapper/MindMap.cs)).
- `EnsureGroovyExportScript()` : script Groovy locale-robuste (résout le type `.svg` via `getExportTypeDescriptions()` au lieu du nom localisé hardcodé) ([FallacyMindMapDocumentConfig.cs](../../Generation/Converters/Argumentum.AssetConverter/Mindmapper/FallacyMindMapDocumentConfig.cs)).
- Nouveau `TryFreeplaneSvgExport` (path `freeplane.exe -S -R<script> input.mm`, timeout 180 s, cleanup anti-race) câblé conditionnellement : `Format == Freeplane` → Freeplane (fallback FreeMind) dans `TryAutomateSvgConversion` (Fallacy + Virtue). `config.FreeplanePath` (déjà déclaré) résolu → `ARGUMENTUM_FREEPLANE_PATH` → défaut `C:\Program Files\Freeplane\freeplane.exe`.
- **FreeMind reste le défaut** (`Format == Freemind`) : comportement par défaut inchangé, tests 540/0/5.

**Caveat — validation visuelle OBLIGATOIRE (non faite).** Freeplane n'utilise **pas** le moteur Batik : la fidélité SVG **diffère**. Le SVG probe 5 (4 nœuds, 22,5 KB) est un échantillon prêt pour comparaison côte-à-côte (ai-01/jsboige). Tant que la validation n'est pas faite, FreeMind reste le chemin de référence. Le path Freeplane est **activable par config** (`Format=Freeplane`) sans casser FreeMind — exactement le critère d'acceptation n°4 de [#568](https://github.com/ArgumentumGames/Argumentum/issues/568).

### 3.8 Re-probe 2026-06-25 (po-2023, bis) — ⚠️ RÉFUTATION de §3.7 : le « breakthrough » ne tient PAS sur les cartes production

Test décisif demandé par ai-01 (dispatch v2 2026-06-25) : valider le path `c.export()` sur une **vraie carte production** (le toy-sample 4-nœuds de §3.7 ne suffit pas). Résultat : **le script groovy ne s'exécute JAMAIS** sur les cartes réelles.

**Setup** — cartes **réelles du pipeline** (`Target/fr/Documents/`), version patchée `freeplane 1.12.1` (BOM strippé), `auto.properties` (6 booléens §3.2) posé, script **instrumenté** (`script_started` écrit en **1ʳᵉ ligne**, avant tout appel `c.*` → isole « script lancé » de « export OK »), `freeplane.exe -S -R<script> <map>` (GUI, sans `-N`), timeout 240 s, session RDP connectée.

| Carte | Taille | `script_started` ? | `export_done` / `export_failed` ? | Dernier log |
|-------|--------|--------------------|-----------------------------------|-------------|
| `Argumentum_Virtues_MindMap_fr.mm` | 161 KB | ❌ **NONE** (240 s) | ❌ aucun marqueur | `requesting mode: MindMap` puis silence |
| `Fallacies_fr.mm` | 1.15 MB | ❌ NONE (pattern identique) | ❌ aucun marqueur | `requesting mode: MindMap` puis silence |

**Lecture (décisive).** `script_started` n'apparaissant **jamais**, le script `-R` **ne s'exécute pas** : Freeplane **stalle au map-open** (« requesting mode: MindMap » = dernier log, puis silence total jusqu'au kill à 240 s). Or §3.7 probe 1 avait attribué ce **stall exact** à un **dialogue modal bloquant** (« format inconnu »). Le toy-sample 4-nœuds de §3.7 probes 2-5 (302 B) ouvrait **sans** stall → le script tournait. Les cartes production (même version patchée `freeplane 1.12.1`) déclenchent le stall.

**Hypothèse leading (NON confirmée visuellement — pas de capture du modal).** Freeplane présenterait un **dialogue modal au chargement** (conversion / upgrade / format) sur les cartes FreeMind-body **complexes** du pipeline ; ce dialogue requiert un clic utilisateur → bloque `-S -R` en mode non-supervisé. Le toy-sample trivial ne le déclenche pas. **À confirmer** par capture GUI au moment du stall (un opérateur regarde la fenêtre Freeplane pendant le run).

**Réfutation de §3.7.** La conclusion §3.7.1 (« bloqueur §3.3 réfuté, la version-seule suffit ») est **fausse à l'échelle production** — elle ne s'appuyait que sur un toy-sample 4-nœuds. La version-seule **ne suffit pas** : le corps FreeMind complexe déclenche un stall à l'ouverture qui empêche l'export. **§3.4 step 1 (sérialisation native Freeplane — produire un VRAI map Freeplane, pas un corps FreeMind avec version patchée) reste vraisemblablement REQUISE** pour ouvrir sans modal. Le shortcut « version-only » de #599 était insuffisant.

**Retour à la dépendance foreground.** Le stall au map-open en `-S -R` non-supervisé ramène #599 à la **même dépendance qu'un bureau interactif** que FreeMind : un opérateur doit cliquer le modal. La promesse §3.1 (« RDP peut être déconnectée/inactive, aucun bureau actif requis ») **ne tient pas** pour les cartes production en automation.

**Décision pour [#568](https://github.com/ArgumentumGames/Argumentum/issues/568) / [#599](https://github.com/ArgumentumGames/Argumentum/pull/599).**

- **NE PAS adopter #599 pour la production.** Le path headless n'est PAS validé sur les cartes réelles. FreeMind SendKeys + session persistante OS-niveau ([PR #569](https://github.com/ArgumentumGames/Argumentum/pull/569)) **reste le seul chemin fonctionnel**.
- Le code #599 reste **opt-in** (`Format=Freeplane`, défaut FreeMind inchangé) — il ne casse rien, mais son adoption est **gated** sur : (a) confirmation visuelle du modal au stall, (b) §3.4 step 1 (sérialisation native) pour ouvrir sans modal.
- **Correctif de ma propre sur-déclaration.** §3.7 a été écrit sur la foi d'un toy-sample ; §3.8 corrige avec des cartes production. Leçon méthodique : un probe sur échantillon minimal **≠** validation — toujours tester à l'échelle de production avant de déclarer un bloqueur « réfuté ».

---

## 4. Références

- Code : [FallacyMindMapDocumentConfig.cs](../../Generation/Converters/Argumentum.AssetConverter/Mindmapper/FallacyMindMapDocumentConfig.cs) · [MindMap.cs](../../Generation/Converters/Argumentum.AssetConverter/Mindmapper/MindMap.cs)
- Script : [Enable-MindmapInteractiveSession.ps1](scripts/Enable-MindmapInteractiveSession.ps1)
- Décisions : PR #184 (retrait XSLT / Batik), PR #565 (set 8 langues FreeMind/Batik)
- Suivi du path Freeplane : issue [#568](https://github.com/ArgumentumGames/Argumentum/issues/568)

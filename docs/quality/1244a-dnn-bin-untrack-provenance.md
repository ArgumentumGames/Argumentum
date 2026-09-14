# #1244(a) — Dé-tracker `DNNPlatform/bin/` : ce que la couche trackée est (et n'est pas)

**Auteur** : po-2023 (worker lane) · **Date** : 2026-09-14 · **Base** : `origin/master` `874a5d98`
**Nature** : mesure (lecture seule) **+** un commit de dé-track. Aucune régénération, aucune publication,
aucune suppression de fichier — en particulier **aucune écriture dans le webroot vivant**.
**Portée** : grain ② de la deep-queue po-2023 (`#458` c.`5657210653`). Décision **(a)** de l'owner actée.

---

## §0 — Décision et périmètre

Le commit retire de l'index les **195 fichiers** que `origin/master` suit sous `DNNPlatform/bin/` :
**179 `.dll`**, 9 `.config`, 3 `.targets`, 3 `.json`, 1 `.xml` — **45,7 Mo** (47 923 623 o).

C'est une opération d'**index**, pas de disque : `git rm --cached` ne supprime aucun fichier, ni dans le
webroot, ni dans le worktree du commit.

---

## §1 — Ce que la couche trackée n'est pas : le servi

Mesure des 195 blobs contre le **webroot vivant** (`D:\Dev\Argumentum\DNNPlatform\bin`, servi = disque établi
aux grains ①-②) :

| | fichiers |
|---|---:|
| présents sur le webroot | **195 / 195** |
| **identiques** au blob tracké | **117** |
| **différents** — le webroot porte un build plus récent | **78** |

⇒ **La couche trackée est un instantané antérieur, pas la plateforme servie.** 78 des 195 fichiers que ce
commit dé-tracke ne sont pas ce que la préprod exécute aujourd'hui : le disque porte des assemblages 10.3.2
plus récents. Dé-tracker l'index **ne retire donc rien de ce qui est servi** — cela retire une photographie
figée que git traînait.

**Ce n'est pas seulement « plus ancien » : c'est une autre majeure.** `DotNetNuke.dll` du jeu **tracké** est en
**9.11.1.0**, quand le webroot vivant porte **10.3.2.0**. Sur la famille d'assemblages `DotNetNuke.*` / `Dnn.*`
(les seuls dont la majeure est celle de la plateforme) :

| | majeure 9 | majeure 10 |
|---|---:|---:|
| jeu **tracké** (195) | **39** | 0 |
| **webroot vivant** | 1 | **46** |

Le `bin/` que ce commit dé-tracke est donc le **baseline 9.11.1** — exactement l'écart que #972/#1049 décrivait
pour `web.config` (« déployé en 10.3.2, tracké au baseline 9.11.1 »), ici appliqué aux assemblages.

---

## §2 — Preuve de préservation, quatre niveaux

| niveau | support | couverture des 195 |
|---|---|---|
| **L1** | **historique git** — tout commit antérieur au dé-track | **195 / 195** |
| **L2** | **archive hors dépôt créée le 14/09** (voir ci-dessous) | **195 / 195**, à l'octet |
| **L3** | sandbox `DNN-Argumentum-sandbox-1032-2026-06-28/bin_post_2sxc_realign` | 109 / 195 (sha256) |
| **L4** | webroot vivant sur disque | 195 / 195 présents — 117 à l'octet, 78 en build plus récent |

**L1 — restauration par git** : `git checkout <commit antérieur à ce merge> -- DNNPlatform/bin/` restitue les
195 fichiers, à l'octet, tant que l'historique est conservé.

**L2 — archive complète, créée avant le commit** :
`G:\Mon Drive\Synchronisation\RooSync\Argumentum-offload\DNN-10.3.2-tracked-bin-pre-1244a-2026-09-14\`
— les 195 fichiers extraits **en lecture seule** (`git cat-file`) depuis `origin/master`, arborescence
`DNNPlatform/bin/…` préservée, plus `sha256-manifest.txt` (195 entrées) et `MANIFEST.md`.
**Contrôle round-trip** : chaque fichier écrit a été relu et re-haché — **195 / 195 conformes**.
*Raison d'être* : sans cette copie, les **78 blobs dont le webroot porte un build plus récent** (L4
« différents ») n'existaient plus **que** dans l'historique git. Le mot du mandat est « complète » : il l'est
désormais sans dépendre de la rétention git.

**L3 — sandbox 28/06** : recouvrement partiel (109/195 par sha256) — utile, mais l'archive L2 le subsume.

⚠️ **Ne pas invoquer l'offload du stash comme preuve du jeu tracké.** `DNN-10.3.2-stash-5a086dfe-2026-09-13`
porte le jeu **untracked** du webroot : population **disjointe par construction** — recouvrement mesuré
**0 / 195**. Deux jeux voisins, deux rôles distincts ; les confondre ferait lire « préservé » sur un fichier
qui n'y est pas.

### §2.1 — La comparaison demandée : 128 extractions vs 330 fichiers runtime

| jeu | fichiers | dll |
|---|---:|---:|
| offload du stash `5a086dfe` | **128** | **60** |
| sandbox `DNN-Argumentum-sandbox-1032-2026-06-28/bin_post_2sxc_realign` | **330** | **255** |
| webroot vivant `bin/` | 281 | 256 |

Recouvrements **à l'octet** :

| paire | communs |
|---|---:|
| stash (128) ∩ sandbox (330) | **112 / 128** |
| stash (128) ∩ webroot (281) | 62 / 128 |
| sandbox (330) ∩ webroot (281) | **270 / 330** |

⇒ Les 128 extractions sont **presque** un sous-ensemble du sandbox — **16 n'y sont pas**. Et surtout :
**`DotNetNuke.dll` est ABSENT de l'offload du stash**, présent au sandbox **et** au webroot.

**Conclusion, sans supposer la complétude** : un jeu de **128 fichiers / 60 DLL ne couvre pas** le besoin
runtime de 330 fichiers — il lui manque au minimum l'assemblage **noyau**. Le sandbox (330 / 255 DLL) est la
source runtime ; l'offload du stash (128) est un **complément**, pas un socle.

**Restauration** :
- **jeu tracké (les 195)** ← archive L2 : recopier `…\DNN-10.3.2-tracked-bin-pre-1244a-2026-09-14\DNNPlatform\bin\`
  vers `<repo>\DNNPlatform\bin\`, puis vérifier contre `sha256-manifest.txt` (195 entrées).
  Version du noyau dans cette archive : `DotNetNuke.dll` = **9.11.1.0** (c'est le tracké).
- **runtime 10.3.2.0** ← sandbox C (330) ou webroot vivant ; `DotNetNuke.dll` y est en **10.3.2.0**.

---

## §3 — L'opération

1. `git worktree add --no-checkout .untrack-bin-1244 -b chore/1244a-untrack-dnn-bin origin/master`
   — worktree **sparse** : les 195 binaires et le reste du webroot ne sont jamais matérialisés.
2. `git rm -r --cached --sparse DNNPlatform/bin/` → **195 entrées retirées de l'index** (`--sparse` requis :
   la garde de sparse-checkout refuse par défaut de toucher hors du cône).
3. `.gitignore` : les **ré-inclusions `!`** de #972 (`!/DNNPlatform/bin/`, `!*.dll`, `!*.pdb`, `!*.exe`) sont
   **inversées** en `/DNNPlatform/bin/`. Sans cette inversion, la règle globale `*.dll` de L19-21 aurait
   re-ignoré les DLL mais laissé les 9 `.config` / 3 `.targets` / 3 `.json` / 1 `.xml` re-trackables — le
   dé-track aurait été partiel et silencieux.

**Aucun fichier n'est supprimé** : ni sur le webroot vivant (autre checkout, jamais touché), ni dans l'archive.

---

## §4 — Effet de bord identifié et traité : le contrat de binding redirect

`DnnBindingRedirectContractTests.PinnedAssembly_RedirectsAgreeWithShippedDll` exige la **présence sur disque**
de `DNNPlatform/bin/ICSharpCode.SharpZipLib.dll` (`File.Exists(...).Should().BeTrue()`). Sur un checkout frais
(CI), le dé-track la fait disparaître ⇒ **rouge**.

Traitement dans le même commit : un attribut `RequiresWebrootBinTheory` (dérivé de `TheoryAttribute`) calcule
son `Skip` **à la découverte**, sur la **présence du répertoire** `DNNPlatform/bin/`. Le test saute alors en
nommant #1244(a) et le chemin de l'archive L2, et **reste armé sans changement** partout où le webroot est
matérialisé. Conséquence assumée et écrite : **la surface d'application du contrat se réduit aux machines
portant le webroot** — un skip motivé, pas un vert par vacuité.

**Voie rejetée, mesurée** : `SkipException.ForSkip(...)` (l'API de skip dynamique de xUnit **v2**) lève bien
l'exception, mais `xunit.runner.visualstudio` **4.0.0** — qui n'honore le skip dynamique que pour xUnit v3 —
la reporte en **FAIL** (`$XunitDynamicSkip$…` dans le message, comptée en échec). Le skip à la découverte est
le mécanisme supporté pour v2 ; vérifié en exécution réelle : **échec 0 · réussite 1 · ignorée 1**.

Le **contre-test** (`PinnedAssembly_IsActuallyRedirectedSomewhere`) n'est pas affecté : le redirect SharpZipLib
vit dans 6 configs trackés, dont **un seul** sous `bin/` (`DotNetNuke.Web.dll.config`) ; les 5 autres
(`DesktopModules/Connectors/{Azure,GoogleAnalytics,GoogleTagManager}/app.config`,
`DesktopModules/SiteExportImport/app.config`, `Install/Web.config`) restent suivis.

**Observation mesurée, hors périmètre de ce commit** — le scan du contrat est
`Directory.EnumerateFiles(DNNPlatform, "*.config", AllDirectories)` : il inclut donc les répertoires
**non trackés** présents sur une machine donnée. Sur po-2023, ce contrat est **rouge aujourd'hui — avant
comme après ce commit** — à cause d'un vestige **local** : `DNNPlatform/bin.contaminated.bak/
DotNetNuke.Web.dll.config` porte `newVersion=1.3.3.11` alors que la DLL livrée est en **1.4.2.13**. Les
**6 configs trackés** sont tous d'accord avec la DLL (1.4.2.13, mesuré). ⇒ Le verdict du contrat dépend d'un
répertoire non tracké : **vert en CI, rouge ici**, sans qu'aucune divergence suivie existe. Correctif possible
(une ligne — restreindre le scan ou exclure `*.bak*`), **non appliqué ici** : décision owner/ai-01.

---

## §5 — Contrôles

| contrôle | énoncé | résultat |
|---|---|---|
| **Cap 0** | le périmètre est bien celui annoncé | `git ls-tree -r origin/master -- DNNPlatform/bin/ \| wc -l` = **195**, dont 179 dll ✅ |
| **positif** | au moins un fichier du jeu doit se retrouver intact hors dépôt | 117 identiques au webroot + 109 au sandbox + **195/195** dans l'archive L2 ✅ |
| **négatif** | la comparaison doit discriminer | 78 blobs trackés **diffèrent** du webroot ⇒ l'instrument ne dit pas « tout identique » ✅ |
| **round-trip archive** | ce qui est écrit doit être relu identique | sha256 après écriture : **195/195 conformes** ✅ |
| **disjonction** | l'offload du stash ne doit pas être compté comme couverture | recouvrement mesuré **0/195** — population untracked ✅ |
| **références** | aucun code ne doit dépendre du suivi git de ces fichiers | 2 références seulement : 1 test (traité §4) et `docs/dnn/repair-bin-net48.ps1` (script de réparation sur disque, sans lien au suivi) ✅ |

---

## §6 — Ce que je n'établis PAS

- **Que le webroot soit sauvegardé.** L4 constate la **présence** des 195 chemins sur le disque vivant ; ce
  n'est pas une sauvegarde, et le disque reste un support unique pour le jeu *untracked*.
- **La provenance des 78 builds divergents.** Je constate que le webroot porte plus récent ; je ne date ni
  n'attribue ces builds.
- **Le sort des 9 `.config` / 3 `.targets` / 3 `.json` / 1 `.xml`** au-delà du dé-track : aucun n'est référencé
  par du code suivi ; je ne juge pas de leur utilité runtime.
- **Aucun verdict sur la publication** ni sur #1180 : hors périmètre de ce grain.

---

*master `874a5d98` · lecture seule : `ls-tree`, `cat-file`, sha256, `git grep` · archive hors dépôt :
`DNN-10.3.2-tracked-bin-pre-1244a-2026-09-14` (195/195) · ⛔ aucune régénération, aucune publication,
aucune écriture dans le webroot · verdict visuel : ai-01.*

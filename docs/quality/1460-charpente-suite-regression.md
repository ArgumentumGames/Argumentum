# #1460 — Charpente de la suite de régression structurelle (étages 1/3/4)

**Auteur** : po-2024 (worker) · **Date** : 2026-09-21 · **Base** : master `6694d702`
**Grain** : pool #458 **v8 ⑦** — *« squelette de la suite de régression structurelle (étages 1/3/4), sans artefacts. »*
**Instrument** : [`tools/1460-structure-charpente-pilot.py`](../../tools/1460-structure-charpente-pilot.py) — lecture seule, **ne réclame aucun artefact**.

> **État** (mise à jour 11/11 par po-2023, master `3e17a576`) : **A livrée** (05/11) · **B organes 1-3 livrés** (06→08/11, #2206/#2207) · **C organes 1-3 + garde de bloc livrés** (06→08/11) · **D gated** « PDF validés » (#2132) · **test du pilote câblé en CI** (job `tree-guard-contract`, depuis le 22/09 — §8). Les §7/§8 ci-dessous sont remis à jour sur ces faits.

> ⛔ **Ce document n'implémente aucune des trois phases.** Il écrit leur **charpente** : les contrats, les points de mesure, et ce qui est décidable **sans verdict humain**. Les organes eux-mêmes sont **POST-TAG** (`#1460`, corps : *« rien à merger avant le tag v2.0.0 »*).

---

## 0. Une tension à déclarer, pas à trancher en silence

Le **corps de #1460** porte deux fois, en gras : *« Scope POST-TAG : rien à merger avant le tag v2.0.0 »*. Le **dispatch** du pool v8 (grain ⑦) dit l'inverse sur un point précis :

> *« La suite est POST-TAG, mais sa **charpente** (contrats, points de mesure, ce qui est décidable sans verdict humain) s'écrit **maintenant**. ⛔ Ne réclame pas le bundle : 3,7 Go, et il vit sur `G:` côté ai-01/po-2023. »*

**Lecture retenue** : ce qui est post-tag, c'est **la suite** (les organes qui assertent) ; ce qui s'écrit maintenant, c'est **la charpente**. Les deux textes ne se contredisent donc pas — mais l'accord tient à cette distinction, et si elle est fausse, **c'est ici qu'il faut me reprendre**, pas après coup.

⚠️ **Conséquence opérationnelle** : cette PR ne doit **rien** changer au comportement du dépôt. Le seul code livré est un instrument **lecture seule** ; les organes xUnit restent à écrire, phase par phase, après le tag (une PR par phase, cf. §5).

## 1. Le problème que la charpente doit résoudre d'abord

Les trois phases assertent **sur les PDF produits**. Or un arbre `Target/` peut se trouver dans **quatre** états, et trois d'entre eux rendraient un organe naïf **vert sur un objet qui ne peut pas porter l'état courant** :

| État de l'arbre | Ce qu'un organe naïf ferait | Ce qu'il doit faire |
|---|---|---|
| Absent | — (crash ou skip) | **NON MESURABLE**, déclaré |
| Présent, **0 PDF** | boucle vide ⇒ **0 échec ⇒ vert** | **NON MESURABLE**, déclaré |
| Présent, peuplé, **antérieur au corpus** | asserte sur des PDF d'avant l'édition ⇒ **vert faux** | **NON MESURABLE**, déclaré |
| Présent, peuplé, **postérieur au corpus** | asserte | asserte |

C'est la famille `regen sans clobber = stale trap`, à laquelle s'ajoute la règle du dépôt : **Debug et Release sont des arbres indépendants** — on mesure sur celui dont la date correspond, ⛔ jamais « le premier trouvé ».

⇒ **La garde d'arbre n'est pas un organe de plus : c'est le préalable commun aux trois phases.** Elle est donc écrite en premier (§3), et c'est la seule chose de cette charpente qui **s'exécute aujourd'hui**.

## 2. Les trois phases — contrats

| | **Phase A** (ex-étage 1) | **Phase B** (ex-étage 3) | **Phase C** (ex-étage 4) |
|---|---|---|---|
| **Mesure** | dimensions de page par format + *page counts* par langue | présence de contenu : FR dans du non-FR · placeholders littéraux · proportion Cyrillic/CJK/Arabic | Rules-first dans les TarotCards · couleurs de famille |
| **Artefact lu** | PDF (parsing déterministe, **sans rendu**) | **gabarit `mustache` × ligne du CSV du DataSet**, substitué hors navigateur (§2.1) — ⛔ ni le PDF (0 couche texte) ni le `.harvest.json` (pixels seuls) | empreinte des **pixels décodés** des pages PDF (ordre) · clés `css` du gabarit (couleurs) — ⛔ pas de texte, il n'y en a pas |
| **Source de l'attendu** | **les organes de contrat existants** (§4) — ⛔ pas de re-dérivation ici | sondes par langue (liste nommée) | ordre `CardSet` + réécriture de classe de langue |
| **Décidable sans verdict humain ?** | **oui** — un nombre de pages et une dimension sont des faits | **oui** — la présence d'un marqueur est un fait | **oui** pour l'ordre et la classe ; ⛔ **non** pour la justesse de la couleur |
| **Mutation falsifiante** | permuter 2 pages ; forcer A4 au lieu de Tarot | injecter `{{` dans une **cellule du CSV** (amont), pas « dans une cellule rendue » | inverser l'ordre `CardSet` ; retirer une classe de famille |
| **Format** | `841×1189 mm` (A0) · `210×297 mm` (A4) · Tarot ≈ `70×120 mm` · Poker | — | — |

⚠️ **Tolérance pointeur** sur les dimensions (spec #1460) : les trois formats ont des valeurs nominales en mm, que le PDF exprime en points ; l'organe asserte **avec tolérance**, jamais à l'octet — sinon il mesure l'arrondi de la bibliothèque, pas le format.

⚠️ **Deux phases, deux natures de vérité.** A et C sont **structurelles** (l'objet est faux ou il ne l'est pas). B est **statistique** (une proportion) : elle doit donc porter un **seuil nommé et justifié**, sinon elle rendra « 3 % de cyrillique » vert sur une carte vide.

### 2.1 La source de la phase B, mesurée le 06/11 (po-2023)

**La phase B telle qu'écrite lisait le PDF. Ce PDF ne porte aucun texte** — la mesure est d'ai-01, sur deux PDF publiés de `v2.1.0-review` : `0` caractère avec `pypdf` **et** `pdftotext`, `0` police dans les ressources, des images seulement ; contrôle positif du même instrument : **17 616 caractères** lus dans un PDF qui porte du texte. Une phase B « par extraction texte » était donc **verte sur n'importe quel défaut**, et l'organe « Rules d'abord » de la phase C ne pouvait pas se lire dans le texte non plus.

Où vit le texte des cartes, **avant** leur passage en image — mesuré sur `fr`, arbre Release du 26/10 (`.prep-1114-worktree`), avec deux instruments et un contrôle positif :

| Artefact | Porte le texte des cartes ? | Mesure |
|---|---|---|
| PDF publié | **non** | 0 caractère, 0 police (ai-01, 06/11 ; contrôle positif 17 616 c) |
| `.harvest.json` | **non** — pixels seuls | 176 `data:image/png;base64`, **0 champ non-`dataUrl`** ; sonde corps `desc_fr` : **0** occurrence. Les titres y survivent **comme clés d'image** (`"Argumentum_Fallacies_1.1.2..Justification triviale": "data:image/png…"`) — un titre trouvé là ne prouve pas que le contenu y est |
| **gabarit `mustache` × ligne du CSV du DataSet** | **oui** | sonde corps `desc_fr` : **1** dans le CSV, **1** dans le rendu substitué, **0** dans le `.harvest.json` ; substitution du gabarit + `#if` : **0 placeholder résiduel** |
| clé `csv` **embarquée** dans le gabarit JSON | ⚠️ **périmé** | `_csv_note` du gabarit : *« STALE — overridden at runtime by DataSet=… Embedded `csv` is a snapshot from template creation »*. Un organe qui lirait cette clé mesurerait un instantané de création, pas le corpus |

⛔ **Le piège à ne pas refaire** : le `.harvest.json` porte 176 images par langue et des clés qui *ressemblent* à des titres. Une sonde de « présence de contenu » lancée dessus rendrait des hits sur les titres et **zéro sur le corps** — un instrument qui confond l'étiquette et le contenu. La même famille que le registre EAV vide-contre-vide (`458-inject42-resultat-2026-11-06.md` §4).

**Ce que le rendu hors navigateur doit couvrir** — census des 46 gabarits (`Cards/**/*.json`, 06/11) : `{{#if}}` **69** (équilibrés avec 69 `{{/if}}`), `{{var}}` **317**, `{{breaklines}}` **21**, `{{ifCond}}` **13**, `{{#each}}` **9**, `{{markdown}}` **7**. 15 gabarits utilisent un helper, dont la majorité dans `Cards/Memo/Archive/2022/` — les gabarits **vivants** concernés sont `Memo_Face_fr`, `Memo_Back_fr` et `Rules_fr`.

**Ce que cette source n'établit pas** : elle lit la **substitution** des variables, pas la mise en page ; elle ne voit ni le débordement d'une cellule, ni une police manquante, ni un texte présent dans le CSV mais **masqué** par le CSS. Ces défauts restent du ressort de la validation visuelle. Elle n'établit pas non plus la **justesse** d'une traduction : elle constate la présence ou l'absence d'un marqueur, jamais sa qualité.

## 3. La garde d'arbre — organe 0, transverse, **exécutable aujourd'hui**

`tools/1460-structure-charpente-pilot.py` — lecture seule, aucun artefact requis.

**Verdict gradué par conséquence** (⛔ pas un booléen) :

| Sortie | Sens |
|---:|---|
| **0** | **MESURABLE** — au moins un arbre frais porte des PDF ; les phases peuvent asserter |
| **2** | **NON MESURABLE** — arbre absent, ou sans PDF, ou **antérieur au corpus** |
| **1** | **DIVERGENT** — arbre frais, mais l'inventaire contredit l'attendu |

⚠️ **L'état 1 reste INATTEIGNABLE *dans le pilote*, et celui-ci le dit lui-même.** Aucune source d'attendu n'y est branchée — **volontairement** : dériver un compte ici **dupliquerait** les organes de contrat (§4) et fabriquerait un second référentiel qui dériverait du premier. ⛔ Ne pas lire « verdict à trois états » comme « trois états implémentés **dans ce pilote** ».

✅ **Mise à jour du 05/11 (phase A, po-2023)** — l'état 1 est désormais **atteint**, là où la source existe déjà : `PdfPageGeometryIntegrity.JudgeTree` (`Tests/PdfAssembly/`), qui compare l'inventaire aux **documents activés de la config × 8 langues** (80 PDF attendus), et dont le cas 8 de `PdfStructurePhaseAContractTests` épingle le rouge (64/80 → `Divergent`, 80/80 → `Measurable`). Le pilote reste volontairement sans source d'attendu — les deux organes ne se contredisent pas, ils opèrent à deux niveaux. Détail : [`1460-phase-a-geometrie-2026-11-05.md`](1460-phase-a-geometrie-2026-11-05.md).

**Contrôle inverse (mutation falsifiante)** — committé et rejouable : `tools/test_1460-structure-charpente-pilot.py` (`python tools/test_1460-structure-charpente-pilot.py`, **7 cas, stdlib seulement, aucun artefact requis**). La garde y est éprouvée sur des arbres **synthétiques** datés par `os.utime`, parce qu'un organe qui ne sait dire que « NON MESURABLE » n'est pas une garde, c'est un constat :

| Cas | Attendu | Obtenu |
|---|---:|---|
| arbre **antérieur** au corpus | 2 | **2** — `ANTERIEUR au corpus de 5.0 j` |
| arbre **postérieur** au corpus | 0 | **0** — `frais : 1 PDF` |
| arbre frais **sans PDF** | 2 | **2** — `MAIS 0 PDF — rien a mesurer` |
| arbre **absent** | 2 | **2** — `arbre ABSENT` |
| dépôt **sans `Cards/`** | ≠ 0 | **1** — refus net `REPO INVALIDE` |
| l'état 1 est **déclaré** inatteignable | — | **le mot est imprimé** |

⚠️ **Le 7ᵉ cas est un *grounding* sur le dépôt réel** : la garde doit tourner et rendre un VERDICT sans planter. ⛔ Il n'asserte **pas** le code (0 ou 2) — figer `0` rendrait ce test rouge sur la machine qui n'a **pas** d'arbre, c'est-à-dire exactement sur le cas nominal de la garde.

⚠️ **La première passe de ce contrôle a rendu un faux « KO »** : l'aiguille du harnais était en minuscules face à `ANTERIEUR` — l'organe était correct, **l'instrument de contrôle était faux**. Même famille que tout ce document dénonce ; consigné parce qu'un contrôle inverse non éprouvé ne prouve rien.

## 4. Frontière — ⛔ ne pas dupliquer les organes existants

Le corps de #1460 le pose : *« Ne pas dupliquer les contract tests existants : ceux-ci gardent les comptes **au CSV** ; la phase A asserte **sur les PDF produits** que ces comptes s'y matérialisent. »*

| Organe existant | Couche gardée | Ce que #1460 ajoute |
|---|---|---|
| `PdfDeckCountContractTests` (#1187/#1288) | comptes **au CSV** | que ces comptes **se matérialisent dans les PDF** |
| `CardSetExpectedCardCountContractTests` (#1212) | attendus **au CSV** | idem |
| `MindmapDeadLinkGateTests` · `MindmapWrapperFreshnessGateTests` (#1446/#1449) | fraîcheur **des mindmaps** | le **même motif**, appliqué aux **arbres d'artefacts PDF** |

Les trois organes cités ont été **vérifiés présents** dans l'arbre avant d'écrire cette ligne (recherche `*GateTests`/`*ContractTests` sur le dépôt) — ⛔ une charpente qui cite des organes absents se lit comme un plan, pas comme un contrat.

## 5. Pattern d'armement (commun aux trois phases)

- **Env-gated** (`ARGUMENTUM_*_GATE=1`), **inerte en CI** — il n'y a pas d'arbre `Target/` en CI. Même famille que `MindmapDeadLinkGateTests` / `MindmapWrapperFreshnessGateTests`.
- **Armé par la lane qui régénère**, pas par un calendrier.
- **Fail-loud quand le gate est armé sans arbre frais** : un gate armé **affirme** « j'ai un arbre frais » ; l'absence d'arbre est donc un **échec**, ⛔ pas un skip. Sinon le gate reproduit exactement le défaut qu'il ferme (cf. #909 — une étape de test qui ne lançait rien et restait verte).
- **Une PR par phase**, instrument + contrôle inverse + mutation falsifiante par organe.

## 6. État mesuré sur cette machine (21/09, po-2024)

| | Valeur |
|---|---|
| Corpus (`Cards/**/*.csv`, hors `Archive/`) | **6 CSV** · 341 fichiers · plus récent **2026-09-21 17:15** |
| Arbre **Debug** | présent · **162 fichiers · 0 PDF** · plus récent **2026-08-28 01:03** |
| Arbre **Release** | **absent** |
| Extensions de l'arbre Debug | `.png ×136` · `.json ×16` · `.csv ×8` · `.owl ×2` |
| Langues | 8/8 présentes, **8/8 sans PDF** |

⇒ **Verdict de la garde ici : NON MESURABLE (sortie 2)** — et c'est le cas **nominal** de l'organe : il tourne sur une machine **sans bundle**, exactement comme il tournera en CI. Le bundle de revue (3,7 Go, QA visuelle PASS sur `c00cf0e9`) vit sur `G:` côté ai-01/po-2023 : ⛔ **il n'est pas réclamé**, et aucune ligne de ce document ne l'a lu.

⚠️ **L'arbre Debug local est antérieur au corpus** (28/08 vs 18→21/09) : sur cet arbre, un organe de phase A/B/C aurait asserté sur des PDF d'avant les écritures de septembre. **Il n'en a produit aucun** — mais c'est précisément l'état qu'un organe naïf aurait traité comme « rien à signaler ».

## 7. Ce que cette charpente n'établit **pas**

⚠️ **Ce document décrit des contrats, il ne les exécute pas** — les organes qui les exécutent sont listés ci-dessous *(un par mesure contractée ; le verdict « phase complète » appartient aux documents de phase, pas à la charpente)* : la **phase A** est livrée depuis le 05/11 : [`1460-phase-a-geometrie-2026-11-05.md`](1460-phase-a-geometrie-2026-11-05.md) ; la **phase B** a **trois organes** — placeholders (06/11) : [`1460-phase-b-placeholders-2026-11-06.md`](1460-phase-b-placeholders-2026-11-06.md) ; aucun mot-outil français en colonne non française (08/11, #2206) : [`1460-phase-b-fr-contamination-2026-11-08.md`](1460-phase-b-fr-contamination-2026-11-08.md) ; proportions d'écritures ru/zh/ar/fa à seuils nommés (08/11, #2207) : [`1460-phase-b-script-proportions-2026-11-08.md`](1460-phase-b-script-proportions-2026-11-08.md) ; la **phase C** a **trois organes + une garde de bloc** — l'ordre « Rules d'abord » mesuré sur les **pixels** faute de couche texte (06/11) : [`1460-phase-c-rules-first-2026-11-06.md`](1460-phase-c-rules-first-2026-11-06.md) ; la **couleur de famille** de chaque carte, CSV × gabarit × pixel (07/11) : [`1460-phase-c-famille-2026-11-07.md`](1460-phase-c-famille-2026-11-07.md) ; la couleur de famille des **vertus** (07/11) : [`1460-phase-c-virtues-2026-11-07.md`](1460-phase-c-virtues-2026-11-07.md) ; la garde de séparation par bloc règle (08/11) : [`1460-phase-c-garde-bloc-2026-11-08.md`](1460-phase-c-garde-bloc-2026-11-08.md) · ⛔ **l'état 1 (DIVERGENT) est inatteignable dans le pilote** (§3 — atteint par la phase A) · ⛔ **la garde ne dit rien de la justesse** : un arbre frais peut être faux — fraîcheur ≠ conformité · ⛔ elle ne voit **pas** la surface de rendu, et un corpus édité **après** la régénération rend l'arbre périmé sans qu'elle le sache (elle compare des **dates**, pas des **contenus**) · ⛔ les **formats** cités (Tarot ≈ `70×120 mm`, Poker) étaient des **ordres de grandeur** recopiés du corps de #1460, ⛔ **non re-mesurés** sur des PDF produits — je n'en avais aucun sous la main. **Re-mesurés le 05/11 par la phase A** : Poker 63,5 × 88,9 mm ✔, A4/A0 conformes aux `PageSizes` de QuestPDF ✔, mais **Tarot produit 59,9 × 112,9 mm, pas 70 × 120** — écart **soldé le 05/11 au soir** (§3.1 erratum du document de phase A) : l'arbre mesuré était antérieur au changement de nominal du 02/09, la chaîne `ImageHelper.ResizeInMM` rend la config souveraine sur la taille de page, et l'organe asserte désormais le nominal de config (± 1 mm) · ⛔ la borne de balayage de l'organe est **nommée** (`Target/` des deux configurations, hors `Archive/`), donc **un scan borné ne vaut pas énumération** · ⛔ les tolérances et seuils de §2 sont des **points de mesure nommés**, pas des valeurs calibrées — le calibrage exige un arbre frais, donc le tag.

## 8. Ce qui est livré

- **La charpente** : ce document — contrat par phase, frontière vs l'existant, pattern d'armement.
- **L'organe 0**, exécutable et falsifié : [`tools/1460-structure-charpente-pilot.py`](../../tools/1460-structure-charpente-pilot.py) — lecture seule, sortie graduée 0/2/1.
- **Son contrôle inverse, committé** : [`tools/test_1460-structure-charpente-pilot.py`](../../tools/test_1460-structure-charpente-pilot.py) — 7 cas, stdlib, arbres synthétiques + 1 *grounding* sur le dépôt réel. ⚠️ Livré dans un **second temps** : la mutation du premier jet vivait dans un scratchpad, donc un relecteur devait croire une transcription — **une mutation non committée n'est pas un contrôle, c'est une affirmation**.
- **Les organes livrés** (chacun avec ses témoins de contrôle inverse exécutés en CI **sans artefact**) : phase A, géométrie de page — [`1460-phase-a-geometrie-2026-11-05.md`](1460-phase-a-geometrie-2026-11-05.md) ; phase B organe 1, aucun placeholder littéral — [`1460-phase-b-placeholders-2026-11-06.md`](1460-phase-b-placeholders-2026-11-06.md) ; phase B organe 2, aucun mot-outil français en colonne non française (`FrenchContaminationGuardTests`, #2206 — la PR inclut le fix de corpus que l'organe a mordu) — [`1460-phase-b-fr-contamination-2026-11-08.md`](1460-phase-b-fr-contamination-2026-11-08.md) ; phase B organe 3, proportions d'écritures à seuils nommés (`ScriptProportionGuardTests`, #2207) — [`1460-phase-b-script-proportions-2026-11-08.md`](1460-phase-b-script-proportions-2026-11-08.md) ; phase C organe 1, ordre « Rules d'abord » — [`1460-phase-c-rules-first-2026-11-06.md`](1460-phase-c-rules-first-2026-11-06.md) ; phase C organe 2, couleur de famille — [`1460-phase-c-famille-2026-11-07.md`](1460-phase-c-famille-2026-11-07.md) ; phase C organe 3, couleur de famille des vertus — [`1460-phase-c-virtues-2026-11-07.md`](1460-phase-c-virtues-2026-11-07.md) ; garde de séparation par bloc règle (suite de l'organe 3, faille relevée en revue) — [`1460-phase-c-garde-bloc-2026-11-08.md`](1460-phase-c-garde-bloc-2026-11-08.md). ⚠️ Un organe livré n'est pas une phase livrée : les deux mesures nommées au contrat de la phase C (ordre, couleurs de famille) sont couvertes sur les deux paquets, mais aucun de ces organes ne dit quelque chose de la **justesse** d'une carte.
- **Câblage CI — corrigé le 11/11 (po-2023)** : « ⛔ Rien n'est câblé en CI » était **périmé dès le lendemain de l'écriture** — le job **`tree-guard-contract`** ([`python-tools-tests.yml`](../../.github/workflows/python-tools-tests.yml)) exécute `tools/test_1460-structure-charpente-pilot.py` depuis `2a24a4be` (#1487, **22/09**, le jour après cette charpente), d'abord sur cron hebdo + manuel, puis sur **PR + push master** en plus depuis #2235 (11/11). Ce qui **reste** vrai : c'est le **test du pilote** (contrôle inverse, std-lib, sans artefact) qui est câblé — la **suite** elle-même reste inerte en CI (pas d'arbre `Target/` en CI), par design (§5). `#1460` reste **POST-TAG** ; ⛔ aucune prétention sur le bundle.

---

*Une charpente utile ne décrit pas ce qu'on fera : elle rend le premier pas exécutable et nomme ce qui reste infalsifiable. Ici, le premier pas est une garde d'arbre à trois sorties qui refuse de verdir sur du vide — et qui, sur cette machine, refuse déjà.*

# #1460 — Charpente de la suite de régression structurelle (étages 1/3/4)

**Auteur** : po-2024 (worker) · **Date** : 2026-09-21 · **Base** : master `6694d702`
**Grain** : pool #458 **v8 ⑦** — *« squelette de la suite de régression structurelle (étages 1/3/4), sans artefacts. »*
**Instrument** : [`tools/1460-structure-charpente-pilot.py`](../../tools/1460-structure-charpente-pilot.py) — lecture seule, **ne réclame aucun artefact**.

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
| **Artefact lu** | PDF (parsing déterministe, **sans rendu**) | PDF (extraction texte) | texte séquentiel (PDF) · clés `css`/`mustache` du gabarit |
| **Source de l'attendu** | **les organes de contrat existants** (§4) — ⛔ pas de re-dérivation ici | sondes par langue (liste nommée) | ordre `CardSet` + réécriture de classe de langue |
| **Décidable sans verdict humain ?** | **oui** — un nombre de pages et une dimension sont des faits | **oui** — la présence d'un marqueur est un fait | **oui** pour l'ordre et la classe ; ⛔ **non** pour la justesse de la couleur |
| **Mutation falsifiante** | permuter 2 pages ; forcer A4 au lieu de Tarot | injecter `{{` dans une cellule rendue | inverser l'ordre `CardSet` ; retirer une classe de famille |
| **Format** | `841×1189 mm` (A0) · `210×297 mm` (A4) · Tarot ≈ `70×120 mm` · Poker | — | — |

⚠️ **Tolérance pointeur** sur les dimensions (spec #1460) : les trois formats ont des valeurs nominales en mm, que le PDF exprime en points ; l'organe asserte **avec tolérance**, jamais à l'octet — sinon il mesure l'arrondi de la bibliothèque, pas le format.

⚠️ **Deux phases, deux natures de vérité.** A et C sont **structurelles** (l'objet est faux ou il ne l'est pas). B est **statistique** (une proportion) : elle doit donc porter un **seuil nommé et justifié**, sinon elle rendra « 3 % de cyrillique » vert sur une carte vide.

## 3. La garde d'arbre — organe 0, transverse, **exécutable aujourd'hui**

`tools/1460-structure-charpente-pilot.py` — lecture seule, aucun artefact requis.

**Verdict gradué par conséquence** (⛔ pas un booléen) :

| Sortie | Sens |
|---:|---|
| **0** | **MESURABLE** — au moins un arbre frais porte des PDF ; les phases peuvent asserter |
| **2** | **NON MESURABLE** — arbre absent, ou sans PDF, ou **antérieur au corpus** |
| **1** | **DIVERGENT** — arbre frais, mais l'inventaire contredit l'attendu |

⚠️ **L'état 1 est aujourd'hui INATTEIGNABLE, et l'organe le dit lui-même.** Aucune source d'attendu n'y est branchée — **volontairement** : dériver un compte ici **dupliquerait** les organes de contrat (§4) et fabriquerait un second référentiel qui dériverait du premier. L'état 1 s'activera quand la **phase A** branchera sa source. ⛔ Ne pas lire « verdict à trois états » comme « trois états implémentés ».

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

⛔ **Aucune des trois phases n'est implémentée** — ce document décrit des contrats, il ne les exécute pas · ⛔ **l'état 1 (DIVERGENT) est inatteignable** aujourd'hui (§3) · ⛔ **la garde ne dit rien de la justesse** : un arbre frais peut être faux — fraîcheur ≠ conformité · ⛔ elle ne voit **pas** la surface de rendu, et un corpus édité **après** la régénération rend l'arbre périmé sans qu'elle le sache (elle compare des **dates**, pas des **contenus**) · ⛔ les **formats** cités (Tarot ≈ `70×120 mm`, Poker) sont des **ordres de grandeur** recopiés du corps de #1460, ⛔ **non re-mesurés** sur des PDF produits — je n'en ai aucun sous la main · ⛔ la borne de balayage de l'organe est **nommée** (`Target/` des deux configurations, hors `Archive/`), donc **un scan borné ne vaut pas énumération** · ⛔ les tolérances et seuils de §2 sont des **points de mesure nommés**, pas des valeurs calibrées — le calibrage exige un arbre frais, donc le tag.

## 8. Ce qui est livré

- **La charpente** : ce document — contrat par phase, frontière vs l'existant, pattern d'armement.
- **L'organe 0**, exécutable et falsifié : [`tools/1460-structure-charpente-pilot.py`](../../tools/1460-structure-charpente-pilot.py) — lecture seule, sortie graduée 0/2/1.
- **Son contrôle inverse, committé** : [`tools/test_1460-structure-charpente-pilot.py`](../../tools/test_1460-structure-charpente-pilot.py) — 7 cas, stdlib, arbres synthétiques + 1 *grounding* sur le dépôt réel. ⚠️ Livré dans un **second temps** : la mutation du premier jet vivait dans un scratchpad, donc un relecteur devait croire une transcription — **une mutation non committée n'est pas un contrôle, c'est une affirmation**.
- **Inchangé** : `#1460` reste **POST-TAG** ; aucune phase n'est ouverte ; ⛔ aucune prétention sur le bundle. ⛔ **Rien n'est câblé en CI** : le pilot comme son test s'exécutent **à la main** (la suite elle-même est « inerte en CI » — pas d'arbre `Target/` en CI) ; câbler le seul test du contrat reste une **option non tranchée**, pas un fait.

---

*Une charpente utile ne décrit pas ce qu'on fera : elle rend le premier pas exécutable et nomme ce qui reste infalsifiable. Ici, le premier pas est une garde d'arbre à trois sorties qui refuse de verdir sur du vide — et qui, sur cette machine, refuse déjà.*

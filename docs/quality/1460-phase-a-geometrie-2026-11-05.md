# #1460 phase A — géométrie de page et comptes de pages sur les PDF produits

**Auteur** : po-2023 (worker) · **Date** : 2026-11-05 (erratum §3.1 le même jour au soir) · **Base** : master `19c7efac`
**Grain** : pool #458 — *« #1460 phase A : une PR par phase, sur le bundle local. Ce sont uniquement des tests. »*
**Charpente** : [`1460-charpente-suite-regression.md`](1460-charpente-suite-regression.md) (po-2024, 21/09) — ce document en exécute la **phase A** et en corrige deux points par la mesure.

---

## 1. Ce que la phase A livre

| Fichier | Rôle | Exécuté par |
|---|---|---|
| `Tests/PdfAssembly/PdfPageGeometryIntegrity.cs` | le **vérificateur pur** (uniformité + format, tolérance pointeur) et la **garde d'arbre** à trois états | — (bibliothèque) |
| `Tests/PdfAssembly/PdfStructurePhaseAContractTests.cs` | le **contrôle inverse** : 8 témoins, dont 2 qui encodent des mesures du 05/11 | **CI** (aucun artefact) |
| `VisualTests/PdfStructurePhaseAGateTests.cs` | l'**organe** : ouvre les PDF produits, applique le vérificateur, et réutilise `PdfDeckCountDerivation` (#1187) pour les comptes | machines de régénération (PdfPig) |
| `tools/1460-structure-charpente-pilot.py` | **correctif** : `DEFAULT_REPO` n'est plus un chemin absolu (§4) | à la main |
| `tools/test_1460-structure-charpente-pilot.py` | 7 → **8** cas (le défaut de `--repo` est désormais éprouvé) | CI (`python-tools-tests.yml`) |

⛔ **Aucun code de production n'est modifié.** La phase A n'ajoute que des tests et un correctif d'instrument.

## 2. Les mesures qui fondent l'attendu (05/11, arbre local, 8/8 langues)

64 PDF dans chaque configuration, **géométrie identique sur les 8 langues** :

| Document | Page produite (pt) | en mm | Pages (arbre du 15/08) |
|---|---|---:|---:|
| `Fallacies_Web_A0` | 2384 × 3371 | 841,0 × 1189,2 | 1 |
| `Fallacies_Web_A4` · `_Thumbnails_A4` | 595 × 842 | 209,9 × 297,0 | 15 · 9 |
| `PokerCards` | 180 × 252 | 63,5 × 88,9 | 334 |
| `PokerCards_Print&Play_A4` | 595 × 842 | 209,9 × 297,0 | 6 |
| `TarotCards` | 169,92 × 320,16 | **59,9 × 112,9** | 381 (pré-#1288) |
| `TarotCards_Print&Play_A4` | 595 × 842 | 209,9 × 297,0 | 99 (pré-#1536) |
| `TarotCards_Virtues` | 169,92 × 320,16 | **59,9 × 112,9** | 226 |

Les comptes de pages de cette colonne sont **périmés** (arbre antérieur au corpus, §5) et ne sont cités que pour situer l'objet : ils ne sont **pas** assertés, et l'organe refuse justement de les lire. Il en va de même des deux lignes **Tarot** : mesurées sur un arbre antérieur au changement de nominal du 02/09 (§3.1), elles portent l'ancien nominal — le Poker, lui, est resté au même nominal et sert de preuve que produit = config.

## 3. Deux corrections à la charpente, apportées par la mesure

### 3.1 ERRATUM (05/11 au soir) — le nominal d'une page carte EST dérivable de la config

La première mouture de cette section (livrée avec #2156) affirmait deux choses — *« le Tarot produit ne fait pas 70 × 120 mm »* et *« `FrontCards.WidthMM/HeigthMM` ne dimensionne pas la page d'un deck »* — dont la **seconde est fausse**, et la première décrivait un arbre périmé, pas la production courante. Correction le même jour, preuve à l'appui :

**La chaîne complète** (l'analyse initiale s'était arrêtée au premier maillon) :

```
config FrontCards/BackCards.WidthMM × HeigthMM
  → ImageHelper.cs:170-185 : Density 300 dpi PUIS ResizeInMM(WidthMM, HeigthMM, BorderMM) sur
    chaque PNG de carte                          ← le maillon manqué (lu comme « asset embarqué »)
  → Magick.NET écrit la page à la taille physique du PNG (PdfManager.GenerateAlternateFaceAndBack)
```

**Deux preuves indépendantes** :

1. **Le Poker produit mesure exactement son nominal de config** (63,5 × 88,9 mm — §2). Si la config ne dimensionnait pas la page, cette égalité exacte serait un hasard impossible.
2. **L'écart Tarot mesuré le 05/11 était l'âge de l'arbre** : #1250 (02/09) a changé le nominal Tarot (front ET back, toutes langues) ; l'arbre local date du **15/08** — antérieur. Sa page porte **l'ancien nominal** (mesurée à la tolérance d'arrondi près), et c'est exactement ce que la garde d'arbre refusait déjà (STALE, 77 j).

**Conséquence sur l'organe** : le nominal étant dérivable, la porte l'**asserte** (config ± 1 mm, aucune valeur recopiée — l'attendu se lit dans la config au moment du run). Le témoin CI devient le **discriminant ancien/nouveau nominal** (`Witness_Tarot_Produced_Under_The_Old_Nominal_Is_Red_Against_The_Current_Config`) : une page produite sous l'ancien nominal contre le nominal courant de config est ROUGE avec les deux nominaux nommés. La bande de plausibilité et l'égalité inter-langues subsistent comme gardes-fous (et restent les seuls prédicats pour un document carte sans nominal de config).

⚠️ **La première validation réelle de l'assertion reste la prochaine régénération fraîche** (§5) — les témoins CI prouvent que le vérificateur sait distinguer les deux nominaux, pas qu'un arbre régénéré sera vert.

### 3.2 `PageSizes.A4` n'est pas 595 × 842 pt

| | Valeur |
|---|---|
| `QuestPDF.Helpers.PageSizes.A4` | **595,4 × 841,9 pt** (210,04 × 297,00 mm) |
| MediaBox **écrit** dans le PDF | **595 × 842 pt** (arrondi au point par QuestPDF) |
| `PageSizes.A0` | 2384,2 × 3370,8 pt (841,09 × 1189,14 mm) ; MediaBox écrit 2384 × 3371 |

⇒ la tolérance pointeur n'est pas une élégance de spec, c'est une **nécessité mesurée** : l'écart déclaré/produit vaut 0,14 mm, et un contrôle à l'octet aurait été rouge sur un PDF sain. Elle est fixée à **1 mm**, ce qui absorbe cet arrondi sans absorber une erreur de format.

## 4. Correctif d'instrument — `DEFAULT_REPO` en dur

`tools/1460-structure-charpente-pilot.py` portait `DEFAULT_REPO = r"D:\Dev\Argumentum"`. Lancé depuis **ce** worktree, il a mesuré le **clone principal** (`depot : D:\Dev\Argumentum`) : un verdict autoritaire sur un autre objet que celui qu'on croit regarder — la famille exacte que la charpente dénonce (« mesure sur l'arbre dont la date correspond, jamais le premier trouvé »). Sur toute machine où le dépôt vit ailleurs, la même ligne rendait `REPO INVALIDE` alors qu'un arbre valide était à côté.

Le défaut est désormais **la racine du dépôt qui porte le script** (`tools/..`), surchargeable par `--repo` ; le cas 8 du test de contrat l'éprouve en lançant le pilote **sans `--repo`, depuis un cwd étranger**.

## 5. Ce que l'organe refuse, et ce qu'il n'a pas encore vu

**La garde d'arbre est la précondition** (charpente §1, quatre états). Les trois états NON MESURABLE sont couverts par témoin ; le quatrième — **DIVERGENT** (arbre frais, inventaire contredit) — était déclaré *inatteignable* (charpente §3) : la phase A lui branche sa source (les documents **activés** de la config × 8 langues = **80 PDF attendus**), et `JudgeTree_Reports_Divergent_When_A_Fresh_Tree_Misses_An_Expected_Pdf` l'épingle (64/80 → 1, 80/80 → 0).

**Mesuré sur cette machine, 05/11** :

```
# Garde d'arbre : NON MESURABLE — arbre ANTÉRIEUR au corpus de 77,0 j — STALE
# Verdict du pilote indépendant : NON MESURABLE (sortie 2), 64 PDF, 15/08 < 31/10
```

L'organe est donc **ROUGE**, avec le motif, le chemin mesuré et la commande qui rend le verdict. ⛔ Pas de `Skip` : un skip reproduirait le défaut que la garde ferme (#909).

⚠️ **Ce lot n'a jamais vu l'organe vert sur un arbre réel.** Il a été vu **rouge** (ci-dessus), et le vérificateur a été vu **rouge et vert** sur des mesures synthétiques (8 témoins CI). La première exécution sur un arbre frais — donc la première validation réelle — aura lieu à la prochaine régénération clobbered. C'est une propriété de la phase A, pas un oubli : il n'existe aucun arbre frais sur cette machine.

## 6. Ce que la phase A n'établit pas

- ⛔ **La justesse d'une taille** : un format peut être régulier, uniforme, identique dans les 8 langues et faux pour autant. L'organe dit *conforme à l'attendu*, jamais *correct*.
- ⛔ **L'orientation** d'une page (aucun contrôle de rotation) ni l'ordre des faces/dos (c'est la phase C, et #1187 pour les comptes).
- ⛔ **L'intérieur d'un PDF** : l'uniformité est échantillonnée sur **première, médiane, dernière page** — borne nommée, l'intérieur n'est pas balayé (un PDF de 381 pages coûte des secondes par page ; un défaut de `PageSize` se voit dès la première).
- ⛔ **Les documents Web** ne sont pas couverts par l'égalité inter-langues (seules les pages cartes le sont) : leur format est déclaré (A4/A0) et c'est cela qui est asserté.
- ⛔ **La conversion des formats carte en nominal** — ~~elle exigerait la taille physique des PNG et l'arbitrage du §3.1~~ **faite** par l'erratum §3.1 : le nominal vient de la config, la chaîne `ResizeInMM` l'impose au PNG. Reste hors périmètre : la *justesse* de ce nominal pour la fabrication (c'est #1187/#1188 qui décident de la boîte et du devis, et eux seuls).

---

*Phase A : la mesure a corrigé deux chiffres de la charpente — un arrondi de bibliothèque (595 × 842) et un faux « nominal non dérivable » (erratum §3.1 : la chaîne `ResizeInMM` rend la config souveraine, et l'écart Tarot du 05/11 était l'âge de l'arbre). L'organe est rouge sur cette machine, et il dit pourquoi.*

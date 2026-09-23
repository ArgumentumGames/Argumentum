# #1499 grain G3 — Dossier d'arbitrage Fallacies : champ `desc_fr` + liste courte (PR autonome)

**Date** : 2026-09-23 · **Lane** : po-2024 (worker) · **Base** : `origin/master` `9fe92572`
**Instrument** : [`docs/corpus/fallacies-desc-diff.py`](../corpus/fallacies-desc-diff.py)
— exécuté **tel que livré** avant commit (rc=0, témoins a–d PASS, sortie `fallacies-desc-diff.json` non committée).
**Dispatch** : pool #458 c.5796151802 (v13) — « G3 : ⛔ ne pas poser de question de régime. Livrer directement le dossier **et** sa liste courte des pertes (format G2-C), sous la règle C. »

**⛔ Garde centrale respectée** : aucune écriture dans le CSV. **Règle C d'entrée** (owner 23/09 12:35Z, [#1499 c.5794911968](https://github.com/ArgumentumGames/Argumentum/issues/1499#issuecomment-5794911968)) : *« Si des choses ont progressé on les garde meilleures donc C on fait du cas par cas »* — **défaut = garder**, la liste ne retient que ce qui mérite l'attention de l'owner, avec une recommandation par ligne.

---

## Réponse en une ligne

G3 mesure **150 cellules `desc_fr` non-IDENT** sur les cartes imprimées HEAD, dont **110 forment la surface agentique** (SUBST, geste postérieur à la baseline 2024). La **liste courte qui suit retient 7 items** — 2 recommandent de revenir à l'ancien, 5 de garder l'actuel en connaissance de cause.

---

## 1. Ventilation (150 cellules)

| classe × couche | agentique | pré-agentique | typo-seul-agent | TOTAL |
|---|---:|---:|---:|---:|
| SUBST | **110** ⭐ | 39 | 1 | 150 |
| C / TRONQ / VIDE / AJOUT / FILL | 0 | 0 | 0 | 0 |
| **TOTAL** | **110** | 39 | 1 | **150** |

⭐ **Surface = 110** (agentique × SUBST). Aucun texte amputé, aucune cellule vidée, aucun remplissage : tout est reformulation ou remplacement, comme pour G2.

## 2. Méthode — identique à G2 (#1518), deux différences

Architecture reprise telle quelle de l'instrument G2 (passe 1 imprimé→HEAD par `path` + contrôle 5 sœurs ré-appariées par titre, 16 jointures ; passe 2 PK baseline imprimées ; couches par PK contre `62b561e75` ; contrôle 6 déplacements verbatim).

1. **Témoin (d) calibré par sonde préalable** : PK 2 (path 1.1) `desc_fr` non-IDENT — 9/10 PK sondés en écart baseline→HEAD le 23/09 (ce dépôt), PK 2 confirmé.
2. **Contrôle 6 rend 1 déplacement** (PK 2 : le texte actuel vit verbatim sur une autre carte d'archive) — contre 3 pour `example_fr`.

Témoins : (a) archives lisibles 70/169 PASS · (b) baseline ∩ HEAD = 175 PASS · (c) PK 598/603/680 couverts PASS · (d) PK 2 rendu PASS. rc=0.

## 3. LISTE COURTE DES PERTES — 7 items (format G2-C)

*À lire sans ouvrir l'issue : une recommandation, l'ancien, l'actuel, une raison en mots courants. Les 103 autres écarts restent tels quels (défaut : garder).*

### Recommandation : revenir à l'ancien (2)

**PK 636 — « Sophisme de régression »**
- Ancien : *Vous attribuez une cause erronée à ce qui n'est que le résultat d'une fluctuation tout à fait normale.*
- Actuel : *Vous attribuez à tort un effet particulier à ce qui n'est qu'un retour à la normale après une fluctuation.*
- Raison : l'actuel inverse cause et effet — le travers est d'**inventer une cause** pour ce qui n'est qu'un retour à la moyenne, pas d'attribuer un effet au retour.

**PK 1361 — « Procès en incohérence »**
- Ancien : *Vous accusez votre contradicteur d'avoir changé d'opinion au fil du temps.*
- Actuel : *Vous relevez les incohérences ou les contradictions de votre interlocuteur, ou vous lui opposez des arguments erronés qui peuvent néanmoins lui sembler valides.*
- Raison : la seconde moitié de l'actuel (« arguments erronés qui semblent valides ») parle d'autre chose que le titre ; l'ancien était sur la cible, trop étroit — l'idéal est l'actuel **réduit à sa première moitié**.

### Recommandation : garder l'actuel — la carte a changé de sophisme sous ce nom (3)

**PK 112 — « Sophisme moraliste »**
- Ancien (l'imprimé décrivait l'appel à la nature) : *Vous considérez comme naturel ce que vous approuvez et comme contre nature ce que vous réprouvez.*
- Actuel : *Vous jugez que certains faits sont vrais ou faux en fonction de vos principes moraux, et non sur la base d'une démonstration objective.*
- Raison : titre, définition et exemple actuels décrivent tous le moralisme — l'ensemble tient ; l'ancien décrivait un autre sophisme.

**PK 658 — « Infini fallacieux »**
- Ancien (décrivait la charge de la preuve) : *Vous exigez que soit prouvée la véracité de la moindre des propositions avancées par votre contradicteur.*
- Actuel : *Votre argument repose sur une mauvaise compréhension de la notion d'infini.*
- Raison : l'actuel colle au titre ; l'ancien décrivait l'exigence de preuve, pas l'infini.

**PK 740 — « Inférence immédiate erronée »**
- Ancien (décrivait le sophisme quantifié) : *Votre argument fait une implication erronnée à partir d'une proposition quantifiée.*
- Actuel : *À partir d'une seule prémisse, vous tirez directement une conclusion incorrecte en lui attribuant des propriétés ou des implications qu'elle ne soutient pas logiquement.*
- Raison : l'actuel définit l'inférence immédiate illicite, exactement le titre ; l'ancien parlait de quantificateurs. (L'ancien portait en outre la coquille « erronné ».)

### Recommandation : garder l'actuel — perte de nuance acceptée en connaissance de cause (2)

**PK 55 — « Sauvetage ad hoc »**
- Ancien : *Vous répondez à de justes critiques de vos arguments par des explications sans fondement qui rendent la proposition de plus en plus irréfutable.*
- Actuel : *Face à des objections valables, vous improvisez des justifications sans fondement pour échapper à la critique.*
- Raison : l'actuel perd le cœur du sauvetage ad hoc — chaque excuse rend la thèse **un peu plus irréfutable** ; plus court, mais appauvri.

**PK 219 — « Humour »**
- Ancien : *Vous utilisez des ressorts humoristiques pour détourner l'attention, atténuer l'effet de vos propos ou emporter la bienveillance de l'auditoire.*
- Actuel : *Vous recourez à l'humour pour rendre votre argumentation plus sympathique, sans vous appuyer sur des faits.*
- Raison : l'actuel ne garde qu'une des trois fonctions (séduire) et perd détourner l'attention et atténuer les propos.

## 4. Signaux systématiques (une ligne chacun)

- **Registre** : deux définitions de l'imprimé étaient à la 3e personne (« Consiste à… », PK 1365 et 1398) — l'actuel les met au « vous », cohérent avec le reste du deck. Rien à arbitrer.
- **Amélioration à signaler** : PK 1313 (« Évasion ») et PK 1314 (« Fausse piste ») partageaient **mot pour mot** la même définition à l'imprimé ; l'actuel les différencie (éviter le fond vs détourner le sujet). Le remplacement est une réparation.
- **Déplacement** : PK 2 — la définition actuelle vit mot pour mot sur une autre carte d'archive (contrôle 6) : remplacement d'époque, pas une réécriture à arbitrer.

## 5. Hors surface

**39 pré-agentiques** (geste d'époque owner, déjà présent en 2024) et **1 typo-seul-agent** sortent de la liste par construction — mêmes limites que G2 §6.

## 6. Ce que ce dossier n'établit pas

- La valeur éditoriale des 103 écarts non listés — le défaut est de les garder, conforme à la règle C.
- La qualification fine de la liste courte est une lecture humaine **nominative** : 7 items jugés un par un, le reste non jugé.
- Ne couvre pas `example_fr` (G2, #1518), `text_fr` (G1-W, #1522), ni Virtues/Rules/Scenarii.

**⛔ Aucune question de régime** : la règle C s'applique, le défaut est posé, aucune écriture n'est demandée ici. Les 2 items « revenir » attendront une fenêtre d'écriture si l'owner les suit.

---

*Mesuré par po-2024 le 2026-09-23. Instrument stdlib, lecture seule stricte, rc=0 exécuté tel que livré avant commit.*

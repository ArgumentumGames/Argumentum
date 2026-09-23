# #1499 grain G3 — Dossier d'arbitrage Fallacies : champ `desc_fr` + liste courte (PR autonome, v2)

**Date** : 2026-09-23 · **Lane** : po-2024 (worker) · **Base** : `origin/master` `9fe92572`
**Instrument** : [`docs/corpus/fallacies-desc-diff.py`](../corpus/fallacies-desc-diff.py)
— exécuté **tel que livré** avant commit (rc=0, témoins a–d PASS, sortie `fallacies-desc-diff.json` non committée, désormais couverte par `.gitignore`).
**Dispatch** : pool #458 c.5796151802 (v13) — « G3 : ⛔ ne pas poser de question de régime. Livrer directement le dossier **et** sa liste courte des pertes (format G2-C), sous la règle C. »
**v2 (review ai-01, #1523)** : la comparaison de l'agent se fait contre la **version 2024** (consigne précisée #458 c.5801633624), les écarts d'époque passent en section séparée.

**⛔ Garde centrale respectée** : aucune écriture dans le CSV. **Règle C d'entrée** (owner 23/09 12:35Z, [#1499 c.5794911968](https://github.com/ArgumentumGames/Argumentum/issues/1499#issuecomment-5794911968)) : *« Si des choses ont progressé on les garde meilleures donc C on fait du cas par cas »* — **défaut = garder**.

---

## Réponse en une ligne

G3 mesure **150 cellules `desc_fr` non-IDENT**, dont **110 forment la surface agentique**. Comparée à la version 2024, **le passage agentique n'a rien fait perdre : 110 cellules lues, 0 perte, 0 contresens** — que des reformulations, avec trois corrections au passage (§3). Les sept écarts de sens restants datent de la période **2022 → 2024** : ils sont requalifiés en section séparée (§3bis), chacun nommant la cible d'un éventuel retour.

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

1. **Témoin (d) calibré par sonde préalable** : PK 2 (path 1.1) `desc_fr` non-IDENT — 9/10 PK sondés en écart baseline→HEAD le 23/09, PK 2 confirmé.
2. **Contrôle 6 rend 1 déplacement** (PK 2) — contre 3 pour `example_fr`.

**Comparaison de l'agent** (consigne #458 c.5801633624) : pour chacun des 110, le texte de la version **2024** (`62b561e75`) a été comparé au texte HEAD, cellule par cellule — « l'ancien » de la liste courte §3 désigne cette version-là.

Témoins : (a) archives lisibles 70/169 PASS · (b) baseline ∩ HEAD = 175 PASS · (c) PK 598/603/680 couverts PASS · (d) PK 2 rendu PASS. rc=0.

## 3. Ce que le passage agentique a fait perdre (2024 → HEAD) — **rien**

**110 cellules lues à la version 2024, 110 reformulations, 0 perte de fait, 0 perte de nuance, 0 contresens introduit.** Le passage agentique sur `desc_fr` a réécrit le style : apostrophes typographiques, « de manière à » → « pour », synonymes proches (« sens établi » ↔ « significations établies », « série » ↔ « séquence », « à tort » ↔ « faussement »).

**Trois corrections relevées au passage** (des gains, pas des pertes — citées pour mémoire) :

- **PK 632** « Interprétation quantitative erronée » : « relations quantitative inexactes » (faute d'accord, 2024) → « relations quantitatives inexactes ».
- **PK 784** « Syllogisme invalide » : « Vous construiez… » (coquille de temps, 2024) → « Vous construisez… ».
- **PK 889** « Mensonge » : « dites des choses qui ne sont pas vraies » (2024, décrit aussi l'erreur) → « affirmez quelque chose que vous savez faux » (la définition exacte du mensonge).

⇒ **Aucune ligne à arbitrer dans cette section : rien à revenir, tout à garder.** (Consigne : une liste vide est un résultat.)

## 3bis. Écarts entre l'imprimé 2022 et la version 2024 — gestes d'époque (section séparée)

Ces sept textes ont changé **entre l'édition imprimée et la version 2024**, avant le passage agentique (mesure ai-01 sur #1523 : similarité imprimé→2024 de 0,18 à 0,56, puis 2024→HEAD de 0,52 à 0,95 ; l'agent n'a que reformulé). ⛔ Revenir ici déferait un geste d'époque, pas un geste de l'agent — c'est l'owner qui décide, en connaissance de cause. La cible d'un éventuel retour est **l'imprimé 2022**.

**PK 636 — « Sophisme de régression »**
- Imprimé 2022 : *Vous attribuez une cause erronée à ce qui n'est que le résultat d'une fluctuation tout à fait normale.*
- Version 2024 puis actuelle : *Vous attribuez à tort un effet particulier à ce qui n'est qu'un retour à la normale après une fluctuation.*
- Raison : la formulation de 2024 est **ambiguë** — on peut y lire « attribuer un effet au retour à la normale » là où le travers est d'inventer une cause ; l'imprimé est plus clair. (Requalifié après review : « inversé » était trop fort.)

**PK 1361 — « Procès en incohérence »**
- Imprimé 2022 : *Vous accusez votre contradicteur d'avoir changé d'opinion au fil du temps.*
- Version 2024 puis actuelle : *Vous relevez les incohérences ou les contradictions de votre interlocuteur, ou vous lui opposez des arguments erronés qui peuvent néanmoins lui sembler valides.*
- Raison : c'est la 2024 qui a élargi la définition et ajouté la seconde moitié (« arguments erronés »), qui parle d'autre chose que le titre.

**PK 112 — « Sophisme moraliste »**
- Imprimé 2022 (décrivait l'appel à la nature) : *Vous considérez comme naturel ce que vous approuvez et comme contre nature ce que vous réprouvez.*
- Version 2024 puis actuelle : *Vous jugez que certains faits sont vrais ou faux en fonction de vos principes moraux, et non sur la base d'une démonstration objective.*
- Raison : le changement de sophisme sous ce titre date de 2024 ; titre, définition et exemple actuels décrivent tous le moralisme — l'ensemble tient.

**PK 658 — « Infini fallacieux »**
- Imprimé 2022 (décrivait la charge de la preuve) : *Vous exigez que soit prouvée la véracité de la moindre des propositions avancées par votre contradicteur.*
- Version 2024 puis actuelle : *Votre argument repose sur une mauvaise compréhension de la notion d'infini.*
- Raison : changement de cible en 2024 ; l'actuel colle au titre.

**PK 740 — « Inférence immédiate erronée »**
- Imprimé 2022 (décrivait le sophisme quantifié) : *Votre argument fait une implication erronnée à partir d'une proposition quantifiée.*
- Version 2024 puis actuelle : *À partir d'une seule prémisse, vous tirez directement une conclusion incorrecte en lui attribuant des propriétés ou des implications qu'elle ne soutient pas logiquement.*
- Raison : changement de cible en 2024 ; l'actuel définit l'inférence immédiate illicite, exactement le titre.

**PK 55 — « Sauvetage ad hoc »**
- Imprimé 2022 : *Vous répondez à de justes critiques de vos arguments par des explications sans fondement qui rendent la proposition de plus en plus irréfutable.*
- Version 2024 puis actuelle : *Face à des objections valables, vous improvisez des justifications sans fondement pour échapper à la critique.*
- Raison : la version 2024 a perdu le cœur du sauvetage ad hoc — chaque excuse rend la thèse **un peu plus irréfutable**.

**PK 219 — « Humour »**
- Imprimé 2022 : *Vous utilisez des ressorts humoristiques pour détourner l'attention, atténuer l'effet de vos propos ou emporter la bienveillance de l'auditoire.*
- Version 2024 puis actuelle : *Vous recourez à l'humour pour rendre votre argumentation plus sympathique, sans vous appuyer sur des faits.*
- Raison : la version 2024 n'a gardé qu'une des trois fonctions (séduire) et perdu détourner l'attention et atténuer les propos.

## 4. Signaux systématiques (une ligne chacun)

- **Registre** : deux définitions de l'imprimé étaient à la 3e personne (« Consiste à… », PK 1365 et 1398) — l'actuel les met au « vous », cohérent avec le reste du deck. Rien à arbitrer.
- **Amélioration** : PK 1313 (« Évasion ») et PK 1314 (« Fausse piste ») partageaient **mot pour mot** la même définition à l'imprimé ; l'actuel les différencie (éviter le fond vs détourner le sujet). Le remplacement est une réparation.
- **Déplacement** : PK 2 — la définition actuelle vit mot pour mot sur une autre carte d'archive (contrôle 6) : remplacement d'époque, pas une réécriture à arbitrer.

## 5. Hors surface

**39 pré-agentiques** (geste d'époque owner, déjà présent en 2024) et **1 typo-seul-agent** sortent de la liste par construction — mêmes limites que G2 §6.

## 6. Ce que ce dossier n'établit pas

- La valeur éditoriale des 110 réécritures stylistiques : jugées ici comme neutres en sens (aucune perte relevée), pas comme « meilleures ».
- La qualification de la section 3bis est une lecture humaine **nominative** : 7 items jugés un par un sur 150.
- Ne couvre pas `example_fr` (G2, #1518 mergée), `text_fr` (G1-W, #1522 mergée), ni Virtues/Rules/Scenarii.

**⛔ Aucune question de régime** : la règle C s'applique, le défaut est posé, aucune écriture n'est demandée ici. Les 7 items de la §3bis attendront une décision owner (et une fenêtre d'écriture) si un retour à l'imprimé est voulu.

---

*Mesuré par po-2024 le 2026-09-23. Instrument stdlib, lecture seule stricte, rc=0 exécuté tel que livré avant commit. v2 : comparaison agent contre 2024, écarts d'époque séparés, PK 636 nuancé, note .gitignore traitée.*
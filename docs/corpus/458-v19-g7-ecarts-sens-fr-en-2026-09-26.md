# Écarts de sens français ↔ anglais — mesure 0-écriture (pool v19, grain 7)

Arbre master `9085635b` (2026-09-26) · dispatch [#458 c.5848171107](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5848171107).

Périmètre : les **18 groupes anglais marqués `[fr=distinct]`** du dossier #1585 (corrigé) — rangées partageant un
titre anglais alors que leurs titres français diffèrent. Question : le titre anglais d'une des rangées **trahit-il le sens
français** ? Jugement outillé par les définitions (`desc_fr`/`desc_en`) citées ci-dessous. **Deck d'abord** (4 groupes sur 18).

## Synthèse

| Classe | Groupes | Lecture |
|---|---:|---|
| **A — écart de sens réel** | 7 (dont 4 avec carte deck) | l'en a fusionné une distinction que la fr garde ; proposition par rangée ci-dessous |
| **B — doublon de contenu quasi exact** | 5 | `desc_fr` identiques ou quasi : la collision découle du doublon, matière dédup (pas renommage) |
| C — synonymie fr sans perte en | 6 | rien à faire côté en |

Preuve mesurée de la classe B : égalité octet des `desc_fr` au sein du groupe —
- «Emotive conjugation» : desc_fr identiques = True
- «Firehose of falsehood» : desc_fr identiques = True
- «Idiosyncratic language» : desc_fr identiques = False
- «On the spot fallacy» : desc_fr identiques = False
- «Spreading» : desc_fr identiques = False

## [A] «Appeal to accomplishment» — DECK

**la polarité s'inverse : 1386 REJETE faute de réussite, 79 ACCORDE sur réussite — un seul libellé en couvre deux sens opposés.**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 1386 | `7.3.2.3` |  | «Appel à la réussite» | «Appeal to accomplishment» | Vous rejetez ce que dit une personne parce qu’elle ne s’est pas illustrée dans le domaine concerné.… |
| 79 | `1.2.1.2.1` | 2 | «Argument d’accomplissement» | «Appeal to accomplishment» | Vous fondez la validité des propos de quelqu’un sur ses succès dans le domaine concerné.… |

**Propositions par rangée (décision : ai-01) :**
- PK 1386 (Appel à la réussite) : proposition : « Appeal to lack of accomplishment » (ou équivalent ad hominem) — décision ai-01
- PK 79 (Argument d’accomplissement) : garde « Appeal to accomplishment » (sens positif, correspond au fr)

## [A] «Circular reasoning» — DECK

**exemple déclencheur d'ai-01 : la fr distingue définir (829) et argumenter (699) en cercle.**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 829 | `5.1.3.2` |  | «Définition circulaire» | «Circular reasoning» | Vous définissez un terme au moyen de la notion même qu’il représente et que vous prétendez définir.… |
| 699 | `4.1.1.1` | 1 | «Argument circulaire» | «Circular reasoning» | Vous raisonnez en cercle : chaque argument repose sur l’acceptation préalable de la conclusion.… |

**Propositions par rangée (décision : ai-01) :**
- PK 699 (Argument circulaire) : garde « Circular reasoning »
- PK 829 (Définition circulaire) : proposition : « Circular definition » (terme établi ; la desc_en parle bien de définir un terme)

## [A] «Moving the goalposts» — DECK

**la fr distingue reformuler après réfutation (1005) et changer les critères en silence (973).**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 1005 | `6.2.2.5.1` |  | «Argument après contestation» | «Moving the goalposts» | Une fois votre argument réfuté, vous le reformulez pour éviter de perdre la face.… |
| 973 | `6.2` | 1 | «Changement de cap» | «Moving the goalposts» | Vous changez les critères du débat sans le dire, afin d’éviter d’admettre votre erreur.… |

**Propositions par rangée (décision : ai-01) :**
- PK 1005 (Argument après contestation) : proposition : « Ad hoc rescue » (reformulation pour sauver la face) — décision ai-01
- PK 973 (Changement de cap) : garde « Moving the goalposts » (changement de critères = sens exact)

## [A] «Vagueness» — DECK

**la fr distingue termes vagues (856) et données imprécises (667).**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 856 | `5.3.2.1` |  | «Expression vague» | «Vagueness» | Vous utilisez des termes si vagues qu’ils ne permettent pas de comprendre clairement ce que vous vou… |
| 667 | `3.3.1` | 2 | «Imprécision» | «Vagueness» | Vos arguments s’appuient sur des données imprécises, ce qui conduit à des conclusions incertaines.… |

**Propositions par rangée (décision : ai-01) :**
- PK 667 (Imprécision) : proposition : « Imprecision » (données imprécises → conclusions incertaines) — décision ai-01
- PK 856 (Expression vague) : garde « Vagueness » (sens exact)

## [C] «Amazing familiarity»

**descs quasi identiques (information qu'on ne devrait pas détenir) — synonymie fr, pas de perte en.**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 64 | `1.1.3.3.1.1` |  | «Familiarité inexpliquée» | «Amazing familiarity» | Vous mentionnez des informations que, d’après vos propres déclarations, vous n’êtes pas censé déteni… |
| 764 | `4.3.1.1.2.1.1` |  | «Familiarité étonnante» | «Amazing familiarity» | Votre argument contient des informations qui semblent impossibles à obtenir.… |

## [C] «Appeal to confidence»

**confiance vs conviction : nuances fr voisines du même concept.**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 75 | `1.2.1.1.2` |  | «Appel à l’assurance» | «Appeal to confidence» | La confiance que vous accordez à une proposition suffit, à vos yeux, à en garantir la validité.… |
| 301 | `2.2.1.1` |  | «Appel à la conviction» | «Appeal to confidence» | Vous présentez une affirmation comme vraie simplement parce qu’une personne y croit avec assurance.… |

## [A] «Contrast effect»

**la fr distingue la tactique rhétorique (390, pousser l'adversaire) et le biais cognitif (1040, perception altérée).**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 390 | `2.3.1.2.3.1` |  | «Jeu de contraste» | «Contrast effect» | Vous poussez votre adversaire vers une position en la faisant contraster avec la position inverse.… |
| 1040 | `6.3.1.1.1.2.1.1` |  | «Effet de contraste» | «Contrast effect» | Votre jugement est influencé par la comparaison d’alternatives contrastées, altérant ainsi votre per… |

**Propositions par rangée (décision : ai-01) :**
- PK 1040 (Effet de contraste) : garde « Contrast effect » (le biais cognitif porte ce nom)
- PK 390 (Jeu de contraste) : proposition : « Contrast framing » (tactique sur l'adversaire) — décision ai-01

## [C] «Deepity»

**décision règle C rendue le 26/09 (revue #1585) : gardé — les descs décrivent le même concept.**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 206 | `2.1.1.5.2.3` |  | «Effet puits» | «Deepity» | Vous employez des formules creuses et pompeuses qui donnent une impression de profondeur, tout en re… |
| 307 | `2.2.1.2.2.2` |  | «Appel à la pseudo-profondeur» | «Deepity» | Vous utilisez des affirmations vagues qui paraissent profondes pour impressionner, sans véritable fo… |

## [B] «Emotive conjugation»

**desc_fr identiques octet pour octet, titres fr differant par casse/accents : doublon de contenu.**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 218 | `2.1.1.6.4` |  | «Conjugaison Émotive» | «Emotive conjugation» | Vous utilisez des verbes différents pour décrire le même comportement selon que vous approuvez ou no… |
| 812 | `5.1.2.2.2.2` |  | «Conjugaison émotive» | «Emotive conjugation» | Vous utilisez des verbes différents pour décrire le même comportement selon que vous approuvez ou no… |

## [B] «Firehose of falsehood»

**desc_fr identiques octet pour octet : doublon de contenu.**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 460 | `2.3.2.3.1.1.2` |  | «Lance à incendie du mensonge» | «Firehose of falsehood» | Vous diffusez en masse des messages répétés sur de multiples canaux (médias, réseaux sociaux), sans … |
| 915 | `6.1.1.2.1.6` |  | «Lance à incendie de mensonges» | «Firehose of falsehood» | Vous diffusez en masse des messages répétés sur de multiples canaux (médias, réseaux sociaux), sans … |

## [A] «Gambler's fallacy»

**miroir exact de la paire ru tranchée le 26/09 (retour « Софизм игрока ») : la fr distingue, l'en a fusionné.**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 654 | `3.2.2.3.2` |  | «Erreur du parieur» | «Gambler's fallacy» | Vous supposez que des écarts statistiques vont forcément se compenser lors de futurs événements aléa… |
| 717 | `4.1.2.6` |  | «Sophisme du joueur» | «Gambler's fallacy» | Vous imaginez une suite d’événements sans tenir compte de leur indépendance aléatoire.… |

**Propositions par rangée (décision : ai-01) :**
- PK 654 (Erreur du parieur) : garde « Gambler's fallacy » (compensation statistique)
- PK 717 (Sophisme du joueur) : à trancher par ai-01 : même perte que la ru réparée — proposition « Fallacy of the player » ou distinction équivalente

## [B] «Idiosyncratic language»

**desc_fr quasi identiques (inversion de sous-phrase) : doublon de contenu.**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 821 | `5.1.2.3.2` |  | «Langage idiosyncrasique» | «Idiosyncratic language» | Vous attribuez un sens particulier et inattendu à des termes pour appuyer votre point de vue.… |
| 1304 | `7.1.3.4` |  | «Langage idiosyncratique» | «Idiosyncratic language» | Vous attribuez à des termes un sens particulier et inattendu afin d’appuyer votre point de vue.… |

## [C] «Less-is-better effect»

**même effet canonique (Hsee), libellés fr différents sans perte.**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 1042 | `6.3.1.1.1.2.1.1.2` |  | «Effet « le moins est meilleur »» | «Less-is-better effect» | Vous préférez une option inférieure ou plus modeste lorsqu’elle est évaluée isolément, mais pas lors… |
| 1239 | `6.3.2.3.2.2` |  | «Effet « moins, c’est mieux »» | «Less-is-better effect» | Vous jugez une option meilleure lorsqu’elle est évaluée seule que lorsqu’elle est comparée directeme… |

## [C] «Not invented here»

**les deux descs décrivent le NIH ; « Hyperlocalisation » est un choix fr, pas une trahison en.**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 106 | `1.2.2.3.2` |  | «Hyperlocalisation» | «Not invented here» | Vous refusez une proposition sous prétexte qu’elle a été élaborée hors de la communauté concernée pa… |
| 1187 | `6.3.2.1.2.1.2` |  | «Effet « pas d’ici »» | «Not invented here» | Vous rejetez les idées, les recherches ou les produits qui ne sont pas développés au sein de votre p… |

## [B] «On the spot fallacy»

**desc_fr quasi identiques ×3 (variantes de formulation d'une même définition) : doublon de contenu en triple exemplaire.**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 477 | `2.3.2.3.2.2.3` |  | «Sophisme sur-le-champ» | «On the spot fallacy» | Vous considérez votre interlocuteur comme incompétent s’il ne peut pas réciter des données spécifiqu… |
| 988 | `6.2.1.3.1` |  | «Sophisme de l’improvisation» | «On the spot fallacy» | Vous jugez votre interlocuteur incompétent s’il ne peut pas citer sur-le-champ des données précises … |
| 1348 | `7.2.2.1.1.1` |  | «Sophisme de l’examen sur-le-champ» | «On the spot fallacy» | Vous jugez votre interlocuteur incompétent s’il ne peut pas citer immédiatement des données précises… |

## [C] «Pseudoscience»

**même concept, trait d'union près côté fr.**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 695 | `3.3.3.3` |  | «Pseudo-science» | «Pseudoscience» | Vous faites passer des théories non fondées pour des faits scientifiques.… |
| 1271 | `6.3.3.3.1.2` |  | «Pseudoscience» | «Pseudoscience» | Vous présentez ou adoptez des croyances ou des pratiques dites scientifiques, mais qui manquent de p… |

## [A] «Sound bite»

**la fr distingue la petite phrase politique (187) et l'accroche réductrice (944).**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 187 | `2.1.1.4.2` |  | «Petite phrase» | «Sound bite» | Afin de convaincre votre auditoire, vous glissez dans un discours public une phrase accrocheuse cens… |
| 944 | `6.1.2.1.1` |  | «Accroche» | «Sound bite» | Vous résumez votre propos par une formule courte et marquante, qui retient l’attention mais peut êtr… |

**Propositions par rangée (décision : ai-01) :**
- PK 187 (Petite phrase) : garde « Sound bite » (petite phrase politique = sens exact)
- PK 944 (Accroche) : proposition : « Hook » (accroche) — décision ai-01

## [B] «Spreading»

**desc_fr quasi identiques (ponctuation près) : doublon de contenu (« Répandage » / « Déluge argumentatif »).**

| PK | path | carte | fr | en | desc_fr (extrait) |
|---|---|---|---|---|---|
| 476 | `2.3.2.3.2.2.2` |  | «Répandage» | «Spreading» | Vous parlez extrêmement vite pour présenter un maximum d’arguments en un minimum de temps, rendant d… |
| 1300 | `7.1.3.2.1` |  | «Déluge argumentatif» | «Spreading» | Vous parlez extrêmement vite pour présenter un maximum d’arguments en un minimum de temps, ce qui re… |

## Ce que cette mesure n'établit pas

- Elle ne décide rien : les propositions de la classe A sont de la matière à arbitrage ai-01 (règle C).
- La classe B (desc_fr identiques) ne relève pas de la règle C : fusionner/dédupliquer des rangées change la taxonomie,
  pas un libellé — hors périmètre du présent pool, à dispatcher séparément si retenu.
- Le jugement de sens repose sur les définitions du corpus, pas sur une source externe ; une relecture native (ex. « Contrast
  framing », « Hook ») reste souhaitable avant correction.
- Ne mesure que les groupes en `[fr=distinct]` ; un titre en unique-porteur peut aussi trahir son fr, hors périmètre.

## Annexe — instrument rejouable

```python
# Rejouer : python g7_ecarts_sens.py (depuis la racine du repo, arbre master 9085635b)
# Grain 7 du pool v19 : ecarts de sens fr<->en dans les groupes anglais [fr=distinct]
# du dossier #1585 (corrigé). Pour chaque groupe : le titre anglais d'une des rangées
# trahit-il le sens français ? Proposition par rangée. Deck d'abord. MESURE SEULEMENT.
import csv, collections, os

rows = list(csv.DictReader(open("Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv", encoding="utf-8-sig")))
by_pk = {r["PK"]: r for r in rows}
groups = collections.defaultdict(list)
for r in rows:
    if r.get("text_en"):
        groups[r["text_en"]].append(r)
distinct = {t: rs for t, rs in groups.items()
            if len(rs) >= 2 and len({r["text_fr"] for r in rs}) > 1}

# Analyse par groupe (jugement de sens outillé par les definitions) :
# classe A = ecart reel (proposition par rangee), B = doublon de contenu quasi exact
# (desc_fr identiques ou quasi -- la collision en decoule, matiere dedup), C = synonymes
# fr sans perte de sens en. PROP = propositions par PK, mesure seulement.
ANALYSIS = {
    "Appeal to accomplishment": ("A", "la polarité s'inverse : 1386 REJETE faute de réussite, 79 ACCORDE sur réussite — un seul libellé en couvre deux sens opposés",
        {"1386": "proposition : « Appeal to lack of accomplishment » (ou équivalent ad hominem) — décision ai-01",
         "79": "garde « Appeal to accomplishment » (sens positif, correspond au fr)"}),
    "Circular reasoning": ("A", "exemple déclencheur d'ai-01 : la fr distingue définir (829) et argumenter (699) en cercle",
        {"829": "proposition : « Circular definition » (terme établi ; la desc_en parle bien de définir un terme)",
         "699": "garde « Circular reasoning »"}),
    "Moving the goalposts": ("A", "la fr distingue reformuler après réfutation (1005) et changer les critères en silence (973)",
        {"1005": "proposition : « Ad hoc rescue » (reformulation pour sauver la face) — décision ai-01",
         "973": "garde « Moving the goalposts » (changement de critères = sens exact)"}),
    "Vagueness": ("A", "la fr distingue termes vagues (856) et données imprécises (667)",
        {"856": "garde « Vagueness » (sens exact)",
         "667": "proposition : « Imprecision » (données imprécises → conclusions incertaines) — décision ai-01"}),
    "Amazing familiarity": ("C", "descs quasi identiques (information qu'on ne devrait pas détenir) — synonymie fr, pas de perte en", {}),
    "Appeal to confidence": ("C", "confiance vs conviction : nuances fr voisines du même concept", {}),
    "Contrast effect": ("A", "la fr distingue la tactique rhétorique (390, pousser l'adversaire) et le biais cognitif (1040, perception altérée)",
        {"390": "proposition : « Contrast framing » (tactique sur l'adversaire) — décision ai-01",
         "1040": "garde « Contrast effect » (le biais cognitif porte ce nom)"}),
    "Deepity": ("C", "décision règle C rendue le 26/09 (revue #1585) : gardé — les descs décrivent le même concept", {}),
    "Emotive conjugation": ("B", "desc_fr identiques octet pour octet, titres fr differant par casse/accents : doublon de contenu", {}),
    "Firehose of falsehood": ("B", "desc_fr identiques octet pour octet : doublon de contenu", {}),
    "Gambler's fallacy": ("A", "miroir exact de la paire ru tranchée le 26/09 (retour « Софизм игрока ») : la fr distingue, l'en a fusionné",
        {"654": "garde « Gambler's fallacy » (compensation statistique)",
         "717": "à trancher par ai-01 : même perte que la ru réparée — proposition « Fallacy of the player » ou distinction équivalente"}),
    "Idiosyncratic language": ("B", "desc_fr quasi identiques (inversion de sous-phrase) : doublon de contenu", {}),
    "Less-is-better effect": ("C", "même effet canonique (Hsee), libellés fr différents sans perte", {}),
    "Not invented here": ("C", "les deux descs décrivent le NIH ; « Hyperlocalisation » est un choix fr, pas une trahison en", {}),
    "On the spot fallacy": ("B", "desc_fr quasi identiques ×3 (variantes de formulation d'une même définition) : doublon de contenu en triple exemplaire", {}),
    "Pseudoscience": ("C", "même concept, trait d'union près côté fr", {}),
    "Sound bite": ("A", "la fr distingue la petite phrase politique (187) et l'accroche réductrice (944)",
        {"187": "garde « Sound bite » (petite phrase politique = sens exact)",
         "944": "proposition : « Hook » (accroche) — décision ai-01"}),
    "Spreading": ("B", "desc_fr quasi identiques (ponctuation près) : doublon de contenu (« Répandage » / « Déluge argumentatif »)", {}),
}

# preuve mesuree : egalite octet des desc_fr par groupe (affirmation de la classe B)
desc_equality = {}
for t, rs in distinct.items():
    ds = [r.get("desc_fr", "") for r in rs]
    desc_equality[t] = len(set(ds)) == 1

L = []
A = L.append
A("# Écarts de sens français ↔ anglais — mesure 0-écriture (pool v19, grain 7)")
A("")
A("Arbre master `9085635b` (2026-09-26) · dispatch [#458 c.5848171107](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5848171107).")
A("")
A(f"Périmètre : les **{len(distinct)} groupes anglais marqués `[fr=distinct]`** du dossier #1585 (corrigé) — rangées partageant un")
A("titre anglais alors que leurs titres français diffèrent. Question : le titre anglais d'une des rangées **trahit-il le sens")
A("français** ? Jugement outillé par les définitions (`desc_fr`/`desc_en`) citées ci-dessous. **Deck d'abord** (4 groupes sur 18).")
A("")
A("## Synthèse")
A("")
nA = sum(1 for v in ANALYSIS.values() if v[0] == "A")
nB = sum(1 for t, v in ANALYSIS.items() if v[0] == "B")
nC = sum(1 for v in ANALYSIS.values() if v[0] == "C")
nA_deck = sum(1 for t, v in ANALYSIS.items() if v[0] == "A" and any(r.get("carte") for r in distinct[t]))
A(f"| Classe | Groupes | Lecture |")
A(f"|---|---:|---|")
A(f"| **A — écart de sens réel** | {nA} (dont {nA_deck} avec carte deck) | l'en a fusionné une distinction que la fr garde ; proposition par rangée ci-dessous |")
A(f"| **B — doublon de contenu quasi exact** | {nB} | `desc_fr` identiques ou quasi : la collision découle du doublon, matière dédup (pas renommage) |")
A(f"| C — synonymie fr sans perte en | {nC} | rien à faire côté en |")
A("")
A("Preuve mesurée de la classe B : égalité octet des `desc_fr` au sein du groupe —")
for t in sorted(distinct):
    if ANALYSIS[t][0] == "B":
        A(f"- «{t}» : desc_fr identiques = {desc_equality[t]}")
A("")
deck_first = sorted(distinct.items(), key=lambda kv: (-any(r.get("carte") for r in kv[1]), kv[0]))
for t, rs in deck_first:
    cls, lecture, props = ANALYSIS[t]
    deck = " — DECK" if any(r.get("carte") for r in rs) else ""
    A(f"## [{'ABC'.index(cls) and '' or ''}{cls}] «{t}»{deck}")
    A("")
    A(f"**{lecture}.**")
    A("")
    A("| PK | path | carte | fr | en | desc_fr (extrait) |")
    A("|---|---|---|---|---|---|")
    for r in sorted(rs, key=lambda r: (bool(r.get("carte")), r["path"])):
        d = (r.get("desc_fr") or "").replace("\n", " ")[:100]
        A(f"| {r['PK']} | `{r['path']}` | {r.get('carte','')} | «{r['text_fr']}» | «{t}» | {d}… |")
    if props:
        A("")
        A("**Propositions par rangée (décision : ai-01) :**")
        for pk in sorted(props):
            A(f"- PK {pk} ({by_pk[pk]['text_fr']}) : {props[pk]}")
    A("")
A("## Ce que cette mesure n'établit pas")
A("")
A("- Elle ne décide rien : les propositions de la classe A sont de la matière à arbitrage ai-01 (règle C).")
A("- La classe B (desc_fr identiques) ne relève pas de la règle C : fusionner/dédupliquer des rangées change la taxonomie,")
A("  pas un libellé — hors périmètre du présent pool, à dispatcher séparément si retenu.")
A("- Le jugement de sens repose sur les définitions du corpus, pas sur une source externe ; une relecture native (ex. « Contrast")
A("  framing », « Hook ») reste souhaitable avant correction.")
A("- Ne mesure que les groupes en `[fr=distinct]` ; un titre en unique-porteur peut aussi trahir son fr, hors périmètre.")
A("")
A("## Annexe — instrument rejouable")
A("")
A("``\`python")
A(open(__file__, encoding="utf-8").read().replace("``\`", "``\\`"))
A("``\`")
A("")
A(f"*po-2024 — mesure, 0 écriture sur le corpus · grain 7 · {len(distinct)} groupes : {nA} A / {nB} B / {nC} C*")

os.makedirs("docs/corpus", exist_ok=True)
with open("docs/corpus/458-v19-g7-ecarts-sens-fr-en-2026-09-26.md", "w", encoding="utf-8", newline="\n") as f:
    f.write("\n".join(L) + "\n")
print(f"groupes: {len(distinct)} | A {nA} / B {nB} / C {nC}")
print(f"dossier: {len(L)} lignes")

```

*po-2024 — mesure, 0 écriture sur le corpus · grain 7 · 18 groupes : 7 A / 5 B / 6 C*

# Campagne de fidélité Scénarios — **en** (0 écriture)

**Mandat** : pool #458, dispatch c.5976781537 — la campagne porte sur les 8 langues, et le bloc
EN en est une. **Statut** : **0 écriture** — aucune cellule CSV modifiée.

## 1. Instrument — et pourquoi cette passe a une place à part

Le bloc EN des Scénarios n'est pas seulement une langue cible : **il est aussi la source de
traduction d'autres langues**. La passe zh l'a établi par la mesure (la carte 6.1.1 : FR 121
caractères → zh 17, le zh étant la traduction littérale de `suggestion_en`, qui dit lui-même
autre chose que le FR). Le bloc EN doit donc être lu **pour lui-même**, comme une traduction
du FR dont les écarts se propagent.

⚠️ **Piège d'instrument propre à cette passe**, trouvé par contrôle croisé entre deux scripts :
le bloc EN est **nu** pour `title`, `context`, `issue`, `smoothTalker`, `drawer` et ne porte le
suffixe que sur **`suggestion_en`**. La dérivation générique du nom de colonne (couper le `_`
final du suffixe) envoyait `suggestion_` vers la colonne **FR** — l'instrument comparait alors
la colonne FR **à elle-même** et rendait **167 faux « =FR »**, un par carte. Corrigé : le vrai
compte est **26**. *(Les autres langues n'étaient pas touchées : `suggestion_zh` etc. sont
corrects.)*

## 2. Couverture et cellules identiques au FR

**1169 / 1169 cellules** (167 × 7 champs rendus) — **0 manquante.**

**26 cellules seulement sont identiques au FR**, et **25 sont des noms propres** : *Veto*,
*Sinon*, *Charles VII*, *Louis XVI*, *Gretel*, *Judas*, *Eve*, *Salomon*, *Hades*, *Loki*,
*Thor*, *Zeus*, *Dorothy*, *Alice*, *Moriarty*, *Sherlock Holmes*, *Don Juan*, *Pollock*,
*Ergo sum*, *Casper*, *Titanic*, *Ross*, *Rachel* — plus les cognats exacts *Adoption*,
*Camouflage*. La 26ᵉ est ***Moralisation*** (6.1.1, titre) : l'anglais s'écrit *Moralization* ;
la graphie française est conservée. Observation, pas défaut de sens.

## 3. Défauts durs

| Carte | Champ | Mesure |
|---|---|---|
| **5.3.2** | `smoothTalker` | **« An anti -vaccin »** — espace parasite avant le trait d'union **et** mot **français** dans une cellule anglaise (l'anglais dit *anti-vaxxer* / *anti-vaccine*). Cellule **rendue sur la face** |
| **2.2.1** | `suggestion_en` | point final **omis** (« Vade retro Satanas ») alors que le FR porte « Vade retro, Satanas**.** » |
| **6.3.2** | `suggestion_en` | point final **omis** ("…could elect a traitor") alors que le FR ponctue |

### Propagation mesurée de ces deux omissions

| Carte | L'EN omet | Langues qui **suivent l'EN** | Langues qui **réparent** |
|---|---|---|---|
| 2.2.1 | le point final | **ar**, **es** | **zh**, **pt** |
| 6.3.2 | le point final | **pt** | **zh** |

C'est le résultat le plus net de la campagne : **une omission de ponctuation dans le bloc EN
se retrouve dans 1 à 2 autres langues**, et deux langues la corrigent spontanément. Un écran
qui ne lirait qu'une langue à la fois ne verrait jamais ce lien.

## 4. Divergences de contenu du bloc EN (relevées pendant la passe zh)

Ces trois écarts étaient initialement imputés au zh ; la lecture à trois voies les a rendus à
l'EN, où ils ont leur origine. **Ils appartiennent à ce dossier.**

| Carte | Champ | FR | EN |
|---|---|---|---|
| **3.1.1** | suggestion | « Laissez-moi tenter ma chance ce soir ; si j'échoue, je vous laisse le champ libre. » | "We can both try, we'll see who gets picked." |
| **3.2.2** | suggestion | « …où est-ce que je vais mettre **mes affaires** ? » | "…where am I going to put **my clothes**?" |
| **6.1.1** | suggestion | « Pour restaurer la confiance, il faut des règles claires : le délai doit courir à partir des faits, comme en droit commun. » | "It is absolutely necessary to rebuild the people's confidence in the political class." |
| **5.2.5** | enjeu | « **Il** doit convaincre Rachel qu'il ne l'a pas trompée. » | "**She** considers it an infidelity: try to prove her wrong." |

Les trois premiers **changent le contenu** (le 6.1.1 remplace une phrase entière par un slogan) ;
le quatrième **déplace le sujet** de la phrase du baratineur vers le piocheur.

⚠️ **Nuance mesurée** : toutes les langues ne suivent pas l'EN. Le **pt** suit le FR sur 2.2.1
et l'EN sur 6.3.2 ; le **fa** suit le FR (euro plutôt que dollar, *Marcellus* à un seul s là où
l'EN écrit *Marsellus*). La source **varie selon la langue et parfois selon la cellule** : un
écart FR↔langue n'est jamais, à lui seul, la preuve d'un défaut.

## 5. Observations

- **3.1.5** — « **Pizzaïolo** » (titre + rôle) : l'italien s'écrit *pizzaiolo*, sans tréma.
  Diacritèse à confirmer en relecture native.
- **5.3.4** — titre réduit à « **5g** » (minuscule) là où le FR dit « La conspiration de la 5G ».
- **3.2.16** — « **His wife** » pour le FR « Sa moitié » (neutre) : un genre est ajouté.
- **11 cellules portent des accents** (*Orléans*, *Mjölnir*, *Déjà Vu*, *Panthéon*, *Périgord*,
  *ménage à trois*…) : **toutes légitimes**, ce sont des noms propres ou des emprunts que
  l'anglais garde accentués.

## 6. Verdict en

- Couverture **complète**.
- **3 défauts durs** : 1 cellule hybride FR/EN avec espace parasite (5.3.2, rendue sur la face),
  2 omissions de point final **dont la propagation dans ar/es/pt est mesurée**.
- **4 divergences de contenu** d'origine EN, dont une qui remplace une phrase entière (6.1.1) —
  ce sont elles qui avaient été imputées à tort au zh avant la lecture à trois voies.
- 3 observations de graphie/registre.
- ⚠️ Tout sweep futur qui écrit dans une langue cible doit **corriger l'EN d'abord** là où
  l'EN est la source, sinon la correction se propage à l'envers.

*po-2024*

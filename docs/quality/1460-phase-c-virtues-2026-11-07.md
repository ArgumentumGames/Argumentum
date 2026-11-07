# #1460 phase C — organe 3 : la couleur de famille des vertus (07/11/2026, po-2023)

Transposition de l'organe 2 ([`1460-phase-c-famille-2026-11-07.md`](1460-phase-c-famille-2026-11-07.md) §6 :
« Les autres paquets : Virtues a ses propres classes de famille ; le même motif s'y transposera le
jour voulu ») au paquet **Virtues**. Même moteur (`FamilyColorIntegrity`, source unique sur disque,
linkée dans VisualTests — #1054), mêmes trois ancres (CSV × gabarit × pixel), mêmes bornes. Ce qui
suit documente **ce qui a été mesuré**, pas supposé — deux faits de gabarit ont exigé un changement
du moteur commun, et l'arbre a livré de lui-même l'état que la charpente appelle « périmé ».

## 1. Les objets, mesurés avant d'écrire

| Objet | Fallacies (organe 2) | Virtues (organe 3) | Nature |
|---|---|---|---|
| CSV | `Argumentum Fallacies - Taxonomy.csv`, 1 408 nœuds, 175 cartes (colonne `carte`) | `Argumentum Virtues - Taxonomy.csv`, **223 nœuds, 131 cartes** (colonne **`card`**) | mesuré 07/11 |
| Colonne de classe | `Famille_camelCase` | **`family_fr_camelcase`** | mesuré (en-tête) |
| Délimiteurs CSV | virgule (0 `;` aux octets) | virgule (0 `;` aux octets) | mesuré aux octets |
| Gabarit | `Argumentum_Fallacies_Face_fr.json` | `Argumentum_Virtues_Face_fr.json` | — |
| Blocs famille | 1 sélecteur par bloc | **7 blocs sur 8 en LISTES de sélecteurs**, chacun précédé d'un commentaire CSS | mesuré 07/11 |
| Noms parsés / couleurs distinctes | 8 / 8 | **15 / 8** (7 alias hérités du renommage additif) | mesuré |
| Préfixe PNG | `argumentum_fallacies_{path}..` | `argumentum_virtues_{path}..` | mesuré |
| Faces sur l'arbre Release local | 175/langue | **113/langue sur 131 attendues** (uniforme, 8 langues) | mesuré 07/11 |

⚠️ La ligne « délimiteurs » est là parce que l'intuition « les CSV du dépôt sont à points-virgules »
a été **mesurée fausse** avant d'entrer dans un code ou une doc : les deux taxonomies sont à
virgules, et le lecteur commun (`HarvestCardIdsCsv`, CsvHelper par défaut) les lit telles quelles.

## 2. Les deux changements du moteur commun

### 2.1 Listes de sélecteurs (renommage additif)

Le gabarit Virtues écrit `card.exactitudeMathématique,\ncard.rigueurMathématique,\ncard.sensQuantitatif { … }`
— trois générations du même nom de famille dans un seul bloc. L'ancien regex
(`card\.X\s*\{`) n'attrapait que **le dernier** sélecteur avant l'accolade : par chance l'ordre
additif met le nom courant en dernier, donc l'organe 2 « marchait » — mais un renommage qui
écrirait le nouveau nom **en premier** rendrait la classe introuvable **sans qu'aucun échec ne le
nomme**. Le parseur itère désormais les blocs règle (`sélecteurs { corps }`), exige que le
sélecteur soit **exactement** une liste `card.X, card.Y` (un sélecteur descendant
`card.X .icone` porte la couleur d'un enfant — il reste exclu), et assigne la couleur à **chaque**
nom de la liste. Les commentaires CSS (`/* --- */`, mesuré : un avant chaque bloc famille Virtues)
sont retirés avant le parse — mesuré, le commentaire précède le sélecteur sur la même capture.

### 2.2 La garde de séparation court sur les couleurs distinctes

Les 7 alias hérités partagent **exactement** la couleur de leur famille (7 paires mesurées à
distance 0,0). Sur l'ancienne garde — paires sur les **noms** — un gabarit sain à 8 familles et
15 noms rougissait 7 fois : l'organe aurait refusé de verdir sur un gabarit **correct**. La garde
groupe désormais les noms par couleur et confronte les **couleurs distinctes**. Sur le gabarit
Fallacies (8 noms, 8 couleurs), le comportement est inchangé — témoin CI dédié.

## 3. Calibrage (07/11, arbre Release local, 8 langues × 113 cartes présentes)

| Mesure | Valeur | Réserve vs borne |
|---|---|---|
| Distance dominante ↔ couleur de famille | **0,00** partout (PNG sans perte, zone de teinte pleine) | borne 30, réserve ∞ |
| Part dominante minimale | **0,4635** (`pt`) ; max 0,5187 (`ar`) | borne 5 %, réserve **9,3×** |
| Familles distinctes les plus proches | **114,3** (`inférenceMaîtrisée` #8dc801 ↔ `honnêtetéIntellectuelle` #ffc307) | borne 60, réserve **1,9×** |

Les bornes de l'organe 2 sont conservées telles quelles : les deux paquets mesurent le même fait
au même instrument, avoir deux jeux de bornes serait une dérive de référentiel.

⚠️ Les 18 cartes manquantes (§4) ne pèsent pas dans ce calibrage : la distance et la part sont
mesurées sur les cartes **présentes**. Le jour d'une régén couvrant Virtues, les 131 seront
mesurées — l'attendu des bornes ne bougera que si la mesure bouge.

## 4. Ce que l'organe a trouvé en s'armant : l'arbre Virtues est périmé

Premier armement sur l'arbre Release local : **113 faces par langue, 18 jointures manquantes,
uniforme sur les 8 langues**. Datations mesurées : les PNG Virtues datent du **25/06/2026**, le CSV
a bougé jusqu'au **31/10/2026** (dernier commit `9038690d`, amendements pk 208). Le côté Fallacies
du même arbre est frais (organe 2 : 175/175) — **le périmé est spécifique au paquet Virtues** :
aucune régén depuis juin n'a couvert les 18 cartes ajoutées au corpus entre juin et août.

Chemins manquants (mesuré, identique 8 langues) : `2.1.1.2.3, 2.2, 2.2.1, 2.2.2, 2.2.3, 2.3.1,
2.3.2, 4.2, 4.2.1, 4.2.2, 4.3, 4.3.2, 4.3.2.1, 4.3.3, 4.3.3.1, 4.3.3.1.1, 4.3.3.1.2, 4.3.3.2.5`.

C'est l'**état 3 de la charpente §1** : « un arbre qui existe mais ne contient pas les objets de la
mesure est un échec, pas un skip ». Le message d'échec le dit mot pour mot (« arbre PÉRIMÉ côté
Virtues ») et nomme les chemins — l'organe ne verdira pas tant qu'une régén couvrant Virtues n'a
pas eu lieu, **et c'est le comportement voulu** : le tableau CLAUDE.md « Virtues Tarot 131 faces »
décrit le corpus, pas cet arbre. La remédiation (fenêtre de régén Virtues) relève de la
coordination — le dashboard la porte déjà pour `es`/`ar` (pk 208).

## 5. Livré

- Moteur étendu : `Tests/Structure/FamilyColorIntegrity.cs` — listes de sélecteurs + commentaires
  + garde sur couleurs distinctes (une seule source, compilée par link dans VisualTests).
- Témoins CI : `Tests/Structure/FamilyColorIntegrityTests.cs` — **4 nouveaux** (15 au total) :
  liste 3 générations avec commentaire, sélecteur descendant exclu + commentaire dans le corps,
  **alias à 0,0 ne rougissent pas la garde** (le témoin du changement), déterminisme dernier-bloc.
- Organe d'arbre : `VisualTests/Structure/VirtuesFamilyColorGateTests.cs` — env
  `ARGUMENTUM_PDF_BUNDLE_ROOT`, NON MESURABLE rendu tel quel sans arbre, échec **nommé** sinon
  (échec de péremption distinct de l'échec de couleur).

## 6. Ce que cet organe n'établit pas

- La **justesse esthétique** de la couleur — verdict humain (charpente §2), inchangé.
- La **fraîcheur** : il la **détecte** (jointures manquantes nommées) mais la garde d'arbre
  (organe 0) en reste le propriétaire.
- Les couleurs secondaires (`--color-text-1…3`) et le dos (`card_001.png`, hors périmètre).
- **Rien sur les 18 cartes manquantes** au-delà de leur nom : l'organe dit qu'elles ne sont pas
  sur l'arbre, pas pourquoi la régén les a sautées — la réponse est dans le calendrier de régén,
  pas dans les pixels.

*po-2023*

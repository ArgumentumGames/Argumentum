# Grain 10 — fidélité des Vertus : 28 cellules corrigées (pool #458, c.6036602091)

**Date :** 2026-10-07 · **Lane :** po-2024 · **Dépôt :** `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv`
**Empreinte du fichier :** `sha256` avant `a4a6c16ebe9558c6…` → après `715554af63a305c3…`

## 0. Pourquoi ce grain est délégable (et pourquoi il a fallu le dire)

Le dossier du grain 6 (#1793) posait la fidélité des Vertus **contre l’EN imprimée**. Cette prémisse est
**fausse** : les Vertus n’ont jamais été imprimées — le fichier `Argumentum_TarotCards_Virtues_{lang}.pdf`
existe, mais aucune carte Vertu n’est sortie en relecture. La conséquence est double :

- il n’y a **pas de contrôle « EN imprimée »** à respecter, donc pas de GO owner requis pour corriger ;
- les langues **fidèles** au FR deviennent l’**ancre** de la correction, au lieu d’un jugement éditorial.

Détail de la correction de prémisse : #1793 c.6036588022. Répartition notée en §4.

## 1. Périmètre mesuré — 28 cellules / 17 rangées / 6 langues

| # | pk | colonne | langue | avant | après |
|---:|---:|---|---|---|---|
| 1 | 2 | `description_en` | en | Well-structured argument, based on facts or solid reasoning | Well-structured argument, based on facts or solid reasoning. |
| 2 | 3 | `description_en` | en | Argument based on concrete and verifiable elements, rather than on op… | Argument based on concrete and verifiable elements, rather than on op… |
| 3 | 4 | `description_en` | en | Argument that logically derives a conclusion from premises accepted a… | Argument that logically derives a conclusion from premises accepted a… |
| 4 | 5 | `description_en` | en | Argument that proposes a probable conclusion from specific examples, … | Argument that proposes a probable conclusion from specific examples, … |
| 5 | 6 | `description_en` | en | Evidence supporting the argument through concrete examples, testimoni… | Evidence supporting the argument through concrete examples, testimoni… |
| 6 | 7 | `description_en` | en | Precise establishment of the debate objective and the parties' positi… | Precise establishment of the debate objective and the parties' positi… |
| 7 | 8 | `description_en` | en | Support for argumentation through specific and relevant examples to m… | Support for argumentation through specific and relevant examples to m… |
| 8 | 11 | `description_en` | en | Reasoning based on rational logic and facts that avoids preconceived … | Reasoning based on rational logic and facts that avoids preconceived … |
| 9 | 12 | `description_en` | en | Bases of an argument considered solid and credible | Bases of an argument considered solid and credible. |
| 10 | 54 | `description_en` | en | Manipulating one's interlocutor by resorting to threats or promises c… | Refusing to manipulate one's interlocutor through threats or promises… |
| 11 | 54 | `description_ru` | ru | Манипулирование собеседником посредством угроз или обещаний является … | Отказ манипулировать собеседником посредством угроз или обещаний, свя… |
| 12 | 54 | `description_pt` | pt | Manipular o interlocutor recorrendo a ameaças ou promessas é uma viol… | Recusar manipular o interlocutor recorrendo a ameaças ou promessas li… |
| 13 | 54 | `description_es` | es | La manipulación del interlocutor mediante amenazas o promesas constit… | Rechazar la manipulación del interlocutor mediante amenazas o promesa… |
| 14 | 89 | `description_en` | en | Progressive reasoning method using a series of intermediate inference… | Progressive reasoning method using a series of intermediate inference… |
| 15 | 90 | `description_en` | en | Rigorous and logical sequence of arguments that does not reveal incon… | Rigorous and logical sequence of arguments that does not reveal incon… |
| 16 | 128 | `title_en` | en | Acceptable informal logic | Solid informal logic |
| 17 | 128 | `title_ru` | ru | Приемлемая неформальная логика | Прочная неформальная логика |
| 18 | 128 | `title_ar` | ar | منطق غير صوري مقبول | منطق غير صوري متين |
| 19 | 128 | `title_es` | es | Lógica informal aceptable | Lógica informal sólida |
| 20 | 162 | `description_en` | en | The expected objective for the opponent must be attainable and fair, … | Requirement of clear, measurable and fair criteria for evaluating the… |
| 21 | 176 | `title_en` | en | Taking the opponent's ideological biases into account | Taking the parties' ideological biases into account |
| 22 | 176 | `title_ru` | ru | Учет идеологических предубеждений оппонента | Учет идеологических предубеждений сторон |
| 23 | 176 | `title_pt` | pt | Consideração dos enviesamentos ideológicos do adversário | Consideração dos enviesamentos ideológicos das partes |
| 24 | 176 | `title_ar` | ar | مراعاة التحيزات الأيديولوجية لدى الخصم | مراعاة التحيزات الأيديولوجية لدى الأطراف |
| 25 | 176 | `title_es` | es | Consideración de los sesgos ideológicos del adversario | Consideración de los sesgos ideológicos de las partes |
| 26 | 176 | `title_fa` | fa | درنظرگرفتن سوگیری‌های ایدئولوژیک طرف مقابل | درنظرگرفتن سوگیری‌های ایدئولوژیک طرف‌های درگیر |
| 27 | 220 | `description_en` | en | It is important to let each participant speak without interruption in… | It is important to let each participant speak without interruption in… |
| 28 | 221 | `description_en` | en | It is important to use a cordial and non-aggressive tone to express o… | It is important to use a cordial and non-aggressive tone to express o… |

Deux familles, distinguées parce qu’elles ne se contrôlent pas de la même façon :

- **15 cellules structurelles** — un contenu faux, écrantable : la forme ancienne disparaît du corpus,
  donc « 0 occurrence » est une preuve. Ce sont `54.en`, `54.ru`, `54.pt`, `54.es`, `128.en`, `128.ru`, `128.ar`, `128.es`, `162.en`, `176.en`, `176.ru`, `176.pt`, `176.ar`, `176.es`, `176.fa`.
- **13 cellules typographiques** (pk 2, 3, 4, 5, 6, 7, 8, 11, 12, 89, 90, 220, 221) — un point final ajouté. La forme ancienne est un
  **préfixe** de la nouvelle : un écran `Contains` matcherait encore la cellule corrigée. Pas d’écran possible,
  donc la garde est l’épingle + un fait de terminaison dédié. Les deux familles sont **entrelacées** dans le
  tableau ci-dessus (son ordre est celui du CSV) : ne pas lire « les 15 premières lignes ».

## 2. Ancrages — ce qui autorise chaque correction

Aucune correction n’est un jugement : chacune est adossée à une langue **fidèle** mesurée.

| Rangée | Défaut | Langues fautives | Ancre(s) |
|---|---|---|---|
| pk 54 | la vertu est décrite comme le **vice** (« Manipuler… ») et le lien aux conséquences a disparu | en, ru, pt, es | **ar, fa, zh** — portent la vertu ET « liées aux conséquences » |
| pk 162 | l’EN seul dérive (« the opponent », « attainable ») | en | **les 7 autres langues** suivent le FR |
| pk 128 | « solide » affaibli en « acceptable » | en, ru, es, ar | **pt** (« sólida »), **fa**, **zh** |
| pk 176 | titre rétréci à « l’opposant » alors que la description dit « les parties » | en, ru, pt, es, ar, fa | **zh** (相关方) |

Le renversement de pk 54 vivait dans **exactement** {en, ru, pt, es} et dans aucune autre langue : c’est une
signature de lignée de traduction, pas une coïncidence — la même mesure a servi au dossier ru (grain 7, #1794).

## 3. Écrans et contrôles

| Contrôle | Instrument | Résultat |
|---|---|---|
| Écran d’éradication (15 cellules) | cellule **parsée** : 1 avant / 0 après | 15/15 conformes, 0 anomalie |
| Même écran en texte **brut** (`Contains`, celui de la garde) | le brut voit-il un 1 sur le fichier pre ? | 15/15 voient leur 1 — l’écran n’est pas aveugle |
| Témoins (fragments légitimes ailleurs) | title_fa `استوار` : 4 rangée(s) · title_ru `оппонент` : 2 rangée(s) · title_ar `مقبول` : 1 rangée(s) · title_pt `sólida` : 1 rangée(s) | survivent — l’écran ne visait que des cellules **entières** |
| Épingle vs fichier | 28 empreintes SHA-256 de la cellule **pleine** | 28/28 d’accord |
| Anti-vacuité | 28 épingles · 28 couples (rangée, colonne) · 17 rangées · 28 empreintes · 16 raisons | conforme |
| Détecteur | mutation en mémoire de pk 54 `description_en` | 1 seul rouge, nommé ; les 27 autres verts |
| **M1 (corpus)** | remise de pk 54 `description_en` à sa valeur d’avant, puis `dotnet test` | **2 rouges exactement** : `Corrected_Cells_Match_Their_Full_Content_Pins` + `Deviating_Cells_Are_Absent_From_The_Corpus` ; 4 autres vertes ; restauration par `cp` — `sha256` revérifié identique |

Garde : `Generation/Converters/Argumentum.AssetConverter.Tests/VirtuesFaithfulnessG10GuardTests.cs` (6 faits).

## 4. Retard déclaré — 10 des 28 cellules vivent aussi dans les cartes mentales

Les corrections de **titre** ne sont pas seulement dans le CSV : les SVG de cartes mentales et leurs
enveloppes HTML embarquent le libellé du nœud. Balayage des 83 `*.svg`/`*.html` de `Cards/` :

| pk | langue | libellé retiré (ancien) | fichiers portant encore l’ancien |
|---:|---|---|---:|
| 128 | en | Acceptable informal logic | 3/3 |
| 128 | ru | Приемлемая неформальная логика | 3/3 |
| 128 | ar | منطق غير صوري مقبول | 3/3 |
| 128 | es | Lógica informal aceptable | 3/3 |
| 176 | en | Taking the opponent's ideological biases into account | 3/3 |
| 176 | ru | Учет идеологических предубеждений оппонента | 3/3 |
| 176 | pt | Consideração dos enviesamentos ideológicos do adversário | 3/3 |
| 176 | ar | مراعاة التحيزات الأيديولوجية لدى الخصم | 3/3 |
| 176 | es | Consideración de los sesgos ideológicos del adversario | 3/3 |
| 176 | fa | درنظرگرفتن سوگیری‌های ایدئولوژیک طرف مقابل | 3/3 |

**30 fichiers** au total (chaque langue-artefact compte 3 : `…content.svg`, `…links.svg`, `Argumentation_Virtues_<lang>.html`).
Ces fichiers **ne sont pas régénérés par cette PR** : ils demandent une re-dérivation FreeMind, hors périmètre.
Le CSV corrigé est la source ; les SVG restent en retard jusqu’à la prochaine re-dérivation des Vertus.

Contrôle inverse : les **descriptions** corrigées (pk 54, pk 162) et les 13 points n’apparaissent dans
**aucun** de ces artefacts — les cartes mentales ne portent que les libellés de nœuds. Mesure : 0 occurrence(s)
de description corrigée dans les `content.svg` des 7 langues. Les 18 cellules non-titre n’ont donc **aucun retard**.

## 4 bis. Second artefact en retard — l’ontologie, et cette fois un organe le tient

Les deux corrections de **titre anglais** changent l’IRI que `VirtueOwlDocumentConfig.GetId` produit.
`docs/ontology/argumentum_virtues.owl` n’a pas été régénéré : la divergence est donc nommée, pas silencieuse —
`VirtueOwlTaxonomyDivergenceCensusTests` l’a **rougie** dès la première exécution de la suite complète.

| Population | Valeur |
|---|---|
| Absentes (produites par la taxonomie, absentes du fichier) | `solidInformalLogic`, `takingThePartiesIdeologicalBiasesIntoAccount` |
| Orphelines (portées par le fichier, plus produites) | `acceptableInformalLogic`, `takingTheOpponentsIdeologicalBiasesIntoAccount` |

Traitement : le plafond « 0 depuis #1681 » est remplacé par une table **`IdentityLag`** — la paire
d’hier et d’aujourd’hui, datée — et l’assertion est une **égalité d’ensembles dans les deux sens** :

- une 3ᵉ divergence non listée rougit (la table n’absorbe rien) ;
- une régénération rougit **aussi** (l’écart tombe à 0 alors que la table est encore là) — le burn-down
  se referme en **supprimant la table**, jamais en la laissant dériver.

Contrôle falsifiant : muter un seul nom de la table fait rougir exactement 1 des 3 faits de la classe
(mesuré), ce qui établit que l’égalité est exacte et non un plafond permissif. La régression ⑱ de pk 176 ar
a été traitée de même : la garde épinglait le titre ar de 176 comme témoin du **verbe** (« مراعاة » *tenir
compte*, opposé à « التعرّف » de 175) — ce verbe est intact, seul l’**objet** a été élargi de « l’adversaire »
aux « parties », que la description ar de la rangée dit elle-même (« لأطراف التبادل »).

## 5. Ce que cette PR ne fait pas

- **Le FR n’est touché nulle part** : il est la source des 17 rangées. Cellules FR dans le diff : **0** (mesuré).
- **Les autres familles typographiques ne sont pas traitées** : les descriptions sans point final se comptent
  ru 4 · pt 13 · es 13 · ar 8 · fa 18 · zh 8 (fr 0 · en 13 traitées ici). Les traiter demande leur propre mesure
  de ponctuation terminale par langue — l’écran ASCII seul rend un faux « tout manquant » en zh (`。！？`).
- **Aucune re-dérivation d’artefact** — ni cartes mentales (§4), ni ontologie (§4 bis) : `v2.0.0-review` reste gelé.
  Les deux retards sont **déclarés et tenus par des organes** (table `IdentityLag` pour l’OWL ; §4 pour les
  SVG), pas laissés en prose.

## 6. Reproduire la mesure

La garde **re-dérive tout à l’exécution** : elle lit le CSV, recalcule les 28 empreintes, refait l’écran
d’éradication et le test de terminaison. Aucun instrument intermédiaire n’a besoin d’être committé.

```
dotnet test "Generation/Converters/Argumentum.AssetConverter.Tests/Argumentum.AssetConverter.Tests.csproj" \
  --filter "FullyQualifiedName~VirtuesFaithfulnessG10GuardTests"
```

Le `sha256` du fichier avant/après est en tête de ce dossier : il suffit à vérifier que la mesure a porté
sur le même octet-à-octet que la correction.

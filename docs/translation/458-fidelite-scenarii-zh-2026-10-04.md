# Campagne de fidélité Scénarios — **zh** (0 écriture)

**Mandat** : pool #458, dispatch c.5976781537 — « nouvelle campagne de fidélité des Scénarios
(jamais relus) : 1 dossier 0 écriture par langue, zh → ar → fa → es → ru → pt → en ».
**Objet** : les 167 cartes du deck Scénarios, lues carte par carte dans les champs que le
gabarit de carte rend réellement.
**Statut** : **0 écriture** — aucune cellule CSV n'a été modifiée par cette campagne.

---

## 1. Instrument

| Point | Valeur mesurée |
|---|---|
| Source | `Cards/Scenarii/Argumentum Scenarii - Cards.csv` |
| Volume | **167 cartes**, 70 colonnes, clé `path` |
| Bloc FR | colonnes 2–9 : `catégorie, sous-catégorie, titre, baratineur, piocheur, contexte, enjeu, suggestion` |
| Bloc EN | colonnes 11–18 : `category, subcategory, title, smoothTalker, drawer, context, issue, suggestion_en` |
| Bloc zh | colonnes 54–61 |
| Champs **rendus** par le gabarit de carte | catégorie, titre, baratineur, piocheur, contexte, enjeu, suggestion (7) |
| `sous-catégorie` | lu, **hors deck** (aucun PDF) |

⚠️ **Cartographie à connaître avant tout sweep i18n** : le bloc EN est **nu** pour `title`,
`context`, `issue`, `smoothTalker`, `drawer` — seul `suggestion_en` porte le suffixe.
Une jointure écrite `*_en` rate donc 5 colonnes sur 8, en silence.

Écrans mécaniques passés : cellules manquantes · cellule identique au FR · latin résiduel ·
ratio de longueur hors bornes · ponctuation finale · espace ASCII adjacent à du CJK.

### 1.1 Correction d'instrument en cours de campagne

Le premier écran de lecture présentait les paires **FR | zh**. Il a été invalidé en route :
la carte **6.1.1** (`suggestion`) montre le FR à 121 caractères — « Pour restaurer la confiance,
il faut des règles claires : le délai doit courir à partir des faits, comme en droit commun. » —
et le zh à 17 caractères : **我们必须重建人民对政治阶层的信任。** Un écart de cette taille se lit
comme une perte de contenu… sauf que le zh est la **traduction littérale de l'EN**
(`suggestion_en` : *"It is absolutely necessary to rebuild the people's confidence in the
political class."*), qui dit lui-même autre chose que le FR.

Tout écart a donc été **re-lu à trois voies FR | EN | zh** (le fichier de lecture côte à côte
et l'écran à trois voies sont dans le scratchpad de session, régénérables en une passe).
Sur les candidats examinés, l'instrument relève **3 cas où le zh suit l'EN contre le FR et
aucun cas inverse** — assez pour imposer la lecture à trois voies, **pas** pour établir la
chaîne de production de la traduction (rapporté, non vérifié).

---

## 2. Couverture

**1336 / 1336 cellules zh remplies** (167 cartes × 8 colonnes). **0 manquante.**
Aucune cellule zh n'est restée en français (`=FR` : 2, ce sont les deux noms propres latins
de la carte 5.2.5 — voir §5).

C'est le résultat principal : **la couverture zh des Scénarios est complète**.

---

## 3. Ce que les écrans ne comptent **pas** comme défaut

À consigner pour l'instrument suivant, qui les compterait sinon comme du bruit :

- **Espace entre CJK et latin/chiffres** — 20 des 24 occurrences relevées (`AI`, `5G`, `S`,
  `2019 年`, `Johnny Hallyday`) sont la **convention typographique chinoise** d'encadrement des
  segments latins, pas une anomalie. Seules 4 occurrences sont d'un autre ordre (§4.7).
- **Latin légitime** — 9 cellules portent du latin : `OK` (3.1.7 ×2), `Bilou` (3.2.6),
  `AI` (4.3.4 ×3), `Ross`/`Rachel` (5.2.5), `Johnny Hallyday` (6.1.3). Aucun n'est une
  contamination : ce sont des noms propres ou des sigles.

---

## 4. Défauts zh confirmés (établis à trois voies)

| # | Carte | Champ | FR | EN | zh | Nature |
|---|---|---|---|---|---|---|
| 1 | **1.1.3** | suggestion | « …pour **mes lions**. » | "…for **my lions**." | 这可是喂我**小猫咪**的上等美味。 | mot substitué (félidé domestique) ; **contredit `issue_zh` de la même carte**, qui dit 狮子 (lions) |
| 2 | **3.2.8** | titre + suggestion | « L'**anniversaire** oublié » / « mon **anniversaire** » | "The Forgotten **Birthday**" / "my **birthday**" | 被忘掉的**纪念日** / 我的**纪念日** | les deux sources disent *anniversaire* ; **`context_zh` de la même carte dit 生日** — incohérence interne |
| 3 | **6.2.1** | titre | « Retrait négocié » | "Negotiated Withdrawal" | 初选连环跳 | titre **inventé** : ne correspond ni au FR ni à l'EN |
| 4 | **4.3.4** | contexte | « …sans aucun garde-fou. » | "…with no safeguards at all." | 诡辩者 声称要打造…却不设置任何安全护栏，**还得说服委员会支持他的项目**。 | l'**enjeu est recopié dans le contexte** (ni FR ni EN ne le portent) — la carte dit deux fois la même chose |
| 5 | **4.3.3** | enjeu | « **il** doit convaincre son fournisseur » | "**he** must convince his supplier" | 这位**转卖商**必须说服供应商 | rôle ajouté ; **`smoothTalker_zh` de la même carte dit 采购商** (acheteur) — incohérence interne |
| 6 | **1.2.3** (×2), **2.1.8** | context/issue, suggestion | point final présent | point final présent | **pas de ponctuation finale** | 3 cellules — ⚠️ **portée corrigée, voir l'erratum en fin de dossier** : c'est un lot inter-langues, pas un trait du zh |
| 7 | **4.3.3 – 4.3.6** | context_zh | — | — | espace parasite après 诡辩者 | **4 cellules en run contigu** ; témoins 4.3.1 / 4.3.2 / 4.3.7 / 4.3.9 **sans** espace ⇒ signature de lot, pas une frappe isolée |

Le défaut 7 est d'un autre ordre que les six autres : c'est une **scorie de lot** dans une
section contiguë, invisible dans un diff de texte mais présente à l'impression.
Les défauts 1, 2, 4, 5 sont des **incohérences internes à la carte** — les plus solides,
parce qu'elles se prouvent sans source externe.

---

## 5. Observations à trancher en relecture native (pas des défauts établis)

- **Rendu des noms propres non uniforme** — 5.2.5 : les rôles restent `Ross` / `Rachel` en
  latin quand le contexte de la **même carte** écrit 罗斯 / 瑞秋 ; 6.1.3 : `enjeu` garde
  `Johnny Hallyday` en latin quand la `suggestion` de la même carte translittère 约翰尼·阿利代.
  Les rôles sont **rendus sur la face** — c'est un choix à arbitrer, pas une erreur.
- **4.2.8** titre — 意外醒来与微妙妥协 : « compromis » lu comme le nom 妥协, et 意外 ajouté
  (absent du FR *et* de l'EN).
- **2.2.9** contexte — ajoute 是神话人物 (« est un personnage mythologique »), absent des deux sources.
- **2.3.1** enjeu — perd « au Kansas » (la `suggestion_zh` de la même carte le garde).
- **5.3.2** piocheur — 一名高风险人群 : classificateur de personne (名) sur un collectif (人群).
- **3.2.16** enjeu — 试图 (« tente de ») là où FR et EN disent « doit convaincre » / "must convince" :
  mode affaibli.
- **3.1.3** titre — 偷偷摘套 (très familier) pour un FR/EN neutres (« Retrait non consenti » / "Stealthing") :
  question de registre.

---

## 6. Écarts **hérités de l'EN** — et non défauts zh

C'est la correction que la lecture à trois voies a produite. Ces trois écarts désignent le
**bloc EN**, pas la traduction chinoise :

| Carte | Champ | FR | EN | zh |
|---|---|---|---|---|
| **3.1.1** | suggestion | « Laissez-moi tenter ma chance ce soir ; si j'échoue, je vous laisse le champ libre. » | "We can both try, we'll see who gets picked." | 我们俩都试试呗，看最后谁被选中。 |
| **3.2.2** | suggestion | « …où est-ce que je vais mettre **mes affaires** ? » | "…where am I going to put **my clothes**?" | 那我的**衣服**要放哪儿？ |
| **6.1.1** | suggestion | « Pour restaurer la confiance, il faut des règles claires… » (121 car.) | "It is absolutely necessary to rebuild the people's confidence…" (89 car.) | 我们必须重建人民对政治阶层的信任。 |

Un dossier qui n'aurait lu que **FR | zh** aurait imputé ces trois écarts à la traduction
chinoise, et fait corriger le mauvais fichier. Ils relèvent d'une passe sur le bloc EN.

---

## 7. Fidélité élevée — à consigner aussi

Le dossier serait faux s'il ne portait que des défauts. Relevés notables :

- **4.3.1** — « Ergo sum » → **故我在** : le titre latin est rendu par la queue du raisonnement
  cartésien chinois (我思故我在), là où une translittération aurait été plate.
- **2.1.4** — le FR dit « polygamie », le zh dit **一妻多夫** (*polyandrie*) : le terme exact
  pour Blanche-Neige et les nains — resserrement, pas écart.
- **5.1.2** — le jeu de mot du FR (« schtroumpfement ») est **recréé** en 蓝精灵式地, et les noms
  officiels chinois sont employés (蓝爸爸, 健健蓝精灵).
- **5.2.1** — la citation d'Ézéchiel (Pulp Fiction) est rendue dans son **registre biblique**.
- **6.3.2** — l'EN omet le point final, le zh **l'ajoute** : le zh répare là où l'EN est fautif.

---

## 8. Verdict zh

- **Couverture : complète** (1336/1336), aucune cellule en français.
- **6 défauts ponctuels** + **1 scorie de lot** (4 cellules contiguës), tous nommés ci-dessus
  avec leur preuve.
- **3 écarts imputés au bloc EN**, retirés du passif zh.
- Le reste = **choix de rendu** à arbitrer en relecture native, listés en §5.
- **0 écriture** : ce dossier n'a modifié aucune cellule ; il ouvre la matière pour la
  relecture native, qui décidera des corrections.

---

## 9. Erratum — portée du défaut n° 6 (ajouté le 2026-10-04, même journée)

La campagne a poursuivi sur les **7 autres langues** avec le même instrument. La carte **1.2.3**
(contexte *et* enjeu) manque la ponctuation finale **en es, ar, fa et zh**, et la carte **2.1.8**
en **es, ar et zh** — mesuré sur les seules cellules où **FR et EN ponctuent tous deux**.

⇒ La ponctuation manquante n'est **pas un trait du zh** : c'est une **signature de lot**
partagée par 3 à 4 langues aux mêmes cellules. Le défaut n° 6 est donc **retiré du passif zh**
et versé à la matrice inter-langues. Par ailleurs `2.2.1`, listée « héritée de l'EN » dans la
passe zh, est confirmée comme telle : le bloc EN omet lui-même le point final.

Le présent dossier est corrigé sur place (document vivant, PR encore ouverte). Le corps de la
PR #1746, qui portait la version initiale, a reçu le même erratum en commentaire.

*po-2024*

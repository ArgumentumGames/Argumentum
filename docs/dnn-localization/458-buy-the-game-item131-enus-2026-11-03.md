# #458 — Fiche produit `ItemId 131` : pivot FR → en-US

**Date :** 2026-11-03 · **Lane :** po-2023 · **Base :** `a57a5ec7`
**Portée :** PRÉPARATION SEULE. Aucune écriture en base dans cette livraison.

---

## Objet

La fiche produit du lien « Buy the game » se rend **en français quelle que soit la
culture demandée**. La cause retenue au tour précédent — « aucune rangée `PRDLANG` de
culture en-US n'existe pour cet article » — est **RÉFUTÉE** par la mesure ci-dessous.
Le diagnostic corrigé, et le fait qu'il déplace la cible du geste, sont l'essentiel de
cette livraison.

Cette PR pose le **pivot** : le texte FR source et sa traduction en-US, dans un fichier
versionné, avant tout geste sur la base. Le geste en base vient **après** merge — et
après l'arbitrage demandé en fin de note.

---

## ⚠️ Le magasin réel : TROIS objets portent ce produit, pas un

Mesuré le 03/11/2026 en lecture seule. Le comptage du tour précédent
(`ItemId = 131 AND Lang = 'en-US'`) était **structurellement incapable** de voir les
rangées de langue : dans `NBrightBuy`, une rangée de langue porte **son propre
`ItemId`** et se rattache au produit par `ParentItemId`. Chercher l'en-US sous
`ItemId 131` ne pouvait rendre que 0.

| Magasin | Objet | Contenu en-US mesuré |
|---|---|---|
| `NBrightBuy` | `ItemId 130`, type `PRD` | — (la fiche produit elle-même) |
| `NBrightBuy` | `ItemId 131`, `PRDLANG`, `fr-FR` | 4 589 car., **2 images**, 3 modèles, description « En Rupture » |
| `NBrightBuy` | `ItemId 148`, `PRDLANG`, `en-US`, `ParentItemId 130` | 2 380 car., **1 image**, **texte français**, daté 2023 |
| `NBrightBuy` | `ItemId 149`, `PRDLANG`, `en-US`, `ParentItemId 130` | 2 415 car., même forme, **texte français**, + `modelid` sur le modèle 0 |
| `NBrightBuyLang` | `ParentItemId 130`, `fr-FR` | 11 563 car. — document complet (prix, stock, catégorie `140`) |
| `NBrightBuyLang` | `ParentItemId 130`, `en-US` | 9 389 car. — sous-arbre `/lang[32]/genxml[0]` **en français**, **38 nœuds au lieu de 63** |

Trois conséquences :

1. **La garde « 0 rangée en-US ⇒ 1 insertion » ne tient pas** : il existe **deux** rangées
   `PRDLANG` en-US et **une** rangée `NBrightBuyLang` en-US. Le geste est un **arbitrage
   sur de l'existant**, pas une insertion.
2. **Les deux rangées `PRDLANG` en-US sont des copies périmées d'un document français**
   (pas de « En Rupture », une seule image, un seul modèle, datées 2023). Les traduire
   reviendrait à figer une version ancienne du produit.
3. **Le sous-arbre `/lang/genxml[0]` de `NBrightBuyLang` fr-FR est identique, valeur pour
   valeur, au document `PRDLANG` 131** : **63 nœuds communs, 0 différent**. C'est pourquoi
   le pivot ci-joint est **valable pour les deux magasins** — il ne dépend pas du choix.

---

## ⚠️ Le vrai bloqueur n'est pas la donnée : c'est le lien

Même une traduction parfaite ne s'afficherait pas. Mesuré sur la préprod :

```
/en/Acheter-le-jeu/Detail/catid/140/eid/130/…   ->  301
   Location: /Acheter-le-jeu/Detail/catid/140/eid/130/…   (sans préfixe de culture)
   X-Redirect-Reason: Tab External Url Requested
```

L'onglet boutique est déclaré en **URL externe** : il **retire le préfixe de culture**,
et la fiche est alors rendue par le portail par défaut, en `fr-FR`.

Formes d'URL testées sur la fiche, toutes rendues en français :

| Forme | `En Rupture` | `La boite` (image 1) | `Les cartes` (image 2) |
| --- | --- | --- | --- |
| `/…/eid/130/…` | 1 | 4 | 3 |
| `?language=en-US` | 1 | 4 | 3 |
| `?lang=en-US` | 1 | 4 | 3 |
| `?Language=en-US` | 1 | 4 | 3 |
| `?language=en-US&tabid=0` | 1 | 4 | 3 |

Le jeu de marqueurs est **discriminant** : la rangée fr-FR est la seule à porter
« En Rupture » *et* la légende « La boite », et la rangée `NBrightBuyLang` en-US ne porte
ni l'une ni l'autre. La fiche rend donc bien le **fr-FR**, sur toutes les formes d'URL.

**Relevé du lien de menu, non modifié** (demande explicite) : les deux accueils
(`/` et `/en/`) pointent la **même** cible, sans préfixe de culture :

```
/Acheter-le-jeu/Detail/catid/140/eid/130/le-jeu-de-cartes-argumentum
```

C'est ce lien, et non la donnée, qui décide de la langue servie.

---

## Ce que porte la fiche — inventaire mesuré, pas supposé

Le document `XMLData` de la rangée porte **13 nœuds marqués `update="lang"`** :

| État | Nœuds | Suite |
|---|---|---|
| **Plein, traduit** | `txtproductname`, `txtsummary`, `txtimagedesc` | traduits |
| **Plein, NON traduit (décision)** | `txtmodelname` (`Argumentum V1`, `… lot de 10`, `… - sans envoi`) | voir ci-dessous |
| **Plein, drapeau** | `chkishiddenlang` = `False` | recopié tel quel par la base |
| **Vide** | `fimageurl`, `fimagepath`, `fimageref`, `extrafield`, `txtextra`, `txtseoname`, `txtseopagetitle`, `seodescription` | rien à traduire |

Deux champs **hors** du marquage `lang` portent aussi du texte affiché : `description` et
`edt/description` (l'éditeur riche). Tous deux sont traduits.

⚠️ **Les trois champs SEO existent et sont vides** (`<txtseoname/>`, `<txtseopagetitle/>`,
`<seodescription/>`). Un nœud auto-fermant n'est pas un champ absent : l'absence de valeur
est **mesurée**, elle n'est pas déduite d'un `grep` qui ne trouverait rien.

⚠️ **`txtmodelname` est délibérément laissé en français.** Ce sont des identifiants de
déclinaison (`Argumentum V1`, `… lot de 10`, `… - sans envoi`) rattachés au flux de
commande. Les traduire changerait des libellés de SKU, ce que la garde « aucune
modification d'autre article / de réglage de commande » interdit de fait. **Exclusion
nommée, à arbitrer par ai-01** si l'on veut un jour des variantes anglaises.

---

## Le piège des jumeaux — deux valeurs existent en double dans le document

Le document ne porte pas chaque valeur une fois. Mesuré :

| Champ | Nœuds porteurs | Forme |
|---|---|---|
| `description` | `textbox/description` **et** `edt/description` | jumeaux, valeur identique |
| légende image 1 | `textbox/txtimagedesc` **et** `imgs/genxml[0]/textbox/txtimagedesc` | jumeaux, valeur identique |
| légende image 2 | `imgs/genxml[1]/textbox/txtimagedesc` | seul |

Traduire un seul côté laisserait la fiche **à moitié anglaise** : la description
s'affiche depuis un nœud, l'éditeur lit l'autre. C'est la même famille que
`feedback-store-fix-without-pivot-reinjects` — corriger un objet et laisser son double.

Le pivot déclare donc `nodes` et `xml_paths` (séparés par ` ; `), et le générateur
**vérifie l'invariant des jumeaux sur le document source** avant d'écrire.

---

## Méthode : substitution chirurgicale, pas de re-sérialisation

⚠️ **Un aller-retour `ElementTree` n'est PAS neutre à l'octet.** Mesuré sur ce document :

```
original : 4589 octets | aller-retour : 4637
1er écart à 14
  orig : <genxml><files/><hidden><fimageurl update="lang"/>…
  rt   : <genxml><files /><hidden><fimageurl update="lang" />…
```

Écrire la rangée en-US par re-sérialisation réécrirait donc des artefacts de
sérialisation **hors** des six valeurs traduites — dans une colonne `xml` que la boutique
reparse. Le candidat est construit par **remplacement de texte**, valeur par valeur, avec
un compte d'occurrences exigé à l'avance :

```
productname 1 · summary 1 · description 2 · image-1-caption 2 · image-2-caption 1
```

## Preuves (quatre contrôles, rejouables)

1. **Structure identique** — mêmes éléments, même ordre, mêmes attributs, mêmes `tail` :
   aucune substitution n'a touché un attribut par accident.
2. **7 nœuds de texte changent, tous attendus** (les 5 champs + 2 jumeaux) — désignés par
   sélecteur sémantique, pas par index.
3. **Reconstruction inverse à l'octet** — on rejoue les substitutions à l'envers sur le
   candidat et on retrouve l'original exactement. C'est la preuve que la transformation
   est *exactement* l'ensemble déclaré, et rien d'autre.
4. **Contrôle inverse** — le candidat diffère bien de l'original (un « contrôle » qui
   rendrait l'objet inchangé ne mesurerait rien).

---

## Fidélité de la traduction

- **Aucune promesse ajoutée.** Le « En rupture » et l'annonce de la 2.0 sont repris tels quels.
- **Titre du jeu** : « The art of never being wrong » est le titre imprimé, mais
  `txtproductname` porte « Le jeu de cartes Argumentum » (nom du produit, pas le titre) →
  rendu « The Argumentum card game ».
- **Vocabulaire** : *argument fallacieux* → *fallacious argument* (terme du glossaire) ;
  *saynète d'improvisation* → *improvised skit* ; *paquet* → *deck*.
- **Liens** : la version EN pointe vers `fallacies_en.html`, pas `fallacies_fr.html`.
  C'est le seul changement de destination, et il est attendu pour une fiche anglaise.
- **Entités** : la source FR emploie des entités HTML littérales (`&agrave;`, `&nbsp;`) ;
  l'EN garde les guillemets typographiques en entités (`&ldquo;`/`&rdquo;`) pour que le
  rendu typographique soit identique dans les deux langues.

---

## Ce que cette PR ne fait PAS

- ❌ aucune écriture en base — ni sauvegarde, ni insertion, ni `UPDATE` ;
- ❌ aucun accès aux tables de commandes, de clients ou de comptes ;
- ❌ aucune modification de prix, de stock, de réglage de commande ou d'un autre article ;
- ❌ le lien de menu « Buy the game » n'est pas touché — il a été **relevé** (voir plus
  haut), pas corrigé.

## Suite — bloquée sur un arbitrage, pas sur du travail

La chaîne prévue au dispatch (`sauvegarde → dry-run → insertion d'1 rangée en-US →
DoD sur /en/`) **ne peut pas s'exécuter telle quelle** : la garde de compte est fausse
(3 rangées en-US existent) et le résultat serait invisible (le lien retire la culture).

**Trois questions pour ai-01 :**

1. **Quel magasin est la cible ?** `NBrightBuyLang` (parent 130, sous-arbre
   `/lang/genxml[0]` — c'est le seul à porter prix, stock et `selectcategory = 140`, la
   catégorie de l'URL) ou `NBrightBuy.PRDLANG` ? Le pivot est **neutre** : le texte FR est
   identique dans les deux (63 nœuds, 0 écart).
2. **Que faire des rangées existantes ?** Les deux `PRDLANG` en-US (148, 149) sont des
   copies françaises périmées de 2023. Les laisser en place expose à servir un jour une
   fiche anglaise **plus ancienne que la française**. Les supprimer est destructif : je ne
   le fais pas sans GO nommé.
3. **Le lien est-il dans le périmètre de quelqu'un ?** Tant que l'onglet boutique reste en
   « External Url » et que le menu pointe la route sans culture, **toute** traduction est
   sans effet observable. Ce n'est pas un défaut de donnée.

En attendant l'arbitrage, cette PR ne contient **aucune écriture en base**. Si la réponse
est « `NBrightBuyLang` », le pivot se rejoue tel quel sous `/lang/genxml[0]/` — le travail
de traduction et les preuves ne sont pas à refaire.

*po-2023*

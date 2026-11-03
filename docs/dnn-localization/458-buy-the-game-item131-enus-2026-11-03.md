# #458 — Fiche produit `ItemId 131` : pivot FR → en-US

**Date :** 2026-11-03 · **Lane :** po-2023 · **Base :** `a57a5ec7`
**Portée :** PRÉPARATION SEULE. Aucune écriture en base dans cette livraison.

---

## Objet

La page `/en/` sert la fiche produit 131 en français. Cause mesurée au tour précédent
(dossier `#458 c.6608611350`) : il n'existe **aucune rangée `PRDLANG` de culture en-US**
pour cet article ; DNN retombe donc sur le français.

Cette PR pose le **pivot** : le texte FR source et sa traduction en-US, dans un fichier
versionné, avant tout geste sur la base. Le geste en base vient **après** merge.

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

## Preuves (`.tmp/buy131/verify_candidate.py`)

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
- ❌ le lien de menu « Buy the game » n'est pas touché (il sera **relevé**, pas corrigé).

## Suite (hors PR)

1. merge → **sauvegarde JSON** des seules rangées produit et `PRDLANG` de `ItemId 131` ;
2. **dry-run** avec garde de compte (0 rangée en-US existante ⇒ 1 insertion attendue) ;
3. insertion en **transaction**, puis recyclage par `touch` de `web.config` (jamais `iisreset`) ;
4. **DoD servie sur `/en/`**, sans geste entre les mesures : marqueurs EN > FR (pied de
   page exclu) ; **relever la cible du lien de menu sans la modifier** ;
5. puis les 6 autres cultures en une PR.

*po-2023*

# #458 grain 9-bis lot 6 — `SharedResources` fa-IR, troisième pack complet de la famille AGR cœur

> Le lot 6 ferme au passage **un trou de garde** que la contre-revue web2 a mesuré sur
> #2079 : le compte exact ne disait rien de l'**appartenance** de l'ensemble manquant.
> §5 détaille le trou, la fermeture et son **contrôle inverse**.

## 1. Ce qui est livré

`DNNPlatform/App_GlobalResources/SharedResources.fa-IR.resx` passe de **3 clés** (fichier
partiel du Tier 1) à **610** — le chrome de l'interface DNN en persan : boutons, messages
de validation, gabarits de courriel d'authentification, libellés d'administration, messages
de l'installateur.

Le dictionnaire `docs/dnn-localization/458-item9bis-fa-IR.translations.json` gagne une
cinquième section `files.SharedResources` (610 entrées).

## 2. Deux comptes différents, tous les deux vrais

Le neutre porte **617** clés, le fichier fa-IR en portait 3 ⇒ **614 manquantes**. Mais
**7 de ces 614 ont une valeur neutre vide** (`File.Text` et six
`DefaultWorkflowState*.OnDiscard*/OnCompleteMessage*` que le produit ne remplit jamais). Le
constructeur refuse de les traduire — les remplir serait inventer du texte.

⇒ Le dénominateur réel est **610**, et c'est ce que la garde épingle. C'est le **même**
dénominateur que les lots 4 (zh-CN) et 5 (ar-SA) : le neutre est le même fichier.

## 3. Méthode — le `.resx` n'est pas recréé

Inchangée depuis les lots 1-5 : on part du fichier **neutre**, on ne remplace que le
contenu des `<value>` des clés traduites. En-tête, schéma `xsd`, `resheader`, ordre des
clés, BOM UTF-8 et fins de ligne LF sont préservés.

**Ordre des clés mesuré** : le pack livré lit **1 952 lignes**, le neutre en compte
**1 973** — écart **21 = 7 blocs × 3 lignes**, exactement les 7 clés retirées. L'ordre
livré est l'ordre du neutre.

### 3.1 Le dictionnaire : contenu inchangé, fins de ligne normalisées — **déclaré**

Le dictionnaire fa-IR était en **CRLF** (368 CRLF) ; il est livré en **LF** (0 CRLF), comme
les dictionnaires zh-CN et ar-SA. Le diff GitHub affichera donc l'ensemble du fichier comme
réécrit : **les quatre sections préexistantes sont identiques au contenu près**
(`FileUpload` 4, `WebControls` 50, `Exceptions` 132, `GlobalResources` 170 — contrôlé par
assertion avant écriture), seule la section `SharedResources` est ajoutée et seule la fin
de ligne change.

C'est la réserve levée par la relecture NanoClaw sur #2079 : la bascule CRLF→LF du
dictionnaire zh-CN n'y était pas déclarée. Ici elle l'est.

### 3.2 Reproductibilité et idempotence

- **Reproductibilité** : le constructeur rejoué **depuis le dictionnaire committé**
  réécrit le `.resx` **byte-identique** (`sha256` avant/après).
- **Idempotence 4/4** : rejoué sur les quatre packs fa-IR **déjà mergés**, le constructeur
  les restitue **byte-identiques** (`sha256`). Le dictionnaire est donc bien la source de
  vérité de ces quatre packs aussi — pas seulement du nouveau.
- ⚠️ Le rapport du constructeur annonce une taille **sans le BOM** (`produced.encode`)
  alors que le fichier sur disque le porte : l'écart de 3 octets sur les quatre packs
  existants n'est pas une divergence de contenu.

## 4. Contrôles — instrument indépendant du constructeur

Parsing XML du fichier produit, pas le rapport du script :

| Contrôle | Résultat |
|---|---|
| Clés livrées | **610** |
| Manquantes vs neutre | **7** — exactement les 7 à neutre vide |
| Clés inventées | **0** |
| Valeurs vides | **0** |
| Multiset des placeholders `{0}`/`{1}` | **0 écart** |
| Identité des jetons DNN (`[Custom:…]`, `[Portal:…]`) | **0 écart** |
| BOM / fins de ligne | présent / **LF seul**, 0 CRLF |
| Ordre des clés | = ordre du neutre (1 952 lignes, écart 21 = 7 × 3) |
| Valeurs Tier 1 déjà arbitrées (`Password.Text`, `cmdCancel.Text`, `Home.Text`) | **identiques** |
| Valeurs (hors verbatim déclaré) portant l'écriture persane | **602 / 602** |
| Clé à neutre vide livrée | **0** |

Huit valeurs restent identiques à l'anglais et sont **déclarées** nominativement :
`True`/`False` (littéraux booléens **parsés par nom** — les traduire changerait le
comportement, pas la langue), `APP ID` (marque produit), `A-Z` (glyphe de tri),
`B/KB/MB/GB` (unités). La cardinalité de cette liste est contrôlée (`= 8`), pour qu'une
liste blanche qui grossit en silence se voie.

### Ce que ces contrôles n'établissent pas

- **Pas de relecture par un locuteur natif.** La traduction est produite par la lane ; les
  contrôles sont mécaniques (complétude, structure, jetons). Une relecture éditoriale fa
  reste ouverte, comme pour les packs zh-CN et ar-SA.
- **Pas de validation visuelle** : elle reste côté ai-01.
- **Aucune valeur ne porte de caractère directionnel** (LRM/RLM) : la direction vient du
  thème (RTL déjà en ligne), pas des chaînes.

## 5. Le trou de garde que ce lot ferme

La contre-revue web2 sur #2079 (reproduite intégralement, chiffres compris) pose le bon
point : `PackCoversTheDeclaredKeyCount` **compte**, `EveryDeliveredKeyIsSaneAgainstTheNeutral`
**itère sur les clés livrées**, et **personne ne disait « les clés absentes sont exactement
celles à neutre vide »**.

Conséquence, mesurée : un **échange tenant le compte** passait les deux gardes. Retirer
une vraie clé et livrer `File.Text` avec un texte inventé laisse 610 clés — et comme la
valeur neutre de `File.Text` est **vide**, ni « copie de l'anglais » ni « valeur vide »
ne pouvaient le prendre. C'est vrai du fichier livré ; **ce n'était pas garanti par la
CI**, et le lot fa-IR serait arrivé sous cette garantie absente.

### 5.1 Ce qui change dans la garde

1. **L'ensemble manquant est déclaré, clé par clé, par fichier**
   (`AllowedMissingByStem`) : `GlobalResources` 10 (2 à neutre vide + 8 blocs
   volontairement non traduits — HTML d'erreur, listes de mots, licence MIT),
   `SharedResources` 7 (les 7 à neutre vide), **rien** pour `FileUpload`, `WebControls`,
   `Exceptions`. Un nouveau test, `TheMissingKeysAreExactlyTheDeclaredOnes`, épingle
   l'ensemble livré (sous-ensemble du neutre) **et** l'ensemble manquant (égal à la
   déclaration).
2. **Une clé dont le neutre est vide ne doit jamais être livrée** : le constructeur refuse
   de l'inventer, la garde refuse qu'un autre chemin la livre.
3. **§4 de la même contre-revue** : les `continue` de `VerbatimByDesign` et `LegalKeys`
   court-circuitaient aussi les contrôles de placeholders et de jetons. Ces deux contrôles
   passent désormais **avant** les `continue` — la garantie structurelle vaut pour **toute**
   clé livrée, plus seulement pour celles qui échappent à la liste blanche. Mesuré :
   **0 écart sur 1 726 clés livrées**, y compris les 9 concernées.

### 5.2 Contrôle inverse — le test peut rougir

Sans lui, le nouveau test serait **vert à vide**. L'échange décrit par web2 a été
appliqué au pack ar-SA (retrait d'une vraie clé, livraison de `File.Text` avec un texte
inventé, compte maintenu à 610) : **les deux règles rougissent indépendamment** —
l'ensemble manquant contient alors `Accept.Text`, et `File.Text` est livrée à neutre vide.
Le fichier a ensuite été restauré **byte-identique** (`sha256`).

### 5.3 Ce que cela change pour `CompletedPacks`

L'exemption `CompletedPacks` de `ChromeTier1LocalizationContractTests` (égalité stricte
des clés impossible sur un pack complet) n'est **plus non bornée** : ce que le pack ajoute
au-delà du Tier 1 est borné par la garde AGR. Le renvoi est écrit dans le fichier.

## 6. Pourquoi une branche empilée

Ce lot touche **les mêmes lignes des deux fichiers de garde** que les lots 4 et 5 : deux
branches parties de `master` produiraient un conflit garanti. La branche est donc empilée
sur `feat/458-sharedresources-ar`, et la PR cible cette branche.

**Ordre de merge :** #2079 (zh-CN) → #2080 (ar-SA) → cette PR. Après le squash-merge de
#2079, GitHub recible automatiquement celle-ci ; le diff reste exact.

## 7. Tests

- Suite filtrée gardes DNN : **94 / 0 / 0**.
- Suite complète : cf. rapport du tick.
- Contrôle inverse de §5.2 : appliqué, mesuré, restauré.

## 8. Ce que cette PR ne fait pas

- **Aucun geste préprod** : un `.resx` est un fichier du dépôt ; sa compilation par IIS
  demande un recyclage du pool applicatif, geste **distinct**. Un pack mergé n'est pas un
  pack servi.
- **`List_Country`** non traité (aucune version pour ces cultures, 256 clés — grain
  différent).
- **Grain module-level** non traité (96 fichiers, 2 355 clés par langue) : reste en file.
- **Pas de validation visuelle** : elle reste côté ai-01.
- **Qualité rédactionnelle non établie** : voir §4.

## 9. Suite

Tête de file du pool #458 après ce lot : `List_Country` (256 clés, aucun fichier pour
ar/fa/zh), puis le grain module-level, la caractérisation du flap 401, et le test
d'import dans une application de test.

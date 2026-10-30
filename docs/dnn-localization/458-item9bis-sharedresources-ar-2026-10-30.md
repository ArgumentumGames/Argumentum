# #458 grain 9-bis lot 5 — `SharedResources` ar-SA, deuxième pack complet de la famille AGR cœur

**Date :** 2026-10-30 · **Lane :** po-2023 · **Culture :** `ar-SA` seule (fa-IR non touché)
**Fichier livré :** `DNNPlatform/App_GlobalResources/SharedResources.ar-SA.resx` — **3 clés → 610**
**Branche :** `feat/458-sharedresources-ar`, empilée sur `feat/458-sharedresources-zh` (voir §6)

**Errata 2026-10-30** (relecture NanoClaw) : deux affirmations corrigées — le **BOM UTF-8** est
**ré-émis à l'écriture**, pas « préservé » (§3) ; la part de valeurs portant de l'arabe est
**602/602**, pas 610/610 (§4) — les 8 verbatim déclarés ne portent pas d'arabe par nature.

---

## 1. Ce qui est livré

`SharedResources` est le chrome de l'interface DNN : boutons, messages de validation,
gabarits de courriel d'authentification, libellés d'administration, messages de
l'installateur. Le fichier `ar-SA` n'en portait que **3 clés** — celles du Tier 1
arbitrées au lot 2 (`Password.Text`, `cmdCancel.Text`, `Home.Text`).

Il en porte désormais **610**, et ces trois valeurs arbitrées sont **inchangées** au
caractère près (contrôle ci-dessous, pas une affirmation).

## 2. Deux comptes différents, tous les deux vrais

Le neutre `SharedResources.resx` porte **617** clés ; le fichier `ar-SA` en portait 3
⇒ **614 manquantes**. Mais **7 de ces 614 ont une valeur neutre VIDE** :

```text
File.Text
DefaultWorkflowState1.OnDiscardMessageBody
DefaultWorkflowState1.OnDiscardMessageSubject
DefaultWorkflowState3.OnCompleteMessageBody
DefaultWorkflowState3.OnCompleteMessageSubject
DefaultWorkflowState3.OnDiscardMessageBody
DefaultWorkflowState3.OnDiscardMessageSubject
```

Le constructeur **refuse** de les traduire (`a un neutre vide hors coquille`) : les
remplir serait inventer du texte que le produit ne remplit jamais. Le dénominateur
réel est donc **610**, et c'est ce que la garde épingle.

⇒ *« 614 manquantes » et « 610 livrées » sont tous les deux vrais.* Citer l'un sans
l'autre fait lire 4 clés perdues.

## 3. Méthode — le `.resx` n'est pas recréé

Inchangée depuis les lots 1-4 : on part du fichier **neutre**, on ne remplace que le
contenu des `<value>` des clés traduites. En-tête, schéma `xsd`, `resheader`, ordre des
clés et fins de ligne LF sont préservés. Une clé non traduite est **absente**
du fichier produit ⇒ DNN retombe sur l'anglais, clé par clé.

Le **BOM UTF-8**, lui, est **ré-émis à l'écriture**, pas préservé : le constructeur lit le
neutre en `utf-8-sig` (le BOM est consommé à la lecture, `458-item9bis-langpack-build.py:133`)
et écrit en `utf-8-sig` (le BOM est ré-ajouté, `:181`). Il est donc présent dans la sortie
**par convention du script** — la formulation exacte est « ajouté à l'écriture », pas
« préservé ».

Le dictionnaire `docs/dnn-localization/458-item9bis-ar-SA.translations.json` gagne une
section `files.SharedResources` (610 entrées) et reste **la** source de vérité du pack.

**Reproductibilité vérifiée** — le contrôle qui manque partout : le constructeur rejoué
**depuis le dictionnaire committé** réécrit les **cinq** `.resx` ar-SA et les rend
**byte-identiques** (`sha256` avant/après, **5/5** `OK`). Un pack qu'on ne peut pas
reconstruire depuis son dictionnaire n'est pas relisible, c'est un artefact.

## 4. Contrôles — instrument indépendant du constructeur

Parsing XML du fichier produit (`verify-ar.py`), **pas** le rapport du script :

| Contrôle | Résultat |
|---|---|
| Clés livrées | **610** |
| Manquantes vs neutre | **7** — exactement les 7 à neutre vide |
| Clés inventées (hors neutre) | **0** |
| Valeurs vides | **0** |
| Multiset des placeholders `{0}`/`{1}` | **0 écart** |
| Identité des jetons DNN (`[Custom:…]`, `[Portal:…]`) | **0 écart** |
| BOM / fins de ligne | présent / **LF seul**, 0 CRLF |
| Valeurs Tier 1 déjà arbitrées | **identiques** |
| Valeurs (hors verbatim déclaré) portant de l'arabe | **602/602** — les 8 verbatim déclarés ci-dessous ne portent pas d'arabe, par nature |

### Verbatim par nature — 8 valeurs, déclarées

`True` / `False` (littéraux booléens **parsés par nom** dans les listes : les traduire
changerait le comportement, pas la langue), `APP ID` (marque produit), `A-Z` (glyphe de
tri), `B` / `KB` / `MB` / `GB` (unités d'octets). Ce sont des **jetons**, pas de la prose.

⚠️ L'instrument a d'abord rendu **FAIL** sur ces 8 valeurs, parce qu'il exigeait de
l'arabe sur *toutes* les lignes. C'est le contrôle qui était faux, pas le livrable : il
a été corrigé en y ajoutant la liste blanche nommée — et un contrôle de cardinalité
(`= 8`) pour qu'une liste blanche qui grossit en silence se voie.

### Ce que ces contrôles n'établissent pas

Ils sont **mécaniques**. Ils prouvent la complétude, la santé structurelle et la
préservation des jetons ; ils ne prouvent **pas** la qualité rédactionnelle de l'arabe.
La traduction a été produite par la lane, pas revue par un locuteur natif : une
relecture éditoriale ar reste ouverte, au même titre que pour le pack zh-CN.

Point de vigilance RTL à cette même relecture : les valeurs n'ont reçu **aucun caractère
directionnel** (LRM/RLM). La direction est portée par le thème (RTL déjà en ligne,
#1932), pas par les chaînes ; une phrase mêlant latin et arabe peut se réordonner à
l'affichage.

## 5. Évolution d'une garde écrite par une autre lane

`ChromeTier1LocalizationContractTests.ScopeIsThirteenKeysAndPartialFilesContainNoExtraKeys`
épingle les **13 clés Tier 1** et exigeait, sur les trois cultures, que l'ensemble des
clés du fichier soit **exactement** ces 3 clés — un **verrou**, pas un contrôle de qualité.

- **Ne change pas** : les 13 clés Tier 1 gardent leur **valeur revue**, assertée cellule
  par cellule (39 cellules).
- **Change** : l'égalité exacte ne vaut plus que pour les fichiers **encore partiels**.
  La liste nommée `CompletedPacks` déclare l'exception ; sur ces couples, la garde exige
  que les clés Tier 1 soient **présentes** et que le fichier en porte **davantage**.
- **Se déplace** : la santé des clés ajoutées appartient à
  `DnnAgrCoreResxIntegrityGuardTests`, qui gagne `("ar-SA", "SharedResources", 610)`.

Le lot 5 fait un pas de plus sur la **déclaration des jetons** : les 8 jetons du pack
`SharedResources` sont écrits **une seule fois** (`SharedResourcesPackTokens`) et
concaténés par culture. Recopiés par culture, ils dériveraient en silence au lot
suivant (fa-IR) sans que rien ne le signale.

## 6. Pourquoi une branche empilée

Les deux fichiers de garde sont modifiés par les lots 4 **et** 5, aux **mêmes lignes**.
Deux branches parties de `master` produiraient un conflit garanti.

`feat/458-sharedresources-ar` part donc de `feat/458-sharedresources-zh`
(`e1b0c371`), et la PR cible **cette branche**. Le diff de la PR ne montre alors que le
travail ar ; quand la PR zh est mergée (squash), GitHub recible automatiquement celle-ci
vers `master` et le diff reste exact, le contenu zh étant déjà dans `master`.

Aucune PR n'est empilée au sens du dispatch (« une culture à la fois, jamais deux dans
la même PR ») : il y a bien **une culture par PR**.

## 7. Tests

| Périmètre | Résultat |
|---|---|
| Les deux classes de garde touchées | **77 / 77** |
| Suite complète | voir le commentaire de preuve de la PR |

## 8. Ce que cette PR ne fait pas

- **Aucun geste préprod** : un `.resx` est un fichier du dépôt ; sa compilation par IIS
  demande un recyclage du pool applicatif, geste **distinct** — et un pack mergé n'est
  pas un pack servi.
- **`fa-IR` non touché** (séquentiel).
- **`List_Country`** non traité (aucune version de ce fichier pour ces cultures, 256 clés
  — grain différent).
- **Pas de validation visuelle** : elle reste côté ai-01.

## 9. Suite

`SharedResources` **fa-IR** (même dénominateur, 610) · puis la tête de file du pool
(`List_Country`, module-level 96 fichiers / 2 355 clés par langue).

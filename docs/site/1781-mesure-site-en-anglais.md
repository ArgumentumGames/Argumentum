# Mesure — chantier « site en anglais » (#1781)

**Objet** : chiffrer ce que coûterait une version anglaise du site, pour que Jesse tranche.
**Demandeur** : Adeline (commentaire #1781 c.6043917035, 07/10/2026).
**Nature de ce document** : **mesure seule, aucune écriture** — ni traduction, ni activation de langue.
**Périmètre mesuré** : site de préprod `dnn.argumentum.myia.io` (IIS, racine physique `D:\Dev\Argumentum\DNNPlatform`, vérifiée dans `applicationHost.config`).
**Date de mesure** : 08/10/2026, ~05:00 locale.
**Instruments** : base DNN (`ArgumentumGames`, lecture seule), sondes HTTP servies (lecture seule), fichiers déployés sur disque, corpus CSV lu sur `origin/master`.

---

## 1. Ce qui est demandé (verbatim)

> « can you add a "Language" button displayed in the top right corner next to Amis that if switched from default "Français" to "English" gives access to the whole content of the website in English. can you translate each page in English so on the new website it can be accessed if the Language button was switched to English? so I can read it and check the translation (of course keep all the pictures and hyperlinks and so on). »

Traduction de l'enjeu : un **bouton de langue** en haut à droite (à côté d'« Amis ») basculant tout le site en anglais, **contenu de chaque page traduit**, images et liens conservés. Adeline veut pouvoir **relire la traduction**.

---

## 2. Périmètre : combien de pages, combien de texte

Le portail porte **29 pages** (`Tabs`, `PortalID=1`, `IsDeleted=0`), dont **22 marquées visibles**. Une sonde servie sur les 22 a donné :

| Catégorie | Pages | Mots visibles |
|---|---:|---:|
| **Contenu éditorial public** | **9** | **12 894** |
| Compte boutique (derrière « Mon Compte ») | 4 | 1 199 |
| Écrans d'identification (chrome de connexion, 81 mots chacun) | 6 | 486 |
| Injoignables par leur chemin (404 anonyme) | 3 | — |
| **Total servi (HTTP 200)** | **19 / 22** | **14 579** |

Détail page par page (mots = texte visible, balises et scripts retirés) :

| TabID | Page | Chemin | HTTP | Mots |
|---:|---|---|---:|---:|
| 138 | Argumentum (accueil) | `/Argumentum` | 200 | 1 490 |
| 148 | Actus | `/Actus` | 200 | 561 |
| 152 | Règles | `/Règles` | 200 | 709 |
| 169 | Argumentation | `/Argumentation` | 200 | 739 |
| 170 | Carte mentale fallacieuse | `/Argumentation/Cartementalefallacieuse` | 302 → 200 | **3 623** |
| 171 | Ontologie fallacieuse | `/Argumentation/Ontologiefallacieuse` | 302 → 200 | **4 480** |
| 149 | Acheter le jeu | `/Acheterlejeu` | 200 | 321 |
| 150 | Téléchargements | `/Téléchargements` | 200 | 575 |
| 151 | Amis | `/Amis` | 200 | 396 |
| 153 | Tests | `/Tests` | 200 | 81 (identification) |
| 154 | QRCodes | `/Tests/QRCodes` | 200 | 81 (identification) |
| 158 | Mon Compte | `/Acheterlejeu/MonCompte` | 200 | 81 (identification) |
| 159 | Mes Commandes | `…/MonCompte/MesCommandes` | 200 | 294 |
| 160 | Mon Carnet d'adresses | `…/MonCompte/MonCarnetdadresses` | 200 | 296 |
| 161 | Ma Liste de courses | `…/MonCompte/MaListedecourses` | 200 | 298 |
| 162 | Mes Identifiants | `…/MonCompte/MesIdentifiants` | 200 | 311 |
| 163 | Mon Panier | `/Acheterlejeu/MonPanier` | 200 | 81 (identification) |
| 166 | Open Store Back Office | `/Acheterlejeu/OpenStoreBackOffice` | 200 | 81 (identification) |
| 146 | File Management | `/Admin/FileManagement` | 200 | 81 (identification) |
| 142/143/144 | My Profile / Friends / Messages | `/ActivityFeed/…` | **404** | — |

**Lecture** : les deux pages les plus lourdes du site ne sont pas des pages DNN rédigées — ce sont des **fichiers HTML générés**, atteints par redirection 302 (§4). Le texte *rédigé* à la main dans DNN est donc de l'ordre de **10 000 mots**, pas 15 000.

---

## 3. Où vit le texte (réservoirs, mesurés)

| Réservoir | Volume mesuré | Ce qu'il porte |
|---|---:|---|
| **Apps 2sxc (EAV `TsDynData*`)** | ~**800 000 caractères** sur 40+ apps ; plus grosses : app 29 = 291 551, app 60 = 254 893, app 1 = 36 308, app 33 = 33 087, app 50 = 31 543 | le **corpus** (fallacies, vertus, règles, glossaire) rendu par les vues Razor des pages |
| **Fichiers HTML statiques générés** | `fallacies_*.html` 2,5→5,2 Mo · `virtues_*.html` 0,5→1,2 Mo | les **2 pages cartes mentales** (contenu SVG inclus inline) |
| `ontologie_fr.html` | 447 Ko | la **page ontologie** |
| **Text/HTML classique (`HtmlText`)** | **8 lignes, 45 050 car.** — dont **une seule** rattachée à un module vivant (page 404, 293 car.) | quasi rien : les autres lignes sont des restes de modules supprimés |
| **Module `Content` DNN (`ContentItems`)** | 451 items, **5 042 car.** | métadonnées de page, pas de corps de texte |
| **Chrome des modules DNN / tiers** | non textuel en base | `Member Directory`, `Console`, `Journal`, `Message Center`, `Search Results`, `ResourceManager`, `ViewProfile`, `User Accounts` (cœur DNN) + **`OS_*` OpenStore** (12 instances, boutique fermée) |
| **Ressources du site (`App_GlobalResources/GlobalResources.fr-FR.resx`)** | FR uniquement | libellés de site (`/terms`, `/privacy`, messages système) |

**Conséquence de méthode** : le gros du volume apparent (≈ 800 000 caractères en 2sxc) n'est **pas** de la prose de site — c'est le **corpus**, qui est déjà traduit dans les CSV en 8 langues (§4). Une traduction « éditoriale » du site ne porte donc pas sur ces 800 000 caractères.

---

## 4. Ce qui existe déjà en anglais

1. **Deux pages sur trois sont déjà déployées en 8 langues.** Sonde HTTP en lecture seule sur les fichiers déployés :

   | Famille | fr | en | ru | pt | es | ar | fa | zh |
   |---|---|---|---|---|---|---|---|---|
   | `fallacies_<lang>.html` | 200 | **200** | 200 | 200 | 200 | 200 | 200 | 200 |
   | `virtues_<lang>.html` | 200 | **200** | 200 | 200 | 200 | 200 | 200 | 200 |

   **16/16 déjà servies.** Un bouton de langue peut pointer dessus **aujourd'hui**, sans traduction ni publication.

2. **La page ontologie est monolingue** : `/ontologie_en.html` → **404** (seul `ontologie_fr.html` existe). C'est la seule des trois pages lourdes qui manque en anglais.

3. **La dimension 2sxc `en-US` existe déjà et est active** (`TsDynDataDimension` id 3, zone 2, `ExternalKey = en-US`, à côté de `fr-FR` id 4). Elle n'est peuplée que pour **2 apps** :

   | App | Valeurs en `en-US` | Caractères EN | Valeurs en `fr-FR` | Caractères FR |
   |---|---:|---:|---:|---:|
   | 29 (zone 2) | **3 046** | **85 077** | 2 027 | 205 625 |
   | 2 (zone 2) | 108 | 1 300 | — | — |
   | **60 (zone 3 — l'app montée sur la page Règles)** | **0** | **0** | — | — |

   Autrement dit : **une traduction anglaise partielle de l'app explorateur existe déjà en zone 2** (85 077 caractères, soit ~13 000 mots), tandis que **la copie montée sur le portail vivant (zone 3) n'a aucune valeur dimensionnée**. Le modèle de données est donc déjà en place ; c'est le remplissage qui manque.

4. **Le corpus est multilingue dans les CSV** (source de référence, lue sur `origin/master`) : sur `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, 1 408 rangées × 104 colonnes, les colonnes `_en` sont remplies à **67 %** (5 651 / 8 448 cellules sur 6 colonnes) — contre **87 %** pour `_fr` et pour les 6 autres langues. L'anglais est donc la langue la moins complète du corpus, mais elle existe déjà en grande partie.

5. **Les modules du cœur DNN** (`Member Directory`, `Console`, `Journal`, `Search Results`, `ResourceManager`, `ViewProfile`, `User Accounts`) embarquent leurs ressources `en-US` en standard : leur libellé d'interface ne coûte rien à traduire.

---

## 5. Mécanismes possibles et coût

| # | Mécanisme | Ce qu'il couvre | Coût estimé | Risque |
|---|---|---|---|---|
| **1** | **Bouton de langue vers les pages statiques déjà déployées** | les **2 pages cartes mentales** (fallacies + virtues), en 8 langues | **Quasi nul** — un lien, zéro traduction, zéro publication | aucun |
| **2** | **Content Localization natif de DNN** (activer `en-US` sur le portail) | **toutes** les pages DNN : chrome, menus, libellés du cœur DNN, contenu des modules localisables | **Moyen** — l'infrastructure est déjà là (`Tabs.CultureCode`, `DefaultLanguageGuid`, `LocalizedVersionGuid`, `HasBeenPublished` ; dimension 2sxc `en-US` active) | moyen : bascule de langue du portail = geste d'administration, à faire en préprod d'abord |
| **3** | **Remplir la dimension 2sxc `en-US` des apps du portail vivant** (zone 3) | le **corpus rendu par les vues** (app 60 sur Règles, etc.) | **Moyen** — le patron existe (app 29 en zone 2, 85 077 car. déjà en EN) ; le contenu peut être **dérivé du CSV** (déjà traduit) plutôt que retraduit | faible |
| **4** | **Générer l'ontologie en anglais** (`ontologie_en.html`) | la **3ᵉ page lourde** | **Faible** — la chaîne de génération produit déjà 8 langues pour les cartes mentales ; il manque la variante EN de l'ontologie | faible |
| **5** | **Pages dédiées par langue** (dupliquer l'arbre de 22 pages) | tout | **Élevé** — duplication et double maintenance de chaque page | élevé — à éviter |

**Ordre de grandeur du travail de traduction à proprement parler** : ≈ **10 000 mots** de texte rédigé (les 9 pages éditoriales publiques), plus le remplissage de la dimension `en-US` des apps du portail, largement **dérivable du corpus CSV déjà traduit**. Les deux pages les plus visibles (cartes mentales, 8 100 mots à elles deux) sont **déjà traduites et déjà en ligne**.

---

## 6. Défauts mesurés au passage (indépendants du chantier anglais)

1. **Les 16 gabarits de cartes mentales déclarent `lang="en"` et le titre « Taxonomy Mind Map » — y compris les fichiers français, russes, arabes…** Mesuré sur les 16 fichiers déployés. Seul `ontologie_fr.html` déclare correctement `lang="fr"` avec un titre français. Conséquence : la page française se présente comme anglaise (accessibilité, moteurs de recherche, dictionnaire du navigateur).
2. **`ontologie_en.html` absent** (404) alors que les deux autres pages lourdes sont en 8 langues.
3. **Deux pages publiques ne sont pas atteignables par leur chemin DNN** (`/ActivityFeed/MyProfile`, `…/Friends`, `…/Messages` → 404) : leur page parente `Activity Feed` (TabID 139) est marquée non visible.

---

## 7. Bornes d'instrument (ce que cette mesure ne dit pas)

- **Mots ≠ caractères.** Les volumes « caractères » viennent de la base ; les volumes « mots » viennent du **texte servi**, balises retirées. Les deux ne se comparent pas directement.
- Le comptage de mots **inclut le chrome servi** (menus, pied de page, titres de modules). Il exclut les images et le texte contenu dans les SVG des cartes mentales (le comptage les voit comme des nœuds de texte, pas comme des mots de phrase).
- Les 6 écrans d'identification comptent 81 mots de chrome : ce ne sont **pas** des pages de contenu public.
- Les volumes 2sxc sont **par app**, pas par page : une même app peut servir plusieurs pages, et une page peut monter plusieurs apps.
- La couverture `_en` du corpus est mesurée **par cellule remplie**, pas par qualité de traduction.
- **Non mesuré** : le volume de texte des modules `OS_*` (OpenStore, boutique fermée) et des modules sociaux du cœur DNN, qui vivent dans des ressources d'interface et non en base.

---

## 8. Recommandation

1. **Répondre à Adeline que la moitié de sa demande est déjà satisfaite sans travail** : les deux pages cartes mentales existent en anglais et sont en ligne ; il ne manque qu'un **bouton de langue** pour y accéder (mécanisme 1, coût quasi nul).
2. **Traiter la page ontologie en anglais** (mécanisme 4) — c'est la seule des trois pages lourdes qui manque, et la chaîne de génération sait déjà produire 8 langues.
3. **Chiffrer le reste via le mécanisme 2** (Content Localization natif) plutôt que par une duplication de pages : l'infrastructure DNN est déjà en place, et le corpus est déjà multilingue dans les CSV.
4. **Corriger le `lang="en"` des 16 gabarits** (défaut §6.1) — indépendant du chantier anglais, mais c'est la page française qui se déclare anglaise aujourd'hui.

**Aucune de ces quatre actions n'est engagée** : le chantier attend la décision de l'owner (calendrier — la question lui a été posée) et le gel de publication reste en vigueur.

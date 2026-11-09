# #2182 / #2181 — 7 articles d'Actus et 7 liens de carte mentale (fenêtre du 09/11/2026)

Fenêtre préprod 10 h – 17 h, annoncée au dashboard avant tout geste. Aucune écriture en
production. Aucune modification de gabarit Razor ni de requête 2sxc (consigne ai-01 du
09/11 06:15Z).

## 1. Ce qui a été écrit

### 1.1 Geste principal (issue #2182)

Sept entités de contenu **News** (type 325, app 52) créées, **publiées et visibles**
(décision owner du 08/11, qui lève la garde #2132 pour ces sept items et eux seuls) :

| Rang | EntityId | Date | Catégorie | Titre |
|---:|---:|---|---|---|
| 1 | 12011 | 2026-11-03 | Évolution | Argumentum : du jeu de cartes au dépôt reproductible |
| 2 | 12012 | 2026-11-04 | Évolution | Chaque sophisme à sa place : l'ontologie d'Argumentum et le format AIF |
| 3 | 12013 | 2026-11-05 | Évolution | La descente guidée dans l'arbre : le plugin Argumentum du projet Epita |
| 4 | 12014 | 2026-11-06 | Évolution | Des carnets de cours lisent le jeu : Argumentum dans CoursIA |
| 5 | 12015 | 2026-11-07 | Évolution | Sophismes × scénarios : le jeu comme jeu de données |
| 6 | 12016 | 2026-11-08 | Évolution | La dette d'irréversibilité du discours : une prédiction posée sur le corpus d'Argumentum |
| 7 | 12017 | 2026-11-09 | Sorties | Le site Argumentum, en huit langues |

11 valeurs par entité, 1 relation de catégorie, **0 valeur de dimension** (le contenu est
saisi en dim-0, la dimension primaire française). Une seule transaction, un seul recyclage
app-domain.

### 1.2 Geste 2 (issue #2181, point 2)

Entité 11915 / attribut 1820, bloc « L'argumentation » : les 7 valeurs dimensionnées
(dims 7 à 13) portaient toutes `href="/fallacies_fr.html"`. Elles portent désormais
`href="/fallacies_<lang>.html"`. dim-0 (français) n'a pas été touchée.

ValueIds réécrits : 28352 (en), 28353 (ru), 28354 (pt), 28355 (es), 28356 (ar),
28357 (fa), 28358 (zh). Transaction 887.

## 2. Contrôles d'après le geste

### 2.1 Non-régression — la garde qu'ai-01 a substituée

> « Les items existants d'Actus gardent, **dans chacune des 8 langues**, le titre et le
> contenu qu'ils avaient avant le geste, à l'octet près, contre ton relevé d'avant. »

Mesuré, titres comparés **un par un** (`-ceq`, pas de hachage global : le sha des titres
bouge forcément avec 7 items en tête) :

| Langue | Avant | Page 1 | Page 2 | Total après | Premier titre | Non-régression |
|---|---:|---:|---:|---:|---|---|
| fr | 5 | 10 | 2 | 12 | Le site Argumentum, en huit langues | **OK 5/5** |
| en | 5 | 10 | 2 | 12 | idem (repli) | **OK 5/5** |
| ru | 5 | 10 | 2 | 12 | idem | **OK 5/5** |
| pt | 5 | 10 | 2 | 12 | idem | **OK 5/5** |
| es | 5 | 10 | 2 | 12 | idem | **OK 5/5** |
| ar | 5 | 10 | 2 | 12 | idem | **OK 5/5** |
| fa | 5 | 10 | 2 | 12 | idem | **OK 5/5** |
| zh | 5 | 10 | 2 | 12 | idem | **OK 5/5** |

Les cinq titres préexistants sont présents **exactement une fois** dans le corpus rendu de
chaque langue, octet pour octet.

### 2.2 Pagination — 12 items, 10 par page

`ItemsInPage = 10` dans la vue 9400 : la page 1 sert 10 blocs, la page 2 en sert 2. Les
12 items sont donc atteignables. Ce point n'était pas acquis : un « 10 » lu seul aurait
laissé croire que 2 articles étaient perdus.

### 2.3 Pages de détail et illustrations — 49 écarts sur 56, tous `fa`

7 articles × 8 langues = 56 pages demandées. **49 conformes** : 200, titre présent,
légende « Illustration » présente, ligne « Sources : dépôt » présente, **aucun résidu
markdown** (`](`, `**`, `##`, backtick).

Illustrations vérifiées **par les octets servis**, jamais par un 200 :

| Rang | Type | Octets |
|---:|---|---:|
| 1 | image/png | 74 568 |
| 2 | image/svg+xml | 1 857 921 |
| 3 | image/png | 275 852 |
| 4 | image/png | 173 838 |
| 5 | image/png | 418 983 |
| 6 | image/svg+xml | 334 024 |
| 7 | image/png | 210 589 |

Le gabarit `_Article Image.cshtml` passe une URL **externe** à travers `Link.Image` en y
ajoutant `?w=800&h=480&quality=80&mode=crop` ; GitHub raw ignore la requête et sert les
octets. Le point était supposé avant la fenêtre, il est mesuré maintenant, pour PNG et
pour SVG.

### 2.4 #2181 — les 8 liens servis, et leur cible

| Langue | Lien servi | Verdict | Cible demandée | Octets servis |
|---|---|---|---:|---:|
| fr | /fallacies_fr.html | OK (fr→fr) | 200 | 2 721 652 |
| en | /fallacies_en.html | OK (en→en) | 200 | 2 570 283 |
| ru | /fallacies_ru.html | OK (ru→ru) | 200 | 3 008 910 |
| pt | /fallacies_pt.html | OK (pt→pt) | 200 | 2 710 191 |
| es | /fallacies_es.html | OK (es→es) | 200 | 2 659 764 |
| ar | /fallacies_ar.html | OK (ar→ar) | 200 | 2 721 859 |
| fa | /fallacies_fa.html | OK (fa→fa) | 200 | 2 819 504 |
| zh | /fallacies_zh.html | OK (zh→zh) | 200 | 5 363 061 |

Contrôle **par la taille**, comme demandé : une URL à la mauvaise casse rend 200 avec une
page d'erreur de ~348 Ko sur ce DNN ; les huit cibles pèsent 2,5 à 5,4 Mo, ce sont de
vraies pages de carte mentale.

## 3. Défaut trouvé, antérieur au geste : en `fa`, toutes les pages de détail répondent 404

**Constat.** `/fa/Actus/details/<slug>` renvoie 404 pour **les 7 nouveaux articles comme
pour les articles de 2023**. Les 7 autres langues répondent 200 sur les mêmes slugs.

**Ce que ce n'est pas.** Ni un routage DNN raté, ni la culture, ni un manque de traduction :

- la page 148 répond 200 en `fa` par `Default.aspx?tabid=148` ;
- `/fa/Actus` répond 200 et rend bien **10 blocs**, `lang="fa-IR" dir="rtl"` ;
- les 5 articles de 2023 ont **3 valeurs en dim 12 (`fa`)** : Title, Teaser, Content —
  donc l'absence de traduction n'est pas la cause ;
- toutes les dimensions sont `Active=True`, `fa` = 12, `ExternalKey='fa-IR'` ;
- `TabUrls` ne porte qu'une ligne (`fr-FR → /actus`), la même situation que pour `en`,
  `ar`, `zh`, qui fonctionnent.

**Ce que c'est.** La page 404 est rendue **par le gabarit lui-même** : `_Details.cshtml`
ligne 17, la branche `if (article == null)` affiche « مقاله پیدا نشد » (*article
introuvable*) et pose `SetHttpStatus(404)`. C'est donc la **source de données du module**
qui ne rend aucun article pour la culture `fa`, alors qu'elle rend les mêmes articles pour
les sept autres cultures. `UrlKey` n'existe qu'en dim-0 dans tous les articles : la
résolution du détail dépend donc du repli de dimension, et ce repli ne joue pas pour `fa`.

**Portée.** 10 liens de détail cassés aujourd'hui sur `/fa/Actus` : 5 l'étaient déjà avant
ce geste, les 7 nouveaux s'y ajoutent. Aucun correctif n'a été tenté : il toucherait la
requête 2sxc ou le gabarit, ce qu'ai-01 a exclu pour cette fenêtre.

**Note de méthode.** Un premier test a prétendu le contraire — il demandait un slug
d'ancien article que j'avais **inventé**, et il rendait 404 *dans les huit langues*. Un
instrument qui échoue partout ne mesure rien ; c'est en demandant les href **réellement
servis** que la dissymétrie est apparue.

## 4. Retour arrière

| Geste | Sauvegarde | Contenu |
|---|---|---|
| Articles | `G:\Mon Drive\MyIA\Argumentum\backups-eav-2182\20261109-112834-articles-apply.json` | 63 608 o, 7 entités complètes |
| Liens | `G:\Mon Drive\MyIA\Argumentum\backups-eav-2181\20261109-113741-apply-2181.json` | 48 034 o, 8 valeurs avec `Len` et `Sha256` |

Retour arrière des articles : `news-item.ps1 -Mode delete -EntityId <id> -Title '<titre
exact>'` — le mode refuse un titre qui ne correspond pas octet pour octet, et contrôle
ensuite `valeurs restantes = 0`. Retour arrière des liens : réécriture inverse des 7
`ValueId`, dont l'aller-retour octet pour octet a été prouvé **avant** le COMMIT.

## 5. Registre

`docs/dnn-localization/eav-content.csv` ré-exporté (673 lignes). Écart avec la version de
master : **42 lignes ajoutées** (7 articles × 6 champs : Content, CropAnchor, Image,
Teaser, Title, UrlKey), **0 retirée**, **1 modifiée** — `8A8636A4…|Text`, le bloc
« L'argumentation », colonnes `en,ru,pt,es,ar,fa,zh`. Aucune dérive extérieure.
`verify` : 673 lignes des deux côtés, **0 écart**, « réimport à blanc = 0 valeur changée ».

## 6. Défauts d'instrument trouvés par les répétitions

Cinq, tous invisibles à la lecture, tous trouvés parce que le script a été joué avant
d'être cru :

| Défaut | Symptôme | Correction |
|---|---|---|
| `@( … \| ConvertFrom-Json )` sous PS 5.1 | « baseline : 1 langues, attendu 8 » | tester `-is [System.Array]` au lieu d'envelopper |
| Paramètre `[string]$Apres` **et** variable `$apres` | la contrainte `[string]` reconvertit le tableau en chaîne, `Count` = 1 | suffixer les paramètres `…File` |
| `Titres` stocké en chaîne jointe dans le JSON des sondes | 5 titres comparés à une chaîne unique, tout en écart | découper des deux côtés |
| `curl` sans `-L` sur `/en/Actus/page/2` | 301 non suivi, lu comme « pas de page 2 » | `-L` |
| Filtre d'image `raw\.githubusercontent\|/Portals/` | prenait le logo du skin (`/Portals/_default/…`), requête en `000` | restreindre à `/Portals/1/adam/` |

Et un marqueur non discriminant : `Sources` seul était vrai **aussi sur la page 404** ; il
est remplacé par `Sources : dépôt`, qui n'apparaît qu'une fois et seulement sur la page
réelle.

*po-2023*

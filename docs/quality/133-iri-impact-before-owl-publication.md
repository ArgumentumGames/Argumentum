# #133 — Impact IRI avant publication OWL : la surface est **déjà instable**

**Auteur** : po-2023 (worker) · **Date** : 2026-09-14 · **Base** : `origin/master` `21a72385`
**Nature** : **mesure**, lecture seule. Aucune régénération, aucune publication, aucune écriture CSV.
**Portée** : grain ③ de la deep-queue po-2023 (`#458` c.`5656689863`), **consomme les grains ① et ②**.
**Ré-orientation** : le dispatch annonçait PK 511 `nonverbalInfluence` → `nonverbalCommunication` comme cas
d'école. **Cette prémisse est sans objet** (§3) — la mesure ci-dessous la remplace par un risque mesuré,
plus large et déjà réalisé.

---

## §0 — Ce qui est mesuré

L'IRI est **une fonction du libellé anglais**. La question que #133 doit trancher — publier — revient donc à
**figer une surface d'IRI**. Ce dossier quantifie cette surface, et le fait qu'elle **a déjà bougé**.

| ensemble | termes (namespace Argumentum) |
|---|---:|
| OWL **publié** (artefact servi, mars 2024 — cf. grain ①) | **1 405** |
| OWL **committé** (`docs/ontology/argumentum.owl` @ `HEAD`) | **1 439** |
| **communs** | **1 264** |
| **divergents** | **316** |

⇒ **316 des 1 405 IRI publiés (22,5 %) ne sont pas ceux qu'une publication du committé servirait.**

---

## §1 — Instrument, et son contrôle zéro

- **Extraction** : `IRI="…"` sur les deux fichiers OWL, restreinte au **namespace Argumentum**
  (`https://www.argumentum.games/argumentum_fallacies.owl#`), puis `comm`.
- **Pourquoi la restriction de namespace est obligatoire** : `IRI="…"` capture aussi les **déclarations de
  préfixes** et les vocabulaires externes (SKOS, RDF, DC, AIF). Une première passe **non filtrée** rendait
  « 192 disparus / 195 apparus » dont l'essentiel était ce bruit de sérialisation (l'artefact publié
  sérialise en préfixes `skos:core#`, le committé en IRI complets). **Sans filtre, la mesure se lit comme un
  écart sémantique massif alors qu'elle ne compare que deux styles d'écriture.** C'est la restriction au
  namespace qui rend le chiffre interprétable.
- **Cap 0** : le nombre de termes du namespace Argumentum dans l'artefact publié (**1 405**) et l'identité de
  blob établie au grain ① (`af9e8f38…` = sha256 servi, atteignable seulement depuis `5a086dfe`) portent sur
  **le même fichier** — le compte est donc bien celui de l'artefact servi, pas d'une copie locale.

---

## §2 — La règle de dérivation, lue dans le code

```csharp
// OwlGeneratorConfig.cs — OwlDocumentConfig
public static string GetId(string text)
    => text.Camelize().Replace("'","").Replace("-","").Replace(",","").Replace(" ","");
```

appelée **sur le libellé anglais** :

```csharp
var fallacyId = GetId(fallacy.TextEn);      // lignes 212 et 323
```

⇒ **`IRI = Camelize(text_en)`**, apostrophes / traits d'union / virgules / espaces retirés **après** le
`Camelize`. `Camelize` est une extension **Humanizer** ; le commentaire de la fonction (`#951`) documente déjà
qu'un changement de **majeure** de Humanizer modifie le résultat — six libellés du corpus produisaient des IRI
contenant des espaces littéraux, invalides en fragment IRI.

**Conséquence directe** : toute édition du libellé EN — y compris une simple **ponctuation** — **déplace
l'IRI** du concept. La surface d'IRI n'est pas un identifiant stable ; c'est une projection du texte.

---

## §3 — PK 511 : la prémisse du dispatch est sans objet

Le dispatch annonce un renommage `nonverbalInfluence` → `nonverbalCommunication` à instruire.

| source | état mesuré |
|---|---|
| CSV source, PK 511 | `text_en` = **`Nonverbal influence`** · `Subsubfamily` = **`Nonverbal influence`** |
| `nonverbalCommunication` dans le dépôt | **0 occurrence** (ni CSV, ni C#, ni OWL, ni doc) |
| IRI `nonverbalInfluence` | **présent dans l'OWL publié *et* dans l'OWL committé** |
| archives 2022 / v3 | `Nonverbal influence` — **inchangé depuis 2022** |

⇒ **Il n'y a aucun renommage à propager.** Rien n'a été renommé nulle part : ni dans le OWL publié (grain ①),
ni dans le source, ni dans une archive. L'IRI `nonverbalInfluence` **ne bouge pas** — il figure dans les
1 264 communs.

Ce qui existe, en revanche, et que le grain ① avait relevé : `nonverbalInfluence` vit dans une **export dérivé**
(`docs/ontology/aif-export/aif-owl-attack-edges.csv`), pas dans le source. Le libellé source est
`Nonverbal influence` ; `nonverbalInfluence` en est la **projection** par `GetId`.

**Le cas d'école réel n'est donc pas PK 511 — ce sont les 73 concepts du §4.**

---

## §4 — Ce qui a déjà bougé : 73 déplacements, et ils sont causés par la ponctuation

Les 316 divergents se décomposent :

| classe | nombre | lecture |
|---|---:|---|
| **couples à variante de casse** (même concept, slug différent) | **73** | **déplacement d'IRI** — le concept reste, l'identifiant change |
| publiés sans jumeau | 68 | IRI qui **disparaîtraient** |
| committés sans jumeau | 102 | IRI qui **apparaîtraient** — dont **32** d'infrastructure AIF/OWL (`*_Conflict`, `*_Conflicted`, `aif*`, `allows`) |

**Les 73 déplacements sont tous du même motif** : le libellé EN a changé de **ponctuation**, et `GetId` suit.

| PK | `text_en` (CSV courant) | IRI **committé** | IRI **publié** (mars 2024) |
|---:|---|---|---|
| 1059 | `Absent-mindedness` | `absentMindedness` | `absentmindedness` |
| 1377 | `Anti-environmentalism` | `antiEnvironmentalism` | `antienvironmentalism` |
| 513 | `Baby-talk` | `babyTalk` | `babytalk` |
| 939 | `Cover-up` | `coverUp` | `coverup` |
| 969 | `File-drawer effect` | `fileDrawerEffect` | `filedrawereffect` |
| 213 | `Dog-whistle politics` | `dogWhistlePolitics` | `dogwhistlepolitics` |

Le **trait d'union** est le discriminant : `Camelize` coupe sur `-` (⇒ `M` majuscule), là où la sérialisation
de 2024 rendait le mot soudé en minuscules. **Une seule marque de ponctuation, 73 identifiants déplacés.**

⚠️ **Contrôle de la règle** : ma ré-implémentation de `GetId` sur ces six libellés du CSV courant reproduit
**exactement** les IRI committés (colonne 3) — et **aucun** des publiés. La règle du §2 est donc confirmée
empiriquement, et l'écart publié↔committé est bien dans le **libellé**, pas dans le namespace (identique :
`https://www.argumentum.games/argumentum_fallacies.owl#`).

---

## §5 — Contrôles

| contrôle | énoncé | résultat |
|---|---|---|
| **Cap 0** | le compte de termes porte sur l'artefact *servi*, pas sur une copie | identité de blob du grain ① (`af9e8f38…` = sha256 servi) ✅ |
| **positif** | un concept non touché doit ressortir commun | `nonverbalInfluence` ∈ communs ✅ |
| **négatif** | la comparaison doit discriminer | `absentmindedness` ≠ `absentMindedness` ✅ |
| **reproduction de la règle** | `GetId` ré-implémenté doit prédire la colonne committée | **6/6** ✅ — et 0/6 des publiés |
| **filtre** | le namespace doit être restreint | sans filtre : 192/195 (bruit de préfixes) ; avec : 141/175 (**terme**) ✅ |

**Transparence sur un faux départ** : ma première extraction, **non filtrée par namespace**, comparait des
déclarations de préfixes à des IRI complets et rendait « 192 disparus / 195 apparus ». Le chiffre était
**plus gros et faux** — il se lisait comme un écart sémantique majeur. Le filtre au namespace Argumentum le
ramène à 141/175, dont 73 sont des déplacements réels. *Un écart d'instrument qui gonfle le résultat se lit
comme une découverte.*

---

## §6 — Ce que je n'établis PAS

- **Quelle surface est canonique.** Je mesure l'écart ; je ne dis pas si les IRI doivent être ceux de 2024 ou
  ceux du générateur courant. C'est l'arbitrage de #133.
- **La cause exacte de chaque déplacement.** J'établis le **mécanisme** (ponctuation du libellé) et je le
  vérifie sur 6 cas ; je n'ai pas daté chaque édition de libellé.
- **La provenance de l'artefact publié.** Établie au grain ① : non tracé, mars 2024. Je n'établis pas **qui**
  l'a produit ni avec **quelle** version du générateur.
- **Les 102 apparus** : je les compte et j'isole 32 termes d'infrastructure ; je ne juge pas des 70 autres.
- **Aucun effet de bord** : aucune régénération, aucune publication, aucune écriture CSV — conforme au ⛔.

---

## §7 — Ce que cette mesure donne à #133

1. **Publier, c'est figer.** L'IRI étant une projection du `text_en`, toute publication crée une surface que
   la prochaine correction de libellé — même typographique — **déplacera**. #133 demande un *endpoint
   stable* ; la stabilité ne peut pas venir du nom si le nom suit le texte.
2. **La surface est déjà instable, mesurablement.** 316/1 405 (22,5 %) diffèrent entre ce qui **est servi
   aujourd'hui** et ce que le dépôt produirait. Republier le committé **déplacerait 73 identifiants** et en
   ferait disparaître 68 — sans aucune décision de nommage.
3. **Le cas PK 511 n'existe pas.** Il n'y a rien à instruire : aucun renommage n'a été fait ni projeté dans
   le dépôt. L'inscrire comme gate ferait **attendre une décision sur un objet inexistant** (`arbitrage D`).
4. **Le vrai gate est une politique d'IRI**, pas un renommage : soit des IRI **découplés du libellé** (PK ou
   identifiant stable, le libellé devenant un `skos:prefLabel`), soit une **résolution de version** assumée
   (`owl:versionIRI` + redirections), soit l'acceptation explicite du déplacement. **Je ne tranche aucune des
   trois** — #133 les porte.

---

*master `21a72385` · OWL publié = `DNNPlatform/argumentum_fallacies.owl` (artefact servi, grain ①) ·
OWL committé = `docs/ontology/argumentum.owl` @ `HEAD` · lecture seule : `grep`, `comm`, lecture de code ·
⛔ aucune régénération, aucune publication, aucune écriture CSV · verdict visuel : ai-01.*

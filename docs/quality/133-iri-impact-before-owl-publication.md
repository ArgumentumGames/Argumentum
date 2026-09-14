# #133 — Impact IRI avant publication OWL : la surface est **déjà instable**

**Auteur** : po-2023 (worker) · **Date** : 2026-09-14 · **Base** : `origin/master` `21a72385`
**Oracle OWL committé** : `git show origin/master:docs/ontology/argumentum.owl` (`0aff63a3bbb4…`) — **jamais**
le fichier de travail local ni le blob de `HEAD`, tous deux **en retard de 62 commits** (le checkout local est
le webroot vivant de préprod).
**Nature** : **mesure**, lecture seule. Aucune régénération, aucune publication, aucune écriture CSV.
**Portée** : grain ③ de la deep-queue po-2023 (`#458` c.`5656689863`), **consomme les grains ① et ②**.
**Ré-orientation (v2, post-revue 14/09)** : le dispatch annonçait PK 511 `nonverbalInfluence` →
`nonverbalCommunication` comme cas d'école. **Ce cas est réel et déjà réalisé côté source** (§3) : le
renommage a été livré le 11/09 (`930ae523`, PR #1331) ; seul l'IRI n'a pas suivi. Il devient l'illustration
vivante du risque mesuré ci-dessous. *(La v1 de ce dossier concluait la prémisse « sans objet » sur la foi
du CSV du checkout local — 62 commits en retard : troisième morsure du piège d'oracle, corrigée ici.)*

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

⚠️ **Provenance des chiffres de la ligne « publié »** : mesurés sur la copie webroot locale
(`DNNPlatform/argumentum_fallacies.owl`), dont l'identité à l'octet avec le servi a été établie aux grains ①-②.
Ils sont **déclarés depuis cette mesure** et **non reproductibles depuis un checkout frais du dépôt** — l'artefact
publié est non tracé. La ligne « committé », elle, est reproductible (`git show origin/master:…`).

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
  blob établie au grain ① portent sur **le même fichier** — le compte est donc bien celui de l'artefact servi,
  pas d'une copie locale. **Deux empreintes, deux espaces de hachage** (ne pas les confondre) : l'OID git du
  fichier webroot est `af9e8f38…` (blob atteignable dans le dépôt **seulement** depuis `5a086dfe`, le commit
  untracked de l'archive 28/08), et son **SHA-256** est `258a9e43…` — c'est cette seconde valeur qui identifie
  l'octet servi. Un OID git et un SHA-256 ne se comparent pas entre eux.
- **Piège d'oracle, rencontré et levé** : ma première passe comparait l'artefact publié au blob de **`HEAD`**
  — or ce checkout est le webroot vivant, **62 commits en retard**. J'ai donc re-mesuré contre
  **`origin/master`**. Les deux blobs **diffèrent** (6 835 266 o / `88062644…` pour `HEAD`, 5 985 122 o /
  `0aff63a3bbb4…` pour `origin/master`), mais leurs **ensembles de termes sont identiques** — `cmp` sur les
  deux listes triées rend l'égalité, 1 439 termes de part et d'autre. L'écart entre les deux blobs porte sur
  les **assertions**, pas sur le vocabulaire déclaré. **Les chiffres du §0 ne dépendent donc pas du choix
  d'oracle** — mais la vérification était nécessaire, et elle est consignée ici plutôt que supposée.

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

## §3 — PK 511 : le cas est réel, déjà réalisé côté source, non propagé à l'IRI

Le dispatch annonce un renommage `nonverbalInfluence` → `nonverbalCommunication` à instruire. **Ce renommage
existe** — livré le 11/09 et présent dans l'oracle (`origin/master`) :

| source | état mesuré (`origin/master`) |
|---|---|
| commit | **`930ae523`** — 2026-09-11 20:17:14 +0200, *« fix(fallacies): PK 511 "Influence non verbale" → "Communication non verbale", 8 langues (#1294) (#1331) »* — ancêtre de la base `21a72385` |
| CSV, PK 511 | `text_en` = **`Nonverbal communication`** · `Subsubfamily` = **`Nonverbal communication`** · `link_en` pointe encore l'ancien slug Wikipedia (`Nonverbal_influence`) |
| CSV, compte | **85 occurrences / 84 lignes** du nouveau libellé ; **0** de l'ancien (`Nonverbal influence`) |
| OWL committé | IRI **`nonverbalInfluence` ×28**, `prefLabel` EN `<Literal xml:lang="EN">Nonverbal influence</Literal>` — **le renommage n'y a pas pénétré** |
| OWL publié (mars 2024) | antérieur au renommage — `nonverbalInfluence`, `prefLabel` ancien |

Le compte 85/84 n'est pas une redondance : le libellé de PK 511 est aussi le `Subsubfamily` de ses enfants,
et chaque entrée de la sous-famille le porte.

### §3.1 — L'IRI n'a pas suivi : la règle §2 à l'œuvre sur un cas vivant

`GetId("Nonverbal communication")` = `nonverbalCommunication` ⇒ à la prochaine génération OWL, l'IRI
**`nonverbalInfluence` disparaît** au profit de **`nonverbalCommunication`**, et le `prefLabel` EN bascule.
Le renommage étant livré dans la source, ce déplacement n'est **plus une hypothèse** : il est programmé par
construction. **L'owner l'a arbitré** (#133 c.`5657175826`, 14/09) : le déplacement sera acté à la prochaine
génération OWL. PK 511 est donc le **cas d'école vivant** du §2 — un renommage de libellé, sans aucune
décision de nommage, déplace l'IRI.

### §3.2 — Les artefacts dérivés sont périmés dans les 8 langues (recensement)

Le renommage a touché **680 cellules sur 8 langues** (#1294) mais **aucun artefact dérivé** n'a été régénéré.
Recensement (oracle `origin/master`, compte de l'**ancien** libellé localisé par fichier) :

| artefact dérivé | ancien libellé | occurrences | nouveau |
|---|---|---:|---:|
| `Cards/Fallacies/Mindmaps/fr/Fallacies_fr.content.svg` | `Influence non verbale` | **85** | 0 |
| `Cards/Fallacies/Mindmaps/en/Fallacies_en.content.svg` | `Nonverbal influence` | **1** | 0 |
| `Cards/Fallacies/Mindmaps/ru/Fallacies_ru.content.svg` | `Невербальное воздействие` | **85** | 0 |
| `Cards/Fallacies/Mindmaps/pt/Fallacies_pt.content.svg` | `Influência não verbal` | **85** | 0 |
| `Cards/Fallacies/Mindmaps/es/Fallacies_es.content.svg` | `Influencia no verbal` | **85** | 0 |
| `Cards/Fallacies/Mindmaps/ar/Fallacies_ar.content.svg` | `التأثير غير اللفظي` | **85** | 0 |
| `Cards/Fallacies/Mindmaps/fa/Fallacies_fa.content.svg` | `تأثیر غیرکلامی` | **85** | 0 |
| `Cards/Fallacies/Mindmaps/zh/Fallacies_zh.content.svg` | `非言语影响` | **85** | 0 |

Ces 8 SVG — et l'export AIF (`docs/ontology/aif-export/aif-owl-attack-edges.csv`), qui porte la même
projection — sont la même classe d'objet que l'OWL : des **projections du libellé** qui divergent dès que la
source avance sans régénération. PK 511 en est la démonstration **chiffrée sur les 8 langues à la fois**.

### §3.3 — Erratum de la v1

La v1 de ce dossier lisait PK 511 dans le **CSV du checkout local** (62 commits en retard, pré-`930ae523`)
et concluait « la prémisse est sans objet : `nonverbalCommunication` = 0 occurrence ». La conclusion était un
**artefact d'oracle** : sur l'oracle `origin/master`, les comptes s'inversent (0 ancien / 85 nouveau). C'est la
troisième morsure du piège documenté en §1 — et celle-ci avait atteint la **conclusion**, pas seulement
l'instrument.

---

## §4 — Ce qui a déjà bougé : 73 déplacements, et ils sont causés par la ponctuation

Les 316 divergents se décomposent :

| classe | nombre | lecture |
|---|---:|---|
| **couples à variante de casse** (même concept, slug différent) | **73** | **déplacement d'IRI** — le concept reste, l'identifiant change |
| publiés sans jumeau | 68 | IRI qui **disparaîtraient** |
| committés sans jumeau | 102 | IRI qui **apparaîtraient** — dont **32** d'infrastructure AIF/OWL (`*_Conflict`, `*_Conflicted`, `aif*`, `allows`) |

**Les 73 déplacements sont tous du même motif** : le libellé EN a changé de **ponctuation**, et `GetId` suit.

| PK | `text_en` (CSV `origin/master`) | IRI **committé** | IRI **publié** (mars 2024) |
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
| **Cap 0** | le compte de termes porte sur l'artefact *servi*, pas sur une copie | identité de blob du grain ① : OID git `af9e8f38…` (depuis `5a086dfe`) **et** SHA-256 servi `258a9e43…` — deux espaces de hachage, cf. §1 ✅ |
| **positif** | un concept non touché doit ressortir commun | `strawMan` ∈ communs ✅ — libellé `Straw man` identique dans les deux OWL (prefLabel EN) et dans le CSV `origin/master` |
| **positif inversé (v1 rétracté)** | ⚠️ la v1 utilisait `nonverbalInfluence` comme concept « non touché » — **à tort** : son libellé a été renommé le 11/09 (§3) ; il ne ressort commun que parce que l'OWL committé est **en retard sur la source**. Un faux contrôle positif qui mesurait la péremption, pas la stabilité |
| **négatif** | la comparaison doit discriminer | `absentmindedness` ≠ `absentMindedness` ✅ |
| **reproduction de la règle** | `GetId` ré-implémenté doit prédire la colonne committée | **6/6** ✅ — et 0/6 des publiés |
| **filtre** | le namespace doit être restreint | sans filtre : 192/195 (bruit de préfixes) ; avec : 141/175 (**terme**) ✅ |
| **oracle** | le résultat ne doit pas dépendre du blob de référence | `cmp` des deux listes de termes (`HEAD` vs `origin/master`) : **identiques**, 1 439 chacun ✅ |

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
3. **PK 511 est le cas vivant, et son issue est déjà arbitrée.** Le renommage est livré dans la source
   (11/09, `930ae523`, 680 cellules × 8 langues) ; l'IRI suivra à la prochaine génération — déplacement
   **accepté par l'owner** (#133 c.`5657175826`). Ce qu'il démontre : un déplacement d'IRI peut être
   **déjà programmé** sans aucune décision de nommage, par simple avance de la source sur les artefacts
   dérivés — OWL, SVG, export AIF (§3.2). Les 73 déplacements du §4 en sont la version accumulée depuis
   mars 2024. *(v1 : « le cas n'existe pas » — artefact d'oracle, cf. §3.3.)*
4. **Le vrai gate est une politique d'IRI**, pas un renommage : soit des IRI **découplés du libellé** (PK ou
   identifiant stable, le libellé devenant un `skos:prefLabel`), soit une **résolution de version** assumée
   (`owl:versionIRI` + redirections), soit l'acceptation explicite du déplacement. **Je ne tranche aucune des
   trois** — #133 les porte. Noter que l'owner vient d'exercer la troisième **une première fois** pour
   PK 511 (§3.1) : la politique existe déjà en acte, pas encore en règle.

---

*master `21a72385` · OWL publié = `DNNPlatform/argumentum_fallacies.owl` (artefact servi, grain ①) ·
OWL committé = `git show origin/master:docs/ontology/argumentum.owl` (`0aff63a3bbb4…`) · lecture seule :
`grep`, `comm`, `cmp`, lecture de code · ⛔ aucune régénération, aucune publication, aucune écriture CSV ·
verdict visuel : ai-01.*

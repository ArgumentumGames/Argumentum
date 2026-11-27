# Régénération de l'OWL Vertus — P24-37 (2026-11-24)

> **Statut : régénération faite sur branche ; rien n'est servi avant le merge.** Le gel de
> l'artefact (décision [#133](https://github.com/ArgumentumGames/Argumentum/issues/133) du
> 17/11, révisée le 21/11) est **levé dans son motif** : le remède d'épinglage des IRI (P24-34,
> mergé le 22/11) tient sur le corpus réel, et le dispatch ai-01 du 24/11 (20:40Z) ouvre la
> régénération (« Puis P24-37 »).
> Issue : #133 · file : #458 (P24-37) · lane : po-2024 · mesure du 2026-11-24.
> Constat antérieur, dont celui-ci clôt la prédiction :
> [`133-owl-virtues-peremption-mesuree-2026-10-09.md`](133-owl-virtues-peremption-mesuree-2026-10-09.md).

## 1. Ce qui est régénéré — et ce qui ne l'est pas

| Artefact | Avant (blob committé) | Après (régénéré) | Delta |
|---|---|---|---|
| `argumentum_virtues.owl` | `d4099c79…` · 1 094 832 o · CSV `c1491de9` (2026-09-30) | **`884be0f3…`** · **1 088 889 o** · CSV `HEAD` | −5 943 o |
| `argumentum.owl` (Fallacies) | `e6da4eb8…` · 6 019 161 o | **identique** | **0 octet, 0 clé changée** |

Toutes les valeurs de ce document sont **mesurées** sauf mention contraire. Le Fallacies est
régénéré par le même `--generate-owl` (les deux fichiers sortent ensemble) puis **jeté** :
surface 1 435 → 1 435, 0 IRI perdue, **0 clé de projection changée**, sha256 identique au blob
committé — les changements du CSV Fallacies depuis le 17/11 ne projettent rien dans l'OWL.
Rien à committer de ce côté.

Les deux fichiers déclarent `owl:versionInfo` **1.0.0** avant comme après : le bump est un
geste de tag (§6.1 du [README](README.md)), pas celui-ci.

## 2. `v1.0.0/` — la génération servie d'avant, gardée

Recommandation §6.1 du README (« copier les artefacts gelés dans `v1.0.0/` avant toute
régénération ») **exécutée**, pas reportée :

| Fichier | Blob committé = copie | Octets | Corps servi (Pages) |
|---|---|---|---:|---|
| [`v1.0.0/argumentum.owl`](v1.0.0/argumentum.owl) | `e6da4eb8…` | 6 019 161 | sha256 identique, mesuré le 24/11 (GET lecture seule) |
| [`v1.0.0/argumentum_virtues.owl`](v1.0.0/argumentum_virtues.owl) | `d4099c79…` | 1 094 832 | sha256 identique, mesuré le 24/11 (GET lecture seule) |

La copie est **à l'octet** le blob `origin/master` (sha256 comparé aux deux), et ce blob est
**la génération que le site sert** (sha256 du corps HTTP comparé au blob — mesuré, pas supposé).
Ce n'est pas un snapshot de tag : c'est le recours de comparaison de la génération remplacée.

## 3. Le DoD, condition par condition

### 3.1 — 0 IRI disparue

Surface d'IRI du namespace Argumentum : **224 → 224 classes déclarées** (223 concepts + le
schéma), **0 perdue, 0 gagnée**. Les deux renommages d'août-septembre qui avaient motivé le gel
gardent leur IRI servi — c'est P24-34 qui tient, sur le corpus réel :

| pk | IRI (inchangée) | `prefLabel@en` avant → après |
|---:|---|---|
| 128 | `…/argumentum_virtues.owl#acceptableInformalLogic` | « Acceptable informal logic » → « Solid informal logic » |
| 176 | `…/argumentum_virtues.owl#takingTheOpponentsIdeologicalBiasesIntoAccount` | « Taking the opponent's ideological biases into account » → « Taking the parties' ideological biases into account » |

Les deux changements viennent du même commit (`371dd730`, 2026-10-07, #1795).

### 3.2 — chaque `seeAlso` retiré rattaché à un changement du CSV, ou rétabli

**Rien à rétablir** : les 78 `rdfs:seeAlso` retirés sont **exactement** les 78 cellules
`link_fr`/`link_en` retirées du CSV entre `c1491de9` et `HEAD` — égalité d'ensembles vérifiée
dans les deux sens par l'instrument (fragment ↔ pk via le registre), et réciproquement les 61
ajoutés sont exactement les 61 cellules ajoutées. Chaque retrait est rattaché (pickaxe
`git log --reverse -S<url> c1491de9..HEAD -- <csv>`, premier commit de la plage) :

| Commit | Date | Sujet | Retraits | Ajouts |
|---|---|---|---:|---:|
| `059f219f` | 2026-10-10 | #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération | 76 | 7 |
| `6ddac169` | 2026-10-16 | #458 virtues re-curation pk 52 + pk 102 — mauvais référents, 12 cellules | 2 | 4 |
| `8253cd3e` | 2026-10-15 | (ajouts seuls) | 0 | 35 |
| `2bb7958b` | 2026-10-14 | (ajouts seuls) | 0 | 15 |

**78/78 retraits et 61/61 ajouts attribués, 0 orphelin.** Le solde est **−17** : le « 17
`rdfs:seeAlso` en moins » qui circulait pour cette régénération est ce **net**, pas un compte
de retraits (78 retirés − 61 ajoutés).

### 3.3 — empreinte du nouveau blob (pour W2-03 au tag)

```
sha256  884be0f3cea4d3666a6778e3910a38de09648af91f833cc9dde3d4249552a9e1
octets  1 088 889
```

C'est l'empreinte du blob que cette PR committe. W2-03 pourra la citer ; le tag v2.0.0
régénérera de toute façon (§6.1) et aura la sienne.

## 4. Les 114 clés changées, et leur cause

Projection du CSV (les règles du générateur) : **114 clés** — 114/114 avec leur entrée CSV
changée, **0 sans cause, 0 hors oracle, 0 effet sans entrée** :

| Prédicat | Changements | Entrée(s) CSV |
|---|---:|---|
| `rdfs:seeAlso@fr` | 74 | `link_fr` |
| `rdfs:seeAlso@en` | 21 | `link_en` |
| `skos:definition@en` | 17 | `description_en` |
| `skos:prefLabel@en` | 2 | `title_en` |

Distribution des littéraux : **fr 37 remplacés / 22 retirés / 15 ajoutés ; en 26 / 12 / 2**.
Conséquence à retenir : **aucun libellé, définition ni commentaire français n'a changé** — les
74 changements fr sont tous des URL (37 + 22 + 15 = 74). Le contenu éditorial FR de l'OWL est
stable depuis `c1491de9` ; tout le mouvement français est dans les liens.

Réconciliation avec un relevé antérieur (rapporté) de « 59 fr / 38 en remplacés » — **dérivé** :
37 + 22 = 59 et 26 + 12 = 38, c'est-à-dire « modifiés **et** retirés » sous un même mot. Sous la
définition stricte (valeur modifiée), la mesure donne 37 fr / 26 en. Ce n'est pas une
contradiction, c'est une définition.

Les 17 définitions anglaises : **13** ne gagnent qu'un point final (`371dd730`, #1795), **4**
sont réécrites — `noBlackmailThroughConsequences` et `appropriateLevelOfProof` (`371dd730`),
`impartialSources` (`da1251e3`, 2026-10-21, #1928), `principleOfCharity` (`edb39554`,
2026-10-08, #1806) :

| pk (IRI) | Avant | Après |
|---|---|---|
| `impartialSources` | Selection of objective sources, free of personal interest, for an argumentation grounded in truth rather than opinion. | Objective sources, free of personal interest, for argumentation grounded in facts rather than opinion. |
| `noBlackmailThroughConsequences` | Manipulating one's interlocutor by resorting to threats or promises constitutes an affront to honest and ethical exchange. | Refusing to manipulate one's interlocutor through threats or promises linked to consequences. |
| `appropriateLevelOfProof` | The expected objective for the opponent must be attainable and fair, with measurable, clear and well-defined criteria for assessing success. | Requirement of clear, measurable and fair criteria for evaluating the success of an argumentation. |

## 5. L'instrument — un oracle dérivé du CSV, jamais de l'OWL

[`tools/133-owl-regen-delta.py`](../../tools/133-owl-regen-delta.py) projette le CSV aux deux
refs à travers les règles du générateur (`VirtueOwlGeneratorConfig`) — `prefLabel` et
`definition` toujours émis, `seeAlso` seulement si la cellule est non vide, `comment` seulement
si `AIF_skosDirectRef` est non vide, attaques et `goodTenorOf` par jeton, `broader/narrower` par
`path`/`depth`, IRI par le registre pk → fragment — soit **2 901 clés** observables. La valeur
attendue vient du **CSV**, la valeur constatée du **XML** : l'instrument ne peut pas se donner
raison à lui-même.

**Calibration exigée avant toute conclusion** : rejoué sur la paire **ancienne** (OWL committé ↔
CSV `c1491de9`), il doit rendre 0 écart — c'est le cas (0/0), et 0/0 aussi sur la paire
nouvelle. Les quatre seaux sont vides : aucun changement OWL sans cause, aucune cause sans
effet, aucune clé hors oracle, aucune entrée absente du CSV ancien (2 901 clés comparables).
Codes de sortie : 0 cohérent · 2 instrument · 3 IRI perdue · 4 OWL changé sans cause CSV ·
5 CSV changé sans effet OWL · 6 clé hors oracle.

Rejeu complet :

```bash
dotnet run --project Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj -- --generate-owl
python -I tools/133-owl-regen-delta.py --repo . \
  --old docs/ontology/v1.0.0/argumentum_virtues.owl --csv-old c1491de9 \
  --new Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0-windows/Target/fr/Ontology/argumentum_virtues.owl \
  --csv-new HEAD --attribute --json .tmp/p2437/delta.json
```

## 6. Contrôles joués

- `dotnet test …Argumentum.AssetConverter.Tests.csproj --filter "FullyQualifiedName~Ontology"` :
  **129 réussis, 0 échec** (22 s) sur le blob régénéré en place. Les contrats qui pincent le
  registre (`OwlIriRegistryCorrespondenceTests` : 223 rangées, 222 `broader`), les fragments, le
  graphe et la casse des `xml:lang` passent inchangés ; aucun test ne pinçait le hash ni la
  taille de l'ancien blob, rien n'a eu à bouger.
- Garde de gros blob : `docs/ontology/` est dans l'allow-list de
  [`tools/large-blob-guard.py`](../../tools/large-blob-guard.py) et 1 088 889 o < 2 MiB de toute
  façon ; la copie `v1.0.0/` a le même contenu que le blob committé (même blob, aucun
  re-déclenchement).
- Contrat d'index : ce fichier et `v1.0.0/` sont cités au §8 du [README](README.md).

## 7. Ce que ce grain ne fait pas — et ce qui reste non vérifié

- **Rien n'est servi avant le merge** : Pages republie au push sur `master` (§6 du README). Le
  sha256 servi passera alors à `884be0f3…` ; le smoke `tools/133-pages-owl-smoke.sh` est à
  rejouer après le merge — il est la mesure, pas ce document.
- **Bump de version non fait** : `versionInfo` reste 1.0.0 jusqu'au tag ; `v1.0.0/` est
  l'instantané de la génération **remplacée**, pas un « release 1.0.0 » — le snapshot de tag de
  la §6.1 (étape 3) reste à faire pour v2.0.0.
- **La réadmission du lien OWL vertus dans le bloc publié n'est pas faite** :
  [`../publication/argumentation-bloc-fr.md`](../publication/argumentation-bloc-fr.md) l'exclut
  « jusqu'à vérification de #133 » ; cette mesure fournit la vérification (0 IRI perdue), mais
  la pose du texte servi est le geste de publication (pose groupée après #131) — hors de ce
  grain.
- **Non vérifié** : le rendu du nouveau fichier dans un outil consommateur (Protégé, WebVOWL,
  raisonneur) — le dépôt n'a pas de porte de raisonnement, et aucune n'a été improvisée ; la
  fidélité mesurée est celle de la projection CSV → OWL, pas celle d'un parseur tiers.
- **Écart de file signalé** : le corps de #458 liste P24-37 en colonne po-2023 ; le dispatch
  ai-01 du 24/11 20:40Z l'assigne à po-2024 (« Puis P24-37 »). Exécuté par po-2024 sous le
  dispatch ; la colonne est à corriger côté #458.

Refs #133, #458, #1066

*po-2024*
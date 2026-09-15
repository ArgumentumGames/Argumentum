# #415 — Options de réduction : réviser l'allow-list du large-blob-guard (dossier tranchable)

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base** : master `c089d526`
**Instrument** : contribution au HEAD mesurée ce jour (`git ls-files -s` + `cat-file --batch-check`
par SHA dédupliqué) ; croissance 10 semaines citée de [`415-pack-growth-explained.md`](415-pack-growth-explained.md)
(PR #1361) ; allow-list lue dans `tools/large-blob-guard.py:64-75`.
**Statut** : **PROPOSITION** — ⛔ aucun history rewrite, ⛔ aucun `gc`/`repack` (Phase 2 toujours
gated sur GO jsboige). **« 2,05 → 5,57 GiB » reste non commensurable** (erratum c.5658942651 :
deux clones différents ; ce clone = **2,22 GiB / 22 packs**, stable depuis le 14/09).

---

## §0 Le résultat en une phrase

Les 11 entrées de l'allow-list pèsent **758,7 Mo au HEAD** (36 % du clone utile) — le dossier les
classe en **3 statu quo évidents** (diff = valeur), **5 candidats LFS post-tag** (dont 2 déjà actés
#628), **1 décision DNN** (le plus gros poste) — toutes les options sont **non destructives et
n'agissent que sur le futur** (l'historique reste ce qu'il est jusqu'à une éventuelle Phase 2 GO).

## §1 La table par entrée (contribution MESURÉE ce jour au HEAD + croissance #1361)

| entrée allow-list | HEAD Mo | fichiers | >2 Mo | croissance 10 sem | option |
|---|---:|---:|---:|---|---|
| `DNNPlatform/` | **209,8** | 9 067 | 15 | +30,95 MiB (734 obj.) | **§2 — décision dédiée** |
| `Cards/Fallacies/Archive/` | **127,4** | 348 | 1 | ~0 (figé) | statu quo — archive de record |
| `Cards/Fallacies/Mindmaps/` | **123,9** | 80 | 29 | majeure part de `Cards/Fallacies` +37,69 MiB | **LFS post-tag** (churn régénérable) |
| `Cards/Packaging/` | **96,9** | 20 | 10 | ~0 | **LFS** (acté #628 : preserve/LFS) |
| `Cards/Fallacies/Assets/` | 76,0 | 192 | 3 | ~0 | LFS post-tag possible |
| `Generation/…/Data/Mindmap/` | 52,7 | 26 | 4 | ~0 | LFS post-tag possible |
| `Generation/Sketch/` | 45,3 | 6 | 1 | ~0 | **LFS** (acté #628) |
| `Generation/CardPen/images/` | 15,7 | 41 | 1 | ~0 | statu quo — vendored |
| `docs/ontology/` | 7,0 | 15 | 1 | +1,84 MiB | statu quo — coût dérisoire |
| `Argumentum Fallacies - Taxonomy.csv` | 3,9 | 1 | 1 | ~0 | **statu quo obligé** — le diff git EST la valeur éditoriale |
| `Argumentum Virtues - Taxonomy.csv` | ~1,7 | 1 | 0 | ~0 | idem |
| **TOTAL** | **758,7** | 9 797 | 66 | | |

Les 3 plus gros blobs de la croissance récente : `docs/ontology/argumentum.owl` **5,71 MiB**
(#1379), `Mindmaps/zh/Fallacies_zh.html` **5,0 MiB**, `Mindmaps/ru/Fallacies_ru.svg` **2,9 MiB**.

## §2 Les trois décisions owner (feuille tranchable)

**D1 — `DNNPlatform/` (209,8 Mo + 31 MiB de croissance, le plus gros poste).** Le périmètre DNN
(#131/#132) n'est **pas déployé** : le retirer de git aujourd'hui casserait le chantier.
Options : (a) *statu quo jusqu'au déploiement, puis extraction vers dépôt dédié ou LFS* *(reco —
non destructif, réversible)* ; (b) LFS immédiat sur les seuls `App_Data/ExtensionPackages/*.resources`
(124 Mo, re-téléchargeables) et `Portals/*/Downloads/*.zip` (205 Mo listés Phase 1 — régénérables
→ politique Phase 3 = GitHub Releases, cf. dossier drift #1383 : les assets servis vivent déjà là).

**D2 — Mindmaps (123,9 Mo, churn par nature).** Les 43 SVG + wrappers se re-commitent à chaque
régén (CLAUDE.md : « ne pas hardcoder les comptes »). Options : (a) *statu quo jusqu'au tag puis
LFS* *(reco)* ; (b) compresser avant commit (SVG→gzip ÷5-10, mais casse la diff git et les wrappers
HTML référencent les SVG nus) ; (c) Release à chaque régén (lourd, multiplie les artefacts).
⚠️ (b) casse la revue par diff — déconseillé tant que le churn est actif.

**D3 — Sources design (Packaging 96,9 + Sketch 45,3 = 142,2 Mo).** Décision **déjà actée #628**
(preserve/LFS) : ce dossier ne fait que la rejouer avec les chiffres courants. Le geste LFS attend
un créneau calme (il réécrit les chemins pour les nouveaux commits — pas l'historique).

## §3 Ce que ce dossier n'établit pas

⛔ Aucun rewrite/gc/repack lancé ni recommandé ici (Phase 2 = GO jsboige, séparé) · ⛔ aucune
entrée retirée de l'allow-list par ce dossier (le levier est owner ; retirer une entrée sans
migration préalable mettrait la CI rouge au prochain commit légitime) · la croissance `tmp/`
(+17,28 MiB, 62 objets < 2 Mo chacun — sous le seuil du garde, pas dans l'allow-list) est un
constat #1361, hors allow-list · les chiffres décrivent **ce clone** (po-2024) — la comparaison
inter-clones reste l'erratum c.5658942651.

---

*po-2024 — pool #458, grain ⑪. Le worker mesure et pose les options ; l'allow-list et le LFS sont à l'owner.*

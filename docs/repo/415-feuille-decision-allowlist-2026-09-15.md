# #415 — Feuille de décision owner : allow-list du large-blob-guard (quoi garder / quoi sortir du HEAD)

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base** : master `608a0fd9`
**Source** : extraction du dossier mergé **#1389** (`9344f75b`) —
[`415-allowlist-options-2026-09-15.md`](415-allowlist-options-2026-09-15.md) §1-§2 ; contributions
RAPPORTÉES du dossier (mesurées 15/09 par SHA dédupliqué sur `c089d526`, revues par ai-01 avant
merge). **Statut** : **support de décision uniquement** — ⛔ 0 rewrite, ⛔ 0 gc/repack, ⛔ aucune
entrée retirée de l'allow-list par ce document.

---

## La question owner en une ligne

Les **11 entrées** de l'allow-list pèsent **758,7 Mo au HEAD** (36 % du clone utile) —
**quoi garder tel quel, quoi migrer (LFS / Releases / extraction) et quand**, sachant que
**toutes les options n'agissent que sur le futur** (l'historique reste intact jusqu'à un
éventuel GO Phase 2 séparé).

## La table « quoi garder / quoi sortir » (coût par entrée, RAPPORTÉ du dossier)

| entrée | HEAD Mo | verdict proposé | coût du geste / garde |
|---|---:|---|---|
| `DNNPlatform/` | **209,8** | **→ Décision D1** (ci-dessous) | migration = geste structurant, à caler sur le déploiement #131/#132 |
| `Cards/Fallacies/Mindmaps/` | **123,9** | **sortir → LFS post-tag** *(D2)* | churn régénérable ; ⚠️ pas de gzip (casse la diff + wrappers référencent les SVG nus) |
| `Cards/Packaging/` | 96,9 | **sortir → LFS** — **déjà acté #628** | geste = rejouer l'acté avec les chiffres courants, dans un créneau calme |
| `Generation/Sketch/` | 45,3 | **sortir → LFS** — **déjà acté #628** | idem |
| `Cards/Fallacies/Assets/` | 76,0 | sortir → LFS post-tag **possible** | non urgent (~0 croissance) |
| `Generation/…/Data/Mindmap/` | 52,7 | sortir → LFS post-tag **possible** | idem |
| `Cards/Fallacies/Archive/` | 127,4 | **garder** | archive de record, ~0 croissance, figée |
| `Generation/CardPen/images/` | 15,7 | **garder** | vendored, diff utile |
| `docs/ontology/` | 7,0 | **garder** | coût dérisoire (+1,84 MiB/10 sem) |
| CSV Fallacies (3,9) + CSV Virtues (1,7) | 5,6 | **garder — obligé** | le diff git **est** la valeur éditoriale |

## Les 3 coches (D1-D3, extraites du §2 du dossier)

**D1 — `DNNPlatform/` (209,8 Mo + 31 MiB de croissance, le plus gros poste).** Le périmètre
(#131/#132) n'est pas déployé : le retirer de git aujourd'hui casserait le chantier.
☐ **(a) statu quo jusqu'au déploiement, puis extraction dépôt dédié ou LFS** *(reco — non
destructif, réversible)* · ☐ (b) LFS immédiat sur les seuls `App_Data/ExtensionPackages/*.resources`
(124 Mo, re-téléchargeables) + `Portals/*/Downloads/*.zip` → politique GitHub Releases.

**D2 — Mindmaps (123,9 Mo, churn par nature).** Les 43 SVG + wrappers se re-commitent à chaque
régén. ☐ **(a) statu quo jusqu'au tag puis LFS** *(reco)* · ☐ (b) compresser avant commit
(gzip ÷5-10 — ⚠️ **déconseillé** : casse la revue par diff et les wrappers HTML référencent les
SVG nus) · ☐ (c) Release à chaque régén (lourd).

**D3 — Sources design (Packaging 96,9 + Sketch 45,3 = 142,2 Mo).** Décision **déjà actée #628**
(preserve/LFS) : ☐ **confirmer le geste LFS dans un créneau calme post-tag** *(reco = exécuter
l'acté)* · ☐ rouvrir la décision.

## Garde (inchangée, rappel du dossier source)

⛔ Aucune entrée retirée sans migration préalable — retirer une entrée de l'allow-list sans
migrer met la CI **rouge au prochain commit légitime** du périmètre · ⛔ Phase 2 (rewrite/gc/
repack) = GO jsboige **séparé**, jamais embarqué · « 2,05 → 5,57 GiB » reste **non
commensurable** (erratum c.5658942651 — deux clones ; ce clone 2,22 GiB / 22 packs stable).

## Ce que cette feuille n'établit pas

⛔ Aucune écriture ni migration lancée · ⛔ contributions **RAPPORTÉES** du dossier mergé
(mesures 15/09 re-vues par ai-01), pas re-mesurées pour cette extraction · ⛔ aucune décision
prise — les reco sont worker, les coches à jsboige · ⛔ la croissance `tmp/` (+17,28 MiB, objets
sous seuil, hors allow-list) reste un constat #1361 sans objet ici.

---
*po-2024 — pool #458 (c.5666260217), grain ⑨. Feuille extraite du dossier #415 mergé (#1389) ;
la méthode de mesure et le détail des options vivent dans le dossier source.*

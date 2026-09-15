# #942 §2 — Inventaire des Dependabot `DNNPlatform/` gelées : 3 PRs, toutes **post-tag**, 0 re-clore, 0 à revoir

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base mesure** : master `6fbde739`
**Grain** : pool #458 renouvelé (c.5666260217), grain ⑧ — suite du tri #1384
(`134-dependabot-pretag-tri-2026-09-15.md`, mergé `b7972aa3`). **⛔ doc only** : aucune PR
Dependabot mergée ni close par le worker.

---

## §0 L'état de la file (MESURÉ le 15/09 soir)

5 PRs Dependabot ouvertes — #1371 et #1370 déjà classées par #1384 (**défermées post-tag** : binaire
du pipeline / instrument de QA). Restent les 3 `DNNPlatform/`, objet de ce dossier :

| PR | ouverte | Bump | Fichiers touchés (MESURÉ) | Dernier commit du chemin avant elle |
|---|---|---|---|---|
| #1328 | 11/09 | `svgo` 2.8.3→2.8.4 (patch) | `Skins/Bootstrap 4 Instant/package-lock.json` **seul** | `6690cb83` 25/07 — bump micromatch #885 (mergé) |
| #1323 | 10/09 | `baseline-browser-mapping` 2.10.27→2.11.22 (mineur) | `2sxc/ImageCompare2/package-lock.json` **seul** | `02c3180e4` 05/05 — bump postcss #256 (mergé) |
| #1322 | 10/09 | `baseline-browser-mapping` 2.10.12→2.11.22 (mineur) | `2sxc/QrCode2/package-lock.json` **seul** | `09e0f07b1` 25/07 — bump json5 #891 (mergé) |

## §1 Le classement demandé (post-tag / re-clore / à revoir)

**Les 3 = post-tag. Aucune re-clore, aucune à revoir.** Motif, triple et mesurable :

1. **Lock-only, dev-deps de builds jamais lancés** : les 3 PRs ne touchent que le `package-lock.json`
   (la range `package.json` ne bouge pas). Et **0 des 5 workflows CI ne contient `npm`** (mesuré :
   `build`/`healthcheck`/`large-blob-guard`/`mindmap-wrapper-behaviour`/`static` — aucun
   `npm install` sur `DNNPlatform/`). Ces builds ne tournent ni en CI ni en local : le lock est
   **inert**. C'est la surface exacte que #942 §1 avait mesurée : 380/383 alertes dependabot du dépôt
   = devDeps d'arbres vendored `DNNPlatform/Portals/**`, **0 alerte sur une surface livrée**.
2. **Le consommateur est lui-même post-tag** : ces dev-deps ne serviraient qu'au build front des
   skins/modules 2sxc — le go-live DNN #131/#132, gated sur le tag v2.0.0. Merger maintenant
   n'achèterait rien ; attendre ne coûte rien (les PRs ne sont pas re-bumpées depuis le 10-11/09).
3. **Pas de motif de fermeture** : le précédent du dépôt est de **merger** ces bumps — #885, #891 et
   #256 sont les bumps Dependabot immédiatement antérieurs **aux mêmes chemins**, tous mergés.
   Fermer #1328/#1323/#1322 créerait une exception sans motif (aucune ne touche le binaire du
   pipeline, l'instrument de QA, ou un pin de licence — vérifié par #1384 §1).

Le lien avec le **§2 réel de #942** (l'angle mort vendored) : les 3 PRs sont l'illustration inverse
du problème — Dependabot voit les `package.json`/locks que nous n'exécutons pas, et ne voit pas les
libs vendored que les cartes exécutent (`CardPen/lib/*.js`, dossier clos #965). Les deux faces du
même écart de visibilité ; l'inventaire présent couvre la face « gelées », la face vendored est
traitée (verdict RESTER, sha immobile).

## §2 Ce que cet inventaire n'établit pas

⛔ Aucune action sur les PRs (merge/close/commentaire) — la décision est ai-01/owner · ⛔ la file
peut bouger avant le tag (Dependabot rouvre périodiquement — re-trier au moment du tag si la fenêtre
dure, cf. #1384 §4) · ⛔ n'audite pas les locks au-delà des PRs ouvertes · ⛔ le sort post-tag
(merger les 3 d'un bloc au go-live DNN vs les laisser pourrir jusqu'à re-bump) est **nommé, pas
tranché** — reco worker : merger d'un bloc quand #131/#132 s'ouvre.

---
*po-2024 — pool #458 renouvelé (c.5666260217), grain ⑧. Enchaîne #1384 (tri pré-tag) et #942 §1-2
(surface non livrée / angle mort vendored) — la file Dependabot DNNPlatform est désormais classée
intégralement.*

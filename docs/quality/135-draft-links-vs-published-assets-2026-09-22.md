# #135 — Liens des brouillons vs assets publiés : chaque lien résolu (MESURÉ 22/09)

**Date** : 2026-09-22 ~08:40 · **Lane** : po-2023 (worker) · **Base** : `origin/master` `762848a6`
**Nature** : mesure seule — ⛔ aucune publication DNN (gel #972), aucun tag, aucun upload, aucune retouche
des brouillons. Graine du grain : *« un artefact validé et le canal qui le sert sont deux objets »*.
**Périmètre** : les **4 brouillons** de la famille PR #1386 (15/09) — `news-article-v2.0.0.{fr,en}.md` +
`downloads-v2.0.0.{fr,en}.md` (l'article pointe vers la page Téléchargements ; les liens de téléchargement
vivent dans les deux).

---

## §1 — Chaque lien que les brouillons portent, résolu (code HTTP + taille constatés)

### Liens HTTP (2 distincts)

| Lien | Porté par | Résolu le 22/09 | Ce que le destinataire obtient |
|---|---|---|---|
| `https://github.com/ArgumentumGames/Argumentum/releases` | news fr (1×) · news en (1×) | **200**, 249 466 o (HTML) | **2 pré-releases publiques** : `v2.0.0-review` (80 assets, 18/09) et `v0.9.0-review` (64 assets, 13/08). La 3ᵉ (`v2.0.0-rc1`, 9 assets) est **DRAFT** → invisible publiquement |
| `https://github.com/ArgumentumGames/Argumentum/issues/135` | les 4 brouillons | **200**, 305 384 o (HTML) | ce ticket |

### Liens relatifs (22 occurrences, 12 cibles distinctes) — toutes résolues depuis `docs/publication/`

| Cible | Occurrences | État sur `origin/master` |
|---|---|---|
| `../RELEASE-NOTES-v2.0.0.md` (= `docs/RELEASE-NOTES-v2.0.0.md`) | 6 | **PRÉSENT** |
| `../release-dossier/134-release-assets-drift-2026-09-15.md` | 8 | **PRÉSENT** |
| `../release-dossier/134-dossier-validation-mesures.md` | 2 | **PRÉSENT** |
| `cards-catalog.{fr,en}.md` | 10 | **PRÉSENT** |
| `news-article-v2.0.0.{fr,en}.md` (croisés) | 6 | **PRÉSENT** |
| `news-article-v0.9.0.{fr,en}.md` (historiques) | 2 | **PRÉSENT** |
| `downloads-v2.0.0.{en,fr}.md` (miroirs) | 3 | **PRÉSENT** |

⚠️ Note d'instrument : vérifier `../RELEASE-NOTES-v2.0.0.md` **depuis `docs/publication/`** (cible =
`docs/RELEASE-NOTES-v2.0.0.md`) ; résolu depuis la racine du repo, il paraît « absent ». Chemin relatif ≠
chemin absolu.

### Les `[PLACEHOLDER]` d'URL (l'attendu du tag, pas encore des liens)

| Schéma attendu par les brouillons | Sondé le 22/09 | Constat |
|---|---|---|
| `releases/download/v2.0.0/…argumentum-{lang}-v2.0.0.zip…` | **404** (corps 9 o) | attendu : le tag `v2.0.0` n'est pas posé (gate #134) — ce 404 est **conforme**, pas un défaut |

## §2 — Le piège `&` → `.` : démontré vivant, quantifié sur le publié

Les brouillons nomment les documents `TarotCards_Print&Play_A4` etc. (table « les 10 documents ») ; la
checklist marque le nommage « à trancher au packaging ». **Le publié a déjà tranché** :

| Sonde (URL réelle, HEAD après redirection) | Code | Taille |
|---|---|---|
| `…/releases/download/v2.0.0-review/Argumentum_TarotCards_Print.Play_A4_fr.pdf` | **302 → 200** | **148 887 560 o** |
| `…/releases/download/v2.0.0-review/Argumentum_TarotCards_Print&Play_A4_fr.pdf` | **404** | 9 o |

- **32/32** PDF Print&Play publiés portent l'orthographe `Print.Play` (4 familles P&P × 8 langues,
  `v2.0.0-review` + `v0.9.0-review`) ; **0** nom publié ne contient `&`.
- Un lien construit depuis le nom de document **des brouillons** (`Print&Play`) → **404** ; depuis le nom
  **publié** (`Print.Play`) → **200**. C'est la panne que le grain demandait de ne pas laisser dormir.

## §3 — Ce que le canal sert réellement vs ce que les brouillons annoncent (écart mesuré)

| Constat | Mesure / dérivation | Statut |
|---|---|---|
| **`/releases` publie une pré-release que les brouillons ne mentionnent pas** | `v2.0.0-review` (80 assets, créée **18/09** — **postérieure** aux brouillons du 15/09) ; l'avertissement pré-release des brouillons ne vise que `v0.9.0-review` (24-25/08) | **MESURÉ** (dates + contenus via API releases) |
| Les assets publics `v2.0.0-review` **précèdent les fixes du 21/09** | release créée 18/09 ; merges #1469 (dos Memo fr) + #1474 le 21/09 ⇒ le PDF fr de la pré-release publique porte l'état **d'avant** ces corrections | **DÉRIVÉ** (dates de release vs merges ; le contenu des assets n'a pas été retéléchargé pour diff) |
| Les **zips** attendus par le schéma des brouillons (`argumentum-{lang}-v2.0.0.zip`) | publiés nulle part publiquement ; ils n'existent que dans la release **DRAFT** `v2.0.0-rc1`, sous un **autre nommage** (`Argumentum_v2.0.0-rc1_{lang}.zip`, 8 × ~476-519 Mo) | **MESURÉ** |
| Structure des assets publics `v2.0.0-review` | 80 PDF = 10 familles × 8 langues, **0 zip**, 0 manifeste public | **MESURÉ** |

## §4 — Ce que cette table n'établit pas

- Aucune **diff de contenu** asset-par-asset entre `v2.0.0-review` (18/09) et le bundle régénéré du 21/09 —
  l'écart §3 ligne 2 est dérivé des dates, pas vérifié octet par octet (téléchargement de 80 PDF hors scope).
- Le **choix de nommage** final (`Print.Play` vs `Print&Play`) reste au packaging (décision owner/ai-01) ;
  cette table mesure que le publié actuel est **uniformément `Print.Play`**.
- Les tailles de la table « par langue » des brouillons (PDF nus, MESURÉES 16/09 sur le bundle) ne sont pas
  re-mesurées ici — le packaging n'a pas eu lieu.
- ⛔ Aucune action : la publication reste GATED (#134 tag / #132 deploy / #131 DNN).

---
*po-2023 (worker lane) · instruments : `gh api` releases (noms+tailles+dates) · `curl` HEAD/GET (codes HTTP,
Content-Length) · résolution de liens relatifs contre `origin/master` · verdict/arbitrage : ai-01 & jsboige*

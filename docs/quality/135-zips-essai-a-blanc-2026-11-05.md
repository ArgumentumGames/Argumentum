# #135 — Zips de téléchargements : script + essai à blanc (2026-11-05)

> Pool #458 c.6657549140, grain po-2023 « #135, les zips : le script avec un essai à blanc ».
> Table de correspondance **décidée** : [#135 c.6650903268](https://github.com/ArgumentumGames/Argumentum/issues/135)
> (ai-01, 05/11). Ce document mesure ; il ne publie rien et ne construit rien — le build a lieu
> **au tag**, sur l'arbre de la release finale.

## L'instrument

`tools/135-build-download-zips.py` — stdlib seul, dry-run par défaut.

- **Faces attendues DÉRIVÉES du CSV** (colonne `carte` non vide = **175**), jamais codées en dur :
  le compte fait partie du contrat, il bouge avec le CSV (#1288).
- `--build` **refuse** (exit 2) sur toute entrée manquante ou tout écart de compte — le dry-run,
  lui, rend son verdict sans échouer : un avertissement du dry-run est une information sur l'arbre.
- `--self-test` : corpus synthétiques à trous connus, **joué PASS avant toute mesure** —
  SVG manquant, dos manquant, écart de faces, `density-1` incohérente : **tous signalés**,
  langue saine jamais signalée. Un instrument qui ne sait pas rougir ne mesure rien.

## Essai à blanc — arbre local Release (regen du 22/08, PO-2023)

⚠️ L'arbre mesuré ici est **local et antérieur au tag** : les chiffres qualifient
l'instrument et la forme des paquets, pas la release. **Tout se re-mesure au tag.**

| Paquet | Verdict de l'essai |
|---|---|
| Cartes numériques (`density-0/Fallacies`) | **176 faces sur les 8 langues = +1 vs CSV (175)** — l'arbre local est **pré-#1288** (il porte encore le doublon PK 96). Dos `card_001.png` présent partout. Le garde bloque un `--build` : **correct**. Taille brute par langue : 35,9–42,8 Mo (PNG déjà déflatés — le zip ne change pas l'ordre de grandeur). |
| Cartes légères (`density-1`) | **ABSENTE sur les 8 langues** — verdict tranché par la table : le paquet **disparaît**, `Fallacies_Web_Thumbnails_A4` le remplace. C'est maintenant un fait mesuré sur l'arbre, pas une supposition. |
| Cartes mentales (SVG du dépôt, 5/langue) | **5/5 présents sur les 8 langues** — 3 Fallacies (main + `content` + `links`) + 2 Virtues. Taille par langue : 8,0–8,8 Mo (zh : **17,9 Mo**, vignettes CJK plus lourdes). Total 8 langues ≈ **76,0 Mo**. |

Décisions de packaging laissées à blanc volontairement (à trancher au tag) :
la variante FR-only `Argumentum_Fallacies_MindMap_cards_fr` (triplet complet) n'est **pas**
dans le paquet — la table dit « SVG Fallacies et Virtues des 8 langues », la variante cards
est un objet différent ; et le `.mm` source (`fallacy_map.mm`, racine Mindmaps) n'est pas
inclus non plus (l'ancien zip le portait — à décider avec ai-01 au packaging).

## Au tag (po-2023)

1. `--self-test` (PASS obligatoire), puis dry-run sur l'**arbre de la release finale** ;
2. si `hard_failures` = 0 : `--build --out <dir>` → 8 zips cartes + 8 zips mindmaps
   (les zips light n'existent que si l'arbre porte une `density-1` — re-mesuré) ;
3. relever les tailles servies → remplir les `[PLACEHOLDER]` des brouillons
   `docs/publication/downloads-v2.0.0.{fr,en}.md` ; ai-01 joint à la release finale ;
4. ⛔ aucun upload vers `v2.1.0-review` ou `v2.0.0-review` — release finale #134 seulement.

*po-2023*

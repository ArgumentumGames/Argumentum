# Passe FreeMind #2025 — mesure de la régénération des cartes mentales (30/10/2026)

> Période : 30/10/2026, ~14:07 → 14:59. Lane worker (po-2023).
> Aucun artefact committé par cette passe : elle écrit **uniquement dans l'arbre de build**
> d'un worktree détaché (arbre gelé `341ef2a3`). Rien sous `Cards/**` n'a été touché
> (gel CSV #2025 respecté). La **livraison** vers `Cards/**` reste une décision d'ai-01.

## Procédure exécutée

Une passe = **une langue**, un tir, dans l'ordre des ancres :

1. Sonde de bureau (session, `OpenInputDesktop`, foreground non nul, inactivité owner ≥ 120 s) —
   une passe ne part jamais sans écran vivant.
2. Verrou anti-double-tireur dans l'état partagé (< 30 min ⇒ une seule passe à la fois).
3. Sauvegarde des deux fichiers de config + **revert par copie + sha256** (jamais `git checkout --`).
4. Patch par ancres sous-chaînes **uniques** (compteur attendu = 1, sinon rien n'est écrit) :
   `Mode=Mindmapper`, `OverwriteExistingHtmlMaps=true`, `Localization.Enabled=false`,
   `DefaultLanguage=<langue>`.
5. Build Debug incrémental, puis run (timeout 300–420 s).
6. Revert byte-exact + consignation de l'état.

## Résultat mesuré

**8 langues / 8 vertes** : fr, en, ru, pt, es, ar, fa, zh.

| Contrôle | Valeur mesurée |
|---|---|
| `Skip existing Html SVG Wrapper` (delta) | **0** sur les 8 runs |
| Résumé pipeline | « every expected SVG export was produced » **×8** |
| Repli XSLT | **jamais** engagé (« XSLT fallback never engaged ») |
| Export FreeMind par langue | **3** SVG exportés |
| Revert des configs | **byte-exact** sur les 8 passes |
| Worktree véhicule après passe | **propre**, sur la tête gelée |

### Un flake, un retry

`ru` a **échoué au premier tir** : FreeMind n'a pas produit le SVG `cards_ru`, le pipeline a
affiché son invite interactive « Press any key to resume and skip », et le run **non interactif**
a pendu jusqu'au timeout (aucun export, config revertée byte-exact, verrou levé).
**Le même retry a réussi** (run suivant : exit 0, tous les exports produits) — famille des
flakes Batik du 03/10, déjà documentée.

⇒ **Reconstruire la leçon** : un export FreeMind raté ne se manifeste **pas** par un exit code
propre mais par une **invite interactive** ; un run non interactif la transforme en **timeout**.
Le timeout n'est donc pas un « pipeline lent » : c'est le symptôme d'un export manquant.

## ⚠️ Deux instruments à corriger

### 1. Le motif de contrôle du DoD est cassé par le retour à la ligne du journal

Le motif `wrapper .* successfully saved` rend **0** sur les 8 runs, alors que les wrappers
**ont bien** été écrits. Cause : le journal console **wrappe à ~80 colonnes** et coupe le mot
« wrapper » **sur la ligne précédente** ; le motif doit porter sur une seule ligne.

Motif juste : `\.html successfully saved` → **6 par langue**.

Même famille que le piège déjà connu sur `FreeMind SVG exported` : compter **l'événement**, pas
la ligne qui le porte.

### 2. La surface de réécriture réelle est 48, pas 34

Le « 34 » de la CLAUDE.md est la **surface committée** (17 wrappers inlining + 17 `_ext`).
La **passe** écrit davantage, parce que la variante *cards* est produite **pour chaque langue** :

| Langue | Wrappers écrits | SVG écrits |
|---|---:|---:|
| fr … zh (chacune) | **6** | **8** |
| **Total passe** | **48** | **64** |

Les 6 wrappers par langue : `Fallacies_<lang>` + `Fallacies_<lang>_ext` + `Fallacies_cards_<lang>`
+ `Fallacies_cards_<lang>_ext` + `Argumentation_Virtues_<lang>` + `Argumentation_Virtues_<lang>_ext`.

⇒ **Un « 34 » vert et un « 48 » vert ne se contredisent pas** : ce sont deux surfaces
différentes (ce qui est committé / ce que la passe réécrit). Comparer l'un à l'autre est un
faux discriminant.

### 3. L'état partagé écrit `exports = 1` quand l'export réel est 3

Écriture de l'état : `exports = $exports.Count` sur un **scalaire** ⇒ `1` quel que soit le
nombre réel. Le compte vrai est dans le champ voisin (`files`). **Lire `files`**, pas `exports`,
pour décider d'un critère de mort.

## Suite

- **Livraison** vers `Cards/**` : **non faite**, décision d'ai-01 (gel #2025 ; et la surface
  à livrer est celle de la surface *committée* — 34 — pas les 48 de l'arbre de build).
- Les artefacts de la passe restent dans l'arbre de build du worktree détaché (à conserver
  jusqu'à la livraison).
- **Prochaine tire** (ordre c.6522990068) : SharedResources ×3 (ar/fa/zh), 8 clés GR,
  caractérisation du flap 401, test d'import en **application de test** (jamais l'app 60).

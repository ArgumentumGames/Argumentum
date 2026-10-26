# #1502 — R-site, phase écriture : les 6 pages Règles de la préprod sont synchronisées aux cartes (2026-10-09)

Exécution du plan §3 de [`1502-r-site-mesure-et-plan.md`](1502-r-site-mesure-et-plan.md)
(arbitrage #1502 c.`5665864605` : **les cartes font foi**). Toutes les écritures ci-dessous
sont sur la **préprod** (`dnn.argumentum.myia.io`) ; la prod n'a pas été touchée ; le CSV
n'a pas été modifié.

## Ce qui a été écrit

22 valeurs régénérées dans `TsDynDataValue` (app 60, zone 3) pour les 5 entités
Game Rule (École 11378, Bingo 11380, Beau 11387, Moulin 11388, Parlote 11389) :
`Summary`, `Material`, `Installation`, `Content`, `Variants` selon présence.

- **Squelette = HTML stocké** : les têtes de sections restent byte-identiques au
  stocké (l'autorité structurelle), seuls les corps sont régénérés de la colonne
  `Text` du CSV master (`origin/master`). Les 18 attributs `data-sheets*` (junk
  Google Sheets hérité du copier-coller initial) sont strippés au passage :
  15 732 → 3 854 caractères pour le Content École, sans perte de texte visible.
- **Appariement par `nkey`** (celui de l'instrument) : chaque tête stockée trouve
  sa section carte ; tête sans contrepartie → corps conservé (web-only) ; section
  carte sans tête → append uniquement si ce n'est pas la ligne compteur
  (« *Règles du jeu : … joueurs* », qui vit sur la landing — mesurée présente sur
  les 4 variantes non-École, jamais appendue).
- **Exceptions nominatives respectées** (plan §3) : items 1 (« A 5 joueurs et plus… »)
  et 5 (« Le baratineur peut poser plusieurs cartes… ») du `Variants` École conservés
  **byte-identiques** (garde positionnelle dans le générateur — abort si l'item stocké
  ne commence pas par la phrase attendue) ; `Memo` **non réécrit**.

## Sécurité de l'écriture

| Étape | Preuve |
| --- | --- |
| Sauvegarde pré-écriture | `rsite-backup-20261009-085550.json`, 52 valeurs, sha256 `c6aa1a89…30d3ad` (scratchpad lane, jamais committé) |
| Anti-mutation-concurrente | chaque UPDATE précédé d'une relecture : valeur actuelle = backup, sinon abort transaction |
| Transaction | tout-ou-rien, 22/22 commitées, **0 écart** en relecture post-commit octet à octet |
| Recycle app-domain | mtime de `web.config` touchée (sha256 inchangé) — le monitor ASP.NET recycle seul ; 1ʳᵉ requête à froid ~33 s, comportement connu |
| Valeurs écrites | sha256 `7c740062…ccdd56` (générateur `--apply`) |

## DoD — rejeu de l'instrument sur la préprod (après correctif autojunk, cf § artefacts)

- **43 paires**, appariement structurel intact.
- **Compteurs joueurs 5/5 « identique »** — y compris École **3-10** : l'écart
  attendu #1502 (« site à re-synchroniser ») est **résorbé**.
- **Aucun verdict « reecrit »**. Matrice : tout `IDENT` sauf :
  - `Variantes` École **0.949 proche** = les 2 items owner (l'exception nominative
    elle-même — le résidu restant est exactement la décision conservée) ;
  - `Materiel` 0.983-0.988 et 3 sections à 0.984-0.989 = l'**artefact "- " des
    `<li>`** (mesuré : 1.000 en comparaison sans les tirets, diff caractérisé :
    uniquement les insertions "- " de `text_of`).
- Sections web-only préservées : `Contenu` ×5, `Carte Mémo`, `Conditions de
  Victoire` + `Nombre de pioches` (École).
- Ancres `sim=0.000 ancre` : titres parents vides des deux côtés (`Déroulé de la
  manche`, `Début de la manche`) — tagués `ancre` par l'instrument, pas des écarts.

## Artefacts d'instrument découverts pendant la phase (correctif embarqué)

1. **`SequenceMatcher` autojunk** — l'heuristique par défaut (`autojunk=True`)
   marque comme « junk » tout caractère ≥1 % de `b` dès 200 caractères, soit
   toutes les lettres fréquentes du français. Mesuré sur `Variantes` École :
   **0.109** en autojunk pour **0.949** en `autojunk=False` — deux textes à
   contenu quasi identique (les items 2-4 et 6 inchangés). Le défaut ne se
   déclenche que si la divergence est **en tête** (les items owner 1 et 5) :
   contrôle moulin (divergence en queue) 0.994 dans les deux modes. Correctif :
   `sim()` passe `autojunk=False` dans l'instrument. Aucun verdict légitime
   dégradé au rejeu.
2. **Préfixe "- " des `<li>`** (déjà documenté dans le plan §4) : `text_of`
   fabrique un tiret par item de liste ; sur les sections à puces ça plafonne
   la sim vers ~0.98. Confirmé exhaustivement : les diffs des 3 sections
   résiduelles (Les pioches, Fin de partie et décompte, Conditions de victoire)
   ne contiennent **que** des insertions "- ".

## Gates

❌ aucune écriture prod · ❌ aucune modification CSV/gabarit/CardPen · ❌ zéro
donnée personnelle (contenu de règles de jeu uniquement) · ❌ pas de verdict QA
(ai-01) · préprod recyclée quelques secondes lors du geste (annoncé au
dashboard).

## Suivi

- La mesure d'avant-écriture et le plan : PR #1833 (branche `docs/1502-rsite-mesure`).
- Contrôle visuel humain des 6 pages : à la discrétion d'ai-01/jsboige.
- Écart résiduel connu et voulu : les 2 items owner du `Variants` École
  (restent à arbitrer au dossier R-cartes, owner-gated).

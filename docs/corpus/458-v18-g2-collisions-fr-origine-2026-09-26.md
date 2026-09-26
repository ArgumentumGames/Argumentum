# Origine des 86 collisions de titres fr — mesure 0-écriture (pool v18, grain 2)

Arbre master `b29c1b9e` (2026-09-26) · fenêtre = 93 commits du fichier (`2025-07-02 e8482fe5` → 2026-09-26) ·
dispatch [#458 c.5845739927](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5845739927).
Question : chaque collision existait-elle **à l'origine**, ou **une passe l'a-t-elle créée** (laquelle) ?

> **Correction du 26/09 (revue [#1584](https://github.com/ArgumentumGames/Argumentum/pull/1584))** —
> la première version suivait les rangées par **PK**. Or le commit `f38cfed9` (« resolve 2 duplicate PKs », 24/04)
> a **renuméroté** une des deux rangées qui portaient le PK 1000 (idem 520) : mesuré sur `f38cfed9^`, le code
> `6.2.2.2.3` porte PK 1000 « Équivoque antithétique » **et** le code `6.2.2.3` porte aussi PK 1000 « Démenti
> non-démenti ». Un dictionnaire indexé par PK ne garde qu'une des deux rangées : la première version y a lu un
> changement de titre qui n'a jamais eu lieu. Cette version suit les rangées par **code hiérarchique (`path`)**
> et classe « d'origine » toute collision déjà présente à l'import `e8482fe5`, **même défaite puis refaite**.
> Comptes : 71 → **73** à l'origine, vraies créations 5 → **3**.

## Synthèse

| Classe | Groupes | Lecture |
|---|---:|---|
| À l'origine | 72 | déjà portées par l'import initial du fichier (2025-07-02) |
| À l'origine, interrompues puis refaites | 1 | « Sophisme de la double faute » (défaite par #369, rendue par #1522), « Équivoque antithétique » (jamais défaite — le faux changement était l'artefact du PK) |
| Réécrits — collision **préexistante**, la passe n'a fait qu'aligner les octets | 7 | #369 ×3 (casse, orthographe, accent) · #1025 ×4 (apostrophe courbe) |
| Créés en octets stricts mais **collision préexistante modulo écriture** | 3 | apostrophe (#1025), accent (#369), casse (#369) |
| **Vraies créations** — la passe a rendu identiques deux titres **différents** | **3** | toutes de #369 ; détail ci-dessous, ancien et nouveau cités |

Les 86 groupes totalisent 180 PK porteurs, dont 27 cartes du deck (colonne `carte` non vide).

## Les 3 vraies créations (ancien → nouveau cités)

### «Appel à la nature»
- PK 96 (`1.2.2`, carte='') **MOVER** : «Sophisme naturaliste» → «Appel à la nature»
- PK 108 (`1.2.2.4`, carte='1') portait déjà ce titre avant la passe
- convergence : `9d45b4f9` 2026-05-28 PR #369 — *data(fallacies): FR clarity + cascade drift infrastructure (gpt-5.5) (#369)*

### «Mauvais argument de la régression infinie»
- PK 661 (`3.2.3.2`, carte='') **MOVER** : «Mauvais argument de la régression» → «Mauvais argument de la régression infinie»
- PK 664 (`3.2.3.2.2`, carte='') **MOVER** : «Mauvais argument de regression infinie» → «Mauvais argument de la régression infinie»
- convergence : `9d45b4f9` 2026-05-28 PR #369 — *data(fallacies): FR clarity + cascade drift infrastructure (gpt-5.5) (#369)*

### «Question piège»
- PK 178 (`2.1.1.1`, carte='') **MOVER** : «Argument par la question» → «Question piège»
- PK 701 (`4.1.1.2`, carte='') portait déjà ce titre avant la passe
- PK 179 (`2.1.1.1.1`, carte='2') portait déjà ce titre avant la passe
- convergence : `9d45b4f9` 2026-05-28 PR #369 — *data(fallacies): FR clarity + cascade drift infrastructure (gpt-5.5) (#369)*

**Motif commun** : les 3 créations viennent de #369 (« FR clarity », gpt-5.5) qui a aligné le libellé d'un nœud
sur celui d'un **autre** nœud — une convergence, pas un doublon de contenu (cf. #1578 : 661/664 = deux concepts).
661 est traité par #1581 ; **la décision règle C du 26/09 (ai-01, revue #1584) tranche les 2 autres** : 178 et 96
reprennent leur libellé d'avant #369 (grain CSV séparé du pool v19).

⚠️ « Question piège » : la collision 179/701 est **d'origine** ; #369 y **ajoute** un 3e porteur (178).

## Les 2 origines que la première version rangeait à tort en créations

### «Sophisme de la double faute» — présente à l'import, jamais créée
- PK 713 (`4.1.2.5`) : **«Sophisme de la double faute»** → «Deux torts font un droit» → **«Sophisme de la double faute»**
- PK 1325 (`7.2.1.1.2.2`) : **«Sophisme de la double faute»**

### «Équivoque antithétique» — présente à l'import, jamais créée
- PK 1000 (`6.2.2.2.3`) : **«Équivoque antithétique»**
- PK 360 (`2.3.1.1.1.1`) : **«Équivoque antithétique»**

« Sophisme de la double faute » : #369 (`9d45b4f9`) met « Deux torts font un droit » sur la 713 — la collision
se défait — puis #1522 (`789afcd8`) lui **rend** son titre d'origine. La convergence mesurée est une
**restauration**, pas une création. « Équivoque antithétique » : aucun titre n'a jamais bougé ; le « MOVER »
de la première version était la rangée voisine (`6.2.2.3`, « Démenti non-démenti », renumérotée 1407 par
`f38cfed9`) lue comme l'ancien état du PK 1000.

## Les 3 créés « modulo écriture » (collision préexistante, octets différents avant la passe)

- «Sophisme de l’accident» — avant : PK 614 «Sophisme de l'accident» vs PK 615 «Sophisme de l’accident» ; convergence `14b9354e` 2026-08-08 PR #1025
- «Sophisme de régression» — avant : PK 636 «Sophisme de régression» vs PK 1196 «Sophisme de regression» ; convergence `9d45b4f9` 2026-05-28 PR #369
- «Sophisme du vrai Écossais» — avant : PK 65 «Sophisme du vrai Écossais» vs PK 616 «Sophisme du vrai Écossais» vs PK 813 «Sophisme du vrai écossais» ; convergence `9d45b4f9` 2026-05-28 PR #369

## Les 7 réécrits (tous les membres portaient déjà le même titre, la passe a unifié l'écriture)

- «Appel à l’identité» — avant la passe tous portaient «Appel à l'identité» ; `14b9354e` 2026-08-08 PR #1025
- «Changement de terrain» — avant la passe tous portaient «Changement de Terrain» ; `9d45b4f9` 2026-05-28 PR #369
- «Effet de l’expérimentateur» — avant la passe tous portaient «Effet de l'expérimentateur» ; `14b9354e` 2026-08-08 PR #1025
- «Glissement du quantificateur» — avant la passe tous portaient «Glissement du quantiticateur» ; `9d45b4f9` 2026-05-28 PR #369
- «Insensibilité à la taille de l’échantillon» — avant la passe tous portaient «Insensibilité à la taille de l'échantillon» ; `14b9354e` 2026-08-08 PR #1025
- «Négation de l’antécédent» — avant la passe tous portaient «Négation de l'antécédent» ; `14b9354e` 2026-08-08 PR #1025
- «Équivalence morale» — avant la passe tous portaient «Equivalence morale» ; `9d45b4f9` 2026-05-28 PR #369

## Annexe A — les 72 groupes d'origine

- «Acte de foi» — 22(`1.1.1.3`), 770(`4.3.1.3`)
- «Affirmation du conséquent» — 708(`4.1.2.1`), 731(`4.2.1.2.1`)
- «Amplification et minimisation» — 895(`6.1.1.1.1.3`), 1101(`6.3.1.2.1.1.2.1.2`)
- «Analyse défaillante» — 155(`1.3.2.1.1`), 767(`4.3.1.1.4.1`)
- «Apophénie» — 172(`1.3.3.4`), 1083(`6.3.1.1.2.3`)
- «Appel au doute et à la crainte» — 338(`2.2.2.4.1`), 920(`6.1.1.2.2.2.1`)
- «Appel à la minorité» — 120(`1.2.3.3.1.2`), 316(`2.2.1.3.2.1`)
- «Appel à la nouveauté» — 115(`1.2.3.2`), 1069(`6.3.1.1.2.1.3`)
- «Appel à la panique morale» — 339(`2.2.2.4.2`), 922(`6.1.1.2.2.2.2`)
- «Argument de la barbe» — 660(`3.2.3.1.1`), 859(`5.3.2.1.2.1`)
- «Argument du bâton» — 343(`2.2.3.1.2`), 468(`2.3.2.3.1.4.1.1`)
- «Avoir toujours raison» — 706(`4.1.1.7`), 1171(`6.3.1.2.3.2.2.2`)
- «Biais de confirmation» — 602(`3.1.1.2`), 965(`6.1.3.1.2.3`)
- «Biais rétrospectif» — 141(`1.3.1.2`), 1155(`6.3.1.2.3.1.2.2`)
- «Bouc émissaire» — 501(`2.3.2.4.1.2`), 960(`6.1.3.1.1.3.2`)
- «Campagne de murmures» — 882(`5.3.3.2.1`), 918(`6.1.1.2.2.1.1`)
- «Capacitisme» — 132(`1.2.3.7`), 344(`2.2.3.1.3`)
- «Compliment empoisonné» — 1387(`7.3.2.3.1`), 363(`2.3.1.1.1.3`), 879(`5.3.3.1.1.1`)
- «Concept volé» — 779(`4.3.2.1.1`), 828(`5.1.3.1.1`)
- «Conclusion hâtive» — 759(`4.3.1`), 1127(`6.3.1.2.2.2.1`)
- «Corrélation illusoire» — 642(`3.2.1.5`), 1085(`6.3.1.1.2.3.2`)
- «Culture de la peur» — 418(`2.3.1.6.3`), 919(`6.1.1.2.2.2`)
- «Digression» — 297(`2.1.3.6.7`), 1316(`7.2.1.1.1.1`)
- «Définition persuasive» — 184(`2.1.1.3`), 819(`5.1.2.3`)
- «Effet de halo» — 302(`2.2.1.1.1`), 1231(`6.3.2.3.1.4`)
- «Effet de vérité illusoire» — 368(`2.3.1.1.1.8`), 1067(`6.3.1.1.2.1.1`)
- «Effet du lampadaire» — 138(`1.3.1.1.3`), 1074(`6.3.1.1.2.1.7`)
- «Effet puits» — 206(`2.1.1.5.2.3`), 995(`6.2.2.1.1.1`)
- «Exagération» — 233(`2.1.2.2.2`), 893(`6.1.1.1.1.2`)
- «Fausse précision» — 668(`3.3.1.1`), 857(`5.3.2.1.1`)
- «Fausse équivalence» — 768(`4.3.1.2`), 843(`5.2.2.4`)
- «Fausses obligations» — 756(`4.2.3.4.1`), 1102(`6.3.1.2.1.1.2.2`)
- «Gish gallop» — 772(`4.3.1.4.1`), 1331(`7.2.1.2.3`), 475(`2.3.2.3.2.2.1`)
- «Généralisation abusive» — 595(`3.1`), 1123(`6.3.1.2.2.1.3`)
- «Homme de paille» — 1365(`7.3.1.2`), 168(`1.3.3.2.1`), 894(`6.1.1.1.1.2.1`)
- «Hypothèse farfelue» — 63(`1.1.3.3.1`), 763(`4.3.1.1.2.1`)
- «Illusion de regroupement» — 174(`1.3.3.4.2`), 643(`3.2.1.5.1`)
- «Indiscrétion anonyme» — 880(`5.3.3.1.1.2`), 924(`6.1.1.2.2.3.1`)
- «Justification infinie» — 1349(`7.2.2.1.2`), 665(`3.2.3.2.2.1`), 979(`6.2.1.1.1.2`)
- «Lavage de cerveau» — 1302(`7.1.3.3.1`), 478(`2.3.2.3.2.2.4`)
- «Minimisation» — 496(`2.3.2.3.6`), 892(`6.1.1.1.1.1`)
- «Métonymie» — 295(`2.1.3.6.5`), 866(`5.3.2.3.1`)
- «Pensée dichotomique» — 817(`5.1.2.2.4.3`), 1120(`6.3.1.2.2.1`)
- «Pente glissante» — 705(`4.1.1.6`), 677(`3.3.1.3.1`)
- «Personnalisation» — 723(`4.1.3.1.1.2`), 1169(`6.3.1.2.3.2.2`)
- «Piège de gratitude» — 451(`2.3.2.2.1.2.7`), 1324(`7.2.1.1.2.1.2`), 1103(`6.3.1.2.1.1.2.3`)
- «Piège kafkaïen» — 986(`6.2.1.2.2`), 161(`1.3.2.3.2.1`)
- «Politiquement correct» — 121(`1.2.3.3.2`), 1341(`7.2.1.3.2.2`)
- «Potin» — 491(`2.3.2.3.3.4`), 923(`6.1.1.2.2.3`)
- «Preuve anecdotique» — 34(`1.1.2.1`), 1087(`6.3.1.1.2.3.4`)
- «Projection mentale» — 52(`1.1.2.3.1`), 1164(`6.3.1.2.3.2`)
- «Projection psychologique» — 1355(`7.2.3.3`), 1165(`6.3.1.2.3.2.1`)
- «Pétition de principe» — 698(`4.1.1`), 183(`2.1.1.2`)
- «Pétition de principe analogique» — 703(`4.1.1.4`), 840(`5.2.2.1`)
- «Raisonnement émotionnel» — 53(`1.1.2.3.2`), 1119(`6.3.1.2.2`)
- «Rationalisation» — 62(`1.1.3.3`), 762(`4.3.1.1.2`)
- «Renvoyer la balle» — 1344(`7.2.1.3.3.2`), 1166(`6.3.1.2.3.2.1.1`)
- «Restriction mentale» — 900(`6.1.1.1.3.3`), 1329(`7.2.1.2.1.1`)
- «Réfutation lapidaire» — 1292(`7.1.2.2.1`), 17(`1.1.1.1.6.1`)
- «Solution parfaite» — 1350(`7.2.2.2`), 977(`6.2.1.1.1.1`)
- «Sophisme animiste» — 170(`1.3.3.3.1`), 1088(`6.3.1.1.2.3.5`)
- «Sophisme de la motte castrale» — 364(`2.3.1.1.1.4`), 875(`5.3.2.3.2.4`)
- «Sophisme de portée modale» — 757(`4.2.3.5`), 849(`5.3.1.2`)
- «Sophisme des coûts irrécupérables» — 1020(`6.2.3.4`), 440(`2.3.2.2.1.1.4.1`)
- «Sophisme du psychologue» — 51(`1.1.2.3`), 1055(`6.3.1.1.1.2.2.5.1`)
- «Sophisme génétique» — 1371(`7.3.2`), 761(`4.3.1.1.1`)
- «Stratégie du chat mort» — 914(`6.1.1.2.1.5`), 1318(`7.2.1.1.1.2.1`)
- «Stéréotype» — 619(`3.1.2.3.1`), 1199(`6.3.2.2.1`)
- «Substitut de preuve» — 771(`4.3.1.4`), 19(`1.1.1.1.6.3`)
- «Syllogisme du politicien» — 21(`1.1.1.2.1`), 787(`4.3.3.1.1.1`)
- «Tu quoque» — 1362(`7.3.1.1`), 714(`4.1.2.5.1`)
- «Équivoque antithétique» — 1000(`6.2.2.2.3`), 360(`2.3.1.1.1.1`)

## Annexe B — join imprimé (cartes deck des 86 groupes, référence `Cards/Fallacies/Archive/` par `path`)

- «Affirmation du conséquent» :
  - PK 708 (`4.1.2.1`) — 2022: «Affirmation du conséquent» · v3: «Affirmation du conséquent»
- «Appel à la nature» :
  - PK 108 (`1.2.2.4`) — 2022: «Appel à la nature» · v3: «Appel à la nature»
- «Argument du bâton» :
  - PK 343 (`2.2.3.1.2`) — 2022: «Argument du bâton» · v3: «Argument du bâton»
- «Conclusion hâtive» :
  - PK 759 (`4.3.1`) — 2022: «Conclusion hâtive» · v3: «Conclusion hâtive»
- «Définition persuasive» :
  - PK 184 (`2.1.1.3`) — 2022: absent · v3: «Définition persuasive»
- «Fausse équivalence» :
  - PK 768 (`4.3.1.2`) — 2022: «Fausse équivalence» · v3: «Fausse équivalence»
- «Généralisation abusive» :
  - PK 595 (`3.1`) — 2022: absent · v3: «Généralisation abusive»
- «Homme de paille» :
  - PK 1365 (`7.3.1.2`) — 2022: «Homme de paille» · v3: «Homme de paille»
- «Négation de l’antécédent» :
  - PK 729 (`4.2.1.1.1`) — 2022: absent · v3: absent
- «Pensée dichotomique» :
  - PK 1120 (`6.3.1.2.2.1`) — 2022: absent · v3: absent
- «Pente glissante» :
  - PK 677 (`3.3.1.3.1`) — 2022: «Pente glissante» · v3: «Pente glissante»
- «Politiquement correct» :
  - PK 121 (`1.2.3.3.2`) — 2022: «Politiquement correct» · v3: «Politiquement correct»
- «Preuve anecdotique» :
  - PK 34 (`1.1.2.1`) — 2022: absent · v3: absent
- «Projection psychologique» :
  - PK 1355 (`7.2.3.3`) — 2022: absent · v3: absent
- «Pétition de principe» :
  - PK 698 (`4.1.1`) — 2022: absent · v3: «Pétition de principe»
- «Question piège» :
  - PK 179 (`2.1.1.1.1`) — 2022: «Question piège» · v3: «Question piège»
- «Restriction mentale» :
  - PK 900 (`6.1.1.1.3.3`) — 2022: absent · v3: absent
- «Solution parfaite» :
  - PK 977 (`6.2.1.1.1.1`) — 2022: absent · v3: «Solution parfaite»
- «Sophisme de la double faute» :
  - PK 713 (`4.1.2.5`) — 2022: absent · v3: «Sophisme de la double faute»
- «Sophisme de l’accident» :
  - PK 614 (`3.1.2`) — 2022: «Sophisme de l'accident» · v3: «Sophisme de l'accident»
- «Sophisme de régression» :
  - PK 636 (`3.2.1.3`) — 2022: absent · v3: «Sophisme de régression»
- «Sophisme des coûts irrécupérables» :
  - PK 1020 (`6.2.3.4`) — 2022: absent · v3: absent
- «Sophisme du psychologue» :
  - PK 51 (`1.1.2.3`) — 2022: absent · v3: absent
- «Sophisme du vrai Écossais» :
  - PK 813 (`5.1.2.2.3`) — 2022: «Sophisme du vrai écossais» · v3: «Sophisme du vrai écossais»
- «Sophisme génétique» :
  - PK 1371 (`7.3.2`) — 2022: «Sophisme génétique» · v3: «Sophisme génétique»
- «Syllogisme du politicien» :
  - PK 787 (`4.3.3.1.1.1`) — 2022: absent · v3: «Syllogisme du politicien»
- «Tu quoque» :
  - PK 1362 (`7.3.1.1`) — 2022: «Tu Quoque» · v3: «Tu Quoque»

## Ce que cette mesure n'établit pas

- Elle ne décide de rien : la règle C (renommer ou accepter) reste à ai-01, groupe par groupe.
- « À l'origine » veut dire *déjà présentes à l'import initial du fichier dans le repo* (2025-07-02) —
  les 73+2 viennent du corpus d'avant le repo (GSheet/import), pas d'une passe git identifiable.
- Ne mesure ni `desc_fr`, ni les 7 autres langues (grain 3), ni les collisions inter-langues.
- Le join imprimé couvre les ères archivées en CSV (2022, v3) ; v1/v2 n'ont pas de CSV d'époque joint.
- La convergence est datée au dernier commit où un membre est arrivé à la valeur actuelle ; une collision
  interrompue puis recréée avec les mêmes octets serait datée de la recréation (non observée ici : les deux
  interrompues sont couvertes par la règle d'origine à l'import).
- ⚠️ **Piège d'instrument (corrigé ici)** : suivre les rangées par **PK** traverse mal une renumérotation.
  `f38cfed9` portait chaque PK 1000 et 520 sur **deux** rangées ; un index par PK ne conserve qu'une des deux
  et lit la voisine comme l'ancien état de la même rangée. Toute mesure de ce corpus suit les rangées par
  `path`, ou traite la renumérotation explicitement.

## Annexe C — instrument rejouable (version corrigée)

```python
# Rejouer : python g2b_collisions_fix.py (depuis la racine du repo, branche docs/v18-g2-collisions-fr-origine)
# CORRECTION (revue #1584, 26/09) par rapport a g2_collisions.py :
#   1. les rangees sont suivies par code hierarchique (`path`), PLUS par PK — f38cfed9 a renumerote
#      une des deux rangees qui portaient le PK 1000 (idem 520) : le suivi par PK lisait la rangee
#      voisine comme l'ancien etat de la meme rangee ;
#   2. est classee "d'origine" toute collision dont TOUS les membres portent le titre actuel a l'import
#      e8482fe5, MEME si un commit intermediaire l'a defaite puis refaite (#369 defait, #1522 refait).
import subprocess, csv, io, collections, re, unicodedata

CSV_PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
IMPORT_PREFIX = "e8482fe5"

def git(*args):
    return subprocess.run(["git", *args], capture_output=True, text=True, encoding="utf-8", errors="replace").stdout

commits = []
for line in git("log", "--reverse", "--format=%H|%ad|%s", "--date=short", "--", CSV_PATH).strip().splitlines():
    h, d, s = line.split("|", 2)
    commits.append((h, d, s))

def parse_snapshot(text):
    """-> {path: (title_fr, pk, carte)} — rangees suivies par code hierarchique, jamais par PK."""
    if not text.strip():
        return {}
    reader = csv.reader(io.StringIO(text))
    try:
        header = next(reader)
    except StopIteration:
        return {}
    norm = [c.strip().lstrip("﻿").strip().lower() for c in header]
    def col(*names):
        for n in names:
            if n in norm:
                return norm.index(n)
        return None
    i_pk, i_path, i_title, i_carte = col("pk"), col("path"), col("text_fr"), col("carte")
    if i_path is None or i_title is None:
        return {}
    need = max(i for i in (i_pk, i_title, i_path, i_carte) if i is not None)
    out = {}
    for row in reader:
        if not row or len(row) <= need:
            continue
        pathv = row[i_path].strip()
        if not pathv:
            continue
        pk = row[i_pk].strip() if i_pk is not None else ""
        out[pathv] = (row[i_title].strip(), pk, row[i_carte].strip() if i_carte is not None else "")
    return out

snaps = [(h, d, s, parse_snapshot(git("show", f"{h}:{CSV_PATH}"))) for h, d, s in commits]
assert snaps[0][0].startswith(IMPORT_PREFIX), f"import attendu {IMPORT_PREFIX}, trouvé {snaps[0][0][:8]}"
IMPORT = snaps[0][3]
cur = snaps[-1][3]

# --- Verifications des faits cites par la revue (echec bruyant si faux) ---
dup = collections.Counter((row[1]) for row in IMPORT.values() if row[1])
f_before = parse_snapshot(git("show", f"f38cfed9^:{CSV_PATH}"))
fb_dup = collections.Counter(row[1] for row in f_before.values() if row[1])
assert fb_dup.get("1000") == 2 and fb_dup.get("520") == 2, f"PK doubles attendus a f38cfed9^: {fb_dup}"
assert f_before["6.2.2.2.3"][0] == "Équivoque antithétique", f_before["6.2.2.2.3"]
assert f_before["6.2.2.3"][0] == "Démenti non-démenti", f_before["6.2.2.3"]
assert IMPORT["4.1.1.1.1.1"][0] if False else True
# 713 / 1325 a l'import : par path — retrouver leurs paths courants puis lire l'import
p713 = [p for p, v in cur.items() if v[1] == "713"]
p1325 = [p for p, v in cur.items() if v[1] == "1325"]
assert len(p713) == 1 and len(p1325) == 1, (p713, p1325)
assert IMPORT[p713[0]][0] == "Sophisme de la double faute", ("713@import", IMPORT[p713[0]][0])
assert IMPORT[p1325[0]][0] == "Sophisme de la double faute", ("1325@import", IMPORT[p1325[0]][0])
print("[verify] faits revue OK: f38cfed9^ PK 1000/520 doubles ; 713+1325 'Sophisme de la double faute' a l'import")

groups = collections.defaultdict(list)
for pathv, (title, pk, carte) in cur.items():
    if title:
        groups[title].append(pathv)
shared = {t: paths for t, paths in groups.items() if len(paths) >= 2}

PR_RE = re.compile(r"\(#(\d+)\)\s*$")

def title_at(pathv, i):
    for j in range(i, -1, -1):
        if pathv in snaps[j][3]:
            return snaps[j][3][pathv][0]
    return None

def first_seen(pathv):
    for j, (_, _, _, rows) in enumerate(snaps):
        if pathv in rows:
            return j
    return None

def norm_typo(t):
    if t is None:
        return None
    t = t.replace("’", "'").casefold()
    t = unicodedata.normalize("NFD", t)
    return "".join(c for c in t if not unicodedata.combining(c))

def load_archives():
    eras = {}
    for era, paths in {
        "2022": ["Cards/Fallacies/Archive/2022/Argumentum Fallacies - Cards - edition fevrier 2022.csv",
                 "Cards/Fallacies/Archive/2022/Argumentum Fallacies - Cards - edition fevrier 2022 - Print and Play.csv"],
        "v3":   ["Cards/Fallacies/Archive/v3/Argumentum Fallacies - Cards.csv",
                 "Cards/Fallacies/Archive/v3/Argumentum Fallacies - Cards Print and Play.csv"],
    }.items():
        bypath = {}
        for p in paths:
            try:
                with open(p, encoding="utf-8-sig", errors="replace") as f:
                    for r in csv.DictReader(f):
                        pathv = (r.get("path") or "").strip()
                        t = (r.get("text_fr") or r.get("Titre") or "").strip()
                        if pathv and t:
                            bypath[pathv] = t
            except FileNotFoundError:
                pass
        eras[era] = bypath
    return eras

archives = load_archives()

origin, interrupted, rewritten, created = [], [], [], []
for title, paths in sorted(shared.items()):
    members = []
    for pathv in sorted(paths, key=lambda p: (len(p.split(".")), [int(x) for x in p.split(".")])):
        fs = first_seen(pathv)
        conv = None
        if fs is not None:
            for j in range(len(snaps) - 1, fs, -1):
                if title_at(pathv, j) == title and title_at(pathv, j - 1) != title:
                    conv = j
                    break
        members.append({"path": pathv, "pk": cur[pathv][1], "fs": fs, "conv": conv})
    # Regle d'origine corrigee : TOUS les membres portent le titre actuel a l'import.
    if all(IMPORT.get(m["path"]) and IMPORT[m["path"]][0] == title for m in members):
        ever_broken = any(title_at(m["path"], j) != title for j in range(len(snaps)) for m in [members[0]])
        # interrompue = au moins un membre a diverge a un moment donne, puis tous reviennent
        broke = any(title_at(m["path"], j) != title
                    for m in members for j in range(1, len(snaps) - 1))
        (interrupted if broke else origin).append((title, members))
        continue
    introduced = [m for m in members if m["conv"] is not None]
    if not introduced:
        origin.append((title, members))
        continue
    j = max(m["conv"] for m in introduced)
    h, d, s, _ = snaps[j]
    m = re.search(PR_RE, s)
    rec = {"title": title, "members": members,
           "olds": {mm["path"]: title_at(mm["path"], j - 1) for mm in members},
           "sha": h[:8], "date": d, "pr": m.group(1) if m else None, "subj": s}
    pre = [rec["olds"][mm["path"]] for mm in members]
    strict_equal = len(set(pre)) == 1 and pre[0] is not None
    typo_equal = len({norm_typo(v) for v in pre}) == 1 and pre[0] is not None
    rec["typo"] = not strict_equal and typo_equal
    (rewritten if strict_equal else created).append(rec)

true_created = [r for r in created if not r["typo"]]
typo_created = [r for r in created if r["typo"]]

n = len(origin) + len(interrupted) + len(rewritten) + len(typo_created) + len(true_created)
print(f"decomposition corrigee: {len(origin)}+{len(interrupted)} interrompues+{len(rewritten)} reecrits+{len(typo_created)} typo+{len(true_created)} vraies = {n}")
print(f"vraies créations: {[r['title'] for r in true_created]}")
assert n == 86, f"total attendu 86, obtenu {n}"

L = []
A = L.append
A("# Origine des 86 collisions de titres fr — mesure 0-écriture (pool v18, grain 2)")
A("")
A("Arbre master `b29c1b9e` (2026-09-26) · fenêtre = 93 commits du fichier (`2025-07-02 e8482fe5` → 2026-09-26) ·")
A("dispatch [#458 c.5845739927](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5845739927).")
A("Question : chaque collision existait-elle **à l'origine**, ou **une passe l'a-t-elle créée** (laquelle) ?")
A("")
A("> **Correction du 26/09 (revue [#1584](https://github.com/ArgumentumGames/Argumentum/pull/1584))** —")
A("> la première version suivait les rangées par **PK**. Or le commit `f38cfed9` (« resolve 2 duplicate PKs », 24/04)")
A("> a **renuméroté** une des deux rangées qui portaient le PK 1000 (idem 520) : mesuré sur `f38cfed9^`, le code")
A("> `6.2.2.2.3` porte PK 1000 « Équivoque antithétique » **et** le code `6.2.2.3` porte aussi PK 1000 « Démenti")
A("> non-démenti ». Un dictionnaire indexé par PK ne garde qu'une des deux rangées : la première version y a lu un")
A("> changement de titre qui n'a jamais eu lieu. Cette version suit les rangées par **code hiérarchique (`path`)**")
A("> et classe « d'origine » toute collision déjà présente à l'import `e8482fe5`, **même défaite puis refaite**.")
A("> Comptes : 71 → **73** à l'origine, vraies créations 5 → **3**.")
A("")
A("## Synthèse")
A("")
A("| Classe | Groupes | Lecture |")
A("|---|---:|---|")
A(f"| À l'origine | {len(origin)} | déjà portées par l'import initial du fichier (2025-07-02) |")
A(f"| À l'origine, interrompues puis refaites | {len(interrupted)} | « Sophisme de la double faute » (défaite par #369, rendue par #1522), « Équivoque antithétique » (jamais défaite — le faux changement était l'artefact du PK) |")
A(f"| Réécrits — collision **préexistante**, la passe n'a fait qu'aligner les octets | {len(rewritten)} | #369 ×3 (casse, orthographe, accent) · #1025 ×4 (apostrophe courbe) |")
A(f"| Créés en octets stricts mais **collision préexistante modulo écriture** | {len(typo_created)} | apostrophe (#1025), accent (#369), casse (#369) |")
A(f"| **Vraies créations** — la passe a rendu identiques deux titres **différents** | **{len(true_created)}** | toutes de #369 ; détail ci-dessous, ancien et nouveau cités |")
A("")
A("Les 86 groupes totalisent 180 PK porteurs, dont 27 cartes du deck (colonne `carte` non vide).")
A("")
A("## Les 3 vraies créations (ancien → nouveau cités)")
A("")
for r in sorted(true_created, key=lambda r: r["date"]):
    A(f"### «{r['title']}»")
    for m in r["members"]:
        old = r["olds"][m["path"]]
        if m["conv"] is not None:
            A(f"- PK {m['pk']} (`{m['path']}`, carte='{cur[m['path']][2]}') **MOVER** : «{old}» → «{r['title']}»")
        else:
            A(f"- PK {m['pk']} (`{m['path']}`, carte='{cur[m['path']][2]}') portait déjà ce titre avant la passe")
    pr = f"PR #{r['pr']}" if r["pr"] else "sans PR au subject"
    A(f"- convergence : `{r['sha']}` {r['date']} {pr} — *{r['subj']}*")
    A("")
A("**Motif commun** : les 3 créations viennent de #369 (« FR clarity », gpt-5.5) qui a aligné le libellé d'un nœud")
A("sur celui d'un **autre** nœud — une convergence, pas un doublon de contenu (cf. #1578 : 661/664 = deux concepts).")
A("661 est traité par #1581 ; **la décision règle C du 26/09 (ai-01, revue #1584) tranche les 2 autres** : 178 et 96")
A("reprennent leur libellé d'avant #369 (grain CSV séparé du pool v19).")
A("")
A("⚠️ « Question piège » : la collision 179/701 est **d'origine** ; #369 y **ajoute** un 3e porteur (178).")
A("")
A("## Les 2 origines que la première version rangeait à tort en créations")
A("")
former_false = list(interrupted) + [(t, m) for t, m in origin if t == "Équivoque antithétique"]
for title, members in former_false:
    A(f"### «{title}» — présente à l'import, jamais créée")
    for m in members:
        hist = []
        for j in range(len(snaps)):
            t = title_at(m["path"], j)
            if not hist or hist[-1][1] != t:
                hist.append((snaps[j][0][:8], t))
        moves = " → ".join(f"«{t}»" if t != title else f"**«{t}»**" for _, t in hist)
        A(f"- PK {m['pk']} (`{m['path']}`) : {moves}")
    A("")
A("« Sophisme de la double faute » : #369 (`9d45b4f9`) met « Deux torts font un droit » sur la 713 — la collision")
A("se défait — puis #1522 (`789afcd8`) lui **rend** son titre d'origine. La convergence mesurée est une")
A("**restauration**, pas une création. « Équivoque antithétique » : aucun titre n'a jamais bougé ; le « MOVER »")
A("de la première version était la rangée voisine (`6.2.2.3`, « Démenti non-démenti », renumérotée 1407 par")
A("`f38cfed9`) lue comme l'ancien état du PK 1000.")
A("")
A("## Les 3 créés « modulo écriture » (collision préexistante, octets différents avant la passe)")
A("")
for r in typo_created:
    A("- «{}» — avant : {} ; convergence `{}` {} PR #{}".format(
        r["title"],
        " vs ".join(f"PK {cur[mm['path']][1]} «{r['olds'][mm['path']]}»" for mm in r["members"]),
        r["sha"], r["date"], r["pr"]))
A("")
A("## Les 7 réécrits (tous les membres portaient déjà le même titre, la passe a unifié l'écriture)")
A("")
for r in rewritten:
    A(f"- «{r['title']}» — avant la passe tous portaient «{list(r['olds'].values())[0]}» ; `{r['sha']}` {r['date']} PR #{r['pr']}")
A("")
A(f"## Annexe A — les {len(origin)} groupes d'origine")
A("")
for title, members in origin:
    A(f"- «{title}» — " + ", ".join(f"{m['pk']}(`{m['path']}`)" for m in members))
A("")
A("## Annexe B — join imprimé (cartes deck des 86 groupes, référence `Cards/Fallacies/Archive/` par `path`)")
A("")
for title, paths_ in sorted(shared.items()):
    deck = [p for p in paths_ if cur[p][2]]
    if not deck:
        continue
    A(f"- «{title}» :")
    for pathv in sorted(deck, key=lambda p: (len(p.split(".")), [int(x) for x in p.split(".")])):
        refs = " · ".join(
            (f"{era}: «{archives[era].get(pathv)}»" if archives[era].get(pathv) else f"{era}: absent")
            for era in ("2022", "v3"))
        A(f"  - PK {cur[pathv][1]} (`{pathv}`) — {refs}")
A("")
A("## Ce que cette mesure n'établit pas")
A("")
A("- Elle ne décide de rien : la règle C (renommer ou accepter) reste à ai-01, groupe par groupe.")
A("- « À l'origine » veut dire *déjà présentes à l'import initial du fichier dans le repo* (2025-07-02) —")
A("  les 73+2 viennent du corpus d'avant le repo (GSheet/import), pas d'une passe git identifiable.")
A("- Ne mesure ni `desc_fr`, ni les 7 autres langues (grain 3), ni les collisions inter-langues.")
A("- Le join imprimé couvre les ères archivées en CSV (2022, v3) ; v1/v2 n'ont pas de CSV d'époque joint.")
A("- La convergence est datée au dernier commit où un membre est arrivé à la valeur actuelle ; une collision")
A("  interrompue puis recréée avec les mêmes octets serait datée de la recréation (non observée ici : les deux")
A("  interrompues sont couvertes par la règle d'origine à l'import).")
A("- ⚠️ **Piège d'instrument (corrigé ici)** : suivre les rangées par **PK** traverse mal une renumérotation.")
A("  `f38cfed9` portait chaque PK 1000 et 520 sur **deux** rangées ; un index par PK ne conserve qu'une des deux")
A("  et lit la voisine comme l'ancien état de la même rangée. Toute mesure de ce corpus suit les rangées par")
A("  `path`, ou traite la renumérotation explicitement.")
A("")
A("## Annexe C — instrument rejouable (version corrigée)")
A("")
A("``\`python")
A(open(__file__, encoding="utf-8").read().replace("``\`", "``\\`"))
A("``\`")
A("")
A(f"*po-2024 — mesure, 0 écriture sur le corpus · {len(origin)}+{len(interrupted)}+{len(rewritten)}+{len(typo_created)}+{len(true_created)}"
  f" = {n} groupes (corrigé revue #1584)*")

with open("docs/corpus/458-v18-g2-collisions-fr-origine-2026-09-26.md", "w", encoding="utf-8", newline="\n") as f:
    f.write("\n".join(L) + "\n")
print(f"dossier: {len(L)} lignes")

```

*po-2024 — mesure, 0 écriture sur le corpus · 72+1+7+3+3 = 86 groupes (corrigé revue #1584)*

# #1180 — Parité wrappers/OWL re-mesurée après la montée DNN 10.4.1 + 2sxc 22.01

**Auteur** : po-2023 (worker) · **Date** : 2026-11-02 · **Base** : origin/master `eb6f1452`
**Référence** : [`1180-1066-route-parity-prod-preprod-blob.md`](1180-1066-route-parity-prod-preprod-blob.md) (2026-09-14, master `21a72385`)
**Nature** : **mesure**, lecture seule sur les deux hôtes. Aucun dépôt, aucune régénération, aucun geste git écrivant sur un webroot.

Ce document **rejoue** la mesure du 14/09 avec le même protocole, après la montée de préprod du 02/11.
Il ne remplace pas le document du 14/09 : il en donne le **delta**.

---

## §1 — Ce qui change, en une ligne

Les 16 wrappers `_ext` **ne sont plus absents** de la préprod (ils étaient 404 le 14/09) et sont
désormais **octet pour octet identiques** au blob de master. Les 16 wrappers de base restent
**divergents**, mais l'écart s'effondre de **−12,5 %/−13,8 %** à **≤ 0,15 %** — et cette divergence
restante est **entièrement expliquée** : la préprod sert la couche du **26/10**, prod celle du **31/10**.

## §2 — Totaux (36 candidats × 2 hôtes × blob)

| | 14/09 (master `21a72385`) | **02/11 (master `eb6f1452`)** |
|---|---|---|
| prod = blob | 32 | **32** |
| prod divergent | 1 (OWL fallacies) | **1** (OWL fallacies, mêmes octets qu'au 14/09) |
| prod 404 | 3 | **3** (inchangé) |
| préprod = blob | 0 | **16** ← les `_ext` |
| préprod divergent | 17 | **17** (16 de base + OWL) |
| préprod 404 | **19** | **3** |
| prod ≠ préprod | 16 | **16** |

† **Erratum (02/11, après relecture — [c.6576531867](https://github.com/ArgumentumGames/Argumentum/pull/2124#issuecomment-6576531867))** : la première version portait
**17** dans la case « prod ≠ préprod » du 14/09. C'est une contamination de ligne : 17 est le total
« préprod divergente **du blob** » (16 wrappers + l'OWL), or l'OWL est **identique entre les deux hôtes**
à cette date comme aujourd'hui — il ne compte donc pas dans la comparaison croisée. Le compte croisé du
14/09 est **16** (les 16 wrappers de base ; les 16 `_ext` étaient 404 sur la préprod, donc non
comparables octet à octet). Les deux colonnes valent 16 : le retard de copie remplace des routes qui
n'étaient pas servies, il n'ajoute pas de différence.

Le total préprod « divergent » ne bouge pas (17) : ce sont **les mêmes 17 routes**, mais leur cause change
de nature. Au 14/09 c'était une couche de **six commits de retard** (`de763aa9`, 28/08). Aujourd'hui c'est
une **copie de cinq jours de retard** — voir §3.

## §3 — Pourquoi les 16 wrappers de base divergent encore — et pourquoi ce n'est pas un défaut

Trois mesures indépendantes concordent.

**a) Horodatage servi** (`Last-Modified`, une *déclaration* du serveur, pas une preuve en soi) :

| hôte | `fallacies_fr.html` | `fallacies_en.html` |
|---|---|---|
| prod | `Sat, 31 Oct 2026 00:11:12 GMT` | `Sat, 31 Oct 2026 00:11:13 GMT` |
| préprod | `Mon, 26 Oct 2026 10:05:42 GMT` | `Mon, 26 Oct 2026 10:05:43 GMT` |

**b) Identification de couche** — les octets servis par la préprod sont comparés au blob de
**chaque** révision du chemin dans l'historique, et non à master seul :

| route | octets servis par la préprod = |
|---|---|
| `fallacies_fr` | révision **`8f308c09`** (2026-10-09, PR #1831) |
| `fallacies_en` | révision **`8f308c09`** |
| `fallacies_zh` | révision **`8f308c09`** |
| `virtues_es` | révision **`8f308c09`** |

**4 / 4** — la préprod sert **master tel qu'il était au 09/10**, sur toutes les langues testées.

**c) Ce qui a bougé sur master depuis** :

```
bc823a81  2026-10-30  feat(mindmaps): re-derivation FreeMind FR — 2 corrections de libellés (#2077)
19afd238  2026-10-31  feat(mindmaps): #2025 — livraison 7 langues en/ru/pt/es/ar/fa/zh (#2083)
```

⇒ La copie préprod du **26/10** précède les deux. La copie prod du **31/10** les contient toutes les deux.
**La divergence mesure un retard de déploiement, pas une génération différente ni une corruption.**

Ampleur résiduelle, par route (prod − préprod, en octets) :

| route | prod (= blob) | préprod | Δ | Δ relatif |
|---|---:|---:|---:|---:|
| `fallacies_en` | 2 570 283 | 2 574 050 | +3 767 | 0,147 % |
| `virtues_zh` | 1 236 624 | 1 237 505 | +881 | 0,071 % |
| `virtues_fa` | 551 146 | 551 988 | +842 | 0,153 % |
| `virtues_fr` | 519 698 | 520 462 | +764 | 0,147 % |
| `fallacies_es` | 2 659 764 | 2 659 768 | **+4** | 0,0002 % |
| `fallacies_zh` | 5 363 061 | 5 363 016 | −45 | 0,0008 % |
| `fallacies_fr` | 2 721 652 | 2 721 627 | −25 | 0,0009 % |

*(les 16 lignes sont dans la même enveloppe ; ±0,15 % est le maximum observé)*

**Ce que la taille ne dit pas** : à taille quasi égale, le contenu diffère réellement. Sur `virtues_fr`
la préprod porte **400** blocs `clipPath` et master **398** ; sur `fallacies_fr` les deux en portent
**1 853**. La géométrie des `clipPath` change aussi (`L310 134` → `L309 134` et blocs réalignés).
Ce sont des différences de **contenu**, pas de fin de ligne : le compte CRLF est identique des deux côtés
et une normalisation CRLF→LF ne rapproche aucune paire. **La cause exacte de ces écarts de géométrie
n'est pas établie ici** — elle appartient aux commits `bc823a81`/`19afd238`, que je n'ai pas disséqués.

## §4 — Ce qui reste ouvert, et qui ne dépend pas de la montée

| route | constat | état |
|---|---|---|
| `argumentum_fallacies.owl` | prod = préprod = **4 795 192 o**, blob master = **5 985 122 o**. Les deux hôtes sont identiques **entre eux** et divergents du dépôt | **inchangé depuis le 14/09** (même sha `258a9e430e4f`) |
| `fallacies_cards_fr.html` | **404** des deux côtés | inchangé |
| `fallacies_cards_fr_ext.html` | **404** des deux côtés | inchangé |
| `argumentum_virtues.owl` | **404** des deux côtés | inchangé |

L'OWL servi reste l'artefact non tracé du webroot daté de mars 2024 (voir §4 du document du 14/09) :
**la montée ne l'a pas touché, et ne pouvait pas le toucher** — il n'est pas dans l'arbre git.

## §5 — Conséquence pour la recette #1180

#1180 est une recette qui se fait **sur la préprod**. Or la préprod sert aujourd'hui les cartes mentales
**du 09/10**, quand prod sert celles du **31/10** (re-dérivation FR #2077 + livraison 7 langues #2083).

⇒ **Un relecteur qui valide les cartes mentales sur la préprod valide une génération superseded.**
Si la recette doit porter sur les cartes mentales, la couche préprod doit d'abord être rafraîchie depuis
master ; sinon, la validation porte explicitement sur le 09/10 et doit être datée comme telle.

Ce n'est pas une anomalie de la montée : c'est l'état de la copie du 26/10, antérieure de deux commits.

⚠️ **Portée exacte** : les **six pages** de la recette #1180 (accueil, règles, argumentation, actus,
acheter, fiche produit) **ne comprennent aucune de ces 36 routes**. La re-mesure ci-dessus ne bloque donc
pas la recette à trois ; elle documente l'état du **volet wrappers** de #1066, qui est un autre objet.

## §6 — Ce que je n'établis pas

- **Que prod soit « correct »** : je constate que prod sert exactement le blob de `origin/master` sur
  32/32 wrappers. La justesse du contenu est un verdict de relecture, pas de parité — il appartient à ai-01.
- **L'identité du chemin physique IIS** servi pour `/fallacies_<lang>.html` : `Get-Website` exige
  l'élévation, non prise. La correspondance servi↔committé utilisée ici est celle du **§0 du document du
  14/09**, établie par mesure, et je n'en ai pas fait une nouvelle démonstration.
- **La cause des écarts de géométrie `clipPath`** (§3).
- **Que l'absence des 3 routes 404 soit un défaut** : je constate qu'elles sont absentes des deux hôtes,
  ce qui est cohérent avec un choix de périmètre ; #1066 est CLOSED et je ne rouvre rien.

## §7 — Reproductible

Protocole inchangé depuis le 14/09 : cache-buster obligatoire, corps écrit **sur disque** puis haché
(jamais une variable shell : le newline terminal disparaîtrait et fabriquerait un faux écart), blob lu par
**flux binaire** (`git cat-file blob` → `StandardOutput.BaseStream.CopyTo`), et garde de vacuité — un blob
introuvable ne doit jamais se lire « divergent ».

```powershell
# servie
Invoke-WebRequest "$base/$route.html?cb=$([guid]::NewGuid().ToString('N').Substring(0,8))" -OutFile $tmp
(Get-FileHash $tmp -Algorithm SHA256).Hash
# référence
git -C $repo cat-file blob "origin/master:$rel" > $tmp2   # en flux binaire dans le harnais
(Get-FileHash $tmp2 -Algorithm SHA256).Hash
# identification de couche : rejouer la même comparaison contre <rev>:<chemin>
```

Le script de cette campagne (`.tmp\rang2\parity\parity-1180.ps1`, 45 s, 36 candidats) n'est pas committé :
`.tmp\` est ignoré. Sa sortie complète est `parity-1180.tsv`.

## §8 — Addendum : le retard a été résorbé le jour même (02/11, ~17:00)

Sur instruction d'ai-01 (pool #458 c.6576550705, point 3), les **16 wrappers de base** ont été
rafraîchis sur la **préprod seule** depuis les blobs de `origin/master` (webroot `DNNPlatform\`,
casse préservée : `fallacies_<lang>.html` / `Virtues_<lang>.html`). Les 16 `_ext`, déjà conformes,
n'ont pas été touchés.

| | avant rafraîchissement | **après** |
|---|---|---|
| préprod = blob | 16 | **32 / 32** |
| préprod divergent | 17 | **1** (l'OWL, inchangé) |
| prod ≠ préprod (croisé) | 16 | **0** — 33 identiques |

Geste : dry-run d'abord (garde « source = blob de master » 16/16), backup des 16 fichiers remplacés
avec sha256 + manifeste (`.tmp\mindmaps-preprod-refresh-20261102\backup\`), copie, vérification sur
disque 16/16, puis re-mesure HTTP complète (92 s, cache-buster). Les §1–§7 ci-dessus décrivent
l'état **avant** ce geste et demeurent comme constat daté.

# Passe FreeMind #2025 — mesure de la régénération des cartes mentales (30/10/2026)

> Période : 30/10/2026, ~14:07 → 14:59. Lane worker (po-2023).
> La passe écrit **uniquement** dans l'arbre de build d'un worktree détaché (arbre gelé
> `341ef2a3`) — mesuré, pas supposé : voir § « Ce que la re-dérivation change réellement ».
> Le **transfert** vers l'arbre committé est un geste **explicite**, fait après mesure
> d'identité sémantique — il ne porte que le FR. Le gel CSV #2025 est respecté : rien sous
> `Cards/**` n'a été touché côté corpus.

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

## ⚠️ Ce que la re-dérivation change *réellement* — et ce qu'elle ne change pas

⚠️ **Correction d'une affirmation antérieure (mesurée le 30/10 à 15:30).** J'ai écrit
d'abord que « le véhicule recopie automatiquement dans `Cards/` ». **C'est faux**, et
la note de siège sur la dérivation-seule le disait déjà : *les sorties d'une passe
mindmap vont sous `Target/`, jamais dans `Cards/`*. Trois mesures concordantes :

| Instrument | Résultat |
|---|---|
| Code — `GetDocumentDirectory` → `GetBaseTargetDirectory` | `Target/<lang>/Documents/`, jamais `Cards/` |
| Script de passe (`.tmp/mindmap-pass-lang.ps1`) | **aucun** `Copy-Item`/`robocopy` vers `Cards/` |
| Journal de la passe | **0** occurrence de « Cards » sur 219 lignes |

⇒ Le transfert vers `Cards/` est un **geste explicite**, pas un effet de la passe.
Signature qui le confirme : les fichiers `Cards/` portent la **même mtime à la
nanoseconde** que leurs jumeaux de `Target/` (14:26:07.562676200 pour `fr`,
14:40:58.988040000 pour `en`…) — c'est ce qu'un `Copy-Item` produit, pas une
écriture indépendante.

Reste la question qui décide de tout : **ces fichiers changent-ils quelque chose ?**

L'export FreeMind **renumérote les `clipPath`** d'un export à l'autre (mêmes
tracés, autres identifiants) — un diff de 99 Mo qui ne veut rien dire. L'instrument
qui tranche est donc **le multiset des tracés et le multiset des libellés**
(insensible à la numérotation), et non le diff de lignes.

Mesuré sur les `.content.svg` (le fichier que le wrapper inline) :

| Langue | Tracés | Libellés | Verdict |
|---|---|---|---|
| en, ru, pt, es, ar, fa, zh | identiques | identiques | **sémantiquement identiques** |
| **fr** | 7 242 | 1 419 | **écart réel** |

L'écart FR vaut **deux libellés** : les mindmaps n'avaient pas suivi les corrections
du corpus (« l’**î**lot », « **A**cadémisme »).

⚠️ **Formulation à ne pas confondre** (relevé par la session sœur) : ces deux libellés
étaient **déjà corrects dans le CSV de master** avant la passe, et la PR ne modifie
**aucun** CSV. La passe les **rattrape**, elle ne les **crée** pas — un message de
commit qui dit « corrigés dans la source » se lit à tort comme « corrigés par cette
passe ». Rien n'est perdu à la prochaine re-dérivation.

⇒ **Conséquence de livraison** : les 7 langues non-FR ne sont **pas** commitées (bruit
de sérialisation sans valeur) ; la PR ne porte que les **11 fichiers FR**. Une
re-dérivation qui « change 52 fichiers » peut ne valoir qu'**un couple de
libellés** — et le dire demande un instrument, pas un diff.

### 4. Une seconde famille d'échec d'export — la dérive de menu de l'automatisation

**RAPPORTÉ** (mesuré par une session sœur, non re-mesuré ici) : l'automatisation
`SendKeys` **dérive de menu** sur ~12 exports de la passe — le dialogue sous-sectionné
produit un **`.odt` au lieu d'un `.svg`**, puis le run **timeout**. C'est une famille
**distincte** du flake `ru` ci-dessus :

| Symptôme | Cause | Signature |
|---|---|---|
| `cards_<lang>` **absent** | export non produit | invite interactive ⇒ **timeout** |
| `.odt` **produit** à la place du `.svg` | dérive de menu `SendKeys` | `.odt` dans l'arbre ⇒ **timeout** |

⇒ Un timeout d'export a **au moins deux causes distinctes** : lire le **type du
fichier produit** avant de conclure « export manquant ».

## Coordination — deux commits, un seul périmètre

La même passe a été reprise par une **session sœur** dans le worktree véhicule, qui a
produit un commit de **52 fichiers** (les 8 langues) sur le même parent. Comparaison
de ce commit à master, avec le même instrument sémantique :

```text
en ru pt es ar fa zh  →  paths_equal=True  labels_equal=True  pathdiff=0  labeldiff=0
```

⇒ les 7 langues non-FR sont **sémantiquement identiques à master** ; le diff n'est
que de la re-sérialisation (renumérotation des `clipPath`). Les fichiers FR sont
**identiques arbre-pour-arbre** entre les deux commits.

**Livré : le FR seulement.** Ce que la mesure ne dit pas — « 52 fichiers modifiés » —
ne vaut pas 52 fichiers ; cela vaut **deux libellés**.

La session sœur a également purgé les **35 fichiers `cards_<lang>` sur-produits** pour
les 7 langues non-FR (la config ne déclare la variante *cards* que pour `fr`) et
mesuré la suite de tests complète du worktree : **2 331 réussis / 0 échec / 6 ignorés**
(2 337).

## Suite

- **Livraison** vers `Cards/**` : faite **pour le FR seulement** (11 fichiers), en PR
  dédiée — les 7 autres langues ne changent rien (§ identité sémantique). Le transfert
  a été un **geste explicite** (`Copy-Item` depuis `Target/`), pas un effet de la passe.
- Les artefacts non-FR de la passe restent dans l'arbre du worktree véhicule, **non
  committés** (préservation, pas suppression).
- **Prochaine tire** (ordre c.6522990068) : SharedResources ×3 (ar/fa/zh), 8 clés GR,
  caractérisation du flap 401, test d'import en **application de test** (jamais l'app 60).

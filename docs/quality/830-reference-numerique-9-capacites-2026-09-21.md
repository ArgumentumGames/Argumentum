# #830 — Référence **numérique** des 9 capacités, figée sur l'arbre re-dérivé

**Auteur** : po-2024 (worker) · **Date** : 2026-09-21 · **Base** : master `6694d702`
**Instrument** : `MindmapWrapperCapabilitiesTests` (VisualTests) — **l'instrument existant, rejoué**, pas un second instrument construit en parallèle · viewport **1400 × 900** · fixtures **committées** (gabarit `included.html` + `.content.svg`), composées par `MindMapHtmlWrapper.FormatWrapper`, chargées en `file:///` · **44 cas, 0 échec, 0 ignoré, 1 min 20 s** (Chromium chaud).

> **Ce que ce dossier ajoute.** [`830-etat-9-capacites-2026-09-15.md`](830-etat-9-capacites-2026-09-15.md) répond à « **quelle capacité est assertée par quoi** ». Celui-ci répond à « **quelle valeur la référence vaut, sur l'arbre fraîchement re-dérivé** ». La distinction n'est pas cosmétique : les assertions du harnais sont **relationnelles** (`ratio ≥ 1,5×`), donc une régén qui ferait chuter le zoom initial de ×7,69 à ×1,60 passerait **verte** en rendant la carte illisible. L'issue exige elle-même des valeurs **nommées avec leur artefact et leur viewport** (§« Règles de mesure ») — ce dossier les fournit, et **0 valeur n'était figée** avant lui (vérifié : les constantes de référence n'apparaissent dans aucun test).

---

## §0 La fenêtre est réelle — vérifiée, pas supposée

Le dispatch fonde ce grain sur « les 17 jeux viennent d'être re-dérivés ». Mesuré sur `6694d702` :

| Contrôle | Résultat |
|---|---|
| Paires (wrapper inlinant ↔ son `.content.svg`) | **16/16** présentes |
| Wrappers plus **vieux** que leur `.content.svg` (le gate #1446) | **0 / 16** |
| Dernier écrivain des 16 wrappers | **`16a0447a`** (#1465, 21/09) |
| Dernier écrivain des 16 `.content.svg` | **`68e8d3d5`** (#1453, 21/09) |

⇒ Les wrappers ont été re-dérivés **après** leurs SVG. Une référence figée avant #1465 aurait mesuré un arbre périmé — c'est exactement ce que le dispatch écarte.

⚠️ **Asymétrie de stems, à connaître avant tout balayage** : la famille **Virtues** porte **deux bases différentes** — le wrapper HTML s'appelle `Argumentation_Virtues_<lang>`, le SVG `Argumentum_Virtues_MindMap_<lang>`. Un contrôle qui suppose une base partagée rend **8 paires sur 16 et 0 Virtues**, ce qui se lit « Virtues absentes ». Rencontré et corrigé dans ce grain (§5).

---

## §1 Valeurs **déclarées** par le gabarit committé

Relevées dans `Cards/Fallacies/Mindmaps/included.html` (L714-718) et `external.html` (L798-806) — **identiques dans les deux** :

| Option | Valeur |
|---|---|
| `fit` | `0` |
| `center` | `0` |
| `minZoom` | `0.15` |
| `maxZoom` | `15` |

Ces quatre valeurs sont la **source de l'assertion Cap 9** : le harnais lit `minZoom` **dans la fixture** (`DeclaredMinZoom()`), pas dans l'observation — c'est la seule quantité de Cap 9 qui ne vient pas de la mesure. `external.html` les déclare **aux mêmes valeurs** malgré une numérotation de lignes différente, donc l'`_ext` ne diverge pas sur les bornes (le reste de son comportement n'est pas mesuré ici — ses fixtures ne sont pas sondables, cf. §4).

---

## §2 Le rejeu — valeurs **observées**, citées verbatim

### Cap 1 — zoom initial + lisibilité

| artefact | `scaleStable` | `fit` | `ratio` | `textVisible` | `medianRenderedHeight` | `readable(≥9px)` |
|---|---:|---:|---:|---:|---:|---:|
| `fr` Fallacies | 0,32192 | 0,04185 | **7,69** | 1423 | **10,62 px** | 827 |
| `fr` Virtues | 0,32192 | 0,14414 | **2,23** | 232 | **10,62 px** | 198 |
| `zh` Fallacies | 0,32192 | 0,04185 | **7,69** | 1408 | **10,62 px** | 805 |
| `ar` Fallacies | 0,32192 | 0,04185 | **7,69** | 1408 | **10,62 px** | 808 |

⭐ **Deux lectures que le critère binaire ne peut pas donner.**

1. **`scaleStable` est identique à 5 décimales pour les quatre cas — y compris Virtues — alors que le `fit` diffère du simple au triple** (0,04185 vs 0,14414). Le zoom initial n'est donc **pas** une règle relative au contenu : c'est une valeur **dérivée du viewport**, que le `fit` ne fait que diviser. C'est ce qui explique que le **ratio** varie (7,69 vs 2,23) sans qu'aucune règle par famille n'existe. Corollaire opérationnel : **un ratio qui baisse n'accuse pas forcément le zoom** — il peut venir d'un `fit` qui monte.
2. **La lisibilité n'a que 6 % de marge.** `medianRenderedHeight` vaut **10,62 px** sur les quatre, contre une barre à **10,0 px**. Le critère passe, mais de peu : une baisse de ~6 % du zoom initial le fait basculer, et la carte devient illisible **avant** que le ratio (barre ×1,5) ne bouge. C'est la mesure la plus fragile de la référence, et elle n'est **pas** exprimée par une assertion dédiée — elle l'est par une borne basse.

Cohérence avec la règle écrite par l'issue (« zoom initial ≈ hauteur/2600 unités-utilisateur ») : le canvas mesuré fait **841 px** de haut, donc 841 / 2600 = **0,32346** contre **0,321923** observé, soit **−0,47 %**. La règle est donc **confirmée à 0,5 % près** — et c'est le canvas, pas la fenêtre (900 px), qui est le bon numérateur.

### Cap 2 — centrage racine (fr · zh · ar)

Trois langues, valeur **identique** : `dx=20,0` `dy=2,0`, canvas `1384x841`, `floor=84,1px`.

| Membre | Valeur | Barre | Verdict |
|---|---:|---:|---|
| `\|offX\| / largeur` | 20,0 / 1384 = **1,45 %** | < 5 % | ✅ |
| `\|offY\| / hauteur` | 2,0 / 841 = **0,24 %** | < 5 % | ✅ |

⚠️ Ces pourcentages sont **dérivés** (le harnais imprime des pixels) : ce sont les miens, calculés sur le canvas qu'il imprime. Un pourcentage de centrage cité sans son canvas n'est pas reproductible — c'est la règle de l'issue, elle vaut ici aussi.

### Cap 3 — pan (fr)

`pan before x=-603,3 y=-2778,7` → `pan after x=-483,3 y=-2688,7` ⇒ **Δ = (+120,0 ; +90,0)**.

### Cap 4 — zoom molette (fr)

`wheel scale before=0,32192` → `after=0,34689` ⇒ **croît vers le curseur**. ✅

### Cap 5 — icônes de contrôle **effectives** (fr)

| Contrôle | Effet mesuré |
|---|---|
| zoom-in | `0,321923 → 0,354115` |
| zoom-out | `0,354115 → 0,321923` |
| reset (Virtues) | `0,321923 → 0,144136` |
| reset (Fallacies) | `0,321923 → 0,041850` |

Les trois **déplacent** le viewport — c'est le membre « effectives » qui distingue un contrôle inerte (présent, visible, cliquable, inutile). Le harnais imprime aussi une seconde mesure, à l'échelle unitaire : `After zoom-in click: 1,100000023841858` / `After zoom-out click: 1,0000000514767393`.

### Cap 6 — double-clic (fr)

`dblclick scale before=0,32192` → `after=0,62775` ⇒ **×1,95**. ✅

### Cap 7 — clic nœud → carte (fr)

`overlay .famille after click = 'Argument fallacieux' (node family='Argument fallacieux')` ⇒ l'overlay est peuplé **du texte du nœud cliqué**, les deux valeurs coïncident.

### Cap 8 — couleurs de famille (fr)

`familyclass 'Argumentfallacieux'` → `card .texte background 'rgb(85, 85, 85)'` = **`#555555`**, la couleur de la famille `argumentValable` documentée. **Fond non blanc** ⇒ la classe est bien appliquée. ✅

### Cap 9 — bornes de zoom (les deux familles)

| artefact | `fit` | `zoomOutFloor` | `ratio(floor/fit)` | `declared minZoom` |
|---|---:|---:|---:|---:|
| `fr` Virtues | 0,144136 | 0,021620 | **0,150** | 0,150 |
| `fr` Fallacies | 0,041850 | 0,006278 | **0,150** | 0,150 |

⇒ `reset` et le zoom-out maximal sont **deux états distincts**, et leur rapport vaut **exactement le `minZoom` déclaré** — donc les bornes sont **relatives au fit**, ce que la mesure **démontre** au lieu de le supposer (un « zoom-out max = fit » donnerait un rapport de 1,00). Le `ratio` observé vaut **0,1500** contre **0,150** déclaré : accord à la précision d'affichage.

### Mesure annexe imprimée au passage — éléments `<path>` du SVG inliné

`en` 634 et 3754 · `es` 616 · `fr` 624 et 3845 · `ar` 625 · `pt` 609 et 3834 · `ru` 597 et 3961 · `zh` 445 · `fa` 640. ⚠️ **Deux populations distinctes** (les ~600 sont des `links.svg`/`cards`, les ~3800 des `Fallacies_<lang>` complets) : ces nombres ne sont **pas** comparables entre eux sans nommer l'artefact, et **ils ne mesurent pas une capacité** — je les consigne parce qu'ils sortent du même run, ⛔ pas comme une référence.

---

## §3 Item 3 — la propagation **est faite**, et le critère de l'issue est atteint

Le corps de #830 porte encore : *« Commentaire de gabarit FAUX ✅ corrigé au gabarit le 20/09 (#1452) — la **propagation aux 34 wrappers construits reste gatée régén** (critère vérifiable : `grep -c "fit complet"` = **34 → 0** à la régén du tag) »*.

**La régén a eu lieu (#1465) et le critère est atteint.** Mesuré sur `6694d702` :

| Motif | Gabarit `included.html` | Les 36 HTML sous `Mindmaps/` |
|---|---:|---:|
| `fit complet` (l'ancien, faux) | **0** | **0** |
| `zoom-out maximal` (le corrigé) | 1 | **36** |

**Contrôle inverse** — un zéro seul ne prouve rien : le commentaire aurait pu **disparaître** au lieu d'être remplacé. Le texte corrigé est présent dans **36/36** (34 wrappers de `<lang>/` + les 2 gabarits racine), et la ligne est lisible dans un wrapper construit (`Fallacies_fr.html` L20393 : *« Additif : le RESET revient au fit, »*).

⚠️ Le piège de pathspec que l'issue signale (`Mindmaps/*` vs `Mindmaps/*/`) **ne mord pas sur un balayage global** : j'ai cherché sur l'ensemble des `.html` sous `Mindmaps/`, donc le superset des deux. Il mord sur un décompte **par sous-dossier**.

---

## §4 Le seul reste ouvert, et il n'est pas côté arbre

**Cap 0 (intégrité de l'instrument) + « commité ≠ servi »** porte l'artefact **servi** : elle est hors de portée du présent dossier par construction (la CI, comme ce rejeu, rend des fichiers **locaux** — zéro réseau).

- **Servi = `de763aa9`** (po-2023, 13/09, 16/16 sha256-identiques) — **6 commits de wrappers derrière HEAD**.
- **Préprod** : ai-01 rapporte ce cycle **16/16 servi == master, octet à octet** — ⚠️ **RAPPORTÉ par le coordinateur, non re-mesuré ici** (lane po-2023 ; le contrôle s'y fait en **octets servis ↔ blob**, jamais en code HTTP).
- **Prod** (`argumentum.games`) : ai-01 l'a mesuré **périmé 16/16** le 21/09.

⚠️ **La divergence de référence est maintenant LOCALISÉE — et elle change de nature.** Le dossier servi (13/09, §5) relevait que `reset()` y rend **1,0** alors que le zoom-out max atteint **0,15**, donc *« reset = le fit »* **ne coïncidait pas avec le servi**.

Sur l'arbre committé, ce rejeu mesure `reset 0,321923 → 0,144136` (Virtues) et `→ 0,041850` (Fallacies) : **le reset rend bien le fit, dans les deux familles**. ⇒ L'écart n'est **pas** une ambiguïté du gabarit (qui déclare `fit:0`, ce qui suffit à le produire) : c'est un écart **entre l'arbre committé et le blob servi**, cohérent avec les 6 commits de retard. **Le présent dossier ne tranche pas** — il constate que les deux mesures ne portent pas sur le même objet, et que **c'est ai-01 qui tranche**.

---

## §5 Deux faux négatifs d'instrument, rencontrés et corrigés

Consignés parce que #830 a précisément pour sujet les pannes d'instrument, et que **chacun a produit un zéro qui se lisait comme une absence** :

1. **Le stem partagé qui n'existe pas** — mon premier contrôle de fraîcheur cherchait `Virtues_<lang>.html` pour les deux familles. Il a rendu **8 paires sur 16, et 0 Virtues**, ce qui se lit « la famille Virtues n'a pas de wrapper ». Le wrapper s'appelle `Argumentation_Virtues_<lang>.html`. Corrigé → **16/16**.
2. **Ma propre commande a tronqué la preuve** — le premier rejeu était pipé vers `Select-Object -Last 120` : le fichier de sortie faisait 6 321 octets et **les lignes de Cap 1 et Cap 6 n'y étaient pas**, alors que le run affichait 44/44. Les valeurs manquantes se lisaient comme « le harnais n'imprime rien pour ces capacités ». Relancé **sans troncature** : 15 580 octets, toutes les lignes présentes.

---

## §6 Ce que ce dossier n'établit pas

⛔ Aucun **verdict** (les workers signalent ; le verdict comportemental et visuel est **ai-01**) · ⛔ aucune mesure **live** nouvelle (la couche servie est citée, pas re-sondée) · ⛔ **Cap 0 non mesurée** ici, par construction · ⛔ aucune modification de test, de code ou de CSV — le compte de cas du harnais passe de 43 à **44** par #1441, consigné au registre, **pas** par ce dossier · ⛔ la couverture langue/famille du harnais est **inchangée** (l'extension est l'option (a) de `830-etat-9-capacites`, **statuée post-tag**) · ⛔ l'état `_ext`/`cards_fr` du live n'est pas re-sondé · ⛔ les valeurs de §2 valent **pour ce viewport** (1400×900) et **ces artefacts** — un rapport d'échelle cité sans les deux n'est pas reproductible.

---

*po-2024 — pool #458 v8, grain ④. L'instrument n'a pas été remplacé : il a été **rejoué**, et ses valeurs **figées avec leur artefact et leur viewport**.*

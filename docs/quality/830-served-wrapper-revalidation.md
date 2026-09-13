# #830 — Golden-master comportemental, couche **SERVIE** (re-validation)

**Auteur** : po-2023 (worker) · **Date** : 2026-09-13 · **Base** : master `35acac04`
**Objet** : mesurer ce que l'utilisateur **reçoit**, là où la CI est structurellement aveugle.
**Statut** : **mesures**. Le verdict visuel/comportemental reste **ai-01** — les workers signalent.

---

## §0 — Pourquoi cette mesure existe

`mindmap-wrapper-behaviour.yml` rend les fixtures **COMMITÉES** (`Cards/Fallacies/Mindmaps/**` +
`.content.svg`) et **ne touche jamais le réseau**. Un vert CI dit donc que *HEAD* se comporte bien,
**jamais** ce que sert `dnn.argumentum.myia.io`.

Or les deux ont divergé. Ce document mesure l'écart et le comportement de la moitié **servie**.

---

## §1 — L'écart commité ↔ servi, chiffré à l'octet (16/16)

Sondé avec `?cb=<horodatage>` (HTTP **200** partout), empreinte **sha256** comparée au blob — la taille
seule aurait été un indice, pas une preuve.

| | servi | blob `de763aa9` | blob HEAD |
|---|---|---|---|
| `fallacies_fr.html` | 2 361 730 o | **2 361 730 o — sha256 IDENTIQUE** | 2 717 114 o |
| `virtues_fr.html` | 512 022 o | **512 022 o — sha256 IDENTIQUE** | 521 917 o |

**Les 16 wrappers** (8 langues × 2 familles) sont **sha256-identiques** au blob de
`de763aa9` — *« fix(mindmap): restore resize/orientationchange reactivity » (#1038, 10/08)*.
Contrôle : HEAD est plus **gros** sur les 16 (ex. `fallacies_*` +13 à +15 %).

⇒ **Le live sert `de763aa9`**, 6 commits de wrappers derrière HEAD. Un vert CI porte sur HEAD.

**Empreinte de déploiement** : les 16 fichiers portent tous `mtime 2026-08-28 10:00:17` — la même
seconde que le déploiement déjà identifié en #1244 (`Default.aspx`, 63 DLL plateforme, dossiers
`Resources/libraries`). **4ᵉ instance du même horodatage.**

---

## §2 — Capacité 0 (intégrité de l'instrument) sur les **octets servis**

Critères **canoniques** de l'issue, appliqués aux 16 fichiers servis :

| critère | résultat |
|---|---|
| corps servi relevé **avant** tout grep | ✅ (tableau §1) |
| `applyTransform` **absent** | ✅ **0** sur les 16 |
| marqueur `maxZoom: 15` présent | ✅ 1 sur 16/16 |
| marqueur `getBBox` présent | ✅ 3 sur 16/16 |
| taille ronde en puissance de 2 (signal de troncature) | ✅ aucune |

⇒ **Cap 0 verte sur la couche servie.** Le bloc d'options de la bibliothèque (`fit:!0, center:!0,
maxZoom:10`) coexiste avec la **surcharge du wrapper** (`fit:0`, `center:0`, `minZoom:0.15`,
`maxZoom:15`) — c'est la surcharge qui gouverne, et elle est bien **présente dans les octets servis**.

---

## §3 — Capacités comportementales mesurées sur le **live**

Instrument : Playwright/Chromium headless, viewport 1400×900, `ignore_https_errors`, **attente de
stabilisation** (deux échantillons consécutifs identiques) — jamais le premier tick (piège #831).

| # | Capacité | Où mesurée | Résultat |
|---|---|---|---|
| 1 | zoom initial lisible | FR | médiane texte **11 px** (≥10 ✅) · zoom stabilisé **7,692** (fallacies) / **2,233** (virtues) · stabilisation **66 / 74 ms** |
| 2 | centrage racine | FR · AR · ZH | offsets **1,31–1,54 %** (X) et **2,48–2,91 %** (Y) — **< 5 % partout** |
| 3 | pan (drag) | FR | ✅ le pan change de la distance draguée |
| 4 | zoom molette | FR | 1,0000 → **1,0192** (croît) |
| 6 | double-clic | FR | 1,0192 → **2,2423** (croît) |
| 7 | clic nœud → carte | FR · AR · ZH | overlay **visible**, titre rempli — et **localisé** (`السفسطة`, `谬论`, `حجة معتبرة`, `有效论证`) |
| 8 | couleur de famille | FR · AR · ZH | classe appliquée à l'overlay : `Argumentfallacieux` / `Argumentvalable` |
| 9 | bornes de zoom | FR | min effectif **0,15** · max effectif **15,0** — **exactement les valeurs de référence de l'issue** |
| 5 | icônes de contrôle | — | **non mesurée** |

⇒ **Sur les capacités mesurées, la couche servie se comporte correctement**, y compris en RTL (ar) et
CJK (zh).

---

## §4 — ⚠️ Trois pièges d'instrument, rencontrés et corrigés (à verser à l'issue)

Le sujet de #830 étant les pannes d'instrument, les trois miens valent d'être consignés — **chacun a
produit un faux « FAIL » sur un produit sain** :

1. **`panZoomInstance` est `var` en portée de FONCTION** — invisible en global. Une sonde qui lit
   `window.panZoomInstance` conclut « instance absente » et déclare Cap 0 rouge. Capturer l'instance
   en enveloppant `svgPanZoom` (`add_init_script`) avant l'exécution des scripts de page.
2. **L'overlay est un élément `<card>`, et `.cardName` est un helper en `display:none` PERMANENT.**
   Tester la visibilité de `.cardName` rend un faux « carte non ouverte » **à perpétuité**. Le signal
   réel : la classe du `<card>` **bascule de `hidden` vers le nom de famille** — `class="hidden"` →
   `class="Argumentfallacieux"`. **Afficher la carte et la colorer sont le même geste.**
3. **Ne pas mesurer le centrage APRÈS un `reset()`.** Le recentrage racine court sur
   `requestAnimationFrame` au chargement / `resize` / `orientationchange` — **pas après un reset**.
   Mesuré après reset, `virtues_fr` rendait 5,35 % (faux FAIL marginal) ; mesuré à l'état initial
   stabilisé, il rend **2,59 %**.

**Un « FAIL » doit être confirmé sur un second instrument avant publication** — les trois ci-dessus
l'auraient été autrement.

---

## §5 — Ce que cette re-validation n'établit PAS

- **Cap 5 (icônes de contrôle)** : non mesurée.
- **Cap 3 / 4 / 6 / 9** : mesurées **sur FR uniquement** (les caps 2/7/8 l'ont été sur FR·AR·ZH).
- **Le second membre du critère Cap 1** (« échelle ≥ 1,5× le fit-à-la-fenêtre ») : ma référence est
  l'état post-`reset()` (zoom 1,0), **pas** la mesure de fit de l'instrument
  `MindmapWrapperCapabilitiesTests.Cap1`. Le membre « ≥ 10 px » passe ; le membre « ≥ 1,5× » est
  mesuré **contre une autre référence** et n'est donc **pas comparable** au 2,5× cité par l'issue.
- ⚠️ **Divergence de référence à arbitrer** : l'issue énonce *« reset revient au fit complet
  (zoom-out max) »*. Sur le **servi**, `reset()` rend **1,0** alors que le zoom-out maximal atteint
  **0,15**. Ce n'est pas nécessairement un défaut (`fit:0` ⇒ l'état d'initialisation est zoom 1),
  mais **la référence écrite et le servi ne coïncident pas** — à trancher par ai-01.
- **Le contenu des 6 commits de wrappers entre `de763aa9` et HEAD** : non diffé ligne à ligne. Le
  live est en retard ; **ce que l'utilisateur n'a pas** n'est pas caractérisé ici.

---

## §6 — Ce que la mesure établit, en une phrase

**La couche servie est saine sur les capacités mesurées** (Cap 0 + 1/2/3/4/6/7/8/9, FR·AR·ZH pour
2/7/8), et elle porte **le blob `de763aa9` à l'octet** — 6 commits de wrappers derrière HEAD, sous une
empreinte de déploiement du **28/08 10:00:17**. **Verdict : ai-01.**

### Reproductibilité

Sondes dans le scratchpad de session : `probe_caps.py` (caps 0/1/2/3/4/6/7/8/9), `probe_v4.py`
(caps 2/7/8 corrigées, multi-langues). Toutes en lecture seule sur le live ; l'écart commité↔servi se
rejoue par `curl` + `git cat-file -s|blob`.

---

🤖 po-2023 — mesures lecture seule, base `35acac04`

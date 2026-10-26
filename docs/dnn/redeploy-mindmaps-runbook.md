# Runbook — Redéploiement des mindmaps servi (`www.argumentum.games`)

> **Version** : 1.2 (2026-10-26) · **Statut** : ⛔ **v1.1 SUPERSEDED — le rafraîchissement du 26/10 est appliqué.** Les **48** fichiers servis ont été **remplacés** depuis les blobs `f2f15389`, après sauvegarde des **48**. Mesuré sur ce qui est servi (cache-buster, octets) : **48/48 sha256 = manifeste**, **48/48 diffèrent de la couche `9cb615f0`**, `Last-Modified` du 26/10, **2/2 sentinelles conformes**, contre-témoin 404. La v1.1 décrivait un premier geste additif (16 remplacements + 32 ajouts) : depuis le 09/10 ces 32 fichiers sont **déjà servis**, donc tout geste ultérieur est un **remplacement des 48**. Mesures antérieures : [`docs/quality/830-mindmaps-servi-etat-2026-10-09.md`](../quality/830-mindmaps-servi-etat-2026-10-09.md). ⛔ **Le verdict §5 #7 reste `ai-01`** — l'opérateur de la mesure a mesuré des octets, pas un rendu. **Ops serveur seul** : à exécuter par l'opérateur qui détient l'accès au webroot.
> **Sources** : dry-run [#830 c.5646290386](https://github.com/ArgumentumGames/Argumentum/issues/830#issuecomment-5646290386) + balayage exhaustif du servi [#830 c.5646564558](https://github.com/ArgumentumGames/Argumentum/issues/830#issuecomment-5646564558) + ordre du rafraîchissement [#458 c.6412064657](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-6412064657). Base git : master `f2f15389`.
> ⚠️ **Dépôt public** : ce runbook ne contient volontairement aucune adresse d'infrastructure ni aucun nom de clé — ces détails vivent sur le dashboard interne.

**Motif** : un commentaire d'issue n'est pas un runbook. Le plan de copie ci-dessous était mesuré et complet mais vivait dans un fil GitHub ; cette page en fait un document versionné, cité et corrigeable en PR.

---

## 0. Invariants — à lire avant tout geste

1. **Le webroot n'est pas un checkout git.** Il est assemblé à la main et sert au moins **2 fichiers qui n'existent dans aucun commit** (§5). On ne sait pas en quoi d'autre il diverge du dépôt.
2. ⛔ **Interdits absolus** :
   - `robocopy /MIR` (ou tout miroir) — supprimerait les orphelins et tout ce qui n'a pas été énuméré ;
   - `git checkout` / `git restore` dans le webroot — le dépôt ne connaît pas son contenu réel ;
   - **publish DNN** — écrase le `web.config` de prod.
3. Le geste est **additif, fichier par fichier** : « jamais de suppression sans preuve de préservation ».
4. L'opération est intégralement réversible **à condition que** la sauvegarde du §3 précède le geste.

---

## 1. Le geste — 48 fichiers, 59 209 092 o, 48 remplacements

**Source** : `Cards/Fallacies/Mindmaps/<lang>/` @ master. Si master a avancé depuis `f2f15389` au moment du GO, **re-dériver d'abord les sha256** (méthode du manifeste, #458 c.6412064657) et mettre à jour ce runbook — le manifeste du geste fait foi.
**Cible** : racine du site servant `www.argumentum.games`.

| # | Source (par langue, ×8) | Nom cible à la racine | Renommage |
|---|---|---|---|
| 1 | `Fallacies_<lang>.html` | `Fallacies_<lang>.html` | non |
| 2 | `Argumentation_Virtues_<lang>.html` | `Virtues_<lang>.html` | **OUI — retirer `Argumentation_`** (mesuré : le stem committé répond 404, le stem court répond 200, sur les 8 langues) |
| 3 | `Fallacies_<lang>_ext.html` | `Fallacies_<lang>_ext.html` | non |
| 4 | `Argumentation_Virtues_<lang>_ext.html` | `Virtues_<lang>_ext.html` | **OUI — DÉRIVÉ** (par symétrie du canon ; servi depuis le 09/10, remplacé le 26/10. Le nom est libre — aucun wrapper n'est référencé par nom par un autre fichier — il fallait juste le fixer) |
| 5 | `Fallacies_<lang>.content.svg` | identique | non — **nom forcé**, référencé littéralement par le `_ext` |
| 6 | `Argumentum_Virtues_MindMap_<lang>.content.svg` | identique | non — **nom forcé**, référencé littéralement par le `_ext` |

**Volumes** : **48 remplacements, 59 209 092 o** — 16 canon (`Fallacies_<lang>.html` + `Virtues_<lang>.html`), 16 `_ext` et 16 `.content.svg`. Aucun fichier n'est ajouté ni supprimé : les 48 noms cibles sont **déjà servis** avant le geste (mesuré le 26/10), et le geste remplace leurs octets.

Les canon **embarquent leur SVG inline** (autoportants, copie simple). Les `_ext` **référencent** leur SVG externe — d'où le §2.

---

## 2. Dépendance SVG — les 16 `.content.svg` sont indivisibles des 16 `_ext`

Chaque `*_ext.html` référence exactement un SVG externe, au nom forcé. **Publier un `_ext` sans son SVG = une page 200 qui n'affiche rien** — un faux vert, strictement pire que le 404 actuel (qui est honnête). Règle : chaque `.content.svg` est copié **avant ou en même temps que** le `_ext` correspondant.

Manifeste des 16 SVG @ couche **servie avant le 26/10** `9cb615f0` (⚠️ **corrections 09/10 et 26/10/2026** : la mention d'origine — « aucun n'est servi aujourd'hui, les 16 partent de zéro » — est **fausse**. Les 16 `.content.svg` **sont servis** depuis le 09/10. Ce tableau décrit donc la **couche avant** le rafraîchissement du 26/10, pas une cible à créer ; le manifeste cible des 48 fichiers, avec leurs sha256 `f2f15389`, est dans #458 c.6412064657) :

| Langue | Famille | Fichier (= nom cible) | Taille (o) | sha256 (16 premiers) |
|---|---|---|---:|---|
| fr | F | `Fallacies_fr.content.svg` | 2 629 639 | `144d2496381f2e5d…` |
| fr | V | `Argumentum_Virtues_MindMap_fr.content.svg` | 434 442 | `9df2854bf95ed4b1…` |
| en | F | `Fallacies_en.content.svg` | 2 480 995 | `fb806f3845641133…` |
| en | V | `Argumentum_Virtues_MindMap_en.content.svg` | 421 104 | `95ea12496a28eda7…` |
| ru | F | `Fallacies_ru.content.svg` | 2 916 249 | `2bfd216cbeae73d7…` |
| ru | V | `Argumentum_Virtues_MindMap_ru.content.svg` | 489 838 | `297bf8c58ba221c4…` |
| pt | F | `Fallacies_pt.content.svg` | 2 626 611 | `f53c945db35589a2…` |
| pt | V | `Argumentum_Virtues_MindMap_pt.content.svg` | 445 737 | `815e45001681e728…` |
| es | F | `Fallacies_es.content.svg` | 2 569 385 | `7435aab5b72c8670…` |
| es | V | `Argumentum_Virtues_MindMap_es.content.svg` | 433 265 | `5872d649b665dff9…` |
| ar | F | `Fallacies_ar.content.svg` | 2 621 502 | `e0012aea9b02bbcb…` |
| ar | V | `Argumentum_Virtues_MindMap_ar.content.svg` | 464 117 | `0a1979fe44890623…` |
| fa | F | `Fallacies_fa.content.svg` | 2 727 765 | `a68f8b2ef5fdedcb…` |
| fa | V | `Argumentum_Virtues_MindMap_fa.content.svg` | 464 554 | `82cb0f158b536014…` |
| zh | F | `Fallacies_zh.content.svg` | 5 234 907 | `08ecb7a94c7e9b95…` |
| zh | V | `Argumentum_Virtues_MindMap_zh.content.svg` | 1 148 721 | `1abe32092a94b149…` |

**Total : 28 108 831 o (26,8 Mo).**

---

## 3. Sauvegarde préalable (obligatoire, AVANT le geste)

1. Sauvegarde **horodatée** des **48 fichiers actuellement servis** — le geste remplace les 48, donc la sauvegarde couvre les 48, pas seulement les 16 canon. (⚠️ **corrections 09/10 et 26/10/2026** : la parenthèse d'origine — « les blobs de `de763aa9`, déployés le 10/08 » — est **périmée**. La couche servie avant le 26/10 est `9cb615f0` (vérifié à l'octet). **Re-mesurer les 48 sha256 avant le geste** et sauvegarder **cette** couche : sauvegarder une autre couche ferait croire à un retour en arrière vers un état qui n'est pas l'état antérieur.)
2. Consigner l'état des **deux sentinelles orphelines** (tailles exactes, sha256 et mtime, §5) — la preuve de non-régression du §5 se compare à cet instantané.
3. **Porte avant copie** : les 48 sha256 servis doivent être **égaux au manifeste « servi avant »** et **différents de la cible**. Un seul écart arrête tout : quelqu'un a touché le site depuis la mesure.
4. **Ne pas toucher** au `web.config`.

---

## 4. Rollback

1. Restaurer les **48 fichiers** depuis la sauvegarde du §3, par le même geste fichier par fichier.
2. **Ne rien supprimer** : le geste est un remplacement des 48 noms déjà servis, pas une addition.
3. Le rollback est intégralement réversible — **à condition que la sauvegarde précède le geste** (§3).
4. Après rollback, rejouer la checklist du §5 : les 48 fichiers doivent retrouver les tailles/sha256 de la colonne « servi avant », et les sentinelles être inchangées.

---

## 5. Checklist post-déploiement (au cache-buster `?cb=<epoch>`)

Le cache de la racine est `max-age=600` (**10 minutes**, pas un an) — la propagation est rapide, le `Last-Modified` doit dater du geste.

> ⛔ **Les critères 1-3 et 5-6 étaient déjà satisfaits avant le geste** (mesuré 09/10, reconfirmé 26/10) : la production sert les **50 stems** et les **deux sentinelles conformes**. Cette checklist ne peut donc **pas** distinguer « geste fait » de « geste déjà fait » : elle est verte dans les deux cas, **y compris si l'opérateur ne fait rien**. ⇒ **Tout critère d'arrivée est relatif à l'état de départ** — relever les 48 sha256 **avant** le geste, puis exiger qu'ils **changent vers ceux du manifeste cible** `f2f15389`, jamais se contenter de « N fichiers répondent 200 ». Le 26/10, les 48 sha256 ont effectivement changé (`9cb615f0` → `f2f15389`), et c'est ce delta qui prouve le geste. Détail de méthode : [`docs/quality/830-mindmaps-servi-etat-2026-10-09.md`](../quality/830-mindmaps-servi-etat-2026-10-09.md) §3.1.

| # | Contrôle | Réussite si |
|---|---|---|
| 1 | **16 canon** (`Fallacies_<lang>.html`, `Virtues_<lang>.html`) | 200 **et** taille **et** sha256 = manifeste cible @ `f2f15389`, **et sha256 ≠ servi avant**. La taille seule ne suffit pas (Capacité 0). |
| 2 | **16 `_ext`** | 200 + taille et sha256 = manifeste cible @ `f2f15389`, **et sha256 ≠ servi avant** — un `_ext` dont le `.content.svg` n'est pas la cible est un faux vert, d'où le contrôle #3. |
| 3 | **16 `.content.svg`** | 200 + taille et sha256 = manifeste cible @ `f2f15389`, **et sha256 ≠ servi avant**. |
| 4 | **Contrôle inverse** | un stem inexistant (ex. `virtues_zz.html`) reste **404** — l'instrument doit pouvoir voir l'échec. |
| 5 | **Sentinelle `fallacies_fr.links.svg`** | reste **200 à exactement 1 552 866 o** (sha256 `e47d5c6a…`). Tout changement = **le geste n'a pas été non destructif**. |
| 6 | **Sentinelle `virtues_fr.svg`** | reste **200 à exactement 555 208 o** (sha256 `b6df4cab…`) — même règle. Ces deux fichiers n'existent dans **aucun commit** : un miroir les détruirait, seul un geste fichier par fichier les laisse intacts. |
| 7 | Passe comportementale des 9 capacités de #830 sur le servi | Capacité 0 verte d'abord. |

Inventaire de référence **avant le premier geste** (12/09) : **18 fichiers servis** (16 canon + 2 orphelins), **96 stems 404** — tableau complet en [#830 c.5646564558](https://github.com/ArgumentumGames/Argumentum/issues/830#issuecomment-5646564558). **État avant le rafraîchissement du 26/10** : **50 servis** — les 48 cibles (16 canon + 16 `_ext` + 16 `.content.svg`) et 2 sentinelles. **Après le rafraîchissement** : **50 servis, mêmes noms**, les 48 cibles ont changé d'octets vers `f2f15389`, sentinelles inchangées, aucune addition ni suppression.

> ✅ **État MESURÉ du 26/10/2026** : 48/48 sha256 servis = manifeste `f2f15389` et ≠ couche avant `9cb615f0`, `Last-Modified` du 26/10, 2/2 sentinelles conformes, contre-témoin 404. La mesure du 09/10 décrivait le premier geste (50 stems, couche `9cb615f0`) ; c'est précisément pour cela que le contrôle d'arrivée du 26/10 est **relatif au sha256 d'avant** (§ note ci-dessus).

---

## 6. Hors périmètre — décisions owner, pas ce runbook

- **Retrait des 2 orphelins** (`fallacies_fr.links.svg`, `virtues_fr.svg`) : décision owner, distincte du redéploiement. Tant qu'elle n'est pas prise, le geste les **préserve** (§5).
- Les autres 404 (triplet `cards`, templates racine `external.html`/`included.html`, `.links.svg` hors fr) : hors périmètre — l'inventaire c.5646564558 sert de référence si l'owner les veut servis un jour.
- **Verdict post-déploiement = ai-01** (barème #830) ; l'opérateur du geste exécute, ne déclare pas PASS.

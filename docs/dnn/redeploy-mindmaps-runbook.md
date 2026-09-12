# Runbook — Redéploiement des mindmaps servi (`www.argumentum.games`)

> **Version** : 1.0 (2026-09-12) · **Statut** : PRÊT — en attente de GO owner. **Ops serveur seul** : à exécuter par l'opérateur qui détient l'accès au webroot (ce n'est une lane d'aucun agent).
> **Sources** : dry-run [#830 c.5646290386](https://github.com/ArgumentumGames/Argumentum/issues/830#issuecomment-5646290386) + balayage exhaustif du servi [#830 c.5646564558](https://github.com/ArgumentumGames/Argumentum/issues/830#issuecomment-5646564558). Base git : master `9cb615f0`.
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

## 1. Le geste — 48 fichiers, 56,3 Mo, copie additive

**Source** : `Cards/Fallacies/Mindmaps/<lang>/` @ master. Si master a avancé depuis `9cb615f0` au moment du GO, **re-dériver d'abord les sha256** (méthode du manifeste, #830 c.5645066675) et mettre à jour ce runbook — les empreintes du §2 font foi.
**Cible** : racine du site servant `www.argumentum.games`.

| # | Source (par langue, ×8) | Nom cible à la racine | Renommage |
|---|---|---|---|
| 1 | `Fallacies_<lang>.html` | `Fallacies_<lang>.html` | non |
| 2 | `Argumentation_Virtues_<lang>.html` | `Virtues_<lang>.html` | **OUI — retirer `Argumentation_`** (mesuré : le stem committé répond 404, le stem court répond 200, sur les 8 langues) |
| 3 | `Fallacies_<lang>_ext.html` | `Fallacies_<lang>_ext.html` | non |
| 4 | `Argumentation_Virtues_<lang>_ext.html` | `Virtues_<lang>_ext.html` | **OUI — DÉRIVÉ** (par symétrie du canon ; aucun `_ext` n'est servi aujourd'hui, rien ne mesure ce nom. Il est libre — aucun wrapper n'est référencé par nom par un autre fichier — il fallait juste le fixer) |
| 5 | `Fallacies_<lang>.content.svg` | identique | non — **nom forcé**, référencé littéralement par le `_ext` |
| 6 | `Argumentum_Virtues_MindMap_<lang>.content.svg` | identique | non — **nom forcé**, référencé littéralement par le `_ext` |

**Volumes** : 16 canon = 28,1 Mo (**remplacement** — les seuls fichiers écrasés) · 16 `_ext` = 1,4 Mo (ajout) · 16 `.content.svg` = 26,8 Mo (ajout).

Les canon **embarquent leur SVG inline** (autoportants, copie simple). Les `_ext` **référencent** leur SVG externe — d'où le §2.

---

## 2. Dépendance SVG — les 16 `.content.svg` sont indivisibles des 16 `_ext`

Chaque `*_ext.html` référence exactement un SVG externe, au nom forcé. **Publier un `_ext` sans son SVG = une page 200 qui n'affiche rien** — un faux vert, strictement pire que le 404 actuel (qui est honnête). Règle : chaque `.content.svg` est copié **avant ou en même temps que** le `_ext` correspondant.

Manifeste des 16 SVG @ master `9cb615f0` (aucun n'est servi aujourd'hui — les 16 partent de zéro) :

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

1. Sauvegarde **horodatée** des **16 canon actuellement servis** — les seuls fichiers que le geste écrase. (Ils sont aujourd'hui les blobs de `de763aa9`, déployés le 10/08 16:55 GMT.)
2. Consigner l'état des **deux sentinelles orphelines** (tailles exactes, §5) — la preuve d'additivité du §5 se compare à cet instantané.
3. **Ne pas toucher** au `web.config`.

---

## 4. Rollback

1. Restaurer les 16 canon depuis la sauvegarde du §3.
2. Supprimer les 32 fichiers ajoutés (16 `_ext` + 16 `.content.svg`).
3. Le rollback est intégralement réversible — **à condition que la sauvegarde précède le geste** (§3).
4. Après rollback, rejouer la checklist du §5 : les 16 canon doivent retrouver leurs tailles/sha256 d'origine, les 32 ajoutés redevenir 404, les sentinelles être inchangées.

---

## 5. Checklist post-déploiement (au cache-buster `?cb=<epoch>`)

Le cache de la racine est `max-age=600` (**10 minutes**, pas un an) — la propagation est rapide, le `Last-Modified` doit dater du geste.

| # | Contrôle | Réussite si |
|---|---|---|
| 1 | **16 canon** (`Fallacies_<lang>.html`, `Virtues_<lang>.html`) | 200 **et** taille **et** sha256 = manifeste @ master. La taille seule ne suffit pas (Capacité 0). |
| 2 | **16 `_ext`** | 200 **et rendu non vide** — pas seulement le code HTTP : un `_ext` 200 sans son SVG est un faux vert. |
| 3 | **16 `.content.svg`** | 200 + sha256 du §2. |
| 4 | **Contrôle inverse** | un stem inexistant (ex. `virtues_zz.html`) reste **404** — l'instrument doit pouvoir voir l'échec. |
| 5 | **Sentinelle `fallacies_fr.links.svg`** | reste **200 à exactement 1 552 866 o** (sha256 `e47d5c6a…`). Tout changement = **le geste n'a pas été additif**. |
| 6 | **Sentinelle `virtues_fr.svg`** | reste **200 à exactement 555 208 o** (sha256 `b6df4cab…`) — même règle. Ces deux fichiers n'existent dans **aucun commit** : un miroir les détruirait, seul un geste additif les laisse intacts. |
| 7 | Passe comportementale des 9 capacités de #830 sur le servi | Capacité 0 verte d'abord. |

Inventaire de référence avant geste (pour comparaison) : **18 fichiers servis** (16 canon + 2 orphelins), **96 stems 404** — tableau complet en [#830 c.5646564558](https://github.com/ArgumentumGames/Argumentum/issues/830#issuecomment-5646564558). Après le geste : **50 servis attendus** — 18 existants dont 16 canon écrasés (contenu nouveau), 2 sentinelles inchangées, + 32 ajoutés (16 `_ext` + 16 `.content.svg`).

---

## 6. Hors périmètre — décisions owner, pas ce runbook

- **Retrait des 2 orphelins** (`fallacies_fr.links.svg`, `virtues_fr.svg`) : décision owner, distincte du redéploiement. Tant qu'elle n'est pas prise, le geste les **préserve** (§5).
- Les autres 404 (triplet `cards`, templates racine `external.html`/`included.html`, `.links.svg` hors fr) : hors périmètre — l'inventaire c.5646564558 sert de référence si l'owner les veut servis un jour.
- **Verdict post-déploiement = ai-01** (barème #830) ; l'opérateur du geste exécute, ne déclare pas PASS.

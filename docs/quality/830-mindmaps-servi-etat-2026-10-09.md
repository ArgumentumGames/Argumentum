# 830 — État servi des mindmaps au 2026-10-09 : le redéploiement est APPLIQUÉ (mesuré), et trois documents disaient le contraire

> **Mesuré par** : lane `myia-web2` (machine de prod), 2026-10-09 ~14:0xZ, sur les **octets servis** de `https://www.argumentum.games`.
> **Objet** : le runbook [`docs/dnn/redeploy-mindmaps-runbook.md`](../dnn/redeploy-mindmaps-runbook.md) déclarait « PRÊT — en attente de GO owner » et « les 16 `.content.svg` partent de zéro ». **Les deux affirmations sont fausses aujourd'hui.**
> **Ce que ce document ne dit pas** : ni qui a posé la couche, ni quand, ni si le rendu est correct (critère §5 #7 — verdict `ai-01`).

---

## 1. Mesure — checklist §5 du runbook, critères 1 à 6

`?cb=<epoch>` sur chaque requête. Le recensement utilise **HEAD pour le code HTTP**, les octets viennent de **GET + `Get-FileHash`**.

| # | Critère §5 | Mesure du 09/10 | Verdict |
|---|---|---|---|
| 1 | **16 canon** | **50/50 stems attendus répondent 200**. Identité à l'octet sur 2 canon : `Fallacies_fr.html` 2 711 714 o `56a9975d42016c11` · `Virtues_fr.html` 521 917 o `e7895bc080d73be1` | ✅ = blob `9cb615f0` |
| 2 | **16 `_ext`** | `Fallacies_fr_ext.html` 200 · 91 269 o `0caafb9213dccedc` | ✅ = blob `9cb615f0` |
| 3 | **16 `.content.svg`** | `Fallacies_fr.content.svg` 2 629 639 o `144d2496381f2e5d` · `Argumentum_Virtues_MindMap_zh.content.svg` 1 148 721 o `1abe32092a94b149` | ✅ = blob `9cb615f0` |
| 4 | **Contrôle inverse** | `virtues_zz.html` **404** · `fallacies_zz.html` **404** · `Fallacies_fr_ext.svg` **404** | ✅ l'instrument voit l'échec |
| 5 | **Sentinelle `fallacies_fr.links.svg`** | 200 · **1 552 866 o** · `e47d5c6aa4e9bed9` | ✅ conforme — **geste additif** |
| 6 | **Sentinelle `virtues_fr.svg`** | 200 · **555 208 o** · `b6df4cab02b32773` | ✅ conforme — **geste additif** |
| 7 | Passe comportementale des 9 capacités | **non mesuré ici** | ⛔ verdict `ai-01` |

Six fichiers vérifiés **un par un** contre le blob git (`git cat-file -p <ref>:<chemin> | sha256sum`), couvrant les trois familles de la table §1 (canon inliner, `_ext`, `.content.svg`) **et deux langues** (`fr`, `ar`). **6/6 byte-identiques à `9cb615f0`.**

**Inventaire servi = 50** : 16 canon + 16 `_ext` + 16 `.content.svg` + 2 sentinelles orphelines. C'est **exactement** l'inventaire post-geste que le §5 du runbook prédit (« Après le geste : 50 servis attendus »). Les **32 fichiers additifs** (16 `_ext` + 16 `.content.svg`) sont donc **servis**, alors qu'ils étaient à 404 dans l'inventaire de référence du 12/09 ([#830 c.5646564558](https://github.com/ArgumentumGames/Argumentum/issues/830#issuecomment-5646564558), « 18 fichiers servis, 96 stems 404 »).

⇒ **Le geste décrit par le runbook est appliqué en production.**

---

## 2. Instrument, et ce qui l'a validé

| Élément | Pourquoi |
|---|---|
| **HEAD pour le recensement, GET pour l'identité** | `HEAD` ne rend pas de `Content-Length` exploitable sur cet hôte (les SVG rendent `System.String[]`, le HTML `-`) : un `200` sans taille peut être un *soft-404* DNN. **HEAD ne sert donc que le code HTTP** ; aucun octet, aucun sha n'en vient. |
| **Contrôle inverse ×3** | Un instrument qui ne peut pas voir un échec ne prouve rien. Trois stems inexistants répondent bien **404**. |
| **Comparaison au BLOB, pas à l'arbre de travail** | `git cat-file -p <ref>:<chemin>` rend les octets **tels que committés** ; l'arbre de travail est filtré (`core.autocrlf`). Comparer le servi à un fichier de travail produirait un faux écart. |
| **⭐ Contrôle positif du correctif #1831** | Le motif `direction: ltr` rend **0** dans le blob prod servi et **2** dans le blob master. Le **même** motif, sur le **même** instrument, **voit** la valeur quand elle est là. ⇒ le zéro servi est une **absence réelle**, pas une panne de motif. |

⚠️ Sans ce contrôle positif, `text-anchor = 0` sur un SVG de 2,7 Mo aurait pu se lire « le fichier est vide » — le blob master n'en porte **qu'un**. *Un `0` n'est une absence que si l'instrument pouvait voir un `1`.*

---

## 3. Trois conséquences pour le runbook

### 3.1 Le critère d'arrivée du §5 ne discrimine plus
« 50 servis » est **déjà vrai avant tout geste**. Le contrôle ne peut donc plus distinguer *geste fait* de *geste déjà fait* — il est vert dans les deux cas, y compris si l'opérateur ne fait rien.

⇒ **Un critère d'arrivée doit être relatif à l'état de départ.** Pour un rafraîchissement, la forme correcte est : *le sha256 servi a changé depuis `<sha de départ>` et vaut `<sha cible>`*, pas *« N fichiers répondent 200 »*.

### 3.2 Le §3 (sauvegarde) vise une couche qui n'est plus servie
Le §3 prescrit de sauvegarder « les 16 canon actuellement servis — **les blobs de `de763aa9`**, déployés le 10/08 ». Or le servi est `9cb615f0`. Un opérateur qui suit le §3 tel quel sauvegarde pour référence une couche qui n'est **déjà plus** en face de lui, et croira restaurer l'état antérieur.

*(Contexte, non mesuré ici : `de763aa9` était la couche de l'**ancienne** production, mesurée jusqu'au 17/09 — `docs/quality/1066-balayage-servi-depot-2026-09-15.md`, `docs/quality/1180-*.md`. La bascule vers web2 du 01/10 est un **candidat** d'explication du changement de couche ; ce document ne l'établit pas.)*

### 3.3 Le prochain geste est un RAFRAÎCHISSEMENT, pas un déploiement

| | Couche | Preuve |
|---|---|---|
| **Prod servie** | `9cb615f0` | 6/6 fichiers byte-identiques (ci-dessus) |
| **master** `37aae03a` | divergente sur **6/6** | `Fallacies_fr.html` 2 721 627 o `1914e898…` · `Fallacies_ar.html` 2 721 828 o `2993e1f9…` · `Argumentation_Virtues_fr.html` 520 462 o `50f460da…` · `Fallacies_fr_ext.html` 91 846 o `30f2a00c…` · `Argumentum_Virtues_MindMap_zh.content.svg` 1 148 515 o `3784bb55…` |

**Le correctif `direction: ltr` de #1831 est ABSENT de la prod** (0 occurrence dans le blob servi, 2 dans le master ; contrôle positif au §2). Le prochain geste emporte donc ce correctif, et sa vérification se fait **au sha**, pas au code HTTP.

---

## 4. Reste ouvert — hors de cette lane

1. **Verdict §5 #7** (passe comportementale des 9 capacités de #830) : **`ai-01`**. Cette lane mesure des octets, pas un rendu. *(Rappel : une comparaison octet à octet prouve la provenance, jamais la justesse.)*
2. **Qui a posé la couche, et quand** : non déterminable depuis l'état servi. Cette lane ne l'affirme pas.
3. **Cible du prochain geste** : rafraîchir depuis `master` (emporte #1831) ou maintenir `9cb615f0` — arbitrage **owner**, comme le §6 le prévoit pour les décisions de périmètre.
4. **Retrait des 2 orphelins** (`fallacies_fr.links.svg`, `virtues_fr.svg`) : décision owner, **distincte** du redéploiement (§6 du runbook). Tant qu'elle n'est pas prise, le geste les **préserve** — mesuré conforme ce jour.

---

*Aucune écriture sur la machine servante n'a été faite pour produire ce document : uniquement des `GET` et des `HEAD`.*

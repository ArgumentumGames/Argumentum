# DNN Visual Diff — Runbook (prod ↔ préprod)

> **Outil** : `tools/dnn-visual-diff/visual_diff.py` (Python local, Playwright + Pillow).
> **Portée** : comparaison **visuelle client, lecture seule** de
> `https://www.argumentum.games` (prod) et `https://dnn.argumentum.myia.io` (préprod).
> **Provenance du design** : mesures #1180 (É1–É7, août 2026) — double métrique pixel,
> contrôle inverse, classification des deltas connus, discipline cache-buster.
> **Hors périmètre** : audit `Default.aspx`/assemblages (file de diagnostic po-2023,
> dernier commentaire #1180 — ne pas dupliquer), tout diagnostic serveur, toute mutation,
> tout login (règle §3).

---

## 0. Prérequis & installation

- **Python 3** (développé/testé 3.14.3) ; **Pillow** pour le moteur de diff
  (développé 12.2.0) ; **Playwright + Chromium** pour la capture uniquement
  (`--self-test`, `unittest` et le mode `report` n'exigent ni réseau ni
  navigateur).

```bash
python -m pip install pillow playwright
python -m playwright install chromium
```

- `routes.json` couvre home, les pages DNN actives, `produit-detail`
  (paramètres complets), et le **wrapper mindmap statique
  `mindmap-fallacies-fr`, ACTIF par défaut** (`enabled` absent = true ;
  16 URLs canoniques {fallacies,virtues}_{8 langues}.html existent —
  #1066 : 404 en préprod, documenté).

## 1. Usage

```bash
cd tools/dnn-visual-diff

# auto-tests hors ligne (aucun réseau, playwright non requis)
python visual_diff.py --self-test
python -m unittest test_visual_diff -v

# capture + rapport (chaque run dans un répertoire NEUF — jamais d'écrasement)
python visual_diff.py --out <repertoire-neuf>

# capture seule, puis rapport reconstruit HORS LIGNE depuis le manifest
python visual_diff.py --only home --mode capture --out <rep>
python visual_diff.py --mode report --out <rep>   # re-vérifie les empreintes SHA256
# (--max-mp y est aussi actif : la limite effective = min(opérateur, manifest))

# une route + détection de nondéterminisme même-hôte (double capture par hôte)
python visual_diff.py --only home --double-capture --out <rep>

# paramètres : viewport WxH validé (>0), --timeout >0, --max-mp >0 (garde mémoire)
python visual_diff.py --viewport 1440x900 --timeout 90 --max-mp 40 --out <rep>
```

Sorties dans `--out` : `report.html` (rapport local, liens vers montage réduit **et**
PNG bruts), `manifest.json` (entrées de capture + verdicts + empreintes SHA256 +
`source` = chemin de l'outil + révision git), un PNG par hôte et par route, montages
`<slug>_sbs.png` et heatmaps `<slug>_heat.png`. **Un run dont le répertoire contient
déjà un `manifest.json` refuse de s'exécuter** — les captures et manifests des runs
précédents, même en échec, restent auditable (modes `full` et `capture`).
**Le mode `report` RÉÉCRIT le manifest** (il y ajoute `rows` +
`inverse_control`) : pour reconstruire un rapport sans altérer les preuves
d'origine, **copier le répertoire d'abord** et viser la copie — les SHA des
captures d'origine restent alors inchangés et vérifiables.

**Code retour** : en modes `full` et `report`, 0 = chaque route a un verdict
comparable non-échouant ; 1 = échec global (zéro paire, instrument aveugle,
≥1 route ERROR / NO-VERDICT / HTTP-ERROR / ATTENDU-HTTP). `DIFF-A-CLASSER` et
`ECART-DIMENSIONS` sont des **résultats**, pas des échecs d'outil.
**En mode `capture`, exit 0 signifie uniquement que le manifest a été écrit** —
pas que les captures ont réussi : les erreurs par route y sont enregistrées et
seront qualifiées au prochain `--mode report`.

## 2. Paires de routes

`routes.json` — mesuré depuis le sitemap prod (`sitemap.aspx`, 18 URLs) et
#1180/#1066. Actives par défaut : les 5 pages publiques + la fiche produit **avec ses
paramètres complets** (`/Acheter-le-jeu/Detail/catid/140/eid/130/...` — le `Detail` nu
répond 000 des deux côtés). Désactivées par défaut : `Search-Results`, `Amis`, pages
compte/paiement (`expect_login_redirect`), `Activity-Feed*` (`expected_status_both:
404`). Le **wrapper mindmap statique est ACTIF par défaut** (cf. §0 — fichier
statique sans marqueurs DNN, fraîcheur `unknown`, aucune attente déclarée).

Ajouter une route = éditer `routes.json` (jamais le code). Doublons de slug, paires
incomplètes et **toute config produisant zéro paire** = erreurs explicites.

## 3. Règles dures

1. **Lecture seule stricte** — navigation + capture. Pas de login, pas de formulaire,
   pas de diagnostic serveur (un login créerait un état de session sur l'hôte).
2. **Viewports identiques** (1440×900 par défaut, validés > 0, `device_scale_factor=1`),
   même navigateur, captures séquentielles par route.
3. **Cache-buster tracé, fraîcheur jamais présumée** — `?cb=<epoch>` est réécouté en
   segment de chemin sur la racine (301 mesuré) et reste une query ailleurs ; le sort
   du paramètre (`kept-query` / `rewritten-to-path` / `lost`) est consigné par capture.
   Les **marqueurs identifient le build qui a répondu, pas le fait qu'il est courant** :
   un HTML ancien porte ses anciens marqueurs. `expected_markers` compare les marqueurs
   observés aux attentes déclarées et rend **matched / mismatched / none-declared**
   (un dictionnaire vide ne déclare RIEN). Une attente **mismatched** = erreur de
   route explicite (mauvais build suspecté). La **fraîcheur reste « unknown » dans
   tous les cas** : aucune preuve de déploiement indépendante n'existe côté client,
   et l'outil n'invente pas de protocole de fraîcheur.
4. **Aucun masque par défaut.** `--double-capture` fait une A/B même-hôte. Une seconde
   capture inutilisable — limite mémoire, screenshot absent, HTTP ≥ 400, capture
   suspecte — rend la route **NO-VERDICT** (jamais ignorée silencieusement) ; les
   contrôles (HTTP, garde d'en-tête, suspicion) passent TOUS avant `compute_diff`,
   et l'échec A/B est un **état structuré** (`ab_verdict`) que le rapport honore :
   la route rend NO-VERDICT **avant** la comparaison prod/préprod (aucune métrique,
   aucun artefact de diff).
   Si l'écart fort dépasse le seuil, la route rend NO-VERDICT (nondéterminisme) avec
   trace complète (fichier, dimensions, SHA256, statut, horodatage de la seconde
   capture) — jamais un PASS masqué. **Un écart de dimensions entre les deux
   chargements même-hôte (p.ex. 64x64 vs 64x128 à zone commune identique) est AUSSI
   du nondéterminisme** : une stabilité prouvée sur la seule zone commune est
   invalide — `dims_match=false` fait basculer le verdict même à écart fort nul.
5. **Jamais de redimensionnement pour comparer.** Un écart de dimensions est un
   verdict ECART-DIMENSIONS (métriques sur la zone commune, étiquetées). Les copies
   d'affichage du montage peuvent être réduites ; elles ne sont **jamais** une entrée
   de verdict, et le rapport lie les PNG bruts.
6. **Garde mémoire AVANT tout** — les dimensions du document sont mesurées dans le
   navigateur **avant** le screenshot (seconde capture incluse) ; au-delà de
   `--max-mp` la capture n'a pas lieu et la route rend NO-VERDICT (memory-limit).
   Sur fichier, la garde lit l'en-tête PNG **sans décoder** les pixels. En mode
   report, les dimensions sont **revalidées depuis les en-têtes des fichiers**
   avant tout décodage — les drapeaux du manifest ne sont jamais crus seuls (un
   manifest altéré ne peut pas faire entrer un PNG surdimensionné dans la
   comparaison). **En mode report, la limite effective est
   `min(--max-mp opérateur, garde du manifest)` — la limite la plus stricte
   gagne**, un opérateur peut donc reserrer la garde a posteriori sans recapturer.
   Une capture ayant déclenché la garde n'est **jamais comparée**.
   **Chaque capture existante DOIT avoir une empreinte SHA256 stockée** — primaire
   comme seconde capture : une empreinte absente est un échec explicite, jamais
   acceptée en silence. Le disque est balayé pour toute capture secondaire
   `<slug>_<hôte>_b.png` **même quand la trace nondéterminisme est vide** — un
   fichier orphelin sans trace est un échec explicite (aucune capture ne reste
   inexpliquée).
7. **Porte HTTP avant tout pixel** — tout statut ≥ 400 (ou une asymétrie de statut)
   rend un verdict échouant : HTTP-ERROR, ou ATTENDU-HTTP quand le statut d'erreur
   était déclaré attendu (état distinct, échec global quand même). **Une page
   d'erreur ne peut jamais être comparée en IDENTIQUE** — même si elle rend
   pareil des deux côtés.
8. **Captures suspectes signalées** — un PNG uniforme (couleur unique) ou minuscule
   est marqué suspect et rend ERROR : une page blanche ou un écran d'erreur n'est
   jamais assimilé à un rendu valide.
9. **Contrôle inverse obligatoire** à chaque rapport (paire synthétique à différence
   connue, ~18,75 % d'écart fort attendu). Échec ⇒ INSTRUMENT-ERROR, tous les
   verdicts du run sont annulés.
10. **Erreurs contenues par route** — capture, comparaison, empreinte : une erreur
    d'une route n'arrête jamais les suivantes ; l'échec global est explicite.
11. **Aucun verdict pixel ne vaut une validation ergonomique ou esthétique.**
    L'outil classe des écarts techniques ; le jugement de goût appartient au regard
    humain (jsboige, Adeline, Thomas). Le rapport existe pour **alimenter** ce
    regard, pas pour le remplacer.

## 4. Sémantique des verdicts et du seuil

⚠️ **Le seuil de 0,05 % est DESCRIPTIF, pas causal.** Un verdict IDENTIQUE-SOUS-SEUIL
dit seulement : l'écart fort mesuré est sous ce seuil. Il **n'établit pas** la cause du
résidu — « bruit d'anticrénelage » était une lecture d'UNE mesure d'août (0,0039 %
sur `/Règles`), pas une loi ; l'outil ne l'affirme plus. Une route sous le seuil reste
ouvrable dans le rapport (PNG bruts, heatmap).

| verdict | sens | action |
|---|---|---|
| `IDENTIQUE-SOUS-SEUIL` | écart fort ≤ 0,05 % (descriptif — cause non établie par l'outil) | consultable ; pas une exemption de revue |
| `DELTA-CONNU` | écart sous le seuil **et** deltas du registre §5 présents | vérifier que chaque delta a le sens attendu |
| `DIFF-A-CLASSER` | écart fort > seuil, **y compris avec des deltas connus** (le registre §5 ne couvre pas cet écart) | **à revérifier** — queue #1180 |
| `ECART-DIMENSIONS` | dimensions différentes | écart explicite ; dire quel hôte change |
| `HTTP-ERROR` / `ATTENDU-HTTP` | erreur HTTP (même déclarée attendue) — aucune comparaison pixel | diagnostiquer ; échec global |
| `ERROR` | capture impossible/suspecte, attentes de marqueurs non satisfaites (la fraîcheur reste toujours `unknown`), empreinte modifiée, erreur de comparaison | la route n'a PAS de verdict |
| `NO-VERDICT` | nondéterminisme même-hôte ou limite mémoire | re-run ; exclusion documentée si récurrent |

## 5. Registre des deltas connus (D1–D7)

Relevés mesurés les 25-26/08/2026 (#1180) — un **instantané historique**, pas un état
permanent : re-vérifier chaque delta au moment d'agir (le libellé D4 notamment
reprend une mesure d'août, qui peut avoir changé depuis).

| id | delta relevé alors | sens relevé alors |
|---|---|---|
| D1 | 2sxc `tosic_sexycontent` 15.2.0 (prod) → `tosic.sxc` 21.7.0 (préprod) | attendu au cutover (6 majeures) |
| D2 | `jquery-migrate` présent prod, absent préprod (page `/`) | validé au chargement (É5) ; se joue au clic |
| D4 | `<noscript>` GTM absent préprod (mesure du 25/08) | manque relevé côté préprod à cette date |
| D5 | versionnage cache `cdv` 181 → 325 | correctif préprod, arrive au go-live |
| D6 | jQuery 3.5.1 → 3.7.1 | probable raison du retrait de migrate |
| D7 | bloc `@font-face` mort en `http://` retiré en préprod | préprod meilleure (3 Mixed Content en moins) |

⚠️ La recette **ne peut pas** s'écrire « préprod == prod » : plusieurs deltas relevés
vont dans le sens préprod meilleure. L'outil **classe**, il ne mesure pas une distance
de conformité. Un marqueur observé n'établit ni la fraîcheur (§3.3) ni la cause d'un
écart visuel.

## 6. Publication des résultats

Rapport HTML et manifest = **artefacts locaux** (ne pas committer les PNG). Le
**résumé** (verdicts + deltas classés + routes à revérifier) va sur le dashboard et/ou
en commentaire de #1180. Les décisions se gravent sur l'issue GitHub, pas seulement
sur le dashboard (le dashboard se condense — mémoire
`hazard_dashboard_condensation_resurrects_retracted_claims`).

## 7. Limites assumées

- L'outil compare le **chargement**, pas l'**interaction** : le risque D2
  (jquery-migrate retiré) se manifeste au clic — non couvert.
- La page d'achat ne rend **aucun contrôle transactionnel** (rupture, mesuré 25/08
  des deux côtés) : le paiement reste non validé jusqu'au restockage (#1188).
- Un verdict n'est valable que pour l'instant de sa capture : re-run après chaque
  déploiement, jamais réutilisation d'un manifest ancien.

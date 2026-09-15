# #830 — État des 9 capacités du golden-master comportemental : assertée par quoi, couverture, suite

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base** : master `608a0fd9`
**Instrument** : lecture du harness `MindmapWrapperCapabilitiesTests.cs` (méthodes + InlineData
extraites mécaniquement), du workflow `mindmap-wrapper-behaviour.yml` (job `behaviour`),
du registre `visual-tests-release-gate.md` (38→39→43), des verdicts #830 (ai-01
c.5649496738, c.5651920638 ; passe web1 c.5650709479). Issue lue intégralement (body + 25
commentaires). **Aucun code modifié** — ce dossier est le socle demandé pour la barre de régén.
Positionnement : la couche **servie** est couverte par [`830-served-wrapper-revalidation.md`](830-served-wrapper-revalidation.md)
(po-2023) ; la politique d'exécution et le registre des comptes par [`visual-tests-release-gate.md`](visual-tests-release-gate.md) — le présent dossier répond à une question distincte : **quelle capacité est assertée par quoi, aujourd'hui**.

---

## §0 La réponse en une table (MESURÉ sur `608a0fd9`)

Les **9 capacités produit (Cap 1-9) ont toutes une assertion directe**, exécutée **en CI** à
chaque PR/push master par le job `behaviour` (fixtures **committées** : wrappers du dépôt +
`.content.svg`, Chromium auto-provisionné, zéro `continue-on-error`, 43 cas). **Cap 0
(intégrité de l'instrument) n'est pas — et structurellement ne peut pas être — dans cette CI** :
elle porte l'artefact **servi** (cache-bust `?cb=`, taille relevée avant grep, stem servi), or la
CI rend des fichiers locaux sans réseau ; elle vit dans le protocole verdict (§2).

| Cap | Assertée par (méthode) | Couverture InlineData | En CI depuis |
|---|---|---|---|
| **1** zoom initial lisible + recentrage | `Cap1_InitialZoom_AfterSettle_ExceedsFit_RecentringRan` | **fr:F · ar:F · zh:F · fr:V** — la seule à la barre complète | 2026-09-11 (#1368) |
| **2** centrage racine | `Cap2_Recentring_CentersRootNode_InViewport` | fr:F · zh:F | 2026-09-11 |
| **3** pan (drag) | `Cap3_Drag_PansViewport` | fr:F | 2026-09-11 |
| **4** zoom molette | `Cap4_Wheel_ZoomsViewport` | fr:F | 2026-09-11 |
| **5** icônes de contrôle **effectives** | `Cap5_ControlIcons_PresentClickableAndEffective` | fr:F · fr:V | 2026-09-14 (mutation : présence≠efficacité) |
| **6** double-clic zoom | `Cap6_DoubleClick_ZoomsViewport` | fr:F | 2026-09-11 |
| **7** clic nœud → carte | `Cap7_ClickRealNode_OpensOverlayCard` | fr:F | 2026-09-11 |
| **8** couleurs de famille | `Cap8_ClickNode_AppliesFamilyClassAndColoursOverlay` | fr:F | 2026-09-11 |
| **9** bornes zoom (reset ≠ zoom-out max) | `Cap9_ResetAndZoomOutMaxAreDistinctStates` | fr:F · fr:V | 2026-09-14 (mutation `minZoom 0.15→1` → rouge, corpus restauré) |
| **0** intégrité instrument | **aucune en CI — par conception** (voir §2) | protocole live | n/a (geste verdict) |

La falsifiabilité des 3 plus récentes est **prouvée par mutation** (Cap5 : un contrôle inerte
est présent+cliquable — l'assertion exige le déplacement ; Cap9 : `minZoom:1` fait rougir ;
consignées dans le registre).

## §1 L'asymétrie que l'état révèle (le vrai reste à faire côté CI)

La barre d'acceptation de l'issue — « les 9 PASS sur **FR (Fallacies + Virtues)** + **1 RTL**
+ **1 CJK** » — n'est réalisée **en CI** que par **Cap 1**. Mesuré :

- Cap 3, 4, 6, 7, 8 : **fr Fallacies seul** — ni RTL, ni CJK, ni Virtues.
- Cap 2 : fr + zh (CJK oui, **RTL non**, Virtues non).
- Cap 5, 9 : FR × 2 familles (Virtues oui, **RTL/CJK non**).

La barre complète a été **tenue une fois au live** (passe web1 du 13/09 : fr/ar/zh × 9
capacités = 36/36, verdict **PASS** ai-01 c.5651920638) — mais ce fut un **geste humain
événementiel**, pas un invariant : rien ne re-vérifie RTL/CJK sur Cap 3-9 au prochain merge.

## §2 Ce que chaque couverture ne dit pas (rôles, par conception)

- **Cap 0 + « commité ≠ servi »** = couche **servie**, hors CI par nature : c'est le protocole
  verdict (cache-bust, taille servie vs blob, stems servis minuscules, `_ext` 404). Référence :
  `830-served-wrapper-revalidation.md` + les 2 verdicts ai-01 des 12-13/09. La CI est **aveugle
  au déploiement** — un vert `behaviour` ne dit rien de ce que voit l'utilisateur tant que le
  live n'est pas re-mesuré (écart 6 commits constaté le 12/09).
- **`_ext` 404 (16/16)** = trou réel #1040, hors périmètre du harness (les fixtures `_ext` ne
  sont pas committées comme wrappers sondables).
- **`fallacies_cards_fr`** = insondable live tant que le déploiement n'est pas refait (absent du
  commit servi `de763aa9`).
- **Commentaire de template FAUX** (`included.html:726`, propagé à 36 wrappers) : différé — sa
  correction propre exige une régénération, hors de ce grain et du gel.

## §3 La suite proposée (2 options, reco marquée — décision owner)

**Option (a) — aligner la CI sur la barre de l'issue** *(reco worker)* : élargir les
InlineData pour que chaque Cap porte `fr:F, fr:V, ar:F, zh:F` (les fixtures existent déjà,
committées, pour les 8 langues). Coût mesurable : **~+14 cas** (Cap2 : +ar,+fr:V ; Cap3/4/6/7/8 :
+ar,+zh,+fr:V ; Cap5/9 : +ar,+zh), CI **+1 à 2 min** (59 s aujourd'hui à chaud ; chaque cas
reste un rendu Chromium borné). Zéro write corpus — c'est un changement de **test uniquement**.
⚠️ Le registre dit explicitement : « a change to that count is a change of instrument and
belongs in this document » — le geste inclut la mise à jour datée du registre. **Timing reco :
post-tag** (changement d'instrument pendant le gel = bruit inutile ; rien ne presse, la barre
live a été tenue).

**Option (b) — statu quo documenté** : la CI garde l'invariant FR (× 2 familles sur Cap1/5/9),
la barre RTL/CJK reste portée par la passe verdict au live. Coût 0 ; contrepartie : la
prochaine régén ne re-vérifie Cap3-9 sur ar/zh que si un humain rejoue la passe.

**Indépendamment de (a)/(b)** — déjà actés ou hors de ce grain : la passe verdict live (Cap 0 +
commité≠servi + SHA anti-harvest) à **chaque redéploiement** de wrappers ; le commentaire de
template à la prochaine régén ; `_ext` = #1040.

## §4 Ce que ce dossier n'établit pas

⛔ Aucune mesure **live** nouvelle (la dernière passe comportementale servie date du 13/09,
verdict PASS — ce dossier ne la rejoue pas) · ⛔ aucun verdict visuel (réservé ai-01) ·
⛔ aucune modification de test/code/CSV (l'option (a) est **proposée**, pas exécutée — le
registre des comptes reste à 43) · ⛔ l'état `_ext`/`cards_fr` du live n'est pas re-sondé
(dernier relevé 12-13/09 cité) · ⛔ Cap 9 « n'établit pas le fit » — nuance conservée du registre.

---
*po-2024 — pool #458 (c.5666260217), grain ④. Le socle : 9/9 assertées en CI, Cap 0 = verdict
live par conception ; l'asymétrie langue/famille est le seul reste côté harness, chiffré en option (a).*

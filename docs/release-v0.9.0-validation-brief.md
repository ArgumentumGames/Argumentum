# v0.9.0 — Dossier de validation « sur pièce » (pour jsboige, dimanche)

**Objet :** présentation propre des docs release **#456** (`CHANGELOG.md` + `RELEASE-NOTES-v0.9.0.md`,
mergés sur master `7b57251e`) en vue de ta validation interactive du dimanche.
**Auteur :** Claude Code @ myia-po-2023 (worker) — **2026-06-13**
**Gate release :** ⛔ TOUJOURS ACTIF. Aucun tag / aucune Release lancée. Ce document **prépare** ta
validation, il ne la remplace pas et ne lève rien.

> Rappel de ta consigne (verbatim) : « on ne lance rien en release tant que je n'ai pas validé
> sur pièce dans cette conversation ». Ce brief est ce que tu m'as demandé de « mettre en forme
> maintenant pour que ce soit prêt » — pas une demande de GO.

---

## 1. Ce que les docs #456 affirment (résumé fidèle)

**v0.9.0 = 8 langues** : FR (source) · EN · RU · PT · ES · AR · FA · ZH.

| Affirmation des docs | Source dans #456 |
|----------------------|------------------|
| CSV 100 % traduits (Fallacies, Virtues, Scenarii, Rules) ×8 langues | CHANGELOG L14 / RELEASE-NOTES L25 |
| PDFs localisés (Tarot, Poker, A0, Print&Play) ×8 | CHANGELOG L15 / RELEASE-NOTES L29-34 |
| Images carte (PNG) ~9 834 | RELEASE-NOTES L36 |
| **MindMap SVGs générés « pour les 8 langues »** | **CHANGELOG L16 / RELEASE-NOTES L25, L35** |
| Pipeline restauré vs Golden Master avril 2024 | CHANGELOG L38-48 / RELEASE-NOTES L39-46 |
| 155 tests (depuis 0) | CHANGELOG L70 / RELEASE-NOTES L46 |
| OWL ontologie : FR seulement | RELEASE-NOTES L37, L91 |

---

## 2. Vérifié sur pièce côté worker (ce qui est SOLIDE)

Ces points sont confirmés par inspection directe des artefacts locaux (régén Release 12 juin) :

- ✅ **64 PDFs = 8 types de documents × 8 langues** (vérifié dans `bin/Release/.../Target/{lang}/`).
  Les 8 types : `TarotCards`, `TarotCards_Virtues`, `TarotCards_Print&Play_A4`, `PokerCards`,
  `PokerCards_Print&Play_A4`, `Fallacies_Web_A0`, `Fallacies_Web_A4`,
  `Fallacies_Web_Thumbnails_A4`. Les 8 sous-dossiers langue (`ar en es fa fr pt ru zh`) sont tous
  présents et peuplés.
- ✅ **~9 834 images** (régén validée 12 juin, exit 0, 5,0 Go).
- ✅ **155 tests pass / 0 fail / 5 skip** (dont 1 skip = test GUI FreePlane « requires interactive
  session » — pertinent pour le point chaud §3.1).
- ✅ **Rendu multi-script validé inline** avec toi : Memo Backs FR/RU/AR/ZH + fronts Fallacies
  « Généralisation hâtive » FR/AR/ZH — Latin / Cyrillique / Arabe-RTL / CJK corrects, zéro glyphe
  manquant, zéro débordement, zéro fallback FR (#452 résolu sur pièce).

---

## 3. ⚠️ À TRANCHER avant tag — 3 écarts trouvés dans les docs #456

J'ai relu les 2 docs ligne à ligne contre les artefacts réels. Je **ne** les rubber-stamp pas :
voici les 3 écarts, du plus matériel au plus trivial.

### 3.1 🔴 MATÉRIEL — « MindMap SVGs pour les 8 langues » est **sur-affirmé**

**Affirmé :** CHANGELOG L16 « FreeMind mind maps generated **for all 8 languages** » ;
RELEASE-NOTES L35 table « MindMap SVGs | **8** | ~40 ».

**Réel (vérifié dans `Cards/Fallacies/Mindmaps/`) :** seules **4 langues** ont des SVGs commités —
`fr` (6), `en` (5), `pt` (5), `ru` (5) = **21 SVGs**. **`es`, `ar`, `fa`, `zh` sont ABSENTS** (pas
de sous-dossier).

**Pire que ce que les docs admettent :** RELEASE-NOTES L92 (Known Limitations) reconnaît
*partiellement* l'écart — « MindMap SVGs for **AR/FA/ZH** : regeneration pending » — mais **omet ES**
et **contredit sa propre table L35** (« 8 »). Donc un lecteur a deux affirmations opposées dans le
même fichier.

**État technique :** le pipeline EST configuré pour les 8 langues (PR #454 : `StaticConversions` +
`MindMapLocalization` AR/FA/ZH/ES présents dans `AssetConverterConfig.cs`). Il manque **uniquement le
RUN de régénération** — qui passe par l'automation **GUI FreePlane** (`SendKeys.SendWait`, desktop
takeover ; c'est exactement le chemin que le test suite **skip** comme « interactive session »).

**Ta décision dimanche (2 options) :**
- **(A) Faire la vérité :** lancer la régén MindMap ES/AR/FA/ZH (Track 1a) — run **attendu/foreground**
  (pas en tick automatisé, risque de keystrokes parasites). Après ça, l'affirmation « 8 langues »
  devient exacte. _Recommandé si les MindMaps font partie du livrable v0.9.0._
- **(B) Aligner les docs :** corriger CHANGELOG L16 + RELEASE-NOTES L25/L35 en « 4 langues
  (FR/EN/RU/PT) générées, ES/AR/FA/ZH en attente de régén » et retirer la contradiction L92.
  _Recommandé si les MindMaps 8-lang peuvent attendre post-v0.9.0._

> Un patch « option B » (docs alignées) peut être préparé en 5 min si tu choisis B. Je n'ai
> **rien modifié** d'office — c'est ton arbitrage.

### 3.2 🟡 MINEUR — table « Generated Assets » : comptes par type non réconciliés

RELEASE-NOTES L31-34 donne des comptes par type qui **ne réconcilient pas** avec la structure réelle
(8 types × 8 langues = 64) :

| Doc dit | Réel |
|---------|------|
| Tarot Card PDFs — « ~64 » | `TarotCards` = 8 (le « ~64 » ressemble au **total global mal étiqueté**) |
| A0 Poster PDFs — « ~16 » | `Fallacies_Web_A0` = 8 |
| Print&Play PDFs — « ~16 » | Tarot P&P (8) + Poker P&P (8) = **16 ✓** |
| Poker Card PDFs — « ~8 » | `PokerCards` = 8 ✓ |

**Suggestion :** remplacer la table par le décompte propre **8 types × 8 langues = 64 PDFs** (liste
des 8 types ci-dessus §2). Cosmétique, mais évite qu'un lecteur additionne ~64+16+16+8 ≈ 104 et se
demande où sont les PDFs manquants.

### 3.3 ⚪ TRIVIAL — placeholders de date

- CHANGELOG L8 : `## [0.9.0] — 2026-06-XX`
- RELEASE-NOTES L3 : `Release date: TBD`

À remplir **au moment du tag** (post-validation, post-go-live DNN). Normal qu'ils soient en attente —
juste à ne pas oublier.

---

## 4. Hors-docs mais lié — bug i18n trouvé pendant l'audit DNN (#457)

Le template DNN `_FallacyExplorer_Root.cshtml` **épingle les champs anglais** (`text_en`/`desc_en`/
`link_en`) et le label « find out more » quelle que soit la culture du visiteur → la liste des
fallacies du site s'affiche en EN même en FR/AR/ZH. **Pas un bug des docs release** ni du pipeline
carte (les cartes sont OK §2) — c'est côté **site DNN**, déjà loggé pour Phase 2/4 de #457. Mentionné
ici pour complétude, pas un bloqueur v0.9.0.

---

## 5. Checklist de validation « sur pièce » (dimanche)

Pour inspecter directement (je peux surfacer chaque artefact inline à la demande) :

- [ ] **PDFs** — ouvrir 1 PDF/langue (ex. `Target/ar/Argumentum_TarotCards_ar.pdf` pour le RTL,
      `Target/zh/...` pour le CJK). Vérifier titres/définitions/exemples dans la bonne langue.
- [ ] **MindMaps** — décider §3.1 option (A) régén ou (B) aligner docs.
- [ ] **Table assets** — valider §3.2 (réconcilier les comptes par type).
- [ ] **CSV data** — spot-check 1 dataset/langue si souhaité (Fallacies/Virtues/Scenarii/Rules).
- [ ] **Docs ton/contenu** — relire CHANGELOG + RELEASE-NOTES (EN impersonnel, FR « Vous »).
- [ ] **GO/NO-GO tag** — seulement après ton OK sur pièce ; le tag reste couplé go-live DNN (#134).

---

## 6. Ce qui reste GELÉ tant que tu n'as pas validé ici

- ⛔ Tag `v0.9.0` / GitHub Release **#134** (couplé go-live DNN + ta validation sur pièce).
- ⛔ Régén Release 8-lang **complète** (Track 1b) — lourde, lancée seulement sur ton nod.
- ⛔ Merge PRs DNN **#442 / #444** (HELD go-live).
- ⏸️ Régén MindMap ES/AR/FA/ZH (Track 1a) — **prête** mais GUI/attended, lancée sur ton go (§3.1-A).

---

*Worker po-2023 signale et prépare ; le verdict QA visuel et le merge sont à ai-01, la validation
release et le GO tag sont à toi. Gate intact.*

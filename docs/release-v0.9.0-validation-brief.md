# v0.9.0 — Dossier de validation « sur pièce » (pour jsboige, dimanche)

> ⚠️ **Document superseded (snapshot 2026-06-13).** Les compteurs et faits ci-dessous (« 64 PDFs = 8 types × 8 langues », etc.) sont **antérieurs au bundle v3** : 80 PDFs (10 types × 8 langues, expansion P&P #648-650), DeviceCMYK + OutputIntent SWOP via Ghostscript (#632/#652), OWL bilingue EN/FR uniquement, 578 tests. **Référence actuelle : [`docs/RELEASE-VALIDATION-v0.9.0.md`](RELEASE-VALIDATION-v0.9.0.md) (dossier v4).** Ce brief reste l'archive de la validation du 13 juin.

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
| Tests : docs disent 155, réel = 159 (PR #465/#28 +4) | CHANGELOG L70 / RELEASE-NOTES L46 — **corrigé ce tick → 159** |
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
- ✅ **159 tests pass / 0 fail / 5 skip** (dont 1 skip = test GUI FreePlane « requires interactive
  session » — pertinent pour le point chaud §3.1). 155 → 159 via PR #465/#28 (front/back target
  dissociation, +4 tests `Issue28TargetDissociationTests`), compté par grep `[Fact]/[Theory]` ce tick.
  PRs #466/#467/#468 sont doc-only / config `Enabled=false` → 0 test runtime.
- ✅ **Rendu multi-script validé inline** avec toi : Memo Backs FR/RU/AR/ZH + fronts Fallacies
  « Généralisation hâtive » FR/AR/ZH — Latin / Cyrillique / Arabe-RTL / CJK corrects, zéro glyphe
  manquant, zéro débordement, zéro fallback FR (#452 résolu sur pièce).

---

## 3. ⚠️ À TRANCHER avant tag — 1 écart résiduel (§3.3 trivial) ; §3.1/§3.2 RÉSOLUS par PR #460, §3.4 CORRIGÉ ce tick

> **Mise à jour 2026-06-14 (po-2023).** En relisant les docs sur master `8382a071`, je constate que la
> PR **#460** (`36124be2` « align v0.9.0 notes with master HEAD reality », merge-bot, post-#456) a
> **déjà corrigé** les écarts §3.1 et §3.2 que je signalais dans la version initiale de ce brief. Cette
> section est alignée : il ne reste qu'un écart trivial (placeholders de date, §3.3 — normal avant tag).
> _Note process : mon brief initial (#462) a été rédigé d'après une lecture pré-#460 et n'avait pas
> été re-syncé — d'où les 2 écarts fantômes. Gap de review signalé à ai-01._

### 3.1 ✅ RÉSOLU (PR #460) — MindMap SVGs : docs désormais alignés sur la réalité 4-langues

**État courant (vérifié 14/06 sur master `8382a071`) :** CHANGELOG L16, RELEASE-NOTES L25 et L35, et
Known Limitations L92 disent **tous** « FR/EN/RU/PT (21 SVGs), ES/AR/FA/ZH configurés mais régén
pending ». **Plus de sur-affirmation, plus de contradiction interne** — l'écart matériel est clos.

**Réel (inchangé, vérifié dans `Cards/Fallacies/Mindmaps/`) :** 4 langues commitées (21 SVGs),
`es/ar/fa/zh` absents. Le pipeline est configuré pour les 8 (PR #454), seul le run GUI FreePlane manque.

**Décision résiduelle pour jsboige (simplifiée) — statu quo vs régén :**
- **(A) Régénérer** MindMap ES/AR/FA/ZH (Track 1a, run attendu/foreground) → les docs passent de
  « 4 + pending » à « 8 ». _Si les MindMaps 8-lang sont dans le scope v0.9.0._
- **(B) Statu quo** (= option B du brief initial, **déjà appliquée par #460**) : les docs restent
  honnêtes sur 4 langues. _Si les MindMaps 8-lang peuvent attendre post-v0.9.0._

> Avec #460, l'option B est **déjà faite** — jsboige n'a plus qu'à décider si elle veut activer A.

### 3.2 ✅ RÉSOLU (PR #460) — table « Generated Assets » : comptes réconciliés

**État courant :** RELEASE-NOTES L31-34 donne désormais **Tarot 24 + Poker 16 + Fallacies Web 24 =
64** (8 langues × {3 + 2 + 3} types). **Réconcilié** avec la structure réelle (§2). Plus d'ambiguïté.

### 3.3 ⚪ TRIVIAL — placeholders de date

- CHANGELOG L8 : `## [0.9.0] — 2026-06-XX`
- RELEASE-NOTES L3 : `Release date: TBD`

À remplir **au moment du tag** (post-validation, post-go-live DNN). Normal qu'ils soient en attente —
juste à ne pas oublier.

### 3.4 ✅ CORRIGÉ ce tick (po-2023) — test count drift (155 → 159)

**Drift détecté :** le CHANGELOG L70 + RELEASE-NOTES L46 disaient **155 tests**, mais le dashboard
ai-01 rapporte **159** après #465/#28. Vérifié par grep `[Fact]/[Theory]` dans
`Argumentum.AssetConverter.Tests/` ce tick : `Issue28TargetDissociationTests.cs` = **4 tests** (PR
#465/#28, sur master `8382a071`). Les PRs #466/#467/#468 sont doc-only / config `Enabled=false` →
**0 test runtime**. Donc 155 + 4 = **159**. ai-01 avait raison.

**Correctif appliqué ce tick** (même patch que ce brief) : CHANGELOG L70 + RELEASE-NOTES L46 mis à
**159** (`+4 from #465/#28`). Plus de drift pour la validation dimanche.

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

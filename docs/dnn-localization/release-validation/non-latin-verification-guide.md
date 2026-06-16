# DNN Site — Guide de vérification des langues non latines (ar/fa/zh, +ru)

**Pourquoi :** jsboige ne lit pas l'arabe (ar), le persan (fa) ni le chinois (zh), et lit peu le
russe (ru). Ce guide donne les **chaînes attendues** et des **signatures visuelles** pour valider ces
langues **sans les lire**.

**Source des chaînes :** `../dnn-ui-strings.csv` (PR #490). ⚠️ Les 7 lignes `res.*` (hors
`MemoInstructions`) ont un **FR inféré** — leurs traductions sont **provisoires** tant que le FR n'est
pas vérifié vs l'export 2sxc (voir `2sxc-export-spec.md`). #490 est **HELD** pour cette raison.

---

## 1. Comment valider une langue qu'on ne lit pas

Trois contrôles indépendants du sens :

1. **Comparaison de chaîne exacte** — la chaîne rendue à l'écran doit **matcher** (caractère pour
   caractère) la chaîne attendue ci-dessous. Copier-coller depuis le site vers un diff texte.
2. **Sens de lecture (RTL)** — pour ar/fa, le conteneur doit être `dir="rtl"` (DevTools → inspecter
   l'élément, ou `dir="auto"` résolvant à RTL). Le texte commence à droite.
3. **Glyphe + police** — pas de tofu (□) ni de glyphes latins substitués. La police attendue est
   chargée (Noto Naskh Arabic / Vazirmatn / Noto Sans SC).

Si les 3 passent, la langue rend correctement (même sans la lire).

## 2. Cheat-sheet — chaînes attendues (#490)

> `res.*` = FR **inféré** (provisoire). `ui.*` = FR solide. `res.RuleMemoInstructions` = absent (non
> traduit, source FR DB-only).

| key | ar (RTL) | fa (RTL) | zh | ru |
|-----|----------|----------|----|----|
| `ui.fallacy.find_out_more` | اعرف المزيد | بیشتر بدانید | 了解更多 | Подробнее |
| `ui.rules.players_range` | من {0} إلى {1} لاعبين | از {0} تا {1} بازیکن | {0} 至 {1} 名玩家 | от {0} до {1} игроков |
| `res.RuleSummary` ⚠️ | ملخص | خلاصه | 摘要 | Краткое описание |
| `res.RuleMaterial` ⚠️ | المواد | محتویات بازی | 材料 | Материалы |
| `res.RuleInstallation` ⚠️ | الإعداد | آماده‌سازی | 设置 | Подготовка |
| `res.RuleVariants` ⚠️ | الأنواع | گونه‌ها | 变体 | Варианты |
| `res.RuleMemoCard` ⚠️ | بطاقة تذكيرية | کارت یادآوری | 备忘卡 | Памятка |
| `res.RuleMemoCardFileNamePrefix` ⚠️ | مذكرة | یادداشت | 备忘 | Памятка |
| `res.RuleMemoCardDownload` ⚠️ | تنزيل البطاقة | دانلود کارت | 下载卡牌 | Скачать карту |

**Contrôles spécifiques aux placeholders** (`ui.rules.players_range`) :
- `{0}` et `{1}` doivent apparaître **tels quels** (accolades incluses) à la position équivalente.
- En ar : `من {0} إلى {1} لاعبين` — les nombres s'insèrent entre les accolades, lecture RTL.
- En zh : `{0} 至 {1} 名玩家` — « de {0} à {1} joueurs ».

## 3. Signatures visuelles par langue

### AR (arabe — RTL)
- **Direction :** RTL. `dir="rtl"` sur le conteneur.
- **Police :** Noto Naskh Arabic. Glyphes liés (l'arabe est cursif — les lettres se joignent).
- **Red flags :** lettres détachées (non jointes), sens LTR, tofu, texte latin.
- **Spot-check :** le libellé du lien FallacyExplorer = `اعرف المزيد` (4 mots arabes joints).

### FA (persan — RTL)
- **Direction :** RTL. `dir="rtl"`.
- **Police :** Vazirmatn. Le persan utilise des formes de lettres supplémentaires (گ چ پ ژ) absentes
  de l'arabe — vérifier qu'elles rendent (ex. `بیشتر` contient `ش`).
- **Red flags :** identiques à ar, MAIS le persan ne doit PAS utiliser uniquement des glyphes arabes
  (si Vazirmatn manque, fallback déforme).
- **Spot-check :** lien FallacyExplorer = `بیشتر بدانید`.

### ZH (chinois simplifié — LTR)
- **Direction :** LTR (comme le latin).
- **Police :** Noto Sans SC. Caractères pleins (CJK), pas de tofu.
- **Red flags :** tofu □, caractères japonais traditionnels (≠ simplifié), texte latin.
- **Spot-check :** lien FallacyExplorer = `了解更多` (4 caractères CJK).

### RU (russe — Cyrillique, LTR)
- **Direction :** LTR.
- **Police :** défaut (le Cyrillique est couvert par la plupart des polices latines).
- **Red flags :** caractères latins à la place du Cyrillique (ex. « C » au lieu de « С »).
- **Spot-check :** lien FallacyExplorer = `Подробнее`.

## 4. Contrôle rapide « 30 secondes »

Pour une validation express d'une langue non lue :
1. Ouvrir la page Rules détail dans la langue cible.
2. Copier le libellé du bouton de téléchargement (en bas de la carte mémo).
3. Le coller dans le diff vs la cellule `res.RuleMemoCardDownload` du tableau §2.
4. Si match exact → la ressource est localisée et la police rend. Sinon → investiguer (fallback FR ?
   police manquante ? clé non résolue ?).

## 5. Limites

- Ce guide couvre les **10 chaînes** de `dnn-ui-strings.csv`. Les contenus DB-only (Glossary, FAQ,
  homepage, corps des règles) ne sont **pas encore extraits** (voir `2sxc-export-spec.md`) — ils
  n'ont donc pas encore de chaînes attendues.
- Le bug FallacyExplorer (§4 du content-audit) fait que l'Explorer affiche de l'anglais en toute
  langue → ne pas conclure « traduction manquante » sur l'Explorer avant le fix.

---

*Les chaînes attendues reflètent #490 (HOLD). Toute correction du FR source → re-run #457 →
mise à jour de ce guide.*

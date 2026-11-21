# 下载页面 — Argumentum v2.0.0（法文定稿草案的中文镜像）

> **状态：草稿（准备 #135 §下载；对齐 #2234 与已裁定的对应表
> [c.6650903268](https://github.com/ArgumentumGames/Argumentum/issues/135#issuecomment-6650903268)，
> P24-03 颗粒，2026-11-21）。** 定稿版本：
> [downloads-v2.0.0.fr.md](downloads-v2.0.0.fr.md)（工作附录 — 发布清单、附件 — 位于法文版与英文镜像）。
> **发布受控（GATED）**：取决于 #134（GitHub Release 打标签）、#132（生产部署）与 #131（DNN 10.3.2
> 上线）。资产文件名不翻译（专有名词）；**`Print.Play` 为已发布形式**（GitHub 会把 `&` 改写为 `.`）。
> 链接**只**指向最终 release（#134），绝不指向预发布（`v0.9.0-review`、`v2.1.0-review`）。

## 预发布警示（打标签时移除）

⚠️ **在 `v2.0.0` 打标签之前**，请勿分发预发布资产：`v0.9.0-review` 的资产日期为 8 月 24-25 日，
早于九月的修正；`v2.1.0-review` 的资产并非获批批次。页面只应引用打标签时新上传的资产。

## 按语言浏览 — 完整内容，八份

每种语言都获得**完整**内容：**10 份 PDF 文档**（已发布名称见
[#2234 派生表](135-downloads-assets-table.md) — 10 个主干 × 8 种语言 = 80 项资产），外加在打标签时由
[`tools/135-build-download-zips.py`](../../tools/135-build-download-zips.py) 构建并附加到最终 release 的
**两个包**：

- **数字卡牌** — `Argumentum_Cards_PNG_zh.zip`：**175** 张卡牌的 PNG，从与 release 相同的生成树重建
  （当前在线的 PNG 仅有法文版且只有 157 张卡牌 — 双重过时）；
- **思维导图** — `Argumentum_MindMaps_zh.zip`：该语言的谬误**与**美德 SVG，取自仓库
  （`Cards/Fallacies/Mindmaps/zh/`），并附指向网站展示页面的链接。

| 语言 | 完整内容（10 份 PDF） | 其中仅 Print & Play | 文档数 |
|---|---:|---:|---:|
| العربية (ar) | **459.9 MB** | 205.6 MB | 10 |
| English (en) | **469.0 MB** | 211.6 MB | 10 |
| Español (es) | **472.2 MB** | 212.9 MB | 10 |
| فارسی (fa) | **470.3 MB** | 211.4 MB | 10 |
| Français (fr) — 源语言 | **465.5 MB** | 210.2 MB | 10 |
| Português (pt) | **481.4 MB** | 217.3 MB | 10 |
| Русский (ru) | **449.3 MB** | 204.7 MB | 10 |
| 中文 (zh) | **478.0 MB** | 213.4 MB | 10 |
| **合计** | **3 745.6 MB** | 1 687.1 MB | **80** |

「仅 Print & Play」= 4 份家庭打印 PDF（塔罗标准版 + Light、扑克标准版 + Light）。
**没有「完整语言」zip**：每份 PDF 从 release 单独提供。

## 按格式浏览 — 10 份文档

| 文档 | 内容 | 页数 | 大小（8 种语言的最小..最大值） | 用途 |
|---|---|---:|---|---|
| `Argumentum_TarotCards` | 主牌组：15 条规则 + 备忘 ×7 + 175 个谬误，双面 **CMYK** | 379 | 57.9..66.9 MB | 印刷厂 |
| `Argumentum_TarotCards_Virtues` | 美德扩展：131 张牌，双面 CMYK | 262 | 83.1..88.0 MB | 印刷厂 |
| `Argumentum_PokerCards` | 情景牌组：167 张牌 ×2、7 种背面，CMYK | 334 | 25.9..32.7 MB | 印刷厂 |
| `Argumentum_TarotCards_Print.Play_A4` | 塔罗 Print & Play（A4，双面） | 105 | 136.1..147.1 MB | 家庭 |
| `Argumentum_TarotCards_Print.Play_Light_A4` | 塔罗 P&P **Light**（省墨） | 21 | 29.7..31.4 MB | 家庭 |
| `Argumentum_PokerCards_Print.Play_A4` | 情景 P&P | 38 | 25.7..32.5 MB | 家庭 |
| `Argumentum_PokerCards_Print.Play_Light_A4` | 情景 P&P Light | 6 | 6.3..7.4 MB | 家庭 |
| `Argumentum_Fallacies_Web_A0` | 175 个谬误的 A0 海报 | 1 | 22.6..28.1 MB | 海报 |
| `Argumentum_Fallacies_Web_A4` | A4 海报 | 15 | 22.0..27.5 MB | 海报 |
| `Argumentum_Fallacies_Web_Thumbnails_A4` | 缩略图页 | 9 | 27.0..29.0 MB | 参考 |

每份文档按 `<主干>_zh.pdf` 展开（如 `Argumentum_TarotCards_Print.Play_A4_zh.pdf`）。

盒装数量：**197 / 364 / 495** 张牌（制造决策 #1187）。页数 = 实测契约 80/80（PR #1409 §C2）—
勿混淆两个牌组：塔罗 **379** 页、扑克 **334** 页。

## 按包类型浏览

对应关系**已裁定**（[c.6650903268](https://github.com/ArgumentumGames/Argumentum/issues/135#issuecomment-6650903268)，
11-05）— 页面今日所提供的内容保持其性质，更新后按 8 种语言展开：

| 包 | 内容 | 大小 | 获取方式 |
|---|---|---|---|
| **完整包** | 80 份 PDF、8 种语言 — 无全局 zip | **3 745.6 MB**（实测） | 上方按语言的链接（每份 PDF 均在最终 release 上） |
| **按语言** | 10 份 PDF + 该语言的卡牌 zip + 思维导图 zip | 449.3..481.4 MB（仅 PDF） | `[PLACEHOLDER — 最终 release #134 的 URL]` |
| **Print & Play** | 每种语言 4 份 P&P PDF（标准 + Light） | 204.7..217.3 MB | `Argumentum_{Tarot,Poker}Cards_Print.Play[_Light]_A4_zh.pdf` |
| **A0 海报** | 175 个谬误的海报 | 22.6..28.1 MB | `Argumentum_Fallacies_Web_A0_zh.pdf` |
| **A4 页** | A4 海报 + 缩略图页 | 22.0..29.0 MB | `Argumentum_Fallacies_Web_{A4,Thumbnails_A4}_zh.pdf` |
| **数字卡牌** | 每种语言 175 张 PNG | `[PLACEHOLDER — 打包时实测]` | `Argumentum_Cards_PNG_zh.zip`（打标签时构建） |
| **轻量卡牌** | 175 张轻密度 PNG | `[PLACEHOLDER — 待实测]` | `Argumentum_Cards_PNG_light_zh.zip` **若** release 树携带该密度；否则该包取消，由 `Argumentum_Fallacies_Web_Thumbnails_A4_zh.pdf` 页替代 |
| **思维导图** | 每种语言的谬误 + 美德 SVG | `[PLACEHOLDER — 打包时实测]` | `Argumentum_MindMaps_zh.zip` + 网站展示页链接 |
| **本体 — 谬误** | 175 个谬误的 OWL（SKOS + AIF），**由网站提供** | `[PLACEHOLDER — 待实测]` | `/argumentum_fallacies.owl`（网站根目录） |
| **本体 — 美德** | 美德的 OWL（SKOS + AIF），**由网站提供** | `[PLACEHOLDER — 待实测]` | `/argumentum_virtues.owl`（网站根目录） |

> OWL 文件名由 blob 声明的 **IRI** 决定，而非仓库中的文件名（`argumentum.owl` → 以
> `argumentum_fallacies.owl` 提供）；版本与日期在打标签前留空。
>
> 下载目录的 6 个旧 zip（08-18，法文）不再被链接；⛔ 未经撤回决定并附保存证明，任何 zip 均不删除。
> 「 Argmentum 」的拼写错误随新名称消失。

专业印刷 PDF 已转换为**带 OutputIntent 的 DeviceCMYK**（`/GTS_PDFX` + `CGATS TR 001`，#1380）；
卡牌为原生 300 dpi。

---

*中文镜像（P24-03，2026-11-21）。资产名称在 8 个文件中完全一致（专有名词）；发布前进行人工
CJK 校验。发布清单、SEO 与附件：定稿 [FR](downloads-v2.0.0.fr.md) · [EN](downloads-v2.0.0.en.md)。
仅做准备 — 不发布任何内容 · 视觉判定：ai-01 · 审阅：jsboige。*

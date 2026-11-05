# #458 — `GlobalResources` zh-CN : les 30 clés ajoutées au neutre par DNN 10.4.1

**Date :** 05/11/2026 · **Base :** `66b28511` · **Grain :** pool #458 c.6645521254, po-2024 « fa-IR, puis zh-CN »
**Périmètre :** 1 resx (`DNNPlatform/App_GlobalResources/GlobalResources.zh-CN.resx`), 30 cellules ajoutées, 0 cellule existante touchée. **Dernier pack du lot** après fr-FR (#2133), pt-BR (#2143), ru-RU (#2147), ar-SA (#2149) et fa-IR (#2151).

## État mesuré avant le geste

| Pack | `<data>` au parseur | Clés neuves présentes |
|---|---:|---:|
| neutre | 211 | 30 (les ajouts 10.4.1) |
| **zh-CN** | **172** | **0** |

Transport zh-CN mesuré : **BOM UTF-8 présente**, LF seuls, **61 612 octets**. Comme ar-SA et fa-IR, son commentaire d'en-tête porte **4** exemples `<data>` (`Name1`/`Color1`/`Bitmap1`/`Icon1`) — grep 176 vs parseur 172. Enveloppe **majoritaire** : `xml:space="preserve"` sur `<data>` (172 cellules) contre 2 sur `<value>` — le lot suit la majorité.

## Script — la garde bannit ce que le pack n'emploie jamais

Census du pack historique : **8 101 caractères Han**, ponctuation pleine-chasse （），：；！, idéographique 、。, guillemets courbes “”, tiret —, points de suspension … — et **0 kana, 0 hangul** (aucune forme pleine-chasse de lettre/chiffre non plus).

⇒ `NoNewCellCarriesKanaOrHangul` bannit les syllabaires japonais (U+3040–U+30FF) et le hangul (U+1100–U+11FF, U+AC00–U+D7AF) : dans un pack chinois simplifié, c'est une contamination par une mauvaise langue. La ponctuation CJK (U+3001 、, U+3002 。) vit **sous** la plage bannie et passe. Contrôles inverses in-test : «テスト» (katakana), «あいう» (hiragana), «한국어» (hangul) déclenchent ; «测试页面已提交» et «、。（）“”—…» passent. Même famille que la garde persan-exclusif ar-SA (#2149) et son miroir fa-IR (#2151).

## Traductions — terminologie comptée dans le pack, jamais inventée

Ancres attestées : « 密码重置令牌 » et « 密码提醒 » (`EMAIL_PASSWORD_REMINDER_*`), « 等待审批 » (`EMAIL_..._ISNOT_APPROVED_BODY`), « 另行发送密码重置邮件 » (clé sœur), « 帐户 » (18 occurrences, **jamais** « 账户 »), « 用户名 » (10), « 显示名称 » (corps admin `EMAIL_USER_REGISTRATION_ADMINISTRATOR_BODY`), « 主机 » (`//Host.String`), « 注册用户 », « 页面 », « 提交 », « 审核 », « 发布 », « 拒绝 », « 角色 », « 验证 », « 登录 », « 订阅 », « 匿名 ».

**Formations sans précédent pack** (déclarées, pas comptées) : `所有人` (Everyone), `超级用户` (SuperUsers), `未认证用户` (Unauthenticated), `未验证用户` (Unverified), `订阅者` (Subscribers), `内容编辑` (Content Editors), `内容管理员` (Content Managers), `丢弃` (discarded), `上线` (live), `以下用户` (the following user).

Le corps e-mail admin **miroire la structure du neutre** (`\n` d'ouverture + `\n` + 4 espaces de fermeture, comme ru-RU et pt-BR — ar-SA a livré sans la fermeture) et **la salutation/termes du pack** (`用户名：`/`显示名称：` du corps admin, `另行发送密码重置邮件` de la clé sœur). Le pack zh-CN n'a aucun corps ouvrant par `\n` (0/25) — miroir de l'amont, comme fa-IR.

## Chirurgie et contrôles

- 30 blocs `<data>` insérés avant `</root>`, style du pack respecté (enveloppe `xml:space` sur `<data>`, majoritaire 172 vs 2) ;
- **+5 343 o** asserté AVANT écriture (delta = taille du bloc encodé), **numstat 96/0** (insertion pure : 30 blocs × 3 lignes + 6 sauts internes du corps e-mail), préfixe historique octet-identique, BOM conservée, 0 CR ;
- Post : **202 `<data>` au parseur** (grep 206 = 176 + 30), round-trip **30/30** valeurs exactes, parité des jetons **30/30** (pré-contrôlé par clé contre le neutre, `[User:UserName]` du corps ≠ `[User:Username]` du sujet conservés), **0 latin hors jetons**, **0 kana/hangul**, **0 entité échappée** (`&amp;`/`&quot;` — engagement #2143) ;
- Dict source en texte clair, échappement à la couche d'écriture seule (leçon double-échappement #2143) ; l'échappeur ASSERT qu'aucune valeur ne porte `& < >`.

## Gardes

**`DnnGlobalResourcesZhCnGuardTests`** (8 faits) : jumeaux du lot + `NoNewCellCarriesKanaOrHangul` propre à ce pack. **Banc rouge mesuré** sur l'arbre pristine (`66b28511`, resx non committée) : **2 échecs nominatifs** — `TheThirtyKeysAreDeliveredInZhCn`, `ZhCnKeepsItsTransportAndDidNotLoseKeys` — 6 verts ; vert après le commit du lot (lecture `HEAD:`, voulu #2138).

**`DnnAgrCoreResxIntegrityGuardTests`** : **3ᵉ dérogation vivante** de `AllowedMissingByFileAndCulture` — `("GlobalResources", "zh-CN")` → les 10 blocs historiques (les mêmes qu'ar-SA et fa-IR) ; `Covered` 172 → 202. **Le défaut à 40 ne couvre plus aucune culture couverte** : le lot des 172-clés est soldé.

## Reste à faire (hors périmètre de ce grain)

- Lot 30 clés : **soldé** (fr, pt, ru, ar, fa, zh livrés).
- 70 cellules EAV, `sans-setif`, SHA-1 du transport batch, #135 en 6 langues (file po-2024).

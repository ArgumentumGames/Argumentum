# #458 — `GlobalResources` fa-IR : les 30 clés ajoutées au neutre par DNN 10.4.1

**Date :** 05/11/2026 · **Base :** `66b28511` · **Grain :** pool #458 c.6645521254, po-2024 « fa-IR, puis zh-CN »
**Périmètre :** 1 resx (`DNNPlatform/App_GlobalResources/GlobalResources.fa-IR.resx`), 30 cellules ajoutées, 0 cellule existante touchée. Dernier jumeau du lot après fr-FR (#2133), pt-BR (#2143), ru-RU (#2147) et ar-SA (#2149).

## État mesuré avant le geste

| Pack | `<data>` au parseur | Clés neuves présentes |
|---|---:|---:|
| neutre | 211 | 30 (les ajouts 10.4.1) |
| **fa-IR** | **172** | **0** |

Transport fa-IR mesuré : **BOM UTF-8 présente**, LF seuls, **89 750 octets**. Comme ar-SA et ru-RU, son commentaire d'en-tête porte **4** exemples `<data>` (`Name1`/`Color1`/`Bitmap1`/`Icon1`) — grep 176 vs parseur 172, compté en zones de commentaires. Enveloppe **majoritaire** : `xml:space="preserve"` sur `<data>` (172 cellules) contre 2 sur `<value>` — le lot suit la majorité.

## Alphabet — la garde bannit ce que le pack n'emploie jamais, pas une pureté théorique

Census mesuré sur les 172 cellules historiques :

| Classe | Occurrences |
|---|---:|
| Persanes exclusives (ی، ک، گ، پ، چ، ژ) + ZWNJ | 2 542 / 694 / 228 / 158 / 60 / 37 / 492 |
| **Arabes exclusives ك ي ة ى** | **0 / 0 / 0 / 0** |
| **أ (alef-hamza)** | **15** |

⇒ `NoNewCellCarriesArabicExclusiveLetters` bannit **ك ي ة ى** et **laisse passer أ** (contrôles inverses : «كتاب» déclenche, «کتاب» et «أ» passent). Le miroir exact de la garde ar-SA (#2149), qui interdisait پ چ ژ گ ی/ک persans et le ZWNJ.

## Traductions — terminologie comptée dans le pack, jamais inventée

Ancres attestées : « نشانه بازنشانی گذرواژه » (jeton de réinitialisation, frères `EMAIL_PASSWORD_*`), « در انتظار تأیید », « حساب کاربری », « نام کاربری », « نام نمایشی », « نقش », « میزبان » (`//Host.String`), « مدیر », « ثبت‌نام », « رایانامه » (courriel — le pack l'emploie), « دسترسی », « محتوا », « یادآور گذرواژه ».

**Formations sans précédent pack** (déclarées, pas comptées) : `مدیران محتوا` (Content Managers), `ویرایشگران محتوا` (Content Editors), `مشترکان` (Subscribers), `کاربران ممتاز` (Superusers), `کاربران واردنشده` (Unauthenticated), `کاربران تأییدنشده` (Unverified).

Le corps e-mail admin **miroire la salutation du pack** (« کاربر گرامی [User:DisplayName]، » — forme des frères `EMAIL_*`) et **ouvre par la ligne vide du neutre** (forme mesurée du neutre, es-ES, fr-FR, pt-BR, ar-SA ; le pack fa n'a aucun corps ouvrant par `\n` — 26 corps multi-lignes, 0 précedent, donc miroir de l'amont).

## Chirurgie et contrôles

- 30 blocs `<data>` insérés avant `</root>`, style du pack respecté ;
- **+7 742 o** asserté AVANT écriture (delta = taille du bloc encodé), **numstat 95/0** (insertion pure), préfixe historique octet-identique, BOM conservée, 0 CR ;
- Post : **202 `<data>` au parseur**, round-trip **30/30** valeurs exactes, parité des jetons **30/30**, **0 latin hors jetons**, **0 lettre arabe-exclusive**, **0 entité échappée** (`&amp;`/`&quot;` — engagement #2143).

## Gardes

**`DnnGlobalResourcesFaIrGuardTests`** (8 faits) : jumeaux du lot + `NoNewCellCarriesArabicExclusiveLetters` propre à ce pack (miroir inversé d'ar-SA). **Banc rouge mesuré** sur l'arbre pristine (`66b28511`, resx non committée) : **2 échecs nominatifs** — `TheThirtyKeysAreDeliveredInFaIr`, `FaIrKeepsItsTransportAndDidNotLoseKeys` — 6 verts ; vert après le commit du lot (lecture `HEAD:`, voulu #2138).

**`DnnAgrCoreResxIntegrityGuardTests`** : 2ᵉ dérogation vivante de `AllowedMissingByFileAndCulture` — `("GlobalResources", "fa-IR")` → les 10 blocs historiques ; `Covered` 172 → 202 ; seul **zh-CN** garde encore le défaut à 40.

## Reste à faire (hors périmètre de ce grain)

- **zh-CN** : dernier pack du lot (même gabarit, garde de script propre — CJK —, sonde de forme du corps e-mail).
- 70 cellules EAV, `sans-setif`, SHA-1 du transport batch, #135 en 6 langues (file po-2024).

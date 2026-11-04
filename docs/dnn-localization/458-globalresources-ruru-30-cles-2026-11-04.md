# #458 — `GlobalResources` ru-RU : les 30 clés ajoutées au neutre par DNN 10.4.1

**Date :** 04/11/2026 · **Base :** `8c74ee13` · **Grain :** pool #458 c.6638047321, po-2024 item ① (ru → ar → fa → zh)
**Périmètre :** 1 resx (`DNNPlatform/App_GlobalResources/GlobalResources.ru-RU.resx`), 30 cellules ajoutées, 0 cellule existante touchée. Jumeau ru-RU des grains fr-FR (#2133) et pt-BR (#2143), même lot de clés.

## État mesuré avant le geste

| Pack | Clés (parseur) | Clés neuves présentes |
|---|---:|---:|
| neutre | 211 | 30 (les ajouts 10.4.1) |
| pt-BR | 212 | 30 (livrées par #2143) |
| **ru-RU** | **182** | **0** |

⚠️ **Le compte se lit au parseur, pas au grep.** `grep -c '<data '` rend **186** sur ru-RU : les 4 surnuméraires (`Name1`, `Color1`, `Bitmap1`, `Icon1`) sont les **exemples du commentaire d'en-tête resx standard**, que pt-BR n'a pas (d'où 212 = 212 au grep là-bas). Même instrument, deux packs, deux écarts différents. Aucun doublon de clé (vérifié : `uniq -d` vide) — un doublon aurait fait lever le `ToDictionary` de la garde.

Transport ru-RU mesuré : **BOM UTF-8 présente** (`od -An -tx1` → `ef bb bf`), LF seuls, 120 822 octets, fichier terminé par `</root>\n`.

## Traductions — terminologie relevée dans le pack et les ressources ru-RU du dépôt

| Terme neutre | Rendement ru-RU | Attestation |
|---|---|---|
| Everyone / Subscribers / Registered Users / SuperUsers / Administrators | **Все пользователи** / **Подписчики** / **Зарегистрированные пользователи** / **Суперпользователи** / **Администраторы** | libellés de rôles déjà présents dans d'autres `*.ru-RU.resx` du dépôt |
| role | **роль** | 11 au pack |
| account | **учетная запись** (sans ё) | « учетн » 14 vs « учётн/Учётн » 5 au pack — alignement sur la majorité |
| unverified | **неподтвержденные** | « неподтвержденную учетную запись » attesté |
| approved / pending approval | **одобрена** / **ожидает одобрения** | « Ваша регистрация была одобрена » (`EMAIL_*` frères) |
| password reset | **сброс пароля** | « Ссылка для сброса пароля » (`EMAIL_PASSWORD_REMINDER_BODY`) — ⚠️ « сброс » compte 4 au pack, dont 2 de glisser-déposer (« местом сброса ») : le compte du radical n'est pas celui du terme |
| User Name / Display Name | **Имя пользователя** / **Отображаемое имя** | frères `EMAIL_*` |
| in the '{1}' state | **в состоянии «{1}»** | « в состоянии » des ressources workflow ru-RU |
| guillemets | **« »** | 13 paires au pack, 0 guillemet droit |

**Formations sans précédent** (le pack ne couvre pas ce domaine — déclarées, pas comptées) : `Редакторы контента`, `Менеджеры контента`, `Неаутентифицированные пользователи`, `пользователи хоста`, `доступ уровня «хост»`.

## Chirurgie et contrôles

- **Leçon pt-BR appliquée** : le dictionnaire source tient du **texte clair** — assertion pré-écriture « aucun `&` ni `"` dans les valeurs » ; l'échappement appartient à la seule couche d'écriture.
- Insertion de 30 blocs avant `</root>\n`, style du pack (`xml:space="preserve"` porté par `<data>`) ; corps e-mail multi-lignes dans l'enveloppe du neutre (`\n…\n    `).
- **Delta asserté AVANT écriture** : +8 803 o (120 822 → 129 625) ; `git diff --numstat` = **96 lignes ajoutées, 0 supprimée** (29 × 3 + 9) ; préfixe historique identique octet pour octet.
- Post : 212 clés au parseur, BOM conservée, 0 CR, XML revalidé, **round-trip 30/30**, **parité des jetons 30/30** (`{N}` + `[Xxx:Yyy]` — dont `[User:UserName]` au corps et `[User:Username]` au sujet, casses distinctes recopiées du neutre), 0 copie anglaise, **0 lettre latine hors jetons**, **0 entité dans le texte parsé** (relevé promis sur #2143).

## Garde `DnnGlobalResourcesRuRuGuardTests` (7 tests, lecture `HEAD:`)

1. `TheThirtyKeysExistInTheNeutral` — anti-vacuité ;
2. `TheThirtyKeysAreDeliveredInRuRu` — livrées et non vides ;
3. `NoRuRuCellIsTheEnglishCopy` ;
4. `PlaceholderParityWithTheNeutral` ;
5. `RuRuKeepsItsTransportAndDidNotLoseKeys` — BOM + LF + **212 au parseur** ;
6. `NoNewCellCarriesLatinTextOutsideTokens` — propre à ce pack : hors jetons DNN, cyrillique présent et **aucune** lettre latine. Garde de **contamination, pas de fidélité** (un emprunt translittéré comme « хост » passe, c'est voulu). Contrôle inverse in-test : un mot anglais resté en place et un gabarit réduit à ses jetons sont vus, la forme livrée passe ;
7. `NoNewCellCarriesEscapedEntityText` — aucun `&amp;` / `&quot;` dans le texte parsé, contrôle inverse inclus.

**Banc rouge-avant / vert-après, mesuré** : garde committée seule sur le pack pristine → **2 échecs nominatifs** (`TheThirtyKeysAreDeliveredInRuRu`, `RuRuKeepsItsTransportAndDidNotLoseKeys`), 5 verts ; après le commit du lot → **7/7**.

## Reste à faire (hors périmètre)

- ar-SA, fa-IR, zh-CN (un grain par langue), chacun avec le relevé d'entités et une garde d'écriture propre à son script.

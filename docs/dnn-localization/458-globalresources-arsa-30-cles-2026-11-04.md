# #458 — `GlobalResources` ar-SA : les 30 clés ajoutées au neutre par DNN 10.4.1

**Date :** 04/11/2026 · **Base :** `8c74ee13` · **Grain :** pool #458 c.6638047321, suite ① (ru → **ar** → fa → zh)
**Périmètre :** 1 resx (`DNNPlatform/App_GlobalResources/GlobalResources.ar-SA.resx`), 30 cellules ajoutées, 0 cellule existante touchée. Jumeau ar-SA de fr-FR (#2133), pt-BR (#2143) et ru-RU (#2147), même lot de clés.

## État mesuré avant le geste

| Pack | `<data>` au parseur | Clés neuves présentes |
|---|---:|---:|
| neutre | 215 | 30 (les ajouts 10.4.1) |
| es-ES | 209 | 30 (paquet officiel aligné — référence sémantique, jamais une source) |
| **ar-SA** | **172** | **0** |

Transport ar-SA mesuré : **BOM UTF-8 présente**, LF seuls, 84 236 octets, insertions avant `</root>`.

> ⚠️ **Piège d'instrument payé ici (2ᵉ occurrence de la famille)** : une première sonde d'en-tête comptait 0 exemple `<data>` dans le commentaire d'en-tête — elle coupait au **premier** `<data ` du fichier, qui est *à l'intérieur* du commentaire (ligne 20). Re-sonde par zones de commentaires : le pack porte **4** exemples (`Name1`/`Color1`/`Bitmap1`/`Icon1`, lignes 20-25), grep 176 vs parseur 172 — **même structure que ru-RU**. L'affirmation initiale « aucun exemple d'en-tête » a été rétractée et corrigée dans la garde. Règle : une sonde qui coupe au premier motif peut s'arrêter *dans* un faux positif ; découper par zones délimitées, pas par premier hit.

## Traductions — terminologie comptée dans le pack, jamais inventée

| Terme neutre | Rendement ar-SA | Compte dans le pack |
|---|---|---|
| role | **دور** | présent (`EMAIL_*`, libellés historiques) |
| Password Reset Token | **رمز إعادة تعيين كلمة المرور** | attesté (frères `EMAIL_*`) |
| user name / display name | **اسم المستخدم** / **الاسم المعروض** | attestés |
| pending approval / approved | **في انتظار الموافقة** / **تمت الموافقة** | attestés |
| registered user | **مستخدم مسجَّل** / **المستخدمون المسجَّلون** | singulier attesté (« التسجيل الموثّق » voisin) |
| login | **تسجيل الدخول** | attesté |
| website | **الموقع الإلكتروني** | attesté (frères `EMAIL_*`) |
| host | **المضيف** (jamais le latin « host ») | — |
| guillemets en prose | **« »** | 9 paires au pack historique |

**Formations naturelles SANS précédent pack** — le pack de 172 clés ne couvre pas ce domaine, l'absence n'est pas une contre-preuve (déclarées, pas comptées) : `محررو المحتوى` (Content Editors), `مديرو المحتوى` (Content Managers), `المشتركون` (Subscribers), `المستخدمون المتميزون` (Superusers), `المستخدمون غير المصادَقين` (Unauthenticated).

## Chirurgie et contrôles

- Insertion de 30 blocs `<data>` avant `</root>`, style du pack respecté (`xml:space="preserve"` sur `<data>`, `<value>` nu — uniforme sur les 172 cellules).
- **Delta d'octets asserté AVANT écriture** : +7 431 attendus = réels ; fix-up ligne vide d'ouverture +1 (voir §Forme du corps e-mail). Assertions pré : BOM présente, 0 CR, **172 `<data>` au parseur** (et 176 au grep, les 4 exemples d'en-tête), ancre `</root>` unique.
- Post : **202 `<data>` au parseur**, BOM conservée, 0 CR, XML revalidé, **round-trip 30/30 valeurs exactes**, **parité des jetons 30/30** (`{N}` + `[Xxx:Yyy]`, espaces internes normalisés), **0 copie anglaise**, **0 lettre latine hors jetons** (le « host » du neutre rendu par المضيف), **0 entité échappée** (`&amp;`/`&quot;` — engagement #2143 tenu), numstat **95/0** (insertion pure).

### Forme du corps e-mail — ligne vide d'ouverture (fix-up +1 o)

Mesuré : le neutre, es-ES, fr-FR (#2133) et pt-BR (#2143) ouvrent tous ce corps par `\n` ; le pack ar-SA historique n'a **aucun** corps multi-lignes (0 précédent, dans aucun sens). Décision : **miroir du neutre** — la ligne vide d'ouverture est la forme amont, les 4 packs alignés la portent. (La sonde de forme a aussi révélé que ru-RU est absent de l'arbre de travail : normal, #2147 n'était pas encore mergée à `8c74ee13` — fichiers distincts, aucun conflit.)

## Garde `DnnGlobalResourcesArSaGuardTests` (8 tests, lecture `HEAD:`)

1. `TheThirtyKeysExistInTheNeutral` — anti-vacuité (les 30 existent au neutre) ;
2. `TheThirtyKeysAreDeliveredInArSa` — livrées et non vides ;
3. `NoArSaCellIsTheEnglishCopy` — aucune copie anglaise (aucun homographe légitime dans ce lot) ;
4. `PlaceholderParityWithTheNeutral` — multiset des jetons identique au neutre ;
5. `ArSaKeepsItsTransportAndDidNotLoseKeys` — **BOM présente** + LF seuls + compte épinglé à **202** (au parseur) ;
6. `NoNewCellCarriesLatinTextOutsideTokens` — hors jetons DNN, une cellule neuve est en arabe sans lettre latine ; contrôles inverses in-test (mot anglais resté, gabarit vidé, forme livrée) ;
7. `NoNewCellCarriesPersianExclusiveLetters` — **propre à ce pack** : miroir de la garde fa-IR (#2141) ; ni پ چ ژ گ ni ی/ک persanes ni ZWNJ dans une cellule neuve (contamination de l'autre pack, pas une variante légitime) ; contrôle inverse sur le témoin «گفت‌وگو» du lot fa-IR ;
8. `NoNewCellCarriesEscapedEntityText` — le texte parsé d'une cellule neuve ne contient ni `&amp;` ni `&quot;` (engagement #2143, c.6629942784) ; contrôles inverses inclus.

**Banc rouge-avant mesuré** (garde seule sur l'arbre pristine, `8c74ee13`) : **2 échecs nominatifs** (`TheThirtyKeysAreDeliveredInArSa`, `ArSaKeepsItsTransportAndDidNotLoseKeys`), 6 verts. Le vert n'est atteignable qu'après le commit du lot — la garde lit `HEAD:`, voulu (#2138).

## Reste à faire (hors périmètre de ce grain)

- Les 2 packs restants de la suite ① : fa-IR, zh-CN (un grain par langue), chacun avec le relevé d'entités échappées et la sonde de forme du corps e-mail.
- #2147 (ru-RU) en attente de merge ai-01 — aucun fichier commun avec ce grain.

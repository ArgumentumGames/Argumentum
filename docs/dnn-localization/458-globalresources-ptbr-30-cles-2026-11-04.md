# #458 — `GlobalResources` pt-BR : les 30 clés ajoutées au neutre par DNN 10.4.1

**Date :** 04/11/2026 · **Base :** `af8723f3` · **Grain :** pool #458 c.6623723673, po-2024 item ①
**Périmètre :** 1 resx (`DNNPlatform/App_GlobalResources/GlobalResources.pt-BR.resx`), 30 cellules ajoutées, 0 cellule existante touchée. Jumeau pt-BR du grain fr-FR (#2133), même lot de clés.

## État mesuré avant le geste

| Pack | `<data>` totaux | Clés neuves présentes |
|---|---:|---:|
| neutre | 215 | 30 (les ajouts 10.4.1) |
| es-ES | 209 | 30 (paquet officiel aligné — référence sémantique, jamais une source) |
| fr-FR | 279 | 30 (livrées par #2133) |
| **pt-BR** | **182** | **0** |

⚠️ **Les packs ne sont pas jumeaux historiques** : `fr − pt = 97` clés, dont seulement 30 au neutre — les 67 autres sont des clés mortes que fr-FR porte et pt-BR n'a jamais eues (libellés de menu `//Admin//…`legacy). Le périmètre du lot = **exactement les 30 du neutre**, recoupées avec es-ES (30/30).

Transport pt-BR mesuré : **BOM UTF-8 présente**, LF seuls, 84 059 octets, insertions avant `</root>` (pas de trailer commentaire contrairement à fr-FR).

> ⚠️ Piège d'instrument payé ici : une première sonde comparait les 3 premiers octets au littéral **doublement échappé** `b'\\xef\\xbb\\xbf'` (12 octets de texte, pas les 3 octets EF BB BF) et lisait « BOM absente ». Le dry-run du script de chirurgie, dont le littéral était correct, a réfuté la sonde ; hexdump `od -An -tx1` a tranché : **BOM présente**. Toute conclusion « absente » sur un littéral d'octets se re-mesure avec un instrument bas niveau avant d'écrire la garde.

## Traductions — terminologie comptée dans le pack, jamais inventée

| Terme neutre | Rendement pt-BR | Compte dans le pack |
|---|---|---:|
| role | **função** (função/Função/funções) | 11 |
| Password Reset Token | **Token de Redefinição de Senha** | « Token » 7 · « redefini » 6 |
| Password Reminder (sujet) | **Lembrete de senha** | 2 (frères `EMAIL_PASSWORD_REMINDER_*`) |
| registration / registered user | **registro** / **usuário registrado** | 12 · présent (`EMAIL_USER_REGISTRATION_PUBLIC_BODY`) |
| pending approval | **pendente de aprovação** | « aprovação » 3 · « aprovado » présent |
| Everyone | **Todos** | 2 |
| e-mail / page / content / state | e-mail / página / conteúdo / estado | 6 · 27 · 36 · 4 |
| host | **host** (usuários host) | 10 |

**Formations naturelles SANS précédent pack** — le pack de 182 clés ne couvre pas ce domaine, l'absence n'est pas une contre-preuve (déclarées, pas comptées) : `Superusuários`, `Gestores de Conteúdo`, `gerenciar`, `Assinantes`, `rejeitar`, `descartar`.

Le corps e-mail administrateur **miroire la forme du neutre** (ligne vide d'ouverture, enveloppe `xml:space="preserve"` sur `<data>`), comme le fr-FR de #2133. Sujet pack : `[Portal:PortalName] Lembrete de senha …` suit la convention des frères existants.

## Chirurgie et contrôles

- Insertion de 30 blocs `<data>` avant `</root>`, style du pack respecté (`<value xml:space="preserve">` pour les cellules simples — 158 cellules existantes sans attribut sur `<data>` ; attribut porté par `<data>` pour le corps multi-lignes).
- **Delta d'octets asserté AVANT écriture** : +6 158 attendus = 6 158 réels (84 059 → 90 217). Assertions pré : BOM présente, 0 CR, 182 `<data>`, ancre `</root>` unique.
- Post : **212 `<data>`**, BOM conservée, 0 CR, XML revalidé, **round-trip 30/30 valeurs exactes** (l'enveloppe `\n…\n    ` de la cellule multi-lignes attendue telle quelle), **parité des jetons 30/30** (`{N}` + `[Xxx:Yyy]`, espaces internes normalisés), **0 copie anglaise**, **0 marqueur espagnol** (`ción`/`sión`/`ñ`).

## Garde `DnnGlobalResourcesPtBrGuardTests` (6 tests, lecture `HEAD:`)

1. `TheThirtyKeysExistInTheNeutral` — anti-vacuité (les 30 existent au neutre) ;
2. `TheThirtyKeysAreDeliveredInPtBr` — livrées et non vides ;
3. `NoPtBrCellIsTheEnglishCopy` — aucune copie anglaise (aucun homographe légitime dans ce lot) ;
4. `PlaceholderParityWithTheNeutral` — multiset des jetons identique au neutre ;
5. `PtBrKeepsItsTransportAndDidNotLoseKeys` — **BOM présente** + LF seuls + compte épinglé à **212** ;
6. `NoNewCellCarriesThePackSSpanishContamination` — propre à ce pack : le pt-BR descend historiquement de l'espagnol (« Actualización », « envío » mesurés dans son corpus, cf. `DnnPtBrSpanishSurfaceGuardTests`) ; les cellules neuves écrivent `-ção`/`-são`, jamais `-ción`/`-sión`/`ñ`.

**Banc rouge-avant / vert-après, mesuré** : garde committée seule sur l'arbre pristine → **2 échecs nominatifs** (`TheThirtyKeysAreDeliveredInPtBr`, `PtBrKeepsItsTransportAndDidNotLoseKeys`), 4 verts ; après le commit du lot → **6/6**.

## Reste à faire (hors périmètre de ce grain)

- Les 4 packs restants : ru-RU, ar-SA, fa-IR, zh-CN (un grain par langue).
- La surface espagnole HISTORIQUE du pack pt-BR (cellules existantes « Actualización », « envío », etc.) : volet es du pool, ces cellules ne sont pas touchées par ce lot.

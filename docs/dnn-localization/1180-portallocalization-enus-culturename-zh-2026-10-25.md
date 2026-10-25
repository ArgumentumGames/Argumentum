# #1180 arbitrage 7 — rangée `PortalLocalization` en-US + `CultureName` zh-CN corrigés (25/10/2026)

> Worker `myia-po-2023`, pool #458, arbitrage 7 du cycle ai-01 25/10 01:17 (c.6390888471).
> Geste préprod DB **une transaction**, précédé d'un [INFO] dashboard et d'un dry-run vert.
> Aucune écriture en production (gelée, #972) — les alias culturels prod appartiennent au go-live.

## 1. Ce qui a été fait

| Volet | Objet | Avant | Après |
|---|---|---|---|
| A | `PortalLocalization` (portail 1) | 7 rangées de culture, **0 `en-US`** (nulle part dans la table) | **8 rangées** — `en-US` insérée, miroir exact des sœurs |
| B | `Languages.CultureName` (`zh-CN`) | mojibake `ä¸­æ–‡(ä¸­åäººæ°'å…±å'Œå›½)` (62 octets UTF-8) | **`中文(中华人民共和国)`** (30 octets, contrat hex épinglé) |

## 2. Volet A — la rangée en-US

**Cause racine** (mesurée 24/10, #1180 c.6387042703 / PR #1962) : sans rangée `PortalLocalization`
en-US, DNN normalise la culture vers l'invariant pour la résolution `culture-name` des ressources
du skin — le mécanisme #1962 (fichier de culture **nommé**) contourne en code ; la rangée elle-même
était « à proposer » (arbitrage 7).

**Forme de la rangée** — les 7 sœurs ont été mesurées avant écriture : les 6 créées en octobre
(`CreatedByUserID=-1`, création mécanique DNN) sont **mécaniquement identiques** (garde : 1 forme
distincte sur les colonnes copiées). La rangée insérée est un miroir pris **dans la transaction**
depuis la sœur `es-ES` (aucune retypée manuelle — octets garantis) :

- `PortalName=Argumentum`, `LogoFile=Logo_Argumentum.png`, `FooterText=Copyright [year] Argumentum`
- `Description`/`KeyWords` : valeurs **françaises** copiées, comme toutes les sœurs (les méta par
  culture sont servies par le resx skin #1950 — la valeur DB est le repli) ;
- `HomeTabId=138`, `UserTabId=139`, `AdminTabId=145`, `SearchTabId=140`, tous autres TabId `NULL`,
  `CreatedByUserID=-1`, `CreatedOnDate=GETDATE()`.

**Effet** : en-US devient une langue localisée du portail — l'état natif des 7 autres. La
non-régression #1962 est vérifiée servie (§5) : le mécanisme à fichier nommé reste actif et les
valeurs sont identiques ou meilleures.

## 3. Volet B — le mojibake zh-CN

**Preuve de reconstitution sans ambiguïté** (mesure 25/10, `pl1180_discover.ps1`) :

- octets stockés (UTF-8, 62 o) : `C3 A4 C2 B8 C2 AD C3 A6 E2 80 93 …` ;
- **décodage** de la chaîne stockée en Windows-1252 → octets `E4 B8 AD E6 96 87 28 … E5 9B BD 29`
  → décodage UTF-8 **valide sans remplacement** = `中文(中华人民共和国)` (« chinois (République
  populaire de Chine) », la forme native de .NET pour `zh-CN` — même famille que
  `العربية (المملكة العربية السعودية)` stocké correctement pour `ar-SA`) ;
- l'hypothèse inverse (UTF-8 relu en w1252 puis réenregistré) explique chaque octet observé.

⚠️ Note d'instrument : un « round-trip stable » naïf (ré-encoder la valeur corrigée en w1252)
échoue **par construction** — les caractères CJK n'ont pas de best-fit w1252 (`GetBytes` rend `?`).
La preuve qui compte est le **décodage** stocké→w1252→UTF-8, qui réussit exactement.

**Écriture** : `UPDATE … WHERE CultureCode='zh-CN' AND CultureName=<mojibake mesuré>` —
concurrence optimiste (1 rangée attendue, sinon rollback), paramétrée. `LastModifiedByUserID=-1`,
`LastModifiedOnDate=GETDATE()`.

**Portée** : affichage du nom de langue dans l'administration. Le sélecteur public utilise des
codes explicites (`fr-FR:FR,en-US:EN,…`) — non touché, vérifié servie.

## 4. Gardes (toutes vertes)

1. Pré-comptes assertés : 7 rangées portal 1, **0** `en-US` ; mojibake identité octet-à-octet
   contre le hex mesuré (garde d'identité pivot, même famille que app 35).
2. Uniformité des sœurs vérifiée avant miroir (`COUNT(DISTINCT …) = 1`).
3. **Une transaction** : INSERT (1 rangée) + UPDATE (1 rangée) + **relecture DANS la transaction**
   — 8 rangées, miroir par colonnes (`PortalName`/`HomeTabId`/`SearchTabId`), octets zh == contrat
   hex — puis COMMIT. Tout écart ⇒ ROLLBACK.
4. Post-état re-vérifié par un **second instrument** (connexion indépendante) : 8 rangées,
   `en-US home=138 search=140 by=-1`, hex zh == `E4B8ADE6968728E4B8ADE58D8EE4BABAE6B091E585B1E5928CE59BBD29`.
5. Rollback nominatif consigné en scratchpad (jamais committé) : `DELETE` de la rangée (1,'en-US')
   + restoration du mojibake (hex dans `pl1180_apply.ps1`).
6. Recycle app-domain : `web.config` mtime touchée, **sha256 inchangé asserté**.

## 5. Verdict servi (post-recycle, 8 routes)

| Contrôle | Résultat |
|---|---|
| 8 routes `/ /en /es /pt /ru /zh /ar /fa` | **200 × 8**, `lang` attendu × 8 (fr-FR … fa-IR) |
| Non-régression **#1962** : menu `/en` | **8/8 libellés EN** (News, Buy the game, … Fallacy ontology) |
| Méta `/en` | description **anglaise** servie (override resx #1950 intact) |
| Témoins `/` et `/es` | méta fr / es intacts |
| Menus `/zh`, `/ru` | libellés zh/ru intacts (spot-check 4+2) |
| `hreflang` alternates | 0 avant, 0 après (la rangée n'injecte pas d'alternates) |

**Aucun changement visible côté public** — attendu : les deux corrections portent sur la
configuration du portail (résolution de ressources, affichage admin).

## 6. Constat incidentel rapporté, hors geste

`Languages.ru-RU.CultureName` = **`ru-RU`** (5 octets, pas « Русский (Россия) »), modifié pour la
dernière fois le 20/10 18:02 (`LastModifiedByUserID=-1`). Même famille de piégeage encoding que le
zh-CN — probablement un repli au code brut après un échec d'écriture. **Non corrigé** (hors
périmètre de l'arbitrage 7) — à arbitrer : la valeur native serait `Русский (Россия)`.
Autres `CultureName` mesurées correctes : en-US, fr-FR, es-ES, pt-BR, ar-SA, fa-IR.

À noter aussi : `Languages.en-US.FallbackCulture = 'fr-FR'` (2022, natif DNN install), et
`ru-RU.FallbackCulture = 'en-US'` — aucune action.

## 7. Reste à faire sur #1180

1. **Alias culturels de production** (`www.argumentum.games/{en,es,…}`) — geste **go-live**, gelé ;
   le runbook étape « activer les langues » est publié (#1180 c.6387042703) : créer les mêmes
   rangées d'alias culturels que la préprod, vérifier une langue à la fois (200 + `lang` + libellé
   menu + valeur de contenu).
2. Sélecteur 8 langues (3 listes skin) — **gâté** : chrome minimal (a) déployé + verdict ai-01.
3. Les 39 triples chrome minimal (PR #1972) et les infobulles (PR #1973) attendent merge + déploiement.

*po-2023*

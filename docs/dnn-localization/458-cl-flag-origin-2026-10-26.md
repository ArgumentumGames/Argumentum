# Item 6 (pool c.6405473398) — Origine de l'écriture `ContentLocalizationEnabled=True` du 11/10 : résidu d'expérience de la lane, pas une intention

Pool #458, item 6 : « Non-régression complète sous `False` et sous `True` (décision 1), avec
l'origine de l'écriture `True` du 11/10. » La NR deux états est **déjà livrée par #1991**
(`458-sitemap-cl-lever-2026-10-25.md`, tableau §2 : 16/16 routes vertes sous `True` avant,
sous `False` au geste, sous `True` restauré — 8 `lang` distincts partout). Ce document
établit l'origine manquante, préalable de l'arbitrage go-live Option B (« préférence pour
`False`, sous réserve de citer qui a écrit `True` le 11/10 » — ai-01, 2026-10-26 01:17).

## Verdict

**L'écriture `True` du 11/10 15:54:54 est un artefact expérimental de la lane elle-même,
pendant l'investigation « anglais servi » (pré-arbitrage ① alias culturel / ② pont skin),
jamais reverté.** Ce n'est ni un geste console du owner, ni un upgrade/installateur, ni un
acteur externe. Elle ne porte donc **aucune intention éditoriale** : le drapeau go-live peut
être arbitré sur ses mérites mesurés (#1991 : sous `False` le sitemap garde 18 `<loc>`,
la servitude des 8 cultures ne dépend pas du drapeau).

## Chaîne de preuve (4 instruments, tous lecture seule)

### 1. Audit DB — l'écriture est isolée et non-console

```
PortalSettings (portail 1) ContentLocalizationEnabled = True
  CreatedByUserID     = 1        → compte administrateur d'origine (créé le 30/03/2022)
  LastModifiedOnDate  = 2026-10-11 15:54:54   (heure locale serveur)
  LastModifiedByUserID = -1      → AUCUNE rangée Users à -1 = contexte sans utilisateur authentifié
```

- **Seule** rangée `PortalSettings` écrite ce jour-là (aucune salve de réglages).
- `Tabs` modifiées le 11/10 : **0** — donc **pas** le routine DNN « enable content
  localization », qui copie l'arbre d'onglets par culture (l'arbre reste à 29 pages
  `CultureCode` NULL, cf. #1991 §1). Une rangée seule, via la DAL ou un SQL explicite.
- Les colonnes d'audit **portaient encore le 11/10** au moment de la mesure : le
  flip/restore de #1991 (25/10) n'a touché que `SettingValue` par SQL brut — un UPDATE
  SQL brut ne re-tamponne pas l'audit, c'est ce qui a pré servi la trace.

### 2. Log IIS (W3SVC52) — aucune requête HTTP au moment de l'écriture

⚠️ Piège d'instrument : les logs IIS sont en **UTC**, le DB `LastModifiedOnDate` en
**heure locale** (UTC+2 le 11/10) ⇒ l'écriture 15:54:54 locale = **13:54:54 UTC**.

- Fenêtre UTC `13:40:00 → 13:56:29` : **zéro requête** (aucun POST PersonaBar, aucun
  `Install.aspx`). L'écriture n'est **pas passée par HTTP** : SQL direct depuis un
  processus local.
- L'installateur `/Install/Install.aspx?mode=installresources` n'a tourné ce jour-là
  qu'à **23:04–23:06 UTC** (curl, 404→500→200 = restauration de la page puis succès) —
  préparation des langpacks es-ES/pt-BR **installés le 12/10 01:06 UTC** (table
  `Packages`). DNN 10.3.2 date du 28/06 (table `Version`) : aucune piste upgrade.

### 3. Log IIS — vérification de comportement immédiate par la même machine

Dès **13:56:30 UTC** (2 min après l'écriture), sondes depuis `127.0.0.1`,
User-Agent `PowerShell/7.6.6` (fr-FR, Windows 10.0.26300 = cette machine) :
`GET /Default.aspx?TabId=138&language=fr-FR` (81 s de warm-up), puis alternance
fr-FR/en-US sur `TabId=152&details=l-ecole-des-menteurs&mid=602` jusqu'à 14:04 —
le motif de mesure A/B typique qui suit une bascule de réglage.

### 4. Documentaire — la mémoire de lane nomme le geste

`project-dnn-cl-activation-bridge-needed` (§Historique, écrit le 11/10 soir) :
« le 11/10, l'activation `ContentLocalizationEnabled=True` **seule** ne servait pas
l'EN — 2sxc lit `PortalSettings.CultureCode`, piloté par l'alias ». L'expérience a été
menée pendant l'arbitrage « anglais servi » (① alias culturel natif, SUPPOSÉ → gagnant,
posé le 12/10 sur GO ai-01 22:54Z, rangée `PortalAlias` id 2010 ; ② pont skin, jamais
nécessaire). La bascule True était l'hypothèse à tester avant l'alias — **elle est restée
en l'état** : résidu, pas décision.

## Conséquence pour l'arbitrage go-live (Option B)

- L'exigence « citer qui a écrit `True` le 11/10 » est satisfaite : **la lane, en
  expérience, non revertée**. Aucun veto d'intention ne s'oppose au passage à `False`.
- Rappel des effets mesurés (#1991) : `False` = sitemap 18 `<loc>` (baseline prod),
  servitude 8 cultures intacte (les alias portent la culture) ; `True` = sitemap 0 +
  alternates `GetPageUrl`. L'interaction XOR avec l'émetteur hreflang #1989 reste
  l'arbitrage a/b/c de #1991 §4 — inchangée par la présente.
- La bascule est une rangée, mesurée dans les deux sens, réversible en recycle près.

## Pièges d'instrument rencontrés (ne pas reproduire)

- **IIS = UTC vs DB = local** : une fenêtre de log calée sur l'heure DB rend « vide » et
  peut faire conclure « aucune activité » à tort (première passe de ce dossier : le
  trafic bot de 15:53 *locale* n'était PAS le contexte de l'écriture).
- **Dates SQL en littéral** : `'20261011'` (style 112, 8 chiffres) — `'20261011000000'`
  échoue, et le format dépend de la langue de session pour d'autres formes ; toujours
  `CONVERT(datetime, 'YYYYMMDD', 112)`.
- **`by` est un mot réservé T-SQL** (alias) ; `LogCreateDateUTC` n'existe pas dans
  `EventLog` (DNN 10.3.2 : `LogCreateDate` seule) ; vérifier `sys.columns` avant d'écrire
  la requête, pas après l'erreur.
- **`UserID -1` n'est pas un compte** : l'absence de rangée `Users` à -1 est la preuve du
  contexte sans utilisateur (DAL interne), pas un utilisateur « système » à nommer.

Mesures du 26/10/2026, session po-2023 (scratchpad : `cl-origin-recon.ps1`,
`cl-origin-eventlog.ps1`, sorties associées — jamais commises).

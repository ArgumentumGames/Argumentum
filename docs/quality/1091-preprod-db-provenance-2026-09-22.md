# #1091 — Provenance de la base de préprod : est-elle issue d'une sauvegarde de prod ?

**Date** : 2026-09-22 ~12:00 · **Lane** : po-2023 (worker) · **Base** : `origin/master` `32f9b826`
**Nature** : mesure **lecture seule** sur la base de préprod (`localhost\SQLEXPRESS` / `ArgumentumGames`,
SQL Server **16.0.1200.5 Express**, compte applicatif `dnn_user`).
**Instrument** : `sqlcmd` batches (`-i`, séparateur `GO`) + inspection disque. Batterie : [`tools/1091-db-provenance-probes.sql`](../../tools/1091-db-provenance-probes.sql).
**Interdits respectés** : ⛔ aucune écriture (gel #972) · ⛔ aucune archive ouverte/extractée/déplacée · ⛔ **aucune**
valeur de credential, hash ou sel reproduit · ⛔ aucun `XMLData` lu (il porte des clés live en clair + des données
client) · ⛔ aucune recherche de port, credential ou contournement NAT (ce n'est pas un problème d'ingéniosité).

---

## Réponse au DoD

> DoD du grain ⑧ : « la préprod **est / n'est pas** issue d'une sauvegarde de prod, **avec sa date et la preuve** ».

**OUI — mesure.** La base de préprod **est** une **restauration** d'une sauvegarde de la base du site, prise le
**2026-06-28 à 22:07:48** (`ArgumentumGames_live_post-migration_20260628.bak`, le fichier est **encore sur le
disque**, octet pour octet cohérent avec l'enregistrement `msdb`), restaurée le **2026-06-29 à 10:50:28** avec
`REPLACE`, par le compte de service SQL.

**Conséquence directe pour #1091** : la mesure de 112 lignes faite le 16/08 **porte bien sur le corpus réel du
site**, arrêté au **28/06/2026 22:07** — et non sur un jeu de données de démonstration ou une installation
fraîche. Le blocage « accès prod » tombe pour cette question-là : les 112 lignes **sont** l'instantané du corpus
à cette date.

---

## 1. Le fait mesuré, et sa triple source

Trois instruments **indépendants** concordent (aucun ne dérive de l'autre) :

| # | Instrument | Valeur mesurée |
|---|---|---|
| a | `sys.databases.create_date` | **2026-06-29T10:50:28.780** |
| b | `msdb.dbo.restorehistory` (1 seule ligne sur l'instance) | `restore_date` = **2026-06-29T10:50:28.730**, `destination_database_name` = `ArgumentumGames`, `[replace]` = 1, `user_name` = `AUTORITE NT\Système` |
| c | `msdb.dbo.backupset` (1 seule ligne pour cette base) | `backup_start` = `backup_finish` = **2026-06-28T22:07:48**, `database_name` = `ArgumentumGames`, `backup_size` = 97 895 424, `physical_device_name` = `D:\Dev\Argumentum\tmp\dnn-backups\ArgumentumGames_live_post-migration_20260628.bak` |
| d | disque (lecture seule) | `ArgumentumGames_live_post-migration_20260628.bak` = **97 927 168 o**, `mtime` = **2026-06-28 22:07** |

**(a) et (b) concordent à 50 ms près** — deux vues serveur distinctes, dont l'une est un journal d'exploitation
(`msdb`) que le code applicatif DNN n'écrit pas. **(d) confirme (c)** : l'horodatage du fichier au disque tombe à
la minute du `backup_finish_date`. L'écart de taille (97 927 168 − 97 895 424 = **31 744 o**) est l'en-tête/pied
de média non comptés dans `backup_size` — **cohérent, pas contradictoire**.

**MESURÉ** : les valeurs ci-dessus. **DÉRIVÉ** : que `create_date` = instant de la restauration (comportement
documenté de `RESTORE`, qui réécrit l'en-tête de la base — un `CREATE DATABASE … FOR ATTACH` **conserverait** la
date de création d'origine ; une base de 2020-2022 attachée le 29/06 porterait donc 2020-2022 en `create_date`,
pas 2026-06-29).

## 2. Contre-épreuve 1 — l'hypothèse « installation fraîche » est réfutée

Une base créée le 29/06/2026 ne peut pas contenir de lignes créées en 2020. Mesuré :

| Table | Plus ancienne ligne | Nombre |
|---|---|---|
| `dbo.Users` | **2020-07-23T16:10:54** | 112 |
| `dbo.Tabs` | **2020-07-23T16:10:44** (plus récente 2024-02-14T13:40:22) | 38 |
| `dbo.Portals` | **2022-03-30T09:55:42** (`PortalName` = « Argumentum ») | 1 |
| `dbo.EventLog` | **2022-04-05T04:06:21** | 2 499 |

Le contenu **précède de ~6 ans** la date de création de la base. L'hypothèse « installation fraîche » est morte.

## 3. Contre-épreuve 2 — le falsificateur qui discrimine les deux candidats

**Deux** sauvegardes du même jour existent sur le disque :

- `ArgumentumGames_pre-migration_20260628.bak` — 96 485 376 o — **16:37**
- `ArgumentumGames_live_post-migration_20260628.bak` — 97 927 168 o — **22:07**

**Test falsifiant** : si la restauration provenait de la sauvegarde **de 16:37**, le dernier évènement journalisé
dans la base ne pourrait pas être **postérieur** à 16:37. Mesuré — dernière ligne `EventLog` **antérieure** à
l'instant de restauration : **2026-06-28T18:42:25.657** ; première ligne **postérieure** : **2026-07-01T00:04:02.003**.

`18:42:25 > 16:37` ⟹ **le candidat de 16:37 est exclu**, seul celui de 22:07 est compatible. `msdb` n'en
enregistre d'ailleurs qu'**un seul** (`backupset` = 1 ligne pour `ArgumentumGames`, `restorehistory` = 1 ligne sur
toute l'instance). Le falsificateur **discrimine** — il ne décore pas.

## 4. La lignée du corpus — d'où vient cette base

`dbo.EventLog.LogServerName` rattache chaque ligne à la **machine qui l'a écrite** (colonne écrite par DNN, non
modifiable par le compte applicatif de lecture) :

| Machine | Lignes avant restauration | Fenêtre | Après restauration |
|---|---:|---|---:|
| `MyIA-Web1` | 623 | 2022-04-05 → **2026-05-30** | 0 |
| `myia-po-2023` | 405 | **2026-06-02** → **2026-06-28T18:42:25** | 1 434 |
| `aricie-vm-jesse` | 37 | 2022-05-05 → 2023-02-20 | 0 |

Réparti par année (avant la coupure) : 2022 → 24, 2023 → 269, 2024 → 188, 2025 → 71, 2026 → 513. La base
**continue** une histoire de site qui a vécu sur `MyIA-Web1` jusqu'au **30/05/2026**, puis sur `myia-po-2023` à
partir du **02/06/2026**, et qui a été **coupée net le 28/06/2026 à 18:42** — l'instant de la sauvegarde.

⚠️ **Note d'instrument — les compteurs qui bougent.** La base **journalise en continu** : `eventlog_newest`,
`eventlog_rows` et la ligne `srv_postcut` de `myia-po-2023` **croissent entre deux exécutions** (mesuré :
1 947 lignes 2026 puis 1 948, `srv_postcut` 1 434 puis 1 435 en quelques minutes le 22/09). Un rejeu de la
batterie ne rendra donc **pas** ces trois valeurs à l'identique, et ce n'est pas un échec — ce sont des
compteurs vivants. Les valeurs **figées** sur lesquelles porte la démonstration (date de sauvegarde, date de
restauration, vintage 2020-2022, lignée par machine, alias, frontière 28/06 → 01/07) sont, elles, stables.

Les alias du portail (`dbo.PortalAlias`, 4 lignes) portent, à côté des alias de préprod
(`dnn.argumentum.myia.io`, `argumentum.myia.org`, `argumentum2.dnndev.me`), l'alias **`www.argumentum.games`** —
le domaine de **production**.

**DÉRIVÉ** (pas mesuré) : que le site servi par cette base le 28/06 était l'instance de *production*. Ce que la
mesure établit, c'est que la base **est** celle du site (lignée, alias prod, 112 comptes, 124 commandes), et que
le nom du fichier de sauvegarde porte `_live_`. L'affectation machine↔rôle (`MyIA-Web1` = prod à cette date)
reste une **SUPPOSÉ** non mesurée ici.

## 5. Instruments qui ne pouvaient pas répondre — dits comme tels

- **`sys.databases.source_database_id` = NULL** : cette colonne n'est renseignée que pour un *snapshot* de base.
  Elle vaut NULL **dans les deux hypothèses** (restaurée ou non). **Instrument non discriminant** — sa valeur ne
  prouve rien, dans aucun sens.
- **`sys.master_files`** : renvoie **0 ligne** pour ce compte (vue filtrée par permission). Ce `0` **n'est pas
  une absence de fichier** — c'est une limite d'instrument. Contournement employé : `SERVERPROPERTY
  ('InstanceDefaultDataPath')` → `D:\SQLData\`.
- **Horodatage des fichiers `.mdf`/`.ldf`** : `D:\SQLData` est **ACL-refusé** au compte de sonde (durcissement
  standard de SQL Server). Ce 4ᵉ témoin, qui aurait dû porter la minute de la restauration, **n'a pas pu être
  lu**. Les trois instruments disponibles concordent ; l'absence du quatrième est une limite, pas un résultat.

## 6. Ce que cette mesure n'établit pas

- **Que la prod d'aujourd'hui soit identique à cet instantané.** La prod est sur une ops VPS dé-couplée ; si elle
  a reçu des écritures **après** le 28/06/2026 22:07, son corpus courant est **plus récent** que celui-ci. Le
  comparatif prod↔préprod est précisément ce que ce grain n'avait pas à aller chercher (⛔ aucun port, credential
  ou contournement NAT).
- **Que l'instance d'écriture du 28/06 était l'hôte de production** (`www.argumentum.games`) — voir §4, DÉRIVÉ.
- **Rien sur l'archive de 2023** : elle n'a été ni ouverte, ni extraite, ni déplacée, ni testée. Voir le grain ⑨.
- **Aucun verdict sur le contenu des données** : aucun `XMLData`, aucun credential, aucun hash, aucune identité
  client n'a été lu. Seuls des **comptes, des dates et des noms de machine** ont été mesurés.

---

## Reproductibilité

```bash
# batterie complète (lecture seule), à lancer avec un compte applicatif en lecture
sqlcmd -S "<serveur>" -d "<base>" -i tools/1091-db-provenance-probes.sql -h -1 -W
```
La batterie est en batches `GO` : un lot compilé d'un bloc s'annule **entier** sur une seule colonne invalide (piège
rencontré : `EventLog.CreatedOnDate` n'existe pas en DNN 10 — la colonne est `LogCreateDate`), et rend alors
`0 ligne` là où l'on croit lire une absence.

**L'instrument livré a été exécuté tel quel avant commit** (`-i` sur le fichier du dépôt, compte `dnn_user`) :
il rend les 30 lignes `K|...` reproduisant **toutes** les valeurs citées ici, y compris les sondes `msdb`
(accessibles à ce compte) et `SERVERPROPERTY` (le chemin `D:\SQLData\` a été obtenu ainsi, `sys.master_files`
étant vide pour ce compte). Les trois compteurs vivants signalés au §3 près, le rejeu est identique.

---
*po-2023 (worker lane) · instruments : `sqlcmd` (batches `GO`), inspection disque lecture seule ·
verdict/arbitrage : ai-01 & jsboige · po-2023 signale, ne déclare pas PASS.*

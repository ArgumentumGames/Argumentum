-- 1091-db-provenance-probes.sql
-- Battery: is a DNN database a RESTORE of an existing database, or a fresh install?
-- Read-only. No writes, no XMLData, no credentials, no customer rows are read.
--
-- Usage:
--   sqlcmd -S "<server>" -d "<db>" -i tools/1091-db-provenance-probes.sql -h -1 -W
--
-- WHY BATCHES: a T-SQL batch is compiled as a whole. One invalid column name aborts the
-- ENTIRE batch and returns zero rows — which reads like "no data" when it is a compile
-- error. Every probe is therefore its own batch (GO separator), so one bad probe cannot
-- mask the others. (Observed in practice: EventLog.CreatedOnDate does not exist in DNN 10;
-- the column is LogCreateDate.)
--
-- READING THE RESULT: each line is  K|<probe>|<value>[|<value>...]
--
-- WHAT THIS CANNOT ANSWER: sys.databases.source_database_id is populated only for database
-- SNAPSHOTS. It is NULL whether or not the database was restored, so it discriminates
-- nothing. Do not read it as evidence in either direction.
-- sys.master_files typically returns 0 rows for a non-privileged application account
-- (permission-filtered view): that 0 is an instrument limit, not a missing file.

SET NOCOUNT ON;

-- 1. Database creation stamp. A RESTORE rewrites the database header, so create_date is the
--    restore instant. CREATE DATABASE ... FOR ATTACH preserves the ORIGINAL creation date.
SELECT 'K|db_create_date|' + CONVERT(varchar(30), create_date, 126) FROM sys.databases WHERE name = DB_NAME();
GO
SELECT 'K|db_recovery|' + recovery_model_desc + '|' + state_desc FROM sys.databases WHERE name = DB_NAME();
GO
-- Non-discriminating by design (see header). Printed so the reader sees it was not skipped.
SELECT 'K|db_source_id|' + ISNULL(CONVERT(varchar(10), source_database_id), 'NULL') FROM sys.databases WHERE name = DB_NAME();
GO

-- 2. The authoritative proof, when msdb history has not been purged: who restored what, from
--    which backup file, and when.
SELECT 'K|restore|' + CONVERT(varchar(30), restore_date, 126) + '|' + ISNULL([user_name], '<null>')
       + '|' + destination_database_name + '|' + CONVERT(varchar(6), [replace])
  FROM msdb.dbo.restorehistory;
GO
SELECT 'K|bset|' + CONVERT(varchar(30), backup_finish_date, 126) + '|' + database_name
       + '|' + ISNULL(server_name, '<null>') + '|' + ISNULL(CONVERT(varchar(24), backup_size), '-')
       + '|' + physical_device_name
  FROM msdb.dbo.backupset bs
  JOIN msdb.dbo.backupmediafamily bmf ON bmf.media_set_id = bs.media_set_id
 ORDER BY backup_finish_date;
GO

-- 3. Content vintage. A database created recently that holds rows older than itself was NOT
--    freshly installed. This is the cheapest single falsifier of the "fresh install" reading.
SELECT 'K|users_oldest|' + CONVERT(varchar(30), MIN(CreatedOnDate), 126) FROM dbo.Users;
GO
SELECT 'K|users_rows|' + CONVERT(varchar(12), COUNT(*)) FROM dbo.Users;
GO
SELECT 'K|tabs_oldest|' + CONVERT(varchar(30), MIN(CreatedOnDate), 126) + '|' + CONVERT(varchar(30), MAX(CreatedOnDate), 126) FROM dbo.Tabs;
GO
SELECT 'K|portals_oldest|' + CONVERT(varchar(30), MIN(CreatedOnDate), 126) FROM dbo.Portals;
GO
SELECT 'K|eventlog_oldest|' + CONVERT(varchar(30), MIN(LogCreateDate), 126) FROM dbo.EventLog;
GO
SELECT 'K|eventlog_newest|' + CONVERT(varchar(30), MAX(LogCreateDate), 126) FROM dbo.EventLog;
GO

-- 4. The falsifier that DISCRIMINATES between two candidate backups taken the same day.
--    Compare the last logged event BEFORE the database creation stamp against the candidate
--    files' timestamps: a backup cannot contain events that happened after it was written.
--    Replace the literal with db_create_date from probe 1.
SELECT 'K|evt_year|' + CONVERT(varchar(4), YEAR(LogCreateDate)) + '|' + CONVERT(varchar(12), COUNT(*))
  FROM dbo.EventLog GROUP BY YEAR(LogCreateDate) ORDER BY YEAR(LogCreateDate);
GO
SELECT 'K|evt_last_before_cut|' + CONVERT(varchar(30), MAX(LogCreateDate), 126)
  FROM dbo.EventLog WHERE LogCreateDate < '2026-06-29T10:50:28';
GO
SELECT 'K|evt_first_after_cut|' + CONVERT(varchar(30), MIN(LogCreateDate), 126)
  FROM dbo.EventLog WHERE LogCreateDate >= '2026-06-29T10:50:28';
GO

-- 5. Which machine WROTE each log line: the database's lineage across hosts. DNN sets this
--    column; it is not something the application account can forge from a reader session.
SELECT 'K|srv_precut|' + ISNULL(LogServerName, '<null>') + '|' + CONVERT(varchar(12), COUNT(*))
       + '|' + CONVERT(varchar(30), MIN(LogCreateDate), 126) + '|' + CONVERT(varchar(30), MAX(LogCreateDate), 126)
  FROM dbo.EventLog WHERE LogCreateDate < '2026-06-29T10:50:28' GROUP BY LogServerName ORDER BY COUNT(*) DESC;
GO
SELECT 'K|srv_postcut|' + ISNULL(LogServerName, '<null>') + '|' + CONVERT(varchar(12), COUNT(*))
  FROM dbo.EventLog WHERE LogCreateDate >= '2026-06-29T10:50:28' GROUP BY LogServerName ORDER BY COUNT(*) DESC;
GO

-- 6. Which site is this database for? Portal aliases include the production domain, which
--    ties the corpus to the live site rather than to a scratch install.
SELECT 'K|alias|' + HTTPAlias FROM dbo.PortalAlias;
GO
SELECT 'K|portalname|' + ISNULL(LogPortalName, '<null>') + '|' + CONVERT(varchar(12), COUNT(*))
  FROM dbo.EventLog WHERE LogCreateDate < '2026-06-29T10:50:28' GROUP BY LogPortalName ORDER BY COUNT(*) DESC;
GO

-- 7. Where the files live (sys.master_files is usually hidden from an application account).
SELECT 'K|datapath|' + ISNULL(CONVERT(varchar(300), SERVERPROPERTY('InstanceDefaultDataPath')), '<null>');
GO
SELECT 'K|ver|' + CONVERT(varchar(50), SERVERPROPERTY('ProductVersion')) + '|' + CONVERT(varchar(50), SERVERPROPERTY('Edition'));
GO

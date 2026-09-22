-- 1091-shop-go-live-probes.sql
-- Battery: date a DNN/NBrightBuy shop's go-live WITHOUT reading any payload.
-- Read-only. No XMLData is read (it carries live payment keys in plaintext and customer PII);
-- only counts and dates are returned.
--
-- Usage:
--   sqlcmd -S "<server>" -d "<db>" -i tools/1091-shop-go-live-probes.sql -h -1 -W
--
-- OUTPUT: K|<probe>|<value>[|<value>...]
--
-- WHY BATCHES: a T-SQL batch compiles as a whole; one invalid column aborts the whole batch and
-- returns zero rows, which reads like "no data". Each probe is its own batch (GO).
--
-- INSTRUMENT LIMIT, STATED UP FRONT: NBrightBuy has NO creation-date column, only ModifiedDate.
-- MIN(ModifiedDate) therefore gives an UPPER BOUND on the first event (a row is created at or
-- before it is modified), never an exact creation date. Say "at or before", not "on".
--
-- WITNESS: the same battery must return pre-target dates for unrelated TypeCodes (PLUGIN/USERDATA/
-- GROUP/MANUALPAYMENT are plugin defaults dated 2020) and for the shop page Tab (2022). If the
-- instrument can see 2020 and 2022, then a 2023 floor on ORDER/CATEGORY is a property of the shop
-- and not instrument blindness. Run probe 4 and probe 1 together or the floor proves nothing.

SET NOCOUNT ON;

-- 1. Does the shop page predate the sales? (It usually does — page existence is NOT sales start.)
SELECT 'K|tab_shop|' + TabName + '|' + ISNULL(CONVERT(varchar(30), CreatedOnDate, 126), '-')
       + '|' + ISNULL(CONVERT(varchar(30), LastModifiedOnDate, 126), '-')
  FROM dbo.Tabs
 WHERE TabName LIKE '%cheter%' OR TabName LIKE '%outique%' OR TabName LIKE '%hop%' OR TabName LIKE '%rder%';
GO

-- 2. The catalogue's own floor: when was the shop SET UP (clients, shipping, categories, product langs)?
SELECT 'K|type_floor|' + TypeCode + '|' + CONVERT(varchar(12), COUNT(*)) + '|' + CONVERT(varchar(30), MIN(ModifiedDate), 126)
  FROM dbo.NBrightBuy GROUP BY TypeCode ORDER BY MIN(ModifiedDate);
GO

-- 3. Payment provider configuration vs first order: the two stamps that define "open for sales".
SELECT 'K|provider|' + TypeCode + '|' + CONVERT(varchar(30), MIN(ModifiedDate), 126)
  FROM dbo.NBrightBuy WHERE TypeCode LIKE '%PAYMENT%' GROUP BY TypeCode ORDER BY MIN(ModifiedDate);
GO
SELECT 'K|first_order|' + CONVERT(varchar(30), MIN(ModifiedDate), 126) FROM dbo.NBrightBuy WHERE TypeCode = 'ORDER';
GO
SELECT 'K|order_years|' + CONVERT(varchar(4), YEAR(ModifiedDate)) + '|' + CONVERT(varchar(12), COUNT(*))
  FROM dbo.NBrightBuy WHERE TypeCode = 'ORDER' GROUP BY YEAR(ModifiedDate) ORDER BY YEAR(ModifiedDate);
GO
SELECT 'K|order_months_first_year|' + CONVERT(varchar(2), MONTH(ModifiedDate)) + '|' + CONVERT(varchar(12), COUNT(*))
  FROM dbo.NBrightBuy WHERE TypeCode = 'ORDER' AND YEAR(ModifiedDate) = YEAR((SELECT MIN(ModifiedDate) FROM dbo.NBrightBuy WHERE TypeCode = 'ORDER'))
 GROUP BY MONTH(ModifiedDate) ORDER BY MONTH(ModifiedDate);
GO

-- 4. WITNESS: the same battery over unrelated TypeCodes must reach well before the floor.
SELECT 'K|witness_pre2023|' + TypeCode + '|' + CONVERT(varchar(30), MIN(ModifiedDate), 126)
  FROM dbo.NBrightBuy GROUP BY TypeCode HAVING MIN(ModifiedDate) < '2023-01-01';
GO

-- 5. FALSIFIER for "no sales before YYYY": one ORDER row earlier would refute it.
SELECT 'K|order_bounds|' + CONVERT(varchar(4), MIN(YEAR(ModifiedDate))) + '|' + CONVERT(varchar(4), MAX(YEAR(ModifiedDate)))
  FROM dbo.NBrightBuy WHERE TypeCode = 'ORDER';
GO
SELECT 'K|orders_before_2023|' + CONVERT(varchar(12), COUNT(*)) FROM dbo.NBrightBuy WHERE TypeCode = 'ORDER' AND ModifiedDate < '2023-01-01';
GO

-- 6. Customer accounts by year: a second, independent curve of the same ramp.
SELECT 'K|client_years|' + CONVERT(varchar(4), YEAR(ModifiedDate)) + '|' + CONVERT(varchar(12), COUNT(*))
  FROM dbo.NBrightBuy WHERE TypeCode = 'CLIENT' GROUP BY YEAR(ModifiedDate) ORDER BY YEAR(ModifiedDate);
GO
SELECT 'K|client_rows|' + CONVERT(varchar(12), COUNT(*)) FROM dbo.NBrightBuy WHERE TypeCode = 'CLIENT';
GO

-- 7. Column inventory — proof that no creation-date column exists (so the ModifiedDate caveat is real).
SELECT 'K|cols|' + c.name FROM sys.columns c WHERE c.object_id = OBJECT_ID('dbo.NBrightBuy') ORDER BY c.column_id;
GO

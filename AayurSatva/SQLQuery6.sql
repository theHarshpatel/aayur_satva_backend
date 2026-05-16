SELECT TOP (1000) [CoId]
      ,[CoName]
  FROM [AayurSatvaDB].[dbo].[Companies]




/*  -- Identity insert chalu karo jethi manually ID insert thai sake
SET IDENTITY_INSERT Companies ON;

-- ID 3 walo data ID 1 sathe insert karo
INSERT INTO Companies (CoId, CoName)
SELECT 1, CoName FROM Companies WHERE CoId = 3;

-- Juno record delete karo
DELETE FROM Companies WHERE CoId = 3;

-- Identity insert bandh karo
SET IDENTITY_INSERT Companies OFF; */



--DBCC CHECKIDENT ('Companies', RESEED, 1);

-- TRUNCATE TABLE Companies;   -- for delete
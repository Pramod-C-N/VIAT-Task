USE [brady]
GO
/****** Object:  User [guru]    Script Date: 01-09-2023 17:11:22 ******/
CREATE USER [guru] FOR LOGIN [guru] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [guru]
GO

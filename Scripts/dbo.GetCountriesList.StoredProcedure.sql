USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetCountriesList]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create     procedure [dbo].[GetCountriesList]  
as   
begin    
select DISTINCT Name,AlphaCode from country where IsActive=1 and tenantid is NULL ORDER BY Name asc;    
end
GO

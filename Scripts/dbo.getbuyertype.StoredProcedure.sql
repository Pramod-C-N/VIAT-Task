USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[getbuyertype]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create     procedure [dbo].[getbuyertype]  
as  
begin  
select Description  from OrganisationType where tenantid=2  
end
GO

USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetTenantpartnerinfoById]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create    Procedure [dbo].[GetTenantpartnerinfoById]                
(                
@Id INT          
)                
as                
begin                
select *,ts.UniqueIdentifier as patunique from TenantShareHolders ts where tenantid=@id and IsDeleted=0
end
GO

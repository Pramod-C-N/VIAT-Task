USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetVendorName]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create        Procedure [dbo].[GetVendorName]      
(      
@Name nvarchar(max),    
@tenantId int=null)      
as        
Begin        
select id,name,TenantType,ConstitutionType,Nationality,ContactNumber from  vendors      
where name lIKE concat('%',@Name,'%')  and  TenantId=@tenantId    
end
GO

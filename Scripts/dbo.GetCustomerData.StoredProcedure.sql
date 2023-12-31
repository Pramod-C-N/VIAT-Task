USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerData]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create          Procedure [dbo].[GetCustomerData]        
(      
@tenantId int=null      
)      
as          
Begin          
select id,name,TenantType,ConstitutionType,Nationality,ContactNumber from  customers      
where TenantId=@tenantId      
end
GO

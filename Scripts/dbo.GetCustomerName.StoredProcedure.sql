USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerName]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE       PROCEDURE [dbo].[GetCustomerName]        
(			
@Name nvarchar(MAX),      
@tenantId int=NULL)        
AS          
Begin 

select top 1 sp.RegistrationName as Name,SP.AdditionalData1 as ad1,* from  SalesInvoiceParty  sp
inner join SalesInvoiceAddress sd on sd.IRNNo=sp.IRNNo 
where RegistrationName lIKE concat('%',@Name,'%')  and  sp.TenantId=@tenantId  and sp.Type='Buyer' and sd.Language='EN'    

end






GO

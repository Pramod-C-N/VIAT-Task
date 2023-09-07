USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerById]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec [GetCustomerById] 113, 2
CREATE           Procedure [dbo].[GetCustomerById]            
(            
	@Id INT,      
	@tenantId INT      
)            
as            
begin        

	select c.id,c.TenantId,Name,Nationality,ContactPerson,ContactNumber,            
	isnull(BuildingNo,'') BuildingNo, isnull(AdditionalNo,'') AdditionalNo, isnull(Street,'') Street, isnull(AdditionalStreet,'') AdditionalStreet,
	isnull(Neighbourhood,'') Neighbourhood, isnull(city,'') city, isnull(State,'') State, isnull(PostalCode,'')  PostalCode, isnull(EmailID,'') EmailID, isnull(cd.DocumentNumber,'') DocumentNumber        
	from customers c            
	--inner join CustomerAddress ca on c.id=ca.CustomerID and c.TenantId=ca.TenantId
	left outer join CustomerAddress ca on c.id=ca.CustomerID and c.TenantId=ca.TenantId    
	--inner join CustomerDocuments cd on c.id=cd.CustomerID and c.TenantId=cd.TenantId  
	left outer join CustomerDocuments cd on c.id=cd.CustomerID and c.TenantId=cd.TenantId    
	where c.id=@Id and c.TenantId=@tenantId    
	--and cd.DocumentTypeCode = 'VAT'  

end
GO

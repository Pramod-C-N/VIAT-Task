USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetTenantById]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE              Procedure [dbo].[GetTenantById]                  
(                  
@Id INT            
)                  
as                  
begin                  
select *,tpvb.TenancyName,td.UniqueIdentifier as docunique ,format(td.RegistrationDate,'dd-MM-yyyy'),substring(LastReturnFiled,0,11) as LastReturnFiled,
concat(ta.BuildingNo,' ',ta.Street,' ',ta.AdditionalStreet,' ',ta.Neighbourhood,' ',ta.city,' ',ta.State) as tenantdd,concat(ta.CountryCode,',',ta.PostalCode) as tenAdd2
 from (  
(select * from TenantBasicDetails where TenantId=@Id) t   
inner join (select * from TenantAddress) ta on t.TenantId=ta.TenantId  
left join (select * from TenantDocuments where IsDeleted=0) td on t.TenantId=td.TenantId  
left join (select * from TenantBusinessPurchase) tp on t.TenantId=tp.TenantId  
left join (select * from TenantBusinessSupplies ) tbs on t.TenantId=tbs.TenantId  
left join (select * from TenantSupplyVATCategory ) tsv on t.TenantId=tsv.TenantId  
left join (select * from TenantPurchaseVatCateory ) tpv on t.TenantId=tpv.TenantId  
left join (select * from AbpTenants ) tpvb on t.TenantId=tpvb.Id  
)   
end
GO

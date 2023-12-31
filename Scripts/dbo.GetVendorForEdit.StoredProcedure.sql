USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetVendorForEdit]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE          Procedure [dbo].[GetVendorForEdit]        
(        
@id int,    
@tenantid int    
)        
as        
begin        
select *,vd.UniqueIdentifier as docunique   from 
(select * from vendors  where tenantid=@tenantid and Id=@id) c    
inner join (select * from VendorAddress where isdeleted=0)ca on c.id=ca.VendorID and c.TenantId=ca.TenantId 
left join (select * from VendorDocuments where isdeleted=0)vd on c.id=vd.VendorID and c.TenantId=vd.TenantId 
left join (select * from VendorTaxDetails where isdeleted=0)vt on c.id=vt.VendorID and c.TenantId=vt.TenantId 
left join (select * from VendorForeignEntity where isdeleted=0) vf on c.id=vf.VendorID and c.TenantId=vf.TenantId; 
;        
end
GO

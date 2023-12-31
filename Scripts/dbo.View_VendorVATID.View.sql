USE [brady]
GO
/****** Object:  View [dbo].[View_VendorVATID]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[View_VendorVATID] 
as  select c.id,c.tenantid,c.name,c.legalname,c.uniqueidentifier,d.documentnumber
from Vendors c left outer join VendorDocuments d on c.id = d.VendorID where d.DocumentName ='VAT'
GO

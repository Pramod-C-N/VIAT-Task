USE [brady]
GO
/****** Object:  View [dbo].[View_CustomerVATID]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[View_CustomerVATID] 
as  select c.id,c.tenantid,c.name,c.legalname,c.uniqueidentifier,d.documentnumber
from customers c left outer join customerdocuments d on c.id = d.CustomerID where d.DocumentName ='VAT'
GO

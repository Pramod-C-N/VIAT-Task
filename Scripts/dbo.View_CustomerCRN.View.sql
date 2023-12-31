USE [brady]
GO
/****** Object:  View [dbo].[View_CustomerCRN]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   view [dbo].[View_CustomerCRN] as SELECT c.Id, c.TenantId, c.Name, c.LegalName, c.UniqueIdentifier, d.DocumentNumber,d.doumentdate
FROM   dbo.Customers AS c LEFT OUTER JOIN
             dbo.CustomerDocuments AS d ON c.Id = d.CustomerID
WHERE (d.DocumentName = 'CRN')
GO

USE [vitadb11]
GO
/****** Object:  StoredProcedure [dbo].[GetPurchaseData]    Script Date: 4/13/2023 3:15:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 ALTER    PROCEDURE [dbo].[GetPurchaseData]  
@fromDate datetime,  
@toDate datetime,
@tenantId int = null
AS  
SET NOCOUNT ON

select   
  s.Id as InvoiceId,   
 format(s.IssueDate,'dd-MM-yyyy') as InvoiceDate,   
  i.LineAmountInclusiveVAT as Amount,   
  b.RegistrationName as CustomerName,   
  c.ContactNumber as ContactNo,  
  ''  as TransactionCategory  
from   
  PurchaseEntry s with (nolock)   
  inner join (select sum(LineAmountInclusiveVAT) as LineAmountInclusiveVAT,IRNNo,TenantId   
  from PurchaseEntryItem with (nolock)   
  group by IRNNo,TenantId) i on s.id = i.IRNNo   and Isnull(i.tenantid,0)=isnull(@tenantId,0) and Isnull(s.tenantid,0)=isnull(@tenantId,0)  
  inner join PurchaseEntryParty b with (nolock) on s.id = b.IRNNo   and Isnull(b.tenantid,0)=isnull(@tenantId,0) 
  and b.Type = 'Buyer'   
  inner join PurchaseEntryContactPerson c with (nolock) on s.id = c.IRNNo   and Isnull(c.tenantid,0)=isnull(@tenantId,0)
  and c.Type = 'Buyer'   
  where CAST(s.IssueDate AS DATE)>=cast(@fromDate as date) and CAST(s.IssueDate AS DATE)<=cast(@toDate as date)
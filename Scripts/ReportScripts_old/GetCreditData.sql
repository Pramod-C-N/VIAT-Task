USE [vitadb11]
GO
/****** Object:  StoredProcedure [dbo].[GetCreditData]    Script Date: 4/13/2023 3:13:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
ALTER       PROCEDURE [dbo].[GetCreditData]        
@fromDate Datetime,        
@toDate Datetime,    
@tenantId int = null     
AS
SET NOCOUNT ON

select         
  s.IrnNo as InvoiceId,         
  format(s.IssueDate,'dd-MM-yyyy') as InvoiceDate,      
  s.BillingReferenceId as ReferenceNumber,      
  i.LineAmountInclusiveVAT as Amount,         
  b.RegistrationName as CustomerName,         
  c.ContactNumber as ContactNo,      
  m.UniqueIdentifier      
from         
  CreditNote s with (nolock)
  inner join (select sum(LineAmountInclusiveVAT) as LineAmountInclusiveVAT,IRNNo,TenantId       
  from CreditNoteItem with (nolock)      
  group by IRNNo,TenantId) i on s.IRNNo = i.IRNNo and Isnull(i.tenantid,0)=isnull(@tenantId,0) and Isnull(s.tenantid,0)=isnull(@tenantId,0)      
  inner join IRNMaster m with (nolock) on s.IRNNo = m.IRNNo and Isnull(m.tenantid,0)=isnull(@tenantId,0)    
  inner join CreditNoteParty b with (nolock) on s.IRNNo = b.IRNNo and Isnull(b.tenantid,0)=isnull(@tenantId,0)        
  and b.Type = 'Buyer'         
  inner join CreditNoteContactPerson c with (nolock) on s.IRNNo = c.IRNNo  and Isnull(c.tenantid,0)=isnull(@tenantId,0)       
  and c.Type = 'Buyer'         
  where CAST(s.IssueDate AS DATE)>=CAST(@fromDate as date) and CAST(s.IssueDate AS DATE)<=CAST(@toDate as date)           
  order by s.CreationTime desc
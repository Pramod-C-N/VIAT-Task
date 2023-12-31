USE [vitadb11]
GO
/****** Object:  StoredProcedure [dbo].[GetDebitData]    Script Date: 4/13/2023 3:15:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER      PROCEDURE [dbo].[GetDebitData]      
@fromDate datetime,      
@toDate datetime  ,  
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
  DebitNote s with (nolock)       
  inner join (select sum(LineAmountInclusiveVAT) as LineAmountInclusiveVAT,IRNNo,TenantId     
  from DebitNoteItem  with (nolock)     
  group by IRNNo,TenantId) i  on s.IRNNo = i.IRNNo   and Isnull(i.tenantid,0)=isnull(@tenantId,0) and Isnull(s.tenantid,0)=isnull(@tenantId,0)      
  inner join IRNMaster m with (nolock) on s.IRNNo = m.IRNNo     and Isnull(m.tenantid,0)=isnull(@tenantId,0)   
  inner join DebitNoteParty b with (nolock) on s.IRNNo = b.IRNNo    and Isnull(b.tenantid,0)=isnull(@tenantId,0)    
  and b.Type = 'Buyer'       
  inner join DebitNoteContactPerson c with (nolock) on s.IRNNo = c.IRNNo     and Isnull(c.tenantid,0)=isnull(@tenantId,0)   
  and c.Type = 'Buyer'       
  where CAST(s.IssueDate AS DATE)>= cast(@fromDate as date) and CAST(s.IssueDate AS DATE)<= cast(@toDate as date)         
  order by s.CreationTime desc
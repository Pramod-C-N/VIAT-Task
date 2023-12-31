USE [vitadb11]
GO
/****** Object:  StoredProcedure [dbo].[GetSalesData]    Script Date: 4/13/2023 3:12:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER        PROCEDURE [dbo].[GetSalesData]          
@fromDate datetime,          
@toDate datetime,        
@tenantId int       
AS        
SET NOCOUNT ON

insert into dbo.logs    
values(cast(@fromDate as varchar),GETDATE(),@tenantId)    
select           
  s.IrnNo as InvoiceId,           
  format(s.IssueDate,'dd-MM-yyyy') as InvoiceDate,           
  i.LineAmountInclusiveVAT as Amount,           
  b.RegistrationName as CustomerName,           
  c.ContactNumber as ContactNo ,          
  m.UniqueIdentifier          
from           
  salesinvoice s  with (nolock)          
  inner join (select sum(LineAmountInclusiveVAT) as LineAmountInclusiveVAT,IRNNo,TenantId         
  from SalesInvoiceItem   with (nolock)      
  group by IRNNo,TenantId) i on s.IRNNo = i.IRNNo  and Isnull(i.tenantid,0)=isnull(@tenantId,0) and Isnull(s.tenantid,0)=isnull(@tenantId,0)        
  inner join IRNMaster m with (nolock) on s.IRNNo = m.IRNNo   and Isnull(m.tenantid,0)=isnull(@tenantId,0)        
  inner join SalesInvoiceParty b with (nolock) on s.IRNNo = b.IRNNo   and Isnull(b.tenantid,0)=isnull(@tenantId,0)        
  and b.Type = 'Buyer'           
  inner join SalesInvoiceContactPerson c with (nolock) on s.IRNNo = c.IRNNo   and Isnull(c.tenantid,0)=isnull(@tenantId,0)        
  and c.Type = 'Buyer'         
  where CAST(s.IssueDate AS DATE)>=CAST(@fromDate as Date) and CAST(s.IssueDate AS DATE)<= CAST(@toDate as Date)         
  order by s.CreationTime desc
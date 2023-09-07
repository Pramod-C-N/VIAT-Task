USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetDebitNotePeriodicalReportIssuedate]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE         procedure [dbo].[GetDebitNotePeriodicalReportIssuedate]    
(    
@fromDate DateTime=null,    
@toDate DateTime=null,
@tenantId int=null
)    
as begin    
    
select     
(case when (irnno is null or irnno = '') then InvoiceNumber else irnno end) as IRNNo,  
BillingReferenceId  as Invoicenumber, Invoicenumber as ReferenceNo,   
format(Issuedate,'dd-MM-yyyy')    
as  InvoiceDate,    
   
   
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)<>'GOVERNMENT'    
and BuyerCountryCode  like 'SA%') Then isnull(LineNetAmount,0) else 0 end ),0)     
 as TaxableAmount,    
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)='GOVERNMENT'    
and BuyerCountryCode  like 'SA%') Then isnull(LineNetAmount,0) else 0 end ),0)     
 as GovtTaxableAmt,    
isnull(sum(case when (VatCategoryCode='Z'     
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0)    
 as ZeroRated,    
isnull(sum(case when ( left(BuyerCountryCode,2) ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0)     
 as Exports,    
    
isnull(sum(case when (VatCategoryCode='E'     
and left(BuyerCountryCode,2) ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0)    
 as Exempt,   
 isnull(sum(case when (VatCategoryCode='O'     
and left(BuyerCountryCode,2) ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0) as OutofScope,  
vatRate as  VatRate,    
sum(VATLineAmount)  as  VatAmount,    
sum(LineAmountInclusiveVAT)  as  TotalAmount   
       
from VI_importstandardfiles_Processed where TenantId=@tenantId and format(issuedate,'yyyy-MM-dd') between     
@fromdate and @todate and invoicetype like 'Debit Note%'    
group by IRNNo,IssueDate,BillingReferenceId,InvoiceNumber,VatRate   
    
end
GO

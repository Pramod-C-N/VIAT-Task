USE [vitadb11]
GO
/****** Object:  StoredProcedure [dbo].[GetDebitNotePeriodicalReporteffdate]    Script Date: 4/13/2023 2:34:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER         procedure [dbo].[GetDebitNotePeriodicalReporteffdate]        
(        
@fromDate Date=null,        
@toDate Date=null,    
@tenantId int=null    
)        
as begin        
        
select         
(case when (irnno is null or irnno = '') then InvoiceNumber else irnno end) as IRNNo,      
BillingReferenceId  as Invoicenumber, BillingReferenceId as ReferenceNo,       
format(effdate,'dd-MM-yyyy')        
as  InvoiceDate,        
       
       
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)<>'GOVERNMENT'        
and BuyerCountryCode LIKE 'SA%') Then isnull(LineNetAmount,0) else 0 end ),0)         
 as TaxableAmount,        
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)='GOVERNMENT'        
and BuyerCountryCode LIKE 'SA%') Then isnull(LineNetAmount,0) else 0 end ),0)         
 as GovtTaxableAmt,        
isnull(sum(case when (VatCategoryCode='Z'         
and BuyerCountryCode LIKE 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0)        
 as ZeroRated,        
isnull(sum(case when ( BuyerCountryCode not LIKE 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0)         
 as Exports,        
        
isnull(sum(case when (VatCategoryCode='E'         
and BuyerCountryCode LIKE 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0)        
 as Exempt,       
 isnull(sum(case when (VatCategoryCode='O'         
and BuyerCountryCode LIKE 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as OutofScope,      
vatRate as  VatRate,        
sum(VATLineAmount)  as  VatAmount,        
sum(LineAmountInclusiveVAT)  as  TotalAmount       
           
from VI_importstandardfiles_Processed with (nolock) 
where TenantId=@tenantId and CAST(IssueDate AS DATE)>=@fromDate and CAST(IssueDate AS DATE)<=@toDate  
and invoicetype like 'Debit Note%'        
group by IRNNo,effdate,BillingReferenceId,InvoiceNumber,VatRate       
        
end
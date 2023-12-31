USE [vitadb11]
GO
/****** Object:  StoredProcedure [dbo].[GetSalesDetailedReportissuedate]    Script Date: 4/13/2023 2:29:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER         procedure [dbo].[GetSalesDetailedReportissuedate]   -- exec GetSalesDetailedReportissuedate '2022-09-01', '2022-09-30'    
(    
@fromDate Date=null,    
@toDate Date=null,  
@tenantId int=null  
)    
as begin    
    
select case when (irnno is null or irnno ='') then InvoiceNumber else irnno end as IRNNo,
case when (BillingReferenceId is null or len(trim(billingreferenceid)) = 0 or BillingReferenceId= -1)  
then InvoiceNumber else BillingReferenceId end as Invoicenumber,    
    
Format(IssueDate,'dd-MM-yyyy')  as  InvoiceDate,    
    
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)<>'GOVERNMENT'    
and left(BuyerCountryCode,2) ='SA') Then isnull(LineNetAmount,0)-isnull(advancercptamtadjusted,0) else 0 end ),0)     
 TaxableAmount,    
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)='GOVERNMENT'    
and left(BuyerCountryCode,2) ='SA') Then isnull(LineNetAmount,0)-isnull(advancercptamtadjusted,0) else 0 end ),0)     
 as GovtTaxableAmt,    
isnull(sum(case when (VatCategoryCode='Z'     
and left(BuyerCountryCode,2) ='SA') Then isnull(LineNetAmount ,0)-isnull(advancercptamtadjusted,0) else 0 end ),0)     
 as ZeroRated,    
isnull(sum(case when ( left(BuyerCountryCode,2) <>'SA') Then isnull(LineNetAmount ,0)-isnull(advancercptamtadjusted,0) else 0 end ),0)     
 as Exports,    
isnull(sum(case when (VatCategoryCode='E'     
and left(BuyerCountryCode,2) ='SA') Then isnull(LineNetAmount ,0)-isnull(advancercptamtadjusted,0) else 0 end ),0)     
as Exempt,    
isnull(sum(case when (VatCategoryCode='O'     
and left(BuyerCountryCode,2) ='SA') Then isnull(LineNetAmount ,0)-isnull(advancercptamtadjusted,0) else 0 end ),0)     
 as OutofScope,    
vatrate as Vatrate,    
sum(isnull(VATLineAmount,0)-isnull(VatOnAdvanceRcptAmtAdjusted,0))      
 as  VatAmount,    
sum(isnull(LineAmountInclusiveVAT,0)-isnull(VatOnAdvanceRcptAmtAdjusted,0)-isnull(advancercptamtadjusted,0))    
 as  TotalAmount,    
BillingReferenceId    
as ReferenceNo    
from VI_importstandardfiles_Processed with (nolock) where TenantId=@tenantId and
CAST(IssueDate AS DATE)>=@fromDate and CAST(IssueDate AS DATE)<=@toDate
and invoicetype like 'Sales Invoice%'    
group by IRNNo,IssueDate,InvoiceNumber,BillingReferenceId,VatRate    
    
end
USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetDebitNotePurchaseDaywiseReport]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    
    
create            procedure [dbo].[GetDebitNotePurchaseDaywiseReport]   --  exec GetDebitNotePurchaseDaywiseReport '2022-11-01', '2022-11-30'        
(        
@fromDate Date=null,        
@toDate Date=null,  
@tenantId int=null  
)        
as begin        
    
  
  exec GetDebitNotePurchaseDaywiseReportIssuedate @fromDate,@toDate,@tenantId  
  
   exec GetDebitNotePurchaseDaywiseReporteffdate @fromDate,@toDate,@tenantId  
----select         
--select        
--IssueDate as  InvoiceDate,count(invoicenumber) as invoicecount,    
--purchasecategory as Purchasecategory,        
--isnull(sum(case when (trim(VatCategoryCode)='S'         
--and trim(BuyerCountryCode) ='SA') Then isnull(LineNetAmount,0) else 0 end) ,0)      
-- as TaxableAmount,      
     
----isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)='GOVERNMENT'        
----and BuyerCountryCode ='SA') Then isnull(LineNetAmount,0) else 0 end ),0) as GovtTaxableAmt,        
--isnull(sum(case when (VatCategoryCode='Z'         
--) Then isnull(LineNetAmount ,0) else 0 end ),0)        
--    --and BuyerCountryCode ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0)        
-- as ZeroRated,      
--isnull(sum(case when (VatCategoryCode='E'         
--and BuyerCountryCode ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0) as Exempt,        
--isnull(sum(case when (VatCategoryCode='O'         
--and BuyerCountryCode ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0) as OutofScope,        
--isnull(sum(case when ( VatCategoryCode='S'  and RCMApplicable='0' and VATDeffered='0' and BuyerCountryCode <> 'SA')     
--Then isnull(LineNetAmount ,0) else 0 end ),0) as ImportVATCustoms,    
--isnull(sum(case when ( VatCategoryCode='S'  and RCMApplicable='0' and VATDeffered='1' and BuyerCountryCode <> 'SA')     
--Then isnull(LineNetAmount ,0) else 0 end ),0) as VATDeffered,    
--isnull(sum(case when ( VatCategoryCode='S'  and RCMApplicable='1') Then isnull(LineNetAmount ,0) else 0 end ),0) as ImportsatRCM,        
--sum(isnull(customspaid,0))         
-- as CustomsPaid,        
--sum(isnull(excisetaxpaid,0))         
-- as ExciseTaxPaid,        
--sum(isnull(OtherChargespaid,0))        
-- as OtherChargesPaid  ,        
--sum(VATLineAmount)  as  VatAmount,        
--sum(LineAmountInclusiveVAT) as  TotalAmount        
--from VI_importstandardfiles_Processed where TenantId=@tenantId and format(issuedate,'yyyy-MM-dd') between         
--@fromDate and @toDate and invoicetype like 'DN Purchase%'        
--group by IssueDate,purchasecategory       
        
  end
GO

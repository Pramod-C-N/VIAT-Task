USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetCreditNotePurchaseDaywiseReporteffdate]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
        
        
CREATE    procedure [dbo].[GetCreditNotePurchaseDaywiseReporteffdate]   --  exec GetCreditNotePurchaseDaywiseReporteffdate '2022-06-01', '2022-06-31',2            
(            
@fromDate Date=null,            
@toDate Date=null,      
@tenantId int=null      
)            
as begin            
        
--select             
select   format(effdate ,'dd-MM-yyyy')  --IssueDate       
 as  InvoiceDate,count(invoicenumber) as invoicecount,        
purchasecategory as Purchasecategory,            
isnull(sum(case when (trim(VatCategoryCode)='S'             
and trim(BuyerCountryCode) like 'SA%') Then isnull(LineNetAmount,0) else 0 end) ,0)          
 as TaxableAmount,          
         
--isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)='GOVERNMENT'            
--and BuyerCountryCode ='SA') Then isnull(LineNetAmount,0) else 0 end ),0) as GovtTaxableAmt,            
isnull(sum(case when (VatCategoryCode='Z'             
) Then isnull(LineNetAmount ,0) else 0 end ),0)            
    --and BuyerCountryCode ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0)            
 as ZeroRated,          
isnull(sum(case when (VatCategoryCode='E'             
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as Exempt,            
isnull(sum(case when (VatCategoryCode='O'             
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as OutofScope,            
isnull(sum(case when ( VatCategoryCode='S'  and RCMApplicable='0' and VATDeffered='0' and BuyerCountryCode not like 'SA%')         
Then isnull(LineNetAmount ,0) else 0 end ),0) as ImportVATCustoms,        
isnull(sum(case when ( VatCategoryCode='S'  and RCMApplicable='0' and VATDeffered='1' and BuyerCountryCode not like 'SA%')         
Then isnull(LineNetAmount ,0) else 0 end ),0) as VATDeffered,        
isnull(sum(case when ( VatCategoryCode='S'  and RCMApplicable='1') Then isnull(LineNetAmount ,0) else 0 end ),0) as ImportsatRCM,            
sum(isnull(customspaid,0))             
 as CustomsPaid,            
sum(isnull(excisetaxpaid,0))             
 as ExciseTaxPaid,            
sum(isnull(OtherChargespaid,0))            
 as OtherChargesPaid  ,            
sum(VATLineAmount)  as  VatAmount,            
sum(LineAmountInclusiveVAT) as  TotalAmount            
from VI_importstandardfiles_Processed where  TenantId=@tenantId and   
CAST(effdate AS DATE)>=@fromDate and CAST(effdate AS DATE)<=@toDate  
and invoicetype like 'CN Purchase%'            
group by effdate,purchasecategory           
            
  end
GO

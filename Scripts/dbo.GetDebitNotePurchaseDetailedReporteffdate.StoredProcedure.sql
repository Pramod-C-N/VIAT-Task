USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetDebitNotePurchaseDetailedReporteffdate]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      procedure [dbo].[GetDebitNotePurchaseDetailedReporteffdate]   --  exec GetDebitNotePurchaseDetailedReporteffdate '2022-11-01', '2022-11-30'            
(            
@fromDate Date=null,            
@toDate Date=null,        
@tenantId int=null        
)            
as begin            
        
        
--select             
select 
--(case when (VI.irnno is null or VI.irnno = '') then InvoiceNumber else VI.irnno end) as IRNNo,         
InvoiceNumber  as Invoicenumber,
 CASE
    WHEN b.Type = 'Supplier' THEN b.RegistrationName
    ELSE BuyerName
END as VendorName,
 format(effdate,'dd-MM-yyyy') as  InvoiceDate,
 billingreferenceid  as ReferenceNo,            
purchasecategory as Purchasecategory,            
isnull(sum(case when (trim(VatCategoryCode)='S'             
and trim(BuyerCountryCode) like 'SA%') Then isnull(LineNetAmount,0) else 0 end) ,0)          
 as TaxableAmount,  
 vatrate             
 as vatrate,        
sum(VATLineAmount)  as  VatAmount,            
sum(LineAmountInclusiveVAT) as  TotalAmount ,
         
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
 sum((isnull(VATLineAmount ,0))+(isnull(CustomsPaid ,0))+(isnull(ExciseTaxPaid ,0))+(isnull(OtherChargesPaid ,0))) as ChargesIncludingVAT            
           
from VI_importstandardfiles_Processed VI
left join PurchaseDebitNoteParty b with (nolock) on vi.IRNNo = b.IRNNo    and Isnull(b.tenantid,0)=isnull(@tenantId,0)  
and b.Type = 'Supplier' where       
CAST(effdate AS DATE)>=@fromDate and CAST(effdate AS DATE)<=@toDate      
and invoicetype like 'DN Purchase%'  and VI.TenantId=@tenantId          
group by VI.IRNNo,effdate,BillingReferenceId,InvoiceNumber,VatRate ,PurchaseCategory,RCMApplicable,VATDeffered,BuyerName, RegistrationName ,Type         
            
  end
GO

USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetPurchaseDaywiseReport]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE        procedure [dbo].[GetPurchaseDaywiseReport]      
(      
@fromDate Date=null,      
@toDate Date=null,  
@tenantId int=null  
)      
as begin      
      
select       
format(cast(IssueDate as date),'dd-MM-yyyy')       
 as  InvoiceDate,      
count(*)        
 as InvoiceNumber,      
      
isnull(sum(case when (VatCategoryCode='S'       
and LEFT(BuyerCountryCode,2) like 'SA%') Then isnull(LineNetAmount,0) else 0 end ),0)       
 as TaxableAmount,      
      
isnull(sum(case when (VatCategoryCode='Z'       
and LEFT(BuyerCountryCode,2) like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0)       
 as ZeroRated,      
      
isnull(sum(case when ( VatCategoryCode='S'  and VATDeffered=1) Then isnull(LineNetAmount ,0)       
else 0 end ),0)       
 as ImportsatCustoms,      
      
isnull(sum(case when ( VatCategoryCode='S'  and RCMApplicable=1) Then isnull(LineNetAmount ,0)       
else 0 end ),0)      
 as ImportsatRCM,      
      
rcmapplicable      
      
 as RCMApplicable,      
      
vatdeffered       
 as VATDeffered,      
      
sum(customspaid)       
 as CustomsPaid,      
      
sum(excisetaxpaid)      
 as ExciseTaxPaid,      
      
sum(OtherChargespaid)      
 as OtherChargesPaid  ,      
      
Purchasecategory      
 as PurchaseCategory,      
      
isnull(sum(case when (VatCategoryCode='E'       
and LEFT(BuyerCountryCode,2) like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0)       
 as Exempt, 
 isnull(sum(case when (VatCategoryCode='O'         
and LEFT(BuyerCountryCode,2) like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as OutofScope,
      
--sum(VATLineAmount)       
   isnull(sum(case when RCMApplicable=1 then 0
   when VATDeffered=1 then 0  else isnull(VATLineAmount,0) end),0)            

 as  VatAmount,      
      
sum(LineAmountInclusiveVAT)      
 as  TotalAmount      
      
from VI_importstandardfiles_Processed 
where TenantId=@tenantId and IssueDate >=@fromDate and IssueDate<=@toDate
--and format(issuedate,'yyyy-MM-dd') between       
--@fromDate and @toDate 
and invoicetype like 'Purchase%'      
group by cast(IssueDate as date),PurchaseCategory,RCMApplicable,VATDeffered      
ORDER BY CAST(ISSUEDATE AS DATE),PurchaseCategory,RCMApplicable,VATDeffered        
      
      
      
end
GO

USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetCreditNoteDaywiseReporteffdate]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      procedure [dbo].[GetCreditNoteDaywiseReporteffdate]    -- exec GetCreditNoteDaywiseReporteffdate '2023-02-09', '2023-02-09',2      
(          
@fromDate Date=null,          
@toDate Date=null,      
@tenantId int=null      
)          
as begin          
          
select           
         
format(effdate,'dd-MM-yyyy')          
as  InvoiceDate,    
  
sum(case when InvoiceLineIdentifier = 1 then 1 else 0 end) as Invoicenumber,  
         
         
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)<>'GOVERNMENT'          
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount,0) else 0 end ),0)           
 as TaxableAmount,          
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)='GOVERNMENT'          
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount,0) else 0 end ),0)           
 as GovtTaxableAmt,          
isnull(sum(case when (VatCategoryCode='Z'           
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0)          
 as ZeroRated,          
isnull(sum(case when ( BuyerCountryCode not like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0)           
 as Exports,          
          
isnull(sum(case when (VatCategoryCode='E'           
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0)          
 as Exempt,         
 isnull(sum(case when (VatCategoryCode='O'           
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as OutofScope,        
--vatRate as  VatRate,          
sum(VATLineAmount)  as  VatAmount,          
sum(LineAmountInclusiveVAT)  as  TotalAmount         
             
from VI_importstandardfiles_Processed where TenantId=@tenantId and     
CAST(effdate AS DATE)>=@fromDate and CAST(effdate AS DATE)<=@toDate    
and invoicetype like 'Credit Note%'          
group by effdate         
          
end
GO

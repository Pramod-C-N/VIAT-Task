USE [vitadb11]
GO
/****** Object:  StoredProcedure [dbo].[GetCreditDetailedReportEffdate]    Script Date: 4/13/2023 2:32:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER      procedure [dbo].[GetCreditDetailedReportEffdate]   --  exec GetCreditDetailedReportEffdate '2023-02-01', '2023-02-28' ,42           
(            
@fromDate Date=null,            
@toDate Date=null,      
@tenantId int=null      
)            
as begin            
            
            
select 
(case when (irnno is null or irnno = '') then InvoiceNumber else irnno end) as IRNNo,
case when (BillingReferenceId is null or len(trim(billingreferenceid)) = 0 or BillingReferenceId= '-1')  
then InvoiceNumber else BillingReferenceId end as Invoicenumber,      
InvoiceNumber as InvoiceNumber,            
format(effdate,'dd-MM-yyyy') as  InvoiceDate,            
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)<>'GOVERNMENT'            
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount,0) else 0 end ),0) as TaxableAmount,            
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)='GOVERNMENT'            
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount,0) else 0 end ),0) as GovtTaxableAmt,  



isnull(sum(case when (VatCategoryCode='Z'             
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as ZeroRated,            
isnull(sum(case when ( BuyerCountryCode NOT like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as Exports,            
isnull(sum(case when (VatCategoryCode='E'             
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as Exempt,            
isnull(sum(case when (VatCategoryCode='O'             
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as OutofScope,            
vatrate as Vatrate,            
sum(VATLineAmount)  as  VatAmount,            
sum(LineAmountInclusiveVAT) as  TotalAmount            
from VI_importstandardfiles_Processed with (nolock) where TenantId=@tenantId     
and  effdate >=@fromDate and effdate<=@toDate     
and invoicetype like 'Credit Note%'            
group by effdate,IRNNo,Billingreferenceid,invoicenumber,VatRate            
    end     
    
--select * from VI_importstandardfiles_Processed where TenantId=33
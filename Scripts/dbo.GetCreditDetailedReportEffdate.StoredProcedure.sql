USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetCreditDetailedReportEffdate]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE       procedure [dbo].[GetCreditDetailedReportEffdate]   --  exec GetCreditDetailedReportEffdate '2023-04-01', '2023-04-30' ,33             
(              
@fromDate Date=null,              
@toDate Date=null,        
@tenantId int=null        
)              
as begin              
              
              
select   
(case when (irnno is null or irnno = '') then InvoiceNumber else irnno end) as IRNNo,  
        
BuyerName as CustomerName,              
format(effdate,'dd-MM-yyyy') as  InvoiceDate,
case when (BillingReferenceId is null or len(trim(billingreferenceid)) = 0 or BillingReferenceId= '-1')    
then InvoiceNumber else BillingReferenceId end as Invoicenumber,
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)<>'GOVERNMENT'              
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount,0) else 0 end ),0) as TaxableAmount,
vatrate as Vatrate,              
sum(VATLineAmount)  as  VatAmount,              
sum(LineAmountInclusiveVAT) as  TotalAmount ,
    
isnull(sum(case when (VatCategoryCode='Z'               
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as ZeroRated,              
isnull(sum(case when ( BuyerCountryCode NOT like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as Exports,              
isnull(sum(case when (VatCategoryCode='E'               
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as Exempt,              
isnull(sum(case when (VatCategoryCode='O'               
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as OutofScope  ,
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)='GOVERNMENT'              
and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount,0) else 0 end ),0) as GovtTaxableAmt
             
from VI_importstandardfiles_Processed where TenantId=@tenantId       
and  effdate >=@fromDate and effdate<=@toDate       
and invoicetype like 'Credit Note%'              
group by effdate,IRNNo,Billingreferenceid,invoicenumber,VatRate,BuyerName             
    end       
      
--select * from VI_importstandardfiles_Processed where TenantId=33  
GO

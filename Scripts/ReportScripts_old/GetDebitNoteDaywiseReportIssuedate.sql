USE [vitadb11]
GO
/****** Object:  StoredProcedure [dbo].[GetDebitNoteDaywiseReportIssuedate]    Script Date: 4/13/2023 2:36:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER       procedure [dbo].[GetDebitNoteDaywiseReportIssuedate]            
(            
@fromDate Date=null,            
@toDate Date=null,        
@tenantId int=null        
)            
as begin            
            
select             
              
 Format(cast(Issuedate as date),'dd-MM-yyyy') --IssueDate             
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
               
from VI_importstandardfiles_Processed with (nolock)     
where TenantId=@tenantId and CAST(IssueDate AS DATE)>=@fromDate and CAST(IssueDate AS DATE)<=@toDate    
--format(issuedate,'yyyy-MM-dd') between             
--@fromdate and @todate     
and invoicetype like 'Debit Note%'            
group by cast(IssueDate as date)      
            
end
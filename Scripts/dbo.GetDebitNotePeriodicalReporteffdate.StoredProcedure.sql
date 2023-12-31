USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetDebitNotePeriodicalReporteffdate]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE         procedure [dbo].[GetDebitNotePeriodicalReporteffdate]          
(          
@fromDate Date=null,          
@toDate Date=null,      
@tenantId int=null      
)          
as begin          

select           
(case when (irnno is null or irnno = '') then InvoiceNumber else irnno end) as IRNNo,
BuyerName as customerName,
 --BillingReferenceId as ReferenceNo,         
format(effdate,'dd-MM-yyyy')          
as  InvoiceDate,
BillingReferenceId  as Invoicenumber,


isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)<>'GOVERNMENT'          
and BuyerCountryCode LIKE 'SA%') Then isnull(LineNetAmount,0) else 0 end ),0)           
as TaxableAmount,
vatRate as  VatRate,          
sum(VATLineAmount)  as  VatAmount,          
sum(LineAmountInclusiveVAT)  as  TotalAmount,
          
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
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)='GOVERNMENT'          
and BuyerCountryCode LIKE 'SA%') Then isnull(LineNetAmount,0) else 0 end ),0)           
as GovtTaxableAmt
         

from VI_importstandardfiles_Processed    
where TenantId=@tenantId and CAST(IssueDate AS DATE)>=@fromDate and CAST(IssueDate AS DATE)<=@toDate    
and invoicetype like 'Debit Note%'          
group by IRNNo,effdate,BillingReferenceId,InvoiceNumber,VatRate ,BuyerName        

end
GO

USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetCreditDetailedReportIssuedate]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   procedure [dbo].[GetCreditDetailedReportIssuedate]   --  exec GetCreditDetailedReportIssuedate '2022-09-01', '2022-09-30'      
(      
@fromDate Date=null,      
@toDate Date=null,
@tenantId int=null
)      
as begin      
      
--select       
select (case when (irnno is null or irnno = '') then InvoiceNumber else irnno end) as IRNNo,BillingReferenceId  as Invoicenumber,
BillingReferenceId as ReferenceNo,      
IssueDate as  InvoiceDate,      
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)<>'GOVERNMENT'      
and BuyerCountryCode ='SA') Then isnull(LineNetAmount,0) else 0 end ),0) as TaxableAmount,      
isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)='GOVERNMENT'      
and BuyerCountryCode ='SA') Then isnull(LineNetAmount,0) else 0 end ),0) as GovtTaxableAmt,      
isnull(sum(case when (VatCategoryCode='Z'       
and BuyerCountryCode ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0) as ZeroRated,      
isnull(sum(case when ( BuyerCountryCode <>'SA') Then isnull(LineNetAmount ,0) else 0 end ),0) as Exports,      
isnull(sum(case when (VatCategoryCode='E'       
and BuyerCountryCode ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0) as Exempt,      
isnull(sum(case when (VatCategoryCode='O'       
and BuyerCountryCode ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0) as OutofScope,      
vatrate as Vatrate,      
sum(VATLineAmount)  as  VatAmount,      
sum(LineAmountInclusiveVAT) as  TotalAmount      
from VI_importstandardfiles_Processed where TenantId=@tenantId and 
CAST(IssueDate AS DATE)>=@fromDate and CAST(IssueDate AS DATE)<=@toDate
and invoicetype like 'Credit Note%'      
group by IssueDate,IRNNo,BillingReferenceId,InvoiceNumber,VatRate      
    end  
      
--1 as IRNNo,      
----Invoicenumber      
--1 as Invoicenumber,      
----BillingReferenceId      
--1 as ReferenceNo,      
----IssueDate      
--1 as  InvoiceDate,      
      
----isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)<>'GOVERNMENT'      
----and BuyerCountryCode ='SA') Then isnull(LineNetAmount,0) else 0 end ),0)       
--1 as TaxableAmount,      
----isnull(sum(case when (VatCategoryCode='S' and upper(orgtype)='GOVERNMENT'      
----and BuyerCountryCode ='SA') Then isnull(LineNetAmount,0) else 0 end ),0)       
--1 as GovtTaxableAmt,      
      
----isnull(sum(case when (VatCategoryCode='Z'       
----and BuyerCountryCode ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0)       
--1 as ZeroRated,      
----isnull(sum(case when ( BuyerCountryCode <>'SA') Then isnull(LineNetAmount ,0) else 0 end ),0)       
--1 as Exports,      
----isnull(sum(case when (VatCategoryCode='E'       
----and BuyerCountryCode ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0)       
--1 as Exempt,      
----isnull(sum(case when (VatCategoryCode='O'       
----and BuyerCountryCode ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0)       
--1 as OutofScope,      
      
----vatrate       
--1 as Vatrate,      
----sum(VATLineAmount)        
--1 as  VatAmount,      
----sum(LineAmountInclusiveVAT)      
--1 as  TotalAmount      
      
----from VI_importstandardfiles_Processed where format(issuedate,'yyyy-MM-dd') between       
----@fromDate and @toDate and invoicetype like 'Credit Note%'      
----group by IRNNo,IssueDate,InvoiceNumber,VatRate,BillingReferenceId      
      
--end      
      
      
      
--select * from VI_importstandardfiles_Processed where InvoiceType like 'Credit%'      
      
--update VI_importstandardfiles_Processed set InvoiceType = 'Credit Note - Export' where InvoiceType ='Credit Invoice - Export'      
      
--update VI_importstandardfiles_Processed set InvoiceType = 'Credit Note - Standard' where InvoiceType ='Credit Invoice - Standard'   
GO

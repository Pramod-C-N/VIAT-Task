USE [vitadb11]
GO
/****** Object:  StoredProcedure [dbo].[GetPurchaseDetailedReport]    Script Date: 4/13/2023 2:37:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER          procedure [dbo].[GetPurchaseDetailedReport]  --exec GetPurchaseDetailedReport '2023-02-01', '2023-02-28',24   
(          
@fromDate Date=null,          
@toDate Date=null,    
@tenantId int=null ,  
@code nvarchar(max)=null  
)          
as begin          
  
declare @sql nvarchar(max)  
--exec GetSalesDetailedReportissuedate @fromDate ,@toDate ,@tenantId   
  --select top 20 * from logs order by id desc
set @sql = (select SPname from ReportCode where Code = @code)  
  
--insert into logs(json,date,batchid) values('Code'+@code,getdate(),@tenantid)  
--insert into logs(json,date,batchid) values('SQL'+@sql,getdate(),@tenantid)  
  
if @code is null or @sql is null or len(@sql)=0  
begin  
  select           
  format(IssueDate,'dd-MM-yyyy')           
   as  InvoiceDate,          
  InvoiceNumber          
  as InvoiceNumber,          
          
  sum(case when (trim(VatCategoryCode)='S'           
  and trim(BuyerCountryCode) like 'SA%') Then isnull(LineNetAmount,0) else 0 end )        
   as TaxableAmount,
          
  isnull(sum(case when (VatCategoryCode='Z'           
  ) Then isnull(LineNetAmount ,0) else 0 end ),0)          
  --and BuyerCountryCode ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0)          
   as ZeroRated,          
          
  isnull(sum(case when ( VatCategoryCode='S' and VATDeffered=1) Then isnull(LineNetAmount ,0)           
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
  vatrate           
   as vatrate,          
          
  isnull(sum(case when (VatCategoryCode='E'           
  and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0)           
   as Exempt,      
   isnull(sum(case when (VatCategoryCode='O'           
  and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as OutofScope,   
          
  isnull(sum(case when RCMApplicable=1 then 0
   when VATDeffered=1 then 0  else isnull(VATLineAmount,0) end),0)            
   as  VatAmount,          
          
  sum(LineAmountInclusiveVAT)           
   as  TotalAmount          
          
  from VI_importstandardfiles_Processed with (nolock)   
  where TenantId=@tenantId and CAST(IssueDate AS DATE)>=@fromDate and CAST(IssueDate AS DATE)<=@toDate   
  and invoicetype like 'Purchase Entry%'          
  group by IssueDate,PurchaseCategory,RCMApplicable,VATDeffered,InvoiceNumber ,VatRate   
  order by issuedate,InvoiceNumber 
end   
 else  
begin  
  exec @sql @fromDate,@toDate,@tenantId,@code  
end  
  
 end


 --select LineNetAmount,VatCategoryCode,BuyerCountryCode from VI_importstandardfiles_Processed where batchid=926 and id=87250
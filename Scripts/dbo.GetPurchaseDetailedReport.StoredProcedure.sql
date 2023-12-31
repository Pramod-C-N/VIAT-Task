USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetPurchaseDetailedReport]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	Create         procedure [dbo].[GetPurchaseDetailedReport]  --exec GetPurchaseDetailedReport '2023-02-01', '2023-02-28',33,'VATPUR000'   
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
	            
	  InvoiceNumber          
	  as InvoiceNumber,          
            BuyerName as VendorName,
			format(IssueDate,'dd-MM-yyyy')           
	   as  InvoiceDate,
	     Purchasecategory           
	   as PurchaseCategory,

	  sum(case when (trim(VatCategoryCode)='S'           
	  and trim(BuyerCountryCode) like 'SA%') Then isnull(LineNetAmount,0) else 0 end )        
	   as TaxableAmount,

	    vatrate           
	   as vatrate,
	     isnull(sum(case when RCMApplicable=1 then 0
	   when VATDeffered=1 then 0  else isnull(VATLineAmount,0) end),0)            
	   as  VatAmount,          
          
	  sum(LineAmountInclusiveVAT)           
	   as  TotalAmount ,
          
	  isnull(sum(case when (VatCategoryCode='Z'           
	  ) Then isnull(LineNetAmount ,0) else 0 end ),0)          
	  --and BuyerCountryCode ='SA') Then isnull(LineNetAmount ,0) else 0 end ),0)          
	   as ZeroRated,
	      
	  isnull(sum(case when (VatCategoryCode='E'           
	  and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0)           
	   as Exempt,      
	   isnull(sum(case when (VatCategoryCode='O'           
	  and BuyerCountryCode like 'SA%') Then isnull(LineNetAmount ,0) else 0 end ),0) as OutofScope ,
	  isnull(sum(case when ( VatCategoryCode='S'  and RCMApplicable='0' and VATDeffered='0' and BuyerCountryCode not like 'SA%')             
Then isnull(LineNetAmount ,0) else 0 end ),0) as ImportVATCustoms,            
isnull(sum(case when ( VatCategoryCode='S'  and RCMApplicable='0' and VATDeffered='1' and BuyerCountryCode not like 'SA%')             
Then isnull(LineNetAmount ,0) else 0 end ),0) as VATDeffered,            
isnull(sum(case when ( VatCategoryCode='S'  and RCMApplicable='1') Then isnull(LineNetAmount ,0) else 0 end ),0) as ImportsatRCM,

	  sum(customspaid)           
	   as CustomsPaid,          
          
	  sum(excisetaxpaid)           
	   as ExciseTaxPaid,          
	  sum(OtherChargespaid)          
	   as OtherChargesPaid  ,  
	   sum((isnull(VATLineAmount ,0))+(isnull(CustomsPaid ,0))+(isnull(ExciseTaxPaid ,0))+(isnull(OtherChargesPaid ,0))) as ChargesIncludingVAT
	          
	           
        
          
	         
          
	  from VI_importstandardfiles_Processed   
	  where TenantId=@tenantId and CAST(IssueDate AS DATE)>=@fromDate and CAST(IssueDate AS DATE)<=@toDate   
	  and invoicetype like 'Purchase Entry%'          
	  group by IssueDate,PurchaseCategory,RCMApplicable,VATDeffered,InvoiceNumber ,VatRate,BuyerName   
	  order by issuedate,InvoiceNumber 
	end   
	 else  
	begin  
	  exec @sql @fromDate,@toDate,@tenantId,@code  
	end  
  
	 end


	 --select LineNetAmount,VatCategoryCode,BuyerCountryCode from VI_importstandardfiles_Processed where batchid=926 and id=87250
GO

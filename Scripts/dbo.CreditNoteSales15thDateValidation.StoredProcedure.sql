USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteSales15thDateValidation]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create            procedure [dbo].[CreditNoteSales15thDateValidation]           --exec CreditNoteSales15thDateValidation 31,'2022-09-01','2022-09-30'      
(                
@batchno numeric,  
@fmdate date,  
@todate date  ,
@validstat int
  
)                
as                
begin                
  
Declare @VATReturnFillingFrequency varchar  
Declare @filingFrequency numeric  
  
--set @VATReturnFillingFrequency = (Select VATReturnFillingFrequency from TenantBasicDetails where tenantid = (select tenantid from batchdata where batchid = @batchno))  
  
--set @filingFrequency  = case when @VATReturnFillingFrequency=null or trim(upper(@VATReturnFillingFrequency)) = 'MONTHLY' then 1 else 3 end    
  
update VI_importstandardfiles_Processed set effdate = OrignalSupplyDate where datediff(DAY,IssueDate,@fmdate) < 15 and issuedate > OrignalSupplyDate    
and OrignalSupplyDate  < @fmdate and BatchId = @batchno and InvoiceType like 'Credit%'  
  
end
GO

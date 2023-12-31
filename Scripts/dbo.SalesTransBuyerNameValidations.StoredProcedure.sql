USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SalesTransBuyerNameValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     procedure [dbo].[SalesTransBuyerNameValidations]  -- exec SalesTransBuyerNameValidations 657237    
(    
@BatchNo numeric,    
@validstat int    
)    
as    
    
  set nocount on   
begin    
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 6    
end    
begin    
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)     
select tenantid,@batchno,uniqueidentifier,'0','Buyer Name cannot be blank',6,0,getdate() from ImportBatchData  with(nolock)   
where invoicetype like 'Sales Invoice%' and (BuyerName is  null or len(BuyerName)= 0) and InvoiceType<> 'Simplified' and batchid = @batchno      
end
GO

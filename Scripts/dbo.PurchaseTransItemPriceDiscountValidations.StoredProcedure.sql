USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[PurchaseTransItemPriceDiscountValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create    procedure [dbo].[PurchaseTransItemPriceDiscountValidations]    -- exec PurchaseTransItemPriceDiscountValidations  657237    
(    
@BatchNo numeric,
@validstat int
)    
as    
begin    
    
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 72    
end    
    
begin    
    
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)     
select tenantid,@batchno,uniqueidentifier,'0','Price Discount should >= 0 and < 100',72,0,getdate() from ImportBatchData     
where invoicetype like 'Purchase%' and (discount < 0 or Discount >= 100) and batchid = @batchno     
    
    
    
    
end
GO

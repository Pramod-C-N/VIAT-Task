USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteTransItemPriceDiscountValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE       procedure [dbo].[CreditNoteTransItemPriceDiscountValidations]    -- exec CreditNoteTransItemPriceDiscountValidations  657237          
(          
@BatchNo numeric,  
@validstat int   
)          
as     
 
begin          
          
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 45          
end          
          
begin          
          
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)           
select tenantid,@batchno,uniqueidentifier,'0','Price Discount should >= 0 and < 100',45,0,getdate() from ImportBatchData  with(nolock)         
where invoicetype like 'Credit%' and (discount < 0 or Discount >= 100) and batchid = @batchno           
          
          
          
          
end
GO

USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNoteTransItemPriceDiscountValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create        procedure [dbo].[DebitNoteTransItemPriceDiscountValidations]    -- exec DebitNoteTransItemPriceDiscountValidations  657237              
(              
@BatchNo numeric ,
@validstat int
)              
as       
set nocount on   
begin              
              
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 211              
end              
              
begin              
              
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)               
select tenantid,@batchno,uniqueidentifier,'0','Price Discount should >= 0 and < 100',211,0,getdate() from ImportBatchData with(nolock)              
where invoicetype like 'Debit Note%' and (discount < 0 or Discount >= 100) and batchid = @batchno               
              
              
              
              
end
GO

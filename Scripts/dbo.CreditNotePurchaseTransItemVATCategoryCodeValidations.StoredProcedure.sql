USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNotePurchaseTransItemVATCategoryCodeValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE          procedure [dbo].[CreditNotePurchaseTransItemVATCategoryCodeValidations]  -- exec CreditNotePurchaseTransItemVATCategoryCodeValidations 657237              
(              
@BatchNo numeric,    
@validstat int    
)              
as      
set nocount on    
begin              
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 170       
end              
begin              
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) select tenantid,@batchno,              
uniqueidentifier,'0','Invalid VAT Category Code',170,0,getdate() from ImportBatchData with(nolock)              
where invoicetype like 'CN Purchase%' and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) not like 'IMPORT%' and VatCategoryCode not in               
(select code from  taxcategory)              
and batchid = @batchno           
end
GO

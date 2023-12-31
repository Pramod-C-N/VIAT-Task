USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteTransInvoicedItemVATCategoryCodeValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE         procedure [dbo].[CreditNoteTransInvoicedItemVATCategoryCodeValidations]  -- exec CreditNoteTransInvoicedItemVATCategoryCodeValidations 657237        
(        
@BatchNo numeric,  
@validstat int  
)        
as    

begin        
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (48,133)        
end        
begin        
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) select tenantid,@batchno,        
uniqueidentifier,'0','Invalid VAT Category Code',48,0,getdate() from ImportBatchData  with(nolock)       
where invoicetype like 'Credit%'  and transtype = 'Sales' and upper(trim(InvoiceType)) not like '%EXPORT' and VatCategoryCode not in         
(select code from  taxcategory with(nolock))        
and batchid = @batchno          
end        
        
begin        
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)       
select tenantid,@batchno,        
uniqueidentifier,'0','For Nominal Credit Note  VAT Category Code not matching',133,0,getdate() from ImportBatchData with(nolock)        
where invoicetype like 'Credit%' and transtype = 'Sales' and upper(trim(InvoiceType)) like '%NOMINAL' and         
Vatcategorycode <> 'O'         
and batchid = @batchno          
        
end
GO

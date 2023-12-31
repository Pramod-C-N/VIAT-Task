USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SalesTransInvoiceCurrencyValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      procedure [dbo].[SalesTransInvoiceCurrencyValidations]  -- exec SalesTransInvoiceCurrencyValidations 2    
(    
@BatchNo numeric,
@validstat int
)    
as    
    
    
begin    
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (5,130)
end    
begin    
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)   
select tenantid,@batchno,    
uniqueidentifier,'0','Invalid  Currency Code',5,0,getdate() from ImportBatchData     
where invoicetype like 'Sales%' and upper(trim(InvoiceType)) not like '%EXPORT' and InvoiceCurrencyCode not in  
(select NationalCurrency from  CurrencyMapping)    
and batchid = @batchno      
end    
    
begin    
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)  
select tenantid,@batchno,    
uniqueidentifier,'0','Currency Code does not exists in ActiveCurrency Master',130,0,getdate() from ImportBatchData     
where invoicetype like 'Sales%' and upper(trim(InvoiceType)) like '%EXPORT' and InvoiceCurrencyCode   
not in (select InvoiceCurrency from  CurrencyMapping)     
and batchid = @batchno      
    
end
GO

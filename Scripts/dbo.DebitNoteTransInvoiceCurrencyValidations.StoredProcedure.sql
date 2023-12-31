USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNoteTransInvoiceCurrencyValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE          procedure [dbo].[DebitNoteTransInvoiceCurrencyValidations]  -- exec DebitNoteTransInvoiceCurrencyValidations 657237                  
(                  
@BatchNo numeric  ,    
@validstat int    
)                  
as                  
begin                  
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (150,151)                  
end                  
begin                  
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                 
select tenantid,@batchno,  uniqueidentifier,'0','Invalid  Currency Code',150,0,getdate()                 
from importbatchdata                   
where invoicetype like 'Debit Note%' and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) not like 'EXPORT%' and InvoiceCurrencyCode not in (select NationalCurrency from  CurrencyMapping)                  
and batchid = @batchno                    
end                  
begin                  
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) select tenantid,@batchno,                  
uniqueidentifier,'0','Currency Code does not exists in ActiveCurrency Master',151,0,getdate() from importbatchdata                   
where invoicetype like 'Debit Note%' and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) like 'EXPORT%' and InvoiceCurrencyCode not in (select AlphabeticCode from  Activecurrency)                   
and batchid = @batchno                    
end
GO

USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[debitNoteTransInvoicedItemVATRateValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create          procedure [dbo].[debitNoteTransInvoicedItemVATRateValidations]  -- exec debitNoteTransInvoicedItemVATRateValidations 819          
(          
@BatchNo numeric  ,
@validstat int
)          
as          
begin          
 delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (222)          
 end          
 begin          
 insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)         
 select tenantid,@batchno,          
 uniqueidentifier,'0','Invalid VAT Rate',222,0,getdate() from ImportBatchData           
 where invoicetype like 'Debit Note%' and transtype = 'Sales' and upper(trim(InvoiceType)) not like '%EXPORT' and           
 upper(trim(InvoiceType)) not like '%NOMINAL' and cast(VatCategoryCode as nvarchar)+cast(vatrate as nvarchar) not in           
 (select cast(taxcode as nvarchar)+cast(convert(decimal(5,2),Rate) as nvarchar) from  taxmaster)          
 and batchid = @batchno            
 end
GO

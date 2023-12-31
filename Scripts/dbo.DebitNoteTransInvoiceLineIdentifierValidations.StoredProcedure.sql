USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNoteTransInvoiceLineIdentifierValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create       procedure [dbo].[DebitNoteTransInvoiceLineIdentifierValidations]  -- exec DebitNoteTransInvoiceLineIdentifierValidations 155123            
(            
@BatchNo numeric ,
@validstat int          
)            
as            
begin            
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 207            
end            
  if @validstat=1          
begin            
            
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)             
select tenantid,@batchno,uniqueidentifier,'0','Duplicate Invoice Line Item',207,0,getdate() from ImportBatchData             
where invoicetype like 'Debit Note%' and cast(InvoiceNumber as nvarchar(15))+ cast(InvoiceLineIdentifier as nvarchar(3)) in            
(select cast(InvoiceNumber as nvarchar(15))+ cast(InvoiceLineIdentifier as nvarchar(3))  from ImportBatchData where batchid = @batchno             
group by cast(InvoiceNumber as nvarchar(15))+ cast(InvoiceLineIdentifier as nvarchar(3)) having count(*) > 1) and batchid = @batchno              
            
end
GO

USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteTransInvoicedQuantityUnitOfMeasureValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE         procedure [dbo].[CreditNoteTransInvoicedQuantityUnitOfMeasureValidations]-- exec CreditNoteTransInvoicedQuantityUnitOfMeasureValidations                
(                
@BatchNo numeric,      
@validstat int       
)                
as      
    
begin                
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 43                
end                
                
begin                
                
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                 
select tenantid,@batchno,uniqueidentifier,'0','Unit of Measurement not defined',43,0,getdate() from ImportBatchData  with(nolock)               
where  invoicetype like 'Credit%' and (UOM is   null and  UOM = '' and  UOM not in (select code from UnitOfMeasurement with(nolock)))and batchid = @batchno    
                
                
end
GO

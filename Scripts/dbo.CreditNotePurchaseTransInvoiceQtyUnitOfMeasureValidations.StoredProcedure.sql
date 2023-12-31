USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNotePurchaseTransInvoiceQtyUnitOfMeasureValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      procedure [dbo].[CreditNotePurchaseTransInvoiceQtyUnitOfMeasureValidations]-- exec CreditNotePurchaseTransInvoiceQtyUnitOfMeasureValidations          
(          
@BatchNo numeric,
@validstat int
)          
as  
set nocount on
begin          
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 165   
end          
begin          
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)           
select tenantid,@batchno,uniqueidentifier,'0','Unit of Measurement not defined',165,0,getdate() from importbatchdata   with(nolock)        
where  invoicetype like 'CN Purchase%' and (UOM is not null and UOM = '') and UOM not in     
(select code from unitofmeasurement) and batchid = @batchno            
end
GO

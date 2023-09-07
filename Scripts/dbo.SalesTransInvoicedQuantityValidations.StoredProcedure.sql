USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SalesTransInvoicedQuantityValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   procedure [dbo].[SalesTransInvoicedQuantityValidations]  -- exec SalesTransInvoicedQuantityValidations 657237
(
@BatchNo numeric,
@validstat int 
)
as

begin

delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 14
end

begin

insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) 
select tenantid,@batchno,uniqueidentifier,'0','Quantity Should be greater than Zero',14,0,getdate() from ImportBatchData 
where invoicetype like 'Sales Invoice%' and Quantity <=0 and batchid = @batchno 




end
GO

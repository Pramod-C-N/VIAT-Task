USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNotePurchaseTransInvoiceQtyUnitOfMeasureValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    
create   procedure [dbo].[DebitNotePurchaseTransInvoiceQtyUnitOfMeasureValidations]-- exec DebitNotePurchaseTransInvoiceQtyUnitOfMeasureValidations     
(               
@BatchNo numeric ,  
@validStat int  
)               
as               
begin  
  
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 241        

if @validStat=1   
begin               
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                
select tenantid,@batchno,uniqueidentifier,'0','Unit of Measurement not defined',241,0,getdate() from importbatchdata                
where  invoicetype like 'DN Purchase%' and (UOM is not null and UOM = '') and UOM not in          
(select code from unitofmeasurement) and batchid = @batchno                 
end           
end
GO

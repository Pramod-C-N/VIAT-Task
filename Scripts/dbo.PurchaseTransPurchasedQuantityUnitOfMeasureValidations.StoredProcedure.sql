USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[PurchaseTransPurchasedQuantityUnitOfMeasureValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create       procedure [dbo].[PurchaseTransPurchasedQuantityUnitOfMeasureValidations]-- exec PurchaseTransPurchasedQuantityUnitOfMeasureValidations 1239,1       
(        
@BatchNo numeric ,  
@validstat int  
)        
as        
begin   
  
 
begin  
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 70        
end        
  
if @validstat=1       
begin        
        
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)         
select tenantid,@batchno,uniqueidentifier,'0','Unit of Measurement not defined',70,0,getdate() from ImportBatchData         
where  invoicetype like 'Purchase%' and (UOM is not null and len(trim(UOM)) = 0) and batchid = @batchno          
and UOM not in (select code from unitofmeasurement)    
        
        
end        
end 

--select uom,* from ImportBatchData         
--where  invoicetype like 'Purchase%' and (UOM is not null and len(trim(UOM)) = 0) and batchid = 1239          
--and UOM not in (select code from unitofmeasurement) 
GO

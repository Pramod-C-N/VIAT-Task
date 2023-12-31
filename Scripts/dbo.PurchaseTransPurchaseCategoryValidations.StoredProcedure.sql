USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[PurchaseTransPurchaseCategoryValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE        procedure [dbo].[PurchaseTransPurchaseCategoryValidations]  -- exec PurchaseTransPurchaseCategoryValidations 859256            
(            
@BatchNo numeric,  
@validStat int  
)            
as    
set nocount on   
begin            
            
 -- select * from BatchData     
          
begin            
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 55            
end            
begin            
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)             
select tenantid,@batchno,uniqueidentifier,'0','Invalid Purchase Category',55,0,getdate() from ImportBatchData with(nolock)            
where Invoicetype like 'Purchase%' and batchid = @batchno             
and (PurchaseCategory is null or len(PurchaseCategory )=0) --or (upper(PurchaseCategory) not in (select upper(Name)  from transactioncategory))               
end            
            
end
GO

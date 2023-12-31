USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[PurchaseTransApportionmentValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     procedure [dbo].[PurchaseTransApportionmentValidations]  -- exec PurchaseTransApportionmentValidations 859256    
(    
@BatchNo numeric,
@validStat int
)    
as    
begin    
    
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 57    
    
--update ImportBatchData set isapportionment ='0' where invoicetype like 'Purchase%'     
--and upper(purchasecategory) like  'OVERHEAD%' and batchid = @batchno     
    
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)     
select tenantid,@batchno,uniqueidentifier,'0','Invalid Apportionment Status',57,0,getdate() from ImportBatchData     
where Invoicetype like 'Purchase%' and upper(purchasecategory) like  'OVERHEAD%' and upper(isapportionment) not in (0,1) and  batchid = @batchno     
    
end
GO

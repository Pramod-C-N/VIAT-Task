USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNotePurchaseTransItemNameValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
create      procedure [dbo].[DebitNotePurchaseTransItemNameValidations]-- exec DebitNotePurchaseTransItemNameValidations 657237             
(           
@BatchNo numeric,
@validStat int  
)            
as             
begin           
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 244           
end           
  
begin           
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)            
select tenantid,@batchno,uniqueidentifier,'0','Item Name Cannot be blank',244,0,getdate() from importbatchdata            
where invoicetype like 'DN Purchase%' and (Itemname is null or itemname = '') and batchid = @batchno            
end
GO

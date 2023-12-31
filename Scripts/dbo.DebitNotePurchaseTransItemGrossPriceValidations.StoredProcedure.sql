USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNotePurchaseTransItemGrossPriceValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create       procedure [dbo].[DebitNotePurchaseTransItemGrossPriceValidations]-- exec DebitNotePurchaseTransItemGrossPriceValidations 657237           
(           
@BatchNo numeric ,
@validStat int
)           
as           
begin           
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 243    
end           
begin           
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)            
select tenantid,@batchno,uniqueidentifier,'0','Gross Price should be greater than Zero',243,0,getdate() from importbatchdata            
where invoicetype like 'DN Purchase%' and Grossprice <=0 and batchid = @batchno            
end
GO

USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNotePurchaseTransItemNameValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      procedure [dbo].[CreditNotePurchaseTransItemNameValidations]-- exec CreditNotePurchaseTransItemNameValidations 657237          
(          
@BatchNo numeric  ,
@validstat int
)          
as 
set nocount on
begin          
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 164          
end          
begin          
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)           
select tenantid,@batchno,uniqueidentifier,'0','Item Name Cannot be blank',164,0,getdate() from importbatchdata with(nolock)          
where invoicetype like 'CN Purchase%' and (Itemname is null or itemname = '') and batchid = @batchno           
end
GO

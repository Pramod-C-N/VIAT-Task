USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNotePurchaseTransItemNetPriceValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      procedure [dbo].[CreditNotePurchaseTransItemNetPriceValidations]-- exec CreditNotePurchaseTransItemNetPriceValidations 657237          
(          
@BatchNo numeric,
@validstat int
)          
as   
set nocount on
begin          
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 168          
end          
begin          
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)           
select tenantid,@batchno,uniqueidentifier,'0','Invalid Net Price',168,0,getdate() from importbatchdata  with(nolock)         
where invoicetype like 'CN Purchase%' and ((discount > 0 and round(Grossprice - (GrossPrice * Discount /100),2) <> round(NetPrice,2)) or           
(discount =0 and GrossPrice<>netprice)) and batchid = @batchno           
end
GO

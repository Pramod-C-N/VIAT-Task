USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNotePurchaseDNTypetoCountryValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE        procedure [dbo].[DebitNotePurchaseDNTypetoCountryValidations]  -- exec DebitNotePurchaseDNTypetoCountryValidations 657237             
(             
@BatchNo numeric ,  
@valiStat int  
)             
    
as        -- select * from ImportBatchData where batchid = 168    
    begin             
    delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 179             
end             
begin             
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) select tenantid,@batchno,             
uniqueidentifier,'0','Country Code mismatch with Debit Note Purchase Type',179,0,getdate() from ImportBatchData              
where invoicetype like 'DN Purchase%' and upper(InvoiceType) like '%IMPORT%' and upper(BuyerCountryCode) like 'SA%'
and batchid = @batchno               
    
end
GO

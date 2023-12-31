USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteTransLineNetAmountValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE         procedure [dbo].[CreditNoteTransLineNetAmountValidations]  -- exec CreditNoteTransLineNetAmountValidations 657237        
(        
@BatchNo numeric,    
@validstat int     
)        
as     
    
begin        
        
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (181,190)        
end        
        
begin        
        
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)         
select tenantid,@batchno,uniqueidentifier,'0','Incorrect Line Net Amount',181,0,getdate() from ImportBatchData  with(nolock)       
where invoicetype like 'Credit Note%' and transtype = 'Sales' and LineNetAmount <> NetPrice * Quantity and batchid = @batchno         
        
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)         
select tenantid,@batchno,uniqueidentifier,'0','Taxable Value should be less than 1000SAR for Simplified Invoice',190,0,getdate() from ImportBatchData with(nolock)        
where invoicetype like 'Credit Note%' and transtype = 'Sales' and LineAmountInclusiveVAT > 1000         
and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) = 'Simplified'        
and batchid = @batchno         
        
end
GO

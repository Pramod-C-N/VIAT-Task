USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteTransCreditNoteReasonValidation]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     procedure [dbo].[CreditNoteTransCreditNoteReasonValidation] -- exec CreditNoteTransCreditNoteReasonValidation 657237      
(      
@BatchNo numeric,  
@validstat int   
)      
as      
 
begin      
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 38      
end      
begin      
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)       
select tenantid,@batchno,uniqueidentifier,'0','Credit Note reason cannot be blank',38,0,getdate() from ImportBatchData  with(nolock)     
where invoicetype like 'Credit Note%' and transtype = 'Sales' and (ReasonForCN is  null or ReasonForCN= '')   
and batchid = @batchno        
end
GO

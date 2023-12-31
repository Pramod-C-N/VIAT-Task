USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteTransBillingReferenceIDValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE      procedure [dbo].[CreditNoteTransBillingReferenceIDValidations]  -- exec CreditNoteTransBillingReferenceIDValidations 657237      
(      
@BatchNo numeric,  
@validstat int  
)      
as      
   
      
begin      
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 36     
end      
begin      
if @validstat = 1
begin
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)       
select tenantid,@batchno,uniqueidentifier,'0','Billing Reference ID cannot be blank',36,0,getdate() from ImportBatchData  with(nolock)     
where invoicetype like 'Credit Note%' and transtype like 'Sales%' and (BillingReferenceId is  null or BillingReferenceId= '')     
and batchid = @batchno        
end
end
GO

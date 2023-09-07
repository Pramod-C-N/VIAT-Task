USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNoteTransVATExemptionReasonValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE       procedure [dbo].[DebitNoteTransVATExemptionReasonValidations]   -- exec DebitNoteTransVATExemptionReasonValidations  2        
(        
@BatchNo numeric ,
@validstat int
)        
as 
set nocount on 
begin        
 delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in(225 ,81)       
 end        
 begin        
 insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)   
 select tenantid,@batchno,        
 uniqueidentifier,'0','VAT Exemption Reason cannot be blank',225,0,getdate() from ImportBatchData  with(nolock)       
 where invoicetype like 'Debit Note%' and upper(trim(vatcategorycode)) in ('E','Z','O') and VatExemptionReasonCode in         
 (select code  from  exemptionreason ) and (VatExemptionReason is null or VatExemptionReason ='')        
 and batchid = @batchno  
 
   if @validstat=1      
       
 insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) select tenantid,@batchno,          
 uniqueidentifier,'0','VAT Exemption Reason not as per Master Definitions',81,0,getdate() from ImportBatchData  with(nolock)         
 where invoicetype like 'Debit Note%' and upper(trim(vatcategorycode)) in ('E','Z') and VatExemptionReasonCode in           
 (select code  from  exemptionreason ) and (VatExemptionReason is not null and VatExemptionReason <>'')          
 and batchid = @batchno
 end
GO

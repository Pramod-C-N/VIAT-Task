USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNodeProcedureTransVATExemptionReasonValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      procedure [dbo].[CreditNodeProcedureTransVATExemptionReasonValidations]   -- exec CreditNodeProcedureTransVATExemptionReasonValidations 657237          
(          
@BatchNo numeric ,
@validstat int
)          
as 
SET NOCOUNT ON
begin          
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 173          
end          
begin          
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) select tenantid,@batchno,          
uniqueidentifier,'0','VAT Exemption reason is required',173,0,getdate() from ImportBatchData  WITH(NOLOCK)         
where invoicetype like 'Credit%' and trim(substring(InvoiceType,16,len(InvoiceType)-15)) in ('E','Z') and VatExemptionReasonCode in           
(select code  from  ExemptionReason ) and (VatExemptionReason is null or VatExemptionReason ='')          
and batchid = @batchno            
end
GO

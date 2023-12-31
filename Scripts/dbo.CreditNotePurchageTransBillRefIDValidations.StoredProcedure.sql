USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNotePurchageTransBillRefIDValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      procedure [dbo].[CreditNotePurchageTransBillRefIDValidations]  -- exec CreditNotePurchageTransBillRefIDValidations 657237                    
(                    
@BatchNo numeric ,          
@validstat int,    
@tenantid int    
)                    
as            
SET NOCOUNT ON          
begin                    
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 161                   
end                    
begin      
if @validstat = 1 
begin
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                     
select tenantid,@batchno,uniqueidentifier,'0','Billing reference Id does not exists in Purchase Table ',161,0,getdate() from importbatchdata WITH (NOLOCK)                    
where invoicetype like 'CN Purchase%' and  concat(BillingReferenceId,cast(OrignalSupplyDate as nvarchar)) not  in                
  (select concat(InvoiceNumber,cast(SupplyDate as nvarchar)) from VI_importstandardfiles_Processed WITH (NOLOCK) where invoicetype like 'Purchase%' and TenantId=@tenantid)                
-- and InvoiceType<> 'Simplified'                   
and batchid = @batchno                      
end
end
GO

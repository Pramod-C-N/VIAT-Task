USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteTransBillingReferenceIDRule01Validations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE     procedure [dbo].[CreditNoteTransBillingReferenceIDRule01Validations]  -- exec CreditNoteTransBillingReferenceIDRule01Validations 441                      
(                      
@BatchNo numeric,            
@validstat int,    
@tenantid int    
)                      
as                      
 set nocount on                     
                      
begin                      
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in(124 ,355)                    
end      
    
--if @validstat=1    
begin                      
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                       
            
select tenantid,@batchno,uniqueidentifier,'0','reference Invoice Number cannot be checked. Do you want to override?',124,0,getdate()     
from ImportBatchData  with(nolock)                     
--where invoicetype like 'Credit Note%' and transtype = 'Sales' and  concat(BillingReferenceId,cast(OrignalSupplyDate as nvarchar)) not in                  
--  (select concat(InvoiceNumber,cast(issuedate as nvarchar)) from VI_importstandardfiles_Processed)                  
where invoicetype like 'Credit Note%' and concat(BillingReferenceId,cast(format(OrignalSupplyDate,'dd-MM-yyyy') as nvarchar)) not in                  
  (select concat(InvoiceNumber,cast(format(effdate,'dd-MM-yyyy') as nvarchar)) from VI_importstandardfiles_Processed  with(nolock)     
where invoicetype like 'Sales%'           
  and invoicenumber in (select InvoiceNumber from VI_importstandardfiles_Processed  with(nolock) where invoicetype like 'Sales%' and tenantid = @tenantid))            
and batchid = @batchno and tenantid = @tenantid           
            
--select BillingReferenceId,orignalsupplydate,concat(BillingReferenceId,cast(format(OrignalSupplyDate,'dd-MM-yyyy') as nvarchar)) from importbatchdata where batchid = 59          
          
--insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                       
--select tenantid,@batchno,uniqueidentifier,'0','Invalid Billing Reference ID',355,0,getdate() from ImportBatchData  with(nolock)                      
----where invoicetype like 'Credit Note%' and transtype = 'Sales' and  concat(BillingReferenceId,cast(OrignalSupplyDate as nvarchar)) not in                  
----  (select concat(InvoiceNumber,cast(issuedate as nvarchar)) from VI_importstandardfiles_Processed)                  
--where invoicetype like 'Credit Note%' and transtype = 'Sales' and  BillingReferenceId not in                  
--  (select InvoiceNumber from VI_importstandardfiles_Processed  with(nolock) where InvoiceType like 'Sales%')                  
--and batchid = @batchno                        
            
end             
            
--select * from vi_importstandardfiles_processed where invoicenumber = '2067'            
          
--delete from vi_importstandardfiles_processed where effdate is null  
GO

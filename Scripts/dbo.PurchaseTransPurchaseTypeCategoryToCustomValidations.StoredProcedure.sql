USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[PurchaseTransPurchaseTypeCategoryToCustomValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE               procedure [dbo].[PurchaseTransPurchaseTypeCategoryToCustomValidations]  -- exec PurchaseTransPurchaseTypeCategoryToCustomValidations 2                      
(                      
@BatchNo numeric,              
@validstat int,              
@tenantid int              
)                      
as                      
begin                      
                    
begin                      
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in ( 527 ,528)                    
end              
                   
            
begin                       
                      
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                       
select tenantid,@batchno,uniqueidentifier,'0','Custom Paid is not required for the Purchase Type ' + upper(trim(substring(InvoiceType,16,len(InvoiceType)-15))) + ' and Purchase Category ' + upper(trim(PurchaseCategory)),527,0,getdate()     
from ImportBatchData                       
where InvoiceType  like 'Purchase%' and trim(substring(InvoiceType,16,len(InvoiceType)-15)) like 'Standard%' and upper(trim(PurchaseCategory)) like 'SERVICE%' and (CustomsPaid <> 0.00)          
and batchid = @batchno                        
end             
        
begin                       
                      
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                       
select tenantid,@batchno,uniqueidentifier,'0','Custom Paid or VAT Deffered required for the Purchase Type ' + upper(trim(substring(InvoiceType,16,len(InvoiceType)-15))) + ' and Purchase Category ' + upper(trim(PurchaseCategory)),528,0,getdate() from      
 ImportBatchData                       
where InvoiceType  like 'Purchase%' and trim(substring(InvoiceType,16,len(InvoiceType)-15)) like 'Import%' and upper(trim(PurchaseCategory)) like 'GOOD%'    
and (CustomsPaid =0.00 and len(VATdeffered) = 0 )          
and batchid = @batchno                        
end         
                      
end
GO

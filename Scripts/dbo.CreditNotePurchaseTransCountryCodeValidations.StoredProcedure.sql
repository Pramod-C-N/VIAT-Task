USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNotePurchaseTransCountryCodeValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create         procedure [dbo].[CreditNotePurchaseTransCountryCodeValidations]-- exec CreditNotePurchaseTransCountryCodeValidations 478             
(              
@BatchNo numeric,        
@validstat int        
)              
as              
begin         
        
       
begin        
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (483,484)  
end              
        
            
begin              
              
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)               
select tenantid,@batchno,uniqueidentifier,'0','Credit Note Purchase Transaction : Country Code mismatch with Purchase type',483,0,getdate() from ImportBatchData               
where  invoicetype like 'CN Purchase%' and trim(substring(InvoiceType,16,len(InvoiceType)-15)) like 'Import%' and BuyerCountryCode like 'SA%' and batchid = @batchno                
                  
end     
  
begin              
              
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)               
select tenantid,@batchno,uniqueidentifier,'0','Credit Note Purchase Transaction : Country Code mismatch with Credit Note  type',484,0,getdate() from ImportBatchData               
where  invoicetype like 'CN Purchase%' and trim(substring(InvoiceType,16,len(InvoiceType)-15)) like 'Standard%' and BuyerCountryCode not like 'SA%' and batchid = @batchno                
                  
end    
end
GO

USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[PurchaseTransInputVATClaimedValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create          procedure [dbo].[PurchaseTransInputVATClaimedValidations]-- exec PurchaseTransInputVATClaimedValidations 478             
(              
@BatchNo numeric,        
@validstat int        
)              
as              
begin         
        
       
begin        
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (538)  
end              
        
            
begin              
              
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)               
select tenantid,@batchno,uniqueidentifier,'0','If INPUT VAT CLAIMED is '+ upper(trim(AffiliationStatus)) +' then Reason for Input VAT not claimed is required ',538,0,getdate() from ImportBatchData               
where  invoicetype like 'Purchase%' 
and upper(trim(AffiliationStatus)) like 'N%' and (ReasonforCN is null or len(ReasonforCN)=0)
and batchid = @batchno                
                  
end     
  

end
GO

USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[PurchaseTransPurchaseCategorytoVATCategoryValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create     procedure [dbo].[PurchaseTransPurchaseCategorytoVATCategoryValidations]-- exec PurchaseTransPurchaseCategorytoVATCategoryValidations 478                   
(                    
@BatchNo numeric,              
@validstat int,  
@tenantid int  
)                    
as                    
begin               
              
             
begin              
delete from importstandardfiles_errorlists where batchid = @BatchNo and TenantId=@tenantid and errortype in (539)        
end                    
              
                  
begin                    
                    
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                     
select tenantid,@batchno,uniqueidentifier,'0','Purchase Category mismatch with Purchase VAT category in Vendor Master',539,0,getdate() from ImportBatchData                     
where  invoicetype like 'Purchase%'       
and upper(trim(BuyerName)) in (select upper(trim(name)) from VI_ImportMasterFiles_Processed where MasterType like 'Vendor%' and TenantId=@tenantid)    
and UPPER(trim(PurchaseCategory)) not in (select upper(trim(BusinessCategory)) from VI_ImportMasterFiles_Processed where MasterType like 'Vendor%' and TenantId=@tenantid)    
and batchid = @batchno and TenantId=@tenantid                      
                        
end           
        
      
end
GO

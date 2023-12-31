USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[PurchaseTransValidation]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--sp_helptext PurchaseTransValidation              
              
    
CREATE     procedure [dbo].[PurchaseTransValidation]  --  exec PurchaseTransValidation 2629              
(                  
@batchno numeric                  
)                  
as                  
Begin                  
               
declare @fmdate date, @todate date                
declare @tenantid int              
declare @validstat int              
                
begin              
select @fmdate = fromdate,@todate = toDate,@tenantid =TenantId from  batchdata with(nolock) where BatchId=@batchno                
             
              
set @validstat = (select ValidStat from ValidationStatus with(nolock) where tenantid=@tenantid)              
              
-------------------------------------------Validation SP Start-------------------------------------------------------              
              
exec PurchaseTransPurchaseCategoryValidations @batchno,@validstat                  
                  
exec PurchaseTransNatureofServicesValidations @batchno ,  @validstat               
                  
exec PurchaseTransApportionmentValidations @batchno,@validstat                  
                  
exec PurchaseTransSupplierInvoiceNumberValidations @batchno,@validstat ,@tenantid                 
              
exec PurchaseTranPurchaseDateValidations  @batchno,@fmdate,@todate,@validstat ,@tenantid                 
              
exec PurchaseTransInvoiceCurrencyCodeValidations @batchno ,@validstat       --------Master Validation Present              
              
exec PurchaseTransSupplierInvoiceDateValidations @batchno,@fmdate,@todate,@validstat ,@tenantid             
              
exec PurchaseTransSellerNameValidations @batchno,@validstat                
              
exec PurchaseTranSupplierVATNumberValidations  @batchno ,@validstat,@tenantid                 
                  
exec PurchaseTransInvoiceLineIdentifierValidations @batchno,@validstat                  
              
exec PurchaseTransItemrMasterCodeValidations @batchno ,@validstat        
    
    
                
exec PurchaseTransItemNameValidations @batchno,@validstat                
              
exec PurchaseTransPurchasedQuantityUnitOfMeasureValidations @batchno ,@validstat     -------Master Validation Present              
                
exec PurchaseTransItemGrossPriceValidations @batchno,@validstat                    
                
exec PurchaseTransItemPriceDiscountValidations @batchno ,@validstat               
              
exec PurchaseTransPurchaseTypeValidations @batchno,@validstat,@tenantid        ------Master validation present              
              
--exec PurchaseTransSupplierTypeValidations @batchno ,@validstat ,@tenantid          -------Master validation present              
                
exec PurchaseTransItemNetPriceValidations @batchno,@validstat                
                  
exec PurchaseTransDateValidationwithinTaxableRange @batchno,@fmdate,@todate,@validstat              
              
exec PurchaseTransPurchasedQuantityValidations @batchno              
            
exec PurchaseTransRule02Validations @batchno,@validstat            
            
exec PurchaseTransRule03Validations @batchno,@validstat           
          
exec PurchaseTransRule04Validations @batchno,@validstat          
          
exec PurchaseTransVATExemptionReasonCodeValidations @batchno,@validstat          
          
exec PurchaseTransPurchaseTypeCategoryToCustomValidations @batchno,@validstat,@tenantid          
          
exec PurchaseTransCountryCodeValidations @batchno,@validstat          
          
exec PurchaseTransInputVATClaimedValidations @batchno,@validstat          
        
exec PurchaseTransReasonInputVATClaimValidations @batchno,@validstat         
          
exec PurchaseTransPurchaseCategorytoVATCategoryValidations @batchno,@validstat,@tenantid          
          
exec PurchaseTransVATLineAmountValidations @batchno,@validstat          
        
exec PurchasetransPurchaseLineNetAmountValidations @batchno,@validstat        
      
exec PurchaseTransTotalTaxableAmountValidations @batchno,@validstat  


      
exec PurchaseTransVATDefferedValidation @batchno,@tenantid,@validstat      
      
exec PurchaseTransRCMApplicableValidations @batchno,@tenantid,@validstat      
      
exec PurchaseTransWHTApplicableValidations @batchno,@tenantid,@validstat     
    
exec PurchaseTransItemVATRateValidations @batchno,@tenantid,@validstat    
              
-------------------------------------------Validation SP End-------------------------------------------------------              
              
exec VI_insertProcessedImportStandardFilesPurchases @batchno,@tenantid                  
                  
              
end                 
end
GO

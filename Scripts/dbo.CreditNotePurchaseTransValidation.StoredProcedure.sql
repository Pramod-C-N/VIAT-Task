USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNotePurchaseTransValidation]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
                                   
CREATE       procedure [dbo].[CreditNotePurchaseTransValidation]  --  exec CreditNotePurchaseTransValidation 4141                                     
(                                        
@batchno numeric                                        
)                                        
as                                 
                               
Begin                                     
declare @fmdate date, @todate date                                      
declare @tenantid int                                    
declare @validStat int=0                                    
                                    
                                      
begin                                    
   set @validstat = (select validStat from ValidationStatus with(nolock) where tenantid=@tenantid)                      
select @fmdate=fromdate, @todate=todate, @tenantid=tenantid from batchdata with(nolock) where batchid=@batchno         
    
      
exec CreditNotePurchaseTransInvoiceTypeValidations @batchno,@validstat ,@tenantid                                  
exec CreditNotePurchaseTransNatureofServicesValidations @batchno,@validstat                                
exec CreditNotePurchaseTransCreditNoteNumberValidations @batchno,@validstat ,@tenantid                                        
exec CreditNotePurchaseInvoiceIssueDateValidations @batchno,@fmdate,@todate,@validstat,@tenantid                                      
exec CreditNotePurchaseTransInvoiceCurrencyValidations @batchno ,@validstat                                        
exec CreditNotePurchageTransBillRefIDValidations @batchno,@validstat,@tenantid                                  
--EXEC CreditNotePurchaseTransOriginalIssueDateValidations @batchno,@fmdate,@todate,@validstat,@tenantid                                 
--exec CreditNotePurchaseTransCreditNoteReasonValidation @batchno,@validstat                                
exec CreditNotePurchaseTransSupplierMasterCodeValidations @batchno,@validstat                                         
exec CreditNotePurchaseTransSupplierNameValidations @batchno,@validstat,@tenantid                               
exec CreditNotePurchaseTranSupplierVATNumberValidations @batchno,@validstat,@tenantid                            
--exec CreditNotePurchaseTransCountryCodeValidations @batchno,@validstat                                
--exec CreditNotePurchaseTransSupplierTypeValidations @batchno,@validstat,@tenantid                                 
exec CreditNotePurchaseTransLineIdentifierValidations @batchno  ,@validstat                                
exec CreditNotePurchaseTransItemNameValidations @batchno ,@validstat                                 
exec CreditNotePurchaseTransInvoiceQtyUnitOfMeasureValidations @batchno,@validstat                                         
exec CreditNotePurchaseTransItemGrossPriceValidations @batchno,@validstat                                         
exec CreditNotePurchaseTransItemPriceDiscountValidations @batchno,@validstat                                         
exec CreditNotePurchaseTransItemNetPriceValidations  @batchno   ,@validstat                                      
exec CreditNotePurchaseTransCreditQuantityValidations @batchno, @validstat,@tenantid                                   
exec CreditNotePurchaseTransItemVATCategoryCodeValidations @batchno  ,@validstat          
exec CreditNotePurchaseTransInvoiceItemVATRateValidations @batchno,@validstat                                          
exec CreditNotePurchaseTransVATExemptionReasonCodeValidations @batchno ,@validstat                                        
exec CreditNodeProcedureTransVATExemptionReasonValidations @batchno   ,@validstat                                      
exec CreditNotePurchaseTransVATLineAmountValidations @batchno   ,@validstat  

if @validstat = 1 
begin
	exec CreditNotePurchaseTransPurchaseCategoryToCNPurchaseValidations @batchno,@validStat,@tenantid                                
	exec CreditNotePurchaseTransRule01Validations @batchno,@validStat                           
    exec CreditNotePurchaseTransRule02Validations @batchno,@validStat,@tenantid                        
    exec CreditNotePurchaseTransRule04Validations @batchno,@validStat                                
    exec CreditNotePurchaseTransRule05Validations @batchno,@validStat                               
end

exec CreditNotePurchaseTransCreditNoteTypeToNetPriceValidations @batchno,@validStat     
exec CreditNotePurchaseTransRule03Validations @batchno,@validStat                                
exec CreditNotePurchasetransPurchaseLineNetAmountValidations @batchno,@validStat                             
exec CreditNotePurchaseTransTotalTaxableAmountValidations @batchno,@validStat                        
exec CreditNotePurchaseTransApportionmentValidations @batchno,@validStat                      
exec CreditNotePurchaseTransVATDefferedValidation @batchno,@validStat,@tenantid                     
exec CreditNotePurchaseTransRCMApplicableValidations @batchno,@validStat,@tenantid                   
exec CreditNotePurchaseTransWHTApplicableValidations @batchno,@validStat,@tenantid                
--exec CreditNotePurchaseTransPurchaseCategoryValidations @batchno,@validstat                                      
--exec CNTransCreditNoteReasonValidation @batchno                                        
--exec CNTransBillingReferenceIDValidations @batchno                                         
--exec CNTransBuyerVATNumberValidations @batchno on hold                                     
--exec CNTransBillingReferenceIDRule01Validations @batchno                                        
exec VI_insertProcessedImportStandardFilesCNPurchase @batchno,@tenantid                                         
end                                      
end
GO

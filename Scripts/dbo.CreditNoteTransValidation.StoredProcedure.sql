USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteTransValidation]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
       
CREATE    procedure [dbo].[CreditNoteTransValidation]  --  exec CreditNoteTransValidation 2566                            
(                                     
                               
@batchno numeric                                
)                                
as                                
                    
Begin                            
                          
declare @fmdate date, @todate date                           
declare @validStat int=0                          
declare @tenantid int                          
                          
                            
begin                          
set @fmdate = (select fromdate from  batchdata with (nolock) where BatchId=@batchno)                                
                            
set @todate = (select todate from  batchdata with (nolock) where BatchId=@batchno)                          
                          
set @tenantid = (select tenantid from batchdata with (nolock) where batchid = @batchno)    
  
set @validstat = (select validStat from ValidationStatus with (nolock) where tenantid=@tenantid)   
                      
--select @fmdate=fromdate, @todate=todate,@tenantid=tenantid from batchdata with (nolock) where batchid = @batchno                      
                          
--create table ValidationStatus (ValidStat int)                          
--insert into ValidationStatus values(1)                          
-- update validationstatus set validstat = 0   validations excluding masters                          
-- update validationstatus set validstat = 1   validations including masters                          
                            
exec CreditNoteTransInvoiceTypeValidations @batchno,@validStat,@tenantid                              
                            
exec CreditNoteTransCreditNoteNumberValidations @batchno,@validStat,@tenantid                              
                            
exec CreditNoteTransIssueDateValidations @batchno,@fmdate,@todate,@validStat,@tenantid                                
                            
exec CreditNoteTransCurrencyValidations @batchno,@validStat ,@tenantid                                
                            
--exec CreditNoteTransBillingReferenceIDValidations @batchno,@validstat                                 commented on 11/08/2023 for client data updation
                            
exec CreditNoteTransOriginalIssueDateValidations @batchno,@fmdate,@todate,@validStat ,@tenantid                            
                            
exec CreditNoteTransCreditNoteReasonValidation @batchno,@validStat                                 
                            
--exec CreditNoteTransBuyerMasterCodeValidations @batchno,@validStat                             
                            
exec CreditNoteTransBuyerNameValidations @batchno,@validStat ,@tenantid                             
                            
--exec CreditNoteTransBuyerVATNumberValidations @batchno,@validstat,@tenantid                             
                            
exec CreditNoteTransBuyerLocationsValidations @batchno,@validStat                            
                            
exec CreditNoteTransInvoiceLineIdentifierValidations @batchno,@validstat                             
                            
exec CreditNoteTransItemNameValidations @batchno,@validStat                                 
                            
exec CreditNoteTransInvoicedQuantityUnitOfMeasureValidations @batchno,@validstat                                 
                            
exec CreditNoteTransItemGrossPriceValidations @batchno,@validstat                                  
                            
exec CreditNoteTransItemPriceDiscountValidations @batchno,@validstat                                 
                            
exec CreditNoteTransItemNetPriceValidations  @batchno,@validstat       
                            
exec CreditNoteTransInvoicedQuantityValidations @batchno,@validstat,@tenantid                                 
                            
exec CreditNoteTransLineNetAmountValidations @batchno,@validstat                            
                            
exec CreditNoteTransInvoicedItemVATCategoryCodeValidations @batchno,@validstat                            
                            
----exec CreditNoteTransInvoicedItemVATRateValidations @batchno,@validstat                                 
                            
exec CreditNoteTransVATExemptionReasonCodeValidations @batchno,@validstat                    
                            
exec CreditNoteTransVATExemptionReasonValidations @batchno,@validstat                                  
                            
exec CreditNoteTransVATLineAmountValidations @batchno,@validstat,@tenantid                             
                            
exec CreditNoteTransBillingReferenceIDRule01Validations @batchno,@validstat,@tenantid                             
                            
exec CreditNotetranslineamountinclusivevatvalidations @batchno,@validstat                
                
exec CreditNoteTransBuyerTypeValidations @batchno,@validstat                     
                        
exec CreditNoteTransRule01Validations @batchno,@validstat                        
                            
exec CreditNoteTransRule02Validations @batchno,@validstat,@tenantid                            
                            
exec CreditNoteTransRule03Validations @batchno,@validstat                           
                          
--exec CreditNoteTransRule04Validations @batchno,@validstat                          
                          
exec CreditNoteTransRule05Validations @batchno,@validstat ,@tenantid                         
                        
----exec CreditNotePurchaseTransOriginalIssueDateValidations @batchno,@fmdate,@todate,validStat,@tenantid                        
                      
--exec CreditNoteSales15thDateValidation @batchno,@fmdate,@todate ,@validStat              
                    
                          
exec VI_insertProcessedImportStandardFilesCN @batchno,@tenantid                                 
                        
                        
end                          
end                           
                        
--select * from importbatchdata where invoicenumber = '2067'                        
                        
--delete from importbatchdata where issuedate is null and invoicetype like 'Sales%'                        
                        
--select * from sys.procedures order by modify_date desc
GO

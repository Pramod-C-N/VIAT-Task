USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SalesTransValidation]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
      
CREATE   procedure [dbo].[SalesTransValidation]  --  exec SalesTransValidation 3956              
(                  
@batchno numeric                  
)                  
as                  
Begin                  
         
declare @fmdate date, @todate date            
          
declare @validStat int=0          
          
declare @tenantid int          
            
begin          
        
--set @fmdate = (select fromdate from  batchdata where BatchId=@batchno)                
--            
--set @todate = (select todate from  batchdata where BatchId=@batchno)     
--set @tenantid = (select tenantid from batchdata where batchid = @batchno)          
      
select @fmdate=fromdate ,@todate = todate,@tenantid = tenantid from batchdata where BatchId=@batchno 

set @validstat = (select validStat from ValidationStatus where tenantid=@tenantid)  
      
--create table ValidationStatus (ValidStat int)          
--insert into ValidationStatus values(1)          
-- update validationstatus set validstat = 0   validations excluding masters          
-- update validationstatus set validstat = 1   validations including masters          
          
--exec ExecuteRules 1,@tenantid,@batchno          
          
exec SalesTransInvoiceTypeValidations @batchno,@validstat,@tenantid                  
              
exec SalesTransTransTypeValidations @batchno,@validstat                  
                  
exec SalesTransInvoiceNumberValidations @batchno,@validstat,@tenantid                   
                  
exec SalesTransInvoiceIssueDateValidations @batchno,@fmdate,@todate,@validstat,@tenantid                  
              
exec SalesTransSupplyDateValidations @batchno,@fmdate,@todate,@validstat,@tenantid               
                  
exec SalesTransInvoiceCurrencyValidations @batchno,@validstat                   
                  
--exec SalesTransBuyerMasterCodeValidations @batchno,@validstat                  
                  
exec SalesTransBuyerNameValidations @batchno,@validstat                  
                  
--exec SalesTransBuyerVatNumberValidations @batchno,@validstat,@tenantid               
            
exec SalesTransSameVATNumberforMultipleBuyerNameValidations @batchno,@validstat,@tenantid             
              
exec SalesTransBuyerLocationsValidations @batchno,@validstat              
                  
exec SalesTransInvoiceLineIdentifierValidations @batchno,@validstat                    
                  
exec SalesTransItemNameValidations @batchno,@validstat                   
                  
exec SalesTransInvoicedQuantityUnitOfMeasureValidations @batchno,@validstat                   
                  
exec SalesTransItemGrossPriceValidations @batchno,@validstat                  
              
exec SalesTransItemPriceDiscountValidations @batchno,@validstat                  
              
exec SalesTransItemNetPriceValidations @batchno,@validstat                  
              
exec SalesTransInvoicedQuantityValidations @batchno,@validStat                  
              
exec SalesTransInvoiceLineNetAmountValidations @batchno,@validStat                  
                  
exec SalesTransInvoicedItemVATCategoryCodeValidations @batchno,@validStat                  
                  
exec SalesTransInvoicedItemVATRateValidations @batchno,@validStat, @tenantid                    
                  
exec SalesTransVATExemptionReasonCodeValidations @batchno,@validStat                   
                  
exec SalesTransVATExemptionReasonValidations @batchno,@validstat                   

exec SalesTransNominalValueValidations @batchno,@validstat,@tenantid          
              
exec SalesTransVATLineAmountValidations @batchno,@validstat , @tenantid                 
              
exec SalesTransLineAmountInclusiveVATValidations @batchno,@validstat,@tenantid                  
              
exec SalesTransAdvanceReceiptValidations @batchno,@validstat                  
                
exec SalesTransADvanceVATAmtValidations @batchno,@validstat                  
                  
exec SaleTransPaymentMeansValidations @batchno,@validStat                  
                  
exec SaleTransPaymentTermsValidations @batchno,@validstat                  
                  
exec SalesTransBuyerTypeValidations @batchno,@validstat                  
                  
exec SalesTransRule01Validations @batchno,@validstat                  
                  
exec SalesTransRule02Validations @batchno,@validstat       
                  
exec SalesTransRule03Validations @batchno,@validstat                  
              
exec SalesTransRule04Validations @batchno,@validstat,@tenantid            
          
exec SalesTransRule05Validations @batchno,@validstat,@tenantid          
          
              
exec VI_insertProcessedImportStandardFiles @batchno,@tenantid                  
                  
end                
end         
        
        
        
--select * from importbatchdata where invoicetype like 'Sales%'
GO

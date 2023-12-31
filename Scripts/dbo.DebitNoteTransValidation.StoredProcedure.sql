USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNoteTransValidation]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--sp_helptext DebitNoteTransValidation            
            
              
              
CREATE       procedure [dbo].[DebitNoteTransValidation]  --  exec [DebitNoteTransValidation] 831              
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
              
--set @todate = (select todate from  batchdata where BatchId=@batchno)            
            
            
            
--set @tenantid = (select tenantid from batchdata where batchid = @batchno)            
            
select  @fmdate = fromdate,@todate=todate,@tenantid=tenantid from  batchdata with(nolock) where BatchId=@batchno   
set @validstat = (select validStat from ValidationStatus with (nolock) where tenantid=@tenantid)  
            
--------------------------VALIDATION SP START-----------------------------------            
exec DebitNoteTransInvoiceTypeValidations @batchno,@validStat,@tenantid             
exec DebitNoteTransInvoiceNumberValidations @batchno,@validStat ,@tenantid           
exec DebitNoteTransIssueDateValidations @batchno,@fmdate,@todate,@validStat,@tenantid            
            
exec DebitNoteTransOriginalIssueDateValidations @batchno,@fmdate,@todate,@validStat,@tenantid            
            
exec DebitNoteTransInvoiceCurrencyValidations @batchno,@validStat            
exec DebitNoteTransBillRefIDValidations @batchno,@validStat            
exec DebitNoteTransReasonForDebitValidations @batchno ,@validStat              
--exec DebitNoteTransBuyerMasterCodeValidations @batchno   ,@validStat            
exec DebitNoteTransBillRefIDRule01Validations @batchno  ,@validStat,@Tenantid            
exec DebitNoteTransBuyerLocationsValidations @batchno  ,@validStat --master            
exec DebitNoteTransBuyerNameValidations @batchno  ,@validStat,@tenantid            
--exec DebitNoteTransBuyerVATNumberValidations @batchno  ,@validStat,@tenantid            
            
exec DebitNoteTransInvoiceLineIdentifierValidations @batchno  ,@validStat            
            
exec DebitNoteTransItemNameValidations @batchno ,@validStat            
            
exec DebitNoteTransInvoicedQuantityUnitOfMeasureValidations @batchno  ,@validStat            
            
exec DebitNoteTransItemGrossPriceValidations @batchno  ,@validStat            
            
exec DebitNoteTransItemPriceDiscountValidations  @batchno  ,@validStat            
exec DebitNoteTransItemNetPriceValidations @batchno  ,@validStat            
            
exec DebitNoteTransInvoicedQuantityValidations @batchno  ,@validStat            
            
exec DebitNoteTransLineNetAmountValidations @batchno  ,@validStat            
            
exec DebitNoteTransInvoicedItemVATCategoryCodeValidations @batchno  ,@validStat            
            
exec debitNoteTransInvoicedItemVATRateValidations @batchno  ,@validStat            
            
            
exec DebitNoteTransVATExemptionReasonCodeValidations @batchno  ,@validStat            
            
exec DebitNoteTransVATExemptionReasonValidations @batchno  ,@validStat            
exec DebitNoteTransVATLineAmountValidations @batchno,@validstat,@tenantid            
            
EXEC DebitNoteTransBuyerTypeValidations @batchno,@validstat            
            
exec DebitNoteTransLineAmountInclusiveVATValidations @batchno,@validstat            
            
exec DebitNoteTransBillRefIDRule01Validations @batchno,@validStat,@tenantid            
            
exec DebitNoteTransBillRefIDRule02Validations @batchno,@validStat            
            
exec DebitNoteTransBillRefIDRule03Validations @batchno,@validStat            
            
exec DebitNoteTransRule01Validations @batchno,@validStat            
            
            
exec DebitNoteTransRule03Validations @batchno,@validStat            
            
exec DebitNoteTransRule04Validations @batchno,@validStat            
            
exec DebitNoteTransRule05Validations @batchno,@validStat,@tenantid            
--------------------------VALIDATION SP END-----------------------------------            
            
            
            
exec VI_insertProcessedImportStandardFilesDN @batchno,@tenantid            
            
exec DebitNoteSales15thDateValidation @batchno,@fmdate,@todate ,@validStat            
            
end              
end
GO

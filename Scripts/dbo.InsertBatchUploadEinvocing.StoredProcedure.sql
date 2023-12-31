USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[InsertBatchUploadEinvocing]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

      
     
CREATE    PROCEDURE [dbo].[InsertBatchUploadEinvocing] (            
  @json nvarchar(max)='[{"TestScenarios":"BRADY-02","TransactionType":"388","SAPInvoice Number":"9300000006","SAPInvoiceDate":"6/23/2023 12:00:00 AM","SAPInvoiceDueDate":"","Supply Date":"6/23/2023 12:00:00 AM","SupplyEndDate":"","PurchaseOrderID":"Test VC","ContractID":"1009241002","OrderPlacedBy":"","PaymentMeans":"","PaymentTerms":"Due net 30 Days","OriginalQuoteNumber":"26875558","DeliveryDocumentNo":"142066010","CarrierAndService":"FEDEX INTL PRIORITY","TermsOfDelivery":"Carriage and Insuran","InvoiceCurrencyCode":"SAR","ExchangeRate":"1","WeAreYourVendor":"","BillingReferenceID":"","CreditOrDebitReasons":"","Notes":"","BankName":"The Saudi British Bank","AccountName":"BRADY ARABIA MANUFACTURING COMPANY","SARAccount":"003-842770-003","IBAN":"SA7045000000003842770001","SWIFT":"SABBSARI","PayerMasterCode":"","PayerName":"AL OTHMAN PLASTIC PRODCUTS LTD CO شركة العثمان لمنتجات البلاستيك المحدودة ","PayerVATNumber":"","PayerStreet":"NAPA COMPOUND","PayerAdditionalStreet":"AL JARN RD","PayerBuildingNumber":"","PayerAdditionalNumber":"","PayerCity":"","PayerPostalCode":"31982","PayerDistrictOrNeighbourhood":"","PayerProvinceOrState":"AL AHSA","PayerCountryCode":"SA-Saudi Arabia","Attn":"MR GEORGE G","PayerOtherID":"","PayerContactNumber":"","ShipToMasterCode":"5648273","ShipToName":"AL OTHMAN PLASTIC PRODCUTS LTD CO","ShipToStreet":"NAPA COMPOUND","ShipToAdditionalStreet":"AL JARN RD","ShipToBuildingNumber":"","ShipToAdditionalNumber":"","ShipToCity":"","ShipToPostalCode":"31982","ShipToDisctrictOrNeighbourhood":"","ShipToProvinceOrState":"Western Province","ShipToCountryCode":"SA-Saudi Arabia","ShipToContactNumber":"","ShipToAttn":"","LineIdentifier":"1","LineOriginIndicator":"000010 SA","PartNumberDescription":"M210 Printer kit ","UnitPrice":"20000","ListPrice":"","ItemUOM":"PCS","ItemQuantity":"3","ItemNetPrice":"60000","CurrencyCode":"SAR","LineAmountExclusiveVAT":"60000","ItemVATCode":"S","ItemVATRate":"15","ItemVATAmount":"9000","LineAmountInclusiveVAT":"69000","VATExemptionReasonCode":"","VATExemptionReason":"","InvoiceNetAmount":"100000","Freight":"","Customs":"","Handling Charges":"","Other Charges":"","InvoiceVAT":"15000","InvoiceTotal":"115000","Footer":"","AdditionalDetails1":"","AdditionalDetails2":"","AdditionalDetails3":"","AdditionalDetails4":"","xml_uuid":"e8ddd106-5c1b-4ccb-b99f-e136ee251453"},{"TestScenarios":"BRADY-02","TransactionType":"388","SAPInvoice Number":"9300000006","SAPInvoiceDate":"6/23/2023 12:00:00 AM","SAPInvoiceDueDate":"","Supply Date":"6/23/2023 12:00:00 AM","SupplyEndDate":"","PurchaseOrderID":"Test VC","ContractID":"1009241002","OrderPlacedBy":"","PaymentMeans":"","PaymentTerms":"Due net 30 Days","OriginalQuoteNumber":"26875558","DeliveryDocumentNo":"142066010","CarrierAndService":"FEDEX INTL PRIORITY","TermsOfDelivery":"Carriage and Insuran","InvoiceCurrencyCode":"SAR","ExchangeRate":"1","WeAreYourVendor":"","BillingReferenceID":"","CreditOrDebitReasons":"","Notes":"","BankName":"The Saudi British Bank","AccountName":"BRADY ARABIA MANUFACTURING COMPANY","SARAccount":"003-842770-003","IBAN":"SA7045000000003842770001","SWIFT":"SABBSARI","PayerMasterCode":"","PayerName":"AL OTHMAN PLASTIC PRODCUTS LTD CO شركة العثمان لمنتجات البلاستيك المحدودة ","PayerVATNumber":"","PayerStreet":"NAPA COMPOUND","PayerAdditionalStreet":"AL JARN RD","PayerBuildingNumber":"","PayerAdditionalNumber":"","PayerCity":"","PayerPostalCode":"31982","PayerDistrictOrNeighbourhood":"","PayerProvinceOrState":"AL AHSA","PayerCountryCode":"SA-Saudi Arabia","Attn":"MR GEORGE G","PayerOtherID":"","PayerContactNumber":"","ShipToMasterCode":"5648273","ShipToName":"AL OTHMAN PLASTIC PRODCUTS LTD CO","ShipToStreet":"NAPA COMPOUND","ShipToAdditionalStreet":"AL JARN RD","ShipToBuildingNumber":"","ShipToAdditionalNumber":"","ShipToCity":"","ShipToPostalCode":"31982","ShipToDisctrictOrNeighbourhood":"","ShipToProvinceOrState":"Western Province","ShipToCountryCode":"SA-Saudi Arabia","ShipToContactNumber":"","ShipToAttn":"","LineIdentifier":"2","LineOriginIndicator":"000010 SA","PartNumberDescription":"M210 Colour Printer kit ","UnitPrice":"20000","ListPrice":"","ItemUOM":"PCS","ItemQuantity":"2","ItemNetPrice":"40000","CurrencyCode":"SAR","LineAmountExclusiveVAT":"40000","ItemVATCode":"S","ItemVATRate":"15","ItemVATAmount":"6000","LineAmountInclusiveVAT":"46000","VATExemptionReasonCode":"","VATExemptionReason":"","InvoiceNetAmount":"","Freight":"","Customs":"","Handling Charges":"","Other Charges":"","InvoiceVAT":"","InvoiceTotal":"","Footer":"","AdditionalDetails1":"","AdditionalDetails2":"","AdditionalDetails3":"","AdditionalDetails4":"","xml_uuid":"7947f6e9-d745-404d-952e-ce00cf1ae524"}]',            
  @fileName nvarchar(max)=null,             
  @tenantId int=127,             
  @fromDate datetime =null,             
  @toDate datetime =null,       
  @outBatchId  int = null OUTPUT       
) AS BEGIN Declare @MaxBatchId int              
    
 Select             
   @MaxBatchId = isnull(max(batchId),0)             
   from             
   BatchData;            
 Declare @batchId int = @MaxBatchId + 1;            
    set @outBatchId = @batchId      
  Insert into dbo.logs             
 values             
   (            
  'Einvoicing '+@json,             
  @toDate,             
  @batchId            
   )            
              
 INSERT INTO [dbo].[BatchData] (            
   [TenantId], [BatchId], [FileName],             
   [TotalRecords], [Status], [Type],             
   [CreationTime], [IsDeleted], fromDate, toDate            
 )             
 VALUES             
   (            
  @tenantId,             
  @batchId,             
  @fileName,             
  0,             
  'Unprocessed',             
  'Sales',             
  GETDATE(),             
  0,            
  @fromDate,            
  @toDate            
   )             
              
IF(@tenantId=127)
BEGIN
exec FileUploadSalesT1Brady  @batchId,@tenantId,@json      
END
ELSE 
begin
exec FileUploadSalesT1 @batchId,@tenantId,@json      
end
end
GO

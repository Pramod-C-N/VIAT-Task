USE [vita05-09 (1)]
GO
/****** Object:  StoredProcedure [dbo].[GenerateHtml]    Script Date: 07-09-2023 14:29:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER  procedure [dbo].[GenerateHtml]                           
(@tenantId int null=127,   
@isqrCode bit = 0,  
@reportName nvarchar(100)='Sales',  
@json nvarchar(max)='{
  "InvoiceNumber": "10143",
  "IssueDate": "2023-09-04T19:36:04.4933333",
  "DateOfSupply": "2023-08-08T00:00:00",
  "InvoiceCurrencyCode": "USD",
  "CurrencyCodeOriginatingCountry": null,
  "PurchaseOrderId": "TEST USD-1",
  "BillingReferenceId": "34",
  "ContractId": "1000053168",
  "LatestDeliveryDate": "2023-09-04T19:36:04.4933333",
  "Location": null,
  "CustomerId": null,
  "Status": null,
  "Additional_Info": null,
  "InvoiceNotes": "Builling external - delivery block 57 have material status of 04 - Inventory Depletion. Please check.",
  "PaymentType": "",
  "Supplier": [
    {
      "RegistrationName": "Brady",
      "VATID": "311673386800003",
      "GroupVATID": null,
      "CRNumber": "2050164710",
      "OtherID": null,
      "CustomerId": null,
      "Type": null,
      "FaxNo": "",
      "Website": "",
      "Address": {
        "Street": "Industrial City Second 2. street number 166",
        "AdditionalStreet": ".",
        "BuildingNo": "0020",
        "AdditionalNo": "",
        "City": "Dammam city",
        "PostalCode": "32244",
        "State": "Jeddah",
        "Neighbourhood": null,
        "CountryCode": "SA",
        "Type": null,
        "Language": null,
        "AdditionalData1": []
      },
      "ContactPerson": {
        "Name": null,
        "EmployeeCode": null,
        "ContactNumber": "998958746756",
        "GovtId": null,
        "Email": null,
        "Address": null,
        "Location": null,
        "Type": null,
        "Language": null,
        "AdditionalData1": []
      },
      "Language": null,
      "AdditionalData1": []
    }
  ],
  "Buyer": [
    {
      "RegistrationName": "AL ABDULKARIM TRADING GROUP",
      "VATID": "333333333333333",
      "GroupVATID": null,
      "CRNumber": "1166000",
      "OtherID": "",
      "CustomerId": "1033866",
      "Type": "Buyer",
      "FaxNo": null,
      "Website": null,
      "Address": {
        "Street": "KING FAISAL STREET",
        "AdditionalStreet": "5777",
        "BuildingNo": "N/A",
        "AdditionalNo": "",
        "City": "DAMMAM",
        "PostalCode": "31432",
        "State": "DAMMAM",
        "Neighbourhood": "N/A",
        "CountryCode": "SA",
        "Type": "Buyer",
        "Language": null,
        "AdditionalData1": []
      },
      "ContactPerson": {
        "Name": "MICHAEL DE SOUZA",
        "EmployeeCode": null,
        "ContactNumber": "96638337110",
        "GovtId": null,
        "Email": "mmadkhoom@akte.saudia1.com",
        "Address": null,
        "Location": null,
        "Type": "Buyer",
        "Language": null,
        "AdditionalData1": []
      },
      "Language": null,
      "AdditionalData1": [
        {
          "crNumber": "1166000",
          "registrationName": "REMAL TELECOM FACTORY SUBSIDIARY OF RAWABI HOLDING CO.",
          "vatid": "333333333333333",
          "address": {
            "buildingNo": "",
            "street": "",
            "additionalStreet": "",
            "city": "AL KHOBAR",
            "neighbourhood": "ALkhobar",
            "state": "ALkhobar",
            "postalCode": "31952",
            "countryCode": "SA"
          },
          "contactPerson": {
            "name": "REMAL TELECOM FACTORY SUBSIDIARY OF RAWABI HOLDING CO.",
            "contactNumber": "96638123282"
          }
        }
      ]
    }
  ],
  "Items": [
    {
      "Identifier": "1",
      "Name": "Y135353",
      "Description": "POSTBOX COLLECTION PLATE 138 X 211 BP2-Y",
      "BuyerIdentifier": null,
      "SellerIdentifier": null,
      "StandardIdentifier": null,
      "Quantity": 1,
      "UOM": "PAC",
      "UnitPrice": 289.95,
      "CostPrice": 289.95,
      "DiscountPercentage": 0,
      "DiscountAmount": 0,
      "GrossPrice": 289.95,
      "NetPrice": 289.95,
      "VATRate": 15,
      "VATCode": "S",
      "VATAmount": 43.49,
      "LineAmountInclusiveVAT": 333.44,
      "CurrencyCode": "USD",
      "TaxSchemeId": null,
      "Notes": null,
      "ExcemptionReasonCode": "",
      "ExcemptionReasonText": "",
      "Language": null,
      "AdditionalData1": [],
      "AdditionalData2": [],
      "isOtherCharges": false
    },
    {
      "Identifier": "2",
      "Name": "Y135353",
      "Description": "AC OLLECTION PLATE 138 X 211 BP2-Y",
      "BuyerIdentifier": null,
      "SellerIdentifier": null,
      "StandardIdentifier": null,
      "Quantity": 2,
      "UOM": "PAC",
      "UnitPrice": 250.31,
      "CostPrice": 250.31,
      "DiscountPercentage": 0,
      "DiscountAmount": 0,
      "GrossPrice": 500.63,
      "NetPrice": 500.63,
      "VATRate": 15,
      "VATCode": "S",
      "VATAmount": 75.09,
      "LineAmountInclusiveVAT": 575.72,
      "CurrencyCode": "USD",
      "TaxSchemeId": null,
      "Notes": null,
      "ExcemptionReasonCode": "",
      "ExcemptionReasonText": "",
      "Language": null,
      "AdditionalData1": [],
      "AdditionalData2": [],
      "isOtherCharges": false
    },
    {
      "Identifier": "1",
      "Name": "Freight",
      "Description": "Freight",
      "BuyerIdentifier": null,
      "SellerIdentifier": null,
      "StandardIdentifier": null,
      "Quantity": 1,
      "UOM": "PAC",
      "UnitPrice": 45.9,
      "CostPrice": 0,
      "DiscountPercentage": 0,
      "DiscountAmount": 0,
      "GrossPrice": 0,
      "NetPrice": 0,
      "VATRate": 0,
      "VATCode": "0",
      "VATAmount": 0,
      "LineAmountInclusiveVAT": 0,
      "CurrencyCode": "USD",
      "TaxSchemeId": null,
      "Notes": null,
      "ExcemptionReasonCode": "",
      "ExcemptionReasonText": "",
      "Language": null,
      "AdditionalData1": [],
      "AdditionalData2": [],
      "isOtherCharges": true
    }
  ],
  "InvoiceSummary": {
    "NetInvoiceAmount": 790.57,
    "NetInvoiceAmountCurrency": "SAR",
    "SumOfInvoiceLineNetAmount": 790.57,
    "SumOfInvoiceLineNetAmountCurrency": "SAR",
    "TotalAmountWithoutVAT": 790.58,
    "TotalAmountWithoutVATCurrency": "SAR",
    "TotalVATAmount": 118.58,
    "CurrencyCode": "SAR",
    "TotalAmountWithVAT": 909.16,
    "PaidAmount": 0,
    "PaidAmountCurrency": "SAR",
    "PayableAmount": 0,
    "PayableAmountCurrency": "SAR",
    "AdvanceAmountwithoutVat": 0,
    "AdvanceVat": 0,
    "AdditionalData1": [
      {
        "val1": "45.9",
        "val2": "0",
        "HandlingCharges": "0",
        "OtherCharges": "0"
      },
      {
        "val1": "45.9",
        "val2": "0",
        "HandlingCharges": "0",
        "OtherCharges": "0"
      }
    ]
  },
  "Discount": [],
  "VATDetails": [],
  "PaymentDetails": [],
  "InvoiceType": null,
  "InvoiceTypeCode": "388",
  "Language": null,
  "AdditionalData1": [
    {
      "exchangeRate": "3.75"
    }
  ],
  "AdditionalData2": [
    {
      "original_quote_number": "",
      "payment_terms": "60 Days end of Month",
      "invoice_reference_date": "08-08-2023 07:31",
      "delivery_date": "08-08-2023",
      "delivery_document_no": "110007380",
      "carrier_and_services": "BEST WAY AIR - INTERNATIONAL",
      "order_placed_by": "MOHAMMED AL MADKHOUM",
      "terms_of_delivery": "DAP/DELIVERED AT PLACE",
      "invoice_due_date": "31-10-2023",
      "we_are_your_vendor#": "N/A",
      "payment_means": "",
      "original_order_number": "1000053168",
      "purchase_order_no": "TEST USD-1",
      "bank_information": " The Saudi British Bank - USD BRADY ARABIA MANUFACTURING COMPANY the USD bank acct should be here SA7045000000003842770001 - USD SABBSARI"
    }
  ],
  "AdditionalData3": [
    {
      "footer": "Commercial registration nr: 2050164710 VAT nr: 311673386800003. For questions concerning the status of your account please contact our Accounts Receivable Department at +971 (4) 881 2524 or via brady.account.mea@bradycorp.com. For more information on Brady products please visit our website at www.bradyeurope.com. For our complete terms and conditions which apply to your order see www.bradyeurope.com/terms"
    }
  ],
  "AdditionalData4": [
    {
      "created_by": "BRADY BELGIUM"
    }
  ]
}')    
as                    
begin       
         
      
      
Declare @irnNo bigint
Declare @langtype nvarchar(200)
select @irnNo=max(irnno)  from irnmaster    
--set @langtype= (select Language from TenantConfiguration where tenantid=@tenantId and TransactionType='General');
Declare @invNo as nvarchar(500)= (select top 1 ClientName from AbpAuditLogs where TenantId=@tenantId and CustomData='Request Body Validation has started')           
--Exec InsertAuditLogs  null,@invNo,'PDF Generation','PDF Generation started',@TenantId                
--select * from logs order by id desc          
--set @reportName='Sales'       
     
declare @invoiceNumber nvarchar(200),                     
@invoiceDate nvarchar(200),                     
@invoiceType nvarchar(200),                     
@headerEnglish nvarchar(200),                     
@headerArabic nvarchar(200),                     
@vendorName nvarchar(200),                     
@vendorAddress nvarchar(1000),      
@faxNo nvarchar(1000),      
@website nvarchar(1000),    
@vendorVatId nvarchar(200),                     
@vendorCrNumber nvarchar(200),                     
@customerName nvarchar(200),                     
@customerAddress nvarchar(1000),                     
@customerContactNumber nvarchar(200),                     
@customerVatId nvarchar(200),                     
@customerEmail nvarchar(200),                     
@customerCrNumber nvarchar(200),                     
@itemRows nvarchar(max),       
@chargesRows nvarchar(max),      
@itemsfooter nvarchar(max),      
@chargefooter nvarchar(max),      
@totalBeforeVat decimal(15,2),                     
@totalDiscount decimal(15,2),                     
@finalTotalBeforeDiscount decimal(15,2),      
@totalothercharges decimal(15,2),      
@totalotherVATcharges decimal(15,2),      
@totalVat decimal(15,2),                     
@totalWithVat decimal(15,2),                     
@dueBalance decimal(15,2),                     
@taxableAmountStd decimal(15,2),                     
@taxAmountStd decimal(15,2),                     
@totalAmountStd decimal(15,2),                     
@taxableAmountZero decimal(15,2),                     
@taxAmountZero decimal(15,2),                     
@totalAmountZero decimal(15,2),      
@taxableAmountOut decimal(15,2),                     
@taxAmountOut decimal(15,2),                     
@totalAmountOut decimal(15,2),      
@taxableAmountExmpt decimal(15,2),                     
@taxAmountExmpt decimal(15,2),                     
@totalAmountExmpt decimal(15,2),      
@referenceNumber nvarchar(50),              
@notes nvarchar(200),                     
@html nvarchar(max),                    
@rowHtml nvarchar(max),        
@chargesrow nvarchar(max),        
@orientation varchar(10),        
@exchangeRate decimal(15,2),      
@invoiceCurrencyCode nvarchar(200),      
@customerId nvarchar(200), 
@lang_vendorbuildingNo nvarchar(max),
@lang_vendoradditionalBuildNo nvarchar(max),
@lang_vendorstreet nvarchar(max),
@lang_vendoradditionalStreet nvarchar(max),
@lang_vendorpostalcode nvarchar(max),
@lang_vendorcountrycode nvarchar(max),
@lang_vendoraddress nvarchar(max),
@lang_vendorName nvarchar(200),
@lang_vendorVatId nvarchar(200),                     
@lang_vendorCrNumber nvarchar(200),                     
@lang_customerName nvarchar(200),   
@lang_customerId nvarchar(200),
@lang_customerAddress nvarchar(1000),                     
@lang_customerContactNumber nvarchar(200),                     
@lang_customerVatId nvarchar(200),                     
@lang_customerEmail nvarchar(200),                     
@lang_customerCrNumber nvarchar(200),
@lang_billtoattention	nvarchar(200),
--  if(@tenantId <> 127) --brady tenantid      
--begin      
--set @tenantId=9          
--set @exchangeRate=1      
--end      
-- Invoice Master Details begin --   
@tenancyname nvarchar(max)

set @tenancyname =(select name from AbpTenants where id=@tenantId)
if (@tenantId<>125 and @tenantId<>129  and @tenantId<>54 and @tenantId<>131 and @tenantId<>127 and @tenantId<>133)    
Begin    
set @tenantId=9      
End    
 select @totalBeforeVat = sum(UnitPrice * Quantity), @totalVat = sum(VATAmount), @totalWithVat=sum(LineAmountInclusiveVAT) from openjson(@json,'$.Items')                     
 with                     
 (UnitPrice decimal(15,2) '$.UnitPrice',                    
 Quantity decimal(15,2) '$.Quantity',                    
 VATAmount decimal(15,2) '$.VATAmount',                    
 LineAmountInclusiveVAT decimal(15,2) '$.LineAmountInclusiveVAT',    
 isOtherCharges bit '$.isOtherCharges'    
 ) where isOtherCharges =0;           
     
 set @totalDiscount = @totalBeforeVat - (@totalWithVat - @totalVat);                 
 set @finalTotalBeforeDiscount = @totalBeforeVat - @totalDiscount;                       
 set @dueBalance  = @totalWithVat;        
         
 select @exchangeRate = exchangeRate        
 from openjson(@json,'$.AdditionalData1[0]')        
 with(exchangeRate decimal(15,2) '$.exchangeRate')        
        
 --print @exchangeRate        
  if (@tenantId<>125)    
begin                  
 if(@totalWithVat<1000)                    
 begin          
 (select @html=Html,@orientation=orientation from ReportTemplate where TenantId = @tenantId and name=@reportName+'Simplified')  -- to be changed to -> name=@reportName+' simplified'                    
 set @rowHtml = (select RowHtml from ReportTemplate where TenantId = @tenantId  and name=@reportName+'Simplified')      
  set @chargesrow = (select chargesrow from ReportTemplate where TenantId = @tenantId  and name=@reportName+'Simplified')-- to be changed to -> name=@reportName+' simplified'     
      
 --set @invoiceNumber = '0200000'        
 set @invoiceType = '0200000'                    
if(@reportName like '%Sales%')      
 begin      
 set @headerEnglish = 'Simplified Tax Invoice'                
 set @headerArabic = N'فاتورة ضريبية مبسطة'       
 end      
 else if(@reportName like '%Credit%')      
 begin      
  set @headerEnglish = 'Simplified Credit Note'                
 set @headerArabic = N'إشعار دائن مبسط'       
 end      
 else      
 begin      
  set @headerEnglish = 'Simplified Debit Note'                
 set @headerArabic = N'إشعار خصم مبسط'       
 end                   
 end     
 else                    
 begin                    
 (select @html=Html,@orientation=orientation from ReportTemplate where TenantId = @tenantId and name=@reportName)                     
 set @rowHtml = (select RowHtml from ReportTemplate where TenantId = @tenantId  and name=@reportName)      
 set @chargesrow = (select chargesrow from ReportTemplate where TenantId = @tenantId  and name=@reportName)      
 --set @invoiceNumber = '0100000'        
 set @invoiceType = '0100000'                    
if(@reportName like '%Sales%')      
 begin      
 set @headerEnglish = 'Tax Invoice'                
 set @headerArabic = N'الفاتورة الضريبية'       
 end        
 else if(@reportName like '%Credit%')      
 begin      
  set @headerEnglish = 'Credit Note'                
 set @headerArabic = N'إشعار دائن'       
 end      
 else      
 begin      
  set @headerEnglish = 'Debit Note'                
 set @headerArabic = N'إشعار مدين'       
 end                    
 end         
 end    
 else    
 begin    
  begin                    
 (select @html=Html,@orientation=orientation from ReportTemplate where TenantId = @tenantId and name=@reportName)                     
 set @rowHtml = (select RowHtml from ReportTemplate where TenantId = @tenantId  and name=@reportName)      
 set @chargesrow = (select chargesrow from ReportTemplate where TenantId = @tenantId  and name=@reportName)      
 --set @invoiceNumber = '0100000'        
 set @invoiceType = '0100000'                    
if(@reportName like '%Sales%')      
 begin      
 set @headerEnglish = 'Tax Invoice'                
 set @headerArabic = N'الفاتورة الضريبية'       
 end        
 else if(@reportName like '%Credit%')      
 begin      
  set @headerEnglish = 'Credit Note'                
 set @headerArabic = N'إشعار دائن'       
 end      
 else      
 begin      
  set @headerEnglish = 'Debit Note'                
 set @headerArabic = N'إشعار مدين'       
 end                    
 end         
 end    
--------Invoice Master Details end -------------                   

------------------------ brady------------------          
declare @billtoattention  nvarchar(200),          
@shiptoaccount  nvarchar(200),          
@phone  nvarchar(200),          
@shiptoaddress  nvarchar(500),          
@shiptoatn  nvarchar(200),          
@referenceinvoiceno  nvarchar(200),          
@invoicereferencedate  nvarchar(200),          
@yourvendor  nvarchar(200),          
@referenceinvoicenoPO  nvarchar(200),          
@termsofdelivery  nvarchar(200),          
@originalquote  nvarchar(200),          
@paymentterms  nvarchar(200),          
@orderplacedby  nvarchar(200),          
@ourorderref  nvarchar(200),          
@delivery  nvarchar(200),          
@deliverydate  nvarchar(200),          
@carrierandservices  nvarchar(200),          
@customer2  nvarchar(200),          
@yourvatnumber  nvarchar(200),          
@freight  decimal(15,2),          
@custom1  decimal(15,2),          
@invoicetotal  decimal(15,2),          
@bank  nvarchar(200),        
@desc1 nvarchar(200),        
@desc2 nvarchar(200),      
@originalorderno nvarchar(200),      
@invoiceRefDate nvarchar(200),      
@deliveryDatee nvarchar(200),    
@customerPo nvarchar(200),    
@invoiceDueDate nvarchar(200)    
    
 declare @accountName nvarchar(max);    
declare @accountNumber nvarchar(max);    
declare @iban nvarchar(max);    
 declare @nameofbank nvarchar(max);    
 declare @swiftcode nvarchar(max);    
  declare @branchName nvarchar(max);    
   declare @branchAddress nvarchar(max);    
     declare @customerCountryCode nvarchar(50)='';        
declare @currency2 nvarchar(50)=''              
        
  select  @shiptoaddress=isnull(buildingNo,'')+' '+          
  isnull(city,'')+' '+isnull(neighbourhood,'')+' '+isnull(street,'')+' '+isnull(state,'')+' '+isnull(postalCode,'')+' '+isnull(countryCode,''),          
  @phone=contactNo,          
  @shiptoaccount = shiptoaccount,          
  @shiptoatn = shiptoatn,        
  @customer2 = shiptocode                    
  from openjson(@json,'$.Buyer[0].AdditionalData1[0]')           
  with          
  (buildingNo nvarchar(200) '$.address.buildingNo',          
  city nvarchar(200) '$.address.city',          
  neighbourhood nvarchar(200) '$.address.neighbourhood',          
  street nvarchar(200) '$.address.street',      
  state nvarchar(200) '$.address.state',          
  postalCode nvarchar(200) '$.address.postalCode',          
  countryCode nvarchar(200) '$.address.countryCode',          
  contactNo nvarchar(200) '$.contactPerson.contactNumber',          
  shiptoaccount nvarchar(200) '$.registrationName',          
  shiptoatn nvarchar(200) '$.contactPerson.name',      
  shiptocode nvarchar(200) '$.crNumber')          
   
   select                     
      
@referenceinvoiceno = billreferenceNumber                   
--@yourvatnumber = REPLACE(customerVatNo,'VAT:','') ,      
--@customerId = customerId      
FROM OPENJSON(@json)                    
  WITH (                  
  billreferenceNumber nvarchar(50) '$.BillingReferenceId'                          
 --customer nvarchar(200) '$.Buyer.RegistrationName',                                
 --customerVatNo nvarchar(200) '$.Buyer.VATID' ,      
 --customerId nvarchar(200) '$.Buyer.CustomerId'      
  )       
  
--  ----- Customer, Vendor, Notes begin ------------                    
                    
       
select                     
@referenceNumber = referenceNumber,      
@invoiceCurrencyCode= invoiceCurrencyCode,      
@invoiceNumber = invoiceNumber,                    
@invoiceDate =  cast(invoiceDate as date),                    
@currency2 = InvoiceCurrencyCode ,  
@notes =notes
FROM OPENJSON(@json)                    
  WITH (                  
  referenceNumber nvarchar(50) '$.BillingReferenceId',      
  invoiceCurrencyCode nvarchar(50) '$.InvoiceCurrencyCode',      
    invoiceNumber nvarchar(50) '$.InvoiceNumber',                    
    invoiceDate nvarchar(50) '$.IssueDate',                                        
 notes nvarchar(500) '$.InvoiceNotes',        
 InvoiceCurrencyCode nvarchar(50) '$.InvoiceCurrencyCode'        
  );                    
           
    
declare @showCur nvarchar(50)='display : none'        
declare @show1 nvarchar(50)='display : none'      
if(@currency2<>'SAR')        
begin        
set @showCur=''      
set @show1='display : none'      
end      
else      
begin      
set @show1=''      
set @showCur='display : none'      
end        
  if(@customerCountryCode<>'SA')                    
  begin                    
   set @invoiceType = STUFF(@invoiceType, 5, 1, '1') --used to replace 5th character of @invoiceType                    
  end                                
------ Customer, Vendor, Notes end -------- 
 




      ---tenant dertails multilang--
	 --select @lang_vendorbuildingNo=BuildingNo,
	 --@lang_vendoradditionalBuildNo= AdditionalBuildingNumber,
	 --@lang_vendorstreet=Street,@lang_vendoradditionalStreet= AdditionalStreet,@lang_vendorpostalcode=PostalCode,@lang_vendorcountrycode=CountryCode from TenantAddress where tenantid=@tenantId;
--	select @lang_vendoraddress = isnull(Street,'')+' '+isnull(AdditionalStreet,'')+' '+isnull(BuildingNo,'')+' '+isnull(city,'')+' '+isnull(state,'')+' '+isnull(PostalCode,'')+' '+isnull(CountryCode,'')
--from TenantAddress where tenantid=@tenantId and AddressType='AR'
set @lang_vendoraddress=N'الأمير سعود الفيصل2652
 7490 الخالدية جدة المنطقة الغربية المملكة العربية السعودية 23422'

set @lang_vendorName=( select LangTenancyName from TenantBasicDetails where tenantid=@tenantId)

 select                     
@vendorName = vendorName,                    
@vendorCrNumber = vendorCrNumber,                    
@vendorVatId = REPLACE(vendorVatId,'VAT:',''),                    
@vendorAddress = isNull(vendorStreet,'')+' '+isNull(vendorAdditonalStreet,'')+' '+isNull(vendorBuildingNo,'')+' '+isNull(vendorCity,'')+' '+    
isNull(vendorState,'')+' '+isNull(vendorPostalCode,'')+' '+isNull(vendorCountryCode,'')+' Contact No: '+isNull(vendorContactNumber,''),     
@faxNo=FaxNo,    
@website=Website      
 from openjson(@json,'$.Supplier')                     
  WITH (                  
 vendorName nvarchar(200) '$.RegistrationName',                    
 vendorStreet nvarchar(200) '$.Address.Street' ,                    
 vendorAdditonalStreet nvarchar(200) '$.Address.AdditionalStreet',    
 faxNo nvarchar(200) '$.FaxNo',    
 website nvarchar(200) '$.Website',    
 vendorBuildingNo nvarchar(200) '$.Address.BuildingNo',                    
    vendorCity nvarchar(200) '$.Address.City' ,                    
 vendorState nvarchar(200) '$.Address.State'  ,                    
 vendorPostalCode nvarchar(200) '$.Address.PostalCode' ,                    
 vendorCountryCode nvarchar(200) '$.Address.CountryCode',                    
    vendorContactNumber nvarchar(200) '$.ContactPerson.ContactNumber' ,                    
 vendorVatId nvarchar(200) '$.VATID',   
 vendorCrNumber nvarchar(200) '$.CRNumber')


 select 
 @lang_customerName=langcustomerName,
 @lang_customerId =langcustomerId,
 @lang_customerAddress=isnull(langcustomerBuildingNo,'')+' '+          
  isnull(langcustomerCity,'')+' '+isnull(langcustomerNeighbour,'')+' '+isnull(langcustomerStreet,'')+' '+isnull(langcustomerState,'')+' '+isnull(langcustomerPostalCode,'')+' '+isnull(langcustomerCountryCode,''),          
  @lang_customerContactNumber=langcustomerContactNumber,          
  @lang_customerEmail = langcustomerEmail,
  @lang_customerVatId=langcustomerVatId,
  @lang_billtoattention	 = langcustomerbilltoAttn
 from openjson(@json,'$.Buyer')                     
 with                     
(langcustomerName nvarchar(200) '$.RegistrationName',
 langcustomerId nvarchar(200) '$.CustomerId', 
 langcustomerStreet nvarchar(200) '$.Address.Street' ,           
 langcustomerAdditonalStreet nvarchar(200) '$.Address.AdditionalStreet',                  
 langcustomerBuildingNo nvarchar(200) '$.Address.BuildingNo',                    
 langcustomerCity nvarchar(200) '$.Address.City' ,                    
 langcustomerState nvarchar(200) '$.Address.State'  , 
 langcustomerNeighbour nvarchar(200) '$.Address.State'  ,                    
 langcustomerPostalCode nvarchar(200) '$.Address.PostalCode' ,                    
 langcustomerCountryCode nvarchar(200) '$.Address.CountryCode',
 langcustomerbilltoAttn nvarchar(200) '$.ContactPerson.Name' ,                   
 langcustomerContactNumber nvarchar(200) '$.ContactPerson.ContactNumber' ,                   
 langcustomerVatId nvarchar(200) '$.VATID',                    
 langcustomerCrNumber nvarchar(200) '$.CRNumber',                    
 langcustomerEmail nvarchar(200) '$.ContactPerson.Email',
 lang nvarchar(200) '$.Language' )where lang <> 'EN'  ;      
 

 select 
@customerAddress=isnull(customerBuildingNo,'')+' '+          
  isnull(customerCity,'')+' '+isnull(customerNeighbour,'')+' '+isnull(customerStreet,'')+' '+isnull(customerState,'')+' '+isnull(customerPostalCode,'')+' '+isnull(customerCountryCode,''),          
  @customerContactNumber=customerContactNumber,          
  @customerEmail = customerEmail,
  @customerName = customerName,                    
@customerName = customerName,                    
  @customerVatId=customerVatId,
  @billtoattention=billtoAttn
 from openjson(@json,'$.Buyer')                     
 with                     
 ( customerName nvarchar(200) '$.RegistrationName',      
 customerId nvarchar(200) '$.CustomerId', 
 customerStreet nvarchar(200) '$.Address.Street' ,           
 customerAdditonalStreet nvarchar(200) '$.Address.AdditionalStreet',                  
 customerBuildingNo nvarchar(200) '$.Address.BuildingNo',                    
 customerCity nvarchar(200) '$.Address.City' ,                    
 customerState nvarchar(200) '$.Address.State'  , 
  customerNeighbour nvarchar(200) '$.Address.State'  ,                    
 customerPostalCode nvarchar(200) '$.Address.PostalCode' ,                    
 customerCountryCode nvarchar(200) '$.Address.CountryCode',
 billtoAttn nvarchar(200) '$.ContactPerson.Name' ,                   
 customerContactNumber nvarchar(200) '$.ContactPerson.ContactNumber' ,                    
 customerVatId nvarchar(200) '$.VATID',                    
 customerCrNumber nvarchar(200) '$.CRNumber',                    
 customerEmail nvarchar(200) '$.ContactPerson.Email',
 customertype nvarchar(200) '$.Type',
 lang nvarchar(200) '$.Language' )where (lang = 'EN' or lang is null) and  customertype = 'Buyer' ; 
 
        
select       @freight= case when val1='' then '0' else val1 end,     @custom1=case when val2='' then '0' else val2 end,         @invoicetotal= isNull(totalOther,0) + @totalWithVat,     @desc1 = desc1,     @desc2 = desc2  
from openjson(@json,'$.InvoiceSummary.AdditionalData1[0]')  
with(val1 nvarchar(200) '$.val1',val2 nvarchar(200) '$.val2',totalOther decimal(15,2) '$.totalOther',desc1 nvarchar(200) '$.desc1',desc2 nvarchar(200) '$.desc2')      
declare @showFreight nvarchar(50)=''         
if(cast(@freight as nvarchar)<>'0.00')        
begin        
set @showFreight='display : none'        
end      
else      
begin      
set @showFreight='display : none'    --exec generatehtml  
end   
  
declare @isqrCodedisplay nvarchar(300) = 'width: 100px; height: 100px; float: initial; vertical-align: top; margin: 0px 50px;'
declare @isqrCodedisplay1 nvarchar(300) = 'width: 150px;height: 150px;display: block;margin-left: auto;margin-right: auto;margin-top: 20px;padding-top: 2%;'  -- for tenant 131

if(@isqrCode = 1)  
begin        
set @isqrCodedisplay='width: 100px; height: 100px; float: initial; vertical-align: top; margin: 0px 50px;display : none;'
set @isqrCodedisplay1 = 'width: 150px;height: 150px;display: block;margin-left: auto;margin-right: auto;margin-top: 20px;padding-top: 2%;display : none;'

end      
else      
begin      
set @isqrCodedisplay='width: 100px; height: 100px; float: initial; vertical-align: top; margin: 0px 50px;'
set @isqrCodedisplay1 = 'width: 150px;height: 150px;display: block;margin-left: auto;margin-right: auto;margin-top: 20px;padding-top: 2%;'

end   
      
      
declare @showCustom nvarchar(50)=''         
if(cast(@custom1 as nvarchar)<>'0.00')        
begin        
set @showCustom=''        
end      
else      
begin      
set @showCustom='display : none'      
end      
      
declare @showTotal nvarchar(50)=''         
if(cast(@freight as nvarchar)<>'0.00' or cast(@custom1 as nvarchar)<>'0.00')        
begin        
set @showTotal=''        
end      
else      
begin      
set @showTotal='display : none'      
end      
     
     
    select        
 @bank=bankInformation,      
 @originalorderno=originalorderno,      
 @invoicereferencedate=dueDate,    
     
 --@referenceinvoiceno=billreferenceNumber,        
 @orderplacedby=orderPlacedBy,       
 @referenceinvoicenoPO=purchaseOrderNo,      
 @originalquote=originalQuoteNum,        
 @termsofdelivery=termsOfDelivery,        
 @carrierandservices=carrier,        
 @delivery=deliveryDocumentNo,      
@deliverydate=dueDate,      
@yourvendor=yourvendor,      
@paymentterms=paymentTerms  ,    
@deliveryDatee=deliveryDate,      
@invoiceRefDate=invoiceRefDate,    
@customerPo=customerPo,    
@invoiceDueDate=invoiceDueDate    
      
  from openjson(@json,'$.AdditionalData2[0]')           
  with          
  (bankInformation nvarchar(200) '$.bank_information',      
  originalorderno nvarchar(200) '$.original_order_number',      
  purchaseOrderNo nvarchar(200) '$.purchase_order_no',        
  orderPlacedBy nvarchar(200) '$.order_placed_by',        
  dueDate nvarchar(200) '$.invoice_due_date',        
  originalQuoteNum nvarchar(200) '$.original_quote_number',        
  deliveryTermsDescription nvarchar(200) '$.delivery_terms_description',        
  termsOfDelivery nvarchar(200) '$.terms_of_delivery',        
  yourvendor nvarchar(200) '$."we_are_your_vendor#"',        
  carrier nvarchar(200) '$.carrier_and_services',        
  deliveryDocumentNo nvarchar(200) '$.delivery_document_no',        
  paymentTerms nvarchar(200) '$.payment_terms',      
  purchaseOrderNo nvarchar(200) '$.purchase_order_no',        
  deliveryDate nvarchar(200) '$.delivery_date',      
  invoiceRefDate nvarchar(200) '$.invoice_reference_date' ,    
 -- customerPo nvarchar(200) '$."customer_po_#_"',   
   customerPo nvarchar(200) '$."customer_po_#"', --SAGCO
  invoiceDueDate nvarchar(200) '$.due_date'  --SAGCO    
  )        
          
  ----------------------        

         
------------------------------------------------          
                    
                   
                 
------ VAT summary begin --------                    
 select                     
 @totalAmountStd = sum(LineAmountInclusiveVAT),                     
 @taxAmountStd = sum(VATAmount),                     
 @taxableAmountStd=sum(NetPrice)                     
 from openjson(@json,'$.Items')                     
 with                     
 (LineAmountInclusiveVAT decimal(15,2) '$.LineAmountInclusiveVAT',                    
 VATAmount decimal(15,2) '$.VATAmount',                    
 NetPrice decimal(15,2) '$.NetPrice',                    
 VATCode nvarchar(50) '$.VATCode') where VATCode='S';                    
                    
 select                     
 @totalAmountZero = sum(LineAmountInclusiveVAT),                     
 @taxAmountZero = sum(VATAmount),                     
 @taxableAmountZero=sum(NetPrice)                     
 from openjson(@json,'$.Items')                     
 with                     
 (LineAmountInclusiveVAT decimal(15,2) '$.LineAmountInclusiveVAT',                    
 VATAmount decimal(15,2) '$.VATAmount',                    
 NetPrice decimal(15,2) '$.NetPrice',                    
  VATCode nvarchar(50) '$.VATCode') where VATCode='Z';         
        
   select                     
 @totalAmountOut = sum(LineAmountInclusiveVAT),                     
 @taxAmountOut = sum(VATAmount),                     
 @taxableAmountOut=sum(NetPrice)                     
 from openjson(@json,'$.Items')                     
 with                     
 (LineAmountInclusiveVAT decimal(15,2) '$.LineAmountInclusiveVAT',                    
 VATAmount decimal(15,2) '$.VATAmount',                    
 NetPrice decimal(15,2) '$.NetPrice',                    
  VATCode nvarchar(50) '$.VATCode') where VATCode='O';      
      
   select                     
 @totalAmountExmpt = sum(LineAmountInclusiveVAT),                     
 @taxAmountExmpt = sum(VATAmount),                     
 @taxableAmountExmpt=sum(NetPrice)                     
 from openjson(@json,'$.Items')                     
 with                     
 (LineAmountInclusiveVAT decimal(15,2) '$.LineAmountInclusiveVAT',                    
 VATAmount decimal(15,2) '$.VATAmount',                    
 NetPrice decimal(15,2) '$.NetPrice',                    
  VATCode nvarchar(50) '$.VATCode') where VATCode='E';      
      
  -- VAT summary end --                    
             ---bank details---    
 select     
  @accountName= AccountName,    
 @accountNumber=AccountNumber,@iban=IBAN,@nameofbank=BankName,@swiftcode=SwiftCode,@branchName=BranchName,@branchAddress=BranchAddress from TenantBankDetail where tenantid=@tenantId;    
     
       


			 select                     
Sum(Quantity) as Quantity,                    
Sum(UnitPrice) as UnitPrice,                    
Sum(DiscountAmount) as DiscountAmount,                    
Sum(cast(((UnitPrice*Quantity)-(DiscountAmount))as decimal(15,2))) as TaxableAmount,                    
Sum(VATAmount) as VATAmount,                    
Sum(LineAmountInclusiveVAT) as LineAmountInclusiveVAT                    
INTO #itemsfooter                    
 from openjson(@json,'$.Items')                     
 with                     
 (Description nvarchar(max) '$.Description',                    
 Quantity decimal(15,2) '$.Quantity',                    
 UnitPrice decimal(15,2) '$.UnitPrice',                    
 DiscountPercentage decimal(15,2) '$.DiscountPercentage',                    
 DiscountAmount decimal(15,2) '$.DiscountAmount',                    
 VATAmount decimal(15,2) '$.VATAmount',         
  LineAmountInclusiveVAT decimal(15,2) '$.LineAmountInclusiveVAT',                    
 LineAmountInclusiveVAT decimal(15,2) '$.LineAmountInclusiveVAT',                    
 VATRate decimal(15,2) '$.VATRate',    
  isOtherCharges bit '$.isOtherCharges') where isOtherCharges = 0  ;   
			 ---/////////////////////////----
  -- dynamic item row generation begin --                    
select         
identity(int,1,1) as Id,      
Name as Name,      
Description as Description,                    
Quantity as Quantity,                    
UnitPrice as UnitPrice,                    
DiscountAmount as DiscountAmount,                    
cast(((UnitPrice*Quantity)-(DiscountAmount))as decimal(15,2)) as TaxableAmount,      
VATCode as VATCode,      
VATRate as VATRate,                    
VATAmount as VATAmount,                    
Identifier as Identifier,                    
uom as uom,                    
LineAmountInclusiveVAT as LineAmountInclusiveVAT,      
ExemptionReason as ExemptionReason,      
ExemptionReasonCode as ExemptionReasonCode,      
isOtherCharges as isOtherCharges      
      
INTO #items                    
 from openjson(@json,'$.Items')                     
 with                     
 (Description nvarchar(max) '$.Description',      
 Name nvarchar(max) '$.Name',      
 Quantity decimal(15,2) '$.Quantity',                    
 UnitPrice decimal(15,2) '$.UnitPrice',                    
 DiscountPercentage decimal(15,2) '$.DiscountPercentage',                    
 DiscountAmount decimal(15,2) '$.DiscountAmount',                    
 VATAmount decimal(15,2) '$.VATAmount',                    
 LineAmountInclusiveVAT decimal(15,2) '$.LineAmountInclusiveVAT',      
 VATCode nvarchar(max) '$.VATCode',      
 VATRate decimal(15,2) '$.VATRate',                    
 Identifier varchar(100) '$.Identifier',          
 ExemptionReasonCode nvarchar(100) '$.ExcemptionReasonCode',      
 ExemptionReason nvarchar(100) '$.ExcemptionReasonText',      
 uom nvarchar(100) '$.UOM',      
 isOtherCharges bit '$.isOtherCharges') ;                    
                    
                    
 ------------------                    
 set @itemRows= ''                    
 declare                     
 @Id int,      
 @Name nvarchar(max),      
 @Description nvarchar(max),                    
 @Quantity int,                    
 @unit nvarchar(max),                    
 @UnitPrice decimal(15,2),                    
 @DiscountAmount decimal(15,2),                    
 @TaxableAmount decimal(15,2),       
 @VATCode nvarchar(max),      
 @VATRate decimal(15,2),                    
 @VATAmount decimal(15,2),        
 @LineAmountInclusiveVATCurr decimal(15,2),        
 @LineAmountInclusiveVAT decimal(15,2),                    
 @Identifier varchar(100),                    
 @uom varchar(100),      
 @ExemptionReasonCode nvarchar(max),      
 @ExemptionReason nvarchar(max),      
 @totalrows int = (select count(*) from #items where isOtherCharges=0),                     
 @currentrow int = 0                    
                        
           --select * from #items where isOtherCharges='false'      
      
    while @currentrow <  @totalrows                      
    begin                     
        select                     
  @Id=Id,      
  @Name=Name,      
  @Description=Description,                    
  @Quantity=Quantity,                    
  @UnitPrice=UnitPrice,                    
  @DiscountAmount = DiscountAmount,                    
  @TaxableAmount= TaxableAmount,                    
  @VATRate = VATRate,        
  @VATCode=VATCode,      
  @VATAmount = VATAmount,                    
  @Identifier = Identifier,        
  @LineAmountInclusiveVATCurr = LineAmountInclusiveVAT ,        
  @LineAmountInclusiveVAT = LineAmountInclusiveVAT ,      
  @ExemptionReason= ExemptionReason,      
  @ExemptionReasonCode = ExemptionReasonCode,      
  @uom = uom                    
  from #items                     
  where Id = @currentrow + 1 and isOtherCharges = 'false';      
   declare @itemsfooterstyle nvarchar(200)      
      
    declare @showreason nvarchar(50)='display : none'      
  if(@VATCode <> 'S')      
  begin      
  set @showreason=''      
  end       
        set @itemRows=@itemRows+' '+@rowHtml                    
    set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@Slno',@Id)    
        set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@Id',@Name)                    
  set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@invoiceDate',cast(@invoiceDate as date))                    
        set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@invoiceNumber',@irnNo)         
    --set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@DescriptionArabic',N'زجاجة - 225 مل')      
      
  set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@Description',@Description)                    
              if(UPPER(@tenancyname) LIKE 'SAGCO%')
			  begin
			    set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@Quantity',@Quantity) --SAGCO
			  end
			  else
			  begin
			  set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@Quantity',concat(Concat(@Quantity,'  '),@uom))
			  end
       -- set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@Quantity',concat(Concat(@Quantity,'  '),@uom))    
  --set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@Quantity',@Quantity) --SAGCO
  set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@profQuantity',@Quantity)      
        set @itemRows = [dbo].ReplaceHtmlDecimal(@itemRows,'@unitprice',@UnitPrice/ @exchangeRate)      
   set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@exemption',concat(Concat(@ExemptionReasonCode,':'),@ExemptionReason))                    
         set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@unit',@uom)      
        set @itemRows = [dbo].ReplaceHtmlDecimal(@itemRows,'@DiscountAmount',@DiscountAmount/ @exchangeRate)                    
        set @itemRows = [dbo].ReplaceHtmlDecimal(@itemRows,'@TaxableAmount',@TaxableAmount/ @exchangeRate)                    
        set @itemRows = [dbo].ReplaceHtmlDecimal(@itemRows,'@VATRate',@VATRate)                    
        set @itemRows = [dbo].ReplaceHtmlDecimal(@itemRows,'@VATAmount',@VATAmount/ @exchangeRate)       
  set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@showreason',@showreason)      
  set @itemRows = [dbo].ReplaceHtmlDecimal(@itemRows,'@LineAmountInclusiveVATCur',cast(@LineAmountInclusiveVAT/ @exchangeRate as decimal(15,2)))        
        set @itemRows = [dbo].ReplaceHtmlDecimal(@itemRows,'@LineAmountInclusiveVAT',@LineAmountInclusiveVAT )       
     set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@itemsfooterstyle','')      
      
       set @currentrow = @currentrow +1                    
    end                      
  -- dynamic item row generation end --          
     ----othercharge      
   declare @chargefooterstyle nvarchar(200)      
   set @chargesRows= ''                    
 declare      
 @CId int,      
 @chargename nvarchar(max),      
 @chargedescription nvarchar(max),                              
 @chargeAmount decimal(15,2),                    
 @totalchargerows int = (select max(Id) from #items where isOtherCharges=1),                     
 @currentchargerow int = (select top 1 Id from #items where isOtherCharges=1)                   
      
              
while @currentchargerow <=  @totalchargerows      
    begin                     
        select         
    @CId=Id,      
  @chargename=Name,      
  @chargedescription=Description,                    
  @chargeAmount=UnitPrice                        
  from #items                     
  where  Id = @currentchargerow  and isOtherCharges =1;      
  ----      
      
        
   set @chargesRows=@chargesRows+' '+ @chargesrow      
          set @chargesRows = [dbo].ReplaceHtmlString(@chargesRows,'@chargename',@chargename)                    
    set @chargesRows = [dbo].ReplaceHtmlString(@chargesRows,'@chargedescription',@chargedescription)            
       set @chargesRows = [dbo].ReplaceHtmlDecimal(@chargesRows,'@chargeAmount',@chargeAmount)        
    set @chargesRows = [dbo].ReplaceHtmlString(@chargesRows,'@chargefooterstyle','')        
      
     set @currentchargerow = @currentchargerow +1      
    end      
      
        
        
  ---footer---        
        

                    
 -------------------------------            
 set @itemsfooter= ''                    
                    
    while @currentrow =  @totalrows                      
    begin                     
        select                     
  @Quantity=Quantity,                    
  @UnitPrice=UnitPrice,                    
  @DiscountAmount = DiscountAmount,                    
  @TaxableAmount= TaxableAmount,                    
  @VATAmount = VATAmount,                    
  @LineAmountInclusiveVAT = LineAmountInclusiveVAT                     
  from #itemsfooter                     
        set @currentrow = @currentrow +1                 
  set @itemsfooterstyle = 'font-weight: bold;'      
        set @itemsfooter=@itemsfooter+' '+@rowHtml                    
        set @itemRows=@itemRows+' '+@rowHtml       
    set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@Slno','Total')    
        set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@Id',' ')                    
  set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@invoiceDate',' ')                    
        set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@invoiceNumber',' ')                    
    set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@DescriptionArabic','')                  
  set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@Description',' ')      
  set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@exemption',' ')  
  if(UPPER(@tenancyname) LIKE 'BRADY%')
			  begin
			    set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@Quantity','')       
				set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@ProfQuantity','')
			  end
			  else
			  begin
					set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@Quantity',@Quantity)       
					set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@ProfQuantity',@Quantity)			 
			end
  --      set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@Quantity',@Quantity)       
  --set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@ProfQuantity',@Quantity)      
        set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@UnitPrice','')                    
    set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@unit',' ')                    
                    
        set @itemRows = [dbo].ReplaceHtmlDecimal(@itemRows,'@DiscountAmount',@DiscountAmount/@exchangeRate)                    
        set @itemRows = [dbo].ReplaceHtmlDecimal(@itemRows,'@TaxableAmount',@TaxableAmount/@exchangeRate)                    
        set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@VATRate',' ')                    
        set @itemRows = [dbo].ReplaceHtmlDecimal(@itemRows,'@VATAmount',@VATAmount/@exchangeRate)             
          set @itemRows = [dbo].ReplaceHtmlDecimal(@itemRows,'@LineAmountInclusiveVATCur',@LineAmountInclusiveVAT/@exchangeRate)                    
        
        set @itemRows = [dbo].ReplaceHtmlDecimal(@itemRows,'@LineAmountInclusiveVAT',@LineAmountInclusiveVAT*@exchangeRate)      
   set @itemRows = [dbo].ReplaceHtmlString(@itemRows,'@itemsfooterstyle',@itemsfooterstyle)      
                      
    end                      
      
  declare @otherchargecount int = (select count(*) from #items where isOtherCharges=1)      
     declare @showother nvarchar(50)=''      
  if(@otherchargecount = 0)      
  begin      
   set @showother='display : none'      
   set @totalotherVATcharges = 0.00      
   set @totalothercharges = 0.00      
      
     end      
  else      
  begin      
    set @totalothercharges = (select sum(UnitPrice) from #items where isOtherCharges=1)      
    if(@totalVat <> '0.00')      
     begin      
      set @totalotherVATcharges = @totalothercharges * 0.15      
      end      
    else      
     begin      
       set @totalotherVATcharges = 0.00      
      end      
    end      
      
 --charge footer--      
              
    while @currentchargerow - 1 =  @totalchargerows         
    begin        
   set @chargefooterstyle= 'font-weight: bold;'                    
  set @chargesRows=@chargesRows+' '+ @chargesrow      
          set @chargesRows = [dbo].ReplaceHtmlString(@chargesRows,'@chargename','Total Other Charges')                    
    set @chargesRows = [dbo].ReplaceHtmlString(@chargesRows,'@chargedescription','')            
      set @chargesRows = [dbo].ReplaceHtmlDecimal(@chargesRows,'@chargeAmount',@totalothercharges)      
    set @chargesRows = [dbo].ReplaceHtmlString(@chargesRows,'@chargefooterstyle',@chargefooterstyle)        
     set @currentchargerow = @currentchargerow +1      
    end                      
                    
set @html = [dbo].ReplaceHtmlString(@html,'@referenceNumber', @referenceNumber)         
set @html = [dbo].ReplaceHtmlString(@html,'@invoiceCurrencyCode', @invoiceCurrencyCode)                
set @html = [dbo].ReplaceHtmlString(@html,'@invoiceNumber', @invoiceNumber)                    
--set @html = [dbo].ReplaceHtmlString(@html,'@invoiceDate',@invoiceDate)  
set @html = [dbo].ReplaceHtmlString(@html,'@invoiceDate',format(cast(@invoiceDate as date) ,'dd-MM-yyyy'))  --SAGCO 
set @html = [dbo].ReplaceHtmlString(@html,'@invoiceType',@invoiceType)                    
set @html = [dbo].ReplaceHtmlString(@html,'@headerEnglish',@headerEnglish)                    
set @html = [dbo].ReplaceHtmlString(@html,'@headerArabic',@headerArabic)                    
set @html = [dbo].ReplaceHtmlString(@html,'@vendorName',@vendorName)                    
set @html = [dbo].ReplaceHtmlString(@html,'@vendorAddress',@vendorAddress)     
set @html = [dbo].ReplaceHtmlString(@html,'@faxNo',@faxNo)     
set @html = [dbo].ReplaceHtmlString(@html,'@website',@website)     
set @html = [dbo].ReplaceHtmlString(@html,'@vendorVatId',@vendorVatId)                    
set @html = [dbo].ReplaceHtmlString(@html,'@vendorCrNumber',@vendorCrNumber)                    
set @html = [dbo].ReplaceHtmlString(@html,'@customerName',@customerName)  --@customerName                  
set @html = [dbo].ReplaceHtmlString(@html,'@customerAddress',@customerAddress)                    
set @html = [dbo].ReplaceHtmlString(@html,'@customerContactNumber',@customerContactNumber)                
set @html = [dbo].ReplaceHtmlString(@html,'@customerVatId',@customerVatId)                    
set @html = [dbo].ReplaceHtmlString(@html,'@customerEmail',@customerEmail)                    
set @html = [dbo].ReplaceHtmlString(@html,'@customerCrNumber',@customerCrNumber)       	
set @html = [dbo].ReplaceHtmlString(@html,'@InvcDueDate',format(cast(@invoiceDueDate as date),'dd-MM-yyyy'))  --SAGCO   
set @html = [dbo].ReplaceHtmlString(@html,'@customerPo',@customerPo)    
        
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalBeforeVatCur',@totalBeforeVat/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalDiscountCur',@totalDiscount/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@finalTotalBeforeDiscountCur',@finalTotalBeforeDiscount/ @exchangeRate + @totalothercharges)        
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalotherchargesCur',@totalothercharges / @exchangeRate)      
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalVatCur', @totalVat  /@exchangeRate + @totalotherVATcharges)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalWithVatCur', @totalWithVat /@exchangeRate + @totalotherVATcharges + @totalothercharges   )                   
set @html = [dbo].ReplaceHtmlDecimal(@html,'@dueBalanceCur',@dueBalance/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxableAmountStdCur',@taxableAmountStd/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxAmountStdCur',@taxAmountStd/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalAmountStdCur',@totalAmountStd/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxableAmountZeroCur',@taxableAmountZero/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxAmountZeroCur',@taxAmountZero/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalAmountZeroCur',@totalAmountZero/ @exchangeRate)       
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxableAmountExmptCur',@taxableAmountExmpt/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxAmountExmptCur',@taxAmountExmpt/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalAmountExmptCur',@totalAmountExmpt/ @exchangeRate)      
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxableAmountOutCur',@taxableAmountOut/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxAmountOutCur',@taxAmountOut/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalAmountOutCur',@totalAmountOut/ @exchangeRate)      
    
    
    
set @html = [dbo].ReplaceHtmlString(@html,'@accountName',@accountName)      
set @html = [dbo].ReplaceHtmlString(@html,'@accountNumber',@accountNumber)      
set @html = [dbo].ReplaceHtmlString(@html,'@iban',@iban)      
set @html = [dbo].ReplaceHtmlString(@html,'@nameofbank',@nameofbank)      
set @html = [dbo].ReplaceHtmlString(@html,'@swiftcode',@swiftcode)      
set @html = [dbo].ReplaceHtmlString(@html,'@branchName',@branchName)     
set @html = [dbo].ReplaceHtmlString(@html,'@branchAddress',@branchAddress)    
    
    
                    
set @html = [dbo].ReplaceHtmlString(@html,'@itemRows',@itemRows)         
set @html = [dbo].ReplaceHtmlString(@html,'@Chargesrow',@chargesRows)       
set @html = [dbo].ReplaceHtmlString(@html,'@itemsfooter',@itemsfooter)                    
set @html = [dbo].ReplaceHtmlString(@html,'@notes',@notes)     
--exchangeRate and customerId added on 14/08/2023    
set @html = [dbo].ReplaceHtmlString(@html,'@exchangerate',@exchangeRate)     --SAGCO
--set @html = [dbo].ReplaceHtmlDecimal(@html,'@customerId',@customerId)    
set @html = [dbo].ReplaceHtmlString(@html,'@customerId',@customerId) --SAGCO
     
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalBeforeVat',@totalBeforeVat)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalDiscount',@totalDiscount)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@finalTotalBeforeDiscount',@finalTotalBeforeDiscount + @totalothercharges)        
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalothercharges',@totalothercharges)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalVat',@totalVat + @totalotherVATcharges)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalWithVat',@totalWithVat + @totalotherVATcharges + @totalothercharges)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@dueBalance',@dueBalance)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxableAmountStd',@taxableAmountStd /@exchangeRate + @totalothercharges )                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxAmountStd',@taxAmountStd + @totalotherVATcharges * @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalAmountStd',@totalAmountStd)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxableAmountZero',@taxableAmountZero/@exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxAmountZero',@taxAmountZero)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalAmountZero',@totalAmountZero + @totalothercharges)      
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxableAmountExmpt',@taxableAmountExmpt/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxAmountExmpt',@taxAmountExmpt/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalAmountExmpt',@totalAmountExmpt/ @exchangeRate)      
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxableAmountOut',@taxableAmountOut/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@taxAmountOut',@taxAmountOut/ @exchangeRate)                    
set @html = [dbo].ReplaceHtmlDecimal(@html,'@totalAmountOut',@totalAmountOut/ @exchangeRate)      
         
        
set @html = [dbo].ReplaceHtmlString(@html,'@currency2',@currency2)                    
set @html = [dbo].ReplaceHtmlString(@html,'@showCur',@showCur)       
set @html = [dbo].ReplaceHtmlString(@html,'@show1',@show1)       
set @html = [dbo].ReplaceHtmlString(@html,'@showCustom',@showCustom)       
set @html = [dbo].ReplaceHtmlString(@html,'@showother',@showother)  
set @html = [dbo].ReplaceHtmlString(@html,'@isQr',@isqrCodedisplay) 
set @html = [dbo].ReplaceHtmlString(@html,'@isQr1',@isqrCodedisplay1)  
      
set @html = [dbo].ReplaceHtmlString(@html,'@showFreight',@showFreight)      
set @html = [dbo].ReplaceHtmlString(@html,'@showTotal',@showTotal)                    



set @html = [dbo].ReplaceHtmlString(@html,'@lang_customerAddress',@lang_customerAddress)                    
set @html = [dbo].ReplaceHtmlString(@html,'@lang_customerContactNumber',@lang_customerContactNumber)                
set @html = [dbo].ReplaceHtmlString(@html,'@lang_customerEmail',@lang_customerEmail)                    
set @html = [dbo].ReplaceHtmlString(@html,'@lang_billtoattention',@lang_billtoattention)                    
set @html = [dbo].ReplaceHtmlString(@html,'@lang_customerName',@lang_customerName)                    
set @html = [dbo].ReplaceHtmlString(@html,'@lang_customerVatId',@lang_customerVatId)                    
set @html = [dbo].ReplaceHtmlString(@html,'@lang_vendoraddress',@lang_vendoraddress)                    
set @html = [dbo].ReplaceHtmlString(@html,'@lang_vendorName',@lang_vendorName)
set @html = [dbo].ReplaceHtmlString(@html,'@designation','')                    
set @html = [dbo].ReplaceHtmlString(@html,'@lang_designation','')                    



set @html = [dbo].ReplaceHtmlString(@html,'@billtoattention', @billtoattention)                
set @html = [dbo].ReplaceHtmlString(@html,'@shiptoaddress', @shiptoaddress)                
set @html = [dbo].ReplaceHtmlString(@html,'@phone', @phone)                
set @html = [dbo].ReplaceHtmlString(@html,'@shiptoaccount', @shiptoaccount)                
set @html = [dbo].ReplaceHtmlString(@html,'@shiptoatn', @shiptoatn)                
set @html = [dbo].ReplaceHtmlString(@html,'@bank', @bank)       
set @html = [dbo].ReplaceHtmlString(@html,'@originalorderno', @originalorderno)       
set @html = [dbo].ReplaceHtmlString(@html,'@referenceinvoicenoPO', @referenceinvoicenoPO)      
set @html = [dbo].ReplaceHtmlString(@html,'@invoiceRefDate', @invoiceRefDate)      
set @html = [dbo].ReplaceHtmlString(@html,'@deliveryDatee', @deliveryDatee)                
      
      
      
set @html = [dbo].ReplaceHtmlString(@html,'@referenceinvoiceno', @referenceinvoiceno)                
set @html = [dbo].ReplaceHtmlString(@html,'@invoicereferencedate', '')        
set @html = [dbo].ReplaceHtmlString(@html,'@dueDate', @invoicereferencedate)      
set @html = [dbo].ReplaceHtmlString(@html,'@yourvendor', @yourvendor)                
set @html = [dbo].ReplaceHtmlString(@html,'@termsofdelivery', @termsofdelivery)                
set @html = [dbo].ReplaceHtmlString(@html,'@originalquote', @originalquote)                
set @html = [dbo].ReplaceHtmlString(@html,'@paymentterms', @paymentterms)                
set @html = [dbo].ReplaceHtmlString(@html,'@orderplacedby', @orderplacedby)                
set @html = [dbo].ReplaceHtmlString(@html,'@ourorderref', @ourorderref)       
set @html = [dbo].ReplaceHtmlString(@html,'@deliverydate', format(getdate(),'dd-MM-yyyy'))            
set @html = [dbo].ReplaceHtmlString(@html,'@invoiceduedate', @deliverydate)      
set @html = [dbo].ReplaceHtmlString(@html,'@delivery', @delivery)                
set @html = [dbo].ReplaceHtmlString(@html,'@carrierandservices', @carrierandservices)                
set @html = [dbo].ReplaceHtmlString(@html,'@customer2',concat(Concat(@customerId,'-'), @customer2))                
set @html = [dbo].ReplaceHtmlString(@html,'@yourvatnumber', @yourvatnumber)                
set @html = [dbo].ReplaceHtmlString(@html,'@freightCur', cast(@freight/@exchangeRate as decimal(15,2)))                
set @html = [dbo].ReplaceHtmlString(@html,'@custom1Cur', cast(@custom1/@exchangeRate as decimal(15,2)))        
set @html = [dbo].ReplaceHtmlString(@html,'@freight1', cast(@freight as decimal(15,2)))                
set @html = [dbo].ReplaceHtmlString(@html,'@custom3', cast(@custom1 as decimal(15,2)))        
set @html = [dbo].ReplaceHtmlString(@html,'@invoicetotalCur', cast(@invoicetotal/@exchangeRate as decimal(15,2)))         
        
set @html = [dbo].ReplaceHtmlString(@html,'@invoicetotal12', cast(@invoicetotal as decimal(15,2)))           
set @html = [dbo].ReplaceHtmlString(@html,'@desc1', @desc1)               
set @html = [dbo].ReplaceHtmlString(@html,'@desc2', @desc2)     



select @html as html, @orientation as orientation;            
--- select @Quantity;      
--select @accountName    
end
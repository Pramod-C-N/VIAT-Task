USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[InsertBatchUploadPayment]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE       
  PROCEDURE [dbo].[InsertBatchUploadPayment] (    
  @json nvarchar(max),     
  @fileName nvarchar(max),     
  @tenantId int = null,  
   @fromdate DateTime=null,    
 @todate datetime=null  
) AS BEGIN Declare @MaxBatchId int     
Select     
  @MaxBatchId = max(batchId)     
from     
  BatchData;    
Declare @batchId int = @MaxBatchId + 1;    
INSERT INTO [dbo].[BatchData] (    
  [TenantId], [BatchId], [FileName],     
  [TotalRecords], [Status], [Type],[fromDate],[toDate],     
  [CreationTime], [IsDeleted]    
)     
VALUES     
  (    
    @tenantId,     
    @batchId,     
    @fileName,     
    0,     
    'Unprocessed',     
    'Payment',     
 @fromdate,  
 @todate,  
    GETDATE(),     
    0    
  ) Insert into dbo.logs     
values     
  (    
    @json,     
    getdate(),     
    @batchId    
  ) Declare @totalRecords int =(    
    select     
      count(*)     
    from     
      OPENJSON(@json)    
  );    
IF (    
  ISJSON(@json) = 1     
  and @totalRecords > 0    
) BEGIN PRINT 'Imported JSON is Valid';    
insert into ImportBatchData(    
  uniqueidentifier, Processed, WHTApplicable,     
  VATDeffered, RCMApplicable, Isapportionment,     
  InvoiceType,    
  BuyerName,     
  OrgType,     
  BuyerCountryCode,
  InvoiceNumber,
  IssueDate,     
  TotalTaxableAmount,     
  InvoiceCurrencyCode,    
  PaymentMeans,     
  ExchangeRate,     
  LineAmountInclusiveVAT,     
  LedgerHeader,    
  NatureofServices,     
  ItemName,    
  PlaceofSupply,    
  AffiliationStatus,     
  PurchaseNumber,    
  SupplyEndDate,    
  ReferenceInvoiceAmount,     
  PaymentTerms,     
  PerCapitaHoldingForiegnCo,     
  CapitalInvestedbyForeignCompany,     
  CapitalInvestmentCurrency,    
  CapitalInvestmentDate,     
  VendorConstitution,    
  BatchId,     
  CreationTime,     
  CreatorUserId,     
  IsDeleted, TenantId,Filename    
)     
select     
  NEWID(),     
  0,     
  0,     
  case when upper(vatdeffered) like 'Y%'  then 1
       else 0 end as VATDeffered,     
  0,     
  0,     
  'WHT - ' + ISNULL(InvoiceType, '') as InvoiceType,     
  ISNULL(BuyerName, '') as BuyerName,     
  case when OrgType= ' ' or OrgType = Null then 'Private' 
  else ISNULL(OrgType, '') end as OrgType,     
  ISNULL(BuyerCountryCode, '') as BuyerCountryCode,
  ISNULL(InvoiceNumber, NEWID()) as InvoiceNumber,
  dbo.ISNULLOREMPTYFORDATE(IssueDate) as IssueDate,     
  dbo.ISNULLOREMPTYFORDECIMAL(TotalTaxableAmount) as TotalTaxableAmount,     
  ISNULL(InvoiceCurrencyCode, '') as InvoiceCurrencyCode,     
  ISNULL(PaymentMeans, '') as PaymentMeans,     
  dbo.ISNULLOREMPTYFORDECIMAL(ExchangeRate) as ExchangeRate,     
  dbo.ISNULLOREMPTYFORDECIMAL(LineAmountInclusiveVAT) as LineAmountInclusiveVAT,     
  ISNULL(LedgerHeader, '') as LedgerHeader,     
  ISNULL(NatureofServices, '') as NatureofServices,     
  ISNULL(ItemName, '') as ItemName,     
  case when upper(PlaceofSupply) = 'INSIDE' then 'Inside Country' 
  when upper(PlaceofSupply) = 'INSIDE KSA' then 'Inside Country' 
  when upper(PlaceofSupply) = 'OUTSIDE' then 'Outside Country'
  when upper(PlaceofSupply) = 'OUTSIDE KSA' then 'Outside Country'
  when upper(PlaceofSupply) = 'N/A' or upper(PlaceofSupply) = 'N.A.' or upper(PlaceofSupply) = 'NA' or upper(PlaceofSupply) = 'N.A' then 'Inside Country'
  else ISNULL(PlaceofSupply, '') end as PlaceofSupply,     
  case when upper(AffiliationStatus) = 'YES' then 'Affiliate'
  when upper(AffiliationStatus) = 'NO' then 'Non-affiliate'
  else ISNULL(AffiliationStatus, '') end as AffiliationStatus,     
  ISNULL(PurchaseNumber, '') as PurchaseNumber,     
  dbo.ISNULLOREMPTYFORDATE(SupplyEndDate) as SupplyEndDate,     
  dbo.ISNULLOREMPTYFORDECIMAL(ReferenceInvoiceAmount) as ReferenceInvoiceAmount,     
  ISNULL(PaymentTerms, '') as PaymentTerms,     
  ISNULL(PerCapitaHoldingForiegnCo, '') as PerCapitaHoldingForiegnCo,     
  ISNULL(    
    CapitalInvestedbyForeignCompany,     
    ''    
  ) as CapitalInvestedbyForeignCompany,     
  ISNULL(CapitalInvestmentCurrency, '') as CapitalInvestmentCurrency,     
  dbo.ISNULLOREMPTYFORDATE(CapitalInvestmentDate) as CapitalInvestmentDate,     
  ISNULL(VendorConstitution, '') as VendorConstitution,     
  @batchId,     
  GETDATE(),     
  1,     
  0,     
  @tenantId ,
  @fileName
from     
  OPENJSON(@json) with (
    VATDeffered nvarchar(max) '$."Obtained required documents"',
    InvoiceType nvarchar(max) '$."Payment Type"',     
    [BuyerName] nvarchar(max) '$."Vendor Name"',     
    OrgType nvarchar(max) '$."Vendor Type"',     
    [BuyerCountryCode] nvarchar(max) '$."Vendor country"',
	 [InvoiceNumber] nvarchar(max) '$."PV JV Number"' ,
    [IssueDate] nvarchar(max) '$."Payment Date"',     
    TotalTaxableAmount nvarchar(max) '$."Amount paid (charge)"',     
    [InvoiceCurrencyCode] nvarchar(max) '$."CCY"',     
    PaymentMeans nvarchar(max) '$."Payment Mode"',     
    ExchangeRate nvarchar(max) '$."Exchange rate"',     
    LineAmountInclusiveVAT nvarchar(max) '$."Amount in Saudi Riyal"',     
    LedgerHeader nvarchar(max) '$."GL Head"',     
    NatureofServices nvarchar(max) '$."Nature of Service"',     
    [ItemName] nvarchar(max) '$."Brief nature of payment (2 or 3 words)"',     
    PlaceofSupply nvarchar(max) '$."Place of performance of services"',     
    AffiliationStatus nvarchar(max) '$."Affiliation status"',     
    PurchaseNumber nvarchar(max) '$."Reference Invoice Number"',     
    SupplyEndDate nvarchar(max) '$."Reference Invoice Date"',     
    ReferenceInvoiceAmount nvarchar(max) '$."Reference Invoice Amount"',     
    PaymentTerms nvarchar(max) '$."Payment Purpose"',     
    PerCapitaHoldingForiegnCo nvarchar(max) '$."% of Capital holding by Foreign Co"',     
    CapitalInvestedbyForeignCompany nvarchar(max) '$."Capital Invested by Foreign Company"',     
    CapitalInvestmentCurrency nvarchar(max) '$."Capital Investment Currency"',     
    CapitalInvestmentDate nvarchar(max) '$."Capital Investment Date"',     
    VendorConstitution nvarchar(max) '$."Vendor Costitution"'    
  );    
update     
  BatchData     
set     
  TotalRecords = @totalRecords,     
  SuccessRecords = @totalRecords,     
  FailedRecords = 0,     
  status = 'Processed',     
  batchid = @batchId     
where     
  FileName = @fileName     
  and Status = 'Unprocessed';    
END ELSE BEGIN PRINT 'Invalid JSON Imported' END END   

begin
exec PaymentTransValidation @batchId
end
GO

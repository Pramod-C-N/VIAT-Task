USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[InsertBatchUploadDebitPurchase]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  
CREATE            
    PROCEDURE [dbo].[InsertBatchUploadDebitPurchase] (  
    @json nvarchar(max)   ,  
 @fileName nvarchar(max),  
    @tenantId int = null,
	 @fromdate DateTime=null,  
 @todate datetime=null
  ) AS BEGIN   
  Declare @MaxBatchId int  
Select @MaxBatchId=max(batchId) from BatchData;  
    
Declare @batchId int = @MaxBatchId + 1;   
  INSERT INTO [dbo].[BatchData] (  
    [TenantId], [BatchId], [FileName],   
    [TotalRecords], [Status],[Type],[fromDate],[toDate], [CreationTime],   
    [IsDeleted]  
  )   
VALUES   
  (  
    @tenantId,   
    @batchId,   
    @fileName,   
    0,   
    'Unprocessed',
	'Debit-Purchase',
	@fromdate,
	@todate,
    GETDATE(),   
    0  
  )   
Insert into dbo.logs  
values(@json,getdate(),@batchId)  
Declare @totalRecords int =(  
  select   
    count(*)   
  from   
    OPENJSON(@json)  
);  
IF (  
  ISJSON(@json) = 1 and @totalRecords>0  
) BEGIN PRINT 'Imported JSON is Valid';  
insert into ImportBatchData(  
  uniqueidentifier,Processed,Isapportionment,  
    InvoiceType,  
    IRNNo,  
    InvoiceNumber,  
    IssueDate,  
    IssueTime,  
    InvoiceCurrencyCode,  
    BillingReferenceId,  
    OrignalSupplyDate,
	SupplyDate,

    ReasonForCN,  
    PurchaseOrderId,  
    ContractId,  
    BuyerMasterCode,  
    BuyerName,  
    BuyerVatCode,  
    BuyerContact,  
    BuyerCountryCode,  
    OrgType,  
    InvoiceLineIdentifier,  
    ItemMasterCode,  
    ItemName,  
    UOM,  
    GrossPrice,  
    Discount,  
    NetPrice,  
    Quantity,  
    LineNetAmount,  
    VatCategoryCode,  
    VatRate,  
    VatExemptionReasonCode,  
    VatExemptionReason,  
    VATLineAmount,  
    LineAmountInclusiveVAT,  
    BillOfEntry,  
   BillOfEntryDate,  
   CustomsPaid,  
   ExciseTaxPaid,  
   OtherChargesPaid,  
   TotalTaxableAmount,  
   VATDeffered,  
   PlaceofSupply,  
   RCMApplicable,  
   WHTApplicable,  
   PaymentMeans,  
    BatchId,  
    CreationTime,  
    CreatorUserId,  
    IsDeleted,  
    TenantId,  
    TransType ,
	PurchaseCategory,
	LedgerHeader,
	NatureofServices,
	[Filename]

--	orgtype,
--	PaymentMeans 
    )  
select   
    NEWID(),0,0, 
	case when InvoiceType is null then 'DN Purchase-STANDARD' when InvoiceType = '' then 'DN Purchase-STANDARD' else 'DN Purchase-' + InvoiceType end,
          --'DN Purchase - '+ ISNULL(InvoiceType,'') as InvoiceType,  
  IRNNo,  
  ISNULL(InvoiceNumber,'') as InvoiceNumber,  
  dbo.ISNULLOREMPTYFORDATE(IssueDate) as IssueDate,  
  ISNULL(IssueTime,'') as IssueTime,  
  ISNULL(InvoiceCurrencyCode,'') as InvoiceCurrencyCode,  
  ISNULL(BillingReferenceId,'') as BillingReferenceId,  
  dbo.ISNULLOREMPTYFORDATE(OrignalSupplyDate) as OrignalSupplyDate,  
  dbo.ISNULLOREMPTYFORDATE(SupplyDate) as SupplyDate, 
ISNULL(ReasonForCN,'') as ReasonForCN,  
ISNULL(PurchaseOrderId,'') as PurchaseOrderId,  
  ISNULL(ContractId,'') as ContractId,  
  ISNULL(BuyerMasterCode,'') as BuyerMasterCode,  
  ISNULL(BuyerName,'') as BuyerName,  
  ISNULL(BuyerVatCode,'') as BuyerVatCode,  
  ISNULL(BuyerContact,'') as BuyerContact,  
  ISNULL(BuyerCountryCode,'') as BuyerCountryCode,  
  isnull(BuyerType,'PRIVATE') as BuyerType,  
  ISNULL(InvoiceLineIdentifier,'') as InvoiceLineIdentifier,  
  ISNULL(ItemMasterCode,'') as ItemMasterCode,  
  ISNULL(ItemName,'') as ItemName,  
  ISNULL(UOM,'') as UOM,  
  dbo.ISNULLOREMPTYFORDECIMAL(GrossPrice) as GrossPrice,  
  dbo.ISNULLOREMPTYFORDECIMAL(Discount) as Discount,  
  dbo.ISNULLOREMPTYFORDECIMAL(NetPrice) as NetPrice,  
  dbo.ISNULLOREMPTYFORDECIMAL(Quantity) as Quantity,  
  dbo.ISNULLOREMPTYFORDECIMAL(LineNetAmount) as LineNetAmount,  
  ISNULL(VatCategoryCode,'') as VatCategoryCode,  
  dbo.ISNULLOREMPTYFORDECIMAL(VatRate) as VatRate,  
  ISNULL(VatExemptionReasonCode,'') as VatExemptionReasonCode,  
  ISNULL(VatExemptionReason,'') as VatExemptionReason,  
  dbo.ISNULLOREMPTYFORDECIMAL(VATLineAmount) as VATLineAmount,  
  dbo.ISNULLOREMPTYFORDECIMAL(LineAmountInclusiveVAT) as LineAmountInclusiveVAT,  
  ISNULL(BillofEntry,'') as BillofEntry,  
  dbo.ISNULLOREMPTYFORDATE(BillofEntryAWBdate) as BillofEntryAWBdate,  
  dbo.ISNULLOREMPTYFORDECIMAL(CustomsPaid) as CustomsPaid,  
  dbo.ISNULLOREMPTYFORDECIMAL(ExciseTaxPaid) as ExciseTaxPaid,  
  dbo.ISNULLOREMPTYFORDECIMAL(OtherChargesPaid) as OtherChargesPaid,  
  dbo.ISNULLOREMPTYFORDECIMAL(TotalTaxableAmt) as TotalTaxableAmt,  
  case when upper(InvoiceType) like '%IMPORT%' and upper(purchasecategory) like 'SERVICE%' and VATdeffered not like 'N%' then 0
       when upper(InvoiceType) like '%IMPORT%' and upper(purchasecategory) like 'GOOD%' and VATdeffered like 'Y%' then 1
       else 0 end as VATDeffered,  
  ISNULL(PlaceofSupply,'') as PlaceofSupply,  
  --dbo.ISNULLOREMPTYFORBITRETURN0(RCMApplicable) as RCMApplicable,
  case when upper(InvoiceType) like '%IMPORT%' and upper(purchasecategory) not like 'GOOD%' and rcmapplicable not like 'N%' then 0
       when upper(InvoiceType) like '%IMPORT%' and upper(purchasecategory) like 'SERVICE%' and rcmapplicable like 'Y%' then 1
       else 0 end as RCMApplicable,
  dbo.ISNULLOREMPTYFORBIT(WHTApplicable) as WHTApplicable,  
  isnull(supplytype,'ECONOMIC'),  
  @batchId,  
  GETDATE(),  
  1,  
  0,  
  @tenantId,  
  'Purchases' as Transtype,
  ISNULL(PurchaseCategory, '') as PurchaseCategory,
  isnull(LedgerHead,' ') as LedgerHead,
  ISNULL(NatureofService, '') as NatureofServices,
  @fileName
--  isnull(supptype, 'PRIVATE'),   
--  isnull(supplytype, 'ECONOMIC')
from   
  OPENJSON(@json) with (  
InvoiceType nvarchar(max) '$."Debit Note Type"',  
IRNNo nvarchar(max) '$."IRN Number"',  
InvoiceNumber nvarchar(max) '$."Debit Note Number *"',  
IssueDate nvarchar(max) '$."Debit Note Issue Date *"',  
IssueTime nvarchar(max) '$."Debit Note Issue Time *"',  
InvoiceCurrencyCode nvarchar(max) '$."Debit Note currency code *"',  
BillingReferenceId nvarchar(max) '$."Billing Refrence Id"',  
OrignalSupplyDate nvarchar(max) '$."Original Purchase  Date*"',  
SupplyDate nvarchar(max) '$."Original issue  Date*"',

ReasonForCN nvarchar(max) '$."Reason for Issuance of Debit Note"',  
PurchaseOrderId nvarchar(max) '$."Purchase Order ID"',  
ContractId nvarchar(max) '$."Contract ID"',  
BuyerMasterCode nvarchar(max) '$."Supplier Master Code"',  
BuyerName nvarchar(max) '$."Supplier Name"',  
BuyerVatCode nvarchar(max) '$."Supplier VAT number"',  
BuyerContact nvarchar(max) '$."Supplier Contact"',  
BuyerCountryCode nvarchar(max) '$."Supplier Country Code"',  
BuyerType nvarchar(max) '$."Supplier Type"',  
InvoiceLineIdentifier nvarchar(max) '$."Debit Note line identifier *"',  
ItemMasterCode nvarchar(max) '$."Item Master Code"',  
ItemName nvarchar(max) '$."Item name"',  
UOM nvarchar(max) '$." quantity unit of measure"',  
GrossPrice nvarchar(max)'$."Item gross price"',  
Discount nvarchar(max) '$."Item price discount"',  
NetPrice [decimal](18,2) '$."Item net price*"',  
Quantity nvarchar(max) '$."Debit Note quantity"',  
LineNetAmount  nvarchar(max) '$."Debit Note line net amount"',  
VatCategoryCode nvarchar(max) '$."Debit Note item VAT category code*"',  
VatRate nvarchar(max) '$."Debit Note item VAT rate*"',  
VatExemptionReasonCode nvarchar(max) '$."VAT exemption reason code"',  
VatExemptionReason nvarchar(max) '$."VAT exemption reason"',  
VATLineAmount  nvarchar(max) '$."VAT line amount*"',  
LineAmountInclusiveVAT  nvarchar(max) '$."Line amount inclusive VAT*"',  
BillofEntry nvarchar(max) '$."Bill of Entry/ Airway Bill No"',  
BillofEntryAWBdate nvarchar(max) '$."Bill of Entry / AWB date"',  
CustomsPaid nvarchar(max) '$."Customs Paid"',  
ExciseTaxPaid nvarchar(max) '$."Excise Tax Paid"',  
OtherChargesPaid nvarchar(max) '$."Other Charges Paid"',  
TotalTaxableAmt nvarchar(max) '$."Total Taxable amount*"',  
VATDeffered nvarchar(max) '$."VAT Deffered"',  
PlaceofSupply nvarchar(max) '$."Place of Supply (Within KSA/ Outside KSA)"',  
RCMApplicable nvarchar(max) '$."RCM Applicable (Y/N)"',  
WHTApplicable nvarchar(max) '$."WHT Applicable (Y/N)"' ,  
PurchaseCategory nvarchar(max) '$."Purchase Category"',
supplytype nvarchar(max) '$."SupplyType"',
LedgerHead nvarchar(max) '$."Ledger Head"',
NatureofService nvarchar(max) '$."Nature of Service (Mandatory for WHT related transactions)"'
  );  
  
update   
  BatchData   
set   
  TotalRecords = @totalRecords,   
  SuccessRecords = @totalRecords,   
  FailedRecords = 0,   
  status = 'Processed',  
  batchid= @batchId  
where   
  FileName = @fileName   
  and Status = 'Unprocessed';  
END ELSE BEGIN PRINT 'Invalid JSON Imported' END END  
begin  
  
exec DebitNotePurchaseTransValidation @batchid   
  
end  


--select SupplyDate,OrignalSupplyDate from ImportBatchData where BatchId=210
--select * from logs where batchid=210
GO

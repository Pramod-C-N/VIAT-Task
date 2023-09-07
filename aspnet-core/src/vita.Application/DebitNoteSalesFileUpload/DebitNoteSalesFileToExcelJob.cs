using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.ObjectMapping;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using vita.Authorization.Roles;
using vita.Authorization.Users;
using vita.Authorization.Users.Dto;
using vita.Authorization.Users.Importing;
using vita.Authorization.Users.Importing.Dto;
using vita.Notifications;
using vita.CreditNoteFileUpload;
//using vita.CreditNoteFileUpload.Dtos;
using vita.CreditNoteFileUpload.Importing;
using vita.Storage;
//using vita.StandardFileUpload.Dtos;
using vita.StandardFileUpload;
using Abp.Runtime.Session;
using vita.ImportBatch.Exporting;
using vita.ImportBatch;
using vita.ImportBatch.Dtos;
using vita.ImportBatch.Importing;
using vita.Credit.Dtos;
using IInvalidInvoiceExporter = vita.CreditNoteFileUpload.Importing.IInvalidInvoiceExporter;
using vita.Credit;
using vita.Sales.Dtos;
using Newtonsoft.Json;
using vita.DebitNoteSalesFileUpload.Importing;
using vita.Debit;
using vita.Debit.Dtos;
using vita.Sales;

namespace vita.CreditNoteFileUpload
{
    public class ImportDebitNoteSalesFileToExcelJob : AsyncBackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly RoleManager _roleManager;
        private readonly IDebitNoteSalesListExcelDataReader _invoiceListExcelDataReader;
        private readonly IInvalidUserExporter _invalidUserExporter;
        private readonly IUserPolicy _userPolicy;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        //private readonly ICreditNoteFilesAppService _creditNoteFilesAppService;
        private readonly IImportBatchDatasAppService _importBatchDatasAppService;
        private readonly IInvalidInvoiceExporter _invalidInvoiceExporter;
        /// private readonly IImportStandardFilesAppService _importStandardFilesAppService;
        private readonly IAbpSession _session;
        private readonly IDebitNotesAppService _debitNotes;
        public readonly ISalesInvoicesAppService _sales;
        public UserManager UserManager { get; set; }

        public ImportDebitNoteSalesFileToExcelJob(
            RoleManager roleManager,
            IDebitNoteSalesListExcelDataReader invoiceListExcelDataReader,
            IInvalidUserExporter invalidUserExporter,
            IUserPolicy userPolicy,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            IObjectMapper objectMapper,
            IUnitOfWorkManager unitOfWorkManager,
            //   ICreditNoteFilesAppService creditNoteFilesAppService,
            IImportBatchDatasAppService importBatchDatasAppService,
            IInvalidInvoiceExporter invalidInvoiceExporter,
            IAbpSession session,
            ISalesInvoicesAppService sales,
        //    IImportStandardFilesAppService importStandardFilesAppService,
           IDebitNotesAppService debitNotes)
        {
            _roleManager = roleManager;
            _invoiceListExcelDataReader = invoiceListExcelDataReader;
            _invalidUserExporter = invalidUserExporter;
            _userPolicy = userPolicy;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
            //   _creditNoteFilesAppService = creditNoteFilesAppService;
            _importBatchDatasAppService = importBatchDatasAppService;
            _invalidInvoiceExporter = invalidInvoiceExporter;
            //  _importStandardFilesAppService = importStandardFilesAppService;
            _session = session;
            _debitNotes  = debitNotes;
            _sales = sales;
        }

        private class InvoiceModel
        {
            [JsonProperty(PropertyName = "Debit Note Type")]
            public string InvoiceType { get; set; }
            [JsonProperty(PropertyName = "TransType")]
            public string TransType { get; set; }
            [JsonProperty(PropertyName = "IRN Number")]
            public string IRNNumber { get; set; }
            [JsonProperty(PropertyName = "Debit Note Number *")]
            public string InvoiceNumber { get; set; }
            [JsonProperty(PropertyName = "Debit Note Issue Date *")]
            public string InvoiceIssueDate { get; set; }
            [JsonProperty(PropertyName = "Debit Note Issue Time *")]
            public string InvoiceIssueTime { get; set; }
            [JsonProperty(PropertyName = "Debit Note currency code *")]
            public string InvoiceCurrencyCode { get; set; }
            [JsonProperty(PropertyName = "Purchase Order ID")]
            public string PurchaseOrderID { get; set; }
            [JsonProperty(PropertyName = "Contract ID")]
            public string ContractID { get; set; }
            [JsonProperty(PropertyName = "Supply Date")]
            public string SupplyDate { get; set; }
            [JsonProperty(PropertyName = "Supply End Date")]
            public string SupplyEndDate { get; set; }
            [JsonProperty(PropertyName = "Buyer Master Code")]
            public string BuyerMasterCode { get; set; }
            [JsonProperty(PropertyName = "Buyer Name")]
            public string BuyerName { get; set; }
            [JsonProperty(PropertyName = "Buyer VAT number")]
            public string BuyerVATNumber { get; set; }
            [JsonProperty(PropertyName = "Buyer Contact")]
            public string BuyerContact { get; set; }
            [JsonProperty(PropertyName = "Buyer Country Code")]
            public string BuyerCountryCode { get; set; }
            [JsonProperty(PropertyName = "Debit Note line identifier *")]
            public string InvoiceLineIdentifier { get; set; }
            [JsonProperty(PropertyName = "Item Master Code")]
            public string ItemMasterCode { get; set; }
            [JsonProperty(PropertyName = "Item name")]
            public string ItemName { get; set; }
            [JsonProperty(PropertyName = "quantity unit of measure")]
            public string InvoicedQuantityUnitOfMeasure { get; set; }
            [JsonProperty(PropertyName = "Item gross price")]
            public string ItemGrossPrice { get; set; }
            [JsonProperty(PropertyName = "Item price discount")]
            public string ItemPriceDiscount { get; set; }
            [JsonProperty(PropertyName = "Item net price*")]
            public string ItemNetPrice { get; set; }
            [JsonProperty(PropertyName = "Debit Note quantity")]
            public string InvoicedQuantity { get; set; }
            [JsonProperty(PropertyName = "Debit Note line net amount")]
            public string InvoiceLineNetAmount { get; set; }
            [JsonProperty(PropertyName = "Debit Note item VAT category code*")]
            public string InvoicedItemVATCategoryCode { get; set; }
            [JsonProperty(PropertyName = "Debit Note item VAT rate*")]
            public string InvoicedItemVATRate { get; set; }
            [JsonProperty(PropertyName = "VAT exemption reason code")]
            public string VATExemptionReasonCode { get; set; }
            [JsonProperty(PropertyName = "VAT exemption reason")]
            public string VATExemptionReason { get; set; }
            [JsonProperty(PropertyName = "VAT line amount*")]
            public string VATLineAmount { get; set; }
            [JsonProperty(PropertyName = "Line amount inclusive VAT*")]
            public string LineAmountInclusiveVAT { get; set; }
            [JsonProperty(PropertyName = "Advance Rcpt Amount Adjusted")]
            public string AdvanceRcptAmountAdjusted { get; set; }
            [JsonProperty(PropertyName = "VAT on Advance Receipt Amount Adjusted")]
            public string VATOnAdvanceReceiptAmountAdjusted { get; set; }
            [JsonProperty(PropertyName = "Advance Receipt Reference Number")]
            public string AdvanceReceiptReferenceNumber { get; set; }
            [JsonProperty(PropertyName = "Payment Means")]
            public string PaymentMeans { get; set; }
            [JsonProperty(PropertyName = "Payment Terms")]
            public string PaymentTerms { get; set; }
            [JsonProperty(PropertyName = "Buyer Type")]
            public string BuyerType { get; set; }

            public string xml_uuid { get; set; }

        }

        class CompareInvNum : IComparer<InvoiceModel>
        {
            public int Compare(InvoiceModel x, InvoiceModel y)
            {
                int a = Convert.ToInt32(x.InvoiceNumber);
                int b = Convert.ToInt32(y.InvoiceNumber);
                if (a == 0 || b == 0)
                {
                    return 0;
                }

                // CompareTo() method
                return a.CompareTo(b);

            }
        }

        private List<CreateOrEditDebitNoteDto> mapJsonToModelDebit(string json)
        {
            List<CreateOrEditDebitNoteDto> output = new List<CreateOrEditDebitNoteDto>();
            try
            {
                List<InvoiceModel> invoiceModels = JsonConvert.DeserializeObject<List<InvoiceModel>>(json);
                invoiceModels.Sort(new CompareInvNum());
                for (var i = 0; i < invoiceModels.Count; i++)
                {
                    int invNum = Convert.ToInt32(invoiceModels[i].InvoiceNumber);
                    CreateOrEditDebitNoteDto model = new CreateOrEditDebitNoteDto()
                    {
                        Items = new List<CreateOrEditDebitNoteItemDto>(),
                        IssueDate = DateTime.ParseExact(invoiceModels[i].InvoiceIssueDate, "MM/dd/yyyy", null),
                        InvoiceNumber = invoiceModels[i].InvoiceNumber,
                        Supplier = new CreateOrEditDebitNotePartyDto(),
                        Buyer = new CreateOrEditDebitNotePartyDto(),
                        InvoiceSummary = new CreateOrEditDebitNoteSummaryDto(),
                        Additional_Info = invoiceModels[i].xml_uuid
                    };
                    bool flag = false;
                    model.Supplier.Address = new CreateOrEditDebitNoteAddressDto();
                    model.Buyer.Address = new CreateOrEditDebitNoteAddressDto();

                    model.Supplier.RegistrationName = "Saudi Arabian Glass Co Ltd";
                    model.Supplier.VATID = "300166057500003";
                    model.Supplier.CustomerId = "";
                    model.Supplier.Address.BuildingNo = "1234";
                    model.Supplier.Address.AdditionalNo = "";
                    model.Supplier.Address.Street = "Industrial Areas";
                    model.Supplier.Address.AdditionalStreet = "Phase 4";
                    model.Supplier.Address.Neighbourhood = "Jeddah";
                    model.Supplier.Address.State = "Jeddah";
                    model.Supplier.Address.PostalCode = "21494";
                    model.Supplier.Address.City = "Jeddah";
                    model.Supplier.Address.CountryCode = "SA";

                    model.Buyer.RegistrationName = invoiceModels[i].BuyerName;
                    model.Buyer.VATID = invoiceModels[i].BuyerVATNumber;
                    model.Buyer.CustomerId = "";
                    model.Buyer.Address.BuildingNo = "3012";
                    model.Buyer.Address.AdditionalNo = "";
                    model.Buyer.Address.Street = "Makhrooj";
                    model.Buyer.Address.AdditionalStreet = "Al Fayaz Road";
                    model.Buyer.Address.Neighbourhood = "Riyadh";
                    model.Buyer.Address.State = "Riyadh";
                    model.Buyer.Address.City = "Riyadh";
                    model.Buyer.Address.PostalCode = "11585";
                    model.Buyer.Address.CountryCode = "SA";


                    while (i < invoiceModels.Count && Convert.ToInt32(invoiceModels[i].InvoiceNumber) == invNum)
                    {
                        model.Items.Add(new CreateOrEditDebitNoteItemDto()
                        {
                            Name = invoiceModels[i].ItemName,
                            DiscountAmount = Convert.ToDecimal(invoiceModels[i].ItemPriceDiscount),
                            UnitPrice = Convert.ToDecimal(invoiceModels[i].ItemGrossPrice),
                            Quantity = Convert.ToDecimal(invoiceModels[i].InvoicedQuantity),
                            UOM = invoiceModels[i].InvoicedQuantityUnitOfMeasure,
                            VATAmount = Convert.ToDecimal(invoiceModels[i].VATLineAmount),
                            LineAmountInclusiveVAT = Convert.ToDecimal(invoiceModels[i].LineAmountInclusiveVAT),
                            VATRate = Convert.ToDecimal(invoiceModels[i].InvoicedItemVATRate),
                            Identifier = invoiceModels[i].PurchaseOrderID,
                            Description = invoiceModels[i].ItemName,
                            GrossPrice = Convert.ToDecimal(invoiceModels[i].ItemGrossPrice),
                            NetPrice = Convert.ToDecimal(invoiceModels[i].ItemGrossPrice) - Convert.ToDecimal(invoiceModels[i].ItemPriceDiscount),

                        });

                        i++;
                        flag = true;
                    }


                    if (flag)
                        i--;

                    model.InvoiceSummary.NetInvoiceAmount = invoiceModels.Sum(p => Convert.ToDecimal(p.ItemNetPrice));
                    model.InvoiceSummary.SumOfInvoiceLineNetAmount = invoiceModels.Sum(p => Convert.ToDecimal(p.InvoiceLineNetAmount));
                    model.InvoiceSummary.TotalAmountWithoutVAT = invoiceModels.Sum(p => Convert.ToDecimal(p.ItemNetPrice));
                    model.InvoiceSummary.TotalAmountWithVAT = invoiceModels.Sum(p => Convert.ToDecimal(p.LineAmountInclusiveVAT));

                    output.Add(model);

                }

                return output;
            }
            catch (Exception e)
            {
                return output;
            }


        }

        public override async Task ExecuteAsync(ImportUsersFromExcelJobArgs args)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(args.TenantId))
                {
                    using (_session.Use(args.TenantId, args.User.UserId))
                    {
                        try
                        {
                            var file = await _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId);

                            var li = _invoiceListExcelDataReader.GetInvoiceFromExcelCustom(file.Bytes);
                            string json = JsonConvert.SerializeObject(li);
                            bool isProcessed = true;
                            await _debitNotes.InsertBatchUploadDebitSales(json, args.filename, args.TenantId, args.fromdate, args.todate);
                            var items = mapJsonToModelDebit(json);

                            int batchId = await _sales.GetLatestBatchId();
                            foreach (var item in items)
                            {

                                await _debitNotes.GenerateInvoice_SG(item, batchId);
                            }
                            if (isProcessed)
                            {
                                await _appNotifier.SendMessageAsync(
                      args.User,
                      new LocalizableString("Debit Note Sales File Upload Success",
                          vitaConsts.LocalizationSourceName),
                      null,
                      Abp.Notifications.NotificationSeverity.Success);
                            }

                            else
                            {
                                await _appNotifier.SendMessageAsync(
                     args.User,
                     new LocalizableString("Debit Note Sales File Upload Failed",
                         vitaConsts.LocalizationSourceName),
                     null,
                     Abp.Notifications.NotificationSeverity.Error);
                            }


                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                        finally
                        {
                            await uow.CompleteAsync();
                        }
                    }

                }
            }

        }



    }
}

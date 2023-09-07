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
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Stripe;
using vita.Authorization.Roles;
using vita.Authorization.Users;
using vita.Authorization.Users.Dto;
using vita.Authorization.Users.Importing;
using vita.Authorization.Users.Importing.Dto;
using vita.ImportBatch.Dtos;
using vita.Notifications;
using vita.ImportBatch;
using vita.ImportBatch.Dtos;
using vita.ImportBatch.Importing;
using vita.Storage;
using Newtonsoft.Json;
using vita.Sales;
using vita.Sales.Dtos;
using vita.Credit.Dtos;
using Newtonsoft.Json.Linq;
using vita.Credit;
using vita.Debit.Dtos;
using vita.Debit;
using Org.BouncyCastle.Utilities;

namespace vita.StandardFileUpload
{
    public class ImportSalesInvoiceToExcelJob : AsyncBackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly RoleManager _roleManager;
        private readonly ISalesInvoiceListExcelDataReader _invoiceListExcelDataReader;
        private readonly IInvalidUserExporter _invalidUserExporter;
        private readonly IUserPolicy _userPolicy;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IImportBatchDatasAppService _importSalesFilesAppService;
        private readonly IInvalidInvoiceExporter _invalidInvoiceExporter;
        private readonly ISalesInvoicesAppService _sales;
        private readonly ICreditNoteAppService _credit;
        private readonly IDebitNotesAppService _debit;


        private readonly IAbpSession _session;
        protected readonly IBackgroundJobManager BackgroundJobManager;
        public UserManager UserManager { get; set; }

        public ImportSalesInvoiceToExcelJob(
            RoleManager roleManager,
           ISalesInvoiceListExcelDataReader invoiceListExcelDataReader,
            IInvalidUserExporter invalidUserExporter,
            IUserPolicy userPolicy,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            IObjectMapper objectMapper,
            IUnitOfWorkManager unitOfWorkManager,
            IImportBatchDatasAppService importSalesFilesAppService,
            IInvalidInvoiceExporter invalidInvoiceExporter,
            ISalesInvoicesAppService sales,
            IAbpSession session,
            IBackgroundJobManager backgroundJobManager,
            ICreditNoteAppService credit,
            IDebitNotesAppService debit)
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
            _importSalesFilesAppService = importSalesFilesAppService;
            _invalidInvoiceExporter = invalidInvoiceExporter;
            _sales = sales;
            _credit = credit;
            _debit = debit;
            _session = session;
            BackgroundJobManager = backgroundJobManager;
        }

        //private class InvoiceModel
        //{
        //    [JsonProperty(PropertyName = "Invoice Type")]
        //    public string InvoiceType { get; set; }
        //    [JsonProperty(PropertyName = "TransType")]
        //    public string TransType { get; set; }
        //    [JsonProperty(PropertyName = "IRN Number")]
        //    public string IRNNumber { get; set; }
        //    [JsonProperty(PropertyName = "Invoice Number *")]
        //    public string InvoiceNumber { get; set; }
        //    [JsonProperty(PropertyName = "Invoice Issue Date*")]
        //    public string InvoiceIssueDate { get; set; }
        //    [JsonProperty(PropertyName = "Invoice Issue Time *")]
        //    public string InvoiceIssueTime { get; set; }
        //    [JsonProperty(PropertyName = "Invoice currency code *")]
        //    public string InvoiceCurrencyCode { get; set; }
        //    [JsonProperty(PropertyName = "Purchase Order ID")]
        //    public string PurchaseOrderID { get; set; }
        //    [JsonProperty(PropertyName = "Contract ID")]
        //    public string ContractID { get; set; }
        //    [JsonProperty(PropertyName = "Supply Date")]
        //    public string SupplyDate { get; set; }
        //    [JsonProperty(PropertyName = "Supply End Date")]
        //    public string SupplyEndDate { get; set; }
        //    [JsonProperty(PropertyName = "Buyer Master Code")]
        //    public string BuyerMasterCode { get; set; }
        //    [JsonProperty(PropertyName = "Buyer Name")]
        //    public string BuyerName { get; set; }
        //    [JsonProperty(PropertyName = "Buyer VAT number")]
        //    public string BuyerVATNumber { get; set; }
        //    [JsonProperty(PropertyName = "Buyer Contact")]
        //    public string BuyerContact { get; set; }
        //    [JsonProperty(PropertyName = "Buyer Country Code")]
        //    public string BuyerCountryCode { get; set; }
        //    [JsonProperty(PropertyName = "Invoice line identifier *")]
        //    public string InvoiceLineIdentifier { get; set; }
        //    [JsonProperty(PropertyName = "Item Master Code")]
        //    public string ItemMasterCode { get; set; }
        //    [JsonProperty(PropertyName = "Item name")]
        //    public string ItemName { get; set; }
        //    [JsonProperty(PropertyName = "Invoiced quantity unit of measure")]
        //    public string InvoicedQuantityUnitOfMeasure { get; set; }
        //    [JsonProperty(PropertyName = "Item gross price")]
        //    public string ItemGrossPrice { get; set; }
        //    [JsonProperty(PropertyName = "Item price discount")]
        //    public string ItemPriceDiscount { get; set; }
        //    [JsonProperty(PropertyName = "Item net price*")]
        //    public string ItemNetPrice { get; set; }
        //    [JsonProperty(PropertyName = "Invoiced quantity ")]
        //    public string InvoicedQuantity { get; set; }
        //    [JsonProperty(PropertyName = "Invoice line net amount")]
        //    public string InvoiceLineNetAmount { get; set; }
        //    [JsonProperty(PropertyName = "Invoiced item VAT category code*")]
        //    public string InvoicedItemVATCategoryCode { get; set; }
        //    [JsonProperty(PropertyName = "Invoiced item VAT rate*")]
        //    public string InvoicedItemVATRate { get; set; }
        //    [JsonProperty(PropertyName = "VAT exemption reason code")]
        //    public string VATExemptionReasonCode { get; set; }
        //    [JsonProperty(PropertyName = "VAT exemption reason")]
        //    public string VATExemptionReason { get; set; }
        //    [JsonProperty(PropertyName = "VAT line amount*")]
        //    public string VATLineAmount { get; set; }
        //    [JsonProperty(PropertyName = "Line amount inclusive VAT*")]
        //    public string LineAmountInclusiveVAT { get; set; }
        //    [JsonProperty(PropertyName = "Advance Rcpt Amount Adjusted")]
        //    public string AdvanceRcptAmountAdjusted { get; set; }
        //    [JsonProperty(PropertyName = "VAT on Advance Receipt Amount Adjusted")]
        //    public string VATOnAdvanceReceiptAmountAdjusted { get; set; }
        //    [JsonProperty(PropertyName = "Advance Receipt Reference Number")]
        //    public string AdvanceReceiptReferenceNumber { get; set; }
        //    [JsonProperty(PropertyName = "Payment Means")]
        //    public string PaymentMeans { get; set; }
        //    [JsonProperty(PropertyName = "Payment Terms")]
        //    public string PaymentTerms { get; set; }
        //    [JsonProperty(PropertyName = "Buyer Type")]
        //    public string BuyerType { get; set; }
        //    public string xml_uuid { get; set; }


        //}

        //class CompareInvNum : IComparer<InvoiceModel>
        //{
        //    public int Compare(InvoiceModel x, InvoiceModel y)
        //    {
        //        var a = Convert.ToString(x.InvoiceNumber);
        //        var b = Convert.ToString(y.InvoiceNumber);
        //        if (a == "0" || b == "0")
        //        {
        //            return 0;
        //        }

        //        // CompareTo() method
        //        return a.CompareTo(b);

        //    }
        //}

        //private List<CreateOrEditSalesInvoiceDto> mapJsonToModel(string json)
        //{
        //    List<CreateOrEditSalesInvoiceDto> output = new List<CreateOrEditSalesInvoiceDto>();
        //    try
        //    {
        //        List<InvoiceModel> invoiceModels = JsonConvert.DeserializeObject<List<InvoiceModel>>(json);
        //        invoiceModels.Sort(new CompareInvNum());
        //        for (var i = 0; i < invoiceModels.Count; i++)
        //        {
        //            var invNum = Convert.ToString(invoiceModels[i].InvoiceNumber);
        //            CreateOrEditSalesInvoiceDto model = new CreateOrEditSalesInvoiceDto()
        //            {
        //                Items = new List<CreateOrEditSalesInvoiceItemDto>(),
        //                IssueDate = DateTime.ParseExact(invoiceModels[i].InvoiceIssueDate, "MM/dd/yyyy", null),
        //                InvoiceNumber = invoiceModels[i].InvoiceNumber,
        //                Supplier = new CreateOrEditSalesInvoicePartyDto(),
        //                Buyer = new CreateOrEditSalesInvoicePartyDto(),
        //                InvoiceSummary = new CreateOrEditSalesInvoiceSummaryDto(),
        //                Additional_Info = invoiceModels[i].xml_uuid

        //            };
        //            bool flag = false;

        //            model.Supplier.Address = new CreateOrEditSalesInvoiceAddressDto();
        //            model.Buyer.Address = new CreateOrEditSalesInvoiceAddressDto();
        //            model.Buyer.ContactPerson= new CreateOrEditSalesInvoiceContactPersonDto();
        //            model.Supplier.RegistrationName = "Saudi Arabian Glass Co Ltd";
        //            model.Supplier.VATID = "300166057500003";
        //            model.Supplier.CustomerId = "";
        //            model.Supplier.Address.BuildingNo = "1234";
        //            model.Supplier.Address.AdditionalNo = "";
        //            model.Supplier.Address.Street = "Industrial Areas";
        //            model.Supplier.Address.AdditionalStreet = "Phase 4";
        //            model.Supplier.Address.Neighbourhood = "Jeddah";
        //            model.Supplier.Address.State = "Jeddah";
        //            model.Supplier.Address.PostalCode = "21494";
        //            model.Supplier.Address.City = "Jeddah";
        //            model.Supplier.Address.CountryCode = "SA";

        //            model.Buyer.RegistrationName = invoiceModels[i].BuyerName;
        //            model.Buyer.VATID = invoiceModels[i].BuyerVATNumber;
        //            model.Buyer.ContactPerson.ContactNumber = invoiceModels[i].BuyerContact;
        //            model.Buyer.CustomerId = "";
        //            model.Buyer.Address.BuildingNo = "3012";
        //            model.Buyer.Address.AdditionalNo = "";
        //            model.Buyer.Address.Street = "Makhrooj";
        //            model.Buyer.Address.AdditionalStreet = "Al Fayaz Road";
        //            model.Buyer.Address.Neighbourhood = "Riyadh";
        //            model.Buyer.Address.State = "Riyadh";
        //            model.Buyer.Address.City = "Riyadh";
        //            model.Buyer.Address.PostalCode = "11585";
        //            model.Buyer.Address.CountryCode = "SA";

        //            while (i < invoiceModels.Count && Convert.ToString(invoiceModels[i].InvoiceNumber) == invNum)
        //            {
        //                model.Items.Add(new CreateOrEditSalesInvoiceItemDto()
        //                {
        //                    Name = invoiceModels[i].ItemName,
        //                    DiscountPercentage = Convert.ToDecimal(invoiceModels[i].ItemPriceDiscount),
        //                    DiscountAmount = Convert.ToDecimal(invoiceModels[i].ItemGrossPrice)*(Convert.ToDecimal(invoiceModels[i].ItemPriceDiscount)/100),
        //                    UnitPrice = Convert.ToDecimal(invoiceModels[i].ItemGrossPrice),
        //                    Quantity = Convert.ToDecimal(invoiceModels[i].InvoicedQuantity),
        //                    UOM = invoiceModels[i].InvoicedQuantityUnitOfMeasure,
        //                    VATAmount = Convert.ToDecimal(invoiceModels[i].VATLineAmount),
        //                    LineAmountInclusiveVAT = Convert.ToDecimal(invoiceModels[i].LineAmountInclusiveVAT),
        //                    VATRate = Convert.ToDecimal(invoiceModels[i].InvoicedItemVATRate),
        //                    Identifier = invoiceModels[i].PurchaseOrderID,
        //                    Description = invoiceModels[i].ItemName,
        //                    GrossPrice = Convert.ToDecimal(invoiceModels[i].ItemGrossPrice),
        //                    NetPrice = Convert.ToDecimal(invoiceModels[i].ItemGrossPrice) - Convert.ToDecimal(invoiceModels[i].ItemPriceDiscount),

        //                });

        //                i++;
        //                flag = true;
        //            }
        //            model.InvoiceSummary.NetInvoiceAmount = invoiceModels.Sum(p => Convert.ToDecimal(p.ItemNetPrice));
        //            model.InvoiceSummary.SumOfInvoiceLineNetAmount = invoiceModels.Sum(p => Convert.ToDecimal(p.InvoiceLineNetAmount));
        //            model.InvoiceSummary.TotalAmountWithoutVAT = invoiceModels.Sum(p => Convert.ToDecimal(p.ItemNetPrice));
        //            model.InvoiceSummary.TotalAmountWithVAT = invoiceModels.Sum(p => Convert.ToDecimal(p.LineAmountInclusiveVAT));

        //            output.Add(model);

        //            if (flag)
        //                i--;
        //        }

        //        return output;
        //    }
        //    catch (Exception e)
        //    {
        //        return output;
        //    }


        //}

     

        public override async Task ExecuteAsync(ImportUsersFromExcelJobArgs args)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(args.TenantId))
                {
                    using (_session.Use(args.TenantId, args.User?.UserId))
                    {
                        try
                        {
                            await _sales.InsertUploadDatatoLogs("FileRecivedatJob", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
                            var file = await _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId);

                            byte[] excelBytes = null;

                            if (args.filename.EndsWith("csv"))
                            {
                                excelBytes = _invoiceListExcelDataReader.ConvertCsvToExcel(file.Bytes);
                            }
                            else
                            {
                                excelBytes = file.Bytes;
                            }

                            var li = _invoiceListExcelDataReader.GetInvoiceFromExcelCustom(excelBytes);



                            string mapping =  await _sales.GetFileMappingById(args.configurationId ?? 1);

                            if (mapping != null)
                            {
                                List<FileMappingModel> Mapli = new List<FileMappingModel>();
                                Mapli = JsonConvert.DeserializeObject<List<FileMappingModel>>(mapping);


                                foreach (var dictionary in li)
                                {
                                    // Create a new dictionary to store the updated key-value pairs
                                    Dictionary<string, string> updatedDictionary = new Dictionary<string, string>();

                                    // Loop through each key-value pair in the original dictionary
                                    foreach (var kvp in dictionary)
                                    {
                                        // Check if the key exists in the mapping dictionary
                                        if (Mapli.Exists(a => a.UploadedFields[0] == kvp.Key))
                                        {
                                            // Get the corresponding new key from the mapping dictionary
                                            string newKey = Mapli.Find(a => a.UploadedFields[0] == kvp.Key).FieldForMapping;

                                            // Add the key-value pair with the updated key to the new dictionary
                                            updatedDictionary[newKey] = kvp.Value;
                                        }
                                        else
                                        {
                                            // If the key is not in the mapping dictionary, add it as is to the new dictionary
                                            updatedDictionary[kvp.Key] = kvp.Value;
                                        }
                                    }

                                    // Replace the old dictionary with the updated dictionary in the list
                                    dictionary.Clear();
                                    foreach (var kvp in updatedDictionary)
                                    {
                                        dictionary[kvp.Key] = kvp.Value;
                                    }
                                }
                            }


                     

                            string json = JsonConvert.SerializeObject(li);
                            bool isProcessed = true;
                            await _sales.InsertBatchUploadSales(json, args.filename, args.TenantId, args.fromdate, args.todate);

                                //var items = mapJsonToModel(json);

                                //int batchId = await _sales.GetLatestBatchId();
                                //foreach (var item in items)
                                //{

                                //    var res = await _sales.GenerateInvoice_SG(item, batchId);
                                //}

                            await SendNotificationAsync(args, isProcessed);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                            await SendNotificationAsync(args, false);


                        }


                        finally
                        {
                            await uow.CompleteAsync();
                        }
                    }

                }
            }

        }



        private async Task SendNotificationAsync(ImportUsersFromExcelJobArgs args, bool isProcessed)
        {
            if (!isProcessed)
            {
                await _appNotifier.SendMessageAsync(
                   args.User,
                   new LocalizableString("Sales File Upload Failed",
                       vitaConsts.LocalizationSourceName),
                   null,
                   Abp.Notifications.NotificationSeverity.Error);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    new LocalizableString("Sales File Upload Success",
                        vitaConsts.LocalizationSourceName),
                    null,
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }


    }
}
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.ObjectMapping;
//using vita.StandardFileUpload.Dtos;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vita.Authorization.Roles;
using vita.Authorization.Users;
using vita.Authorization.Users.Dto;
using vita.Authorization.Users.Importing;
//using IInvalidInvoiceExporter = vita.CreditNotePurchaseFileUpload.Importing;
using vita.Credit;
using vita.CreditNotePurchaseFileUpload.Importing;
using vita.ImportBatch;
using vita.Notifications;
using vita.Sales;
//using vita.CreditNoteFileUpload.Dtos;
using vita.Storage;

namespace vita.CreditNoteFileUpload
{
    public class ImportCreditNotePurchaseFileToExcelJob : AsyncBackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly RoleManager _roleManager;
        private readonly ICreditNotePurchaseListExcelDataReader _invoiceListExcelDataReader;
        private readonly IInvalidUserExporter _invalidUserExporter;
        private readonly IUserPolicy _userPolicy;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IObjectMapper _objectMapper;
        public readonly ISalesInvoicesAppService _sales;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        //private readonly ICreditNoteFilesAppService _creditNoteFilesAppService;
        private readonly IImportBatchDatasAppService _importBatchDatasAppService;
        //private readonly IInvalidInvoiceExporter _invalidInvoiceExporter;
        /// private readonly IImportStandardFilesAppService _importStandardFilesAppService;
        private readonly IAbpSession _session;
        private readonly ICreditNotePurchaseAppService _creditNote;

        public UserManager UserManager { get; set; }

        public ImportCreditNotePurchaseFileToExcelJob(
            RoleManager roleManager,
           ICreditNotePurchaseListExcelDataReader invoiceListExcelDataReader,
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
            //IInvalidInvoiceExporter invalidInvoiceExporter,
            IAbpSession session,
            ISalesInvoicesAppService sales,
        //    IImportStandardFilesAppService importStandardFilesAppService,
           ICreditNotePurchaseAppService creditNote)
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
            //_invalidInvoiceExporter = invalidInvoiceExporter;
            //  _importStandardFilesAppService = importStandardFilesAppService;
            _session = session;
            _creditNote = creditNote;
            _sales = sales;
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



                            string mapping = await _sales.GetFileMappingById(args.configurationId ?? 5);

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
                            bool isProcessed = await _creditNote.InsertBatchUploadCreditPurchase(json, args.filename, args.TenantId, args.fromdate, args.todate);

                            if (isProcessed)
                            {
                                await _appNotifier.SendMessageAsync(
                      args.User,
                      new LocalizableString("Credit File Upload Success",
                          vitaConsts.LocalizationSourceName),
                      null,
                      Abp.Notifications.NotificationSeverity.Success);
                            }

                            else
                            {
                                await _appNotifier.SendMessageAsync(
                     args.User,
                     new LocalizableString("Credit File Upload Failed",
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

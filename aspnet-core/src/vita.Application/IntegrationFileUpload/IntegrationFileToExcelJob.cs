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
using vita.Debit.Dtos;
using vita.Sales;
using vita.PdfGenerator;

namespace vita.IntegrationFileUpload
{
    public class ImportIntegrationFileToExcelJob : AsyncBackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly RoleManager _roleManager;
        private readonly IIntegrationListExcelDataReader _invoiceListExcelDataReader;
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
      private readonly ICreditNoteAppService _creditNote;
        private readonly ISalesInvoicesAppService _sales;

        protected readonly IBackgroundJobManager BackgroundJobManager;
        public UserManager UserManager { get; set; }

        public ImportIntegrationFileToExcelJob(
            RoleManager roleManager,
           IIntegrationListExcelDataReader invoiceListExcelDataReader,
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
        //    IImportStandardFilesAppService importStandardFilesAppService,
           ICreditNoteAppService creditNote,
           ISalesInvoicesAppService sales,
           IBackgroundJobManager backgroundJobManager)
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
         _creditNote = creditNote;
            _sales = sales;
            BackgroundJobManager = backgroundJobManager;
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

                            if (args.filename.ToLower().EndsWith("csv"))
                            {
                                excelBytes = _invoiceListExcelDataReader.ConvertCsvToExcel(file.Bytes);
                            }
                            else
                            {
                                excelBytes = file.Bytes;
                            }

                            var li = _invoiceListExcelDataReader.GetInvoiceFromExcelCustom(excelBytes); 
                            
                            string json = JsonConvert.SerializeObject(li);
                            bool isProcessed = false;
                            await _sales.InsertBatchUploadSales(json, args.filename, args.TenantId, args.fromdate, args.todate,false);

                            int batchId = await _sales.GetLatestBatchId();

                            var data = await _sales.GetIrnForFileUpload(batchId);
                            if(data.Count>0)
                            {
                                isProcessed = true;
                                await BackgroundJobManager.EnqueueAsync<PdfGeneratorJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                                {
                                    TenantId = args.TenantId,
                                    BinaryObjectId = args.BinaryObjectId,
                                    User = args.User,
                                    filename = args.filename,
                                    fromdate = args.fromdate,
                                    todate = args.todate,
                                    Id = data
                                });
                            }
                           
                            if (isProcessed)
                            {
                                await _appNotifier.SendMessageAsync(
                      args.User,
                      new LocalizableString("Sales Invoice Upload Success",
                          vitaConsts.LocalizationSourceName),
                      null,
                      Abp.Notifications.NotificationSeverity.Success);
                            }

                            else
                            {
                                await _appNotifier.SendMessageAsync(
                     args.User,
                     new LocalizableString("Sales Invoice Upload Failed",
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

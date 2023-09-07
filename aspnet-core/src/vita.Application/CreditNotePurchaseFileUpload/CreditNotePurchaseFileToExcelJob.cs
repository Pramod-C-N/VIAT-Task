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
//using IInvalidInvoiceExporter = vita.CreditNotePurchaseFileUpload.Importing;
using vita.Credit;
using vita.Sales.Dtos;
using Newtonsoft.Json;
using vita.CreditNotePurchaseFileUpload.Importing;

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
                            bool isProcessed = await _creditNote.InsertBatchUploadCreditPurchase(json, args.filename, args.TenantId,args.fromdate,args.todate);

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

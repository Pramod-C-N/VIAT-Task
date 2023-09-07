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
//using vita.CreditNoteFileUpload.Dtos;
using vita.CreditNoteFileUpload.Importing;
using vita.Storage;
//using vita.SalesInvoice.Dtos;
using vita.StandardFileUpload;
using Abp.Runtime.Session;
//using vita.SalesInvoice;
//using vita.CreditNote.Dtos;
//using vita.PurchaseEntry.Dtos;

using vita.Purchase;
using vita.PurchaseFileUpload.Importing;
using vita.ImportBatch;
using vita.ImportBatch.Dtos;
using vita.ImportBatch.Importing;
using Newtonsoft.Json;

namespace vita.PurchaseFileUpload
{
    public class ImportPurchaseFileToExcelJob : AsyncBackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly RoleManager _roleManager;
        private readonly IImportPurchaseListExcelDataReader _invoiceListExcelDataReader;
        private readonly IInvalidUserExporter _invalidUserExporter;
        private readonly IUserPolicy _userPolicy;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
      //  private readonly IImportPurchaseFilesAppService _creditNoteFilesAppService;
        private readonly Importing.IInvalidInvoiceExporter _invalidInvoiceExporter;
      //  private readonly IImportStandardFilesAppService _importStandardFilesAppService;
        private readonly IAbpSession _session;
       private readonly IPurchaseEntriesAppService _purchase;

        public UserManager UserManager { get; set; }

        public ImportPurchaseFileToExcelJob(
            RoleManager roleManager,
           IImportPurchaseListExcelDataReader invoiceListExcelDataReader,
            IInvalidUserExporter invalidUserExporter,
            IUserPolicy userPolicy,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            IObjectMapper objectMapper,
            IUnitOfWorkManager unitOfWorkManager,
          //  IImportPurchaseFilesAppService creditNoteFilesAppService,
           Importing.IInvalidInvoiceExporter invalidInvoiceExporter,
            IAbpSession session,
     ////       IImportStandardFilesAppService importStandardFilesAppService,
        IPurchaseEntriesAppService purchase)
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
          //  _creditNoteFilesAppService = creditNoteFilesAppService;
            _invalidInvoiceExporter = invalidInvoiceExporter;
       //     _importStandardFilesAppService = importStandardFilesAppService;
            _session = session;
         _purchase = purchase;
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
                            bool isProcessed = await _purchase.InsertBatchUploadPurchase(json, args.filename, args.TenantId,args.fromdate,args.todate);

                            if (isProcessed)
                            {
                                await _appNotifier.SendMessageAsync(
                      args.User,
                      new LocalizableString("Purchase File Upload Success",
                          vitaConsts.LocalizationSourceName),
                      null,
                      Abp.Notifications.NotificationSeverity.Success);
                            }

                            else
                            {
                                await _appNotifier.SendMessageAsync(
                     args.User,
                     new LocalizableString("Purchase File Upload Failed",
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

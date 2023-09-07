using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vita.Authorization.Users;
using vita.Authorization.Users.Dto;
using vita.Authorization.Users.Importing;
//using vita.CreditNote;
using vita.CreditNoteFileUpload;
using vita.ImportPaymentFileupload.Importing;
using vita.Notifications;
using vita.StandardFileUpload;
using vita.Storage;
using Abp.ObjectMapping;
using vita.Authorization.Roles;
using Abp.Localization;
using Abp.UI;
//using vita.CreditNote.Dtos;
//using vita.SalesInvoice.Dtos;
using vita.ImportBatch;
using vita.ImportBatch.Dtos;
using vita.ImportBatch.Importing;
using Newtonsoft.Json;
using vita.PaymentFileUpload;
using vita.Payment;
//using vita.StandardFileUpload.Dtos;

namespace vita.ImportPaymentFileupload
{
    public class ImportPaymentFileToExcelJob : AsyncBackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly RoleManager _roleManager;
        private readonly IImportPaymentListExcelDataReader _importPaymentListExcelDataReader;
        private readonly IInvalidUserExporter _invalidUserExporter;
        private readonly IUserPolicy _userPolicy;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
       // private readonly ICreditNoteFilesAppService _creditNoteFilesAppService;
        //private readonly IInvalidInvoiceExporter _invalidInvoiceExporter;
      //  private readonly IImportStandardFilesAppService _importStandardFilesAppService;
        private readonly IAbpSession _session;
        //  private readonly ICreditNoteService _creditNote;
        private readonly IPaymentFileUpload _Payment;

        public UserManager UserManager { get; set; }

        public ImportPaymentFileToExcelJob(
            RoleManager roleManager,
           IImportPaymentListExcelDataReader invoiceListExcelDataReader,
            IInvalidUserExporter invalidUserExporter,
            IUserPolicy userPolicy,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            IObjectMapper objectMapper,
            IUnitOfWorkManager unitOfWorkManager,
         //   ICreditNoteFilesAppService creditNoteFilesAppService,
            //IInvalidInvoiceExporter invalidInvoiceExporter,
            IAbpSession session,
            IPaymentFileUpload payment)
          //  IImportStandardFilesAppService importStandardFilesAppService,
          //  ICreditNoteService creditNote)
        {
            _roleManager = roleManager;
            _importPaymentListExcelDataReader = invoiceListExcelDataReader;
            _invalidUserExporter = invalidUserExporter;
            _userPolicy = userPolicy;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
            _Payment = payment;
          //  _creditNoteFilesAppService = creditNoteFilesAppService;
            //_invalidInvoiceExporter = invalidInvoiceExporter;
         //   _importStandardFilesAppService = importStandardFilesAppService;
            _session = session;
          //  _creditNote = creditNote;
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

                            var li = _importPaymentListExcelDataReader.GetInvoiceFromExcelCustom(file.Bytes);
                            string json = JsonConvert.SerializeObject(li);
                            bool isProcessed = await _Payment.InsertBatchUploadPayment(json, args.filename, args.TenantId,args.fromdate,args.todate);

                            if (isProcessed)
                            {
                                await _appNotifier.SendMessageAsync(
                      args.User,
                      new LocalizableString("Payment File Upload Success",
                          vitaConsts.LocalizationSourceName),
                      null,
                      Abp.Notifications.NotificationSeverity.Success);
                            }

                            else
                            {
                                await _appNotifier.SendMessageAsync(
                     args.User,
                     new LocalizableString("Payment File Upload Failed",
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

﻿using Abp.BackgroundJobs;
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
using vita.ImportPaymentFileupload.Importing;
using vita.Notifications;
using vita.Storage;
using Abp.ObjectMapping;
using vita.Authorization.Roles;
using Abp.Localization;
using Newtonsoft.Json;
using vita.Payment;
using vita.Customer;
using Abp.Domain.Repositories;
using vita.TenantDetails;
using vita.TenantFileUpload.Importing;

namespace vita.ImportTenantFileupload
{
    public class ImportTenantFileToExcelJob : AsyncBackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly IImportTenantListExcelDataReader _importPaymentListExcelDataReader;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpSession _session;
        private readonly ICustomersesAppService _customer;
        private readonly ITenantBasicDetailsAppService _tenant;

        public UserManager UserManager { get; set; }

        public ImportTenantFileToExcelJob(
           IImportTenantListExcelDataReader invoiceListExcelDataReader,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            IUnitOfWorkManager unitOfWorkManager,
            IAbpSession session,
        ICustomersesAppService customer,
        ITenantBasicDetailsAppService tenant
            )
        {
            _importPaymentListExcelDataReader = invoiceListExcelDataReader;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _unitOfWorkManager = unitOfWorkManager;
            _customer = customer;
            _session = session;
            _tenant = tenant;
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
                            bool isProcessed = await _tenant.InsertBatchUploadTenant(json, args.filename, args.TenantId);

                            if (isProcessed)
                            {
                                await _appNotifier.SendMessageAsync(
                      args.User,
                      new LocalizableString("Tenant File Upload Success",
                          vitaConsts.LocalizationSourceName),
                      null,
                      Abp.Notifications.NotificationSeverity.Success);
                            }

                            else
                            {
                                await _appNotifier.SendMessageAsync(
                     args.User,
                     new LocalizableString("Tenant File Upload Failed",
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

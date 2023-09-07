using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Runtime.Session;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vita.Authorization.Users.Dto;
using vita.Notifications;
using vita.Sales;
using vita.Sales.Dtos;
using vita.Storage;

namespace vita.PdfGenerator
{
    public class PdfGeneratorJob : AsyncBackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpSession _session;
        private readonly ISalesInvoicesAppService _salesInvoicesAppService;
        private readonly ISalesInvoicesDomainService salesInvoicesDomainService;

        public PdfGeneratorJob(IUnitOfWorkManager unitOfWorkManager,
         IAbpSession session, ISalesInvoicesAppService salesInvoicesAppService, ISalesInvoicesDomainService salesInvoicesDomainService)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _session = session;
            _salesInvoicesAppService = salesInvoicesAppService;
            this.salesInvoicesDomainService = salesInvoicesDomainService;
        }
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
                            InvoiceResponse response = new();
                            foreach (var irn in args.Id)
                            {
                                response = await salesInvoicesDomainService.GenerateDraftInvoice(irn.IRNNo,args.TenantId??0,irn.TransTypeDescription);
                                await salesInvoicesDomainService.UpdateInvoiceURL(response, args.TenantId ?? 0, "Draft");
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

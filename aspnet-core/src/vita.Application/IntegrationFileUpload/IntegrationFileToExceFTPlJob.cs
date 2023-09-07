using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.ObjectMapping;
//using vita.StandardFileUpload.Dtos;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using vita.Authorization.Roles;
using vita.Authorization.Users;
using vita.Authorization.Users.Dto;
using vita.Authorization.Users.Importing;
using vita.Credit;
//using vita.CreditNoteFileUpload.Dtos;
using vita.CreditNoteFileUpload.Importing;
using vita.ImportBatch;
using vita.Notifications;
using vita.PdfGenerator;
using vita.Sales;
using vita.Storage;
using IInvalidInvoiceExporter = vita.CreditNoteFileUpload.Importing.IInvalidInvoiceExporter;

namespace vita.IntegrationFileUpload
{
    public class ImportIntegrationFileToExcelFTPJob : AsyncBackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly IIntegrationListExcelDataReader _invoiceListExcelDataReader;
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IImportBatchDatasAppService _importBatchDatasAppService;
        private readonly IInvalidInvoiceExporter _invalidInvoiceExporter;
        private readonly IAbpSession _session;
        private readonly ICreditNoteAppService _creditNote;
        private readonly ISalesInvoicesAppService _sales;
        private readonly ITenantStore _tenantStore;

        protected readonly IBackgroundJobManager BackgroundJobManager;
        public UserManager UserManager { get; set; }

        public ImportIntegrationFileToExcelFTPJob(
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
            IImportBatchDatasAppService importBatchDatasAppService,
            IInvalidInvoiceExporter invalidInvoiceExporter,
            IAbpSession session,
           ICreditNoteAppService creditNote,
           ISalesInvoicesAppService sales,
           IBackgroundJobManager backgroundJobManager,
           ITenantStore tenantStore)
        {
            _tenantStore = tenantStore;
            _invoiceListExcelDataReader = invoiceListExcelDataReader;
            _objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
            _importBatchDatasAppService = importBatchDatasAppService;
            _invalidInvoiceExporter = invalidInvoiceExporter;
            _session = session;
            _creditNote = creditNote;
            _sales = sales;
            BackgroundJobManager = backgroundJobManager;
        }

        public override async Task ExecuteAsync(ImportUsersFromExcelJobArgs args)
        {
            byte[] fbytes = null;
            int? tenantId = _tenantStore.Find("Brady")?.Id;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://20.85.229.3/AbylleQA/Inbox");
                ftpRequest.Credentials = new NetworkCredential("Braidy_sftp", "@bylle$olution5");
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());
                WebClient client = new WebClient();
                client.Credentials = new NetworkCredential("Braidy_sftp", "@bylle$olution5");
                List<string> fileList = new List<string>();
                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    if (line.ToLower().EndsWith("xlsx") || line.ToLower().EndsWith("csv"))
                    {
                        fileList.Add(line);
                    }
                    line = streamReader.ReadLine();
                }
                streamReader.Close();

                if (fileList.Count == 0) return;
                using (var uow = _unitOfWorkManager.Begin())
                {
                    using (CurrentUnitOfWork.SetTenantId(tenantId))
                    {
                        using (_session.Use(tenantId, args.User?.UserId))
                        {
                            foreach (var file in fileList)
                            {
                                try
                                {
                                    var guid = Guid.NewGuid().ToString();
                                    fbytes = client.DownloadData("ftp://20.85.229.3/AbylleQA/" + file);
                                    if (file.ToLower().EndsWith("csv"))
                                    {
                                        fbytes = _invoiceListExcelDataReader.ConvertCsvToExcel(fbytes);
                                    }
                                    var li = _invoiceListExcelDataReader.GetInvoiceFromExcelCustom(fbytes);
                                    string json = JsonConvert.SerializeObject(li);
                                    bool isProcessed = false;
                                    await _sales.InsertBatchUploadSales(json, args.filename, tenantId, args.fromdate, args.todate, false);

                                    int batchId = await _sales.GetLatestBatchId();

                                    var data = await _sales.GetIrnForFileUpload(batchId);
                                    if (data.Count > 0)
                                    {
                                        isProcessed = true;
                                        await BackgroundJobManager.EnqueueAsync<PdfGeneratorJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                                        {
                                            TenantId = tenantId,
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
                                        Uri serverFile = new Uri("ftp://20.85.229.3/AbylleQA/" + file);
                                        FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(serverFile);
                                        reqFTP.Method = WebRequestMethods.Ftp.Rename;
                                        reqFTP.Credentials = new NetworkCredential("Braidy_sftp", "@bylle$olution5");
                                        reqFTP.RenameTo = "../Processed/" + Guid.NewGuid().ToString() + "_" + file.Split('/')?.Last() ?? "";
                                        FtpWebResponse res = (FtpWebResponse)reqFTP.GetResponse();
                                    }
                                    else
                                    {
                                        throw new Exception("File not processed");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex.Message);
                                    Uri serverFile = new Uri("ftp://20.85.229.3/AbylleQA/" + file);
                                    FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(serverFile);
                                    reqFTP.Method = WebRequestMethods.Ftp.Rename;
                                    reqFTP.Credentials = new NetworkCredential("Braidy_sftp", "@bylle$olution5");
                                    reqFTP.RenameTo = "../Failed/" + Guid.NewGuid().ToString() + "_" + file.Split('/')?.Last() ?? "";
                                    FtpWebResponse res = (FtpWebResponse)reqFTP.GetResponse();
                                }

                            }
                            await uow.CompleteAsync();
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }


        }



    }
}

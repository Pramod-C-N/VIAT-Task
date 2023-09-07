using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class TransactionCategoryExcelExporter : NpoiExcelExporterBase, ITransactionCategoryExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TransactionCategoryExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTransactionCategoryForViewDto> transactionCategory)
        {
            return CreateExcelPackage(
                "TransactionCategory.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("TransactionCategory"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, transactionCategory,
                        _ => _.TransactionCategory.Name,
                        _ => _.TransactionCategory.Description,
                        _ => _.TransactionCategory.Code,
                        _ => _.TransactionCategory.IsActive
                        );

                });
        }
    }
}
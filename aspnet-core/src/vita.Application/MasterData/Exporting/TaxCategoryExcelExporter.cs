using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class TaxCategoryExcelExporter : NpoiExcelExporterBase, ITaxCategoryExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TaxCategoryExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTaxCategoryForViewDto> taxCategory)
        {
            return CreateExcelPackage(
                "TaxCategory.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("TaxCategory"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsKSAApplicable"),
                        L("TaxSchemeID"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, taxCategory,
                        _ => _.TaxCategory.Name,
                        _ => _.TaxCategory.Description,
                        _ => _.TaxCategory.Code,
                        _ => _.TaxCategory.IsKSAApplicable,
                        _ => _.TaxCategory.TaxSchemeID,
                        _ => _.TaxCategory.IsActive
                        );

                });
        }
    }
}
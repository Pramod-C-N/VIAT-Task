using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class TaxSubCategoryExcelExporter : NpoiExcelExporterBase, ITaxSubCategoryExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TaxSubCategoryExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTaxSubCategoryForViewDto> taxSubCategory)
        {
            return CreateExcelPackage(
                "TaxSubCategory.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("TaxSubCategory"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, taxSubCategory,
                        _ => _.TaxSubCategory.Name,
                        _ => _.TaxSubCategory.Description,
                        _ => _.TaxSubCategory.Code,
                        _ => _.TaxSubCategory.IsActive
                        );

                });
        }
    }
}
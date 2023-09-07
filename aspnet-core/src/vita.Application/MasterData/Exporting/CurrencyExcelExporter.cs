using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class CurrencyExcelExporter : NpoiExcelExporterBase, ICurrencyExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CurrencyExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCurrencyForViewDto> currency)
        {
            return CreateExcelPackage(
                "Currency.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Currency"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("NumericCode"),
                        L("MinorUnit"),
                        L("Country"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, currency,
                        _ => _.Currency.Name,
                        _ => _.Currency.Description,
                        _ => _.Currency.Code,
                        _ => _.Currency.NumericCode,
                        _ => _.Currency.MinorUnit,
                        _ => _.Currency.Country,
                        _ => _.Currency.IsActive
                        );

                });
        }
    }
}
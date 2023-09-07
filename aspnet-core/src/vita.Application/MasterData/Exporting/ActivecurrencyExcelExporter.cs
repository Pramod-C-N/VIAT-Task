using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class ActivecurrencyExcelExporter : NpoiExcelExporterBase, IActivecurrencyExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ActivecurrencyExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetActivecurrencyForViewDto> activecurrency)
        {
            return CreateExcelPackage(
                "Activecurrency.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Activecurrency"));

                    AddHeader(
                        sheet,
                        L("Entity"),
                        L("Currency"),
                        L("AlphabeticCode"),
                        L("NumericCode"),
                        L("MinorUnit"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, activecurrency,
                        _ => _.Activecurrency.Entity,
                        _ => _.Activecurrency.Currency,
                        _ => _.Activecurrency.AlphabeticCode,
                        _ => _.Activecurrency.NumericCode,
                        _ => _.Activecurrency.MinorUnit,
                        _ => _.Activecurrency.IsActive
                        );

                });
        }
    }
}
using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class CountryExcelExporter : NpoiExcelExporterBase, ICountryExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CountryExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCountryForViewDto> country)
        {
            return CreateExcelPackage(
                "Country.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Country"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("StateName"),
                        L("Sovereignty"),
                        L("AlphaCode"),
                        L("NumericCode"),
                        L("InternetCCTLD"),
                        L("SubDivisionCode"),
                        L("Alpha3Code"),
                        L("CountryGroup"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, country,
                        _ => _.Country.Name,
                        _ => _.Country.StateName,
                        _ => _.Country.Sovereignty,
                        _ => _.Country.AlphaCode,
                        _ => _.Country.NumericCode,
                        _ => _.Country.InternetCCTLD,
                        _ => _.Country.SubDivisionCode,
                        _ => _.Country.Alpha3Code,
                        _ => _.Country.CountryGroup,
                        _ => _.Country.IsActive
                        );

                });
        }
    }
}
using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class ConstitutionExcelExporter : NpoiExcelExporterBase, IConstitutionExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ConstitutionExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetConstitutionForViewDto> constitution)
        {
            return CreateExcelPackage(
                "Constitution.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Constitution"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, constitution,
                        _ => _.Constitution.Name,
                        _ => _.Constitution.Description,
                        _ => _.Constitution.Code,
                        _ => _.Constitution.IsActive
                        );

                });
        }
    }
}
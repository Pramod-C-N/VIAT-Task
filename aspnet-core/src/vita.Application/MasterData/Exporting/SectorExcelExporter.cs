using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class SectorExcelExporter : NpoiExcelExporterBase, ISectorExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SectorExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSectorForViewDto> sector)
        {
            return CreateExcelPackage(
                "Sector.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Sector"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("GroupName"),
                        L("IndustryGroupCode"),
                        L("IndustryGroupName"),
                        L("IndustryCode"),
                        L("IndustryName"),
                        L("SubIndustryCode"),
                        L("SubIndustryName"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, sector,
                        _ => _.Sector.Name,
                        _ => _.Sector.Description,
                        _ => _.Sector.Code,
                        _ => _.Sector.GroupName,
                        _ => _.Sector.IndustryGroupCode,
                        _ => _.Sector.IndustryGroupName,
                        _ => _.Sector.IndustryCode,
                        _ => _.Sector.IndustryName,
                        _ => _.Sector.SubIndustryCode,
                        _ => _.Sector.SubIndustryName,
                        _ => _.Sector.IsActive
                        );

                });
        }
    }
}
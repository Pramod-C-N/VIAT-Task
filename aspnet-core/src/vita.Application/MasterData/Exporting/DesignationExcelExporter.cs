using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class DesignationExcelExporter : NpoiExcelExporterBase, IDesignationExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DesignationExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDesignationForViewDto> designation)
        {
            return CreateExcelPackage(
                    "Designation.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("Designation"));

                        AddHeader(
                            sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                            );

                        AddObjects(
                            sheet, designation,
                        _ => _.Designation.Name,
                        _ => _.Designation.Description,
                        _ => _.Designation.Code,
                        _ => _.Designation.IsActive
                            );

                    });

        }
    }
}
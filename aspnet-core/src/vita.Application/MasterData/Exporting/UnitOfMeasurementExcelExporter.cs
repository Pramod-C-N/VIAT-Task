using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class UnitOfMeasurementExcelExporter : NpoiExcelExporterBase, IUnitOfMeasurementExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UnitOfMeasurementExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUnitOfMeasurementForViewDto> unitOfMeasurement)
        {
            return CreateExcelPackage(
                "UnitOfMeasurement.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("UnitOfMeasurement"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, unitOfMeasurement,
                        _ => _.UnitOfMeasurement.Name,
                        _ => _.UnitOfMeasurement.Description,
                        _ => _.UnitOfMeasurement.Code,
                        _ => _.UnitOfMeasurement.IsActive
                        );

                });
        }
    }
}
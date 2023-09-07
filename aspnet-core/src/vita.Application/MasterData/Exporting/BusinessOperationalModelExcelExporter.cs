using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class BusinessOperationalModelExcelExporter : NpoiExcelExporterBase, IBusinessOperationalModelExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessOperationalModelExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessOperationalModelForViewDto> businessOperationalModel)
        {
            return CreateExcelPackage(
                "BusinessOperationalModel.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("BusinessOperationalModel"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, businessOperationalModel,
                        _ => _.BusinessOperationalModel.Name,
                        _ => _.BusinessOperationalModel.Description,
                        _ => _.BusinessOperationalModel.Code,
                        _ => _.BusinessOperationalModel.IsActive
                        );

                });
        }
    }
}
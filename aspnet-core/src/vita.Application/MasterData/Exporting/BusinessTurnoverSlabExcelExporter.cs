using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class BusinessTurnoverSlabExcelExporter : NpoiExcelExporterBase, IBusinessTurnoverSlabExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessTurnoverSlabExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessTurnoverSlabForViewDto> businessTurnoverSlab)
        {
            return CreateExcelPackage(
                "BusinessTurnoverSlab.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("BusinessTurnoverSlab"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, businessTurnoverSlab,
                        _ => _.BusinessTurnoverSlab.Name,
                        _ => _.BusinessTurnoverSlab.Description,
                        _ => _.BusinessTurnoverSlab.Code,
                        _ => _.BusinessTurnoverSlab.IsActive
                        );

                });
        }
    }
}
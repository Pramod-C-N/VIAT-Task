using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class DocumentMasterExcelExporter : NpoiExcelExporterBase, IDocumentMasterExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DocumentMasterExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDocumentMasterForViewDto> documentMaster)
        {
            return CreateExcelPackage(
                "DocumentMaster.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("DocumentMaster"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("IsActive"),
                        L("Validformat")
                        );

                    AddObjects(
                        sheet, documentMaster,
                        _ => _.DocumentMaster.Name,
                        _ => _.DocumentMaster.Description,
                        _ => _.DocumentMaster.Code,
                        _ => _.DocumentMaster.IsActive,
                        _ => _.DocumentMaster.Validformat
                        );

                });
        }
    }
}
﻿using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using vita.DataExporting.Excel.NPOI;
using vita.MasterData.Dtos;
using vita.Dto;
using vita.Storage;

namespace vita.MasterData.Exporting
{
    public class GenderExcelExporter : NpoiExcelExporterBase, IGenderExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GenderExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGenderForViewDto> gender)
        {
            return CreateExcelPackage(
                "Gender.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Gender"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, gender,
                        _ => _.Gender.Name,
                        _ => _.Gender.IsActive
                        );

                });
        }
    }
}
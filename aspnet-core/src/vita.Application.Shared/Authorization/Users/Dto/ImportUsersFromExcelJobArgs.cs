using System;
using System.Collections.Generic;
using Abp;
using iText.Layout.Element;
using vita.ImportBatch.Dtos;
using vita.Sales.Dtos;

namespace vita.Authorization.Users.Dto
{
    public class ImportUsersFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }

        public string filename { get; set; }
        public DateTime? fromdate { get; set; } 
        public DateTime? todate { get; set; }
        public int? configurationId { get; set; }
        public List<GetPdfIrnData> Id { get; set; }
    }
}

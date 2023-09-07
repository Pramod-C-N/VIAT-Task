using System;
using Abp;
using vita.ImportBatch.Dtos;

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
        public CreateOrEditImportBatchDataDto sales { get; set; }
    }
}

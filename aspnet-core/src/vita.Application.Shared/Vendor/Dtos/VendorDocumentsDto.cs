using System;
using Abp.Application.Services.Dto;

namespace vita.Vendor.Dtos
{
    public class VendorDocumentsDto : EntityDto<long>
    {
        public string VendorID { get; set; }

        public Guid VendorUniqueIdentifier { get; set; }

        public string DocumentTypeCode { get; set; }

        public string DocumentName { get; set; }

        public string DocumentNumber { get; set; }

        public DateTime DoumentDate { get; set; }

        public string Status { get; set; }

    }
}
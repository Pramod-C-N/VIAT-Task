using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Vendor.Dtos
{
    public class CreateOrEditVendorDocumentsDto : EntityDto<long?>
    {

        public Guid UniqueId { get; set; }

        public string VendorID { get; set; }

        public Guid VendorUniqueIdentifier { get; set; }

        public string DocumentTypeCode { get; set; }

        public string DocumentName { get; set; }

        public string DocumentNumber { get; set; }

        public DateTime DoumentDate { get; set; }

        public string Status { get; set; }

    }
}
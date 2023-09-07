using System;
using Abp.Application.Services.Dto;

namespace vita.Customer.Dtos
{
    public class CustomerDocumentsDto : EntityDto<long>
    {
        public string CustomerID { get; set; }

        public Guid CustomerUniqueIdentifier { get; set; }

        public string DocumentTypeCode { get; set; }

        public string DocumentName { get; set; }

        public string DocumentNumber { get; set; }

        public DateTime DoumentDate { get; set; }

        public string Status { get; set; }

    }
}
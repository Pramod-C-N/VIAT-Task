using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Customer.Dtos
{
    public class CreateOrEditCustomerDocumentsDto : EntityDto<long?>
    {
        public Guid UniqueId { get; set; }

        public string CustomerID { get; set; }

        public Guid CustomerUniqueIdentifier { get; set; }

        public string DocumentTypeCode { get; set; }

        public string DocumentName { get; set; }

        public string DocumentNumber { get; set; }

        public DateTime DoumentDate { get; set; }

        public string Status { get; set; }

    }
}
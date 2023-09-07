using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Customer.Dtos
{
    public class CreateOrEditCustomerForeignEntityDto : EntityDto<long?>
    {

        public string CustomerID { get; set; }

        public Guid CustomerUniqueIdentifier { get; set; }

        public string ForeignEntityName { get; set; }

        public string ForeignEntityAddress { get; set; }

        public string LegalRepresentative { get; set; }

        public string Country { get; set; }

    }
}
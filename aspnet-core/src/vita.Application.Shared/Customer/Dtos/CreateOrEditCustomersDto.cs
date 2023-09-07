using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Xml;

namespace vita.Customer.Dtos
{
    public class CreateOrEditCustomersDto : EntityDto<long?>
    {

        public Guid UniqueId {get; set;}
        public string TenantType { get; set; }

        public string ConstitutionType { get; set; }

        public string Name { get; set; }

        public string LegalName { get; set; }

        public string ContactPerson { get; set; }

        public string ContactNumber { get; set; }

        public string EmailID { get; set; }

        public string Nationality { get; set; }

        public string Designation { get; set; }

        public CreateOrEditCustomerAddressDto Address { get; set; }

        public List<CreateOrEditCustomerDocumentsDto> Documents { get; set; }


        public CreateOrEditCustomerTaxDetailsDto Taxdetails { get; set; }

        public CreateOrEditCustomerForeignEntityDto Foreign { get; set; }
    }
}
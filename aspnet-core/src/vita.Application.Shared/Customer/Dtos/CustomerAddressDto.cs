using System;
using Abp.Application.Services.Dto;

namespace vita.Customer.Dtos
{
    public class CustomerAddressDto : EntityDto<long>
    {
        public string CustomerID { get; set; }

        public Guid CustomerUniqueIdentifier { get; set; }

        public string Street { get; set; }

        public string AdditionalStreet { get; set; }

        public string BuildingNo { get; set; }

        public string AdditionalNo { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string State { get; set; }

        public string Neighbourhood { get; set; }

        public string CountryCode { get; set; }

        public string Type { get; set; }

    }
}
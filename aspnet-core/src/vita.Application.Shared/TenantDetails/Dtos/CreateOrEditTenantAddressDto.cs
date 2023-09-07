using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.TenantDetails.Dtos
{
    public class CreateOrEditTenantAddressDto : EntityDto<int?>
    {

        public string AddressTypeId { get; set; }

        public string AddressType { get; set; }

        public string BuildingNo { get; set; }

        public string AdditionalBuildingNumber { get; set; }

        public string Street { get; set; }

        public string AdditionalStreet { get; set; }

        public string Neighbourhood { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }

    }
}
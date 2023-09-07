using System;
using Abp.Application.Services.Dto;

namespace vita.Debit.Dtos
{
    public class DebitNoteAddressDto : EntityDto<long>
    {
        public string IRNNo { get; set; }

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

        public string AdditionalData1 { get; set; }

        public string Language { get; set; }

    }
}
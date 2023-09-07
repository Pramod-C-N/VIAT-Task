using System;
using Abp.Application.Services.Dto;

namespace vita.Customer.Dtos
{
    public class CustomerOwnershipDetailsDto : EntityDto<long>
    {
        public string CustomerID { get; set; }

        public Guid CustomerUniqueIdentifier { get; set; }

        public string PartnerName { get; set; }

        public string PartnerConstitution { get; set; }

        public string PartnerNationality { get; set; }

        public decimal CapitalAmount { get; set; }

        public decimal CapitalShare { get; set; }

        public decimal ProfitShare { get; set; }

        public string RepresentativeName { get; set; }

    }
}
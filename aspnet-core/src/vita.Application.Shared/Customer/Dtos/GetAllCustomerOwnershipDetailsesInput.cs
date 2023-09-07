using Abp.Application.Services.Dto;
using System;

namespace vita.Customer.Dtos
{
    public class GetAllCustomerOwnershipDetailsesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CustomerIDFilter { get; set; }

        public Guid? CustomerUniqueIdentifierFilter { get; set; }

        public string PartnerNameFilter { get; set; }

        public string PartnerConstitutionFilter { get; set; }

        public string PartnerNationalityFilter { get; set; }

        public decimal? MaxCapitalAmountFilter { get; set; }
        public decimal? MinCapitalAmountFilter { get; set; }

        public decimal? MaxCapitalShareFilter { get; set; }
        public decimal? MinCapitalShareFilter { get; set; }

        public decimal? MaxProfitShareFilter { get; set; }
        public decimal? MinProfitShareFilter { get; set; }

        public string RepresentativeNameFilter { get; set; }

    }
}
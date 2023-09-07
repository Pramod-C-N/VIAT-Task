using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.TenantDetails.Dtos
{
    public class CreateOrEditTenantShareHoldersDto : EntityDto<int?>
    {


        public Guid ShareUniqueId { get; set; }
        public string PartnerName { get; set; }

        public string Designation { get; set; }

        public string Nationality { get; set; }

        public string CapitalAmount { get; set; }

        public string CapitalShare { get; set; }

        public string ProfitShare { get; set; }

        public string ConstitutionName { get; set; }

        public string RepresentativeName { get; set; }

        public string DomesticName { get; set; }

    }
}
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.MasterData.Dtos
{
    public class CreateOrEditApportionmentBaseDataDto : EntityDto<int?>
    {

        public DateTime? EffectiveFromDate { get; set; }

        public DateTime? EffectiveTillEndDate { get; set; }

        public decimal? TaxableSupply { get; set; }

        public decimal? TotalSupply { get; set; }

        public decimal? TaxablePurchase { get; set; }

        public decimal? TotalPurchase { get; set; }

        public string FinYear { get; set; }

        public decimal? TotalExemptSales { get; set; }

        public decimal? TotalExemptPurchase { get; set; }

        public decimal? MixedOverHeads { get; set; }

        public decimal? ApportionmentSupplies { get; set; }

        public decimal? ApportionmentPurchases { get; set; }

    }
}
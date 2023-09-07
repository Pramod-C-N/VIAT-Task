using Abp.Application.Services.Dto;
using System;

namespace vita.MasterData.Dtos
{
    public class GetAllApportionmentBaseDataInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public DateTime? MaxEffectiveFromDateFilter { get; set; }
        public DateTime? MinEffectiveFromDateFilter { get; set; }

        public DateTime? MaxEffectiveTillEndDateFilter { get; set; }
        public DateTime? MinEffectiveTillEndDateFilter { get; set; }

        public decimal? MaxTaxableSupplyFilter { get; set; }
        public decimal? MinTaxableSupplyFilter { get; set; }

        public decimal? MaxTotalSupplyFilter { get; set; }
        public decimal? MinTotalSupplyFilter { get; set; }

        public decimal? MaxTaxablePurchaseFilter { get; set; }
        public decimal? MinTaxablePurchaseFilter { get; set; }

        public decimal? MaxTotalPurchaseFilter { get; set; }
        public decimal? MinTotalPurchaseFilter { get; set; }

        public string FinYearFilter { get; set; }

        public decimal? MaxTotalExemptSalesFilter { get; set; }
        public decimal? MinTotalExemptSalesFilter { get; set; }

        public decimal? MaxTotalExemptPurchaseFilter { get; set; }
        public decimal? MinTotalExemptPurchaseFilter { get; set; }

        public decimal? MaxMixedOverHeadsFilter { get; set; }
        public decimal? MinMixedOverHeadsFilter { get; set; }

        public decimal? MaxApportionmentSuppliesFilter { get; set; }
        public decimal? MinApportionmentSuppliesFilter { get; set; }

        public decimal? MaxApportionmentPurchasesFilter { get; set; }
        public decimal? MinApportionmentPurchasesFilter { get; set; }

    }
}
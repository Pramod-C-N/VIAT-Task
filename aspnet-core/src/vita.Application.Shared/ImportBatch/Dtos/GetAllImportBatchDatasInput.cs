using Abp.Application.Services.Dto;
using System;

namespace vita.ImportBatch.Dtos
{
    public class GetAllImportBatchDatasInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxBatchIdFilter { get; set; }
        public int? MinBatchIdFilter { get; set; }

        public string FilenameFilter { get; set; }

        public string InvoiceTypeFilter { get; set; }

        public string IRNNoFilter { get; set; }

        public string InvoiceNumberFilter { get; set; }

        public DateTime? MaxIssueDateFilter { get; set; }
        public DateTime? MinIssueDateFilter { get; set; }

        public string IssueTimeFilter { get; set; }

        public string InvoiceCurrencyCodeFilter { get; set; }

        public string PurchaseOrderIdFilter { get; set; }

        public string ContractIdFilter { get; set; }

        public DateTime? MaxSupplyDateFilter { get; set; }
        public DateTime? MinSupplyDateFilter { get; set; }

        public DateTime? MaxSupplyEndDateFilter { get; set; }
        public DateTime? MinSupplyEndDateFilter { get; set; }

        public string BuyerMasterCodeFilter { get; set; }

        public string BuyerNameFilter { get; set; }

        public string BuyerVatCodeFilter { get; set; }

        public string BuyerContactFilter { get; set; }

        public string BuyerCountryCodeFilter { get; set; }

        public string InvoiceLineIdentifierFilter { get; set; }

        public string ItemMasterCodeFilter { get; set; }

        public string ItemNameFilter { get; set; }

        public string UOMFilter { get; set; }

        public decimal? MaxGrossPriceFilter { get; set; }
        public decimal? MinGrossPriceFilter { get; set; }

        public decimal? MaxDiscountFilter { get; set; }
        public decimal? MinDiscountFilter { get; set; }

        public decimal? MaxNetPriceFilter { get; set; }
        public decimal? MinNetPriceFilter { get; set; }

        public decimal? MaxQuantityFilter { get; set; }
        public decimal? MinQuantityFilter { get; set; }

        public decimal? MaxLineNetAmountFilter { get; set; }
        public decimal? MinLineNetAmountFilter { get; set; }

        public string VatCategoryCodeFilter { get; set; }

        public decimal? MaxVatRateFilter { get; set; }
        public decimal? MinVatRateFilter { get; set; }

        public string VatExemptionReasonCodeFilter { get; set; }

        public string VatExemptionReasonFilter { get; set; }

        public decimal? MaxVATLineAmountFilter { get; set; }
        public decimal? MinVATLineAmountFilter { get; set; }

        public decimal? MaxLineAmountInclusiveVATFilter { get; set; }
        public decimal? MinLineAmountInclusiveVATFilter { get; set; }

        public int? MaxProcessedFilter { get; set; }
        public int? MinProcessedFilter { get; set; }

        public string ErrorFilter { get; set; }

        public string BillingReferenceIdFilter { get; set; }

        public DateTime? MaxOrignalSupplyDateFilter { get; set; }
        public DateTime? MinOrignalSupplyDateFilter { get; set; }

        public string ReasonForCNFilter { get; set; }

        public string BillOfEntryFilter { get; set; }

        public DateTime? MaxBillOfEntryDateFilter { get; set; }
        public DateTime? MinBillOfEntryDateFilter { get; set; }

        public decimal? MaxCustomsPaidFilter { get; set; }
        public decimal? MinCustomsPaidFilter { get; set; }

        public decimal? MaxCustomTaxFilter { get; set; }
        public decimal? MinCustomTaxFilter { get; set; }

        public int? WHTApplicableFilter { get; set; }

        public string PurchaseNumberFilter { get; set; }

        public string PurchaseCategoryFilter { get; set; }

        public string LedgerHeaderFilter { get; set; }

        public string TransTypeFilter { get; set; }

        public decimal? MaxAdvanceRcptAmtAdjustedFilter { get; set; }
        public decimal? MinAdvanceRcptAmtAdjustedFilter { get; set; }

        public decimal? MaxVatOnAdvanceRcptAmtAdjustedFilter { get; set; }
        public decimal? MinVatOnAdvanceRcptAmtAdjustedFilter { get; set; }

        public string AdvanceRcptRefNoFilter { get; set; }

        public string PaymentMeansFilter { get; set; }

        public string PaymentTermsFilter { get; set; }

        public string NatureofServicesFilter { get; set; }

        public int? IsapportionmentFilter { get; set; }

        public decimal? MaxExciseTaxPaidFilter { get; set; }
        public decimal? MinExciseTaxPaidFilter { get; set; }

        public decimal? MaxOtherChargesPaidFilter { get; set; }
        public decimal? MinOtherChargesPaidFilter { get; set; }

        public decimal? MaxTotalTaxableAmountFilter { get; set; }
        public decimal? MinTotalTaxableAmountFilter { get; set; }

        public int? VATDefferedFilter { get; set; }

        public string PlaceofSupplyFilter { get; set; }

        public int? RCMApplicableFilter { get; set; }

        public string OrgTypeFilter { get; set; }

        public decimal? MaxExchangeRateFilter { get; set; }
        public decimal? MinExchangeRateFilter { get; set; }

        public string AffiliationStatusFilter { get; set; }

        public decimal? MaxReferenceInvoiceAmountFilter { get; set; }
        public decimal? MinReferenceInvoiceAmountFilter { get; set; }

        public string PerCapitaHoldingForiegnCoFilter { get; set; }

        public string CapitalInvestedbyForeignCompanyFilter { get; set; }

        public string CapitalInvestmentCurrencyFilter { get; set; }

        public DateTime? MaxCapitalInvestmentDateFilter { get; set; }
        public DateTime? MinCapitalInvestmentDateFilter { get; set; }

        public string VendorConstitutionFilter { get; set; }

    }
}
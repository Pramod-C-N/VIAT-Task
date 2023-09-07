using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace vita.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var draftVATDetails = pages.CreateChildPermission(AppPermissions.Pages_DraftVATDetails, L("DraftVATDetails"));
            draftVATDetails.CreateChildPermission(AppPermissions.Pages_DraftVATDetails_Create, L("CreateNewDraftVATDetail"));
            draftVATDetails.CreateChildPermission(AppPermissions.Pages_DraftVATDetails_Edit, L("EditDraftVATDetail"));
            draftVATDetails.CreateChildPermission(AppPermissions.Pages_DraftVATDetails_Delete, L("DeleteDraftVATDetail"));

            var draftSummaries = pages.CreateChildPermission(AppPermissions.Pages_DraftSummaries, L("DraftSummaries"));
            draftSummaries.CreateChildPermission(AppPermissions.Pages_DraftSummaries_Create, L("CreateNewDraftSummary"));
            draftSummaries.CreateChildPermission(AppPermissions.Pages_DraftSummaries_Edit, L("EditDraftSummary"));
            draftSummaries.CreateChildPermission(AppPermissions.Pages_DraftSummaries_Delete, L("DeleteDraftSummary"));

            var draftPaymentDetails = pages.CreateChildPermission(AppPermissions.Pages_DraftPaymentDetails, L("DraftPaymentDetails"));
            draftPaymentDetails.CreateChildPermission(AppPermissions.Pages_DraftPaymentDetails_Create, L("CreateNewDraftPaymentDetail"));
            draftPaymentDetails.CreateChildPermission(AppPermissions.Pages_DraftPaymentDetails_Edit, L("EditDraftPaymentDetail"));
            draftPaymentDetails.CreateChildPermission(AppPermissions.Pages_DraftPaymentDetails_Delete, L("DeleteDraftPaymentDetail"));

            var draftParties = pages.CreateChildPermission(AppPermissions.Pages_DraftParties, L("DraftParties"));
            draftParties.CreateChildPermission(AppPermissions.Pages_DraftParties_Create, L("CreateNewDraftParty"));
            draftParties.CreateChildPermission(AppPermissions.Pages_DraftParties_Edit, L("EditDraftParty"));
            draftParties.CreateChildPermission(AppPermissions.Pages_DraftParties_Delete, L("DeleteDraftParty"));

            var draftItems = pages.CreateChildPermission(AppPermissions.Pages_DraftItems, L("DraftItems"));
            draftItems.CreateChildPermission(AppPermissions.Pages_DraftItems_Create, L("CreateNewDraftItem"));
            draftItems.CreateChildPermission(AppPermissions.Pages_DraftItems_Edit, L("EditDraftItem"));
            draftItems.CreateChildPermission(AppPermissions.Pages_DraftItems_Delete, L("DeleteDraftItem"));

            var draftDiscounts = pages.CreateChildPermission(AppPermissions.Pages_DraftDiscounts, L("DraftDiscounts"));
            draftDiscounts.CreateChildPermission(AppPermissions.Pages_DraftDiscounts_Create, L("CreateNewDraftDiscount"));
            draftDiscounts.CreateChildPermission(AppPermissions.Pages_DraftDiscounts_Edit, L("EditDraftDiscount"));
            draftDiscounts.CreateChildPermission(AppPermissions.Pages_DraftDiscounts_Delete, L("DeleteDraftDiscount"));

            var draftContactPersons = pages.CreateChildPermission(AppPermissions.Pages_DraftContactPersons, L("DraftContactPersons"));
            draftContactPersons.CreateChildPermission(AppPermissions.Pages_DraftContactPersons_Create, L("CreateNewDraftContactPerson"));
            draftContactPersons.CreateChildPermission(AppPermissions.Pages_DraftContactPersons_Edit, L("EditDraftContactPerson"));
            draftContactPersons.CreateChildPermission(AppPermissions.Pages_DraftContactPersons_Delete, L("DeleteDraftContactPerson"));

            var draftAddresses = pages.CreateChildPermission(AppPermissions.Pages_DraftAddresses, L("DraftAddresses"));
            draftAddresses.CreateChildPermission(AppPermissions.Pages_DraftAddresses_Create, L("CreateNewDraftAddress"));
            draftAddresses.CreateChildPermission(AppPermissions.Pages_DraftAddresses_Edit, L("EditDraftAddress"));
            draftAddresses.CreateChildPermission(AppPermissions.Pages_DraftAddresses_Delete, L("DeleteDraftAddress"));

            var drafts = pages.CreateChildPermission(AppPermissions.Pages_Drafts, L("Drafts"));
            drafts.CreateChildPermission(AppPermissions.Pages_Drafts_Create, L("CreateNewDraft"));
            drafts.CreateChildPermission(AppPermissions.Pages_Drafts_Edit, L("EditDraft"));
            drafts.CreateChildPermission(AppPermissions.Pages_Drafts_Delete, L("DeleteDraft"));

            var tenantConfiguration = pages.CreateChildPermission(AppPermissions.Pages_TenantConfiguration, L("TenantConfiguration"));
            tenantConfiguration.CreateChildPermission(AppPermissions.Pages_TenantConfiguration_Create, L("CreateNewTenantConfiguration"));
            tenantConfiguration.CreateChildPermission(AppPermissions.Pages_TenantConfiguration_Edit, L("EditTenantConfiguration"));
            tenantConfiguration.CreateChildPermission(AppPermissions.Pages_TenantConfiguration_Delete, L("DeleteTenantConfiguration"));

            var tenantBankDetails = pages.CreateChildPermission(AppPermissions.Pages_TenantBankDetails, L("TenantBankDetails"));
            tenantBankDetails.CreateChildPermission(AppPermissions.Pages_TenantBankDetails_Create, L("CreateNewTenantBankDetail"));
            tenantBankDetails.CreateChildPermission(AppPermissions.Pages_TenantBankDetails_Edit, L("EditTenantBankDetail"));
            tenantBankDetails.CreateChildPermission(AppPermissions.Pages_TenantBankDetails_Delete, L("DeleteTenantBankDetail"));

            var purchaseDebitNoteDiscount = pages.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteDiscount, L("PurchaseDebitNoteDiscount"));
            purchaseDebitNoteDiscount.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteDiscount_Create, L("CreateNewPurchaseDebitNoteDiscount"));
            purchaseDebitNoteDiscount.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteDiscount_Edit, L("EditPurchaseDebitNoteDiscount"));
            purchaseDebitNoteDiscount.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteDiscount_Delete, L("DeletePurchaseDebitNoteDiscount"));

            var purchaseDebitNoteVATDetail = pages.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteVATDetail, L("PurchaseDebitNoteVATDetail"));
            purchaseDebitNoteVATDetail.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteVATDetail_Create, L("CreateNewPurchaseDebitNoteVATDetail"));
            purchaseDebitNoteVATDetail.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteVATDetail_Edit, L("EditPurchaseDebitNoteVATDetail"));
            purchaseDebitNoteVATDetail.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteVATDetail_Delete, L("DeletePurchaseDebitNoteVATDetail"));

            var purchaseDebitNoteContactPerson = pages.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteContactPerson, L("PurchaseDebitNoteContactPerson"));
            purchaseDebitNoteContactPerson.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteContactPerson_Create, L("CreateNewPurchaseDebitNoteContactPerson"));
            purchaseDebitNoteContactPerson.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteContactPerson_Edit, L("EditPurchaseDebitNoteContactPerson"));
            purchaseDebitNoteContactPerson.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteContactPerson_Delete, L("DeletePurchaseDebitNoteContactPerson"));

            var purchaseDebitNoteItem = pages.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteItem, L("PurchaseDebitNoteItem"));
            purchaseDebitNoteItem.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteItem_Create, L("CreateNewPurchaseDebitNoteItem"));
            purchaseDebitNoteItem.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteItem_Edit, L("EditPurchaseDebitNoteItem"));
            purchaseDebitNoteItem.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteItem_Delete, L("DeletePurchaseDebitNoteItem"));

            var purchaseDebitNotePaymentDetail = pages.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNotePaymentDetail, L("PurchaseDebitNotePaymentDetail"));
            purchaseDebitNotePaymentDetail.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNotePaymentDetail_Create, L("CreateNewPurchaseDebitNotePaymentDetail"));
            purchaseDebitNotePaymentDetail.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNotePaymentDetail_Edit, L("EditPurchaseDebitNotePaymentDetail"));
            purchaseDebitNotePaymentDetail.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNotePaymentDetail_Delete, L("DeletePurchaseDebitNotePaymentDetail"));

            var purchaseDebitNoteAddress = pages.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteAddress, L("PurchaseDebitNoteAddress"));
            purchaseDebitNoteAddress.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteAddress_Create, L("CreateNewPurchaseDebitNoteAddress"));
            purchaseDebitNoteAddress.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteAddress_Edit, L("EditPurchaseDebitNoteAddress"));
            purchaseDebitNoteAddress.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteAddress_Delete, L("DeletePurchaseDebitNoteAddress"));

            var purchaseDebitNoteParty = pages.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteParty, L("PurchaseDebitNoteParty"));
            purchaseDebitNoteParty.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteParty_Create, L("CreateNewPurchaseDebitNoteParty"));
            purchaseDebitNoteParty.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteParty_Edit, L("EditPurchaseDebitNoteParty"));
            purchaseDebitNoteParty.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteParty_Delete, L("DeletePurchaseDebitNoteParty"));

            var purchaseDebitNoteSummary = pages.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteSummary, L("PurchaseDebitNoteSummary"));
            purchaseDebitNoteSummary.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteSummary_Create, L("CreateNewPurchaseDebitNoteSummary"));
            purchaseDebitNoteSummary.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteSummary_Edit, L("EditPurchaseDebitNoteSummary"));
            purchaseDebitNoteSummary.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNoteSummary_Delete, L("DeletePurchaseDebitNoteSummary"));

            var purchaseDebitNote = pages.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNote, L("PurchaseDebitNote"));
            purchaseDebitNote.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNote_Create, L("CreateNewPurchaseDebitNote"));
            purchaseDebitNote.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNote_Edit, L("EditPurchaseDebitNote"));
            purchaseDebitNote.CreateChildPermission(AppPermissions.Pages_PurchaseDebitNote_Delete, L("DeletePurchaseDebitNote"));

            var purchaseCreditNoteDiscount = pages.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteDiscount, L("PurchaseCreditNoteDiscount"));
            purchaseCreditNoteDiscount.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteDiscount_Create, L("CreateNewPurchaseCreditNoteDiscount"));
            purchaseCreditNoteDiscount.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteDiscount_Edit, L("EditPurchaseCreditNoteDiscount"));
            purchaseCreditNoteDiscount.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteDiscount_Delete, L("DeletePurchaseCreditNoteDiscount"));

            var purchaseCreditNoteItem = pages.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteItem, L("PurchaseCreditNoteItem"));
            purchaseCreditNoteItem.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteItem_Create, L("CreateNewPurchaseCreditNoteItem"));
            purchaseCreditNoteItem.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteItem_Edit, L("EditPurchaseCreditNoteItem"));
            purchaseCreditNoteItem.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteItem_Delete, L("DeletePurchaseCreditNoteItem"));

            var purchaseCreditNoteVATDetail = pages.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteVATDetail, L("PurchaseCreditNoteVATDetail"));
            purchaseCreditNoteVATDetail.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteVATDetail_Create, L("CreateNewPurchaseCreditNoteVATDetail"));
            purchaseCreditNoteVATDetail.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteVATDetail_Edit, L("EditPurchaseCreditNoteVATDetail"));
            purchaseCreditNoteVATDetail.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteVATDetail_Delete, L("DeletePurchaseCreditNoteVATDetail"));

            var purchaseCreditNotePaymentDetail = pages.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNotePaymentDetail, L("PurchaseCreditNotePaymentDetail"));
            purchaseCreditNotePaymentDetail.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNotePaymentDetail_Create, L("CreateNewPurchaseCreditNotePaymentDetail"));
            purchaseCreditNotePaymentDetail.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNotePaymentDetail_Edit, L("EditPurchaseCreditNotePaymentDetail"));
            purchaseCreditNotePaymentDetail.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNotePaymentDetail_Delete, L("DeletePurchaseCreditNotePaymentDetail"));

            var purchaseCreditNoteContactPerson = pages.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteContactPerson, L("PurchaseCreditNoteContactPerson"));
            purchaseCreditNoteContactPerson.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteContactPerson_Create, L("CreateNewPurchaseCreditNoteContactPerson"));
            purchaseCreditNoteContactPerson.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteContactPerson_Edit, L("EditPurchaseCreditNoteContactPerson"));
            purchaseCreditNoteContactPerson.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteContactPerson_Delete, L("DeletePurchaseCreditNoteContactPerson"));

            var purchaseCreditNoteAddress = pages.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteAddress, L("PurchaseCreditNoteAddress"));
            purchaseCreditNoteAddress.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteAddress_Create, L("CreateNewPurchaseCreditNoteAddress"));
            purchaseCreditNoteAddress.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteAddress_Edit, L("EditPurchaseCreditNoteAddress"));
            purchaseCreditNoteAddress.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteAddress_Delete, L("DeletePurchaseCreditNoteAddress"));

            var purchaseCreditNoteParty = pages.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteParty, L("PurchaseCreditNoteParty"));
            purchaseCreditNoteParty.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteParty_Create, L("CreateNewPurchaseCreditNoteParty"));
            purchaseCreditNoteParty.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteParty_Edit, L("EditPurchaseCreditNoteParty"));
            purchaseCreditNoteParty.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteParty_Delete, L("DeletePurchaseCreditNoteParty"));

            var purchaseCreditNoteSummary = pages.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteSummary, L("PurchaseCreditNoteSummary"));
            purchaseCreditNoteSummary.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteSummary_Create, L("CreateNewPurchaseCreditNoteSummary"));
            purchaseCreditNoteSummary.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteSummary_Edit, L("EditPurchaseCreditNoteSummary"));
            purchaseCreditNoteSummary.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNoteSummary_Delete, L("DeletePurchaseCreditNoteSummary"));

            var purchaseCreditNote = pages.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNote, L("PurchaseCreditNote"));
            purchaseCreditNote.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNote_Create, L("CreateNewPurchaseCreditNote"));
            purchaseCreditNote.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNote_Edit, L("EditPurchaseCreditNote"));
            purchaseCreditNote.CreateChildPermission(AppPermissions.Pages_PurchaseCreditNote_Delete, L("DeletePurchaseCreditNote"));

            var designation = pages.CreateChildPermission(AppPermissions.Pages_Designation, L("Designation"));
            designation.CreateChildPermission(AppPermissions.Pages_Designation_Create, L("CreateNewDesignation"));
            designation.CreateChildPermission(AppPermissions.Pages_Designation_Edit, L("EditDesignation"));
            designation.CreateChildPermission(AppPermissions.Pages_Designation_Delete, L("DeleteDesignation"));

            var businessOperationalModel = pages.CreateChildPermission(AppPermissions.Pages_BusinessOperationalModel, L("BusinessOperationalModel"));
            businessOperationalModel.CreateChildPermission(AppPermissions.Pages_BusinessOperationalModel_Create, L("CreateNewBusinessOperationalModel"));
            businessOperationalModel.CreateChildPermission(AppPermissions.Pages_BusinessOperationalModel_Edit, L("EditBusinessOperationalModel"));
            businessOperationalModel.CreateChildPermission(AppPermissions.Pages_BusinessOperationalModel_Delete, L("DeleteBusinessOperationalModel"));

            var businessTurnoverSlab = pages.CreateChildPermission(AppPermissions.Pages_BusinessTurnoverSlab, L("BusinessTurnoverSlab"));
            businessTurnoverSlab.CreateChildPermission(AppPermissions.Pages_BusinessTurnoverSlab_Create, L("CreateNewBusinessTurnoverSlab"));
            businessTurnoverSlab.CreateChildPermission(AppPermissions.Pages_BusinessTurnoverSlab_Edit, L("EditBusinessTurnoverSlab"));
            businessTurnoverSlab.CreateChildPermission(AppPermissions.Pages_BusinessTurnoverSlab_Delete, L("DeleteBusinessTurnoverSlab"));

            var apportionmentBaseData = pages.CreateChildPermission(AppPermissions.Pages_ApportionmentBaseData, L("ApportionmentBaseData"));
            apportionmentBaseData.CreateChildPermission(AppPermissions.Pages_ApportionmentBaseData_Create, L("CreateNewApportionmentBaseData"));
            apportionmentBaseData.CreateChildPermission(AppPermissions.Pages_ApportionmentBaseData_Edit, L("EditApportionmentBaseData"));
            apportionmentBaseData.CreateChildPermission(AppPermissions.Pages_ApportionmentBaseData_Delete, L("DeleteApportionmentBaseData"));

            var customReport = pages.CreateChildPermission(AppPermissions.Pages_CustomReport, L("CustomReport"));
            customReport.CreateChildPermission(AppPermissions.Pages_CustomReport_Create, L("CreateNewCustomReport"));
            customReport.CreateChildPermission(AppPermissions.Pages_CustomReport_Edit, L("EditCustomReport"));
            customReport.CreateChildPermission(AppPermissions.Pages_CustomReport_Delete, L("DeleteCustomReport"));

            var tenantSectors = pages.CreateChildPermission(AppPermissions.Pages_TenantSectors, L("TenantSectors"));
            tenantSectors.CreateChildPermission(AppPermissions.Pages_TenantSectors_Create, L("CreateNewTenantSectors"));
            tenantSectors.CreateChildPermission(AppPermissions.Pages_TenantSectors_Edit, L("EditTenantSectors"));
            tenantSectors.CreateChildPermission(AppPermissions.Pages_TenantSectors_Delete, L("DeleteTenantSectors"));

            var tenantPurchaseVatCateory = pages.CreateChildPermission(AppPermissions.Pages_TenantPurchaseVatCateory, L("TenantPurchaseVatCateory"));
            tenantPurchaseVatCateory.CreateChildPermission(AppPermissions.Pages_TenantPurchaseVatCateory_Create, L("CreateNewTenantPurchaseVatCateory"));
            tenantPurchaseVatCateory.CreateChildPermission(AppPermissions.Pages_TenantPurchaseVatCateory_Edit, L("EditTenantPurchaseVatCateory"));
            tenantPurchaseVatCateory.CreateChildPermission(AppPermissions.Pages_TenantPurchaseVatCateory_Delete, L("DeleteTenantPurchaseVatCateory"));

            var tenantSupplyVATCategory = pages.CreateChildPermission(AppPermissions.Pages_TenantSupplyVATCategory, L("TenantSupplyVATCategory"));
            tenantSupplyVATCategory.CreateChildPermission(AppPermissions.Pages_TenantSupplyVATCategory_Create, L("CreateNewTenantSupplyVATCategory"));
            tenantSupplyVATCategory.CreateChildPermission(AppPermissions.Pages_TenantSupplyVATCategory_Edit, L("EditTenantSupplyVATCategory"));
            tenantSupplyVATCategory.CreateChildPermission(AppPermissions.Pages_TenantSupplyVATCategory_Delete, L("DeleteTenantSupplyVATCategory"));

            var tenantBusinessSupplies = pages.CreateChildPermission(AppPermissions.Pages_TenantBusinessSupplies, L("TenantBusinessSupplies"));
            tenantBusinessSupplies.CreateChildPermission(AppPermissions.Pages_TenantBusinessSupplies_Create, L("CreateNewTenantBusinessSupplies"));
            tenantBusinessSupplies.CreateChildPermission(AppPermissions.Pages_TenantBusinessSupplies_Edit, L("EditTenantBusinessSupplies"));
            tenantBusinessSupplies.CreateChildPermission(AppPermissions.Pages_TenantBusinessSupplies_Delete, L("DeleteTenantBusinessSupplies"));

            var tenantBusinessPurchase = pages.CreateChildPermission(AppPermissions.Pages_TenantBusinessPurchase, L("TenantBusinessPurchase"));
            tenantBusinessPurchase.CreateChildPermission(AppPermissions.Pages_TenantBusinessPurchase_Create, L("CreateNewTenantBusinessPurchase"));
            tenantBusinessPurchase.CreateChildPermission(AppPermissions.Pages_TenantBusinessPurchase_Edit, L("EditTenantBusinessPurchase"));
            tenantBusinessPurchase.CreateChildPermission(AppPermissions.Pages_TenantBusinessPurchase_Delete, L("DeleteTenantBusinessPurchase"));

            var tenantDocuments = pages.CreateChildPermission(AppPermissions.Pages_TenantDocuments, L("TenantDocuments"));
            tenantDocuments.CreateChildPermission(AppPermissions.Pages_TenantDocuments_Create, L("CreateNewTenantDocuments"));
            tenantDocuments.CreateChildPermission(AppPermissions.Pages_TenantDocuments_Edit, L("EditTenantDocuments"));
            tenantDocuments.CreateChildPermission(AppPermissions.Pages_TenantDocuments_Delete, L("DeleteTenantDocuments"));

            var tenantAddress = pages.CreateChildPermission(AppPermissions.Pages_TenantAddress, L("TenantAddress"));
            tenantAddress.CreateChildPermission(AppPermissions.Pages_TenantAddress_Create, L("CreateNewTenantAddress"));
            tenantAddress.CreateChildPermission(AppPermissions.Pages_TenantAddress_Edit, L("EditTenantAddress"));
            tenantAddress.CreateChildPermission(AppPermissions.Pages_TenantAddress_Delete, L("DeleteTenantAddress"));

            var tenantShareHolders = pages.CreateChildPermission(AppPermissions.Pages_TenantShareHolders, L("TenantShareHolders"));
            tenantShareHolders.CreateChildPermission(AppPermissions.Pages_TenantShareHolders_Create, L("CreateNewTenantShareHolders"));
            tenantShareHolders.CreateChildPermission(AppPermissions.Pages_TenantShareHolders_Edit, L("EditTenantShareHolders"));
            tenantShareHolders.CreateChildPermission(AppPermissions.Pages_TenantShareHolders_Delete, L("DeleteTenantShareHolders"));

            var tenantBasicDetails = pages.CreateChildPermission(AppPermissions.Pages_TenantBasicDetails, L("TenantBasicDetails"));
            tenantBasicDetails.CreateChildPermission(AppPermissions.Pages_TenantBasicDetails_Create, L("CreateNewTenantBasicDetails"));
            tenantBasicDetails.CreateChildPermission(AppPermissions.Pages_TenantBasicDetails_Edit, L("EditTenantBasicDetails"));
            tenantBasicDetails.CreateChildPermission(AppPermissions.Pages_TenantBasicDetails_Delete, L("DeleteTenantBasicDetails"));

            var activecurrency = pages.CreateChildPermission(AppPermissions.Pages_Activecurrency, L("Activecurrency"));
            activecurrency.CreateChildPermission(AppPermissions.Pages_Activecurrency_Create, L("CreateNewActivecurrency"));
            activecurrency.CreateChildPermission(AppPermissions.Pages_Activecurrency_Edit, L("EditActivecurrency"));
            activecurrency.CreateChildPermission(AppPermissions.Pages_Activecurrency_Delete, L("DeleteActivecurrency"));

            var batchData = pages.CreateChildPermission(AppPermissions.Pages_BatchData, L("BatchData"));
            batchData.CreateChildPermission(AppPermissions.Pages_BatchData_Create, L("CreateNewBatchData"));
            batchData.CreateChildPermission(AppPermissions.Pages_BatchData_Edit, L("EditBatchData"));
            batchData.CreateChildPermission(AppPermissions.Pages_BatchData_Delete, L("DeleteBatchData"));

            var vendorSectorDetails = pages.CreateChildPermission(AppPermissions.Pages_VendorSectorDetails, L("VendorSectorDetails"));
            vendorSectorDetails.CreateChildPermission(AppPermissions.Pages_VendorSectorDetails_Create, L("CreateNewVendorSectorDetail"));
            vendorSectorDetails.CreateChildPermission(AppPermissions.Pages_VendorSectorDetails_Edit, L("EditVendorSectorDetail"));
            vendorSectorDetails.CreateChildPermission(AppPermissions.Pages_VendorSectorDetails_Delete, L("DeleteVendorSectorDetail"));

            var vendorTaxDetailses = pages.CreateChildPermission(AppPermissions.Pages_VendorTaxDetailses, L("VendorTaxDetailses"));
            vendorTaxDetailses.CreateChildPermission(AppPermissions.Pages_VendorTaxDetailses_Create, L("CreateNewVendorTaxDetails"));
            vendorTaxDetailses.CreateChildPermission(AppPermissions.Pages_VendorTaxDetailses_Edit, L("EditVendorTaxDetails"));
            vendorTaxDetailses.CreateChildPermission(AppPermissions.Pages_VendorTaxDetailses_Delete, L("DeleteVendorTaxDetails"));

            var vendorForeignEntities = pages.CreateChildPermission(AppPermissions.Pages_VendorForeignEntities, L("VendorForeignEntities"));
            vendorForeignEntities.CreateChildPermission(AppPermissions.Pages_VendorForeignEntities_Create, L("CreateNewVendorForeignEntity"));
            vendorForeignEntities.CreateChildPermission(AppPermissions.Pages_VendorForeignEntities_Edit, L("EditVendorForeignEntity"));
            vendorForeignEntities.CreateChildPermission(AppPermissions.Pages_VendorForeignEntities_Delete, L("DeleteVendorForeignEntity"));

            var vendorOwnershipDetailses = pages.CreateChildPermission(AppPermissions.Pages_VendorOwnershipDetailses, L("VendorOwnershipDetailses"));
            vendorOwnershipDetailses.CreateChildPermission(AppPermissions.Pages_VendorOwnershipDetailses_Create, L("CreateNewVendorOwnershipDetails"));
            vendorOwnershipDetailses.CreateChildPermission(AppPermissions.Pages_VendorOwnershipDetailses_Edit, L("EditVendorOwnershipDetails"));
            vendorOwnershipDetailses.CreateChildPermission(AppPermissions.Pages_VendorOwnershipDetailses_Delete, L("DeleteVendorOwnershipDetails"));

            var vendorDocumentses = pages.CreateChildPermission(AppPermissions.Pages_VendorDocumentses, L("VendorDocumentses"));
            vendorDocumentses.CreateChildPermission(AppPermissions.Pages_VendorDocumentses_Create, L("CreateNewVendorDocuments"));
            vendorDocumentses.CreateChildPermission(AppPermissions.Pages_VendorDocumentses_Edit, L("EditVendorDocuments"));
            vendorDocumentses.CreateChildPermission(AppPermissions.Pages_VendorDocumentses_Delete, L("DeleteVendorDocuments"));

            var vendorContactPersons = pages.CreateChildPermission(AppPermissions.Pages_VendorContactPersons, L("VendorContactPersons"));
            vendorContactPersons.CreateChildPermission(AppPermissions.Pages_VendorContactPersons_Create, L("CreateNewVendorContactPerson"));
            vendorContactPersons.CreateChildPermission(AppPermissions.Pages_VendorContactPersons_Edit, L("EditVendorContactPerson"));
            vendorContactPersons.CreateChildPermission(AppPermissions.Pages_VendorContactPersons_Delete, L("DeleteVendorContactPerson"));

            var vendorAddresses = pages.CreateChildPermission(AppPermissions.Pages_VendorAddresses, L("VendorAddresses"));
            vendorAddresses.CreateChildPermission(AppPermissions.Pages_VendorAddresses_Create, L("CreateNewVendorAddress"));
            vendorAddresses.CreateChildPermission(AppPermissions.Pages_VendorAddresses_Edit, L("EditVendorAddress"));
            vendorAddresses.CreateChildPermission(AppPermissions.Pages_VendorAddresses_Delete, L("DeleteVendorAddress"));

            var vendorses = pages.CreateChildPermission(AppPermissions.Pages_Vendorses, L("Vendorses"));
            vendorses.CreateChildPermission(AppPermissions.Pages_Vendorses_Create, L("CreateNewVendors"));
            vendorses.CreateChildPermission(AppPermissions.Pages_Vendorses_Edit, L("EditVendors"));
            vendorses.CreateChildPermission(AppPermissions.Pages_Vendorses_Delete, L("DeleteVendors"));

            var customerSectorDetails = pages.CreateChildPermission(AppPermissions.Pages_CustomerSectorDetails, L("CustomerSectorDetails"));
            customerSectorDetails.CreateChildPermission(AppPermissions.Pages_CustomerSectorDetails_Create, L("CreateNewCustomerSectorDetail"));
            customerSectorDetails.CreateChildPermission(AppPermissions.Pages_CustomerSectorDetails_Edit, L("EditCustomerSectorDetail"));
            customerSectorDetails.CreateChildPermission(AppPermissions.Pages_CustomerSectorDetails_Delete, L("DeleteCustomerSectorDetail"));

            var customerTaxDetailses = pages.CreateChildPermission(AppPermissions.Pages_CustomerTaxDetailses, L("CustomerTaxDetailses"));
            customerTaxDetailses.CreateChildPermission(AppPermissions.Pages_CustomerTaxDetailses_Create, L("CreateNewCustomerTaxDetails"));
            customerTaxDetailses.CreateChildPermission(AppPermissions.Pages_CustomerTaxDetailses_Edit, L("EditCustomerTaxDetails"));
            customerTaxDetailses.CreateChildPermission(AppPermissions.Pages_CustomerTaxDetailses_Delete, L("DeleteCustomerTaxDetails"));

            var customerForeignEntities = pages.CreateChildPermission(AppPermissions.Pages_CustomerForeignEntities, L("CustomerForeignEntities"));
            customerForeignEntities.CreateChildPermission(AppPermissions.Pages_CustomerForeignEntities_Create, L("CreateNewCustomerForeignEntity"));
            customerForeignEntities.CreateChildPermission(AppPermissions.Pages_CustomerForeignEntities_Edit, L("EditCustomerForeignEntity"));
            customerForeignEntities.CreateChildPermission(AppPermissions.Pages_CustomerForeignEntities_Delete, L("DeleteCustomerForeignEntity"));

            var customerOwnershipDetailses = pages.CreateChildPermission(AppPermissions.Pages_CustomerOwnershipDetailses, L("CustomerOwnershipDetailses"));
            customerOwnershipDetailses.CreateChildPermission(AppPermissions.Pages_CustomerOwnershipDetailses_Create, L("CreateNewCustomerOwnershipDetails"));
            customerOwnershipDetailses.CreateChildPermission(AppPermissions.Pages_CustomerOwnershipDetailses_Edit, L("EditCustomerOwnershipDetails"));
            customerOwnershipDetailses.CreateChildPermission(AppPermissions.Pages_CustomerOwnershipDetailses_Delete, L("DeleteCustomerOwnershipDetails"));

            var customerDocumentses = pages.CreateChildPermission(AppPermissions.Pages_CustomerDocumentses, L("CustomerDocumentses"));
            customerDocumentses.CreateChildPermission(AppPermissions.Pages_CustomerDocumentses_Create, L("CreateNewCustomerDocuments"));
            customerDocumentses.CreateChildPermission(AppPermissions.Pages_CustomerDocumentses_Edit, L("EditCustomerDocuments"));
            customerDocumentses.CreateChildPermission(AppPermissions.Pages_CustomerDocumentses_Delete, L("DeleteCustomerDocuments"));

            var customerContactPersons = pages.CreateChildPermission(AppPermissions.Pages_CustomerContactPersons, L("CustomerContactPersons"));
            customerContactPersons.CreateChildPermission(AppPermissions.Pages_CustomerContactPersons_Create, L("CreateNewCustomerContactPerson"));
            customerContactPersons.CreateChildPermission(AppPermissions.Pages_CustomerContactPersons_Edit, L("EditCustomerContactPerson"));
            customerContactPersons.CreateChildPermission(AppPermissions.Pages_CustomerContactPersons_Delete, L("DeleteCustomerContactPerson"));

            var customerAddresses = pages.CreateChildPermission(AppPermissions.Pages_CustomerAddresses, L("CustomerAddresses"));
            customerAddresses.CreateChildPermission(AppPermissions.Pages_CustomerAddresses_Create, L("CreateNewCustomerAddress"));
            customerAddresses.CreateChildPermission(AppPermissions.Pages_CustomerAddresses_Edit, L("EditCustomerAddress"));
            customerAddresses.CreateChildPermission(AppPermissions.Pages_CustomerAddresses_Delete, L("DeleteCustomerAddress"));

            var customerses = pages.CreateChildPermission(AppPermissions.Pages_Customerses, L("Customerses"));
            customerses.CreateChildPermission(AppPermissions.Pages_Customerses_Create, L("CreateNewCustomers"));
            customerses.CreateChildPermission(AppPermissions.Pages_Customerses_Edit, L("EditCustomers"));
            customerses.CreateChildPermission(AppPermissions.Pages_Customerses_Delete, L("DeleteCustomers"));

            var importBatchDatas = pages.CreateChildPermission(AppPermissions.Pages_ImportBatchDatas, L("ImportBatchDatas"));
            importBatchDatas.CreateChildPermission(AppPermissions.Pages_ImportBatchDatas_Create, L("CreateNewImportBatchData"));
            importBatchDatas.CreateChildPermission(AppPermissions.Pages_ImportBatchDatas_Edit, L("EditImportBatchData"));
            importBatchDatas.CreateChildPermission(AppPermissions.Pages_ImportBatchDatas_Delete, L("DeleteImportBatchData"));

            var purchaseEntryParties = pages.CreateChildPermission(AppPermissions.Pages_PurchaseEntryParties, L("PurchaseEntryParties"));
            purchaseEntryParties.CreateChildPermission(AppPermissions.Pages_PurchaseEntryParties_Create, L("CreateNewPurchaseEntryParty"));
            purchaseEntryParties.CreateChildPermission(AppPermissions.Pages_PurchaseEntryParties_Edit, L("EditPurchaseEntryParty"));
            purchaseEntryParties.CreateChildPermission(AppPermissions.Pages_PurchaseEntryParties_Delete, L("DeletePurchaseEntryParty"));

            var purchaseEntrySummaries = pages.CreateChildPermission(AppPermissions.Pages_PurchaseEntrySummaries, L("PurchaseEntrySummaries"));
            purchaseEntrySummaries.CreateChildPermission(AppPermissions.Pages_PurchaseEntrySummaries_Create, L("CreateNewPurchaseEntrySummary"));
            purchaseEntrySummaries.CreateChildPermission(AppPermissions.Pages_PurchaseEntrySummaries_Edit, L("EditPurchaseEntrySummary"));
            purchaseEntrySummaries.CreateChildPermission(AppPermissions.Pages_PurchaseEntrySummaries_Delete, L("DeletePurchaseEntrySummary"));

            var purchaseEntryPaymentDetails = pages.CreateChildPermission(AppPermissions.Pages_PurchaseEntryPaymentDetails, L("PurchaseEntryPaymentDetails"));
            purchaseEntryPaymentDetails.CreateChildPermission(AppPermissions.Pages_PurchaseEntryPaymentDetails_Create, L("CreateNewPurchaseEntryPaymentDetail"));
            purchaseEntryPaymentDetails.CreateChildPermission(AppPermissions.Pages_PurchaseEntryPaymentDetails_Edit, L("EditPurchaseEntryPaymentDetail"));
            purchaseEntryPaymentDetails.CreateChildPermission(AppPermissions.Pages_PurchaseEntryPaymentDetails_Delete, L("DeletePurchaseEntryPaymentDetail"));

            var purchaseEntryItems = pages.CreateChildPermission(AppPermissions.Pages_PurchaseEntryItems, L("PurchaseEntryItems"));
            purchaseEntryItems.CreateChildPermission(AppPermissions.Pages_PurchaseEntryItems_Create, L("CreateNewPurchaseEntryItem"));
            purchaseEntryItems.CreateChildPermission(AppPermissions.Pages_PurchaseEntryItems_Edit, L("EditPurchaseEntryItem"));
            purchaseEntryItems.CreateChildPermission(AppPermissions.Pages_PurchaseEntryItems_Delete, L("DeletePurchaseEntryItem"));

            var purchaseEntryVATDetails = pages.CreateChildPermission(AppPermissions.Pages_PurchaseEntryVATDetails, L("PurchaseEntryVATDetails"));
            purchaseEntryVATDetails.CreateChildPermission(AppPermissions.Pages_PurchaseEntryVATDetails_Create, L("CreateNewPurchaseEntryVATDetail"));
            purchaseEntryVATDetails.CreateChildPermission(AppPermissions.Pages_PurchaseEntryVATDetails_Edit, L("EditPurchaseEntryVATDetail"));
            purchaseEntryVATDetails.CreateChildPermission(AppPermissions.Pages_PurchaseEntryVATDetails_Delete, L("DeletePurchaseEntryVATDetail"));

            var purchaseEntryDiscounts = pages.CreateChildPermission(AppPermissions.Pages_PurchaseEntryDiscounts, L("PurchaseEntryDiscounts"));
            purchaseEntryDiscounts.CreateChildPermission(AppPermissions.Pages_PurchaseEntryDiscounts_Create, L("CreateNewPurchaseEntryDiscount"));
            purchaseEntryDiscounts.CreateChildPermission(AppPermissions.Pages_PurchaseEntryDiscounts_Edit, L("EditPurchaseEntryDiscount"));
            purchaseEntryDiscounts.CreateChildPermission(AppPermissions.Pages_PurchaseEntryDiscounts_Delete, L("DeletePurchaseEntryDiscount"));

            var purchaseEntryContactPersons = pages.CreateChildPermission(AppPermissions.Pages_PurchaseEntryContactPersons, L("PurchaseEntryContactPersons"));
            purchaseEntryContactPersons.CreateChildPermission(AppPermissions.Pages_PurchaseEntryContactPersons_Create, L("CreateNewPurchaseEntryContactPerson"));
            purchaseEntryContactPersons.CreateChildPermission(AppPermissions.Pages_PurchaseEntryContactPersons_Edit, L("EditPurchaseEntryContactPerson"));
            purchaseEntryContactPersons.CreateChildPermission(AppPermissions.Pages_PurchaseEntryContactPersons_Delete, L("DeletePurchaseEntryContactPerson"));

            var purchaseEntryAddresses = pages.CreateChildPermission(AppPermissions.Pages_PurchaseEntryAddresses, L("PurchaseEntryAddresses"));
            purchaseEntryAddresses.CreateChildPermission(AppPermissions.Pages_PurchaseEntryAddresses_Create, L("CreateNewPurchaseEntryAddress"));
            purchaseEntryAddresses.CreateChildPermission(AppPermissions.Pages_PurchaseEntryAddresses_Edit, L("EditPurchaseEntryAddress"));
            purchaseEntryAddresses.CreateChildPermission(AppPermissions.Pages_PurchaseEntryAddresses_Delete, L("DeletePurchaseEntryAddress"));

            var purchaseEntries = pages.CreateChildPermission(AppPermissions.Pages_PurchaseEntries, L("PurchaseEntries"));
            purchaseEntries.CreateChildPermission(AppPermissions.Pages_PurchaseEntries_Create, L("CreateNewPurchaseEntry"));
            purchaseEntries.CreateChildPermission(AppPermissions.Pages_PurchaseEntries_Edit, L("EditPurchaseEntry"));
            purchaseEntries.CreateChildPermission(AppPermissions.Pages_PurchaseEntries_Delete, L("DeletePurchaseEntry"));

            var irnMasters = pages.CreateChildPermission(AppPermissions.Pages_IRNMasters, L("IRNMasters"));
            irnMasters.CreateChildPermission(AppPermissions.Pages_IRNMasters_Create, L("CreateNewIRNMaster"));
            irnMasters.CreateChildPermission(AppPermissions.Pages_IRNMasters_Edit, L("EditIRNMaster"));
            irnMasters.CreateChildPermission(AppPermissions.Pages_IRNMasters_Delete, L("DeleteIRNMaster"));
            var creditNoteParty = pages.CreateChildPermission(AppPermissions.Pages_CreditNoteParty, L("CreditNoteParty"));
            creditNoteParty.CreateChildPermission(AppPermissions.Pages_CreditNoteParty_Create, L("CreateNewCreditNoteParty"));
            creditNoteParty.CreateChildPermission(AppPermissions.Pages_CreditNoteParty_Edit, L("EditCreditNoteParty"));
            creditNoteParty.CreateChildPermission(AppPermissions.Pages_CreditNoteParty_Delete, L("DeleteCreditNoteParty"));

            var creditNoteSummary = pages.CreateChildPermission(AppPermissions.Pages_CreditNoteSummary, L("CreditNoteSummary"));
            creditNoteSummary.CreateChildPermission(AppPermissions.Pages_CreditNoteSummary_Create, L("CreateNewCreditNoteSummary"));
            creditNoteSummary.CreateChildPermission(AppPermissions.Pages_CreditNoteSummary_Edit, L("EditCreditNoteSummary"));
            creditNoteSummary.CreateChildPermission(AppPermissions.Pages_CreditNoteSummary_Delete, L("DeleteCreditNoteSummary"));

            var creditNotePaymentDetail = pages.CreateChildPermission(AppPermissions.Pages_CreditNotePaymentDetail, L("CreditNotePaymentDetail"));
            creditNotePaymentDetail.CreateChildPermission(AppPermissions.Pages_CreditNotePaymentDetail_Create, L("CreateNewCreditNotePaymentDetail"));
            creditNotePaymentDetail.CreateChildPermission(AppPermissions.Pages_CreditNotePaymentDetail_Edit, L("EditCreditNotePaymentDetail"));
            creditNotePaymentDetail.CreateChildPermission(AppPermissions.Pages_CreditNotePaymentDetail_Delete, L("DeleteCreditNotePaymentDetail"));

            var creditNoteItem = pages.CreateChildPermission(AppPermissions.Pages_CreditNoteItem, L("CreditNoteItem"));
            creditNoteItem.CreateChildPermission(AppPermissions.Pages_CreditNoteItem_Create, L("CreateNewCreditNoteItem"));
            creditNoteItem.CreateChildPermission(AppPermissions.Pages_CreditNoteItem_Edit, L("EditCreditNoteItem"));
            creditNoteItem.CreateChildPermission(AppPermissions.Pages_CreditNoteItem_Delete, L("DeleteCreditNoteItem"));

            var creditNoteVATDetail = pages.CreateChildPermission(AppPermissions.Pages_CreditNoteVATDetail, L("CreditNoteVATDetail"));
            creditNoteVATDetail.CreateChildPermission(AppPermissions.Pages_CreditNoteVATDetail_Create, L("CreateNewCreditNoteVATDetail"));
            creditNoteVATDetail.CreateChildPermission(AppPermissions.Pages_CreditNoteVATDetail_Edit, L("EditCreditNoteVATDetail"));
            creditNoteVATDetail.CreateChildPermission(AppPermissions.Pages_CreditNoteVATDetail_Delete, L("DeleteCreditNoteVATDetail"));

            var creditNoteDiscount = pages.CreateChildPermission(AppPermissions.Pages_CreditNoteDiscount, L("CreditNoteDiscount"));
            creditNoteDiscount.CreateChildPermission(AppPermissions.Pages_CreditNoteDiscount_Create, L("CreateNewCreditNoteDiscount"));
            creditNoteDiscount.CreateChildPermission(AppPermissions.Pages_CreditNoteDiscount_Edit, L("EditCreditNoteDiscount"));
            creditNoteDiscount.CreateChildPermission(AppPermissions.Pages_CreditNoteDiscount_Delete, L("DeleteCreditNoteDiscount"));

            var creditNoteContactPerson = pages.CreateChildPermission(AppPermissions.Pages_CreditNoteContactPerson, L("CreditNoteContactPerson"));
            creditNoteContactPerson.CreateChildPermission(AppPermissions.Pages_CreditNoteContactPerson_Create, L("CreateNewCreditNoteContactPerson"));
            creditNoteContactPerson.CreateChildPermission(AppPermissions.Pages_CreditNoteContactPerson_Edit, L("EditCreditNoteContactPerson"));
            creditNoteContactPerson.CreateChildPermission(AppPermissions.Pages_CreditNoteContactPerson_Delete, L("DeleteCreditNoteContactPerson"));

            var creditNoteAddress = pages.CreateChildPermission(AppPermissions.Pages_CreditNoteAddress, L("CreditNoteAddress"));
            creditNoteAddress.CreateChildPermission(AppPermissions.Pages_CreditNoteAddress_Create, L("CreateNewCreditNoteAddress"));
            creditNoteAddress.CreateChildPermission(AppPermissions.Pages_CreditNoteAddress_Edit, L("EditCreditNoteAddress"));
            creditNoteAddress.CreateChildPermission(AppPermissions.Pages_CreditNoteAddress_Delete, L("DeleteCreditNoteAddress"));

            var creditNote = pages.CreateChildPermission(AppPermissions.Pages_CreditNote, L("CreditNote"));
            creditNote.CreateChildPermission(AppPermissions.Pages_CreditNote_Create, L("CreateNewCreditNote"));
            creditNote.CreateChildPermission(AppPermissions.Pages_CreditNote_Edit, L("EditCreditNote"));
            creditNote.CreateChildPermission(AppPermissions.Pages_CreditNote_Delete, L("DeleteCreditNote"));
            var debitNoteParties = pages.CreateChildPermission(AppPermissions.Pages_DebitNoteParties, L("DebitNoteParties"));
            debitNoteParties.CreateChildPermission(AppPermissions.Pages_DebitNoteParties_Create, L("CreateNewDebitNoteParty"));
            debitNoteParties.CreateChildPermission(AppPermissions.Pages_DebitNoteParties_Edit, L("EditDebitNoteParty"));
            debitNoteParties.CreateChildPermission(AppPermissions.Pages_DebitNoteParties_Delete, L("DeleteDebitNoteParty"));

            var debitNoteSummaries = pages.CreateChildPermission(AppPermissions.Pages_DebitNoteSummaries, L("DebitNoteSummaries"));
            debitNoteSummaries.CreateChildPermission(AppPermissions.Pages_DebitNoteSummaries_Create, L("CreateNewDebitNoteSummary"));
            debitNoteSummaries.CreateChildPermission(AppPermissions.Pages_DebitNoteSummaries_Edit, L("EditDebitNoteSummary"));
            debitNoteSummaries.CreateChildPermission(AppPermissions.Pages_DebitNoteSummaries_Delete, L("DeleteDebitNoteSummary"));

            var debitNotePaymentDetails = pages.CreateChildPermission(AppPermissions.Pages_DebitNotePaymentDetails, L("DebitNotePaymentDetails"));
            debitNotePaymentDetails.CreateChildPermission(AppPermissions.Pages_DebitNotePaymentDetails_Create, L("CreateNewDebitNotePaymentDetail"));
            debitNotePaymentDetails.CreateChildPermission(AppPermissions.Pages_DebitNotePaymentDetails_Edit, L("EditDebitNotePaymentDetail"));
            debitNotePaymentDetails.CreateChildPermission(AppPermissions.Pages_DebitNotePaymentDetails_Delete, L("DeleteDebitNotePaymentDetail"));

            var debitNoteItems = pages.CreateChildPermission(AppPermissions.Pages_DebitNoteItems, L("DebitNoteItems"));
            debitNoteItems.CreateChildPermission(AppPermissions.Pages_DebitNoteItems_Create, L("CreateNewDebitNoteItem"));
            debitNoteItems.CreateChildPermission(AppPermissions.Pages_DebitNoteItems_Edit, L("EditDebitNoteItem"));
            debitNoteItems.CreateChildPermission(AppPermissions.Pages_DebitNoteItems_Delete, L("DeleteDebitNoteItem"));

            var debitNoteVATDetails = pages.CreateChildPermission(AppPermissions.Pages_DebitNoteVATDetails, L("DebitNoteVATDetails"));
            debitNoteVATDetails.CreateChildPermission(AppPermissions.Pages_DebitNoteVATDetails_Create, L("CreateNewDebitNoteVATDetail"));
            debitNoteVATDetails.CreateChildPermission(AppPermissions.Pages_DebitNoteVATDetails_Edit, L("EditDebitNoteVATDetail"));
            debitNoteVATDetails.CreateChildPermission(AppPermissions.Pages_DebitNoteVATDetails_Delete, L("DeleteDebitNoteVATDetail"));

            var debitNoteDiscounts = pages.CreateChildPermission(AppPermissions.Pages_DebitNoteDiscounts, L("DebitNoteDiscounts"));
            debitNoteDiscounts.CreateChildPermission(AppPermissions.Pages_DebitNoteDiscounts_Create, L("CreateNewDebitNoteDiscount"));
            debitNoteDiscounts.CreateChildPermission(AppPermissions.Pages_DebitNoteDiscounts_Edit, L("EditDebitNoteDiscount"));
            debitNoteDiscounts.CreateChildPermission(AppPermissions.Pages_DebitNoteDiscounts_Delete, L("DeleteDebitNoteDiscount"));

            var debitNoteContactPersons = pages.CreateChildPermission(AppPermissions.Pages_DebitNoteContactPersons, L("DebitNoteContactPersons"));
            debitNoteContactPersons.CreateChildPermission(AppPermissions.Pages_DebitNoteContactPersons_Create, L("CreateNewDebitNoteContactPerson"));
            debitNoteContactPersons.CreateChildPermission(AppPermissions.Pages_DebitNoteContactPersons_Edit, L("EditDebitNoteContactPerson"));
            debitNoteContactPersons.CreateChildPermission(AppPermissions.Pages_DebitNoteContactPersons_Delete, L("DeleteDebitNoteContactPerson"));

            var debitNoteAddresses = pages.CreateChildPermission(AppPermissions.Pages_DebitNoteAddresses, L("DebitNoteAddresses"));
            debitNoteAddresses.CreateChildPermission(AppPermissions.Pages_DebitNoteAddresses_Create, L("CreateNewDebitNoteAddress"));
            debitNoteAddresses.CreateChildPermission(AppPermissions.Pages_DebitNoteAddresses_Edit, L("EditDebitNoteAddress"));
            debitNoteAddresses.CreateChildPermission(AppPermissions.Pages_DebitNoteAddresses_Delete, L("DeleteDebitNoteAddress"));

            var debitNotes = pages.CreateChildPermission(AppPermissions.Pages_DebitNotes, L("DebitNotes"));
            debitNotes.CreateChildPermission(AppPermissions.Pages_DebitNotes_Create, L("CreateNewDebitNote"));
            debitNotes.CreateChildPermission(AppPermissions.Pages_DebitNotes_Edit, L("EditDebitNote"));
            debitNotes.CreateChildPermission(AppPermissions.Pages_DebitNotes_Delete, L("DeleteDebitNote"));

            var invoiceType = pages.CreateChildPermission(AppPermissions.Pages_InvoiceType, L("InvoiceType"));
            invoiceType.CreateChildPermission(AppPermissions.Pages_InvoiceType_Create, L("CreateNewInvoiceType"));
            invoiceType.CreateChildPermission(AppPermissions.Pages_InvoiceType_Edit, L("EditInvoiceType"));
            invoiceType.CreateChildPermission(AppPermissions.Pages_InvoiceType_Delete, L("DeleteInvoiceType"));

            var financialYear = pages.CreateChildPermission(AppPermissions.Pages_FinancialYear, L("FinancialYear"));
            financialYear.CreateChildPermission(AppPermissions.Pages_FinancialYear_Create, L("CreateNewFinancialYear"));
            financialYear.CreateChildPermission(AppPermissions.Pages_FinancialYear_Edit, L("EditFinancialYear"));
            financialYear.CreateChildPermission(AppPermissions.Pages_FinancialYear_Delete, L("DeleteFinancialYear"));

            var errorType = pages.CreateChildPermission(AppPermissions.Pages_ErrorType, L("ErrorType"));
            errorType.CreateChildPermission(AppPermissions.Pages_ErrorType_Create, L("CreateNewErrorType"));
            errorType.CreateChildPermission(AppPermissions.Pages_ErrorType_Edit, L("EditErrorType"));
            errorType.CreateChildPermission(AppPermissions.Pages_ErrorType_Delete, L("DeleteErrorType"));

            var headOfPayment = pages.CreateChildPermission(AppPermissions.Pages_HeadOfPayment, L("HeadOfPayment"));
            headOfPayment.CreateChildPermission(AppPermissions.Pages_HeadOfPayment_Create, L("CreateNewHeadOfPayment"));
            headOfPayment.CreateChildPermission(AppPermissions.Pages_HeadOfPayment_Edit, L("EditHeadOfPayment"));
            headOfPayment.CreateChildPermission(AppPermissions.Pages_HeadOfPayment_Delete, L("DeleteHeadOfPayment"));

            var currency = pages.CreateChildPermission(AppPermissions.Pages_Currency, L("Currency"));
            currency.CreateChildPermission(AppPermissions.Pages_Currency_Create, L("CreateNewCurrency"));
            currency.CreateChildPermission(AppPermissions.Pages_Currency_Edit, L("EditCurrency"));
            currency.CreateChildPermission(AppPermissions.Pages_Currency_Delete, L("DeleteCurrency"));

            var taxCategory = pages.CreateChildPermission(AppPermissions.Pages_TaxCategory, L("TaxCategory"));
            taxCategory.CreateChildPermission(AppPermissions.Pages_TaxCategory_Create, L("CreateNewTaxCategory"));
            taxCategory.CreateChildPermission(AppPermissions.Pages_TaxCategory_Edit, L("EditTaxCategory"));
            taxCategory.CreateChildPermission(AppPermissions.Pages_TaxCategory_Delete, L("DeleteTaxCategory"));

            var invoiceCategory = pages.CreateChildPermission(AppPermissions.Pages_InvoiceCategory, L("InvoiceCategory"));
            invoiceCategory.CreateChildPermission(AppPermissions.Pages_InvoiceCategory_Create, L("CreateNewInvoiceCategory"));
            invoiceCategory.CreateChildPermission(AppPermissions.Pages_InvoiceCategory_Edit, L("EditInvoiceCategory"));
            invoiceCategory.CreateChildPermission(AppPermissions.Pages_InvoiceCategory_Delete, L("DeleteInvoiceCategory"));

            var errorGroup = pages.CreateChildPermission(AppPermissions.Pages_ErrorGroup, L("ErrorGroup"));
            errorGroup.CreateChildPermission(AppPermissions.Pages_ErrorGroup_Create, L("CreateNewErrorGroup"));
            errorGroup.CreateChildPermission(AppPermissions.Pages_ErrorGroup_Edit, L("EditErrorGroup"));
            errorGroup.CreateChildPermission(AppPermissions.Pages_ErrorGroup_Delete, L("DeleteErrorGroup"));

            var affiliation = pages.CreateChildPermission(AppPermissions.Pages_Affiliation, L("Affiliation"));
            affiliation.CreateChildPermission(AppPermissions.Pages_Affiliation_Create, L("CreateNewAffiliation"));
            affiliation.CreateChildPermission(AppPermissions.Pages_Affiliation_Edit, L("EditAffiliation"));
            affiliation.CreateChildPermission(AppPermissions.Pages_Affiliation_Delete, L("DeleteAffiliation"));

            var placeOfPerformance = pages.CreateChildPermission(AppPermissions.Pages_PlaceOfPerformance, L("PlaceOfPerformance"));
            placeOfPerformance.CreateChildPermission(AppPermissions.Pages_PlaceOfPerformance_Create, L("CreateNewPlaceOfPerformance"));
            placeOfPerformance.CreateChildPermission(AppPermissions.Pages_PlaceOfPerformance_Edit, L("EditPlaceOfPerformance"));
            placeOfPerformance.CreateChildPermission(AppPermissions.Pages_PlaceOfPerformance_Delete, L("DeletePlaceOfPerformance"));

            var organisationType = pages.CreateChildPermission(AppPermissions.Pages_OrganisationType, L("OrganisationType"));
            organisationType.CreateChildPermission(AppPermissions.Pages_OrganisationType_Create, L("CreateNewOrganisationType"));
            organisationType.CreateChildPermission(AppPermissions.Pages_OrganisationType_Edit, L("EditOrganisationType"));
            organisationType.CreateChildPermission(AppPermissions.Pages_OrganisationType_Delete, L("DeleteOrganisationType"));

            var purchaseType = pages.CreateChildPermission(AppPermissions.Pages_PurchaseType, L("PurchaseType"));
            purchaseType.CreateChildPermission(AppPermissions.Pages_PurchaseType_Create, L("CreateNewPurchaseType"));
            purchaseType.CreateChildPermission(AppPermissions.Pages_PurchaseType_Edit, L("EditPurchaseType"));
            purchaseType.CreateChildPermission(AppPermissions.Pages_PurchaseType_Delete, L("DeletePurchaseType"));

            var allowanceReason = pages.CreateChildPermission(AppPermissions.Pages_AllowanceReason, L("AllowanceReason"));
            allowanceReason.CreateChildPermission(AppPermissions.Pages_AllowanceReason_Create, L("CreateNewAllowanceReason"));
            allowanceReason.CreateChildPermission(AppPermissions.Pages_AllowanceReason_Edit, L("EditAllowanceReason"));
            allowanceReason.CreateChildPermission(AppPermissions.Pages_AllowanceReason_Delete, L("DeleteAllowanceReason"));

            var unitOfMeasurement = pages.CreateChildPermission(AppPermissions.Pages_UnitOfMeasurement, L("UnitOfMeasurement"));
            unitOfMeasurement.CreateChildPermission(AppPermissions.Pages_UnitOfMeasurement_Create, L("CreateNewUnitOfMeasurement"));
            unitOfMeasurement.CreateChildPermission(AppPermissions.Pages_UnitOfMeasurement_Edit, L("EditUnitOfMeasurement"));
            unitOfMeasurement.CreateChildPermission(AppPermissions.Pages_UnitOfMeasurement_Delete, L("DeleteUnitOfMeasurement"));

            var natureofServices = pages.CreateChildPermission(AppPermissions.Pages_NatureofServices, L("NatureofServices"));
            natureofServices.CreateChildPermission(AppPermissions.Pages_NatureofServices_Create, L("CreateNewNatureofServices"));
            natureofServices.CreateChildPermission(AppPermissions.Pages_NatureofServices_Edit, L("EditNatureofServices"));
            natureofServices.CreateChildPermission(AppPermissions.Pages_NatureofServices_Delete, L("DeleteNatureofServices"));

            var exemptionReason = pages.CreateChildPermission(AppPermissions.Pages_ExemptionReason, L("ExemptionReason"));
            exemptionReason.CreateChildPermission(AppPermissions.Pages_ExemptionReason_Create, L("CreateNewExemptionReason"));
            exemptionReason.CreateChildPermission(AppPermissions.Pages_ExemptionReason_Edit, L("EditExemptionReason"));
            exemptionReason.CreateChildPermission(AppPermissions.Pages_ExemptionReason_Delete, L("DeleteExemptionReason"));

            var reasonCNDN = pages.CreateChildPermission(AppPermissions.Pages_ReasonCNDN, L("ReasonCNDN"));
            reasonCNDN.CreateChildPermission(AppPermissions.Pages_ReasonCNDN_Create, L("CreateNewReasonCNDN"));
            reasonCNDN.CreateChildPermission(AppPermissions.Pages_ReasonCNDN_Edit, L("EditReasonCNDN"));
            reasonCNDN.CreateChildPermission(AppPermissions.Pages_ReasonCNDN_Delete, L("DeleteReasonCNDN"));

            var documentMaster = pages.CreateChildPermission(AppPermissions.Pages_DocumentMaster, L("DocumentMaster"));
            documentMaster.CreateChildPermission(AppPermissions.Pages_DocumentMaster_Create, L("CreateNewDocumentMaster"));
            documentMaster.CreateChildPermission(AppPermissions.Pages_DocumentMaster_Edit, L("EditDocumentMaster"));
            documentMaster.CreateChildPermission(AppPermissions.Pages_DocumentMaster_Delete, L("DeleteDocumentMaster"));

            var paymentMeans = pages.CreateChildPermission(AppPermissions.Pages_PaymentMeans, L("PaymentMeans"));
            paymentMeans.CreateChildPermission(AppPermissions.Pages_PaymentMeans_Create, L("CreateNewPaymentMeans"));
            paymentMeans.CreateChildPermission(AppPermissions.Pages_PaymentMeans_Edit, L("EditPaymentMeans"));
            paymentMeans.CreateChildPermission(AppPermissions.Pages_PaymentMeans_Delete, L("DeletePaymentMeans"));

            var businessProcess = pages.CreateChildPermission(AppPermissions.Pages_BusinessProcess, L("BusinessProcess"));
            businessProcess.CreateChildPermission(AppPermissions.Pages_BusinessProcess_Create, L("CreateNewBusinessProcess"));
            businessProcess.CreateChildPermission(AppPermissions.Pages_BusinessProcess_Edit, L("EditBusinessProcess"));
            businessProcess.CreateChildPermission(AppPermissions.Pages_BusinessProcess_Delete, L("DeleteBusinessProcess"));

            var taxSubCategory = pages.CreateChildPermission(AppPermissions.Pages_TaxSubCategory, L("TaxSubCategory"));
            taxSubCategory.CreateChildPermission(AppPermissions.Pages_TaxSubCategory_Create, L("CreateNewTaxSubCategory"));
            taxSubCategory.CreateChildPermission(AppPermissions.Pages_TaxSubCategory_Edit, L("EditTaxSubCategory"));
            taxSubCategory.CreateChildPermission(AppPermissions.Pages_TaxSubCategory_Delete, L("DeleteTaxSubCategory"));

            var country = pages.CreateChildPermission(AppPermissions.Pages_Country, L("Country"));
            country.CreateChildPermission(AppPermissions.Pages_Country_Create, L("CreateNewCountry"));
            country.CreateChildPermission(AppPermissions.Pages_Country_Edit, L("EditCountry"));
            country.CreateChildPermission(AppPermissions.Pages_Country_Delete, L("DeleteCountry"));

            var salesInvoicePaymentDetails = pages.CreateChildPermission(AppPermissions.Pages_SalesInvoicePaymentDetails, L("SalesInvoicePaymentDetails"));
            salesInvoicePaymentDetails.CreateChildPermission(AppPermissions.Pages_SalesInvoicePaymentDetails_Create, L("CreateNewSalesInvoicePaymentDetail"));
            salesInvoicePaymentDetails.CreateChildPermission(AppPermissions.Pages_SalesInvoicePaymentDetails_Edit, L("EditSalesInvoicePaymentDetail"));
            salesInvoicePaymentDetails.CreateChildPermission(AppPermissions.Pages_SalesInvoicePaymentDetails_Delete, L("DeleteSalesInvoicePaymentDetail"));

            var salesInvoiceVATDetails = pages.CreateChildPermission(AppPermissions.Pages_SalesInvoiceVATDetails, L("SalesInvoiceVATDetails"));
            salesInvoiceVATDetails.CreateChildPermission(AppPermissions.Pages_SalesInvoiceVATDetails_Create, L("CreateNewSalesInvoiceVATDetail"));
            salesInvoiceVATDetails.CreateChildPermission(AppPermissions.Pages_SalesInvoiceVATDetails_Edit, L("EditSalesInvoiceVATDetail"));
            salesInvoiceVATDetails.CreateChildPermission(AppPermissions.Pages_SalesInvoiceVATDetails_Delete, L("DeleteSalesInvoiceVATDetail"));

            var salesInvoiceDiscounts = pages.CreateChildPermission(AppPermissions.Pages_SalesInvoiceDiscounts, L("SalesInvoiceDiscounts"));
            salesInvoiceDiscounts.CreateChildPermission(AppPermissions.Pages_SalesInvoiceDiscounts_Create, L("CreateNewSalesInvoiceDiscount"));
            salesInvoiceDiscounts.CreateChildPermission(AppPermissions.Pages_SalesInvoiceDiscounts_Edit, L("EditSalesInvoiceDiscount"));
            salesInvoiceDiscounts.CreateChildPermission(AppPermissions.Pages_SalesInvoiceDiscounts_Delete, L("DeleteSalesInvoiceDiscount"));

            var salesInvoiceContactPersons = pages.CreateChildPermission(AppPermissions.Pages_SalesInvoiceContactPersons, L("SalesInvoiceContactPersons"));
            salesInvoiceContactPersons.CreateChildPermission(AppPermissions.Pages_SalesInvoiceContactPersons_Create, L("CreateNewSalesInvoiceContactPerson"));
            salesInvoiceContactPersons.CreateChildPermission(AppPermissions.Pages_SalesInvoiceContactPersons_Edit, L("EditSalesInvoiceContactPerson"));
            salesInvoiceContactPersons.CreateChildPermission(AppPermissions.Pages_SalesInvoiceContactPersons_Delete, L("DeleteSalesInvoiceContactPerson"));

            var salesInvoiceAddresses = pages.CreateChildPermission(AppPermissions.Pages_SalesInvoiceAddresses, L("SalesInvoiceAddresses"));
            salesInvoiceAddresses.CreateChildPermission(AppPermissions.Pages_SalesInvoiceAddresses_Create, L("CreateNewSalesInvoiceAddress"));
            salesInvoiceAddresses.CreateChildPermission(AppPermissions.Pages_SalesInvoiceAddresses_Edit, L("EditSalesInvoiceAddress"));
            salesInvoiceAddresses.CreateChildPermission(AppPermissions.Pages_SalesInvoiceAddresses_Delete, L("DeleteSalesInvoiceAddress"));

            var salesInvoiceParties = pages.CreateChildPermission(AppPermissions.Pages_SalesInvoiceParties, L("SalesInvoiceParties"));
            salesInvoiceParties.CreateChildPermission(AppPermissions.Pages_SalesInvoiceParties_Create, L("CreateNewSalesInvoiceParty"));
            salesInvoiceParties.CreateChildPermission(AppPermissions.Pages_SalesInvoiceParties_Edit, L("EditSalesInvoiceParty"));
            salesInvoiceParties.CreateChildPermission(AppPermissions.Pages_SalesInvoiceParties_Delete, L("DeleteSalesInvoiceParty"));

            var salesInvoiceSummaries = pages.CreateChildPermission(AppPermissions.Pages_SalesInvoiceSummaries, L("SalesInvoiceSummaries"));
            salesInvoiceSummaries.CreateChildPermission(AppPermissions.Pages_SalesInvoiceSummaries_Create, L("CreateNewSalesInvoiceSummary"));
            salesInvoiceSummaries.CreateChildPermission(AppPermissions.Pages_SalesInvoiceSummaries_Edit, L("EditSalesInvoiceSummary"));
            salesInvoiceSummaries.CreateChildPermission(AppPermissions.Pages_SalesInvoiceSummaries_Delete, L("DeleteSalesInvoiceSummary"));
            var transactionCategory = pages.CreateChildPermission(AppPermissions.Pages_TransactionCategory, L("TransactionCategory"));
            transactionCategory.CreateChildPermission(AppPermissions.Pages_TransactionCategory_Create, L("CreateNewTransactionCategory"));
            transactionCategory.CreateChildPermission(AppPermissions.Pages_TransactionCategory_Edit, L("EditTransactionCategory"));
            transactionCategory.CreateChildPermission(AppPermissions.Pages_TransactionCategory_Delete, L("DeleteTransactionCategory"));

            var constitution = pages.CreateChildPermission(AppPermissions.Pages_Constitution, L("Constitution"));
            constitution.CreateChildPermission(AppPermissions.Pages_Constitution_Create, L("CreateNewConstitution"));
            constitution.CreateChildPermission(AppPermissions.Pages_Constitution_Edit, L("EditConstitution"));
            constitution.CreateChildPermission(AppPermissions.Pages_Constitution_Delete, L("DeleteConstitution"));

            var tenantType = pages.CreateChildPermission(AppPermissions.Pages_TenantType, L("TenantType"));
            tenantType.CreateChildPermission(AppPermissions.Pages_TenantType_Create, L("CreateNewTenantType"));
            tenantType.CreateChildPermission(AppPermissions.Pages_TenantType_Edit, L("EditTenantType"));
            tenantType.CreateChildPermission(AppPermissions.Pages_TenantType_Delete, L("DeleteTenantType"));

            var sector = pages.CreateChildPermission(AppPermissions.Pages_Sector, L("Sector"));
            sector.CreateChildPermission(AppPermissions.Pages_Sector_Create, L("CreateNewSector"));
            sector.CreateChildPermission(AppPermissions.Pages_Sector_Edit, L("EditSector"));
            sector.CreateChildPermission(AppPermissions.Pages_Sector_Delete, L("DeleteSector"));

            var gender = pages.CreateChildPermission(AppPermissions.Pages_Gender, L("Gender"));
            gender.CreateChildPermission(AppPermissions.Pages_Gender_Create, L("CreateNewGender"));
            gender.CreateChildPermission(AppPermissions.Pages_Gender_Edit, L("EditGender"));
            gender.CreateChildPermission(AppPermissions.Pages_Gender_Delete, L("DeleteGender"));
            var salesInvoiceItems = pages.CreateChildPermission(AppPermissions.Pages_SalesInvoiceItems, L("SalesInvoiceItems"));
            salesInvoiceItems.CreateChildPermission(AppPermissions.Pages_SalesInvoiceItems_Create, L("CreateNewSalesInvoiceItem"));
            salesInvoiceItems.CreateChildPermission(AppPermissions.Pages_SalesInvoiceItems_Edit, L("EditSalesInvoiceItem"));
            salesInvoiceItems.CreateChildPermission(AppPermissions.Pages_SalesInvoiceItems_Delete, L("DeleteSalesInvoiceItem"));

            var salesInvoices = pages.CreateChildPermission(AppPermissions.Pages_SalesInvoices, L("SalesInvoices"));
            salesInvoices.CreateChildPermission(AppPermissions.Pages_SalesInvoices_Create, L("CreateNewSalesInvoice"));
            salesInvoices.CreateChildPermission(AppPermissions.Pages_SalesInvoices_Edit, L("EditSalesInvoice"));
            salesInvoices.CreateChildPermission(AppPermissions.Pages_SalesInvoices_Delete, L("DeleteSalesInvoice"));

            var title = pages.CreateChildPermission(AppPermissions.Pages_Title, L("Title"));
            title.CreateChildPermission(AppPermissions.Pages_Title_Create, L("CreateNewTitle"));
            title.CreateChildPermission(AppPermissions.Pages_Title_Edit, L("EditTitle"));
            title.CreateChildPermission(AppPermissions.Pages_Title_Delete, L("DeleteTitle"));

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var module = administration.CreateChildPermission(AppPermissions.Pages_Administration_Module, L("Module"), multiTenancySides: MultiTenancySides.Host);
            module.CreateChildPermission(AppPermissions.Pages_Administration_Module_Create, L("CreateNewModule"), multiTenancySides: MultiTenancySides.Host);
            module.CreateChildPermission(AppPermissions.Pages_Administration_Module_Edit, L("EditModule"), multiTenancySides: MultiTenancySides.Host);
            module.CreateChildPermission(AppPermissions.Pages_Administration_Module_Delete, L("DeleteModule"), multiTenancySides: MultiTenancySides.Host);

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangeProfilePicture, L("UpdateUsersProfilePicture"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("ChangeDefaultLanguage"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            var massNotification = administration.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification, L("MassNotifications"));
            massNotification.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification_Create, L("MassNotificationCreate"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);

            var maintenance = administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            maintenance.CreateChildPermission(AppPermissions.Pages_Administration_NewVersion_Create, L("SendNewVersionNotification"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, vitaConsts.LocalizationSourceName);
        }
    }
}
﻿using vita.CustomReportSP.Dtos;
using vita.CustomReportSP;
using vita.TenantDetails.Dtos;
using vita.TenantDetails;
using vita.Vendor.Dtos;
using vita.Vendor;
using vita.Customer.Dtos;
using vita.Customer;
using vita.Purchase.Dtos;
using vita.ImportBatch.Dtos;
using vita.ImportBatch;
using vita.Purchase.Dtos;
using vita.Purchase;
using vita.Credit.Dtos;
using vita.Credit;
using vita.Debit.Dtos;
using vita.Debit;
using vita.Sales.Dtos;
using vita.Sales;
using vita.MasterData.Dtos;
using vita.MasterData;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using IdentityServer4.Extensions;
using vita.Auditing.Dto;
using vita.Authorization.Accounts.Dto;
using vita.Authorization.Delegation;
using vita.Authorization.Permissions.Dto;
using vita.Authorization.Roles;
using vita.Authorization.Roles.Dto;
using vita.Authorization.Users;
using vita.Authorization.Users.Delegation.Dto;
using vita.Authorization.Users.Dto;
using vita.Authorization.Users.Importing.Dto;
using vita.Authorization.Users.Profile.Dto;
using vita.Chat;
using vita.Chat.Dto;
using vita.DynamicEntityProperties.Dto;
using vita.Editions;
using vita.Editions.Dto;
using vita.Friendships;
using vita.Friendships.Cache;
using vita.Friendships.Dto;
using vita.Localization.Dto;
using vita.MultiTenancy;
using vita.MultiTenancy.Dto;
using vita.MultiTenancy.HostDashboard.Dto;
using vita.MultiTenancy.Payments;
using vita.MultiTenancy.Payments.Dto;
using vita.Notifications.Dto;
using vita.Organizations.Dto;
using vita.Sessions.Dto;
using vita.WebHooks.Dto;

namespace vita
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditModuleDto, Module>().ReverseMap();
            configuration.CreateMap<ModuleDto, Module>().ReverseMap();
            configuration.CreateMap<CreateOrEditDesignationDto, Designation>().ReverseMap();
            configuration.CreateMap<DesignationDto, Designation>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessOperationalModelDto, BusinessOperationalModel>().ReverseMap();
            configuration.CreateMap<BusinessOperationalModelDto, BusinessOperationalModel>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessTurnoverSlabDto, BusinessTurnoverSlab>().ReverseMap();
            configuration.CreateMap<BusinessTurnoverSlabDto, BusinessTurnoverSlab>().ReverseMap();
            configuration.CreateMap<CreateOrEditApportionmentBaseDataDto, ApportionmentBaseData>().ReverseMap();
            configuration.CreateMap<ApportionmentBaseDataDto, ApportionmentBaseData>().ReverseMap();
            configuration.CreateMap<CreateOrEditCustomReportDto, CustomReport>().ReverseMap();
            configuration.CreateMap<CustomReportDto, CustomReport>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantSectorsDto, TenantSectors>().ReverseMap();
            configuration.CreateMap<TenantSectorsDto, TenantSectors>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantPurchaseVatCateoryDto, TenantPurchaseVatCateory>().ReverseMap();
            configuration.CreateMap<TenantPurchaseVatCateoryDto, TenantPurchaseVatCateory>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantSupplyVATCategoryDto, TenantSupplyVATCategory>().ReverseMap();
            configuration.CreateMap<TenantSupplyVATCategoryDto, TenantSupplyVATCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantBusinessSuppliesDto, TenantBusinessSupplies>().ReverseMap();
            configuration.CreateMap<TenantBusinessSuppliesDto, TenantBusinessSupplies>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantBusinessPurchaseDto, TenantBusinessPurchase>().ReverseMap();
            configuration.CreateMap<TenantBusinessPurchaseDto, TenantBusinessPurchase>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantDocumentsDto, TenantDocuments>().ReverseMap();
            configuration.CreateMap<TenantDocumentsDto, TenantDocuments>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantAddressDto, TenantAddress>().ReverseMap();
            configuration.CreateMap<TenantAddressDto, TenantAddress>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantShareHoldersDto, TenantShareHolders>().ReverseMap();
            configuration.CreateMap<TenantShareHoldersDto, TenantShareHolders>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantBasicDetailsDto, TenantBasicDetails>().ReverseMap();
            configuration.CreateMap<TenantBasicDetailsDto, TenantBasicDetails>().ReverseMap();
            configuration.CreateMap<CreateOrEditActivecurrencyDto, Activecurrency>().ReverseMap();
            configuration.CreateMap<ActivecurrencyDto, Activecurrency>().ReverseMap();
            configuration.CreateMap<CreateOrEditBatchDataDto, BatchData>().ReverseMap();
            configuration.CreateMap<BatchDataDto, BatchData>().ReverseMap();
            configuration.CreateMap<CreateOrEditVendorSectorDetailDto, VendorSectorDetail>().ReverseMap();
            configuration.CreateMap<VendorSectorDetailDto, VendorSectorDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditVendorTaxDetailsDto, VendorTaxDetails>().ReverseMap();
            configuration.CreateMap<VendorTaxDetailsDto, VendorTaxDetails>().ReverseMap();
            configuration.CreateMap<CreateOrEditVendorForeignEntityDto, VendorForeignEntity>().ReverseMap();
            configuration.CreateMap<VendorForeignEntityDto, VendorForeignEntity>().ReverseMap();
            configuration.CreateMap<CreateOrEditVendorOwnershipDetailsDto, VendorOwnershipDetails>().ReverseMap();
            configuration.CreateMap<VendorOwnershipDetailsDto, VendorOwnershipDetails>().ReverseMap();
            configuration.CreateMap<CreateOrEditVendorDocumentsDto, VendorDocuments>().ReverseMap();
            configuration.CreateMap<VendorDocumentsDto, VendorDocuments>().ReverseMap();
            configuration.CreateMap<CreateOrEditVendorContactPersonDto, VendorContactPerson>().ReverseMap();
            configuration.CreateMap<VendorContactPersonDto, VendorContactPerson>().ReverseMap();
            configuration.CreateMap<CreateOrEditVendorAddressDto, VendorAddress>().ReverseMap();
            configuration.CreateMap<VendorAddressDto, VendorAddress>().ReverseMap();
            configuration.CreateMap<CreateOrEditVendorsDto, Vendors>().ReverseMap();
            configuration.CreateMap<VendorsDto, Vendors>().ReverseMap();
            configuration.CreateMap<CreateOrEditCustomerSectorDetailDto, CustomerSectorDetail>().ReverseMap();
            configuration.CreateMap<CustomerSectorDetailDto, CustomerSectorDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditCustomerTaxDetailsDto, CustomerTaxDetails>().ReverseMap();
            configuration.CreateMap<CustomerTaxDetailsDto, CustomerTaxDetails>().ReverseMap();
            configuration.CreateMap<CreateOrEditCustomerForeignEntityDto, CustomerForeignEntity>().ReverseMap();
            configuration.CreateMap<CustomerForeignEntityDto, CustomerForeignEntity>().ReverseMap();
            configuration.CreateMap<CreateOrEditCustomerOwnershipDetailsDto, CustomerOwnershipDetails>().ReverseMap();
            configuration.CreateMap<CustomerOwnershipDetailsDto, CustomerOwnershipDetails>().ReverseMap();
            configuration.CreateMap<CreateOrEditCustomerDocumentsDto, CustomerDocuments>().ReverseMap();
            configuration.CreateMap<CustomerDocumentsDto, CustomerDocuments>().ReverseMap();
            configuration.CreateMap<CreateOrEditCustomerContactPersonDto, CustomerContactPerson>().ReverseMap();
            configuration.CreateMap<CustomerContactPersonDto, CustomerContactPerson>().ReverseMap();
            configuration.CreateMap<CreateOrEditCustomerAddressDto, CustomerAddress>().ReverseMap();
            configuration.CreateMap<CustomerAddressDto, CustomerAddress>().ReverseMap();
            configuration.CreateMap<CreateOrEditCustomersDto, Customers>().ReverseMap();
            configuration.CreateMap<CustomersDto, Customers>().ReverseMap();
            configuration.CreateMap<CreateOrEditImportBatchDataDto, ImportBatchData>().ReverseMap();
            configuration.CreateMap<ImportBatchDataDto, ImportBatchData>().ReverseMap();
            configuration.CreateMap<CreateOrEditPurchaseEntryPartyDto, PurchaseEntryParty>().ReverseMap();
            configuration.CreateMap<PurchaseEntryPartyDto, PurchaseEntryParty>().ReverseMap();
            configuration.CreateMap<CreateOrEditPurchaseEntrySummaryDto, PurchaseEntrySummary>().ReverseMap();
            configuration.CreateMap<PurchaseEntrySummaryDto, PurchaseEntrySummary>().ReverseMap();
            configuration.CreateMap<CreateOrEditPurchaseEntryPaymentDetailDto, PurchaseEntryPaymentDetail>().ReverseMap();
            configuration.CreateMap<PurchaseEntryPaymentDetailDto, PurchaseEntryPaymentDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditPurchaseEntryItemDto, PurchaseEntryItem>().ReverseMap();
            configuration.CreateMap<PurchaseEntryItemDto, PurchaseEntryItem>().ReverseMap();
            configuration.CreateMap<CreateOrEditPurchaseEntryVATDetailDto, PurchaseEntryVATDetail>().ReverseMap();
            configuration.CreateMap<PurchaseEntryVATDetailDto, PurchaseEntryVATDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditPurchaseEntryDiscountDto, PurchaseEntryDiscount>().ReverseMap();
            configuration.CreateMap<PurchaseEntryDiscountDto, PurchaseEntryDiscount>().ReverseMap();
            configuration.CreateMap<CreateOrEditPurchaseEntryContactPersonDto, PurchaseEntryContactPerson>().ReverseMap();
            configuration.CreateMap<PurchaseEntryContactPersonDto, PurchaseEntryContactPerson>().ReverseMap();
            configuration.CreateMap<CreateOrEditPurchaseEntryAddressDto, PurchaseEntryAddress>().ReverseMap();
            configuration.CreateMap<PurchaseEntryAddressDto, PurchaseEntryAddress>().ReverseMap();
            configuration.CreateMap<CreateOrEditPurchaseEntryDto, PurchaseEntry>().ReverseMap();
            configuration.CreateMap<PurchaseEntryDto, PurchaseEntry>().ReverseMap();
            configuration.CreateMap<CreateOrEditIRNMasterDto, IRNMaster>().ReverseMap();
            configuration.CreateMap<IRNMasterDto, IRNMaster>().ReverseMap();
            configuration.CreateMap<CreateOrEditCreditNotePartyDto, CreditNoteParty>().ReverseMap();
            configuration.CreateMap<CreditNotePartyDto, CreditNoteParty>().ReverseMap();
            configuration.CreateMap<CreateOrEditCreditNoteSummaryDto, CreditNoteSummary>().ReverseMap();
            configuration.CreateMap<CreditNoteSummaryDto, CreditNoteSummary>().ReverseMap();
            configuration.CreateMap<CreateOrEditCreditNotePaymentDetailDto, CreditNotePaymentDetail>().ReverseMap();
            configuration.CreateMap<CreditNotePaymentDetailDto, CreditNotePaymentDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditCreditNoteItemDto, CreditNoteItem>().ReverseMap();
            configuration.CreateMap<CreditNoteItemDto, CreditNoteItem>().ReverseMap();
            configuration.CreateMap<CreateOrEditCreditNoteVATDetailDto, CreditNoteVATDetail>().ReverseMap();
            configuration.CreateMap<CreditNoteVATDetailDto, CreditNoteVATDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditCreditNoteDiscountDto, CreditNoteDiscount>().ReverseMap();
            configuration.CreateMap<CreditNoteDiscountDto, CreditNoteDiscount>().ReverseMap();
            configuration.CreateMap<CreateOrEditCreditNoteContactPersonDto, CreditNoteContactPerson>().ReverseMap();
            configuration.CreateMap<CreditNoteContactPersonDto, CreditNoteContactPerson>().ReverseMap();
            configuration.CreateMap<CreateOrEditCreditNoteAddressDto, CreditNoteAddress>().ReverseMap();
            configuration.CreateMap<CreditNoteAddressDto, CreditNoteAddress>().ReverseMap();
            configuration.CreateMap<CreateOrEditCreditNoteDto, CreditNote>().ReverseMap();
            configuration.CreateMap<CreditNoteDto, CreditNote>().ReverseMap();
            configuration.CreateMap<CreateOrEditDebitNotePartyDto, DebitNoteParty>().ReverseMap();
            configuration.CreateMap<DebitNotePartyDto, DebitNoteParty>().ReverseMap();
            configuration.CreateMap<CreateOrEditDebitNoteSummaryDto, DebitNoteSummary>().ReverseMap();
            configuration.CreateMap<DebitNoteSummaryDto, DebitNoteSummary>().ReverseMap();
            configuration.CreateMap<CreateOrEditDebitNotePaymentDetailDto, DebitNotePaymentDetail>().ReverseMap();
            configuration.CreateMap<DebitNotePaymentDetailDto, DebitNotePaymentDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditDebitNoteItemDto, DebitNoteItem>().ReverseMap();
            configuration.CreateMap<DebitNoteItemDto, DebitNoteItem>().ReverseMap();
            configuration.CreateMap<CreateOrEditDebitNoteVATDetailDto, DebitNoteVATDetail>().ReverseMap();
            configuration.CreateMap<DebitNoteVATDetailDto, DebitNoteVATDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditDebitNoteDiscountDto, DebitNoteDiscount>().ReverseMap();
            configuration.CreateMap<DebitNoteDiscountDto, DebitNoteDiscount>().ReverseMap();
            configuration.CreateMap<CreateOrEditDebitNoteContactPersonDto, DebitNoteContactPerson>().ReverseMap();
            configuration.CreateMap<DebitNoteContactPersonDto, DebitNoteContactPerson>().ReverseMap();
            configuration.CreateMap<CreateOrEditDebitNoteAddressDto, DebitNoteAddress>().ReverseMap();
            configuration.CreateMap<DebitNoteAddressDto, DebitNoteAddress>().ReverseMap();
            configuration.CreateMap<CreateOrEditDebitNoteDto, DebitNote>().ReverseMap();
            configuration.CreateMap<DebitNoteDto, DebitNote>().ReverseMap();
            configuration.CreateMap<CreateOrEditInvoiceTypeDto, InvoiceType>().ReverseMap();
            configuration.CreateMap<InvoiceTypeDto, InvoiceType>().ReverseMap();
            configuration.CreateMap<CreateOrEditFinancialYearDto, FinancialYear>().ReverseMap();
            configuration.CreateMap<FinancialYearDto, FinancialYear>().ReverseMap();
            configuration.CreateMap<CreateOrEditErrorTypeDto, ErrorType>().ReverseMap();
            configuration.CreateMap<ErrorTypeDto, ErrorType>().ReverseMap();
            configuration.CreateMap<CreateOrEditHeadOfPaymentDto, HeadOfPayment>().ReverseMap();
            configuration.CreateMap<HeadOfPaymentDto, HeadOfPayment>().ReverseMap();
            configuration.CreateMap<CreateOrEditCurrencyDto, Currency>().ReverseMap();
            configuration.CreateMap<CurrencyDto, Currency>().ReverseMap();
            configuration.CreateMap<CreateOrEditTaxCategoryDto, TaxCategory>().ReverseMap();
            configuration.CreateMap<TaxCategoryDto, TaxCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditInvoiceCategoryDto, InvoiceCategory>().ReverseMap();
            configuration.CreateMap<InvoiceCategoryDto, InvoiceCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditErrorGroupDto, ErrorGroup>().ReverseMap();
            configuration.CreateMap<ErrorGroupDto, ErrorGroup>().ReverseMap();
            configuration.CreateMap<CreateOrEditAffiliationDto, Affiliation>().ReverseMap();
            configuration.CreateMap<AffiliationDto, Affiliation>().ReverseMap();
            configuration.CreateMap<CreateOrEditPlaceOfPerformanceDto, PlaceOfPerformance>().ReverseMap();
            configuration.CreateMap<PlaceOfPerformanceDto, PlaceOfPerformance>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrganisationTypeDto, OrganisationType>().ReverseMap();
            configuration.CreateMap<OrganisationTypeDto, OrganisationType>().ReverseMap();
            configuration.CreateMap<CreateOrEditPurchaseTypeDto, PurchaseType>().ReverseMap();
            configuration.CreateMap<PurchaseTypeDto, PurchaseType>().ReverseMap();
            configuration.CreateMap<CreateOrEditAllowanceReasonDto, AllowanceReason>().ReverseMap();
            configuration.CreateMap<AllowanceReasonDto, AllowanceReason>().ReverseMap();
            configuration.CreateMap<CreateOrEditUnitOfMeasurementDto, UnitOfMeasurement>().ReverseMap();
            configuration.CreateMap<UnitOfMeasurementDto, UnitOfMeasurement>().ReverseMap();
            configuration.CreateMap<CreateOrEditNatureofServicesDto, NatureofServices>().ReverseMap();
            configuration.CreateMap<NatureofServicesDto, NatureofServices>().ReverseMap();
            configuration.CreateMap<CreateOrEditExemptionReasonDto, ExemptionReason>().ReverseMap();
            configuration.CreateMap<ExemptionReasonDto, ExemptionReason>().ReverseMap();
            configuration.CreateMap<CreateOrEditReasonCNDNDto, ReasonCNDN>().ReverseMap();
            configuration.CreateMap<ReasonCNDNDto, ReasonCNDN>().ReverseMap();
            configuration.CreateMap<CreateOrEditDocumentMasterDto, DocumentMaster>().ReverseMap();
            configuration.CreateMap<DocumentMasterDto, DocumentMaster>().ReverseMap();
            configuration.CreateMap<CreateOrEditPaymentMeansDto, PaymentMeans>().ReverseMap();
            configuration.CreateMap<PaymentMeansDto, PaymentMeans>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessProcessDto, BusinessProcess>().ReverseMap();
            configuration.CreateMap<BusinessProcessDto, BusinessProcess>().ReverseMap();
            configuration.CreateMap<CreateOrEditTaxSubCategoryDto, TaxSubCategory>().ReverseMap();
            configuration.CreateMap<TaxSubCategoryDto, TaxSubCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditCountryDto, Country>().ReverseMap();
            configuration.CreateMap<CountryDto, Country>().ReverseMap();
            configuration.CreateMap<CreateOrEditSalesInvoicePaymentDetailDto, SalesInvoicePaymentDetail>().ReverseMap();
            configuration.CreateMap<SalesInvoicePaymentDetailDto, SalesInvoicePaymentDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditSalesInvoiceVATDetailDto, SalesInvoiceVATDetail>().ReverseMap();
            configuration.CreateMap<SalesInvoiceVATDetailDto, SalesInvoiceVATDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditSalesInvoiceDiscountDto, SalesInvoiceDiscount>().ReverseMap();
            configuration.CreateMap<SalesInvoiceDiscountDto, SalesInvoiceDiscount>().ReverseMap();
            configuration.CreateMap<CreateOrEditSalesInvoiceContactPersonDto, SalesInvoiceContactPerson>().ReverseMap();
            configuration.CreateMap<SalesInvoiceContactPersonDto, SalesInvoiceContactPerson>().ReverseMap();
            configuration.CreateMap<CreateOrEditSalesInvoiceAddressDto, SalesInvoiceAddress>().ReverseMap();
            configuration.CreateMap<SalesInvoiceAddressDto, SalesInvoiceAddress>().ReverseMap();
            configuration.CreateMap<CreateOrEditSalesInvoicePartyDto, SalesInvoiceParty>().ReverseMap();
            configuration.CreateMap<SalesInvoicePartyDto, SalesInvoiceParty>().ReverseMap();
            configuration.CreateMap<CreateOrEditSalesInvoiceSummaryDto, SalesInvoiceSummary>().ReverseMap();
            configuration.CreateMap<SalesInvoiceSummaryDto, SalesInvoiceSummary>().ReverseMap();
            configuration.CreateMap<CreateOrEditTransactionCategoryDto, TransactionCategory>().ReverseMap();
            configuration.CreateMap<TransactionCategoryDto, TransactionCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditConstitutionDto, Constitution>().ReverseMap();
            configuration.CreateMap<ConstitutionDto, Constitution>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantTypeDto, TenantType>().ReverseMap();
            configuration.CreateMap<TenantTypeDto, TenantType>().ReverseMap();
            configuration.CreateMap<CreateOrEditSectorDto, Sector>().ReverseMap();
            configuration.CreateMap<SectorDto, Sector>().ReverseMap();
            configuration.CreateMap<CreateOrEditGenderDto, Gender>().ReverseMap();
            configuration.CreateMap<GenderDto, Gender>().ReverseMap();
            configuration.CreateMap<CreateOrEditSalesInvoiceItemDto, SalesInvoiceItem>().ReverseMap();
            configuration.CreateMap<SalesInvoiceItemDto, SalesInvoiceItem>().ReverseMap();
            configuration.CreateMap<CreateOrEditSalesInvoiceDto, SalesInvoice>().ReverseMap();
            configuration.CreateMap<SalesInvoiceDto, SalesInvoice>().ReverseMap();
            configuration.CreateMap<CreateOrEditTitleDto, Title>().ReverseMap();
            configuration.CreateMap<TitleDto, Title>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();

            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicProperty, DynamicPropertyDto>().ReverseMap();
            configuration.CreateMap<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
            configuration.CreateMap<DynamicEntityProperty, DynamicEntityPropertyDto>()
                .ForMember(dto => dto.DynamicPropertyName,
                    options => options.MapFrom(entity => entity.DynamicProperty.DisplayName.IsNullOrEmpty() ? entity.DynamicProperty.PropertyName : entity.DynamicProperty.DisplayName));
            configuration.CreateMap<DynamicEntityPropertyDto, DynamicEntityProperty>();

            configuration.CreateMap<DynamicEntityPropertyValue, DynamicEntityPropertyValueDto>().ReverseMap();

            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}
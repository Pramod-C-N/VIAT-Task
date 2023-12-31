﻿import { AbpHttpInterceptor, RefreshTokenService, AbpHttpConfigurationService } from 'abp-ng2-module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import * as ApiServiceProxies from './service-proxies';
import { ZeroRefreshTokenService } from '@account/auth/zero-refresh-token.service';
import { ZeroTemplateHttpConfigurationService } from './zero-template-http-configuration.service';
import { AppCommonModule } from '@app/shared/common/app-common.module';

@NgModule({
    providers: [
        ApiServiceProxies.DraftsServiceProxy,
        ApiServiceProxies.GenerateXmlServiceProxy,
        ApiServiceProxies.TenantConfigurationServiceProxy,
        ApiServiceProxies.OverheadApportionmentServiceProxy,
        ApiServiceProxies.PurchaseDebitNoteServiceProxy,
        ApiServiceProxies.PurchaseCreditNoteServiceProxy,
        ApiServiceProxies.CreditNotePurchaseServiceProxy,
        ApiServiceProxies.PurchaseCreditNoteDiscountServiceProxy,
        ApiServiceProxies.DesignationServiceProxy,
        ApiServiceProxies.VendorsesServiceProxy,
        ApiServiceProxies.BusinessOperationalModelServiceProxy,
        ApiServiceProxies.BusinessTurnoverSlabServiceProxy,
        ApiServiceProxies.CustomReportServiceProxy,
        ApiServiceProxies.ActivecurrencyServiceProxy,
        ApiServiceProxies.InvoiceTypeServiceProxy,
        ApiServiceProxies.ModuleServiceProxy,        
        ApiServiceProxies.DesignationServiceProxy,        
        ApiServiceProxies.VendorsesServiceProxy,        
        ApiServiceProxies.BusinessOperationalModelServiceProxy,        
        ApiServiceProxies.BusinessTurnoverSlabServiceProxy,        
        ApiServiceProxies.CustomReportServiceProxy,        
        ApiServiceProxies.ActivecurrencyServiceProxy,        
        ApiServiceProxies.InvoiceTypeServiceProxy,  
        ApiServiceProxies.SalesInvoicesServiceProxy,
        ApiServiceProxies.CreditNoteServiceProxy,
        ApiServiceProxies.DebitNotesServiceProxy,
        ApiServiceProxies.PurchaseEntriesServiceProxy,
        ApiServiceProxies.FinancialYearServiceProxy,
        ApiServiceProxies.ErrorTypeServiceProxy,
        ApiServiceProxies.HeadOfPaymentServiceProxy,
        ApiServiceProxies.CurrencyServiceProxy,
        ApiServiceProxies.TaxCategoryServiceProxy,
        ApiServiceProxies.InvoiceCategoryServiceProxy,
        ApiServiceProxies.ErrorGroupServiceProxy,
        ApiServiceProxies.AffiliationServiceProxy,
        ApiServiceProxies.PlaceOfPerformanceServiceProxy,
        ApiServiceProxies.OrganisationTypeServiceProxy,
        ApiServiceProxies.PurchaseTypeServiceProxy,
        ApiServiceProxies.AllowanceReasonServiceProxy,
        ApiServiceProxies.UnitOfMeasurementServiceProxy,
        ApiServiceProxies.NatureofServicesServiceProxy,
        ApiServiceProxies.ExemptionReasonServiceProxy,
        ApiServiceProxies.ReasonCNDNServiceProxy,
        ApiServiceProxies.DocumentMasterServiceProxy,
        ApiServiceProxies.PaymentMeansServiceProxy,
        ApiServiceProxies.BusinessProcessServiceProxy,
        ApiServiceProxies.TaxSubCategoryServiceProxy,
        ApiServiceProxies.CountryServiceProxy,
        ApiServiceProxies.TransactionCategoryServiceProxy,
        ApiServiceProxies.ConstitutionServiceProxy,
        ApiServiceProxies.TenantTypeServiceProxy,
        ApiServiceProxies.SectorServiceProxy,
        ApiServiceProxies.GenderServiceProxy,
        ApiServiceProxies.TitleServiceProxy,
        ApiServiceProxies.AuditLogServiceProxy,
        ApiServiceProxies.CachingServiceProxy,
        ApiServiceProxies.ChatServiceProxy,
        ApiServiceProxies.CommonLookupServiceProxy,
        ApiServiceProxies.EditionServiceProxy,
        ApiServiceProxies.FriendshipServiceProxy,
        ApiServiceProxies.HostSettingsServiceProxy,
        ApiServiceProxies.InstallServiceProxy,
        ApiServiceProxies.LanguageServiceProxy,
        ApiServiceProxies.NotificationServiceProxy,
        ApiServiceProxies.OrganizationUnitServiceProxy,
        ApiServiceProxies.PermissionServiceProxy,
        ApiServiceProxies.ProfileServiceProxy,
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.TenantServiceProxy,
        ApiServiceProxies.TenantDashboardServiceProxy,
        ApiServiceProxies.TenantSettingsServiceProxy,
        ApiServiceProxies.TimingServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.UserLinkServiceProxy,
        ApiServiceProxies.UserLoginServiceProxy,
        ApiServiceProxies.WebLogServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.TenantRegistrationServiceProxy,
        ApiServiceProxies.HostDashboardServiceProxy,
        ApiServiceProxies.PaymentServiceProxy,
        ApiServiceProxies.DemoUiComponentsServiceProxy,
        ApiServiceProxies.InvoiceServiceProxy,
        ApiServiceProxies.SubscriptionServiceProxy,
        ApiServiceProxies.InstallServiceProxy,
        ApiServiceProxies.UiCustomizationSettingsServiceProxy,
        ApiServiceProxies.PayPalPaymentServiceProxy,
        ApiServiceProxies.StripePaymentServiceProxy,
        ApiServiceProxies.DashboardCustomizationServiceProxy,
        ApiServiceProxies.WebhookEventServiceProxy,
        ApiServiceProxies.WebhookSubscriptionServiceProxy,
        ApiServiceProxies.WebhookSendAttemptServiceProxy,
        ApiServiceProxies.UserDelegationServiceProxy,
        ApiServiceProxies.DynamicPropertyServiceProxy,
        ApiServiceProxies.DynamicEntityPropertyDefinitionServiceProxy,
        ApiServiceProxies.DynamicEntityPropertyServiceProxy,
        ApiServiceProxies.DynamicPropertyValueServiceProxy,
        ApiServiceProxies.DynamicEntityPropertyValueServiceProxy,
        ApiServiceProxies.TwitterServiceProxy,
        ApiServiceProxies.ReportServiceProxy,
        ApiServiceProxies.ImportBatchDatasServiceProxy,
        ApiServiceProxies.TwitterServiceProxy,
        ApiServiceProxies.CustomersesServiceProxy,
        ApiServiceProxies.TenantServiceProxy,
        ApiServiceProxies.TenantBasicDetailsServiceProxy,
        { provide: RefreshTokenService, useClass: ZeroRefreshTokenService },
        { provide: AbpHttpConfigurationService, useClass: ZeroTemplateHttpConfigurationService },
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true },
    ],
})
export class ServiceProxyModule {}

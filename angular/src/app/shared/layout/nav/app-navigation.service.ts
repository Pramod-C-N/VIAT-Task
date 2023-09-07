import { PermissionCheckerService } from 'abp-ng2-module';
import { AppSessionService } from '@shared/common/session/app-session.service';

import { Injectable } from '@angular/core';
import { AppMenu } from './app-menu';
import { AppMenuItem } from './app-menu-item';
import { AppConsts } from '@shared/AppConsts';
import { GlobalConstsCustomService } from '@shared/customService/global-consts-service';

@Injectable()
export class AppNavigationService {
    isVita: boolean = false;
    isGoods: boolean = false;
    isServices: boolean = false;

    constructor(
        private _permissionCheckerService: PermissionCheckerService,
        private _appSessionService: AppSessionService,
        private _globalConstsService: GlobalConstsCustomService
    ) {
        this._globalConstsService.data$.subscribe((e) => {
            this.isVita = e.isVita;
        });

        this._globalConstsService.tenantType$.subscribe((e: string) => {
            this.isServices = e.includes('S');
            this.isGoods = e.includes('G');
        });
    }

    getMenu(): AppMenu {
        if (this.isVita) {
            let appmenu = new AppMenu('MainMenu', 'MainMenu', [
                new AppMenuItem('Dashboard', 'Pages.Tenant.Dashboard', 'flaticon-line-graph', '/app/main/dashboard'),
                new AppMenuItem('Tenants', 'Pages.Tenants', 'flaticon-list-3', '/app/admin/tenants'),
                new AppMenuItem(
                    'Purchase',
                    'Pages.Tenant.Dashboard',
                    'flaticon-interface-8',
                    '',
                    [],
                    [
                        new AppMenuItem(
                            'Purchase Transaction',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/sales/purchaseTransaction'
                        ),
                        // new AppMenuItem('Purchase Entry', 'Pages.Title', 'flaticon-more', '/app/main/sales/purchaseEntry'),
                        new AppMenuItem(
                            'Create Purchase Entry',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/sales/createPurchaseEntry'
                        ),
                        // new AppMenuItem('Debit Note', 'Pages.Title', 'flaticon-more', '/app/main/sales/debitNote'),
                        new AppMenuItem(
                            'Create Credit Note',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/sales/purchaseCreateCreditNote'
                        ),

                        new AppMenuItem(
                            'Create Debit Note',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/sales/createPurchaseDebitNote'
                        ),
                    ]
                ),
                new AppMenuItem(
                    'Phase 2',
                    'Pages.Title',
                    'flaticon-more',
                    '/app/main/sales/phase2'
                ),
                new AppMenuItem(
                    'Reports',
                    'Pages.Tenant.Dashboard',
                    'flaticon-interface-8',
                    '',
                    [],
                    [
                        new AppMenuItem(
                            'Sales Report',
                            'Pages.HeadOfPayment',
                            'fi fi-rs-file-invoice-dollar',
                            '/app/main/Report/SalesReport'
                        ),
                        //new AppMenuItem('Sales Daywise Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/SalesDaywiseReport'),
                        new AppMenuItem(
                            'Credit Note Sales',
                            'Pages.HeadOfPayment',
                            'flaticon-more',
                            '/app/main/Report/CreditSalesReport'
                        ),
                        // new AppMenuItem('Credit Note (Sales) Daywise Report', 'Pages.Title', 'flaticon-more', '/app/main/Report/CreditNoteSalesDayWiseReport'),
                        new AppMenuItem(
                            'Debit Note Sales',
                            'Pages.HeadOfPayment',
                            'flaticon-more',
                            '/app/main/Report/DebitSalesReport'
                        ),
                        //new AppMenuItem('Debit Note (Sales) Daywise Report', 'Pages.Title', 'flaticon-more', '/app/main/Report/DebitNoteSalesDayWiseReport'),
                        new AppMenuItem(
                            'Purchase Entry Report',
                            'Pages.HeadOfPayment',
                            'flaticon-more',
                            '/app/main/Report/PurchaseEntryDetailedReport'
                        ),
                        // new AppMenuItem('Purchase Entry Daywise Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/PurchaseEntryDaywiseReport'),
                        new AppMenuItem(
                            'Credit Note Purchase',
                            'Pages.HeadOfPayment',
                            'flaticon-more',
                            '/app/main/Report/CreditNotePurchaseDetailedReport'
                        ),
                        // new AppMenuItem('Credit Note (Purchase) Daywise Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/CreditNotePurchaseDayWiseReport'),
                        new AppMenuItem(
                            'Debit Note Purchase',
                            'Pages.HeadOfPayment',
                            'flaticon-more',
                            '/app/main/Report/DebitNotePurchaseDetailedReport'
                        ),
                        // new AppMenuItem('Debit Note (Purchase) Daywise Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/DebitNotePurchaseDayWiseReport'),
                        new AppMenuItem(
                            'Override Report',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/Report/OverrideReport'
                        ),
                        
                        new AppMenuItem(
                            'VAT Return Report',
                            'Pages.HeadOfPayment',
                            'flaticon-more',
                            '/app/main/Report/VATReport'
                        ),
                        // new AppMenuItem(
                        //     'Sales Report',
                        //     'Pages.HeadOfPayment',
                        //     'fi fi-rs-file-invoice-dollar',
                        //     '/app/main/Report/SalesReport'
                        // ),
                        new AppMenuItem(
                            'Reconciliation Report',
                            '',
                            'flaticon-more',
                            '',
                            [],
                            [
                                new AppMenuItem(
                                    'Sales Invoice',
                                    'Pages.HeadOfPayment',
                                    'flaticon-more',
                                    '/app/main/Report/ReconciliationReport'
                                ),
                                new AppMenuItem(
                                    'Sales Credit Note',
                                    'Pages.HeadOfPayment',
                                    'flaticon-more',
                                    '/app/main/Report/SalesCreditNoteReconciliationReport'
                                ),
                                new AppMenuItem(
                                    'Sales Debit Note',
                                    'Pages.HeadOfPayment',
                                    'flaticon-more',
                                    '/app/main/Report/SalesDebitNoteReconciliationReport'
                                ),
                                new AppMenuItem(
                                    'Purchase',
                                    'Pages.HeadOfPayment',
                                    'flaticon-more',
                                    '/app/main/Report/PurchaseReconciliationReport'
                                ),
                                new AppMenuItem(
                                    'Purchase Credit Note',
                                    'Pages.HeadOfPayment',
                                    'flaticon-more',
                                    '/app/main/Report/PurchaseCreditNoteReconciliationReport'
                                ),
                                new AppMenuItem(
                                    'Purchase Debit Note',
                                    'Pages.HeadOfPayment',
                                    'flaticon-more',
                                    '/app/main/Report/PurchaseDebitNoteReconciliationReport'
                                ),
                                new AppMenuItem(
                                    'Overheads',
                                    'Pages.HeadOfPayment',
                                    'flaticon-more',
                                    '/app/main/Report/OverheadsReconciliationReport'
                                ),
                                new AppMenuItem(
                                    'Overhead Gap Analysis',
                                    'Pages.Title',
                                    'flaticon-more',
                                    '/app/main/Report/ReconciliationReports/OverHeadGapAnalysisReport'
                                ),
                            ]
                        ),

                        //new AppMenuItem('Reconciliation Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/ReconciliationReport'),
                        new AppMenuItem(
                            'WHT Detailed Report',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/Payment/DetailedReport'
                        ),
                        new AppMenuItem(
                            'WHT Return Report',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/Payment/ReturnReport'
                        ),
                        new AppMenuItem(
                            'Master Report',
                            '',
                            'flaticon-more',
                            '',
                            [],
                            [
                                new AppMenuItem(
                                    'Tenant Report',
                                    'Pages.HeadOfPayment',
                                    'flaticon-more',
                                    '/app/main/Report/TenantReport'
                                ),
                                new AppMenuItem(
                                    'Customer Report',
                                    'Pages.HeadOfPayment',
                                    'flaticon-more',
                                    '/app/main/Report/CustomerReport'
                                ),
                                new AppMenuItem(
                                    'Vendor Report',
                                    'Pages.HeadOfPayment',
                                    'flaticon-more',
                                    '/app/main/Report/VendorReport'
                                ),
                            ]
                        ),
                        new AppMenuItem(
                            'Custom Report',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/Reports/CustomReport'
                        ),
                    ]
                ),

                new AppMenuItem(
                    'File Upload (Transaction)',
                    'Pages.Tenant.Dashboard',
                    'flaticon-interface-8',
                    '',
                    [],
                    [
                        new AppMenuItem(
                            'Batch Summary',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/sales/BatchSummary'
                        ),
                        new AppMenuItem(
                            'Sales',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/FileUpload/salesFileUpload'
                        ),
                        new AppMenuItem(
                            'E-Invoicing',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/FileUpload/integratedsalesFileUpload'
                        ),
                        new AppMenuItem(
                            'Credit Note (Sales)',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/FileUpload/CreditNoteFileupload'
                        ),
                        new AppMenuItem(
                            'Debit Note (Sales)',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/FileUpload/DebitNoteSalesFileUpload'
                        ),
                        new AppMenuItem(
                            'Purchase Entry',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/FileUpload/PurchaseEntryFileupload'
                        ),
                        new AppMenuItem(
                            'Credit Note (Purchase)',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/FileUpload/CreditNotePurchaseFileUpload'
                        ),
                        new AppMenuItem(
                            'Debit Note (Purchase)',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/FileUpload/DebitNotePurchaseFileUpload'
                        ),
                        new AppMenuItem(
                            'Payment',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/FileUpload/PaymentFileupload'
                        ),
                        //new AppMenuItem('Customer File Upload', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/customerFileUpload'),
                        //new AppMenuItem('Vendor File Upload', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/vendorFileUpload'),
                        new AppMenuItem(
                            'Trial Balance',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/FileUpload/TrialBalanceFileUpload'
                        ),
                    ]
                ),
                new AppMenuItem(
                    'File Upload - Master',
                    'Pages.Tenant.Dashboard',
                    'flaticon-interface-8',
                    '',
                    [],
                    [
                        new AppMenuItem(
                            'File Mapper',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/FileUpload/file-mapper'
                        ),
                        new AppMenuItem(
                            'Master Batch Summary',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/sales/MasterBatchSummary'
                        ),
                        new AppMenuItem(
                            'Tenant File Upload',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/FileUpload/tenantFileUpload'
                        ),
                        new AppMenuItem(
                            'Customer File Upload',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/FileUpload/customerFileUpload'
                        ),
                        new AppMenuItem(
                            'Vendor File Upload',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/FileUpload/vendorFileUpload'
                        ),
                        //new AppMenuItem('Customer File Upload', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/customerFileUpload'),
                        //new AppMenuItem('Vendor File Upload', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/vendorFileUpload'),                    new AppMenuItem('Vendor', 'Pages.Title', 'flaticon-more', '/app/main/Report/CorporateTaxReport/VendorMaster'),
                        new AppMenuItem(
                            'Ledger Master',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/FileUpload/LedgerFileUpload'
                        ),
                    ]
                ),

                // new AppMenuItem(
                //     'Masters',
                //     'Pages.Administration.Host.Settings',
                //     'flaticon-interface-8',
                //     '',
                //     [],
                //     [
                //         new AppMenuItem('Title', 'Pages.Title', 'flaticon-more', '/app/main/masterData/title'),
                //         new AppMenuItem('Gender', 'Pages.Gender', 'flaticon-more', '/app/main/masterData/gender'),
                //         // new AppMenuItem('Sector', 'Pages.Sector', 'flaticon-more', '/app/main/masterData/sector'),
                //         new AppMenuItem(
                //             'Tenant Type',
                //             'Pages.TenantType',
                //             'flaticon-more',
                //             '/app/main/masterData/tenantType'
                //         ),
                //         new AppMenuItem(
                //             'Constitution',
                //             'Pages.Constitution',
                //             'flaticon-more',
                //             '/app/main/masterData/constitution'
                //         ),
                //         //  new AppMenuItem('Transaction Category', 'Pages.TransactionCategory', 'flaticon-more', '/app/main/masterData/transactionCategory'),
                //         new AppMenuItem('Country', 'Pages.Country', 'flaticon-more', '/app/main/masterData/country'),
                //         //new AppMenuItem('Tax SubCategory', 'Pages.TaxSubCategory', 'flaticon-more', '/app/main/masterData/taxSubCategory'),
                //         //new AppMenuItem('BusinessProcess', 'Pages.BusinessProcess', 'flaticon-more', '/app/main/masterData/businessProcess'),
                //         //new AppMenuItem('PaymentMeans', 'Pages.PaymentMeans', 'flaticon-more', '/app/main/masterData/paymentMeans'),
                //         //new AppMenuItem('DocumentMaster', 'Pages.DocumentMaster', 'flaticon-more', '/app/main/masterData/documentMaster'),
                //         //new AppMenuItem('Reason CNDN', 'Pages.ReasonCNDN', 'flaticon-more', '/app/main/masterData/reasonCNDN'),
                //         //new AppMenuItem('Exemption Reason', 'Pages.ExemptionReason', 'flaticon-more', '/app/main/masterData/exemptionReason'),
                //         //new AppMenuItem('Nature of Service', 'Pages.NatureofServices', 'flaticon-more', '/app/main/masterData/natureofServices'),
                //         //new AppMenuItem('UnitOfMeasurement', 'Pages.UnitOfMeasurement', 'flaticon-more', '/app/main/masterData/unitOfMeasurement'),
                //         //new AppMenuItem('AllowanceReason', 'Pages.AllowanceReason', 'flaticon-more', '/app/main/masterData/allowanceReason'),
                //         //new AppMenuItem('PurchaseType', 'Pages.PurchaseType', 'flaticon-more', '/app/main/masterData/purchaseType'),
                //         //new AppMenuItem('OrganisationType', 'Pages.OrganisationType', 'flaticon-more', '/app/main/masterData/organisationType'),
                //         //new AppMenuItem('PlaceOfPerformance', 'Pages.PlaceOfPerformance', 'flaticon-more', '/app/main/masterData/placeOfPerformance'),
                //         //new AppMenuItem('Affiliation', 'Pages.Affiliation', 'flaticon-more', '/app/main/masterData/affiliation'),
                //         //new AppMenuItem('ErrorGroup', 'Pages.ErrorGroup', 'flaticon-more', '/app/main/masterData/errorGroup'),
                //         //new AppMenuItem('InvoiceType', 'Pages.InvoiceType', 'flaticon-more', '/app/main/masterData/invoiceType'),
                //         //new AppMenuItem('InvoiceCategory', 'Pages.InvoiceCategory', 'flaticon-more', '/app/main/masterData/invoiceCategory'),
                //         //new AppMenuItem('TaxCategory', 'Pages.TaxCategory', 'flaticon-more', '/app/main/masterData/taxCategory'),
                //         //new AppMenuItem('Currency', 'Pages.Currency', 'flaticon-more', '/app/main/masterData/currency'),
                //         //new AppMenuItem('HeadOfPayment', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/masterData/headOfPayment'),
                //         //new AppMenuItem('ErrorType', 'Pages.ErrorType', 'flaticon-more', '/app/main/masterData/errorType'),
                //         //new AppMenuItem('FinancialYear', 'Pages.FinancialYear', 'flaticon-more', '/app/main/masterData/financialYear'),
                //         //new AppMenuItem('Activecurrency', 'Pages.Activecurrency', 'flaticon-more', '/app/main/masterData/activecurrency'),
                //         //new AppMenuItem('CustomReport', 'Pages.CustomReport', 'flaticon-more', '/app/main/customReportSP/customReport'),
                //         //new AppMenuItem('TurnoverSlab', 'Pages.BusinessTurnoverSlab', 'flaticon-more', '/app/main/masterData/businessTurnoverSlab'),
                //         new AppMenuItem(
                //             'Operational Model',
                //             'Pages.BusinessOperationalModel',
                //             'flaticon-more',
                //             '/app/main/masterData/businessOperationalModel'
                //         ),
                //     ]
                // ),
                new AppMenuItem(
                    'Corporate Tax',
                    'Pages.Tenant.Dashboard',
                    'flaticon-interface-8',
                    '',
                    [],
                    [
                        new AppMenuItem(
                            'Tax Return Report',
                            'Pages.Title',
                            'flaticon-more',
                            '/app/main/Report/CorporateTaxReport/TaxReturnReport'
                        ),
                    ]
                ),

                new AppMenuItem(
                    'Master',
                    'Pages.Tenant.Dashboard',
                    'flaticon-interface-8',
                    '',
                    [],
                    [
                        new AppMenuItem(
                            'Tenant Details',
                            'Pages.Tenant.Dashboard',
                            'flaticon-line-graph',
                            '/app/main/TenantDetails'
                        ),
                        new AppMenuItem('Customer', 'Pages.Title', 'flaticon-more', '/app/main/Customers'),
                        new AppMenuItem('Vendor', 'Pages.Title', 'flaticon-more', '/app/main/Vendors'),
                    ]
                ),

                new AppMenuItem('Editions', 'Pages.Tenants', 'flaticon-app', '/app/admin/editions'),
                new AppMenuItem(
                    'Masters',
                    'Pages.Tenants',
                    'flaticon-interface-8',
                    '',
                    [],
                    [
                        new AppMenuItem('Title', 'Pages.Title', 'flaticon-more', '/app/main/masterData/title'),
                        new AppMenuItem('Gender', 'Pages.Gender', 'flaticon-more', '/app/main/masterData/gender'),
                        new AppMenuItem('Sector', 'Pages.Sector', 'flaticon-more', '/app/main/masterData/sector'),
                        new AppMenuItem(
                            'Tenant Type',
                            'Pages.TenantType',
                            'flaticon-more',
                            '/app/main/masterData/tenantType'
                        ),
                        new AppMenuItem(
                            'Constitution',
                            'Pages.Constitution',
                            'flaticon-more',
                            '/app/main/masterData/constitution'
                        ),
                        new AppMenuItem(
                            'Transaction Category',
                            'Pages.TransactionCategory',
                            'flaticon-more',
                            '/app/main/masterData/transactionCategory'
                        ),
                        new AppMenuItem('Country', 'Pages.Country', 'flaticon-more', '/app/main/masterData/country'),
                        new AppMenuItem(
                            'Tax SubCategory',
                            'Pages.TaxSubCategory',
                            'flaticon-more',
                            '/app/main/masterData/taxSubCategory'
                        ),
                        new AppMenuItem(
                            'BusinessProcess',
                            'Pages.BusinessProcess',
                            'flaticon-more',
                            '/app/main/masterData/businessProcess'
                        ),
                        new AppMenuItem(
                            'PaymentMeans',
                            'Pages.PaymentMeans',
                            'flaticon-more',
                            '/app/main/masterData/paymentMeans'
                        ),
                        new AppMenuItem(
                            'DocumentMaster',
                            'Pages.DocumentMaster',
                            'flaticon-more',
                            '/app/main/masterData/documentMaster'
                        ),
                        new AppMenuItem(
                            'Reason CNDN',
                            'Pages.ReasonCNDN',
                            'flaticon-more',
                            '/app/main/masterData/reasonCNDN'
                        ),
                        new AppMenuItem(
                            'Exemption Reason',
                            'Pages.ExemptionReason',
                            'flaticon-more',
                            '/app/main/masterData/exemptionReason'
                        ),
                        new AppMenuItem(
                            'Nature of Service',
                            'Pages.NatureofServices',
                            'flaticon-more',
                            '/app/main/masterData/natureofServices'
                        ),
                        new AppMenuItem(
                            'UnitOfMeasurement',
                            'Pages.UnitOfMeasurement',
                            'flaticon-more',
                            '/app/main/masterData/unitOfMeasurement'
                        ),
                        new AppMenuItem(
                            'AllowanceReason',
                            'Pages.AllowanceReason',
                            'flaticon-more',
                            '/app/main/masterData/allowanceReason'
                        ),
                        new AppMenuItem(
                            'PurchaseType',
                            'Pages.PurchaseType',
                            'flaticon-more',
                            '/app/main/masterData/purchaseType'
                        ),
                        new AppMenuItem(
                            'OrganisationType',
                            'Pages.OrganisationType',
                            'flaticon-more',
                            '/app/main/masterData/organisationType'
                        ),
                        new AppMenuItem(
                            'PlaceOfPerformance',
                            'Pages.PlaceOfPerformance',
                            'flaticon-more',
                            '/app/main/masterData/placeOfPerformance'
                        ),
                        new AppMenuItem(
                            'Affiliation',
                            'Pages.Affiliation',
                            'flaticon-more',
                            '/app/main/masterData/affiliation'
                        ),
                        // new AppMenuItem('ErrorGroup', 'Pages.ErrorGroup', 'flaticon-more', '/app/main/masterData/errorGroup'),
                        new AppMenuItem(
                            'InvoiceType',
                            'Pages.InvoiceType',
                            'flaticon-more',
                            '/app/main/masterData/invoiceType'
                        ),
                        new AppMenuItem(
                            'InvoiceCategory',
                            'Pages.InvoiceCategory',
                            'flaticon-more',
                            '/app/main/masterData/invoiceCategory'
                        ),
                        new AppMenuItem(
                            'TaxCategory',
                            'Pages.TaxCategory',
                            'flaticon-more',
                            '/app/main/masterData/taxCategory'
                        ),
                        new AppMenuItem('Currency', 'Pages.Currency', 'flaticon-more', '/app/main/masterData/currency'),
                        new AppMenuItem(
                            'HeadOfPayment',
                            'Pages.HeadOfPayment',
                            'flaticon-more',
                            '/app/main/masterData/headOfPayment'
                        ),
                        // new AppMenuItem('ErrorType', 'Pages.ErrorType', 'flaticon-more', '/app/main/masterData/errorType'),
                        new AppMenuItem(
                            'FinancialYear',
                            'Pages.FinancialYear',
                            'flaticon-more',
                            '/app/main/masterData/financialYear'
                        ),
                        new AppMenuItem(
                            'Activecurrency',
                            'Pages.Activecurrency',
                            'flaticon-more',
                            '/app/main/masterData/activecurrency'
                        ),
                        // new AppMenuItem('CustomReport', 'Pages.CustomReport', 'flaticon-more', '/app/main/customReportSP/customReport'),
                        new AppMenuItem(
                            'TurnoverSlab',
                            'Pages.BusinessTurnoverSlab',
                            'flaticon-more',
                            '/app/main/masterData/businessTurnoverSlab'
                        ),
                        new AppMenuItem(
                            'Operational Model',
                            'Pages.BusinessOperationalModel',
                            'flaticon-more',
                            '/app/main/masterData/businessOperationalModel'
                        ),
                        new AppMenuItem(
                            'Designation',
                            'Pages.Designation',
                            'flaticon-more',
                            '/app/main/masterData/designation'
                        ),
                    ]
                ),
                new AppMenuItem(
                    'Configurations',
                    'Pages.Tenant.Dashboard',
                    'flaticon-more',
                    '/app/main/FileUpload/clearBatchSummery'
                ),
                new AppMenuItem(
                    'Administration',
                    'Pages.Tenants',
                    'flaticon-interface-8',
                    '',
                    [],
                    [
                        new AppMenuItem(
                            'OrganizationUnits',
                            'Pages.Administration.OrganizationUnits',
                            'flaticon-map',
                            '/app/admin/organization-units'
                        ),
                        new AppMenuItem('Roles', 'Pages.Administration.Roles', 'flaticon-suitcase', '/app/admin/roles'),
                        new AppMenuItem('Users', 'Pages.Administration.Users', 'flaticon-users', '/app/admin/users'),
                        new AppMenuItem(
                            'Languages',
                            'Pages.Administration.Languages',
                            'flaticon-tabs',
                            '/app/admin/languages',
                            ['/app/admin/languages/{name}/texts']
                        ),
                        new AppMenuItem(
                            'AuditLogs',
                            'Pages.Administration.AuditLogs',
                            'flaticon-folder-1',
                            '/app/admin/auditLogs'
                        ),
                        new AppMenuItem(
                            'Maintenance',
                            'Pages.Administration.Host.Maintenance',
                            'flaticon-lock',
                            '/app/admin/maintenance'
                        ),
                        new AppMenuItem(
                            'Subscription',
                            'Pages.Administration.Tenant.SubscriptionManagement',
                            'flaticon-refresh',
                            '/app/admin/subscription-management'
                        ),
                        new AppMenuItem(
                            'VisualSettings',
                            'Pages.Administration.UiCustomization',
                            'flaticon-medical',
                            '/app/admin/ui-customization'
                        ),
                        new AppMenuItem(
                            'WebhookSubscriptions',
                            'Pages.Administration.WebhookSubscription',
                            'flaticon2-world',
                            '/app/admin/webhook-subscriptions'
                        ),
                        new AppMenuItem(
                            'DynamicProperties',
                            'Pages.Administration.DynamicProperties',
                            'flaticon-interface-8',
                            '/app/admin/dynamic-property'
                        ),
                        new AppMenuItem(
                            'Settings',
                            'Pages.Administration.Host.Settings',
                            'flaticon-settings',
                            '/app/admin/hostSettings'
                        ),
                        new AppMenuItem(
                            'Settings',
                            'Pages.Administration.Tenant.Settings',
                            'flaticon-settings',
                            '/app/admin/tenantSettings'
                        ),
                        new AppMenuItem(
                            'Notifications',
                            '',
                            'flaticon-alarm',
                            '',
                            [],
                            [
                                new AppMenuItem('Inbox', '', 'flaticon-mail-1', '/app/notifications'),
                                new AppMenuItem(
                                    'MassNotifications',
                                    'Pages.Administration.MassNotification',
                                    'flaticon-paper-plane',
                                    '/app/admin/mass-notifications'
                                ),
                            ]
                        ),
                    ]
                ),

                // new AppMenuItem(
                //     'FileUpload',
                //     '',
                //     'flaticon-interface-8',
                //     '',
                //     [],
                //     [
                //         new AppMenuItem('Sales File Upload', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/salesFileUpload'),
                //     ]),
                // new AppMenuItem(
                //     'DemoUiComponents',
                //     'Pages.DemoUiComponents',
                //     'flaticon-shapes',
                //     '/app/admin/demo-ui-components'
                // ),
            ]);

            let salesMenu = new AppMenuItem('Sales', 'Pages.Tenant.Dashboard', 'flaticon-interface-8', '', [], []);
            let goods = [
                new AppMenuItem('Transaction', 'Pages.Title', 'flaticon-more', '/app/main/sales/transactions'),
                // new AppMenuItem(
                //     'Create Sales Invoice',
                //     'Pages.Title',
                //     'flaticon-more',
                //     '/app/main/sales/createSalesInvoice'
                // ),
                new AppMenuItem(
                    'Sales Invoice',
                    'Pages.Title',
                    'fi fi-rs-edit',
                    '/app/main/sales/createSalesInvoiceBrady'
                ),
                new AppMenuItem(
                    'Credit Note',
                    'Pages.Title',
                    'fi fi-rs-edit',
                    '/app/main/sales/createCreditNoteBrady'
                ),
                new AppMenuItem(
                    'Debit Note',
                    'Pages.Title',
                    'fi fi-rs-edit',
                    '/app/main/sales/createDebitNoteBrady'
                ),
                // new AppMenuItem(
                //     'Create Credit Note',
                //     'Pages.Title',
                //     'flaticon-more',
                //     '/app/main/sales/createCreditNote'
                // ),
                // new AppMenuItem('Create Debit Note', 'Pages.Title', 'flaticon-more', '/app/main/sales/createDebitNote'),
            ];

            let services = [
                new AppMenuItem(
                    'Create Professional Sales Invoice',
                    'Pages.Title',
                    'flaticon-more',
                    '/app/main/sales/createSalesInvoiceProfessional'
                ),

                new AppMenuItem(
                    'Create Professional Credit Note',
                    'Pages.Title',
                    'flaticon-more',
                    '/app/main/sales/createCreditNoteProfessional'
                ),
                new AppMenuItem(
                    'Create Professional Debit Note',
                    'Pages.Title',
                    'flaticon-more',
                    '/app/main/sales/createDebitNoteProfessional'
                ),
  
            ];
            // this.isGoods ? salesMenu.items.push(...goods) : null
            // this.isServices ? salesMenu.items.push(...services) : null
            salesMenu.items.push(...goods);
            salesMenu.items.push(...services);
            appmenu.items.splice(2, 0, salesMenu);
            return appmenu;
        } else {
            let appmenu = new AppMenu('MainMenu', 'MainMenu', [
                // new AppMenuItem(
                //     'Dashboard',
                //     'Pages.Administration.Host.Dashboard',
                //     'flaticon-line-graph',
                //     '/app/admin/hostDashboard'
                // ),
                new AppMenuItem('Dashboard', 'Pages.Tenant.Dashboard', 'flaticon-line-graph', '/app/main/dashboard'),
                new AppMenuItem('Companies', 'Pages.Tenants', 'flaticon-list-3', '/app/admin/tenants'),
                //   new AppMenuItem('Company Details', 'Pages.Administration.Host.Settings', 'flaticon-more', '/app/main/TenantDetails'),
                // new AppMenuItem('Customer', 'Pages.Tenant.Dashboard', 'flaticon-more', '/app/main/Customers'),
                /// new AppMenuItem('Vendor', 'Pages.Title', 'flaticon-more', '/app/main/Vendors'),
                // new AppMenuItem(
                //     'Sales',
                //     '',
                //     'flaticon-interface-8',
                //     '',
                //     [],
                //     [
                // new AppMenuItem(
                //     'Create Sales Invoice',
                //     'Pages.Title',
                //     'flaticon-more',
                //     '/app/main/sales/createSalesInvoice'
                // ),
                // new AppMenuItem('Sales Invoice', 'Pages.Title', 'flaticon-more', '/app/main/sales/salesInvoice'),
                // new AppMenuItem(
                //     'Create Credit Note',
                //     'Pages.Title',
                //     'flaticon-more',
                //     '/app/main/sales/createCreditNote'
                // ),
                // new AppMenuItem('Credit Note', 'Pages.Title', 'flaticon-more', '/app/main/sales/creditNote'),
                // new AppMenuItem(
                //     'Create Debit Note',
                //     'Pages.Title',
                //     'flaticon-more',
                //     '/app/main/sales/createDebitNote'
                // ),
                //         new AppMenuItem('Debit Note', 'Pages.Title', 'flaticon-more', '/app/main/sales/debitNote'),
                //     ]
                // ),

                // new AppMenuItem(
                //     'Purchase',
                //     '',
                //     'flaticon-interface-8',
                //     '',
                //     [],
                //     [
                //         new AppMenuItem('Purchase Entry', 'Pages.Title', 'flaticon-more', '/app/main/sales/purchaseEntry'),
                //         new AppMenuItem('Create Purchase Entry', 'Pages.Title', 'flaticon-more', '/app/main/sales/createPurchaseEntry'),
                //         // new AppMenuItem('Debit Note', 'Pages.Title', 'flaticon-more', '/app/main/sales/debitNote'),
                //         // new AppMenuItem('Create Debit Note', 'Pages.Title', 'flaticon-more', '/app/main/sales/createDebitNote')

                //     ]),

                new AppMenuItem(
                    'Reports',
                    'Pages.Tenant.Dashboard',
                    'flaticon-interface-11',
                    '',
                    [],
                    [
                        new AppMenuItem(
                            'Sales Report',
                            'Pages.HeadOfPayment',
                            'fi fi-rs-file-invoice-dollar',
                            '/app/main/Report/SalesReport'
                        ),
                        new AppMenuItem(
                            'Credit Note Report',
                            'Pages.HeadOfPayment',
                            'fi fi-rs-file-invoice',
                            '/app/main/Report/CreditSalesReport'
                        ),
                        new AppMenuItem(
                            'Debit Note Report',
                            'Pages.HeadOfPayment',
                            'fi fi-rs-file-invoice',
                            '/app/main/Report/DebitSalesReport'
                        ),

                        // new AppMenuItem('Sales Detailed Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/SalesDetailedReport'),
                        // new AppMenuItem('Sales Daywise Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/SalesDaywiseReport'),
                        // new AppMenuItem('Credit Note (Sales) Detailed Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/CreditNoteReport'),
                        // new AppMenuItem('Credit Note (Sales) Daywise Report', 'Pages.Title', 'flaticon-more', '/app/main/Report/CreditNoteSalesDayWiseReport'),
                        // new AppMenuItem('Debit Note (Sales) Detailed Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/DebitNoteReport'),
                        // new AppMenuItem('Debit Note (Sales) Daywise Report', 'Pages.Title', 'flaticon-more', '/app/main/Report/DebitNoteSalesDayWiseReport'),
                        // new AppMenuItem('Purchase Entry Detailed Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/PurchaseEntryDetailedReport'),
                        // new AppMenuItem('Purchase Entry Daywise Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/PurchaseEntryDaywiseReport'),
                        //  new AppMenuItem('Credit Note (Purchase) Detailed Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/CreditNotePurchaseDetailedReport'),
                        //  new AppMenuItem('Credit Note (Purchase) Daywise Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/CreditNotePurchaseDayWiseReport'),
                        //   new AppMenuItem('Debit Note (Purchase) Detailed Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/DebitNotePurchaseDetailedReport'),
                        //  new AppMenuItem('Debit Note (Purchase) Daywise Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/DebitNotePurchaseDayWiseReport'),
                        //  new AppMenuItem('Override Report', 'Pages.Title', 'flaticon-more', '/app/main/Report/OverrideReport'),
                        new AppMenuItem(
                            'VAT Return Report',
                            'Pages.HeadOfPayment',
                            'fi fi-rs-clipboard-list',
                            '/app/main/Report/VATReport'
                        ),
                        //   new AppMenuItem(
                        //         'Reconciliation Report',
                        //         '',
                        //         'flaticon-more',
                        //         '',
                        //         [],
                        //         [
                        //             new AppMenuItem('Sales Invoice', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/ReconciliationReport'),
                        //             new AppMenuItem('Sales Credit Note', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/SalesCreditNoteReconciliationReport'),
                        //             new AppMenuItem('Sales Debit Note', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/SalesDebitNoteReconciliationReport'),
                        //             new AppMenuItem('Purchase', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/PurchaseReconciliationReport'),
                        //             new AppMenuItem('Purchase Credit Note', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/PurchaseCreditNoteReconciliationReport'),
                        //             new AppMenuItem('Purchase Debit Note', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/PurchaseDebitNoteReconciliationReport'),
                        //             new AppMenuItem('Overheads', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/OverheadsReconciliationReport'),

                        //         ]
                        //         ),

                        //new AppMenuItem('Reconciliation Report', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/Report/ReconciliationReport'),
                        //    new AppMenuItem('WHT Detailed Report', 'Pages.Title', 'flaticon-more', '/app/main/Payment/DetailedReport'),
                        //   new AppMenuItem('WHT Return Report', 'Pages.Title', 'flaticon-more', '/app/main/Payment/ReturnReport'),
                        //   new AppMenuItem('Custom Report', 'Pages.Title', 'flaticon-more', '/app/main/Reports/CustomReport')
                    ]
                ),

                // new AppMenuItem(
                //     'File Upload (Transaction)',
                //     '',
                //     'flaticon-interface-8',
                //     '',
                //     [],
                //     [
                //         new AppMenuItem('Batch Summary', 'Pages.Title', 'flaticon-more', '/app/main/sales/BatchSummary'),
                //         new AppMenuItem('Sales', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/salesFileUpload'),
                //         new AppMenuItem('Credit Note (Sales)', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/CreditNoteFileupload'),
                //         new AppMenuItem('Debit Note (Sales)', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/DebitNoteSalesFileUpload'),
                //       //  new AppMenuItem('Purchase Entry', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/PurchaseEntryFileupload'),
                //       //  new AppMenuItem('Credit Note (Purchase)', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/CreditNotePurchaseFileUpload'),
                //     //    new AppMenuItem('Debit Note (Purchase)', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/DebitNotePurchaseFileUpload'),
                //       //  new AppMenuItem('Payment', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/PaymentFileupload'),
                //         //new AppMenuItem('Customer File Upload', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/customerFileUpload'),
                //         //new AppMenuItem('Vendor File Upload', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/vendorFileUpload'),
                //       //  new AppMenuItem('Trial Balance', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/TrialBalanceFileUpload'),

                //     ]),

                // new AppMenuItem(
                //     'File Upload - Master',
                //     '',
                //     'flaticon-interface-8',
                //     '',
                //     [],
                //     [
                //         new AppMenuItem('Master Batch Summary', 'Pages.Title', 'flaticon-more', '/app/main/sales/MasterBatchSummary'),
                //         new AppMenuItem('Customer File Upload', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/customerFileUpload'),
                //        // new AppMenuItem('Vendor File Upload', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/vendorFileUpload'),
                //         //new AppMenuItem('Customer File Upload', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/customerFileUpload'),
                //         //new AppMenuItem('Vendor File Upload', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/vendorFileUpload'),                    new AppMenuItem('Vendor', 'Pages.Title', 'flaticon-more', '/app/main/Report/CorporateTaxReport/VendorMaster'),
                //        // new AppMenuItem('Ledger Master', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/LedgerFileUpload'),

                //     ]),

                // new AppMenuItem(
                //     'Corporate Tax',
                //     '',
                //     'flaticon-interface-8',
                //     '',
                //     [],
                //     [
                //         new AppMenuItem('Tax Return Report', 'Pages.Title', 'flaticon-more', '/app/main/Report/CorporateTaxReport/TaxReturnReport'),
                //     ]),

                new AppMenuItem('Editions', 'Pages.Editions', 'flaticon-app', '/app/admin/editions'),
                // new AppMenuItem(
                //     'Masters',
                //     'Pages.Administration.Host.Settings',
                //     'flaticon-interface-8',
                //     '',
                //     [],
                //     [
                //         new AppMenuItem('Title', 'Pages.Title', 'flaticon-more', '/app/main/masterData/title'),
                //         new AppMenuItem('Gender', 'Pages.Gender', 'flaticon-more', '/app/main/masterData/gender'),
                //         // new AppMenuItem('Sector', 'Pages.Sector', 'flaticon-more', '/app/main/masterData/sector'),
                //         new AppMenuItem(
                //             'Tenant Type',
                //             'Pages.TenantType',
                //             'flaticon-more',
                //             '/app/main/masterData/tenantType'
                //         ),
                //         new AppMenuItem(
                //             'Constitution',
                //             'Pages.Constitution',
                //             'flaticon-more',
                //             '/app/main/masterData/constitution'
                //         ),
                //         //  new AppMenuItem('Transaction Category', 'Pages.TransactionCategory', 'flaticon-more', '/app/main/masterData/transactionCategory'),
                //         new AppMenuItem('Country', 'Pages.Country', 'flaticon-more', '/app/main/masterData/country'),
                //         //new AppMenuItem('Tax SubCategory', 'Pages.TaxSubCategory', 'flaticon-more', '/app/main/masterData/taxSubCategory'),
                //         //new AppMenuItem('BusinessProcess', 'Pages.BusinessProcess', 'flaticon-more', '/app/main/masterData/businessProcess'),
                //         //new AppMenuItem('PaymentMeans', 'Pages.PaymentMeans', 'flaticon-more', '/app/main/masterData/paymentMeans'),
                //         //new AppMenuItem('DocumentMaster', 'Pages.DocumentMaster', 'flaticon-more', '/app/main/masterData/documentMaster'),
                //         //new AppMenuItem('Reason CNDN', 'Pages.ReasonCNDN', 'flaticon-more', '/app/main/masterData/reasonCNDN'),
                //         //new AppMenuItem('Exemption Reason', 'Pages.ExemptionReason', 'flaticon-more', '/app/main/masterData/exemptionReason'),
                //         //new AppMenuItem('Nature of Service', 'Pages.NatureofServices', 'flaticon-more', '/app/main/masterData/natureofServices'),
                //         //new AppMenuItem('UnitOfMeasurement', 'Pages.UnitOfMeasurement', 'flaticon-more', '/app/main/masterData/unitOfMeasurement'),
                //         //new AppMenuItem('AllowanceReason', 'Pages.AllowanceReason', 'flaticon-more', '/app/main/masterData/allowanceReason'),
                //         //new AppMenuItem('PurchaseType', 'Pages.PurchaseType', 'flaticon-more', '/app/main/masterData/purchaseType'),
                //         //new AppMenuItem('OrganisationType', 'Pages.OrganisationType', 'flaticon-more', '/app/main/masterData/organisationType'),
                //         //new AppMenuItem('PlaceOfPerformance', 'Pages.PlaceOfPerformance', 'flaticon-more', '/app/main/masterData/placeOfPerformance'),
                //         //new AppMenuItem('Affiliation', 'Pages.Affiliation', 'flaticon-more', '/app/main/masterData/affiliation'),
                //         //new AppMenuItem('ErrorGroup', 'Pages.ErrorGroup', 'flaticon-more', '/app/main/masterData/errorGroup'),
                //         //new AppMenuItem('InvoiceType', 'Pages.InvoiceType', 'flaticon-more', '/app/main/masterData/invoiceType'),
                //         //new AppMenuItem('InvoiceCategory', 'Pages.InvoiceCategory', 'flaticon-more', '/app/main/masterData/invoiceCategory'),
                //         //new AppMenuItem('TaxCategory', 'Pages.TaxCategory', 'flaticon-more', '/app/main/masterData/taxCategory'),
                //         //new AppMenuItem('Currency', 'Pages.Currency', 'flaticon-more', '/app/main/masterData/currency'),
                //         //new AppMenuItem('HeadOfPayment', 'Pages.HeadOfPayment', 'flaticon-more', '/app/main/masterData/headOfPayment'),
                //         //new AppMenuItem('ErrorType', 'Pages.ErrorType', 'flaticon-more', '/app/main/masterData/errorType'),
                //         //new AppMenuItem('FinancialYear', 'Pages.FinancialYear', 'flaticon-more', '/app/main/masterData/financialYear'),
                //         //new AppMenuItem('Activecurrency', 'Pages.Activecurrency', 'flaticon-more', '/app/main/masterData/activecurrency'),
                //         //new AppMenuItem('CustomReport', 'Pages.CustomReport', 'flaticon-more', '/app/main/customReportSP/customReport'),
                //         //new AppMenuItem('TurnoverSlab', 'Pages.BusinessTurnoverSlab', 'flaticon-more', '/app/main/masterData/businessTurnoverSlab'),
                //         new AppMenuItem(
                //             'Operational Model',
                //             'Pages.BusinessOperationalModel',
                //             'flaticon-more',
                //             '/app/main/masterData/businessOperationalModel'
                //         ),
                //     ]
                // ),
                new AppMenuItem(
                    'Masters',
                    'Pages.Administration.Tenant.Settings',
                    'flaticon-interface-8',
                    '',
                    [],
                    [
                        new AppMenuItem(
                            'Company Details',
                            'Pages.Administration.Tenant.Settings',
                            'fi fi-tr-city',
                            '/app/main/TenantDetails'
                        ),
                        //  new AppMenuItem(
                        //         'Settings',
                        //         'Pages.Administration.Tenant.Settings',
                        //         'flaticon-settings',
                        //         '/app/admin/tenantSettings'
                        //     ),
                        new AppMenuItem('Customer', 'Pages.Title', 'flaticon-more', '/app/main/Customers'),
                        new AppMenuItem('Users', 'Pages.Administration.Users', 'flaticon-users', '/app/admin/users'),
                        new AppMenuItem('AuditLogs', 'Pages.Title', 'flaticon-folder-1', '/app/admin/auditLogs'),
                    ]
                ),
                new AppMenuItem(
                    'Administration',
                    'Pages.Administration.Host.Settings',
                    'flaticon-interface-8',
                    '',
                    [],
                    [
                        new AppMenuItem(
                            'OrganizationUnits',
                            'Pages.Administration.Host.Settings',
                            'flaticon-map',
                            '/app/admin/organization-units'
                        ),
                        // new AppMenuItem(
                        //     'Roles',
                        //     'Pages.Administration.Host.Settings',
                        //     'flaticon-suitcase',
                        //     '/app/admin/roles'
                        // ),
                        new AppMenuItem('Users', 'Pages.Administration.Users', 'flaticon-users', '/app/admin/users'),
                        new AppMenuItem(
                            'Languages',
                            'Pages.Administration.Host.Settings',
                            'flaticon-tabs',
                            '/app/admin/languages',
                            ['/app/admin/languages/{name}/texts']
                        ),
                        new AppMenuItem(
                            'AuditLogs',
                            'Pages.Administration.Host.Settings',
                            'flaticon-folder-1',
                            '/app/admin/auditLogs'
                        ),
                        new AppMenuItem(
                            'Maintenance',
                            'Pages.Administration.Host.Settings',
                            'flaticon-lock',
                            '/app/admin/maintenance'
                        ),
                        new AppMenuItem(
                            'Subscription',
                            'Pages.Administration.Host.Settings',
                            'flaticon-refresh',
                            '/app/admin/subscription-management'
                        ),
                        new AppMenuItem(
                            'VisualSettings',
                            'Pages.Administration.Host.Settings',
                            'flaticon-medical',
                            '/app/admin/ui-customization'
                        ),
                        new AppMenuItem(
                            'WebhookSubscriptions',
                            'Pages.Administration.Host.Settings',
                            'flaticon2-world',
                            '/app/admin/webhook-subscriptions'
                        ),
                        new AppMenuItem(
                            'DynamicProperties',
                            'Pages.Administration.Host.Settings',
                            'flaticon-interface-8',
                            '/app/admin/dynamic-property'
                        ),
                        new AppMenuItem(
                            'Settings',
                            'Pages.Administration.Host.Settings',
                            'flaticon-settings',
                            '/app/admin/hostSettings'
                        ),
                        // new AppMenuItem(
                        //     'Settings',
                        //     'Pages.Administration.Tenant.Settings',
                        //     'flaticon-settings',
                        //     '/app/admin/tenantSettings'
                        // ),
                        new AppMenuItem(
                            'Module',
                            'Pages.Administration.Users',
                            'flaticon-more',
                            '/app/admin/masterData/module'
                        ),

                        new AppMenuItem(
                            'Notifications',
                            'Pages.Administration.Host.Settings',
                            'flaticon-alarm',
                            '',
                            [],
                            [
                                new AppMenuItem('Inbox', '', 'flaticon-mail-1', '/app/notifications'),
                                new AppMenuItem(
                                    'MassNotifications',
                                    'Pages.Administration.MassNotification',
                                    'flaticon-paper-plane',
                                    '/app/admin/mass-notifications'
                                ),
                            ]
                        ),
                    ]
                ),
                new AppMenuItem(
                    'Masters',
                    'Pages.Administration.Host.Settings',
                    'flaticon-interface-8',
                    '',
                    [],
                    [
                        new AppMenuItem('Title', 'Pages.Title', 'flaticon-more', '/app/main/masterData/title'),
                        new AppMenuItem('Gender', 'Pages.Gender', 'flaticon-more', '/app/main/masterData/gender'),
                        new AppMenuItem('Sector', 'Pages.Sector', 'flaticon-more', '/app/main/masterData/sector'),
                        new AppMenuItem(
                            'Tenant Type',
                            'Pages.TenantType',
                            'flaticon-more',
                            '/app/main/masterData/tenantType'
                        ),
                        new AppMenuItem(
                            'Constitution',
                            'Pages.Constitution',
                            'flaticon-more',
                            '/app/main/masterData/constitution'
                        ),
                        new AppMenuItem(
                            'Transaction Category',
                            'Pages.TransactionCategory',
                            'flaticon-more',
                            '/app/main/masterData/transactionCategory'
                        ),
                        new AppMenuItem('Country', 'Pages.Country', 'flaticon-more', '/app/main/masterData/country'),
                        new AppMenuItem(
                            'Tax SubCategory',
                            'Pages.TaxSubCategory',
                            'flaticon-more',
                            '/app/main/masterData/taxSubCategory'
                        ),
                        new AppMenuItem(
                            'BusinessProcess',
                            'Pages.BusinessProcess',
                            'flaticon-more',
                            '/app/main/masterData/businessProcess'
                        ),
                        new AppMenuItem(
                            'PaymentMeans',
                            'Pages.PaymentMeans',
                            'flaticon-more',
                            '/app/main/masterData/paymentMeans'
                        ),
                        new AppMenuItem(
                            'DocumentMaster',
                            'Pages.DocumentMaster',
                            'flaticon-more',
                            '/app/main/masterData/documentMaster'
                        ),
                        new AppMenuItem(
                            'Reason CNDN',
                            'Pages.ReasonCNDN',
                            'flaticon-more',
                            '/app/main/masterData/reasonCNDN'
                        ),
                        new AppMenuItem(
                            'Exemption Reason',
                            'Pages.ExemptionReason',
                            'flaticon-more',
                            '/app/main/masterData/exemptionReason'
                        ),
                        new AppMenuItem(
                            'Nature of Service',
                            'Pages.NatureofServices',
                            'flaticon-more',
                            '/app/main/masterData/natureofServices'
                        ),
                        new AppMenuItem(
                            'UnitOfMeasurement',
                            'Pages.UnitOfMeasurement',
                            'flaticon-more',
                            '/app/main/masterData/unitOfMeasurement'
                        ),
                        new AppMenuItem(
                            'AllowanceReason',
                            'Pages.AllowanceReason',
                            'flaticon-more',
                            '/app/main/masterData/allowanceReason'
                        ),
                        new AppMenuItem(
                            'PurchaseType',
                            'Pages.PurchaseType',
                            'flaticon-more',
                            '/app/main/masterData/purchaseType'
                        ),
                        new AppMenuItem(
                            'OrganisationType',
                            'Pages.OrganisationType',
                            'flaticon-more',
                            '/app/main/masterData/organisationType'
                        ),
                        new AppMenuItem(
                            'PlaceOfPerformance',
                            'Pages.PlaceOfPerformance',
                            'flaticon-more',
                            '/app/main/masterData/placeOfPerformance'
                        ),
                        new AppMenuItem(
                            'Affiliation',
                            'Pages.Affiliation',
                            'flaticon-more',
                            '/app/main/masterData/affiliation'
                        ),
                        // new AppMenuItem('ErrorGroup', 'Pages.ErrorGroup', 'flaticon-more', '/app/main/masterData/errorGroup'),
                        new AppMenuItem(
                            'InvoiceType',
                            'Pages.InvoiceType',
                            'flaticon-more',
                            '/app/main/masterData/invoiceType'
                        ),
                        new AppMenuItem(
                            'InvoiceCategory',
                            'Pages.InvoiceCategory',
                            'flaticon-more',
                            '/app/main/masterData/invoiceCategory'
                        ),
                        new AppMenuItem(
                            'TaxCategory',
                            'Pages.TaxCategory',
                            'flaticon-more',
                            '/app/main/masterData/taxCategory'
                        ),
                        new AppMenuItem('Currency', 'Pages.Currency', 'flaticon-more', '/app/main/masterData/currency'),
                        new AppMenuItem(
                            'HeadOfPayment',
                            'Pages.HeadOfPayment',
                            'flaticon-more',
                            '/app/main/masterData/headOfPayment'
                        ),
                        // new AppMenuItem('ErrorType', 'Pages.ErrorType', 'flaticon-more', '/app/main/masterData/errorType'),
                        new AppMenuItem(
                            'FinancialYear',
                            'Pages.FinancialYear',
                            'flaticon-more',
                            '/app/main/masterData/financialYear'
                        ),
                        new AppMenuItem(
                            'Activecurrency',
                            'Pages.Activecurrency',
                            'flaticon-more',
                            '/app/main/masterData/activecurrency'
                        ),
                        // new AppMenuItem('CustomReport', 'Pages.CustomReport', 'flaticon-more', '/app/main/customReportSP/customReport'),
                        new AppMenuItem(
                            'TurnoverSlab',
                            'Pages.BusinessTurnoverSlab',
                            'flaticon-more',
                            '/app/main/masterData/businessTurnoverSlab'
                        ),
                        new AppMenuItem(
                            'Operational Model',
                            'Pages.BusinessOperationalModel',
                            'flaticon-more',
                            '/app/main/masterData/businessOperationalModel'
                        ),
                        new AppMenuItem(
                            'Designation',
                            'Pages.Designation',
                            'flaticon-more',
                            '/app/main/masterData/designation'
                        ),
                    ]
                ),

                // new AppMenuItem(
                //     'FileUpload',
                //     '',
                //     'flaticon-interface-8',
                //     '',
                //     [],
                //     [
                //         new AppMenuItem('Sales File Upload', 'Pages.Title', 'flaticon-more', '/app/main/FileUpload/salesFileUpload'),
                //     ]),
                // new AppMenuItem(
                //     'DemoUiComponents',
                //     'Pages.DemoUiComponents',
                //     'flaticon-shapes',
                //     '/app/admin/demo-ui-components'
                // ),
            ]);

            let salesMenu = new AppMenuItem(
                'Transactions',
                'Pages.Tenant.Dashboard',
                'flaticon-graphic',
                '',
                [],
                [new AppMenuItem('Transactions', 'Pages.Title', 'fi fi-rs-ballot', '/app/main/sales/transactions')]
            );

            let goods = [
                // new AppMenuItem(
                //     'Transactions',
                //     'Pages.Title',
                //     'fi fi-rs-ballot',
                //     '/app/main/sales/bradytransactions'
                // ),
                // new AppMenuItem(
                //     'Sales Invoice',
                //     'Pages.Title',
                //     'fi fi-rs-edit',
                //     '/app/main/sales/createSalesInvoice'
                // ),
                new AppMenuItem(
                    'Sales Invoice',
                    'Pages.Title',
                    'fi fi-rs-edit',
                    '/app/main/sales/createSalesInvoiceBrady'
                ),
                // new AppMenuItem(
                //     'Credit Note',
                //     'Pages.Title',
                //     'fi fi-rs-add-document',
                //     '/app/main/sales/createCreditNote'
                // ),
                new AppMenuItem('Credit Note', 'Pages.Title', 'fi fi-rs-edit', '/app/main/sales/createCreditNoteBrady'),
                // new AppMenuItem(
                //     'Debit Note',
                //     'Pages.Title',
                //     'fi fi-rs-layer-plus',
                //     '/app/main/sales/createDebitNote'
                // ),
                new AppMenuItem('Debit Note', 'Pages.Title', 'fi fi-rs-edit', '/app/main/sales/createDebitNoteBrady'),
            ];

            let services = [
                new AppMenuItem(
                    'Professional Sales Invoice',
                    'Pages.Title',
                    'fi fi-rs-edit',
                    '/app/main/sales/createSalesInvoiceProfessional'
                ),

                new AppMenuItem(
                    'Professional Credit Note ',
                    'Pages.Title',
                    'fi fi-rs-add-document',
                    '/app/main/sales/createCreditNoteProfessional'
                ),

                new AppMenuItem(
                    'Professional Debit Note',
                    'Pages.Title',
                    'fi fi-rs-layer-plus',
                    '/app/main/sales/createDebitNoteProfessional'
                ),
            ];

            this.isGoods ? salesMenu.items.push(...goods) : null;
            this.isServices ? salesMenu.items.push(...services) : null;
            appmenu.items.splice(2, 0, salesMenu);

            return appmenu;
        }
    }

    checkChildMenuItemPermission(menuItem): boolean {
        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName === '' || subMenuItem.permissionName === null) {
                if (subMenuItem.route) {
                    return true;
                }
            } else if (this._permissionCheckerService.isGranted(subMenuItem.permissionName)) {
                return true;
            }

            if (subMenuItem.items && subMenuItem.items.length) {
                let isAnyChildItemActive = this.checkChildMenuItemPermission(subMenuItem);
                if (isAnyChildItemActive) {
                    return true;
                }
            }
        }
        return false;
    }

    showMenuItem(menuItem: AppMenuItem): boolean {
        if (
            menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement' &&
            this._appSessionService.tenant &&
            !this._appSessionService.tenant.edition
        ) {
            return false;
        }

        let hideMenuItem = false;

        if (menuItem.requiresAuthentication && !this._appSessionService.user) {
            hideMenuItem = true;
        }

        if (menuItem.permissionName && !this._permissionCheckerService.isGranted(menuItem.permissionName)) {
            hideMenuItem = true;
        }

        if (this._appSessionService.tenant || !abp.multiTenancy.ignoreFeatureCheckForHostUsers) {
            if (menuItem.hasFeatureDependency() && !menuItem.featureDependencySatisfied()) {
                hideMenuItem = true;
            }
        }

        if (!hideMenuItem && menuItem.items && menuItem.items.length) {
            return this.checkChildMenuItemPermission(menuItem);
        }

        return !hideMenuItem;
    }

    /**
     * Returns all menu items recursively
     */
    getAllMenuItems(): AppMenuItem[] {
        let menu = this.getMenu();
        let allMenuItems: AppMenuItem[] = [];
        menu.items.forEach((menuItem) => {
            allMenuItems = allMenuItems.concat(this.getAllMenuItemsRecursive(menuItem));
        });

        return allMenuItems;
    }

    private getAllMenuItemsRecursive(menuItem: AppMenuItem): AppMenuItem[] {
        if (!menuItem.items) {
            return [menuItem];
        }

        let menuItems = [menuItem];
        menuItem.items.forEach((subMenu) => {
            menuItems = menuItems.concat(this.getAllMenuItemsRecursive(subMenu));
        });

        return menuItems;
    }
}

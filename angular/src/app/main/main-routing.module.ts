import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    {
                        path: 'fileUpload/results',
                        loadChildren: () =>
                            import('./FileUpload/FileUploadOutput/fileUploadOutput.module').then(
                                (m) => m.FileUploadOutputModule
                            ),
                        data: { permission: 'Pages.Designation' },
                    },
                    {
                        path: 'masterData/designation',
                        loadChildren: () =>
                            import('./masterData/designation/designation.module').then((m) => m.DesignationModule),
                        data: { permission: 'Pages.Designation' },
                    },

                    {
                        path: 'TenantDetails',
                        loadChildren: () =>
                            import('./TenantDetails/tenantdetails.module').then((m) => m.TenantDetailsModule),
                        data: { permission: 'Pages.Tenant.Dashboard' },
                    },
                    {
                        path: 'masterData/businessOperationalModel',
                        loadChildren: () =>
                            import('./masterData/businessOperationalModel/businessOperationalModel.module').then(
                                (m) => m.BusinessOperationalModelModule
                            ),
                        data: { permission: 'Pages.BusinessOperationalModel' },
                    },

                    {
                        path: 'masterData/businessTurnoverSlab',
                        loadChildren: () =>
                            import('./masterData/businessTurnoverSlab/businessTurnoverSlab.module').then(
                                (m) => m.BusinessTurnoverSlabModule
                            ),
                        data: { permission: 'Pages.BusinessTurnoverSlab' },
                    },

                    {
                        path: 'customReportSP/customReport',
                        loadChildren: () =>
                            import('./customReportSP/customReport/customReport.module').then(
                                (m) => m.CustomReportModule
                            ),
                        data: { permission: 'Pages.CustomReport' },
                    },
                    {
                        path: 'masterData/activecurrency',
                        loadChildren: () =>
                            import('./masterData/activecurrency/activecurrency.module').then(
                                (m) => m.ActivecurrencyModule
                            ),
                        data: { permission: 'Pages.Activecurrency' },
                    },
                    {
                        path: 'sales/transactions',
                        loadChildren: () =>
                            import('./sales/transactions/transactions.module').then((m) => m.TransactionsModule),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'sales/purchaseTransaction',
                        loadChildren: () =>
                            import('./sales/purchaseTransaction/purchaseTransaction.module').then(
                                (m) => m.PurchaseTransactionModule
                            ),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'sales/createSalesInvoice',
                        loadChildren: () =>
                            import('./sales/createSalesInvoice/createSalesInvoice.module').then(
                                (m) => m.CreateSalesInvoiceModule
                            ),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'sales/salesInvoice',
                        loadChildren: () =>
                            import('./sales/salesInvoice/salesInvoice.module').then((m) => m.SalesInvoiceModule),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'Report/SalesReport',
                        loadChildren: () =>
                            import('./Report/salesReport/salesReport.module').then((m) => m.SalesReportModule),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'Report/CreditSalesReport',
                        loadChildren: () =>
                            import('./Report/creditSalesReport/creditSalesReport.module').then(
                                (m) => m.CreditSalesReportModule
                            ),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'Report/DebitSalesReport',
                        loadChildren: () =>
                            import('./Report/debitSalesReport/debitSalesReport.module').then(
                                (m) => m.DebitSalesReportModule
                            ),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'Customers',
                        loadChildren: () => import('./customers/customers.module').then((m) => m.CustomerModule),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'TenantDetails',
                        loadChildren: () =>
                            import('./TenantDetails/tenantdetails.module').then((m) => m.TenantDetailsModule),
                        data: { permission: 'Pages.Tenant.Dashboard' },
                    },
                    {
                        path: 'customers/createCustomers',
                        loadChildren: () =>
                            import('./customers/createCustomers/createCustomers.module').then(
                                (m) => m.CreateCustomerModule
                            ),
                        data: { permission: 'Pages.InvoiceType' },
                    },

                    {
                        path: 'Vendors',
                        loadChildren: () => import('./vendors/vendors.module').then((m) => m.VendorModule),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'vendors/createVendors',
                        loadChildren: () =>
                            import('./vendors/createVendors/createVendors.module').then((m) => m.CreateVendorModule),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'sales/createCreditNote',
                        loadChildren: () =>
                            import('./sales/createCreditNote/createCreditNote.module').then(
                                (m) => m.CreateCreditNoteModule
                            ),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'Report/OverrideReport',
                        loadChildren: () =>
                            import('./Report/OverrideReport/overridereport.module').then((m) => m.OverrideReportModule),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'sales/createCreditNote',
                        loadChildren: () =>
                            import('./sales/createCreditNote/createCreditNote.module').then(
                                (m) => m.CreateCreditNoteModule
                            ),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'sales/creditNote',
                        loadChildren: () =>
                            import('./sales/creditNote/creditNote.module').then((m) => m.CreditNoteModule),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'sales/createDebitNote',
                        loadChildren: () =>
                            import('./sales/createDebitNote/createDebitNote.module').then(
                                (m) => m.CreateDebitNoteModule
                            ),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'sales/debitNote',
                        loadChildren: () => import('./sales/debitNote/debitNote.module').then((m) => m.DebitNoteModule),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'sales/createPurchaseEntry',
                        loadChildren: () =>
                            import('./sales/createPurchaseEntry/createPurchaseEntry.module').then(
                                (m) => m.CreatePurchaseEntryModule
                            ),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'sales/purchaseEntry',
                        loadChildren: () =>
                            import('./sales/purchaseEntry/purchaseEntry.module').then((m) => m.PurchaseEntryModule),
                        data: { permission: 'Pages.InvoiceType' },
                    },
                    {
                        path: 'masterData/invoiceType',
                        loadChildren: () =>
                            import('./masterData/invoiceType/invoiceType.module').then((m) => m.InvoiceTypeModule),
                        data: { permission: 'Pages.InvoiceType' },
                    },

                    {
                        path: 'masterData/financialYear',
                        loadChildren: () =>
                            import('./masterData/financialYear/financialYear.module').then(
                                (m) => m.FinancialYearModule
                            ),
                        data: { permission: 'Pages.FinancialYear' },
                    },

                    {
                        path: 'masterData/errorType',
                        loadChildren: () =>
                            import('./masterData/errorType/errorType.module').then((m) => m.ErrorTypeModule),
                        data: { permission: 'Pages.ErrorType' },
                    },

                    {
                        path: 'masterData/headOfPayment',
                        loadChildren: () =>
                            import('./masterData/headOfPayment/headOfPayment.module').then(
                                (m) => m.HeadOfPaymentModule
                            ),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/SalesDetailedReport',
                        loadChildren: () =>
                            import('./Report/salesdetailedreport/salesdetailedreport.module').then(
                                (m) => m.SalesDetailedReportModule
                            ),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/SalesDaywiseReport',
                        loadChildren: () =>
                            import('./Report/SalesDaywiseReport/salesdaywisereport.module').then(
                                (m) => m.SalesDaywiseReportModule
                            ),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/PurchaseEntryDetailedReport',
                        loadChildren: () =>
                            import('./Report/PurchaseDetailedReport/purchasedetailedreport.module').then(
                                (m) => m.PurchaseDetailedReportModule
                            ),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },

                    {
                        path: 'Report/PurchaseEntryDaywiseReport',
                        loadChildren: () =>
                            import('./Report/PurchaseDaywiseReport/purchasedaywisereport.module').then(
                                (m) => m.purchaseEntryDaywiseSummaryModule
                            ),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/DebitNoteReport',
                        loadChildren: () =>
                            import('./Report/DebitNoteReport/debitnotereport.module').then(
                                (m) => m.DebitNoteReportModule
                            ),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/VendorReport',
                        loadChildren: () =>
                            import('./Report/VendorReport/vendorreport.module').then((m) => m.VendorReportModule),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/CustomerReport',
                        loadChildren: () =>
                            import('./Report/CustomerReport/Customerreport.module').then((m) => m.CustomerReportModule),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/TenantReport',
                        loadChildren: () =>
                            import('./Report/TenantReport/Tenantreport.module').then((m) => m.TenantNoteReportModule),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/CreditNoteReport',
                        loadChildren: () =>
                            import('./Report/CreditNoteReport/creditnotereport.module').then(
                                (m) => m.CreditNoteReportModule
                            ),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/VATReport',
                        loadChildren: () =>
                            import('./Report/VATreport/vatreport.module').then((m) => m.VatReportNewModule),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/ReconciliationReport',
                        loadChildren: () =>
                            import(
                                './Report/Reconciliation Reports/SalesReconciliationReport/reconciliationreport.module'
                            ).then((m) => m.reconciliationreportNewModule),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/SalesCreditNoteReconciliationReport',
                        loadChildren: () =>
                            import(
                                './Report/Reconciliation Reports/SalesCreditReconciliationReport/salescredit.module'
                            ).then((m) => m.salescreditModule),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/SalesDebitNoteReconciliationReport',
                        loadChildren: () =>
                            import(
                                './Report/Reconciliation Reports/SalesDebitReconciliationReport/salesdebit.module'
                            ).then((m) => m.salesdebitModule),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/PurchaseReconciliationReport',
                        loadChildren: () =>
                            import(
                                './Report/Reconciliation Reports/PurchaseReconciliationReport/purchasereconciliationreport.module'
                            ).then((m) => m.purchasereconciliationreportNewModule),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/PurchaseCreditNoteReconciliationReport',
                        loadChildren: () =>
                            import(
                                './Report/Reconciliation Reports/PurchaseCreditReconciliationReport/purchasecredit.module'
                            ).then((m) => m.purchasecreditreportNewModule),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/PurchaseDebitNoteReconciliationReport',
                        loadChildren: () =>
                            import(
                                './Report/Reconciliation Reports/PurchaseDebitReconciliationReport/purchasedebit.module'
                            ).then((m) => m.purchasedebitreportNewModule),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/OverheadsReconciliationReport',
                        loadChildren: () =>
                            import(
                                './Report/Reconciliation Reports/OverheadaReconciliationReport/overheadsreconciliationreport.module'
                            ).then((m) => m.overheadsreconciliationreportNewModule),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },

                    {
                        path: 'masterData/currency',
                        loadChildren: () =>
                            import('./masterData/currency/currency.module').then((m) => m.CurrencyModule),
                        data: { permission: 'Pages.Currency' },
                    },

                    {
                        path: 'masterData/taxCategory',
                        loadChildren: () =>
                            import('./masterData/taxCategory/taxCategory.module').then((m) => m.TaxCategoryModule),
                        data: { permission: 'Pages.TaxCategory' },
                    },

                    {
                        path: 'masterData/invoiceCategory',
                        loadChildren: () =>
                            import('./masterData/invoiceCategory/invoiceCategory.module').then(
                                (m) => m.InvoiceCategoryModule
                            ),
                        data: { permission: 'Pages.InvoiceCategory' },
                    },

                    {
                        path: 'masterData/errorGroup',
                        loadChildren: () =>
                            import('./masterData/errorGroup/errorGroup.module').then((m) => m.ErrorGroupModule),
                        data: { permission: 'Pages.ErrorGroup' },
                    },

                    {
                        path: 'masterData/affiliation',
                        loadChildren: () =>
                            import('./masterData/affiliation/affiliation.module').then((m) => m.AffiliationModule),
                        data: { permission: 'Pages.Affiliation' },
                    },

                    {
                        path: 'masterData/placeOfPerformance',
                        loadChildren: () =>
                            import('./masterData/placeOfPerformance/placeOfPerformance.module').then(
                                (m) => m.PlaceOfPerformanceModule
                            ),
                        data: { permission: 'Pages.PlaceOfPerformance' },
                    },

                    {
                        path: 'masterData/organisationType',
                        loadChildren: () =>
                            import('./masterData/organisationType/organisationType.module').then(
                                (m) => m.OrganisationTypeModule
                            ),
                        data: { permission: 'Pages.OrganisationType' },
                    },

                    {
                        path: 'masterData/purchaseType',
                        loadChildren: () =>
                            import('./masterData/purchaseType/purchaseType.module').then((m) => m.PurchaseTypeModule),
                        data: { permission: 'Pages.PurchaseType' },
                    },

                    {
                        path: 'masterData/allowanceReason',
                        loadChildren: () =>
                            import('./masterData/allowanceReason/allowanceReason.module').then(
                                (m) => m.AllowanceReasonModule
                            ),
                        data: { permission: 'Pages.AllowanceReason' },
                    },

                    {
                        path: 'masterData/unitOfMeasurement',
                        loadChildren: () =>
                            import('./masterData/unitOfMeasurement/unitOfMeasurement.module').then(
                                (m) => m.UnitOfMeasurementModule
                            ),
                        data: { permission: 'Pages.UnitOfMeasurement' },
                    },

                    {
                        path: 'masterData/natureofServices',
                        loadChildren: () =>
                            import('./masterData/natureofServices/natureofServices.module').then(
                                (m) => m.NatureofServicesModule
                            ),
                        data: { permission: 'Pages.NatureofServices' },
                    },

                    {
                        path: 'masterData/exemptionReason',
                        loadChildren: () =>
                            import('./masterData/exemptionReason/exemptionReason.module').then(
                                (m) => m.ExemptionReasonModule
                            ),
                        data: { permission: 'Pages.ExemptionReason' },
                    },

                    {
                        path: 'masterData/reasonCNDN',
                        loadChildren: () =>
                            import('./masterData/reasonCNDN/reasonCNDN.module').then((m) => m.ReasonCNDNModule),
                        data: { permission: 'Pages.ReasonCNDN' },
                    },

                    {
                        path: 'masterData/documentMaster',
                        loadChildren: () =>
                            import('./masterData/documentMaster/documentMaster.module').then(
                                (m) => m.DocumentMasterModule
                            ),
                        data: { permission: 'Pages.DocumentMaster' },
                    },

                    {
                        path: 'masterData/paymentMeans',
                        loadChildren: () =>
                            import('./masterData/paymentMeans/paymentMeans.module').then((m) => m.PaymentMeansModule),
                        data: { permission: 'Pages.PaymentMeans' },
                    },

                    {
                        path: 'masterData/businessProcess',
                        loadChildren: () =>
                            import('./masterData/businessProcess/businessProcess.module').then(
                                (m) => m.BusinessProcessModule
                            ),
                        data: { permission: 'Pages.BusinessProcess' },
                    },

                    {
                        path: 'masterData/taxSubCategory',
                        loadChildren: () =>
                            import('./masterData/taxSubCategory/taxSubCategory.module').then(
                                (m) => m.TaxSubCategoryModule
                            ),
                        data: { permission: 'Pages.TaxSubCategory' },
                    },

                    {
                        path: 'masterData/country',
                        loadChildren: () => import('./masterData/country/country.module').then((m) => m.CountryModule),
                        data: { permission: 'Pages.Country' },
                    },

                    {
                        path: 'masterData/transactionCategory',
                        loadChildren: () =>
                            import('./masterData/transactionCategory/transactionCategory.module').then(
                                (m) => m.TransactionCategoryModule
                            ),
                        data: { permission: 'Pages.TransactionCategory' },
                    },

                    {
                        path: 'masterData/constitution',
                        loadChildren: () =>
                            import('./masterData/constitution/constitution.module').then((m) => m.ConstitutionModule),
                        data: { permission: 'Pages.Constitution' },
                    },
                    {
                        path: 'errorList/importstandardfilesErrorLists/:id',
                        loadChildren: () =>
                            import('./importstandardfilesErrorLists/importstandardfilesErrorList.module').then(
                                (m) => m.ImportstandardfilesErrorListModule
                            ),
                        data: { permission: 'Pages.Administration.ImportstandardfilesErrorLists' },
                    },
                    {
                        path: 'sales/BatchSummary',
                        loadChildren: () =>
                            import('./FileUpload/BatchSummary/batchUpload.module').then((m) => m.BatchUploadModule),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'sales/MasterBatchSummary',
                        loadChildren: () =>
                            import('./FileUpload/Master Batch Summary/MasterbatchUpload.module').then(
                                (m) => m.MasterBatchUploadModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'masterData/tenantType',
                        loadChildren: () =>
                            import('./masterData/tenantType/tenantType.module').then((m) => m.TenantTypeModule),
                        data: { permission: 'Pages.TenantType' },
                    },

                    {
                        path: 'masterData/sector',
                        loadChildren: () => import('./masterData/sector/sector.module').then((m) => m.SectorModule),
                        data: { permission: 'Pages.Sector' },
                    },

                    {
                        path: 'masterData/gender',
                        loadChildren: () => import('./masterData/gender/gender.module').then((m) => m.GenderModule),
                        data: { permission: 'Pages.Gender' },
                    },

                    {
                        path: 'masterData/title',
                        loadChildren: () => import('./masterData/title/title.module').then((m) => m.TitleModule),
                        data: { permission: 'Pages.Title' },
                    },

                    {
                        path: 'dashboard',
                        loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
                        data: { permission: 'Pages.Tenant.Dashboard' },
                    },
                    {
                        path: 'FileUpload/salesFileUpload',
                        loadChildren: () =>
                            import('./FileUpload/salesInvoiceFileUpload/salesInvoiceFileUpload.module').then(
                                (m) => m.SalesInvoiceFileUploadModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'FileUpload/customerFileUpload',
                        loadChildren: () =>
                            import('./FileUpload/customerFileUpload/customerFileUpload.module').then(
                                (m) => m.CustomerUploadModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'FileUpload/tenantFileUpload',
                        loadChildren: () =>
                            import('./FileUpload/TenantFileUpload/tenantfileupload.module').then(
                                (m) => m.TenantUploadModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'FileUpload/vendorFileUpload',
                        loadChildren: () =>
                            import('./FileUpload/vendorFileUpload/vendorFileUpload.module').then(
                                (m) => m.VendorUploadModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'FileUpload/LedgerFileUpload',
                        loadChildren: () =>
                            import('./FileUpload/LedgerFileUpload/Ledgerfileupload.module').then(
                                (m) => m.LedgerUploadModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'FileUpload/TrialBalanceFileUpload',
                        loadChildren: () =>
                            import('./FileUpload/TrialBalance/TrialBalancefileupload.module').then(
                                (m) => m.TrialBalanceUploadModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'FileUpload/CreditNoteFileupload',
                        loadChildren: () =>
                            import('./FileUpload/CreditNoteFileUpload/creditnotefileupload.module').then(
                                (m) => m.NewFileCreditBatchUploadModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'FileUpload/PurchaseEntryFileupload',
                        loadChildren: () =>
                            import('./FileUpload/PurchaseFileUpload/purchasefileupload.module').then(
                                (m) => m.NewFilePurchaseBatchUploadModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'FileUpload/PaymentFileupload',
                        loadChildren: () =>
                            import('./FileUpload/PaymentFileUpload/paymentfileupload.module').then(
                                (m) => m.WTHPaymentUploadModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'FileUpload/DebitNoteSalesFileUpload',
                        loadChildren: () =>
                            import('./FileUpload/DebitNoteSalesFileUpload/debitnotesalesfileupload.module').then(
                                (m) => m.NewFileDebitSalesBatchUploadModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'FileUpload/CreditNotePurchaseFileUpload',
                        loadChildren: () =>
                            import('./FileUpload/CreditNotePurchaseFileUpload/creditnotepurcasefileupload.module').then(
                                (m) => m.NewFileCreditPurchaseBatchUploadModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'FileUpload/DebitNotePurchaseFileUpload',
                        loadChildren: () =>
                            import('./FileUpload/DebitNotePurchaseFileUpload/DebitNotePurchaseFileUpload.module').then(
                                (m) => m.NewFileDebitPurchaseBatchUploadModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'Report/CorporateTaxReport/TaxReturnReport',
                        loadChildren: () =>
                            import('./Report/CorporateTaxReport/TaxReturnReport/taxreturnreport.module').then(
                                (m) => m.TaxReturnReportNewModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'Payment/DetailedReport',
                        loadChildren: () =>
                            import('./Report/PaymentReport/DetailedReport/detailedReport.module').then(
                                (m) => m.DetailedReportModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'Payment/ReturnReport',
                        loadChildren: () =>
                            import('./Report/PaymentReport/ReturnReport/returnReport.module').then(
                                (m) => m.ReturnReportModule
                            ),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'Reports/CustomReport',
                        loadChildren: () =>
                            import('./Report/CustomReport/customReport.module').then((m) => m.CustomReportModule),
                        data: { permission: 'Pages.Title' },
                    },
                    {
                        path: 'Report/CreditNoteSalesDayWiseReport',
                        loadChildren: () =>
                            import('./Report/CreditNoteSalesDayWiseReport/creditnotesalesdaywise.module').then(
                                (m) => m.CreditNoteSalesDaywiseReportModule
                            ),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/DebitNoteSalesDayWiseReport',
                        loadChildren: () =>
                            import('./Report/DebitNoteSalesDayWiseReport/debitnotesalesdaywisereport.module').then(
                                (m) => m.DebitNoteSalesDaywiseReportModule
                            ),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/DebitNotePurchaseDayWiseReport',
                        loadChildren: () =>
                            import('./Report/DebitNotePurchaseDayWiseReport/debitnotepurchasedaywise.module').then(
                                (m) => m.DebitNotePurchaseDayWiseReportModule
                            ),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/DebitNotePurchaseDetailedReport',
                        loadChildren: () =>
                            import(
                                './Report/DebitNotePurchaseDetailedReport/debitnotepurchasedetailedreport.module'
                            ).then((m) => m.DebitNotePurchaseDetailedReportModule),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/CreditNotePurchaseDetailedReport',
                        loadChildren: () =>
                            import(
                                './Report/CreditNotePurchaseDetailedReport/creditnotepurchasedetailedreport.module'
                            ).then((m) => m.CreditNotePurchaseDetailedReportModule),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    {
                        path: 'Report/CreditNotePurchaseDayWiseReport',
                        loadChildren: () =>
                            import('./Report/CreditNotePurchaseDayWiseReport/creditnotepurchasedaywise.module').then(
                                (m) => m.CreditNotePurchaseDayWiseReportModule
                            ),
                        data: { permission: 'Pages.HeadOfPayment' },
                    },
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    { path: '**', redirectTo: 'dashboard' },
                ],
            },
        ]),
    ],
    exports: [RouterModule],
})
export class MainRoutingModule {}

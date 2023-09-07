import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import {
    GetCountryForViewDto,
    CountryServiceProxy,
    CreditNoteServiceProxy,
    CreateOrEditCreditNoteItemDto,
    CreateOrEditDebitNotePartyDto,
    CreateOrEditSalesInvoiceItemDto,
    CreateOrEditCreditNoteSummaryDto,
    CreateOrEditDebitNoteVATDetailDto,
    CreateOrEditDebitNoteDiscountDto,
    CreateOrEditDebitNotePaymentDetailDto,
    CreateOrEditDebitNoteContactPersonDto,
    DebitNoteAddressDto,
    CreateOrEditSalesInvoiceAddressDto,
    SalesInvoicesServiceProxy,
    CreateOrEditCreditNoteDto,
    CreditNoteAddressDto,
    DebitNotesServiceProxy,
    CreateOrEditDebitNoteDto,
    DebitNotePartiesServiceProxy,
    CreateOrEditDebitNoteItemDto,
    CreateOrEditDebitNoteSummaryDto,
    CreateOrEditDebitNoteAddressDto,
    InvoiceDto,
    TenantBasicDetailsServiceProxy,
    ActivecurrencyServiceProxy,
    CreateOrEditSalesInvoiceContactPersonDto,
    CreateOrEditSalesInvoiceDto,
    CreateOrEditSalesInvoicePartyDto,
    CreateOrEditSalesInvoiceVATDetailDto,
    TenantConfigurationServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { throws } from 'assert';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';

enum Tabs {
    Customer = 1,
    Shipment,
    Additional,
}
@Component({
    templateUrl: './createDebitNoteProf.component.html',
    styleUrls: ['./createDebitNoteProf.component.css'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class CreateDebitNoteProfComponent extends AppComponentBase {
    vatCodeData = [
        {
            name: 'Standard Rated @ 15%',
            value: '15',
            vatcode: 'S'
        },
        {
            name: 'Zero Rated @ 0%',
            value: '0',
            vatcode: 'Z'
        },
        {
            name: 'Out of Scope @ 0%',
            value: '15',
            vatcode: 'O'
        },
        {
            name: 'Exempt @ 0%',
            value: '15',
            vatcode: 'E'
        }
    ];
    exemptionReason: any[] = [];
    exmptReason: string;
    isExempt = false;
    countryname: string;
    currencycode: string;
    buyertype: any[]=[];


    convertedInvoice: CreateOrEditSalesInvoiceDto = new CreateOrEditSalesInvoiceDto();
    vatDetails: CreateOrEditSalesInvoiceVATDetailDto[] = [];
    vatDetail: CreateOrEditSalesInvoiceVATDetailDto = new CreateOrEditSalesInvoiceVATDetailDto();
    public deliveryFormGroup: FormGroup;
    public additionalFormGroup: FormGroup;
    public deliveryDetailFields = [];
    public additionalDetailFields = [];
    reasonfilter: any = {};
    isExport = false;
    takedatafromcus = false;
    //additionalFormGroup: FormGroup<{}>;
    date = new Date();
    addressLang: CreateOrEditSalesInvoiceAddressDto = new CreateOrEditSalesInvoiceAddressDto();
    customerLang: CreateOrEditSalesInvoicePartyDto = new CreateOrEditSalesInvoicePartyDto();

    month =
        (this.date.getMonth() + 1).toString().length > 1 ? this.date.getMonth() + 1 : '0' + (this.date.getMonth() + 1);
    day = this.date.getDate().toString().length > 1 ? this.date.getDate() : '0' + this.date.getDate();
    maxDate = this.date.getFullYear() + '-' + this.month + '-' + this.day;
    editMode = false;
    dateofsupply: DateTime;
    issueDate = new Date().toISOString().slice(0, 10);
    profileType = '';
    discount = 0.0;
    invoice: CreateOrEditDebitNoteDto = new CreateOrEditDebitNoteDto();
    supplier: CreateOrEditDebitNotePartyDto = new CreateOrEditDebitNotePartyDto();
    customer: CreateOrEditDebitNotePartyDto = new CreateOrEditDebitNotePartyDto();
    countries: any[] = [];
    invoiceItems: CreateOrEditDebitNoteItemDto[] = [];
    invoiceItem: CreateOrEditDebitNoteItemDto = new CreateOrEditDebitNoteItemDto();
    invoiceSummary: CreateOrEditDebitNoteSummaryDto = new CreateOrEditDebitNoteSummaryDto();
    discountDetails: CreateOrEditDebitNoteDiscountDto[] = [];
    discountDetail: CreateOrEditDebitNoteDiscountDto = new CreateOrEditDebitNoteDiscountDto();
    paymentDetails: CreateOrEditDebitNotePaymentDetailDto[] = [];
    paymentDetail: CreateOrEditDebitNotePaymentDetailDto = new CreateOrEditDebitNotePaymentDetailDto();
    address: CreateOrEditDebitNoteAddressDto = new CreateOrEditDebitNoteAddressDto();
    isSaving: boolean;
    billingReferenceIds: string[] = [];
    irnNos: string[] = [];
    basicForm: FormGroup;
    alpha3code: string;
    prchaseEntryDate: any;
    editaddressMode: boolean;
    multilanguage= false;
    showBack: boolean = false;
    showNext: boolean = false;
    availableTabs: Tabs[] = [Tabs.Customer];
    public deliveryLangFormGroup: FormGroup;
    public deliveryLangDetailFields = [];
    public multilangform: FormGroup;
    deliveryDetailsModel: any = null;
    additionalDetailsModel: any = null;
    exchange: any[] = [];



    //salesInvoice: GetInvoiceDto = new GetInvoiceDto();
    //--------------------brady-----------------------------
    header_Additional1: any = {
        exchangeRate: 1.0,
    };
    currentStep = 1;
    delivery_customer: CreateOrEditSalesInvoicePartyDto = new CreateOrEditSalesInvoicePartyDto();
    additional_info: any = {};
    summary_Additional1: any = {
        val1: 0.0,
        val2: 0.0,
        totalOther: 0.0,
    };
    activeCurrency: any[] = [];
    defaultcurrency: any[] = [];
    isAutoCompleteDisabled = false;
    filterdcountries: any[] = [];
    filterdcurrencies: any[] = [];
    filterdname: any[] = [];
    filterdcurrency: any[] = [];
    filterdalpha3code: any[] = [];
    language: number = 1;
    //--------------------brady-----------------------------
    constructor(
        injector: Injector,
        private fb: FormBuilder,
        //private _customerServiceProxy: CustomerServiceProxy,
        //private _productsServiceProxy: ProductsServiceProxy,
        private _CreditNoteServiceProxy: DebitNotesServiceProxy,
        //private _invoiceHeaderServiceProxy: InvoiceHeadersServiceProxy,
        private _salesInvoiceServiceProxy: SalesInvoicesServiceProxy,
        private _activeCurrency: ActivecurrencyServiceProxy,
        private _tenantbasicdetailsServiceProxy: TenantBasicDetailsServiceProxy,
        private _notifyService: NotifyService,
        private _masterCountriesServiceProxy: CountryServiceProxy,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _sessionService: AppSessionService,
        private _tenantConfigurations: TenantConfigurationServiceProxy,

        private _router: Router
    ) {
        super(injector);
        this.debitForm();
    }

    debitForm() {
        this.basicForm = this.fb.group({
            Name: ['', Validators.required],
            Buildno: [''],
            street: [''],
            Neighbourhood: [''],
            pin: [''],
            city: [''],
            state: [''],
            nationality: ['', Validators.required],
            ContactNo: ['', Validators.pattern('^[0-9]*$')],
            Email: ['', [Validators.email]],
            vatid: ['', []],
            //--------------------brady-----------------------------
            billToAttn: [''], // customer.contactPerson.name
            exchangeRate: [1.0, Validators.required], // header_Additional1.exchangeRate
            //-------------------------------------------------------
        });
        this.multilangform = this.fb.group({
            Name: ['', Validators.required],
            Buildno: [''],
            CustomerId:[''],
            street: [''],
            Neighbourhood: [''],
            additionalstreet:[''],
            pin: [''],
            city: [''],
            state: [''],
            nationality: ['', Validators.required],
            ContactNo: ['', Validators.pattern('^[0-9]*$')],
            Email: ['', [Validators.email]],
            vatid: ['', []],
            //--------------------brady-----------------------------
            billToAttn: [''], // customer.contactPerson.name
            exchangeRate: [1.0, Validators.required], // header_Additional1.exchangeRate
            buyertype:['']
            //-------------------------------------------------------
        });
    }

    isFormValid() {
        return this.basicForm.valid;
    }

    focusVatPerInput(): void {
        setTimeout(() => {
            document.getElementById('itemVATPercent').focus();
        }, 100);
    }
    //ngoninit

    ngOnInit(): void {
        // this._tenantConfigurations.getTenantConfigurationByTransactionType('Debit Note').subscribe((e) => {
        //     if (e.tenantConfiguration.shipmentJson) {
        //         this.deliveryDetailsModel = JSON.parse(e.tenantConfiguration.shipmentJson);
        //         this.deliveryFormGroup = this.buildForm(this.deliveryDetailsModel, this.deliveryDetailFields);
        //         this.deliveryLangFormGroup = this.buildForm(this.deliveryDetailsModel, this.deliveryLangDetailFields);
        //         this.availableTabs.push(Tabs.Shipment);
        //         this.showNext = true;
        //     }

        //     if (e.tenantConfiguration.additionalFieldsJson) {
        //         this.additionalDetailsModel = JSON.parse(e.tenantConfiguration.additionalFieldsJson);
        //         this.additionalFormGroup = this.buildForm(this.additionalDetailsModel, this.additionalDetailFields);
        //         this.availableTabs.push(Tabs.Additional);
        //         this.showNext = true;
        //     }
        // });

        // this._tenantConfigurations.getTenantConfigurationByTransactionType('General').subscribe((e) => {
        //     if(e.tenantConfiguration.language)
        //     {
        //         if(e.tenantConfiguration.language == 'dual')
        //         {
        //             this.multilanguage=true;
        //         }
        //     }
        // });
        this.address.countryCode = null;
        this.getCountriesDropdown();
        this.getbuyertype();
        this.create();
        this.getProducts();
        this.onSelectCustomer();
        this.invoiceSummary.totalAmountWithVAT = 0;
        this.invoiceSummary.totalAmountWithoutVAT = 0;
        this.invoiceSummary.sumOfInvoiceLineNetAmount = 0;
        this.invoiceSummary.netInvoiceAmount = 0;
        this.invoiceSummary.totalVATAmount = 0;
        this.invoiceItem.vatCode = 'S';
        this.discount = 0;
        this.invoiceItem.quantity = 0;
        this.invoiceItem.discountPercentage = 0;
        this.invoiceItem.uom = 'PC';
        this.invoiceItem.vatRate = 15;
        this.supplier.registrationName = this._sessionService.tenant.name;
        this.supplier.contactPerson.email = this._sessionService.user.emailAddress;
        this.invoice.customerId = '567';
        this.invoiceSummary.sumOfInvoiceLineNetAmountCurrency = 'SAR';
        this.invoice.invoiceCurrencyCode = 'SAR';
    }
    SetExchangeRate(currencyCode: any) {
        if (currencyCode == 'SAR') {
            this.header_Additional1.exchangeRate = 1.0;
        } else if (currencyCode == 'USD') {
            this.header_Additional1.exchangeRate = 3.75;
        } else {
            this.header_Additional1.exchangeRate = 1.0;
        }
    }

    changeStep(step: Tabs) {
        this.language=1;
        this.currentStep=step
        let changeIndex = this.availableTabs.findIndex((a) => a == step);
        if (changeIndex == this.availableTabs.length - 1) {
            this.showNext = false;
            this.showBack = true;
        } else {
            this.showNext = true;
            this.showBack = false;
        }

        if (changeIndex == 0) {
            this.showNext = true;
            this.showBack = false;
        } else {
            this.showNext = false;
            this.showBack = true;
        }
    }

    changeLanguage(lan: number) {
        this.language = lan;
      }

    nextStep() {
        let curIndex = this.availableTabs.findIndex((a) => a == this.currentStep);

        if (curIndex <= this.availableTabs.length - 2) this.currentStep = this.availableTabs[curIndex + 1];

        if (curIndex == this.availableTabs.length - 2) {
            this.showNext = false;
            this.showBack = true;
        } else {
            this.showNext = true;
            this.showBack = false;
        }
    }

    previousStep() {
        let curIndex = this.availableTabs.findIndex((a) => a == this.currentStep);

        if (curIndex >= 1) this.currentStep = this.availableTabs[curIndex - 1];

        if (curIndex == 1) {
            this.showNext = true;
            this.showBack = false;
        } else {
            this.showNext = false;
            this.showBack = true;
        }
    }

    checkIfEmpty(val: any) {
        if (val === '') {
            return 0;
        } else {
            return val;
        }
    }

    calculateTotalOtherCharge() {
        this.summary_Additional1.totalOther =
            (this.checkIfEmpty(this.summary_Additional1.val1) + this.checkIfEmpty(this.summary_Additional1.val2)) *
            this.header_Additional1.exchangeRate;
    }
    getActiveCurrency(alpha3Code: any) {
        this.alpha3code = alpha3Code;
        this._activeCurrency.getActiveCurrencies(alpha3Code).subscribe((e) => {
            this.activeCurrency = e;
            this.defaultcurrency = this.activeCurrency.filter((p) => p.entity == alpha3Code);
            if (this.defaultcurrency.length != 0 && this.currencycode ==  undefined) {
                this.invoice.invoiceCurrencyCode = this.defaultcurrency[0].alphabeticCode;
                this.SetExchangeRate(this.invoice.invoiceCurrencyCode);
                this.currencycode = this.defaultcurrency[0].alphabeticCode;
            }
        });
    
        this.countrychanges();
    }
    onExchangeRateChange() {
        if (this.checkIfEmpty(this.header_Additional1.exchangeRate) == 0) {
            this.header_Additional1.exchangeRate = 1;
            this.notify.error('Exchange rate cannot be 0');
        }
        this.calculateInvoiceSummary(this.invoiceSummary, this.invoiceItems);
    }
    countrychanges() {
        if (this.basicForm.get('nationality').value === 'SA' || this.address.countryCode === 'SA') {
            this.isExport = false;
            // for setting validations
            this.basicForm
                .get('vatid')
                .addValidators([Validators.minLength(15), Validators.maxLength(15), Validators.pattern('^3[0-9]*3$')]);
            this.basicForm.get('state').addValidators([Validators.required, Validators.pattern('[a-zA-Z][a-zA-Z ]+')]);
            this.basicForm.get('street').addValidators([Validators.required]);
            this.basicForm.get('city').addValidators([Validators.required, Validators.pattern('[a-zA-Z][a-zA-Z ]+')]);
            this.basicForm.get('Buildno').addValidators([Validators.pattern('^[0-9]*$'), Validators.required]);
            this.basicForm
                .get('pin')
                .addValidators([
                    Validators.minLength(5),
                    Validators.maxLength(5),
                    Validators.pattern('^[0-9]*$'),
                    Validators.required,
                ]);
        }
        if (this.basicForm.get('nationality').value !== 'SA' && this.address.countryCode !== 'SA') {
            this.isExport = true;
            // for clearing validations
            this.basicForm.get('vatid').setValidators([]);
            this.basicForm.get('pin').setValidators([Validators.pattern('^[0-9]*$')]);
            this.basicForm.get('Buildno').setValidators([Validators.pattern('^[0-9]*$')]);
            this.basicForm.get('state').setValidators([Validators.pattern('[a-zA-Z][a-zA-Z ]+')]);
            this.basicForm.get('city').setValidators([Validators.pattern('[a-zA-Z][a-zA-Z ]+')]);
            //this.basicForm.get('street').setValidators([Validators.pattern('[a-zA-Z][a-zA-Z ]+')]);
        }
        this.basicForm.get('vatid').updateValueAndValidity();
        this.basicForm.get('pin').updateValueAndValidity();
        this.basicForm.get('Buildno').updateValueAndValidity();
        this.basicForm.get('state').updateValueAndValidity();
        this.basicForm.get('city').updateValueAndValidity();
        this.basicForm.get('street').updateValueAndValidity();
    }

    //--------------------brady----------------------------
    //--------------------------------------auto complete starts---------------------------------------------------------

    filterCustomers(event): void {
        //   this._customerServiceProxy.getCustomers(event.query).subscribe((data) => {
        //     this.filteredCustomers = data;
        // });
    }

    onSelectCustomer() {
        this._tenantbasicdetailsServiceProxy.getTenantById(this._sessionService.tenant.id).subscribe((data) => {
            //supplier
            this.supplier.address.city = data[0].city;
            this.supplier.address.state = data[0].state;
            this.supplier.address.countryCode = data[0].country;
            this.supplier.address.postalCode = data[0].postalCode;
            this.supplier.address.additionalNo = data[0].additionalNo;
            this.supplier.address.additionalStreet = data[0].additionalStreet;
            this.supplier.address.buildingNo = data[0].buildingNo;
            this.supplier.address.neighbourhood = data[0].neighbourhood;
            this.supplier.address.street = data[0].street;
            this.supplier.vatid = data[0].vatid;
            this.supplier.crNumber = data[0].crNumber;
            this.supplier.contactPerson.contactNumber = data[0].contactNumber;
        });
    }
    changedata(event) {
        // this.customer.registrationName=event.registrationName
    }
    //--------------------------------------auto complete ends---------------------------------------------------------
    //get countries from master data
    getCountriesDropdown() {
        this._masterCountriesServiceProxy.getCountriesList().subscribe((result) => {
            this.countries = result;
        });
    }
    getProducts(): void {
        // this._productsServiceProxy
        //   .getAll("","","","","","","","",undefined,undefined,undefined,undefined,"",undefined,undefined,"",undefined,undefined,undefined
        //   )
        //   .subscribe((result) => {
        //    this.products = result.items;
        //   });
    }

    updateProduct(i: number) {
        //  this.product = this.filteredProducts[i].product;
        //  this.invoiceItem.name = this.product.name;
        //   this.invoiceItem.unitPrice = this.product.unitPrice;
        //   this.invoiceItem.costPrice = this.product.costPrice;
        //   this.invoiceItem.description = this.product.description;
        //   this.invoiceItem.uom = this.product.uom;
    }

    filterProduct() {
        // let name = this.product.name
        // console.log(name,this.products);
        // this.filteredProducts = JSON.parse(JSON.stringify(this.products)).filter(function (str) {
        //   return str.product.name.indexOf(name||"") === -1; });
        //   console.log(this.filteredProducts);
    }

    create() {
        this.customer.contactPerson = new CreateOrEditDebitNoteContactPersonDto();
        this.supplier.contactPerson = new CreateOrEditDebitNoteContactPersonDto();
        this.supplier.address = new CreateOrEditDebitNoteAddressDto();
        // this.invoiceItem.
        //--------------------brady-----------------------------

        this.delivery_customer.contactPerson = new CreateOrEditSalesInvoiceContactPersonDto();
        this.delivery_customer.address = new CreateOrEditSalesInvoiceAddressDto();
        //--------------------brady-----------------------------
    }

    //add item to invoiceitems
    addItem() {
        this.invoiceItem.quantity=1;
        if (!this.address.countryCode) {
            this.notify.error(this.l('Please select a country.'));
            this.isSaving = false;
            return;
        }
        if (this.invoiceItem.quantity < 0 || this.invoiceItem.quantity == 0) {
            this.message.warn('Quantity cannot be less than or equal to 0 ');
            return;
        }
        if (this.invoiceItem.unitPrice < 0 || this.invoiceItem.unitPrice == 0) {
            this.message.warn('Rate cannot be less than or equal to 0 ');
            return;
        }
        if (this.invoiceItem.discountPercentage < 0 || this.invoiceItem.discountPercentage > 100) {
            this.message.warn('Discount must be in between 0 to 100');
            return;
        }
        if (
            this.invoiceItem.name == null ||
            this.invoiceItem.description == null ||
            this.invoiceItem.quantity == null ||
            this.invoiceItem.quantity == null ||
            this.invoiceItem.unitPrice == null ||
            this.invoiceItem.discountPercentage == null
        ) {
            this.notify.error(this.l('Please fill all required fields to add item to invoice.'));
            return;
        }
        // if (this.invoiceItem.vatRate !== 15 && this.invoiceItem.vatRate !== 0) {
        //     this.notify.error(this.l('VAT rate error'));
        //     return;
        // }

        // if (this.address.countryCode !== 'SA' && this.invoiceItem.vatRate !== 0) {
        //     this.notify.error(this.l('Exports are exempt from VAT'));
        //     return;
        // }

        this.invoiceItem.discountAmount =
            (this.invoiceItem.quantity * this.invoiceItem.unitPrice * this.invoiceItem.discountPercentage) / 100.0;
        this.invoiceItem.vatAmount =
            ((this.invoiceItem.quantity * this.invoiceItem.unitPrice - this.invoiceItem.discountAmount) *
                this.invoiceItem.vatRate) /
            100.0;
        this.invoiceItem.lineAmountInclusiveVAT =
            this.invoiceItem.quantity * this.invoiceItem.unitPrice -
            this.invoiceItem.discountAmount +
            this.invoiceItem.vatAmount;
        this.invoiceItem.netPrice = this.invoiceItem.lineAmountInclusiveVAT - this.invoiceItem.vatAmount;
        this.invoiceItem.currencyCode = 'SAR';
        this.invoiceItem.identifier = 'Debit';
        // if (this.invoiceItem.vatCode !== 'O') {
        //     if (this.vatDetail.excemptionReasonCode === undefined ||
        //         this.vatDetail.excemptionReasonText === undefined ||
        //         this.vatDetail.excemptionReasonCode === null ||
        //         this.vatDetail.excemptionReasonText === null) {
        //         this.notify.error(this.l('Please select a exemption reason.'));
        //         return;
        //     }
        // }
        this.invoiceItem.excemptionReasonCode = this.vatDetail.excemptionReasonCode;
        this.invoiceItem.excemptionReasonText = this.vatDetail.excemptionReasonText;
        this.invoiceItems.push(this.invoiceItem);
        this.invoiceItem = new CreateOrEditSalesInvoiceItemDto();
        this.calculateInvoiceSummary(this.invoiceSummary, this.invoiceItems);
        this.invoiceItem.vatCode = 'S';
        this.invoiceItem.uom = 'PC';
        this.invoiceItem.quantity = 0;
        this.invoiceItem.discountPercentage = 0;
        this.invoiceItem.vatRate = 15;
        this.isExempt = false;
        this.exmptReason=null;

    }

    deleteItem(index: number) {
        this.invoiceItems.splice(index, 1);
        this.vatDetail = new CreateOrEditSalesInvoiceVATDetailDto();
        this.calculateInvoiceSummary(this.invoiceSummary, this.invoiceItems);
    }
    editItem(index: number) {
        this.invoiceItem = this.invoiceItems[index];
        this.onVatChange();
        this.invoiceItems.splice(index, 1);
        this.calculateInvoiceSummary(this.invoiceSummary, this.invoiceItems);
    }

    clearItem() {
        this.invoiceItem = new CreateOrEditSalesInvoiceItemDto();
        this.invoiceItem.vatCode = 'S';
        this.invoiceItem.uom = 'PC';
        this.onVatChange();
        this.invoiceItem.discountPercentage = 0;
    }

    calculateInvoiceSummary(summary, items) {
        summary.totalAmountWithVAT = 0;
        summary.totalAmountWithoutVAT = 0;
        summary.sumOfInvoiceLineNetAmount = 0;
        summary.netInvoiceAmount = 0;
        summary.totalVATAmount = 0;
        this.discount = 0;

        items.forEach((invoiceItem) => {
            summary.totalAmountWithVAT += invoiceItem.lineAmountInclusiveVAT;
            summary.totalAmountWithoutVAT += invoiceItem.quantity * invoiceItem.unitPrice - invoiceItem.discountAmount;
            summary.sumOfInvoiceLineNetAmount += invoiceItem.netPrice;
            summary.netInvoiceAmount += invoiceItem.quantity * invoiceItem.unitPrice;
            this.discount += invoiceItem.discountAmount;
            summary.totalVATAmount = summary.totalAmountWithVAT - summary.totalAmountWithoutVAT;
        });
    }

    fillDummy() {
        this.invoice.status = 'Paid';
        this.invoice.paymentType = 'Cash';
        if (this.dateofsupply === undefined || this.dateofsupply == null) {
            this.invoice.dateOfSupply = this.invoice.issueDate;
        } else {
            this.invoice.dateOfSupply = this.dateofsupply;
        }
        this.invoice.invoiceNumber = '-1';
        this.invoice.invoiceCurrencyCode = 'SAR';
        //this.invoice.buyer.crNumber = '1234567890';
        this.invoice.invoiceSummary.currencyCode = 'SAR';
        this.invoice.invoiceSummary.paidAmountCurrency = 'SAR';
        this.invoice.invoiceSummary.payableAmountCurrency = 'SAR';
        this.invoice.invoiceSummary.netInvoiceAmountCurrency = 'SAR';
        this.invoice.invoiceSummary.totalAmountWithoutVATCurrency = 'SAR';


        // this.invoice.buyer.contactPerson.name = !this.customer.registrationName?.trim()
        //     ? 'NA'
        //     : this.customer.registrationName;
        // this.invoice.buyer.contactPerson.type = !this.customer.contactPerson.type?.trim()
        //     ? 'NA'
        //     : this.customer.contactPerson.type;
        // this.invoice.buyer.address.type = !this.address.type?.trim() ? 'NA' : this.address.type;
        // this.invoice.buyer.address.additionalNo = !this.address.additionalNo?.trim() ? 'NA' : this.address.additionalNo;
        // this.invoice.buyer.address.city = !this.invoice.buyer.address.city?.trim()
        //     ? 'NA'
        //     : this.invoice.buyer.address.city;
        // this.invoice.buyer.address.countryCode = !this.invoice.buyer.address.countryCode?.trim()
        //     ? 'NA'
        //     : this.invoice.buyer.address.countryCode;
        // this.invoice.buyer.address.postalCode = !this.invoice.buyer.address.postalCode?.trim()
        //     ? 'NA'
        //     : this.invoice.buyer.address.postalCode;
        // this.invoice.buyer.address.state = !this.invoice.buyer.address.state?.trim()
        //     ? 'NA'
        //     : this.invoice.buyer.address.state;
        // this.invoice.buyer.address.street = !this.invoice.buyer.address.street?.trim()
        //     ? 'NA'
        //     : this.invoice.buyer.address.street;
        // this.invoice.buyer.address.state = !this.invoice.buyer.address.state?.trim()
        //     ? 'NA'
        //     : this.invoice.buyer.address.state;
        // this.invoice.buyer.address.buildingNo = !this.invoice.buyer.address.buildingNo?.trim()
        //     ? 'NA'
        //     : this.invoice.buyer.address.buildingNo;
        // this.invoice.buyer.address.neighbourhood = !this.invoice.buyer.address.neighbourhood?.trim()
        //     ? 'NA'
        //     : this.invoice.buyer.address.neighbourhood;
        // this.invoice.buyer.contactPerson.email = !this.customer.contactPerson.email?.trim()
        //     ? 'NA'
        //     : this.customer.contactPerson.email;

        this.invoice.items.forEach((item) => {
            item.id = null;
        });
    }

    parseDate(dateString: string): DateTime {
        let time = new Date().toLocaleTimeString();
        let date = new Date(dateString);

        if (time.includes('PM')) {
            date.setHours(parseInt(time.split(':')[0]) + 12);
        } else {
            date.setHours(parseInt(time.split(':')[0]));
        }
        date.setMinutes(parseInt(time.split(':')[1]));
        date.setSeconds(parseInt(time.split(':')[2]));
        dateString = date.toISOString();

        if (dateString) {
            return DateTime.fromISO(new Date(dateString).toISOString());
        }
        return null;
    }

    //--------------------------------------auto complete starts---------------------------------------------------------

    filterInvoices(event): void {
        this._salesInvoiceServiceProxy.getInvoiceSuggestions(event.query,'').subscribe((data) => {
            if (data == null || data === undefined || data.length === 0) {
                this.invoice.billingReferenceId = event.query;
                this.editaddressMode = false;

            } else {
                this.editaddressMode = true;

                for (let i = 0; i < data.length; i++) {
                    data[i] = data[i].irnNo + ' ' + data[i].issueDate.toString();
                }

                this.billingReferenceIds = data;
            }

            //event.query.split(" ")[0]
        });
    }
    //Get Debit Note Customer Details
    getCustomerInfo(data) {
        if (data.length <= 0) return;
        data.forEach((data)=>{ if (data.language === 'AR' && this.multilanguage) {
          this.multilangform.get('CustomerId').setValue(data.customerId);
          this.multilangform.get('Name').setValue(data.name);
          this.multilangform.get('vatid').setValue(data.vatid);
          this.multilangform.get('Buildno').setValue(data.buildingNo);
          this.multilangform.get('street').setValue(data.street);
          this.multilangform.get('additionalstreet').setValue(data.additionalStreet);
          this.multilangform.get('Neighbourhood').setValue(data.neighbourhood);
          this.multilangform.get('city').setValue(data.city?.trim());
          this.multilangform.get('state').setValue(data.state);
          this.multilangform.get('pin').setValue(data.postalCode);
          this.multilangform.get('nationality').setValue(data.countryCode);
          this.multilangform.get('ContactNo').setValue(data.contactNumber);
          this.multilangform.get('Email').setValue(data.email);
            this.multilangform.get('billToAttn').setValue(data.billToAttn);
            this.multilangform.get('buyertype').setValue(data.otherDocumentTypeId);
      } else {
          this.invoice.billingReferenceId = data.irnNo;
          this.invoice.invoiceNumber = data.billingReferenceId;
          this.invoice.invoiceNotes = data.invoiceNotes;
          this.invoice.invoiceCurrencyCode = data.invoiceCurrencyCode;
          this.currencycode = data.invoiceCurrencyCode;
          this.dateofsupply = data.issueDate;
          this.prchaseEntryDate = data.issueDate;
          this.customer.contactPerson.contactNumber = data.contactNumber;
          this.customer.vatid = data.vatid;
          this.customer.customerId = data.customerId;
          this.customer.registrationName = data.name;
          this.address.buildingNo = data.buildingNo;
          this.address.street = data.street?.trim();
          this.address.additionalStreet = data.additionalStreet?.trim();
          this.address.neighbourhood = data.neighbourhood?.trim();
          this.address.city = data.city?.trim();
          this.address.state = data.state;
          this.address.countryCode = data.countryCode;
          this.address.postalCode = data.postalCode;
          this.customer.contactPerson.email = data.email;
          this.customer.otherID = data.otherDocumentTypeId;
          this.exchange = JSON.parse(data.exchange);
          this.header_Additional1.exchangeRate = this.exchange[0].exchangeRate;
          this.basicForm.get('exchangeRate').setValue(this.exchange[0].exchangeRate);
          // this.customer.contactPerson.name = data[0].billToAttn;
          // this.getActiveCurrency(data[0].countryCode);
          this.countryname = this.countries.find((p) => p.alphaCode == data.countryCode)?.name;
          this.selectOption(this.countryname);
      }});
           
      }
    //Get Shipment and Delivery Details
    getShipmentInfo(data) {
        this.deliveryFormGroup.get('registrationName').setValue(data[0].registrationName);
        this.deliveryFormGroup.get('crNumber').setValue(data[0].crNumber);
        this.deliveryFormGroup.get('vatid').setValue(data[0].vatnumber);
        this.deliveryFormGroup.get('address.buildingNo').setValue(data[0].add_buildingNo);
        this.deliveryFormGroup.get('address.street').setValue(data[0].add_street);
        this.deliveryFormGroup.get('address.additionalStreet').setValue(this.address.additionalStreet);
        this.deliveryFormGroup.get('address.city').setValue(data[0].add_city);
        this.deliveryFormGroup.get('address.additionalNo').setValue(data[0].add_additionalNo);
        this.deliveryFormGroup.get('address.neighbourhood').setValue(data[0].add_neighbourhood);
        this.deliveryFormGroup.get('address.state').setValue(data[0].add_state);
        this.deliveryFormGroup.get('address.postalCode').setValue(data[0].add_postalCode);
        this.deliveryFormGroup.get('address.countryCode').setValue(data[0].add_countryCode);
        this.deliveryFormGroup.get('contactPerson.name').setValue(data[0].contact_name);
        this.deliveryFormGroup.get('contactPerson.contactNumber').setValue(data[0].contactNumber);
    }
    //Additional Information
    getAdditionalInfo(data) {
        this.additionalFormGroup.get('invoiceRefDate').setValue(data[0].additional_invoiceRefDate);
        this.additionalFormGroup.get('deliveyDate').setValue(data[0].additional_deliveyDate);
        this.additionalFormGroup.get('dueDate').setValue(data[0].additional_dueDate);
        this.additionalFormGroup.get('purchaseOrderNo').setValue(data[0].additional_purchaseOrderNo);
        this.additionalFormGroup.get('originalOrderNo').setValue(data[0].additional_originalOrderNo);
        this.additionalFormGroup.get('paymentMeans').setValue(data[0].additional_paymentMeans);
        this.additionalFormGroup.get('carrier').setValue(data[0].additional_carrier);
        this.additionalFormGroup.get('deliveryDocumentNo').setValue(data[0].additional_deliveryDocumentNo);
        this.additionalFormGroup.get('termsOfDelivery').setValue(data[0].additional_termsOfDelivery);
        this.additionalFormGroup.get('vendorNo').setValue(data[0].additional_vendorNo);
        this.additionalFormGroup.get('originalQuoteNum').setValue(data[0].additional_originalQuoteNum);
        this.additionalFormGroup.get('paymentTerms').setValue(data[0].additional_paymentTerms);
        this.additionalFormGroup.get('orderPlacedBy').setValue(data[0].additional_orderPlacedBy);
        this.additionalFormGroup.get('deliveryTermsDescription').setValue(data[0].additional_deliveryTermsDescription);
        this.additionalFormGroup.get('bankInformation').setValue(data[0].additional_bankInformation);
    }

    onSelectInvoice(event): void {
        this._salesInvoiceServiceProxy.getsalesdetails(event.split(' ')[0],undefined).subscribe((data) => {
            this.getCustomerInfo(data);
           // this.getShipmentInfo(data);
            //this.getAdditionalInfo(data);
        });
    }
    checkautocomplete(){
        if(this.invoiceItems.length > 0 && (this.address.countryCode != null && this.address.countryCode != undefined))
        {
            this.notify.error('Cannot Change Country After Adding Item');
            this.isAutoCompleteDisabled=true;
        }
    }
    filterOptions(event) {
        this.filterdname = [];
        this.filterdcountries = this.countries.filter((p) => p.name.toLowerCase().includes(event.query.toLowerCase()));
        if (this.filterdcountries.length != 0) {
            for (let i = 0; i < this.filterdcountries.length; i++) {
                this.filterdname.push(this.filterdcountries[i].name);
            }
        } else {
            this.notify.error('Please Enter Valid Country Name');
            this.filterdname = [];
            this.address.countryCode = '';
            this.invoice.invoiceCurrencyCode = '';
            this.countryname = '';
            return;
        }
    }
    filterCustomerCountry(countryCode){
        this.filterdname = [];
        this.filterdcountries = this.countries.filter((p) => p.alphaCode.toLowerCase().includes(countryCode.toLowerCase()));
        if (this.filterdcountries.length != 0) {
            for (let i = 0; i < this.filterdcountries.length; i++) {
                this.filterdname.push(this.filterdcountries[i].name);
            }}
    }
    getdefault() {
        this.filterdcurrency = [];
        this.filterdcurrencies = this.activeCurrency.filter(
            (p) => p.entity.toLowerCase() == this.address.countryCode.toLowerCase()
        );
        if (this.filterdcurrencies.length != 0) {
            for (let i = 0; i < this.filterdcurrencies.length; i++) {
                this.filterdcurrency.push(this.filterdcurrencies[i].alphabeticCode);
            }
        }
    }
    filterCurrencyOptions(event) {
        this.filterdcurrency = [];
        this.filterdcurrencies = this.activeCurrency.filter((p) =>
            p.alphabeticCode.toLowerCase().includes(event.query.toLowerCase())
        );
        if (this.filterdcurrencies.length != 0) {
            for (let i = 0; i < this.filterdcurrencies.length; i++) {
                this.filterdcurrency.push(this.filterdcurrencies[i].alphabeticCode);
            }
        } else {
            this.notify.error('Please Enter Valid Currency Code');
            this.filterdcurrency = [];
            this.currencycode='';
        this.invoice.invoiceCurrencyCode = '';
            return;
        }
    }
    selectOption(event) {
        // Perform the desired action with the selected option
        this.filterdalpha3code = this.countries.filter((p) => p.name.toLowerCase() == event.toLowerCase());
        this.getActiveCurrency(this.filterdalpha3code[0].alphaCode);
        this.address.countryCode = this.filterdalpha3code[0].alphaCode;
        this.countrychanges();
    }
    
    selectCurrencyOption(event) {
        // Perform the desired action with the selected option
        //     this.filterdalpha3code = this.countries.filter(p => (p.name).toLowerCase() == ((event).toLowerCase())
        //     );
        //     this.getActiveCurrency(this.filterdalpha3code[0].alphaCode);
        //     this.address.countryCode=this.filterdalpha3code[0].alphaCode;
        //    this.countrychanges();
        this.invoice.invoiceCurrencyCode = event;
        this.SetExchangeRate(event);
    }
    //--------------------------------------auto complete ends---------------------------------------------------------

    ConvertToSAR() {
        let temp = JSON.parse(JSON.stringify(this.invoiceItems));
        this.convertedInvoice.items.forEach((item) => {
            item.unitPrice = item.unitPrice * this.header_Additional1.exchangeRate;
            item.discountAmount = (item.quantity * item.unitPrice * item.discountPercentage) / 100.0;
            item.vatAmount = ((item.quantity * item.unitPrice - item.discountAmount) * item.vatRate) / 100.0;
            item.lineAmountInclusiveVAT = item.quantity * item.unitPrice - item.discountAmount + item.vatAmount;
            item.netPrice = item.lineAmountInclusiveVAT - item.vatAmount;
        });
        this.calculateInvoiceSummary(this.convertedInvoice.invoiceSummary, this.convertedInvoice.items);
        let otherCharges = JSON.parse(this.convertedInvoice.invoiceSummary.additionalData1)[0];
        otherCharges.totalOther = otherCharges.totalOther * this.header_Additional1.exchangeRate;
        otherCharges.val1 = otherCharges.val1 * this.header_Additional1.exchangeRate;
        otherCharges.val2 = otherCharges.val2 * this.header_Additional1.exchangeRate;
        this.convertedInvoice.invoiceSummary.additionalData1 = JSON.stringify([otherCharges]);
        // this.calculateTotalOtherCharge()
    }
    save() {
        //this.delivery_customer = this.deliveryFormGroup.value;
        //this.additional_info = this.additionalFormGroup.value;
        if (this.address.neighbourhood == null || this.address.neighbourhood === undefined) {
            this.basicForm.get('Neighbourhood').setValue('.');
        }
        let isSimplified = this.invoiceSummary.totalAmountWithVAT < 1000;
        // if (parseInt(this.address.buildingNo) > 9999 && !isSimplified) {
        //     this.notify.error(this.l('Building number cannot be greater than 4 digits'));
        //     this.isSaving = false;

        //     return;
        // }
        // if ((parseInt(this.address.postalCode) > 99999 || parseInt(this.address.postalCode) <= 9999) && !isSimplified) {
        //     this.notify.error(this.l('Please enter a valid postal code.'));
        //     this.isSaving = false;

        //     return;
        // }

        if (this.issueDate.toString() > DateTime.now().toString()) {
            this.notify.error(this.l('Invoice Date Should not be greater than Current Date.'));
            this.isSaving = false;
            return;
        }
        // if (this.customer.registrationName==null || this.customer.registrationName==undefined) {
        //     this.notify.error(this.l('Please select a registered customer.'));
        //     this.isSaving = false;

        //     return;
        // }
        if (!this.address.countryCode) {
            this.notify.error(this.l('Please select a country.'));
            this.isSaving = false;
            return;
        }
        if (this.invoiceItems.length <= 0) {
            this.notify.error(this.l('Please add at least one item to save.'));
            this.isSaving = false;
            return;
        }
        if (this.isFormValid()) {
            this.invoice.invoiceSummary = this.invoiceSummary;
            this.invoice.items = this.invoiceItems;
            this.customerLang.customerId=this.multilangform.get('CustomerId').value;
            this.customerLang.registrationName=this.multilangform.get('Name').value;
            this.customerLang.vatid=this.multilangform.get('vatid').value;
            this.addressLang.buildingNo= this.multilangform.get('Buildno').value;
            this.addressLang.street=this.multilangform.get('street').value;
            this.addressLang.additionalStreet=this.multilangform.get('additionalstreet').value;
            this.addressLang.city=this.multilangform.get('city').value;
            this.addressLang.state=this.multilangform.get('state').value;
            this.addressLang.countryCode=this.multilangform.get('nationality').value;
            this.addressLang.postalCode = this.multilangform.get('pin').value;
            this.customerLang.language='AR';
            this.addressLang.language='AR';
            this.address.language='EN';
            this.customer.language='EN';
            this.invoice.supplier = [this.supplier];
            if(this.multilanguage==true)
            {
            this.invoice.buyer = [this.customer,this.customerLang];
            this.invoice.buyer[0].address = this.address;
            this.invoice.buyer[1].address = this.addressLang;
            }
            else
            {
              this.invoice.buyer = [this.customer]; 
              this.invoice.buyer[0].address = this.address;
            }
            try {
                this.invoice.issueDate = this.parseDate(this.issueDate.toString());
                if(this.prchaseEntryDate != null || this.prchaseEntryDate!=undefined)
                {
                if (this.invoice.issueDate < this.parseDate(this.prchaseEntryDate.toString())) {
                  if (this.editaddressMode === true)  {

                  this.notify.error(this.l('Debit Note date should not be less than the invoice date.'));
                  this.isSaving = false;
                  return;
                  }
                }
            }
            } catch (e) {
                this.notify.error(this.l('Please enter valid issue date.'));
                this.isSaving = false;
                return;
            }
            this.isSaving = true;
            this.fillDummy();
            //-------------------brady---------------------------------
            this.invoice.invoiceSummary.additionalData1 = JSON.stringify([this.summary_Additional1]);
            this.invoice.additionalData1 = JSON.stringify([this.header_Additional1]);
            this.invoice.additionalData2 = JSON.stringify([this.additional_info]);
            this.invoice.buyer[0].additionalData1 = JSON.stringify([this.delivery_customer]);
            if(this.multilanguage==true)
            {
                this.invoice.buyer[1].additionalData1 = JSON.stringify([this.deliveryLangFormGroup.value]);
                }
            this.basicForm.markAsUntouched();
            //----------------------------------------------------------
            this.invoice.vatDetails = this.vatDetails;
            this.convertedInvoice = JSON.parse(JSON.stringify(this.invoice));
            this.ConvertToSAR();

            this.basicForm.markAsUntouched();
            this._CreditNoteServiceProxy.createDebitNote(this.invoice).subscribe({
            next: (result) => {
                this.notify.success(this.l('SavedSuccessfully'));
                this.editMode = false;
                this.isSaving = false;
                this._router.navigate(['/app/main/transactions'], { state: { tabvaule: 'Debit Note' } });
              },
              error: (err) => {
                 this.isSaving = false;
              },
              complete: () => console.log('complete')
            });
        } else {
            this.basicForm.markAllAsTouched();
            window.scroll(0, 0);
            this.notify.error(this.l('Please fill all the required fields'));
        }
    }
    download(id, uid) {
        //window.location.reload();
        let pdfUrl =
            AppConsts.pdfUrl +
            '/InvoiceFiles/' +
            this._sessionService.tenantId +
            '/' +
            uid +
            '/' +
            uid +
            '_' +
            id +
            '.pdf';
        window.open(pdfUrl);
    }
    refresh() {
        //reload page
        location.reload();
    }
    onVatChange() {
        this.vatDetail.excemptionReasonCode = null;
        this.vatDetail.excemptionReasonText = null;
        if (this.invoiceItem.vatCode === 'S') {
            this.isExempt = false;
        } else {
            this.isExempt = true;
            this.getExemptionReason(this.invoiceItem.vatCode);
        }
    }
    getExemptionReason(vatcode) {
        this._salesInvoiceServiceProxy.getExemptionReason(vatcode).subscribe((e) => {
            this.exemptionReason = e;
            this.reasonfilter=this.exemptionReason.filter((p) => p.name == this.invoiceItem.excemptionReasonCode);
            this.exmptReason=this.reasonfilter[0].id;
            this.vatDetail.excemptionReasonCode=this.invoiceItem.excemptionReasonCode;
            this.vatDetail.excemptionReasonText=this.invoiceItem.excemptionReasonText;
        });
    }

    onReasonChange(event) {
        this.reasonfilter = this.exemptionReason.filter((p) => p.id == this.exmptReason);
        this.vatDetail.excemptionReasonCode = this.reasonfilter[0].name; //this.exemptionReason[parseInt(event.target.value)].name;
        this.vatDetail.excemptionReasonText = this.reasonfilter[0].description; //this.exemptionReason[parseInt(event.target.value)].description;
    }
    checkValue(takedatafromcus) {
        if (takedatafromcus === true) {
            this.deliveryFormGroup.get('registrationName').setValue(this.customer.registrationName);
            this.deliveryFormGroup.get('vatid').setValue(this.customer.vatid);
            this.deliveryFormGroup.get('address.buildingNo').setValue(this.address.buildingNo);
            this.deliveryFormGroup.get('address.additionalNo').setValue(this.address.additionalNo);
            this.deliveryFormGroup.get('address.street').setValue(this.address.street);
            this.deliveryFormGroup.get('address.additionalStreet').setValue(this.address.additionalStreet);
            this.deliveryFormGroup.get('address.city').setValue(this.address.city);
            this.deliveryFormGroup.get('address.neighbourhood').setValue(this.address.neighbourhood);
            this.deliveryFormGroup.get('address.state').setValue(this.address.state);
            this.deliveryFormGroup.get('address.postalCode').setValue(this.address.postalCode);
            this.deliveryFormGroup.get('address.countryCode').setValue(this.address.countryCode);
            this.deliveryFormGroup.get('contactPerson.contactNumber').setValue(this.customer.contactPerson.contactNumber);
        } else {
            this.deliveryFormGroup.get('registrationName').setValue('');
            this.deliveryFormGroup.get('vatid').setValue('');
            this.deliveryFormGroup.get('address.buildingNo').setValue('');
            this.deliveryFormGroup.get('address.additionalNo').setValue('');
            this.deliveryFormGroup.get('address.street').setValue('');
            this.deliveryFormGroup.get('address.additionalStreet').setValue('');
            this.deliveryFormGroup.get('address.city').setValue('');
            this.deliveryFormGroup.get('address.neighbourhood').setValue('');
            this.deliveryFormGroup.get('address.state').setValue('');
            this.deliveryFormGroup.get('address.postalCode').setValue('');
            this.deliveryFormGroup.get('address.countryCode').setValue('');
            this.deliveryFormGroup.get('contactPerson.contactNumber').setValue('');
        }
    }
    //--------------------dynamic form-----------------------------------

    private buildForm(model: any, fields: any[]) {
        const formGroupFields = this.getFormControlsFields(model, fields);
        return new FormGroup(formGroupFields);
    }

    private getFormControlsFields(model: any, fields: any[], formGroupName: string = '') {
        const formGroupFields = {};
        for (const field of Object.keys(model)) {
            if (field === 'isChildGroup') {
                continue;
            }
            const fieldProps = model[field];
            if (fieldProps?.isChildGroup) {
                let children = this.getFormControlsFields(fieldProps, fields, field);
                formGroupFields[field] = new FormGroup(children);
            } else {
                const validators = this.addValidator(fieldProps.rules);
                formGroupFields[field] = new FormControl(fieldProps.value, validators);
                fields.push({ ...fieldProps, fieldName: field, formGroupName: formGroupName });
            }
        }
        return formGroupFields;
    }

    private addValidator(rules) {
        if (!rules) {
            return [];
        }
        const validators = Object.keys(rules).map((rule) => {
            switch (rule) {
                case 'required':
                    return Validators.required;
                //add more case for future.
            }
        });
        return validators;
    }
    //--------------------dynamic form-----------------------------------
    getbuyertype(){
        this._salesInvoiceServiceProxy.getBuyertypelist().subscribe((result) => {
           this.buyertype = result;
       });
   }
}

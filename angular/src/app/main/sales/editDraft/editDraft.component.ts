import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import {
    CustomersesServiceProxy,
    CustomersDto,
    GetCountryForViewDto,
    CreateOrEditSalesInvoiceDto,
    CountryServiceProxy,
    CreditNoteServiceProxy,
    CreateOrEditDraftItemDto,
    CreateOrEditDraftPartyDto,
    CreateOrEditCreditNoteSummaryDto,
    CreateOrEditDraftVATDetailDto,
    CreateOrEditDraftDiscountDto,
    CreateOrEditCreditNotePaymentDetailDto,
    CreateOrEditCreditNoteContactPersonDto,
    CreateOrEditDraftAddressDto,
    CreateOrEditSalesInvoiceAddressDto,
    SalesInvoicesServiceProxy,
    TenantBasicDetailsServiceProxy,
    CreateOrEditSalesInvoicePartyDto,
    ActivecurrencyServiceProxy,
    CreateOrEditSalesInvoiceContactPersonDto,
    TenantConfigurationServiceProxy,
    DraftsServiceProxy,
    CreateOrEditDraftDto,
    CreateOrEditSalesInvoiceItemDto,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { HttpHeaders } from '@angular/common/http';
import { clear } from 'localforage';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';

enum Tabs {
    Customer = 1,
    Shipment,
    Additional,
}

@Component({
    templateUrl: './editDraft.component.html',
    styleUrls: ['./editDraft.component.css'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class EditDraftComponent extends AppComponentBase {
    public deliveryFormGroup: FormGroup;
    public additionalFormGroup: FormGroup;
    public deliveryLangFormGroup: FormGroup;
    public deliveryLangDetailFields = [];
    deliveryDetailsModel: any = null;
    additionalDetailsModel: any = null;
    multilanguage = false;
    weareyourvendor:string;
    id: string;
    shipment: any[] = [];
    othercharges: any[] = [];
    totalcharge: number;
    additionalData: any[] = [];
    exchangeRateVal: number;
    exchangerate: any[] = [];
    type:string;
    theme:string;
    transtype: string;
    // additionalFormGroup: FormGroup<{}>;
    public deliveryDetailFields = [];
    public additionalDetailFields = [];
    deliveryfields = true;
    additionalfields = true;
    takedatafromcus = false;
    filterdalpha3code: any[] = [];
    filterdcurrency: any[] = [];
    currencycode: string;
    countryname: string;
    generateDraft:boolean=false
    countryNameFilter: any[] = [];
    dataType: string;

    vatCodeData = [
        {
            name: 'Standard Rated @ 15%',
            value: '15',
            vatcode: 'S',
        },
        {
            name: 'Zero Rated @ 0%',
            value: '0',
            vatcode: 'Z',
        },
        {
            name: 'Out of Scope @ 0%',
            value: '15',
            vatcode: 'O',
        },
        {
            name: 'Exempt @ 0%',
            value: '15',
            vatcode: 'E',
        },
    ];

    date = new Date();
    month =
        (this.date.getMonth() + 1).toString().length > 1 ? this.date.getMonth() + 1 : '0' + (this.date.getMonth() + 1);
    day = this.date.getDate().toString().length > 1 ? this.date.getDate() : '0' + this.date.getDate();
    maxDate = this.date.getFullYear() + '-' + this.month + '-' + this.day;
    editMode = false;
    editaddressMode = false;
    dateofsupply: DateTime;
    issueDate = new Date().toISOString().slice(0, 10);
    profileType = '';
    discount = 0.0;
    invoice: CreateOrEditDraftDto = new CreateOrEditDraftDto();
    convertedInvoice: CreateOrEditDraftDto = new CreateOrEditDraftDto();
    supplier: CreateOrEditDraftPartyDto = new CreateOrEditDraftPartyDto();
    customer: CreateOrEditDraftPartyDto = new CreateOrEditDraftPartyDto();
    customerLang: CreateOrEditDraftPartyDto = new CreateOrEditDraftPartyDto();
    countries: any[] = [];
    invoiceItems: CreateOrEditDraftItemDto[] = [];
    Items: CreateOrEditDraftItemDto[] = [];
    invoiceItem: CreateOrEditDraftItemDto = new CreateOrEditDraftItemDto();
    invoiceCharges: CreateOrEditDraftItemDto[] = [];
    invoiceCharge: CreateOrEditDraftItemDto = new CreateOrEditDraftItemDto();
    invoiceSummary: CreateOrEditCreditNoteSummaryDto = new CreateOrEditCreditNoteSummaryDto();
    vatDetails: CreateOrEditDraftVATDetailDto[] = [];
    vatDetail: CreateOrEditDraftVATDetailDto = new CreateOrEditDraftVATDetailDto();
    discountDetails: CreateOrEditDraftDiscountDto[] = [];
    discountDetail: CreateOrEditDraftDiscountDto = new CreateOrEditDraftDiscountDto();
    paymentDetails: CreateOrEditCreditNotePaymentDetailDto[] = [];
    paymentDetail: CreateOrEditCreditNotePaymentDetailDto = new CreateOrEditCreditNotePaymentDetailDto();
    address: CreateOrEditDraftAddressDto = new CreateOrEditDraftAddressDto();
    addressLang: CreateOrEditDraftAddressDto = new CreateOrEditDraftAddressDto();
    delivery_customer: CreateOrEditDraftPartyDto = new CreateOrEditDraftPartyDto();
    delivery_customerlang: any = {};
    delivery: CreateOrEditDraftPartyDto = new CreateOrEditDraftPartyDto();
    deliverylang: CreateOrEditDraftPartyDto = new CreateOrEditDraftPartyDto();
    deliveryAddress: CreateOrEditDraftAddressDto = new CreateOrEditDraftAddressDto();
    deliveryAddressLang: CreateOrEditDraftAddressDto = new CreateOrEditDraftAddressDto();
    billingReferenceIds: string[] = [];
    sapRefNos:string[]=[];
    irnNos: string[] = [];
    refNos:string[]=[];
    basicForm: FormGroup;
    public multilangform: FormGroup;
    invoiceCount = 0;
    editItemIndex: number;
    editQuantity: number;
    originalInvoiceItems: CreateOrEditDraftItemDto[];
    isSaving: boolean;
    alpha3code: string;
    editWithoutBillingRefId = false;
    prchaseEntryDate: any;
    showNext = false;
    isIRNo:boolean=false;
    isRefNo:boolean=false;
    showBack = false;
    exchange: any[] = [];
    availableTabs: Tabs[] = [Tabs.Customer];
    isAutoCompleteDisabled = false;
    customers: CustomersDto[] = new Array<CustomersDto>();
    filteredCustomers: any[] = [];
    header_Additional1: any = {
        exchangeRate: 1.0,
    };
    currentStep: Tabs = Tabs.Customer;
    additional_info: any = {};
    activeCurrency: any[] = [];
    defaultcurrency: any[] = [];
    exemptionReason: any[] = [];
    summary_Additional1: any = {
        val1: 0.0,
        val2: 0.0,
        totalOther: 0.0,
    };
    header: string;
    isExempt = false;
    isExport = false;
    enablearabic = true;
    enablecharge = false;
    chargeVATAmount = 0;
    totalChargeVATAmount = 0;
    buyertype: any[] = [];
    reasonfilter: any = {};
    exmptReason: string;
    filterdcountries: any[] = [];
    filterdcurrencies: any[] = [];
    filterdname: any[] = [];
    language = 1;
    constructor(
        injector: Injector,
        private fb: FormBuilder,
        private _CreditNoteServiceProxy: CreditNoteServiceProxy,
        private _salesInvoiceServiceProxy: SalesInvoicesServiceProxy,
        private _draftInvoiceServiceProxy: DraftsServiceProxy,
        private _activeCurrency: ActivecurrencyServiceProxy,
        private _masterCountriesServiceProxy: CountryServiceProxy,
        private _tenantbasicdetailsServiceProxy: TenantBasicDetailsServiceProxy,
        private _notifyService: NotifyService,
        private _draftServiceProxy: DraftsServiceProxy,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _sessionService: AppSessionService,
        private _tenantConfigurations: TenantConfigurationServiceProxy,
        private route: ActivatedRoute,
        private router: Router,
        private _customerServiceProxy: CustomersesServiceProxy
    ) {
        super(injector);
        this.CreditForm();
    }
    CreditForm() {
        this.basicForm = this.fb.group({
            Name: ['', Validators.required],
            CustomerId: [''],
            Buildno: [''],
            street: [''],
            Neighbourhood: [''],
            pin: [''],
            city: [''],
            state: [''],
            referenceNo:[''],
            nationality: ['', Validators.required],
            Email: ['', [Validators.email]],
            ContactNo: ['', [Validators.pattern('^[0-9]*$')]],
            vatid: [''],
            //--------------------brady-----------------------------
            billToAttn: [''], // customer.contactPerson.name
            exchangeRate: [Validators.required], // header_Additional1.exchangeRate
            currency: [Validators.required],
            //-------------------------------------------------------
        });
        this.multilangform = this.fb.group({
            Name: ['', Validators.required],
            Buildno: [''],
            CustomerId: [''],
            street: [''],
            Neighbourhood: [''],
            additionalstreet: [''],
            pin: [''],
            city: [''],
            state: [''],
            nationality: ['', Validators.required],
            ContactNo: ['', Validators.pattern('^[0-9]*$')],
            Email: ['', [Validators.email]],
            vatid: ['', []],
            buyertype: [''],
            //--------------------brady-----------------------------
            billToAttn: [''], // customer.contactPerson.name
            exchangeRate: [Validators.required], // header_Additional1.exchangeRate
            currency: [Validators.required], // header_Additional1.exchangeRate
            //-------------------------------------------------------
        });
    }
    isFormValid() {
        this.deliveryfields = this.availableTabs.find((a) => a == Tabs.Shipment) ? this.deliveryFormGroup?.valid : true;
        this.additionalfields = this.availableTabs.find((a) => a == Tabs.Additional)
            ? this.additionalFormGroup?.valid
            : true;
        console.log(this.basicForm,'s');
        return this.basicForm.valid && this.deliveryfields && this.additionalfields;
    }
    SetExchangeRate(currencyCode: any) {
        if (currencyCode === 'SAR') {
            this.header_Additional1.exchangeRate = 1.0;
        } else if (currencyCode === 'USD') {
            this.header_Additional1.exchangeRate = 3.75;
        } else {
            this.header_Additional1.exchangeRate = 1.0;
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

            if (this.defaultcurrency.length != 0) {
                if (this.invoice.invoiceCurrencyCode == null || this.invoice.invoiceCurrencyCode == null) {
                    this.invoice.invoiceCurrencyCode = this.defaultcurrency[0].alphabeticCode;
                    this.SetExchangeRate(this.invoice.invoiceCurrencyCode);
                }
            }
        });
        this.countrychanges();
    }

    onExchangeRateChange() {
        if (this.checkIfEmpty(this.header_Additional1.exchangeRate) == 0) {
            this.header_Additional1.exchangeRate = 1;
            this.notify.error('Exchange rate cannot be 0');
        }
        this.invoiceItems.forEach((i) => {
            i.lineAmountInclusiveVAT =
                (i.quantity * i.unitPrice - i.discountAmount + i.vatAmount) * this.header_Additional1.exchangeRate;
        });

        this.calculateInvoiceSummary(this.invoiceSummary, this.invoiceItems);
        this.calculateTotalOtherCharge();
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
    filterCustomerCountry(countryCode) {
        this.filterdname = [];
        this.filterdcountries = this.countries.filter((p) =>
            p.alphaCode.toLowerCase().includes(countryCode.toLowerCase())
        );
        if (this.filterdcountries.length != 0) {
            for (let i = 0; i < this.filterdcountries.length; i++) {
                this.filterdname.push(this.filterdcountries[i].name);
            }
        }
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
            this.currencycode = '';
            this.invoice.invoiceCurrencyCode = '';
            return;
        }
    }
    selectCurrencyOption(event) {
        this.invoice.invoiceCurrencyCode = event;
        this.SetExchangeRate(event);
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
            this.basicForm.get('street').addValidators([Validators.required]);
        }
        this.basicForm.get('vatid').updateValueAndValidity();
        this.basicForm.get('pin').updateValueAndValidity();
        this.basicForm.get('Buildno').updateValueAndValidity();
        this.basicForm.get('state').updateValueAndValidity();
        this.basicForm.get('city').updateValueAndValidity();
        this.basicForm.get('street').updateValueAndValidity();
    }
    // eslint-disable-next-line @angular-eslint/use-lifecycle-interface
    ngOnInit(): void {
        this.route.paramMap.subscribe((paramMap) => {
            //this.id = paramMap.get('id');
            //this.type=paramMap.get('type');
        this.id = paramMap.get('id');
        this.type=paramMap.get('type');
        this.theme=paramMap.get('theme');

            if((this.theme).toLowerCase() == 'sales')
            {
                this.header='Sales';
                this.transtype='388';
            }
            else if((this.theme).toLowerCase()=='credit')
            {
                this.header='Credit Note';
                this.transtype='381';
            }
            else if((this.theme).toLowerCase()=='credit'){
                this.header='Debit Note';
            }
            else{
                this.header='Draft';
                this.transtype='383';
            }
        });
        this.getbuyertype();
        this._tenantConfigurations.getTenantConfigurationByTransactionType(this.header).subscribe((e) => {
            if (e?.tenantConfiguration?.shipmentJson) {
                this.deliveryDetailsModel = JSON.parse(e.tenantConfiguration.shipmentJson);
                this.deliveryFormGroup = this.buildForm(this.deliveryDetailsModel, this.deliveryDetailFields);
                this.deliveryLangFormGroup = this.buildForm(this.deliveryDetailsModel, this.deliveryLangDetailFields);
                this.availableTabs.push(Tabs.Shipment);
                this.showNext = true;
            }

            if (e?.tenantConfiguration?.additionalFieldsJson) {
                this.additionalDetailsModel = JSON.parse(e.tenantConfiguration.additionalFieldsJson);
                this.additionalFormGroup = this.buildForm(this.additionalDetailsModel, this.additionalDetailFields);
                this.availableTabs.push(Tabs.Additional);
                this.showNext = true;
            }
        });

        this._tenantConfigurations.getTenantConfigurationByTransactionType('General').subscribe((e) => {
            this.generateDraft = e?.tenantConfiguration.additionalData2==='true'
            if (e?.tenantConfiguration?.language) {
                if (e.tenantConfiguration.language == 'dual') {
                    this.multilanguage = true;
                }
            }
            this.getInvoiceData(this.id);
            //this.getsalesitemdetail(this.id,this.type);
        });

        this.address.countryCode = null;
        this.getCountriesDropdown();
        this.create();
        this.getProducts();
        //this.onSelectCustomer();
        this.invoiceSummary.totalAmountWithVAT = 0;
        this.invoiceSummary.totalAmountWithoutVAT = 0;
        this.invoiceSummary.sumOfInvoiceLineNetAmount = 0;
        this.invoiceSummary.netInvoiceAmount = 0;
        this.invoiceItem.vatCode = 'S';
        this.discount = 0;
        this.invoiceItem.uom = 'PC';
        this.invoiceItem.quantity = 0;
        this.invoiceItem.vatRate = 15;
        this.invoiceItem.discountPercentage = 0;
        this.supplier.registrationName = this._sessionService.tenant.name;
        this.supplier.contactPerson.email = this._sessionService.user.emailAddress;
        //this.invoice.customerId = '567';
        if((this.supplier.registrationName).toLowerCase() == 'brady')
        {
            this.basicForm.get('referenceNo').addValidators([Validators.required]);
           // this.isreference=true;

        }
        else
        {
            this.basicForm.get('referenceNo').addValidators([]);
            //this.isreference=false;


        }
        this.basicForm.get('referenceNo').updateValueAndValidity();
        this.invoiceSummary.sumOfInvoiceLineNetAmountCurrency = 'SAR';
        this.invoice.location = 'Ind';
    }
    //get countries from master data
    getCountriesDropdown() {
        this._masterCountriesServiceProxy.getCountriesList().subscribe((result) => {
            this.countries = result;
        });
    }
    //--------------------------------------auto complete starts---------------------------------------------------------
    filterInvoices(event): void {
        this._salesInvoiceServiceProxy.getInvoiceSuggestions(event.query,'').subscribe((data) => {
            if (data == null || data === undefined || data.length === 0) {
                this.invoice.billingReferenceId = event.query;
                this.editaddressMode = false;
                this.isIRNo=true;
            } else {
                this.editaddressMode = true;
                this.isIRNo=false;
                for (let i = 0; i < data.length; i++) {
                    data[i] = data[i].irnNo + ' ' + data[i].issueDate.toString();
                }
                this.billingReferenceIds = data;
            }
        });
    }
    filterSAPInvoices(event): void {
        this._salesInvoiceServiceProxy.getInvoiceSuggestions('',event.query).subscribe((data) => {
            if (data == null || data === undefined || data.length === 0) {
                this.invoice.invoiceNumber = event.query;
                this.editaddressMode = false;
                this.isRefNo=true;
            } else {
                this.editaddressMode = true;
                this.isRefNo=false;
                for (let i = 0; i < data.length; i++) {
                    data[i] = data[i].invoiceNumber + ' ' + data[i].issueDate.toString();
                }
                this.sapRefNos = data;
            }
        });
    }
    filterCustomers(event): void {
        this._customerServiceProxy.getCustomerName(event.query).subscribe((data) => {
            this.filteredCustomers = data;
            if (data.length === 0) {
                this.customer.registrationName = event.query;
            }
        });
    }
    //Get Credit Note Customer Details
    onSelectCustomer(event) {
        // this.filteredCustomerAddress=this.filteredCustomers.filter(p=>p.name == event.query);
        this.customer.registrationName = event.name;
        this.customer.vatid = event.vatid;
        this.customer.customerId = event.customerId;
        this.address.buildingNo = event.buildingNo;
        this.address.street = event.street;
        this.address.additionalStreet = event.additionalstreet;
        this.address.neighbourhood = event.neighbourhood;
        this.address.city = event.city;
        this.address.state = event.state;
        this.address.countryCode = event.countryCode;
        this.countryname = this.countries.find((p) => p.alphaCode == event.countryCode).name;
        this.selectOption(this.countryname);
        this.address.postalCode = event.postalCode;
        this.customer.contactPerson.contactNumber = event.contactNumber;
        this.customer.contactPerson.email = event.email;
    }

    getCustomerInfo(data, filter) {
        this.customer=data.buyer.filter(p=>p.language='EN');
        this.dataType=data.source;
        if(data.buyer.length > 1)
        {
        this.delivery=data.buyer[1];
        this.delivery.id=data.buyer[1].id;
        this.delivery.address.id=data.buyer[1].id;
        this.delivery.contactPerson.id=data.buyer[1].contactPerson.id;

        }
        if(this.theme!='sales')
            {
              this.invoice.billingReferenceId = data.billingReferenceId;
              this.invoice.invoiceNumber = data.billingReferenceId;
            }else{
            this.invoice.billingReferenceId = data.irnNo;
            this.invoice.invoiceNumber = data.billingReferenceId;
            }
            this.invoice.id=data.id;
            this.invoice.uniqueIdentifier=data.uniqueIdentifier;
        this.exchangerate = JSON.parse(data.additionalData1);
        this.exchangeRateVal = this.exchangerate[0].exchangeRate;
        this.header_Additional1.exchangeRate=this.exchangeRateVal;
        this.address=this.customer[0].address;
        this.countryname = this.countries.find((p) => p.alphaCode == this.address.countryCode)?.name;
            this.selectOption(this.countryname);
        this.customer.contactPerson=this.customer[0].contactPerson;
        this.customer.vatid = this.customer[0].vatid;
        this.customer.registrationName = this.customer[0].registrationName;
        this.customer.customerId = this.customer[0].customerId;
        this.customer.id=data.buyer[0].id;
        this.customer.id=data.buyer[0].id;
       // this.delivery.id=data.buyer[1].id;
       // this.delivery.address.id=data.buyer[1].id;
        this.invoiceSummary.id=data.invoiceSummary.id;
        this.discount=data.discount;
        if(data.vatDetails != undefined )
        {
        this.vatDetails[0]=data.vatDetails;
        this.vatDetails[0].id=data?.vatDetails[0].id;
        }
        if(data.paymentDetails != undefined )
        {
        this.paymentDetails=data.paymentDetails;
        this.paymentDetails[0].id=data?.paymentDetails[0].id;
        }
        this.customer.contactPerson.id=data.buyer[0].contactPerson.id;
      //  this.delivery.contactPerson.id=data.buyer[1].contactPerson.id;
        this.shipment = JSON.parse(this.customer[0].additionalData1);
        this.additionalData = JSON.parse(data.additionalData2);
        const jsonData = JSON.parse(data.additionalData2);
        this.additionalFormGroup.patchValue(jsonData[0]);
        //this.getAdditionalInfo(data);
        this.getShipmentInfo(data);
        this.Items=data.items;
        this.invoiceItems=this.Items.filter(p=>p.isOtherCharges == false);
        this.invoiceCharges=this.Items.filter(p=>p.isOtherCharges == true);
        if(this.invoiceCharges.length >0)
        {
            this.enablecharges(true);
        }
        this.invoiceSummary=data.invoiceSummary;
        console.log(this.customer,'address');
        if (data.language === 'AR' && this.multilanguage) {
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
            //  this.multilangform.get('BillToAttn').setValue(data[0].billToAttn);
            //   this.multilangform.get('buyertype').setValue(data[0].otherDocumentTypeId);
        } else {
            if(this.theme!='sales')
            {
                
                
              this.invoice.billingReferenceId = data.invoiceNumber;
              //this.invoice.invoiceNumber = data.invoiceNumber;
            }else{
            this.invoice.billingReferenceId = data.irnNo;
            this.invoice.invoiceNumber = data.billingReferenceId;
            }
          //  this.exchangerate = JSON.parse(data.additionalData1);
           // this.exchangeRateVal = this.exchangerate[0].exchangeRate;
            this.invoice.invoiceNotes = data.invoiceNotes;
            this.invoice.invoiceCurrencyCode = data?.invoiceCurrencyCode;
            this.currencycode = data.invoiceCurrencyCode;
            this.dateofsupply = data.issueDate;
            this.prchaseEntryDate = data.issueDate;
            this.customer.contactPerson.contactNumber = data.contactNumber;
           // this.customer.vatid = data.vatid;
            //this.customer.customerId = data.customerId;
            //this.customer.registrationName = data.registrationName;
            // this.address.buildingNo = data.address.buildingNo;
            // this.address.street = data.address[0].street?.trim();
            // this.address.additionalStreet = data[0].additionalStreet?.trim();
            // this.address.neighbourhood = data[0].neighbourhood?.trim();
            // this.address.city = data[0].city?.trim();
            // this.address.state = data[0].state;
            // this.address.countryCode = data[0].countryCode;
            // this.address.postalCode = data[0].postalCode;
           // this.address=data.address.filter(p=>p.language == 'EN');
           // this.customer.contactPerson.email = data[0].email;
            this.customer.otherID = data?.otherDocumentTypeId;
            this.exchange = JSON.parse(data.exchange);
            this.header_Additional1.exchangeRate = this.exchange[0].exchangeRate;
            this.basicForm.get('exchangeRate').setValue(this.exchange[0].exchangeRate);
            // this.customer.contactPerson.name = data[0].billToAttn;
            // this.getActiveCurrency(data[0].countryCode);
            this.countryname = this.countries.find((p) => p.alphaCode == data[0].countryCode)?.name;
            this.selectOption(this.countryname);
        }
        // this._salesInvoiceServiceProxy.getsalesitemdetail(filter,'sales').subscribe((data) => {
        //     this.enablecharge = false;
        //     this.invoiceCharges = [];
        //     for (let i = 0; i < data.length; i++) {
        //         if (this.currencycode == 'SAR') {
        //             this.invoiceItem.costPrice = data[i].costPrice;
        //             this.invoiceItem.description = data[i].description;
        //             this.invoiceItem.excemptionReasonCode = data[i].excemptionReasonCode;
        //             this.invoiceItem.excemptionReasonText = data[i].excemptionReasonText;
        //             this.invoiceItem.name = data[i].name;
        //             this.invoiceItem.discountAmount = data[i].discountAmount;
        //             this.invoiceItem.discountPercentage = data[i].discountPercentage;
        //             this.invoiceItem.grossPrice = data[i].grossPrice;
        //             this.invoiceItem.lineAmountInclusiveVAT = data[i].lineAmountInclusiveVAT;
        //             this.invoiceItem.quantity = data[i].quantity;
        //             this.invoiceItem.unitPrice = data[i].unitPrice;
        //             this.invoiceItem.netPrice = data[i].netPrice;
        //             this.invoiceItem.vatAmount = data[i].vatAmount;
        //             this.invoiceItem.vatRate = data[i].vatRate;
        //             this.invoiceItem.vatCode = data[i].vatRate === 15 ? 'S' : 'Z';
        //             this.invoiceItem.isOtherCharges = data[i].isOtherCharges;
        //             if (data[i].isOtherCharges) {
        //                 this.invoiceCharges.push(this.invoiceItem);
        //                 this.enablecharge = true;
        //             } else {
        //                 this.invoiceItems.push(this.invoiceItem);
        //             }
        //             this.originalInvoiceItems = JSON.parse(JSON.stringify(this.invoiceItems));
        //             this.invoiceCount = this.invoiceItems.length;
        //             this.invoiceItem = new CreateOrEditDraftItemDto();
        //         } else {
        //             this.invoiceItem.costPrice = data[i].costPrice / this.header_Additional1.exchangeRate;
        //             this.invoiceItem.description = data[i].description;
        //             this.invoiceItem.excemptionReasonCode = data[i].excemptionReasonCode;
        //             this.invoiceItem.excemptionReasonText = data[i].excemptionReasonText;
        //             this.invoiceItem.name = data[i].name;
        //             this.invoiceItem.discountAmount = data[i].discountAmount;
        //             this.invoiceItem.discountPercentage = data[i].discountPercentage;
        //             this.invoiceItem.grossPrice = data[i].grossPrice / this.header_Additional1.exchangeRate;
        //             this.invoiceItem.lineAmountInclusiveVAT =
        //                 data[i].lineAmountInclusiveVAT / this.header_Additional1.exchangeRate;
        //             this.invoiceItem.quantity = data[i].quantity;
        //             if (data[i].isOtherCharges) {
        //                 this.invoiceItem.unitPrice = data[i].unitPrice;
        //             } else {
        //                 this.invoiceItem.unitPrice = data[i].unitPrice / this.header_Additional1.exchangeRate;
        //             }
        //             this.invoiceItem.netPrice = data[i].netPrice / this.header_Additional1.exchangeRate;
        //             this.invoiceItem.vatAmount = data[i].vatAmount / this.header_Additional1.exchangeRate;
        //             this.invoiceItem.vatRate = data[i].vatRate;
        //             this.invoiceItem.vatCode = data[i].vatRate === 15 ? 'S' : 'Z';
        //             this.invoiceItem.isOtherCharges = data[i].isOtherCharges;
        //             if (data[i].isOtherCharges) {
        //                 this.invoiceCharges.push(this.invoiceItem);
        //                 this.enablecharge = true;
        //             } else {
        //                 this.invoiceItems.push(this.invoiceItem);
        //             }
        //             this.originalInvoiceItems = JSON.parse(JSON.stringify(this.invoiceItems));
        //             this.invoiceCount = this.invoiceItems.length;
        //             this.invoiceItem = new CreateOrEditDraftItemDto();
        //         }
        //     }
        //     this.calculateInvoiceSummary(this.invoiceSummary, this.invoiceItems);
        //     this.calculategridTotalOtherCharge();
        // });
    }
    //Get Shipment and Delivery Details
    getShipmentInfo(data) {
       // this.shipment = JSON.parse(data.shipment);
        for (let i = 0; i < this.shipment.length; i++) {
            if (this.multilanguage) {
                this.deliveryLangFormGroup.get('registrationName').setValue(data[i].registrationName);
                this.deliveryLangFormGroup.get('crNumber').setValue(data[i].crNumber);
                this.deliveryLangFormGroup.get('vatid').setValue(data[i].vatnumber);
                this.deliveryLangFormGroup.get('address.buildingNo').setValue(data[i].add_buildingNo);
                this.deliveryLangFormGroup.get('address.additionalNo').setValue(data[i].add_additionalNo);
                this.deliveryLangFormGroup.get('address.street').setValue(data[i].add_street);
                this.deliveryLangFormGroup.get('address.city').setValue(data[i].add_city);
                this.deliveryLangFormGroup.get('address.state').setValue(data[i].add_state);
                this.deliveryLangFormGroup.get('address.postalCode').setValue(data[i].add_postalCode);
                this.deliveryLangFormGroup.get('address.countryCode').setValue(data[i].add_countryCode);
                this.deliveryLangFormGroup.get('contactPerson.name').setValue(data[i].contact_name);
                this.deliveryLangFormGroup.get('address.neighbourhood').setValue(data[i].add_neighbourhood);
                this.deliveryLangFormGroup.get('contactPerson.contactNumber').setValue(data[i].contactNumber);
            } else {
                this.deliveryFormGroup.get('registrationName').setValue(this.shipment[i].registrationName);
                this.deliveryFormGroup.get('crNumber').setValue(this.shipment[i].crNumber);
                this.deliveryFormGroup.get('vatid').setValue(this.shipment[i].vatid);
                this.deliveryFormGroup.get('address.buildingNo').setValue(this.shipment[i].address.buildingNo);
                this.deliveryFormGroup.get('address.street').setValue(this.shipment[i].address.street);
                this.deliveryFormGroup.get('address.additionalStreet').setValue(this.shipment[i].address.additionalStreet);
                this.deliveryFormGroup.get('address.city').setValue(this.shipment[i].address.city);
                this.deliveryFormGroup.get('address.additionalNo').setValue(this.shipment[i].address.additionalNo);
                this.deliveryFormGroup.get('address.neighbourhood').setValue(this.shipment[i].address.neighbourhood);
                this.deliveryFormGroup.get('address.state').setValue(this.shipment[i].address.state);
                this.deliveryFormGroup.get('address.postalCode').setValue(this.shipment[i].address.postalCode);
                this.deliveryFormGroup.get('address.countryCode').setValue(this.shipment[i].address.countryCode);
                this.deliveryFormGroup.get('contactPerson.name').setValue(this.shipment[i].contactPerson.name);
                //this.deliveryFormGroup.get('contactPerson.contactNumber').setValue(data[i].contactNumber);
            }
        }
    }
    //Additional Information
    getAdditionalInfo(data) {
        const jsonData = JSON.parse(data[0].additionalData2);
        this.additionalFormGroup.patchValue(jsonData[0]);
        console.log(this.additionalFormGroup);
    }

    onSelectInvoice(event,refNo): void {
        this._salesInvoiceServiceProxy.getsalesdetails(event.split(' ')[0],undefined).subscribe((data) => {
            this.getCustomerInfo(data, event.split(' ')[0]);
            this.delivery_customer = this.deliveryFormGroup?.value ?? null;
            this.delivery_customerlang = this.deliveryLangFormGroup?.value ?? null;
            this.additional_info = this.additionalFormGroup?.value ?? null;
            if (
                this.delivery_customer != null &&
                this.delivery_customerlang != null &&
                this.delivery_customer != undefined &&
                this.delivery_customerlang != undefined
            ) {
                this.getShipmentInfo(data);
            }
            if (
                this.additional_info != null &&
                this.additional_info != null &&
                this.additional_info != undefined &&
                this.additional_info != undefined
            ) {
                this.getAdditionalInfo(data);
            }
        });
        this.invoiceItems = [];
    }
    onSelectSAPInvoice(irnNo,event): void {
        this._salesInvoiceServiceProxy.getsalesdetails(undefined,event.split(' ')[0]).subscribe((data) => {
            this.getCustomerInfo(data, event.split(' ')[0]);
            this.delivery_customer = this.deliveryFormGroup?.value ?? null;
            this.delivery_customerlang = this.deliveryLangFormGroup?.value ?? null;
            this.additional_info = this.additionalFormGroup?.value ?? null;
            if (
                this.delivery_customer != null &&
                this.delivery_customerlang != null &&
                this.delivery_customer != undefined &&
                this.delivery_customerlang != undefined
            ) {
                this.getShipmentInfo(data);
            }
            if (
                this.additional_info != null &&
                this.additional_info != null &&
                this.additional_info != undefined &&
                this.additional_info != undefined
            ) {
                this.getAdditionalInfo(data);
            }
        });
        this.invoiceItems = [];
    }
    //--------------------------------------auto complete ends---------------------------------------------------------
    getProducts(): void {}
    updateProduct(i: number) {}

    filterProduct() {}

    changeStep(step: Tabs) {
        this.language = 1;
        this.currentStep = step;
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

    nextStep() {
        let curIndex = this.availableTabs.findIndex((a) => a == this.currentStep);

        if (curIndex <= this.availableTabs.length - 2) {
            this.currentStep = this.availableTabs[curIndex + 1];
        }

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

        if (curIndex >= 1) {
            this.currentStep = this.availableTabs[curIndex - 1];
        }

        if (curIndex == 1) {
            this.showNext = true;
            this.showBack = false;
        } else {
            this.showNext = false;
            this.showBack = true;
        }
    }

    create() {
        this.customer.contactPerson = new CreateOrEditCreditNoteContactPersonDto();
        this.delivery.contactPerson = new CreateOrEditCreditNoteContactPersonDto();
        this.supplier.contactPerson = new CreateOrEditCreditNoteContactPersonDto();
        this.supplier.address = new CreateOrEditDraftAddressDto();
        //--------------------brady-----------------------------
        this.delivery_customer.contactPerson = new CreateOrEditCreditNoteContactPersonDto();
        this.delivery_customer.address = new CreateOrEditDraftAddressDto();
        this.delivery_customerlang.contactPerson = new CreateOrEditCreditNoteContactPersonDto();
        this.delivery_customerlang.address = new CreateOrEditDraftAddressDto();
        //--------------------brady-----------------------------
    }

    //add item to invoiceitems
    addItem() {
        if (!this.address.countryCode) {
            this.notify.error(this.l('Please select a country.'));
            this.isSaving = false;
            return;
        }
        if (this.invoiceItem.vatCode === 'S') {
            this.invoiceItem.vatRate = 15;
        } else {
            this.invoiceItem.vatRate = 0;
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
            !(
                this.invoiceItem.name &&
                this.invoiceItem.description &&
                this.invoiceItem.quantity &&
                this.invoiceItem.unitPrice
            )
        ) {
            this.notify.error(this.l('Please fill all required fields to add item to invoice.'));
            return;
        }
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
        this.invoiceItem.identifier = 'Credit';
        if (this.address.countryCode !== 'SA' && this.invoiceItem.vatRate !== 0) {
            this.notify.error(this.l('Exports are exempt from VAT'));
            return;
        }
        if (this.invoiceItem.vatCode != 'S') {
            if (
                this.vatDetail.excemptionReasonCode == undefined ||
                this.vatDetail.excemptionReasonText == undefined ||
                this.vatDetail.excemptionReasonCode == null ||
                this.vatDetail.excemptionReasonText == null
            ) {
                this.notify.error(this.l('Please select a exemption reason.'));
                return;
            }
        }
        this.invoiceItem.excemptionReasonCode = this.vatDetail.excemptionReasonCode;
        this.invoiceItem.excemptionReasonText = this.vatDetail.excemptionReasonText;
        this.invoiceItem.isOtherCharges = false;
        this.invoiceItems.push(this.invoiceItem);
        //this.vatDetails.push(this.vatDetail);
        this.invoiceItem = new CreateOrEditDraftItemDto();
        this.calculateInvoiceSummary(this.invoiceSummary, this.invoiceItems);
        this.invoiceItem.vatCode = 'S';
        this.invoiceItem.uom = 'PC';
        this.invoiceItem.quantity = 0;
        this.invoiceItem.discountPercentage = 0;
        this.invoiceItem.vatRate = 15;
        this.isExempt = false;
        this.exmptReason = null;
    }

    deleteItem(index: number) {
        this.invoiceItems.splice(index, 1);
        if (this.invoiceItems.length == 0) {
            this.isAutoCompleteDisabled = false;
        }
        this.vatDetail = new CreateOrEditDraftVATDetailDto();
        this.calculateInvoiceSummary(this.invoiceSummary, this.invoiceItems);
    }

    editItem(index: number) {
        this.invoiceItem = this.invoiceItems[index];
        this.invoiceItems.splice(index, 1);
        this.onVatChange();
        this.calculateInvoiceSummary(this.invoiceSummary, this.invoiceItems);
    }
    clearItem() {
        this.invoiceItem = new CreateOrEditDraftItemDto();
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
        if (this.dateofsupply === undefined || this.dateofsupply == null){
          this.invoice.dateOfSupply = this.invoice.issueDate;
        } else {
          this.invoice.dateOfSupply = this.dateofsupply;
        }        //this.invoice.invoiceCurrencyCode = 'SAR';
        //  this.invoice.buyer.crNumber = '1234567890';
        this.invoice.invoiceSummary.currencyCode = 'SAR';
        this.invoice.invoiceSummary.paidAmountCurrency = 'SAR';
        this.invoice.invoiceSummary.payableAmountCurrency = 'SAR';
        this.invoice.invoiceSummary.netInvoiceAmountCurrency = 'SAR';
        this.invoice.invoiceSummary.totalAmountWithoutVATCurrency = 'SAR';
       //this.invoice.invoiceNumber = '-1'; //!this.invoice.billingReferenceId ? " " : this.invoice.billingReferenceId
        /// this.invoice.buyer.vatid = !this.customer.vatid?.trim() ? ' ' : this.customer.vatid;
        // this.invoice.buyer.contactPerson.name = !this.customer.registrationName?.trim()
        //     ? 'NA'
        //     : this.customer.registrationName;
        //  this.invoice.buyer.contactPerson.type = !this.customer.contactPerson.type?.trim() ? "NA" : this.customer.contactPerson.type;
        // this.invoice.buyer.address.type = !this.address.type?.trim() ? ' ' : this.address.type;
        // this.invoice.buyer.address.additionalNo = !this.address.additionalNo?.trim() ? ' ' : this.address.additionalNo;
        // this.invoice.buyer.address.city = !this.invoice.buyer.address.city?.trim()
        //   ? ' '
        //   : this.invoice.buyer.address.city;
        // this.invoice.buyer.address.countryCode = !this.invoice.buyer.address.countryCode?.trim()
        //   ? ' '
        //   : this.invoice.buyer.address.countryCode;
        // this.invoice.buyer.address.postalCode = !this.invoice.buyer.address.postalCode?.trim()
        //   ? ''
        //   : this.invoice.buyer.address.postalCode;
        // this.invoice.buyer.address.state = !this.invoice.buyer.address.state?.trim()
        //   ? ''
        //   : this.invoice.buyer.address.state;
        // this.invoice.buyer.address.street = !this.invoice.buyer.address.street?.trim()
        //   ? ' '
        //   : this.invoice.buyer.address.street;
        // this.invoice.buyer.address.state = !this.invoice.buyer.address.state?.trim()
        //   ? ' '
        //   : this.invoice.buyer.address.state;
        // this.invoice.buyer.address.buildingNo = !this.invoice.buyer.address.buildingNo?.trim()
        //   ? ''
        //   : this.invoice.buyer.address.buildingNo;
        // this.invoice.buyer.address.neighbourhood = !this.invoice.buyer.address.neighbourhood?.trim()
        //   ? ' '
        //   : this.invoice.buyer.address.neighbourhood;
        // this.invoice.buyer.contactPerson.email = !this.customer.contactPerson.email?.trim()
        //   ? ' '
        //   : this.customer.contactPerson.email;
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
    }

    async save() {
        if (await this.isRefNumExists()) {
            this.notify.error(this.l('Entered Reference Number already exists'));
            return null;
        }
        if (this.header_Additional1.exchangeRate == null || this.header_Additional1.exchangeRate == undefined) {
            this.notify.error(this.l('Please enter exhange rate'));
            return null;
        }
        this.delivery_customer = this.deliveryFormGroup?.value ?? null;
        this.delivery_customerlang = this.deliveryLangFormGroup?.value ?? null;
        this.additional_info = this.additionalFormGroup?.value ?? null;
        if (
            this.delivery_customer != null &&
            this.delivery_customerlang != null &&
            this.delivery_customer != undefined &&
            this.delivery_customerlang != undefined
        ) {
            this.adddelivery();
        }
        if (this.address.neighbourhood == null || this.address.neighbourhood === undefined) {
            this.basicForm.get('Neighbourhood').setValue('.');
        }
        let isSimplified = this.invoiceSummary.totalAmountWithVAT < 1000;

        if (this.issueDate.toString() > DateTime.now().toString()) {
            this.notify.error(this.l('Invoice Date Should not be greater than Current Date.'));
            this.isSaving = false;
            return;
        }
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
            //language
            this.customerLang.customerId = this.multilangform.get('CustomerId').value;
            this.customerLang.registrationName = this.multilangform.get('Name').value;
            this.customerLang.vatid = this.multilangform.get('vatid').value;
            this.addressLang.buildingNo = this.multilangform.get('Buildno').value;
            this.addressLang.street = this.multilangform.get('street').value;
            this.addressLang.additionalStreet = this.multilangform.get('additionalstreet').value;
            this.addressLang.city = this.multilangform.get('city').value;
            this.addressLang.state = this.multilangform.get('state').value;
            this.addressLang.countryCode = this.multilangform.get('nationality').value;
            this.delivery.contactPerson.type = 'Delivery';
            this.deliveryAddress.type = 'Delivery';
            this.delivery.type = 'Delivery';
            this.customerLang.language = 'AR';
            this.addressLang.language = 'AR';
            this.address.language = 'EN';
            this.customer.language = 'EN';
            this.invoice.paymentDetails=this.paymentDetails;
            this.invoice.supplier = [this.supplier];
            if (this.multilanguage == true) {
                this.invoice.buyer = [this.customer, this.customerLang, this.delivery, this.deliverylang];
                this.invoice.buyer[0].address = this.address;
                this.invoice.buyer[1].address = this.addressLang;
                this.invoice.buyer[2].address = this.delivery.address;
                this.invoice.buyer[3].address = this.deliverylang.address;
            } else {
                this.invoice.buyer = [this.customer, this.delivery];
                this.invoice.buyer[0].address = this.address;
                this.invoice.buyer[1].address = this.delivery.address;
            }

            try {
                this.invoice.issueDate = this.parseDate(this.issueDate.toString());
                if(this.prchaseEntryDate != null || this.prchaseEntryDate!=undefined)
                {
                if (this.invoice.issueDate < this.parseDate(this.prchaseEntryDate.toString())) {
                  if (this.editaddressMode === true)  {

                  this.notify.error(this.l('Credit Note date should not be less than the invoice date.'));
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
            this.invoice.supplier[0].address = this.supplier.address;
            this.fillDummy();
            //-------------------brady---------------------------------
            this.invoice.invoiceSummary.additionalData1 = JSON.stringify([this.summary_Additional1]);
            this.invoice.additionalData1 = JSON.stringify([this.header_Additional1]);
            if (
                this.additional_info != null &&
                this.additional_info != null &&
                this.additional_info != undefined &&
                this.additional_info != undefined
            ) {
                this.invoice.additionalData2 = JSON.stringify([this.additional_info]);
            }
            if (
                this.delivery_customer != null &&
                this.delivery_customerlang != null &&
                this.delivery_customer != undefined &&
                this.delivery_customerlang != undefined
            ) {
                this.invoice.buyer[0].additionalData1 = JSON.stringify([this.delivery_customer]);
            }
            if (this.multilanguage == true) {
                this.invoice.buyer[1].additionalData1 = JSON.stringify([this.deliveryLangFormGroup.value]);
            }
            //----------------------------------------------------------
            this.invoice.vatDetails = this.vatDetails;
            this.convertedInvoice = this.invoice;//JSON.parse(JSON.stringify(this.invoice));
            this.ConvertToSAR();
            if(this.header == 'Sales')
            {
                this.convertedInvoice.billingReferenceId=this.invoice.invoiceNumber;
            }
            var obj = new CreateOrEditDraftDto();
            obj.init(this.convertedInvoice)
            obj.source=this.dataType;
            obj.invoiceTypeCode=this.transtype;
            obj.transTypeDescription=this.transtype;
            if (this.invoiceCharges.length > 0) {
                for (let j = 0; j < this.invoiceCharges.length; j++) {
                    this.invoiceCharges[j].isOtherCharges = true;
                    this.convertedInvoice.items.push(JSON.parse(JSON.stringify(this.invoiceCharges[j])));
                }
            }
          
            
            this._draftServiceProxy.createDraft(obj).subscribe((result) => {
                                this.notify.success(this.l('SavedSuccessfully'));
                              this.editMode = false;
                               this.isSaving = false;
                              this.router.navigate(['/app/main/transactions'], { state: { tabvaule: 'Credit Note' } });
                          });
        //     if(this.isRefNo && this.isIRNo)
        //     {
        //         Swal.fire({
        //             title: 'Invoice or SAP Reference Number Does Not Exists, Still Do You Want to Save?',
        //             text: '',
        //             icon: 'warning',
        //             showCancelButton: true,
        //             confirmButtonColor: '#3085d6',
        //             cancelButtonColor: '#d33',
        //             confirmButtonText: 'Yes, save it!',
        //         }).then((result) => {
        //             if (result.isConfirmed) {
        //                 if(this.generateDraft){
        //                     this._draftServiceProxy.createDraft(obj).subscribe((result) => {
        //                         this.notify.success(this.l('SavedSuccessfully'));
        //                         this.editMode = false;
        //                         this.isSaving = false;
        //                         this.router.navigate(['/app/main/transactions'], { state: { tabvaule: 'Credit Note' } });
        //                     });
        //                 }else{
        //                     // this._CreditNoteServiceProxy.createCreditNote(this.convertedInvoice).subscribe((result) => {
        //                     //     this.notify.success(this.l('SavedSuccessfully'));
        //                     //     this.editMode = false;
        //                     //     this.isSaving = false;
        //                     //     this.router.navigate(['/app/main/transactions'], { state: { tabvaule: 'Credit Note' } });
        //                     //     //this.download(result.invoiceId, result.uuid);
        //                     // });
        //                 }
        //             }
        //             else{
        //                 this.isSaving = false;
        //             }
        //         });
        //     }
        //     else
        //     {
        //     this.basicForm.markAsUntouched();
        //     if(this.generateDraft){
        //         this._draftServiceProxy.createDraft(obj).subscribe((result) => {
        //             this.notify.success(this.l('SavedSuccessfully'));
        //             this.editMode = false;
        //             this.isSaving = false;
        //             this.router.navigate(['/app/main/transactions'], { state: { tabvaule: 'Credit Note' } });
        //         });
        //     }else{
        //         // this._CreditNoteServiceProxy.createCreditNote(this.convertedInvoice).subscribe((result) => {
        //         //     this.notify.success(this.l('SavedSuccessfully'));
        //         //     this.editMode = false;
        //         //     this.isSaving = false;
        //         //     this.router.navigate(['/app/main/transactions'], { state: { tabvaule: 'Credit Note' } });
        //         //     //this.download(result.invoiceId, result.uuid);
        //         // });
        //     }
        // }
        } else {
            if (this.additionalDetailsModel != undefined || this.deliveryDetailsModel != undefined) {
                if (!this.basicForm.valid) {
                    this.basicForm.markAllAsTouched();
                    this.currentStep = 1;
                } else if (!this.deliveryfields) {
                    this.deliveryFormGroup.markAllAsTouched();
                    this.currentStep = 2;
                } else if (!this.additionalfields) {
                    this.additionalFormGroup.markAllAsTouched();
                    this.currentStep = 3;
                }
            } else {
                this.basicForm.markAllAsTouched();
                this.currentStep = 1;
            }
            window.scroll(0, 0);
            this.notify.error(this.l('Please fill all the required fields'));
        }
    }

    adddelivery() {
        this.delivery.registrationName = this.delivery_customer.registrationName;
        this.delivery.vatid = this.delivery_customer.vatid;
        this.deliveryAddress.buildingNo = this.delivery_customer.address.buildingNo;
        this.deliveryAddress.additionalNo = this.delivery_customer.address.additionalNo;
        this.deliveryAddress.street = this.delivery_customer.address.street;
        this.deliveryAddress.additionalStreet = this.delivery_customer.address.additionalStreet;
        this.deliveryAddress.city = this.delivery_customer.address.city;
        this.deliveryAddress.neighbourhood = this.delivery_customer.address.neighbourhood;
        this.deliveryAddress.state = this.delivery_customer.address.state;
        this.deliveryAddress.postalCode = this.delivery_customer.address.postalCode;
        this.deliveryAddress.countryCode = this.delivery_customer.address.countryCode;
        this.delivery.contactPerson.contactNumber = this.delivery_customer.contactPerson.contactNumber;
        this.delivery.contactPerson.email = this.delivery_customer.contactPerson.email;
        this.delivery.contactPerson.type = 'Delivery';
        this.deliveryAddress.type = 'Delivery';
        this.delivery.type = 'Delivery';
        this.delivery.address = this.deliveryAddress;
        this.deliverylang.language = 'AR';
        this.deliveryAddressLang.language = 'AR';
        this.deliveryAddress.language = 'EN';
        this.delivery.language = 'EN';
        if (this.multilanguage == true) {
            this.deliverylang.registrationName = this.delivery_customerlang.registrationName;
            this.deliverylang.vatid = this.delivery_customerlang.vatid;
            this.deliveryAddressLang.buildingNo = this.delivery_customerlang.address.buildingNo;
            this.deliveryAddressLang.additionalNo = this.delivery_customerlang.address.additionalNo;
            this.deliveryAddressLang.street = this.delivery_customerlang.address.street;
            this.deliveryAddressLang.additionalStreet = this.delivery_customerlang.address.additionalStreet;
            this.deliveryAddressLang.city = this.delivery_customerlang.address.city;
            this.deliveryAddressLang.neighbourhood = this.delivery_customerlang.address.neighbourhood;
            this.deliveryAddressLang.state = this.delivery_customerlang.address.state;
            this.deliveryAddressLang.postalCode = this.delivery_customerlang.address.postalCode;
            this.deliveryAddressLang.countryCode = this.delivery_customerlang.address.countryCode;
            this.deliverylang.type = 'Delivery';
            this.deliveryAddressLang.type = 'Delivery';
            this.deliverylang.address = this.deliveryAddressLang;
            this.invoice.delivery = [this.delivery, this.deliverylang];
            this.invoice.delivery[0].address = this.delivery.address;
            this.invoice.delivery[1].address = this.deliverylang.address;
        } else {
            this.invoice.delivery = [this.delivery];
            this.invoice.delivery[0].address = this.delivery.address;
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
            this.reasonfilter = this.exemptionReason.filter((p) => p.name == this.invoiceItem.excemptionReasonCode);
            this.exmptReason = this.reasonfilter[0].id;
            this.vatDetail.excemptionReasonCode = this.invoiceItem.excemptionReasonCode;
            this.vatDetail.excemptionReasonText = this.invoiceItem.excemptionReasonText;
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
        }
    }
    //--------------------dynamic form-----------------------------------
    checkautocomplete() {
        if (this.invoiceItems.length > 0 && this.address.countryCode != null && this.address.countryCode != undefined) {
            this.notify.error('Cannot Change Country After Adding Item');
            this.isAutoCompleteDisabled = true;
        }
    }
    selectOption(event) {
        // Perform the desired action with the selected option
        if (event != undefined) {
            this.filterdalpha3code = this.countries.filter((p) => p.name.toLowerCase() == event.toLowerCase());
            this.getActiveCurrency(this.filterdalpha3code[0].alphaCode);
            this.address.countryCode = this.filterdalpha3code[0].alphaCode;
            this.countrychanges();
        }
    }
    private buildForm(model: any, fields: any[]) {
        const formGroupFields = this.getFormControlsFields(model, fields);
        return new FormGroup(formGroupFields);
    }

    private getFormControlsFields(model: any, fields: any[], formGroupName: string = '') {
        const formGroupFields = {};
        for (const field of Object.keys(model)) {
            if (field == 'isChildGroup') {
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

    changeLanguage(lan: number) {
        this.language = lan;
    }

    //--------------------dynamic form-----------------------------------

    //--------------------Other Charges-----------------------------------
    addCharges() {
        if (!(this.invoiceCharge.name && this.invoiceCharge.description && this.invoiceCharge.unitPrice)) {
            this.notify.error(this.l('Please fill all required fields to add charges to invoice.'));
            return;
        }
        if (this.invoiceItems.filter((p) => p.vatAmount > 0).length > 0) {
          this.invoiceCharge.vatAmount = (this.invoiceCharge.unitPrice * 15) / 100;
          this.invoiceCharge.lineAmountInclusiveVAT = this.invoiceCharge.unitPrice*this.header_Additional1.exchangeRate + this.invoiceCharge.vatAmount;
      } else {
          this.invoiceCharge.vatAmount = 0;
          this.invoiceCharge.lineAmountInclusiveVAT = this.invoiceCharge.unitPrice*this.header_Additional1.exchangeRate;
      }
        this.invoiceCharges.push(this.invoiceCharge);
        //this.vatDetails.push(this.vatDetail);
        this.invoiceCharge = new CreateOrEditSalesInvoiceItemDto();
        this.calculategridTotalOtherCharge();
        this.invoiceCharge.name=null;
        this.invoiceCharge.unitPrice=null;
    }

    editCharges(index: number) {
        this.invoiceCharge = this.invoiceCharges[index];
        this.invoiceCharges.splice(index, 1);
        this.calculategridTotalOtherCharge();
    }

    deleteCharges(index: number) {
        this.invoiceCharges.splice(index, 1);
        this.calculategridTotalOtherCharge();
    }

    clearCharges() {
        this.invoiceCharge = new CreateOrEditDraftItemDto();
    }
    calculategridTotalOtherCharge() {
        this.summary_Additional1.totalOther = 0;
        this.totalChargeVATAmount = 0;
        for (let i = 0; i < this.invoiceCharges.length; i++) {
            this.summary_Additional1.totalOther += this.invoiceCharges[i].unitPrice;
            if (this.invoiceItems.filter((p) => p.vatAmount > 0).length > 0) {
                this.chargeVATAmount = (this.invoiceCharges[i].unitPrice * 15) / 100;
                this.totalChargeVATAmount += this.chargeVATAmount;
            } else {
                this.totalChargeVATAmount = 0;
            }
        }
    }

    setdescription(event) {
        this.invoiceCharge.description = event.target.value;
    }
    enablecharges(status) {
        if (this.invoiceItems.length == 0) {
            this.notify.error(this.l('Please add atleast one item to add charges to invoice.'));
            this.enablecharge = false;
            return;
        }
        this.enablecharge = status;
    }
    enablearabictab() {
        if (this.enablearabic == true) {
            this.enablearabic = false;
        } else {
            this.enablearabic = true;
        }
    }
    async isRefNumExists() {
        //return false
        // var res = await firstValueFrom(this._salesInvoiceServiceProxy.checkIfRefNumExists(this.invoice.billingReferenceId))
        return false;
    }
    getbuyertype() {
        this._salesInvoiceServiceProxy.getBuyertypelist().subscribe((result) => {
            this.buyertype = result;
        });
    }
    //--------------------Other Charges-----------------------------------


    //---Get Draft Data
    getInvoiceData(id) {
        this._draftInvoiceServiceProxy.getDraftInvoice(id.toString(),this._sessionService.tenantId,this.transtype).subscribe((data) => {
            console.log(data,'getdata');
           // this.invoice=data;
            this.getCustomerInfo(data,'');        
        });
        // this._salesInvoiceServiceProxy.viewInvoice(id.toString(),this.type).subscribe((data) => {
        //     this.getCustomerInfo(data,'');
        //     this.weareyourvendor=data[0].youvendor;
        //     this.delivery_customer = this.deliveryFormGroup?.value ?? null;
        //     this.delivery_customerlang = this.deliveryLangFormGroup?.value ?? null;
        //     this.additional_info = this.additionalFormGroup?.value ?? null;
        //     if (
        //         this.delivery_customer != null &&
        //         this.delivery_customerlang != null &&
        //         this.delivery_customer != undefined &&
        //         this.delivery_customerlang != undefined
        //     ) {
        //         this.getShipmentInfo(data);
        //     }
        //     if (
        //         this.additional_info != null &&
        //         this.additional_info != null &&
        //         this.additional_info != undefined &&
        //         this.additional_info != undefined
        //     ) {
        //         this.getAdditionalInfo(data);
        //     }
        // });
        // this.invoiceItems = [];
    }
    getsalesitemdetail(id,type) {
        this._salesInvoiceServiceProxy.getsalesitemdetail(id,type).subscribe((data) => {
            this.enablecharge = false;
                this.invoiceCharges = [];
                for (let i = 0; i < data.length; i++) {
                    if (this.currencycode == 'SAR') {
                        this.invoiceItem.costPrice = data[i].costPrice;
                        this.invoiceItem.description = data[i].description;
                        this.invoiceItem.excemptionReasonCode = data[i].excemptionReasonCode;
                        this.invoiceItem.excemptionReasonText = data[i].excemptionReasonText;
                        this.invoiceItem.name = data[i].name;
                        this.invoiceItem.discountAmount = data[i].discountAmount;
                        this.invoiceItem.discountPercentage = data[i].discountPercentage;
                        this.invoiceItem.grossPrice = data[i].grossPrice;
                        this.invoiceItem.lineAmountInclusiveVAT = data[i].lineAmountInclusiveVAT;
                        this.invoiceItem.quantity = data[i].quantity;
                        this.invoiceItem.unitPrice = data[i].unitPrice;
                        this.invoiceItem.netPrice = data[i].netPrice;
                        this.invoiceItem.vatAmount = data[i].vatAmount;
                        this.invoiceItem.vatRate = data[i].vatRate;
                        this.invoiceItem.vatCode = data[i].vatRate === 15 ? 'S' : 'Z';
                        this.invoiceItem.isOtherCharges = data[i].isOtherCharges;
                        if (data[i].isOtherCharges) {
                            this.invoiceCharges.push(this.invoiceItem);
                            this.enablecharge = true;
                        } else {
                            this.invoiceItems.push(this.invoiceItem);
                        }
                        this.originalInvoiceItems = JSON.parse(JSON.stringify(this.invoiceItems));
                        this.invoiceCount = this.invoiceItems.length;
                        this.invoiceItem = new CreateOrEditDraftItemDto();
                    } else {
                        this.invoiceItem.costPrice = data[i].costPrice / this.header_Additional1.exchangeRate;
                        this.invoiceItem.description = data[i].description;
                        this.invoiceItem.excemptionReasonCode = data[i].excemptionReasonCode;
                        this.invoiceItem.excemptionReasonText = data[i].excemptionReasonText;
                        this.invoiceItem.name = data[i].name;
                        this.invoiceItem.discountAmount = data[i].discountAmount;
                        this.invoiceItem.discountPercentage = data[i].discountPercentage;
                        this.invoiceItem.grossPrice = data[i].grossPrice / this.header_Additional1.exchangeRate;
                        this.invoiceItem.lineAmountInclusiveVAT =
                            data[i].lineAmountInclusiveVAT / this.header_Additional1.exchangeRate;
                        this.invoiceItem.quantity = data[i].quantity;
                        if (data[i].isOtherCharges) {
                            this.invoiceItem.unitPrice = data[i].unitPrice;
                        } else {
                            this.invoiceItem.unitPrice = data[i].unitPrice / this.header_Additional1.exchangeRate;
                        }
                        this.invoiceItem.netPrice = data[i].netPrice / this.header_Additional1.exchangeRate;
                        this.invoiceItem.vatAmount = data[i].vatAmount / this.header_Additional1.exchangeRate;
                        this.invoiceItem.vatRate = data[i].vatRate;
                        this.invoiceItem.vatCode = data[i].vatRate === 15 ? 'S' : 'Z';
                        this.invoiceItem.isOtherCharges = data[i].isOtherCharges;
                        if (data[i].isOtherCharges) {
                            this.invoiceCharges.push(this.invoiceItem);
                            this.enablecharge = true;
                        } else {
                            this.invoiceItems.push(this.invoiceItem);
                        }
                        this.originalInvoiceItems = JSON.parse(JSON.stringify(this.invoiceItems));
                        this.invoiceCount = this.invoiceItems.length;
                        this.invoiceItem = new CreateOrEditDraftItemDto();
                    }
                }
                this.calculateDraftSummary(this.invoiceSummary, this.invoiceItems, this.othercharges);
                this.otherchargevatcalc();
        });
    }

    otherchargevatcalc() {
        this.totalChargeVATAmount = 0;
          for (let i = 0; i < this.othercharges.length; i++) {
              if (this.invoiceItems.filter((p) => p.vatAmount > 0).length > 0) {
                  this.chargeVATAmount = this.othercharges[i].unitPrice * 0.15;
                  this.totalChargeVATAmount += this.chargeVATAmount;
              } else {
                  this.totalChargeVATAmount = 0;
              }
          }
      }
      calculateDraftSummary(summary, items, charges) {
          summary.totalAmountWithVAT = 0;
          summary.totalAmountWithoutVAT = 0;
          summary.sumOfInvoiceLineNetAmount = 0;
          summary.netInvoiceAmount = 0;
          summary.totalVATAmount = 0;
          this.discount = 0;
          this.totalcharge = 0;
          charges.forEach((charge) => {
              this.totalcharge += charge.unitPrice;
          });
          items.forEach((invoiceItem) => {
              summary.totalAmountWithVAT += invoiceItem.lineAmountInclusiveVAT;
              summary.totalAmountWithoutVAT += invoiceItem.quantity * invoiceItem.unitPrice - invoiceItem.discountAmount;
              summary.sumOfInvoiceLineNetAmount += invoiceItem.netPrice;
              summary.netInvoiceAmount += invoiceItem.quantity * invoiceItem.unitPrice;
              this.discount += invoiceItem.discountAmount;
              summary.totalVATAmount = summary.totalAmountWithVAT - summary.totalAmountWithoutVAT;
          });
      }
    //---end
}

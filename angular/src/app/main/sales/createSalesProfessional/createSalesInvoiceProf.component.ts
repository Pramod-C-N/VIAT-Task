import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomersesServiceProxy, CustomersDto, SalesInvoicesServiceProxy, CreateOrEditSalesInvoiceDto, CreateOrEditSalesInvoicePartyDto, CreateOrEditSalesInvoiceItemDto, CreateOrEditSalesInvoiceSummaryDto, CreateOrEditSalesInvoiceVATDetailDto, CreateOrEditSalesInvoiceDiscountDto, CreateOrEditSalesInvoicePaymentDetailDto, CreateOrEditSalesInvoiceContactPersonDto, SalesInvoiceAddressDto, CreateOrEditSalesInvoiceAddressDto, CountryServiceProxy, GetCountryForViewDto, TenantBasicDetailsServiceProxy, CreateOrEditTenantBasicDetailsDto, ActivecurrencyServiceProxy, TenantConfigurationServiceProxy } from '@shared/service-proxies/service-proxies';
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
import { Theme2ThemeUiSettingsComponent } from '@app/admin/ui-customization/theme2-theme-ui-settings.component';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';

enum Tabs {
  Customer = 1,
  Shipment,
  Additional,
}

@Component({
  templateUrl: './createSalesInvoiceProf.component.html',
  styleUrls: ['./createSalesInvoiceProf.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class CreateSalesInvoiceProfComponent extends AppComponentBase {

  date = new Date();
  month =
      (this.date.getMonth() + 1).toString().length > 1 ? this.date.getMonth() + 1 : '0' + (this.date.getMonth() + 1);
  day = this.date.getDate().toString().length > 1 ? this.date.getDate() : '0' + this.date.getDate();
  maxDate = this.date.getFullYear() + '-' + this.month + '-' + this.day;
  deliveryDetailsModel: any = null;

  additionalDetailsModel: any = null;

  public deliveryFormGroup: FormGroup;
  public deliveryLangFormGroup: FormGroup;
  public deliveryDetailFields = [];
  public deliveryLangDetailFields = [];
  public additionalDetailFields = [];
  private el: ElementRef;
  reasonfilter: any = {};
  takedatafromcus = false;
  countryname: string;
  currencycode: string;
  multilanguage= false;
  editMode = false;
  issueDate = new Date().toISOString().slice(0, 10);
  buyertype: any[]=[];
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

  isExempt: boolean = false;
  exmptReason: string;
  profileType = '';

  discount = 0.0;
  quantity = 0.0;
  alpha3code: string;
  invoice: CreateOrEditSalesInvoiceDto = new CreateOrEditSalesInvoiceDto();
  convertedInvoice: CreateOrEditSalesInvoiceDto = new CreateOrEditSalesInvoiceDto();

  supplier: CreateOrEditSalesInvoicePartyDto = new CreateOrEditSalesInvoicePartyDto();
  customer: CreateOrEditSalesInvoicePartyDto = new CreateOrEditSalesInvoicePartyDto();
  customerLang: CreateOrEditSalesInvoicePartyDto = new CreateOrEditSalesInvoicePartyDto();

  invoiceItems: CreateOrEditSalesInvoiceItemDto[] = [];
  invoiceItem: CreateOrEditSalesInvoiceItemDto = new CreateOrEditSalesInvoiceItemDto();

  invoiceCharges: CreateOrEditSalesInvoiceItemDto[] = [];
  invoiceCharge: CreateOrEditSalesInvoiceItemDto = new CreateOrEditSalesInvoiceItemDto();

  invoiceSummary: CreateOrEditSalesInvoiceSummaryDto = new CreateOrEditSalesInvoiceSummaryDto();
  vatDetails: CreateOrEditSalesInvoiceVATDetailDto[] = [];
  vatDetail: CreateOrEditSalesInvoiceVATDetailDto = new CreateOrEditSalesInvoiceVATDetailDto();

  discountDetails: CreateOrEditSalesInvoiceDiscountDto[] = [];
  discountDetail: CreateOrEditSalesInvoiceDiscountDto = new CreateOrEditSalesInvoiceDiscountDto();

  paymentDetails: CreateOrEditSalesInvoicePaymentDetailDto[] = [];
  paymentDetail: CreateOrEditSalesInvoicePaymentDetailDto = new CreateOrEditSalesInvoicePaymentDetailDto();
  tenants: CreateOrEditTenantBasicDetailsDto = new CreateOrEditTenantBasicDetailsDto();

  address: CreateOrEditSalesInvoiceAddressDto = new CreateOrEditSalesInvoiceAddressDto();
  addressLang: CreateOrEditSalesInvoiceAddressDto = new CreateOrEditSalesInvoiceAddressDto();
  customers: CustomersDto[] = new Array<CustomersDto>();
  filteredCustomers: any[]=[];
  filteredCustomerAddress: any[]=[];
  countries: any[] = [];
  isSaving = false;
  basicForm: FormGroup;
  public multilangform: FormGroup;
  isExport = false;
  enablecharge = false;
  enablearabic= true;
  chargeVATAmount = 0;
  totalChargeVATAmount = 0;

  //--------------------brady-----------------------------

  header_Additional1: any = {
      exchangeRate: 1.0,
  };

  currentStep: Tabs = Tabs.Customer;
  showBack: boolean = false;
  showNext: boolean = false;

  delivery_customer: any = {};
  additional_info: any = {};

  summary_Additional1: any = {
      val1: 0.0,
      val2: 0.0,
      totalOther: 0.0,
  };

  activeCurrency: any[] = [];
  defaultcurrency: any[] = [];
  exemptionReason: any[] = [];
  filterdcountries: any[] = [];
  filterdcurrencies: any[] = [];
  filterdname: any[] = [];
  filterdcurrency: any[] = [];
  filterdalpha3code: any[] = [];
  availableTabs: Tabs[] = [Tabs.Customer];
  isAutoCompleteDisabled = false;
  additionalFormGroup: FormGroup<{}>;
  language: number = 1;
  //--------------------brady-----------------------------

  constructor(
      private fb: FormBuilder,
      injector: Injector,
      private _activeCurrency: ActivecurrencyServiceProxy,
      private _salesInvoiceServiceProxy: SalesInvoicesServiceProxy,
      private _masterCountriesServiceProxy: CountryServiceProxy,
      private _tenantbasicdetailsServiceProxy: TenantBasicDetailsServiceProxy,
      private _sessionService: AppSessionService,
      private router: Router,
      private _tenantConfigurations: TenantConfigurationServiceProxy,
      private _customerServiceProxy: CustomersesServiceProxy
  ) {
      super(injector);
      this.salesForm();
  }

  salesForm() {
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
  //--------------------brady-----------------------------

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
      if (val == '') return 0;
      else return val;
  }

  calculateTotalOtherCharge() {
      this.summary_Additional1.totalOther =
          this.checkIfEmpty(this.summary_Additional1.val1) + this.checkIfEmpty(this.summary_Additional1.val2);
  }

  getActiveCurrency(alpha3Code: any) {
      this.alpha3code = alpha3Code;
      this._activeCurrency.getActiveCurrencies(alpha3Code).subscribe((e) => {
          this.activeCurrency = e;
          this.defaultcurrency = this.activeCurrency.filter((p) => p.entity == alpha3Code);
          if (this.defaultcurrency.length != 0) {
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
      this.calculateTotalOtherCharge();
  }
  //--------------------brady-----------------------------

  isFormValid() {
      return this.basicForm.valid;
  }

  // eslint-disable-next-line @angular-eslint/use-lifecycle-interface
   ngOnInit() {
      this.address.countryCode = null;
      this.exmptReason = null;
      this.getCountriesDropdown();
       this.getbuyertype();
      //this.getActiveCurrency('SA');
      this.create();
      this._tenantConfigurations.getTenantConfigurationByTransactionType('Sales').subscribe((e) => {
          if (e.tenantConfiguration.shipmentJson) {
              this.deliveryDetailsModel = JSON.parse(e.tenantConfiguration.shipmentJson);
              this.deliveryFormGroup = this.buildForm(this.deliveryDetailsModel, this.deliveryDetailFields);
              this.deliveryLangFormGroup = this.buildForm(this.deliveryDetailsModel, this.deliveryLangDetailFields);
              this.availableTabs.push(Tabs.Shipment);
              this.showNext = true;
          }

          if (e.tenantConfiguration.additionalFieldsJson) {
              this.additionalDetailsModel = JSON.parse(e.tenantConfiguration.additionalFieldsJson);
              this.additionalFormGroup = this.buildForm(this.additionalDetailsModel, this.additionalDetailFields);
              this.availableTabs.push(Tabs.Additional);
              this.showNext = true;
          }
      });

      this._tenantConfigurations.getTenantConfigurationByTransactionType('General').subscribe((e) => {
          if(e.tenantConfiguration.language)
          {
              if(e.tenantConfiguration.language == 'dual')
              {
                  this.multilanguage=true;
              }
          }
      });
      //this.onSelectCustomer();
      //this.getProducts();
      this.invoiceSummary.totalAmountWithVAT = 0;
      this.invoiceSummary.totalAmountWithoutVAT = 0;
      this.invoiceSummary.sumOfInvoiceLineNetAmount = 0;
      this.invoiceSummary.netInvoiceAmount = 0;
      this.invoiceItem.vatCode = 'S';
      this.discount = 0;
      this.invoiceItem.quantity = 0;
      this.invoiceItem.discountPercentage = 0;
      this.invoiceItem.uom = 'PC';
      this.supplier.registrationName = this._sessionService.tenant.name;
      this.supplier.contactPerson.email = this._sessionService.user.emailAddress;
      this.invoice.customerId = '567';
      this.invoice.invoiceCurrencyCode = 'SAR';

  }
  //get countries from master data
  getCountriesDropdown() {
      this._masterCountriesServiceProxy.getCountriesList().subscribe((result) => {
          console.log(result);
          this.countries = result;
      });
  }
  //--------------------------------------auto complete starts---------------------------------------------------------

  filterCustomers(event): void {
       this._customerServiceProxy.getCustomerName(event.query).subscribe((data) => {
             this.filteredCustomers = data;
              if (data.length == 0) {
               this.customer.registrationName = event.query;
             }
            });
  }

  onSelectCustomer(event) {
     // this.filteredCustomerAddress=this.filteredCustomers.filter(p=>p.name == event.query);
     this.customer.registrationName=event.name;
      this.customer.vatid=event.vatid;
      this.customer.customerId=event.customerId;
      this.address.buildingNo=event.buildingNo;
      this.address.street=event.street;
      this.address.additionalStreet=event.additionalstreet;
      this.address.neighbourhood=event.neighbourhood;
      this.address.city=event.city;
      this.address.state=event.state;
      this.address.countryCode=event.countryCode;
      this.countryname=this.countries.find((p=>p.alphaCode == event.countryCode)).name;
      this.selectOption(this.countryname);
      this.address.postalCode=event.postalCode;
      this.customer.contactPerson.contactNumber=event.contactNumber;
      this.customer.contactPerson.email=event.email;
      // this._tenantbasicdetailsServiceProxy.getTenantById(this._sessionService.tenant.id).subscribe((data) => {
      //     console.log(data, 'data');
      //     this.address.city = data[0].city?.trim();
      //     this.address.state = data[0].state;
      //     this.address.countryCode = data[0].country;
      //     this.address.postalCode = data[0].postalCode;
      //     //supplier
      //     this.supplier.address.city = data[0].city;
      //     this.supplier.address.state = data[0].state;
      //     this.supplier.address.countryCode = data[0].country;
      //     this.supplier.address.postalCode = data[0].postalCode;
      //     this.supplier.address.additionalNo = data[0].additionalNo;
      //     this.supplier.address.additionalStreet = data[0].additionalStreet;
      //     this.supplier.address.buildingNo = data[0].buildingNo;
      //     this.supplier.address.neighbourhood = data[0].neighbourhood;
      //     this.supplier.address.street = data[0].street;
      //     this.supplier.vatid = data[0].vatid;
      //     this.supplier.crNumber = data[0].crNumber;
      //     this.supplier.contactPerson.contactNumber = data[0].contactNumber;
      // });
  }
  changedata(event) {
      this.customer.registrationName = event.registrationName;
  }
  //--------------------------------------auto complete ends---------------------------------------------------------

  create() {
      this.customer.contactPerson = new CreateOrEditSalesInvoiceContactPersonDto();
      this.supplier.contactPerson = new CreateOrEditSalesInvoiceContactPersonDto();
      this.supplier.address = new CreateOrEditSalesInvoiceAddressDto();
      this.customerLang.contactPerson=new CreateOrEditSalesInvoiceContactPersonDto();
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
      if (this.invoiceItem.vatCode == 'S') {
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
      // if (this.invoiceItem.vatRate != 15 || this.invoiceItem.vatRate != 0) {
      //     this.notify.error(this.l('VAT rate error'));
      //     return;
      // }

      //   if ( this.invoiceItem.discountPercentage >= 15|| this.invoiceItem.discountPercentage<=100 ) {
      //     this.notify.error(this.l('Discount rate error'));
      //   return;
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
      //    * this.header_Additional1.exchangeRate;
      this.invoiceItem.netPrice = this.invoiceItem.lineAmountInclusiveVAT - this.invoiceItem.vatAmount;
      this.invoiceItem.currencyCode = 'SAR';
      this.invoiceItem.identifier = 'Sales';
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

   editItem(index: number) {
      this.invoiceItem = this.invoiceItems[index];
      this.onVatChange();
      this.invoiceItems.splice(index, 1);
      this.calculateInvoiceSummary(this.invoiceSummary, this.invoiceItems);
  }

  deleteItem(index: number) {
      this.invoiceItems.splice(index, 1);
      if(this.invoiceItems.length == 0)
      {
          this.isAutoCompleteDisabled=false;
      }
      this.vatDetail = new CreateOrEditSalesInvoiceVATDetailDto();
      this.calculateInvoiceSummary(this.invoiceSummary, this.invoiceItems);
  }

  clearItem() {
      this.invoiceItem = new CreateOrEditSalesInvoiceItemDto();
      this.invoiceItem.vatCode = 'S';
      this.invoiceItem.uom = 'PC';
      this.onVatChange();
      //this.product = new CreateOrEditProductDto();
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

  //

  fillDummy() {
      this.invoice.status = 'Paid';
      this.invoice.paymentType = 'Cash';
      this.invoice.dateOfSupply = DateTime.utc();
      //this.invoice.invoiceCurrencyCode = 'SAR';
      //this.invoice.buyer.crNumber = '1234567890';
      this.invoice.invoiceSummary.currencyCode = 'SAR';
      this.invoice.invoiceSummary.paidAmountCurrency = 'SAR';
      this.invoice.invoiceSummary.payableAmountCurrency = 'SAR';
      this.invoice.invoiceSummary.netInvoiceAmountCurrency = 'SAR';
      this.invoice.invoiceSummary.totalAmountWithoutVATCurrency = 'SAR';
      this.invoice.invoiceNumber = '-1'; //!this.invoice.billingReferenceId ? " " : this.invoice.billingReferenceId
     // this.invoice.buyer.vatid = !this.customer.vatid?.trim() ? ' ' : this.customer.vatid;
      // this.invoice.buyer.contactPerson.name = !this.customer.registrationName?.trim()
      //     ? 'NA'
      //     : this.customer.registrationName;
      //  this.invoice.buyer.contactPerson.type = !this.customer.contactPerson.type?.trim() ? "NA" : this.customer.contactPerson.type;
     // this.invoice.buyer.address.type = !this.address.type?.trim() ? ' ' : this.address.type;
      //this.invoice.buyer.address.additionalNo = !this.address.additionalNo?.trim() ? ' ' : this.address.additionalNo;
     // this.invoice.buyer.address.city = !this.invoice.buyer.address.city?.trim()
       //   ? ' '
     //     : this.invoice.buyer.address.city;
      // this.invoice.buyer.address.countryCode = !this.invoice.buyer.address.countryCode?.trim()
      //     ? ' '
      //     : this.invoice.buyer.address.countryCode;
      // this.invoice.buyer.address.postalCode = !this.invoice.buyer.address.postalCode?.trim()
      //     ? ''
      //     : this.invoice.buyer.address.postalCode;
      // this.invoice.buyer.address.state = !this.invoice.buyer.address.state?.trim()
      //     ? ''
      //     : this.invoice.buyer.address.state;
      // this.invoice.buyer.address.street = !this.invoice.buyer.address.street?.trim()
      //     ? ' '
      //     : this.invoice.buyer.address.street;
      // this.invoice.buyer.address.state = !this.invoice.buyer.address.state?.trim()
      //     ? ' '
      //     : this.invoice.buyer.address.state;
      // this.invoice.buyer.address.buildingNo = !this.invoice.buyer.address.buildingNo?.trim()
      //     ? ''
      //     : this.invoice.buyer.address.buildingNo;
      // this.invoice.buyer.address.neighbourhood = !this.invoice.buyer.address.neighbourhood?.trim()
      //     ? ' '
      //     : this.invoice.buyer.address.neighbourhood;
      // this.invoice.buyer.contactPerson.email = !this.customer.contactPerson.email?.trim()
      //     ? ' '
      //     : this.customer.contactPerson.email;
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

  validateFields() {
      return (
          this.address.buildingNo &&
          this.address.city &&
          this.address.countryCode &&
          this.address.street &&
          this.address.postalCode &&
          this.address.state &&
          this.address.neighbourhood &&
          this.invoice.issueDate
      );
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
      // this.calculateTotalOtherCharge()
  }

  async save() {

      if (await this.isRefNumExists()) {
          this.notify.error(this.l('Entered Reference Number already exists'));
          return null;
      }
     // this.delivery_customer = this.deliveryFormGroup.value;
   //   this.additional_info = this.additionalFormGroup.value;


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
          //language
          this.customerLang.customerId=this.multilangform.get('CustomerId').value;
          this.customerLang.registrationName=this.multilangform.get('Name').value;
          this.customerLang.vatid=this.multilangform.get('vatid').value;
          this.addressLang.buildingNo= this.multilangform.get('Buildno').value;
          this.addressLang.street=this.multilangform.get('street').value;
          this.addressLang.additionalStreet=this.multilangform.get('additionalstreet').value;
          this.addressLang.city=this.multilangform.get('city').value;
          this.addressLang.neighbourhood=this.multilangform.get('Neighbourhood').value;
          this.addressLang.state=this.multilangform.get('state').value;
          this.addressLang.countryCode=this.multilangform.get('nationality').value;
          this.customerLang.contactPerson.name=this.multilangform.get('billToAttn')?.value;
          this.customerLang.contactPerson.contactNumber=this.multilangform.get('ContactNo').value;
          this.customerLang.contactPerson.email=this.multilangform.get('Email').value;
          this.addressLang.postalCode = this.multilangform.get('pin').value;
          this.customerLang.otherDocumentTypeId = this.multilangform.get('buyertype').value;
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
          this.invoice.additionalData2 = JSON.stringify([this.additional_info]);

      //  this.invoice.buyer[0].additionalData1 = JSON.stringify([this.deliveryFormGroup.value]);
    
          if(this.multilanguage==true && this.deliveryLangFormGroup != undefined)
          {
          this.invoice.buyer[1].additionalData1 = JSON.stringify([this.deliveryLangFormGroup.value]);
          }
          //----------------------------------------------------------
          this.invoice.vatDetails = this.vatDetails;
          console.log(this.invoice);
          this.convertedInvoice = JSON.parse(JSON.stringify(this.invoice));
          this.ConvertToSAR();

          console.log(this.invoice);
          if (this.invoiceCharges.length > 0) {
              for (let j = 0; j < this.invoiceCharges.length; j++) {
                  this.invoiceCharges[j].isOtherCharges = true;
                  this.convertedInvoice.items.push(JSON.parse(JSON.stringify(this.invoiceCharges[j])));
              }
          }
          this.basicForm.markAsUntouched();
          this._salesInvoiceServiceProxy.createSalesInvoice(this.convertedInvoice).subscribe((result) => {
              // this._salesInvoiceServiceProxy.insertSalesReportData(Number(result.invoiceId)).subscribe((result) => {
              // });
              this.notify.success(this.l('SavedSuccessfully'));
              this.editMode = false;
              this.isSaving = false;
              this.router.navigate(['/app/main/transactions'], { state: { tabvaule: 'Sales Invoice' } });
              //this.download(result.invoiceId, result.uuid);
          });
      } else {
          this.basicForm.markAllAsTouched();
          window.scroll(0, 0);
          this.currentStep = 1;
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
      if (takedatafromcus == true) {
          // this.deliveryFormGroup.setValue({
          //     registrationName:this.customer.registrationName
          // })
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
          this.deliveryFormGroup
              .get('contactPerson.contactNumber')
              .setValue(this.customer.contactPerson.contactNumber);
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
          if (field == 'isChildGroup') continue;
          const fieldProps = model[field];
          if (fieldProps?.isChildGroup) {
              var children = this.getFormControlsFields(fieldProps, fields, field);
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
      this.invoiceCharge = new CreateOrEditSalesInvoiceItemDto();
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
  enablearabictab(){
      if(this.enablearabic == true)
      {
      this.enablearabic=false;
      }
      else
      {
          this.enablearabic=true;

      }
  }
  //--------------------Other Charges-----------------------------------

  async isRefNumExists(){
      //return false 
      // var res = await firstValueFrom(this._salesInvoiceServiceProxy.checkIfRefNumExists(this.invoice.billingReferenceId))
      return false;
  }


   getbuyertype(){
       this._salesInvoiceServiceProxy.getBuyertypelist().subscribe((result) => {
          this.buyertype = result;
      });
  }

}

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomersesServiceProxy, CustomersDto, SalesInvoicesServiceProxy, CreateOrEditSalesInvoiceDto, CreateOrEditSalesInvoicePartyDto, CreateOrEditSalesInvoiceItemDto, CreateOrEditSalesInvoiceSummaryDto, CreateOrEditSalesInvoiceVATDetailDto, CreateOrEditSalesInvoiceDiscountDto, CreateOrEditSalesInvoicePaymentDetailDto, CreateOrEditSalesInvoiceContactPersonDto, SalesInvoiceAddressDto, CreateOrEditSalesInvoiceAddressDto, CountryServiceProxy, GetCountryForViewDto, TenantBasicDetailsServiceProxy, CreateOrEditTenantBasicDetailsDto } from '@shared/service-proxies/service-proxies';
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
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  templateUrl: './createSalesInvoice.component.html',
  styleUrls: ['./createSalesInvoice.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class CreateSalesInvoiceComponent extends AppComponentBase {

  date = new Date();
  month = (this.date.getMonth() + 1).toString().length > 1 ? this.date.getMonth() + 1 : "0" + (this.date.getMonth() + 1)
  day = (this.date.getDate()).toString().length > 1 ? this.date.getDate() : "0" + (this.date.getDate())
  maxDate = this.date.getFullYear() + "-" + this.month + "-" + this.day;
  editMode: boolean = false;
  issueDate=new Date().toISOString().slice(0, 10);

  profileType: string = "";

  discount: number = 0.0
  quantity: number = 0.0
  invoice: CreateOrEditSalesInvoiceDto = new CreateOrEditSalesInvoiceDto();
  supplier: CreateOrEditSalesInvoicePartyDto = new CreateOrEditSalesInvoicePartyDto();
  customer: CreateOrEditSalesInvoicePartyDto = new CreateOrEditSalesInvoicePartyDto();


  invoiceItems: CreateOrEditSalesInvoiceItemDto[] = [];
  invoiceItem: CreateOrEditSalesInvoiceItemDto = new CreateOrEditSalesInvoiceItemDto();

  invoiceSummary: CreateOrEditSalesInvoiceSummaryDto = new CreateOrEditSalesInvoiceSummaryDto();
  vatDetails: CreateOrEditSalesInvoiceVATDetailDto[] = [];
  vatDetail: CreateOrEditSalesInvoiceVATDetailDto = new CreateOrEditSalesInvoiceVATDetailDto();

  discountDetails: CreateOrEditSalesInvoiceDiscountDto[] = [];
  discountDetail: CreateOrEditSalesInvoiceDiscountDto = new CreateOrEditSalesInvoiceDiscountDto();

  paymentDetails: CreateOrEditSalesInvoicePaymentDetailDto[] = [];
  paymentDetail: CreateOrEditSalesInvoicePaymentDetailDto = new CreateOrEditSalesInvoicePaymentDetailDto();
  tenants:CreateOrEditTenantBasicDetailsDto=new CreateOrEditTenantBasicDetailsDto();

  //product: CreateOrEditProductDto = new CreateOrEditProductDto();

  //products: GetProductForViewDto[] = [];
  //filteredProducts: GetProductForViewDto[] = [];

  address: CreateOrEditSalesInvoiceAddressDto = new CreateOrEditSalesInvoiceAddressDto();
  customers: CustomersDto[] = new Array<CustomersDto>();
  filteredCustomers: CustomersDto[];
  countries: GetCountryForViewDto[] = [];
  isSaving: boolean = false;
  basicForm: FormGroup
  



  constructor(
    private fb: FormBuilder,
    injector: Injector,
    private _customerServiceProxy: CustomersesServiceProxy,
    // private _productsServiceProxy: ProductsServiceProxy,
    private _salesInvoiceServiceProxy: SalesInvoicesServiceProxy,
    // private _invoiceHeaderServiceProxy: InvoiceHeadersServiceProxy,
    private _masterCountriesServiceProxy: CountryServiceProxy,
    private _tenantbasicdetailsServiceProxy : TenantBasicDetailsServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService,
    private _dateTimeService: DateTimeService,
    private _sessionService: AppSessionService,
    private router : Router

  ) {
    super(injector);
    this.salesForm();
  }

  salesForm() {
    this.basicForm = this.fb.group({
      Name:['',Validators.required],
      Buildno: ['', [Validators.required,
        Validators.maxLength(4),
      Validators.pattern("^[0-9]*$") ]],      
      street: ['', Validators.required ],      
      Neighbourhood:['',Validators.required],
      pin: ['', [Validators.required,
        Validators.maxLength(5),
    Validators.pattern("^[0-9]*$") ] ],
      city: ['', Validators.required ],
      state: ['', Validators.required ],
      nationality: ['', Validators.required ],
      ContactNo: ['', [Validators.required, Validators.pattern("^[0-9]*$")] ],
      vatid:['', [Validators.required,
        Validators.maxLength(15),
        Validators.minLength(15),
      Validators.pattern("^3[0-9]*3$")]]
    })
  }

  isFormValid() {
    return (this.basicForm.valid);
  }
  //ngoninit

  ngOnInit(): void {
    this.address.countryCode=null;
    this.getCountriesDropdown();
    this.create();
    this.onSelectCustomer();
    //this.getProducts();
    this.invoiceSummary.totalAmountWithVAT = 0
    this.invoiceSummary.totalAmountWithoutVAT = 0
    this.invoiceSummary.sumOfInvoiceLineNetAmount = 0
    this.invoiceSummary.netInvoiceAmount = 0
    this.discount = 0
    this.invoiceItem.quantity = 0
    this.invoiceItem.discountPercentage = 0;
    this.invoiceItem.vatRate=15;
    this.supplier.registrationName = this._sessionService.tenant.name;
    this.supplier.vatid = "5672345"
    this.supplier.contactPerson.email = this._sessionService.user.emailAddress
    this.supplier.contactPerson.contactNumber = "567898084"
    this.invoice.customerId = "567"

    //get all invoice headers
    this._salesInvoiceServiceProxy.getAll("", "", "", undefined, undefined, undefined, undefined, "", "", "", "", "", undefined, undefined, "", "", "", "", 0, 0, 0, "", "", "", "", "", "", 0, 1234567).subscribe((result) => {
      console.log(result);
    });


  }
  //get countries from master data
  getCountriesDropdown() {
    this._masterCountriesServiceProxy.getAll("", "", "", "", "", undefined, undefined, "", "", undefined, undefined, undefined, undefined, 200)
      .subscribe(result => {
        this.countries = result.items;
        console.warn(this.countries);
      });
  }
  //--------------------------------------auto complete starts---------------------------------------------------------

  // filterCustomers(event): void {
  //   this._customerServiceProxy.getCustomerName(event.query).subscribe((data) => {
  //     this.filteredCustomers = data;
  //     if (data.length == 0) {
  //       this.customer.registrationName = event.query;
  //     }
  //   });
  // }

  onSelectCustomer() {
    this._tenantbasicdetailsServiceProxy.getTenantById(this._sessionService.tenant.id).subscribe((data) => {       
        this.address.city = data[0].city?.trim();
        this.address.state = data[0].state;
        this.address.countryCode = data[0].country;
        this.address.postalCode = data[0].postalCode;
      });
  }
  changedata(event) {
    this.customer.registrationName = event.registrationName
  }
  //--------------------------------------auto complete ends---------------------------------------------------------




  // getProducts(): void {

  //   this._productsServiceProxy
  //     .getAll("", "", "", "", "", "", "", "", undefined, undefined, undefined, undefined, "", undefined, undefined, "", undefined, undefined, undefined
  //     )
  //     .subscribe((result) => {
  //       this.products = result.items;
  //     });
  // }


  // updateProduct(i: number) {

  //   this.product = this.filteredProducts[i].product;

  //   this.invoiceItem.name = this.product.name;
  //   this.invoiceItem.unitPrice = this.product.unitPrice;
  //   this.invoiceItem.costPrice = this.product.costPrice;
  //   this.invoiceItem.description = this.product.description;
  //   this.invoiceItem.uom = this.product.uom;

  // }

  // filterProduct() {
  //   let name = this.product.name
  //   this.filteredProducts = JSON.parse(JSON.stringify(this.products)).filter(function (str) {
  //     return str.product.name.indexOf(name || "") === -1;
  //   });
  // }


  create() {
    this.customer.contactPerson = new CreateOrEditSalesInvoiceContactPersonDto();
    this.supplier.contactPerson = new CreateOrEditSalesInvoiceContactPersonDto();
    // this.invoice.buyer.contactPerson = new CreateOrEditSalesInvoiceContactPersonDto();

    // this.invoiceItem.
  }


  //add item to invoiceitems
  addItem() {


    if (this.invoiceItem.quantity < 0) {
      this.message.warn("Quantity cannot be less than 0 ");
      return;
    }
    if (this.invoiceItem.unitPrice < 0) {
      this.message.warn("Rate cannot be less than 0 ");
      return;
    }
    if (this.invoiceItem.discountPercentage < 0 || this.invoiceItem.discountPercentage > 100) {
      this.message.warn("Discount must be in between 0 to 100");
      return;
    }
    if (!(this.invoiceItem.name && this.invoiceItem.description && this.invoiceItem.quantity
      && this.invoiceItem.unitPrice)) {

      this.notify.error(this.l('Please fill all required fields to add item to invoice.'));
      return;
    }
    if (this.invoiceItem.vatRate != 15 && this.invoiceItem.vatRate != 0) {
      this.notify.error(this.l('VAT rate error'));
      return;
    }

    if(this.address.countryCode!='SA' && this.invoiceItem.vatRate!=0)
    {
      this.notify.error(this.l('Exports are exempt from VAT'));
      return;
    }
    //   if ( this.invoiceItem.discountPercentage >= 15|| this.invoiceItem.discountPercentage<=100 ) {
    //     this.notify.error(this.l('Discount rate error'));
    //   return; 
    // }
    this.invoiceItem.discountAmount = this.invoiceItem.quantity * this.invoiceItem.unitPrice * this.invoiceItem.discountPercentage / 100.0
    this.invoiceItem.vatAmount = ((this.invoiceItem.quantity * this.invoiceItem.unitPrice) - this.invoiceItem.discountAmount) * this.invoiceItem.vatRate / 100.0
    this.invoiceItem.lineAmountInclusiveVAT = ((this.invoiceItem.quantity * this.invoiceItem.unitPrice) - this.invoiceItem.discountAmount) + this.invoiceItem.vatAmount
    this.invoiceItem.netPrice = this.invoiceItem.lineAmountInclusiveVAT - this.invoiceItem.vatAmount
    this.invoiceItem.currencyCode = "SAR"
    this.invoiceItem.identifier = "Sales"
    this.invoiceItem.vatCode = this.invoiceItem.vatRate == 15 ? "S" : "Z"
    this.invoiceItem.uom = "1"
    this.invoiceItems.push(this.invoiceItem);
    this.invoiceItem = new CreateOrEditSalesInvoiceItemDto();
    this.calculateInvoiceSummary()
    this.invoiceItem.quantity = 0
    this.invoiceItem.discountPercentage = 0;
    this.invoiceItem.vatRate=15;
  }

  deleteItem(index: number) {
    this.invoiceItems.splice(index, 1);

    this.calculateInvoiceSummary()
  }

  clearItem() {
    this.invoiceItem = new CreateOrEditSalesInvoiceItemDto();
    //this.product = new CreateOrEditProductDto();  
    this.invoiceItem.discountPercentage = 0;
  }


  calculateInvoiceSummary() {

    this.invoiceSummary.totalAmountWithVAT = 0
    this.invoiceSummary.totalAmountWithoutVAT = 0
    this.invoiceSummary.sumOfInvoiceLineNetAmount = 0
    this.invoiceSummary.netInvoiceAmount = 0
    this.invoiceSummary.totalVATAmount=0
    this.discount = 0

    this.invoiceItems.forEach(invoiceItem => {
      this.invoiceSummary.totalAmountWithVAT += invoiceItem.lineAmountInclusiveVAT
      this.invoiceSummary.totalAmountWithoutVAT += (invoiceItem.quantity * invoiceItem.unitPrice - invoiceItem.discountAmount)
      this.invoiceSummary.sumOfInvoiceLineNetAmount += invoiceItem.netPrice
      this.invoiceSummary.netInvoiceAmount += invoiceItem.quantity * invoiceItem.unitPrice
      this.discount += invoiceItem.discountAmount
      this.invoiceSummary.totalVATAmount = this.invoiceSummary.totalAmountWithVAT - this.invoiceSummary.totalAmountWithoutVAT
    })



  }

  //


  fillDummy() {
    this.invoice.status = "Paid"
    this.invoice.paymentType = "Cash"
    this.invoice.dateOfSupply = DateTime.utc();

    this.invoice.invoiceCurrencyCode = "SAR"
    this.invoice.buyer.crNumber = "1234567890"
    this.invoice.supplier.crNumber = "1234567890";
    this.invoice.supplier.vatid = "5672345";
    this.invoice.invoiceSummary.currencyCode = "SAR";
    this.invoice.invoiceSummary.paidAmountCurrency = "SAR";
    this.invoice.invoiceSummary.payableAmountCurrency = "SAR";
    this.invoice.invoiceSummary.netInvoiceAmountCurrency = "SAR";
    this.invoice.invoiceSummary.totalAmountWithoutVATCurrency = "SAR";
    this.invoice.invoiceNumber = '-1' //!this.invoice.billingReferenceId ? " " : this.invoice.billingReferenceId 

    this.invoice.supplier.contactPerson.name = !this.invoice.supplier.contactPerson.name ? "Admin" : this.invoice.supplier.contactPerson.name;
    this.invoice.supplier.contactPerson.contactNumber = !this.invoice.supplier.contactPerson.contactNumber ? "567898084" : this.invoice.supplier.contactPerson.contactNumber;
    this.invoice.supplier.address.city = !this.invoice.supplier.address.city ? "Bengaluru" : this.invoice.supplier.address.city;
    // this.invoice.supplier.contactPerson.type = !this.customer.contactPerson.type?.trim() ? "NA" : this.customer.contactPerson.type;
    this.invoice.supplier.address.type = !this.invoice.supplier.address.type?.trim() ? "NA" : this.invoice.supplier.address.type;
    this.invoice.supplier.address.additionalNo = !this.invoice.supplier.address.additionalNo ? "Gurappa Ave" : this.invoice.supplier.address.additionalNo;
    this.invoice.supplier.address.countryCode = !this.invoice.supplier.address.countryCode ? "UAE" : this.invoice.supplier.address.countryCode;
    this.invoice.supplier.address.postalCode = !this.invoice.supplier.address.postalCode ? "560025" : this.invoice.supplier.address.postalCode;
    this.invoice.supplier.address.state = !this.invoice.supplier.address.state ? "Karnataka" : this.invoice.supplier.address.state;
    this.invoice.supplier.address.street = !this.invoice.supplier.address.street ? "Diamond House, OFF" : this.invoice.supplier.address.street;
    this.invoice.supplier.address.buildingNo = !this.invoice.supplier.address.buildingNo ? "11" : this.invoice.supplier.address.buildingNo;
    this.invoice.supplier.address.neighbourhood = !this.invoice.supplier.address.neighbourhood ? "Ashok Nagar" : this.invoice.supplier.address.neighbourhood;
    this.invoice.buyer.vatid = !this.customer.vatid?.trim() ? "NA" : this.customer.vatid;
    this.invoice.buyer.contactPerson.name = !this.customer.registrationName?.trim() ? "NA" : this.customer.registrationName;
    //  this.invoice.buyer.contactPerson.type = !this.customer.contactPerson.type?.trim() ? "NA" : this.customer.contactPerson.type;
    this.invoice.buyer.address.type = !this.address.type?.trim() ? "NA" : this.address.type;
    this.invoice.buyer.address.additionalNo = !this.address.additionalNo?.trim() ? "NA" : this.address.additionalNo;
    this.invoice.buyer.address.city = !this.invoice.buyer.address.city?.trim() ? "NA" : this.invoice.buyer.address.city;
    this.invoice.buyer.address.countryCode = !this.invoice.buyer.address.countryCode?.trim() ? "NA" : this.invoice.buyer.address.countryCode;
    this.invoice.buyer.address.postalCode = !this.invoice.buyer.address.postalCode?.trim() ? "NA" : this.invoice.buyer.address.postalCode;
    this.invoice.buyer.address.state = !this.invoice.buyer.address.state?.trim() ? "NA" : this.invoice.buyer.address.state;
    this.invoice.buyer.address.street = !this.invoice.buyer.address.street?.trim() ? "NA" : this.invoice.buyer.address.street;
    this.invoice.buyer.address.state = !this.invoice.buyer.address.state?.trim() ? "NA" : this.invoice.buyer.address.state;
    this.invoice.buyer.address.buildingNo = !this.invoice.buyer.address.buildingNo?.trim() ? "NA" : this.invoice.buyer.address.buildingNo;
    this.invoice.buyer.address.neighbourhood = !this.invoice.buyer.address.neighbourhood?.trim() ? "NA" : this.invoice.buyer.address.neighbourhood;
    this.invoice.buyer.contactPerson.email = !this.customer.contactPerson.email?.trim() ? "NA" : this.customer.contactPerson.email;
  }

  parseDate(dateString: string): DateTime {

    var time = new Date().toLocaleTimeString();
    var date = new Date(dateString);

    if (time.includes("PM")) {
      date.setHours(parseInt(time.split(":")[0]) + 12)
    } else {
      date.setHours(parseInt(time.split(":")[0]));
    } date.setMinutes(parseInt(time.split(":")[1]));
    date.setSeconds(parseInt(time.split(":")[2]));
    dateString = date.toISOString();


    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());

    }
    return null;
  }

  validateFields() {

    return (this.address.buildingNo && this.address.city && this.address.countryCode && this.address.street &&
      this.address.postalCode && this.address.state && this.address.neighbourhood)
  }


  save() {

    if(this.address.neighbourhood==null || this.address.neighbourhood==undefined)
    {
      this.basicForm.get('Neighbourhood').setValue('.');
    }

    var isSimplified = this.invoiceSummary.totalAmountWithVAT < 1000
    if (parseInt(this.address.buildingNo) > 9999 && !isSimplified) {
      this.notify.error(this.l('Building number cannot be greater than 4 digits'));
      this.isSaving = false;

      return;
    }
    if ((parseInt(this.address.postalCode) > 99999 || parseInt(this.address.postalCode) <= 9999) && !isSimplified) {
      this.notify.error(this.l('Please enter a valid postal code.'));
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

    if (!isSimplified && !this.validateFields()) {
      this.notify.error(this.l('Please add required fields.'));
      this.isSaving = false;

      return;
    }    
  
    if (this.isFormValid()) {
      
      this.invoice.invoiceSummary = this.invoiceSummary
      this.invoice.items = this.invoiceItems
      this.invoice.supplier = this.supplier
      this.invoice.buyer = this.customer


      try {
        this.invoice.issueDate = this.parseDate(this.issueDate.toString())
      } catch (e) {
        this.notify.error(this.l('Please enter valid issue date.'));
        this.isSaving = false;
        return

      }
      this.isSaving = true;
      this.invoice.supplier.address = new CreateOrEditSalesInvoiceAddressDto()
      this.invoice.buyer.address = this.address



      this.fillDummy();
      console.log(this.invoice)
      this._salesInvoiceServiceProxy.createSalesInvoice(this.invoice)
        .subscribe((result) => {
          // this._salesInvoiceServiceProxy.insertSalesReportData(Number(result.invoiceId)).subscribe((result) => {
          // });
          this.notify.success(this.l('SavedSuccessfully'));
          this.editMode = false;
          this.isSaving = false;
          this.router.navigate(['/app/main/transactions'],{state: {tabvaule:'Sales Invoice'}});
          this.download(result.invoiceId, result.uuid)
          
        });
    } else {
      this.notify.error(this.l('Please fill all the required fields'));      
    }

  }
  download(id, uid) {
    //window.location.reload();
    var pdfUrl = AppConsts.pdfUrl + '/InvoiceFiles/' + this._sessionService.tenantId + "/" + uid + "/" + uid + "_" + id + ".pdf";
    window.open(pdfUrl)
  }


}
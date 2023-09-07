import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomersesServiceProxy, CustomersDto, SalesInvoicesServiceProxy, CreateOrEditSalesInvoiceDto, CreateOrEditSalesInvoicePartyDto, CreateOrEditSalesInvoiceItemDto, CreateOrEditSalesInvoiceSummaryDto, CreateOrEditSalesInvoiceVATDetailDto, CreateOrEditSalesInvoiceDiscountDto, CreateOrEditSalesInvoicePaymentDetailDto, CreateOrEditSalesInvoiceContactPersonDto, SalesInvoiceAddressDto, CreateOrEditSalesInvoiceAddressDto, CountryServiceProxy, GetCountryForViewDto, TenantBasicDetailsServiceProxy, CreateOrEditTenantBasicDetailsDto, ImportBatchDatasServiceProxy } from '@shared/service-proxies/service-proxies';
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
import Swal from 'sweetalert2/dist/sweetalert2.js';

@Component({
  templateUrl: './clearBatchSummery.component.html',
  styleUrls: ['./clearBatchSummery.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class ClearBatchSummeryComponent extends AppComponentBase {

  date = new Date();
  month = (this.date.getMonth() + 1).toString().length > 1 ? this.date.getMonth() + 1 : '0' + (this.date.getMonth() + 1);
  day = (this.date.getDate()).toString().length > 1 ? this.date.getDate() : '0' + (this.date.getDate());
  maxDate = this.date.getFullYear() + '-' + this.month + '-' + this.day;
  editMode = false;
  issueDate = new Date().toISOString().slice(0, 10);
  batchid: number;
  transMaster: number;
  profileType = '';
  delete: number;
  discount = 0.0;
  quantity = 0.0;
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
  tenants: CreateOrEditTenantBasicDetailsDto = new CreateOrEditTenantBasicDetailsDto();

  //product: CreateOrEditProductDto = new CreateOrEditProductDto();

  //products: GetProductForViewDto[] = [];
  //filteredProducts: GetProductForViewDto[] = [];

  address: CreateOrEditSalesInvoiceAddressDto = new CreateOrEditSalesInvoiceAddressDto();
  customers: CustomersDto[] = new Array<CustomersDto>();
  filteredCustomers: CustomersDto[];
  countries: GetCountryForViewDto[] = [];
  isSaving = false;
  basicForm: FormGroup;
  setview = false;
  type: string;
  fileName: string;
  Deletepage = false;
  constructor(
    private fb: FormBuilder,
    injector: Injector,
    private _customerServiceProxy: CustomersesServiceProxy,
    // private _productsServiceProxy: ProductsServiceProxy,
    private _salesInvoiceServiceProxy: SalesInvoicesServiceProxy,
    // private _invoiceHeaderServiceProxy: InvoiceHeadersServiceProxy,
    private _masterCountriesServiceProxy: CountryServiceProxy,
    private _tenantbasicdetailsServiceProxy: TenantBasicDetailsServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService,
    private _dateTimeService: DateTimeService,
    private _sessionService: AppSessionService,
    private router: Router,
    private _fileUpload: ImportBatchDatasServiceProxy,

  ) {
    super(injector);
    this.salesForm();
  }

  salesForm() {
    this.basicForm = this.fb.group({
      Name: ['', Validators.required],
      Buildno: ['', [Validators.required,
        Validators.maxLength(4),
      Validators.pattern('^[0-9]*$') ]],
      street: ['', Validators.required ],
      Neighbourhood: ['', Validators.required],
      pin: ['', [Validators.required,
        Validators.maxLength(5),
    Validators.pattern('^[0-9]*$') ] ],
      city: ['', Validators.required ],
      state: ['', Validators.required ],
      nationality: ['', Validators.required ],
      ContactNo: ['', [Validators.required, Validators.pattern('^[0-9]*$')] ],
      vatid: ['', [Validators.required,
        Validators.maxLength(15),
        Validators.minLength(15),
      Validators.pattern('^3[0-9]*3$')]]
    });
  }

  isFormValid() {
    return (this.basicForm.valid);
  }

  // eslint-disable-next-line @angular-eslint/use-lifecycle-interface
  ngOnInit(): void {
    this.Deletepage = true;
    this.address.countryCode = null;
    this.getCountriesDropdown();
    this.create();
    this.onSelectCustomer();
    //this.getProducts();
    this.invoiceSummary.totalAmountWithVAT = 0;
    this.invoiceSummary.totalAmountWithoutVAT = 0;
    this.invoiceSummary.sumOfInvoiceLineNetAmount = 0;
    this.invoiceSummary.netInvoiceAmount = 0;
    this.discount = 0;
    this.invoiceItem.quantity = 0;
    this.invoiceItem.discountPercentage = 0;
    this.invoiceItem.vatRate = 15;
    this.supplier.registrationName = this._sessionService.tenant.name;
    this.supplier.vatid = '5672345';
    this.supplier.contactPerson.email = this._sessionService.user.emailAddress;
    this.supplier.contactPerson.contactNumber = '567898084';
    this.invoice.customerId = '567';
   

  }

  onOptionSelected(selectedValue: string) {
    if (selectedValue) {
      console.log(selectedValue,'URLLLL')
      this.router.navigateByUrl(selectedValue);
    }
  }
  //get countries from master data
  getCountriesDropdown() {
    this._masterCountriesServiceProxy.getAll('', '', '', '', '', undefined, undefined, '', '', undefined, undefined, undefined, undefined, 200)
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
    this.customer.registrationName = event.registrationName;
  }
  //--------------------------------------auto complete ends---------------------------------------------------------

  create() {
    this.customer.contactPerson = new CreateOrEditSalesInvoiceContactPersonDto();
    this.supplier.contactPerson = new CreateOrEditSalesInvoiceContactPersonDto();
    // this.invoice.buyer.contactPerson = new CreateOrEditSalesInvoiceContactPersonDto();

    // this.invoiceItem.
  }


  //add item to invoiceitems
  addItem() {


    if (this.invoiceItem.quantity < 0) {
      this.message.warn('Quantity cannot be less than 0 ');
      return;
    }
    if (this.invoiceItem.unitPrice < 0) {
      this.message.warn('Rate cannot be less than 0 ');
      return;
    }
    if (this.invoiceItem.discountPercentage < 0 || this.invoiceItem.discountPercentage > 100) {
      this.message.warn('Discount must be in between 0 to 100');
      return;
    }
    if (!(this.invoiceItem.name && this.invoiceItem.description && this.invoiceItem.quantity
      && this.invoiceItem.unitPrice)) {

      this.notify.error(this.l('Please fill all required fields to add item to invoice.'));
      return;
    }
    if (this.invoiceItem.vatRate !== 15 && this.invoiceItem.vatRate !== 0) {
      this.notify.error(this.l('VAT rate error'));
      return;
    }

    if (this.address.countryCode !== 'SA' && this.invoiceItem.vatRate !== 0) {
      this.notify.error(this.l('Exports are exempt from VAT'));
      return;
    }
    //   if ( this.invoiceItem.discountPercentage >= 15|| this.invoiceItem.discountPercentage<=100 ) {
    //     this.notify.error(this.l('Discount rate error'));
    //   return;
    // }
    this.invoiceItem.discountAmount = this.invoiceItem.quantity * this.invoiceItem.unitPrice * this.invoiceItem.discountPercentage / 100.0;
    this.invoiceItem.vatAmount = ((this.invoiceItem.quantity * this.invoiceItem.unitPrice) - this.invoiceItem.discountAmount) * this.invoiceItem.vatRate / 100.0;
    this.invoiceItem.lineAmountInclusiveVAT = ((this.invoiceItem.quantity * this.invoiceItem.unitPrice) - this.invoiceItem.discountAmount) + this.invoiceItem.vatAmount;
    this.invoiceItem.netPrice = this.invoiceItem.lineAmountInclusiveVAT - this.invoiceItem.vatAmount;
    this.invoiceItem.currencyCode = 'SAR';
    this.invoiceItem.identifier = 'Sales';
    this.invoiceItem.vatCode = this.invoiceItem.vatRate === 15 ? 'S' : 'Z';
    this.invoiceItem.uom = '1';
    this.invoiceItems.push(this.invoiceItem);
    this.invoiceItem = new CreateOrEditSalesInvoiceItemDto();
    this.calculateInvoiceSummary();
    this.invoiceItem.quantity = 0;
    this.invoiceItem.discountPercentage = 0;
    this.invoiceItem.vatRate = 15;
  }

  deleteItem(index: number) {
    this.invoiceItems.splice(index, 1);

    this.calculateInvoiceSummary();
  }

  clearItem() {
    this.invoiceItem = new CreateOrEditSalesInvoiceItemDto();
    //this.product = new CreateOrEditProductDto();
    this.invoiceItem.discountPercentage = 0;
  }


  calculateInvoiceSummary() {

    this.invoiceSummary.totalAmountWithVAT = 0;
    this.invoiceSummary.totalAmountWithoutVAT = 0;
    this.invoiceSummary.sumOfInvoiceLineNetAmount = 0;
    this.invoiceSummary.netInvoiceAmount = 0;
    this.invoiceSummary.totalVATAmount = 0;
    this.discount = 0;

    this.invoiceItems.forEach(invoiceItem => {
      this.invoiceSummary.totalAmountWithVAT += invoiceItem.lineAmountInclusiveVAT;
      this.invoiceSummary.totalAmountWithoutVAT += (invoiceItem.quantity * invoiceItem.unitPrice - invoiceItem.discountAmount);
      this.invoiceSummary.sumOfInvoiceLineNetAmount += invoiceItem.netPrice;
      this.invoiceSummary.netInvoiceAmount += invoiceItem.quantity * invoiceItem.unitPrice;
      this.discount += invoiceItem.discountAmount;
      this.invoiceSummary.totalVATAmount = this.invoiceSummary.totalAmountWithVAT - this.invoiceSummary.totalAmountWithoutVAT;
    });



  }

  //


  fillDummy() {
    this.invoice.status = 'Paid';
    this.invoice.paymentType = 'Cash';
    this.invoice.dateOfSupply = DateTime.utc();

    this.invoice.invoiceCurrencyCode = 'SAR';

    this.invoice.invoiceSummary.currencyCode = 'SAR';
    this.invoice.invoiceSummary.paidAmountCurrency = 'SAR';
    this.invoice.invoiceSummary.payableAmountCurrency = 'SAR';
    this.invoice.invoiceSummary.netInvoiceAmountCurrency = 'SAR';
    this.invoice.invoiceSummary.totalAmountWithoutVATCurrency = 'SAR';
    this.invoice.invoiceNumber = '-1'; //!this.invoice.billingReferenceId ? " " : this.invoice.billingReferenceId

  }

  parseDate(dateString: string): DateTime {

    let time = new Date().toLocaleTimeString();
    let date = new Date(dateString);

    if (time.includes('PM')) {
      date.setHours(parseInt(time.split(':')[0]) + 12);
    } else {
      date.setHours(parseInt(time.split(':')[0]));
    } date.setMinutes(parseInt(time.split(':')[1]));
    date.setSeconds(parseInt(time.split(':')[2]));
    dateString = date.toISOString();


    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());

    }
    return null;
  }

  validateFields() {

    return (this.address.buildingNo && this.address.city && this.address.countryCode && this.address.street &&
      this.address.postalCode && this.address.state && this.address.neighbourhood);
  }


  save() {

    if (this.address.neighbourhood == null || this.address.neighbourhood === undefined) {
      this.basicForm.get('Neighbourhood').setValue('.');
    }

    let isSimplified = this.invoiceSummary.totalAmountWithVAT < 1000;
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

      this.invoice.invoiceSummary = this.invoiceSummary;
      this.invoice.items = this.invoiceItems;



      try {
        this.invoice.issueDate = this.parseDate(this.issueDate.toString());
      } catch (e) {
        this.notify.error(this.l('Please enter valid issue date.'));
        this.isSaving = false;
        return;

      }
      this.isSaving = true;




      this.fillDummy();
      console.log(this.invoice);
      this._salesInvoiceServiceProxy.createSalesInvoice(this.invoice)
        .subscribe((result) => {
          // this._salesInvoiceServiceProxy.insertSalesReportData(Number(result.invoiceId)).subscribe((result) => {
          // });
          this.notify.success(this.l('SavedSuccessfully'));
          this.editMode = false;
          this.isSaving = false;
          this.router.navigate(['/app/main/transactions'], {state: {tabvaule: 'Sales Invoice'}});
          this.download(result.invoiceId, result.uuid);

        });
    } else {
      this.notify.error(this.l('Please fill all the required fields'));
    }

  }
  download(id, uid) {
    //window.location.reload();
    let pdfUrl = AppConsts.pdfUrl + '/InvoiceFiles/' + this._sessionService.tenantId + '/' + uid + '/' + uid + '_' + id + '.pdf';
    window.open(pdfUrl);
  }

  deleteBatch(){

      Swal.fire({
        title: 'Do you want to Delete the Batch',
        text: '',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Delete it'
      }).then((result) => {
        if (result.isConfirmed) {
          Swal.fire(
            'Delete!',
            'Batch Is Deleted Succesfully.',
            'success'
          );
          this._fileUpload.deleteBatchSummary(this.batchid, this.transMaster).subscribe((result) => {
        this.delete = result;
        this.setview = false;
          });

        }
      });



  }

  Search(){
    this.setview = true;    
    if (this.transMaster ==1){
      this.type = 'Master';
    }else{
      this.type = 'Transaction';
    }
    this.batchid = this.batchid;
    this.fileName = '';
  }

}

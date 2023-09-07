import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GetCountryForViewDto, CountryServiceProxy, CreditNoteServiceProxy, CreateOrEditCreditNoteItemDto, CreateOrEditCreditNotePartyDto, CreateOrEditSalesInvoiceItemDto, CreateOrEditCreditNoteSummaryDto, CreateOrEditCreditNoteVATDetailDto, CreateOrEditCreditNoteDiscountDto, CreateOrEditCreditNotePaymentDetailDto, CreateOrEditCreditNoteContactPersonDto, CreditNoteAddressDto, CreateOrEditSalesInvoiceAddressDto, SalesInvoicesServiceProxy, CreateOrEditCreditNoteDto, TenantBasicDetailsServiceProxy, CreateOrEditCreditNoteAddressDto } from '@shared/service-proxies/service-proxies';
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
import { HttpHeaders } from '@angular/common/http';
import { clear } from 'localforage';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  templateUrl: './createCreditNote.component.html',
  styleUrls: ['./createCreditNote.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class CreateCreditNoteComponent extends AppComponentBase {
  date = new Date();
  month = (this.date.getMonth() + 1).toString().length > 1 ? this.date.getMonth() + 1 : '0' + (this.date.getMonth() + 1);
  day = (this.date.getDate()).toString().length > 1 ? this.date.getDate() : '0' + (this.date.getDate());
  maxDate = this.date.getFullYear() + '-' + this.month + '-' + this.day;
  editMode = false;
  editaddressMode = false;
  dateofsupply: DateTime;
  issueDate = new Date().toISOString().slice(0, 10);
  profileType = '';
  discount = 0.0;
  invoice: CreateOrEditCreditNoteDto = new CreateOrEditCreditNoteDto();
  supplier: CreateOrEditCreditNotePartyDto = new CreateOrEditCreditNotePartyDto();
  customer: CreateOrEditCreditNotePartyDto = new CreateOrEditCreditNotePartyDto();
  countries: GetCountryForViewDto[] = [];
  invoiceItems: CreateOrEditCreditNoteItemDto[] = [];
  invoiceItem: CreateOrEditCreditNoteItemDto = new CreateOrEditCreditNoteItemDto();
  invoiceSummary: CreateOrEditCreditNoteSummaryDto = new CreateOrEditCreditNoteSummaryDto();
  vatDetails: CreateOrEditCreditNoteVATDetailDto[] = [];
  vatDetail: CreateOrEditCreditNoteVATDetailDto = new CreateOrEditCreditNoteVATDetailDto();
  discountDetails: CreateOrEditCreditNoteDiscountDto[] = [];
  discountDetail: CreateOrEditCreditNoteDiscountDto = new CreateOrEditCreditNoteDiscountDto();
  paymentDetails: CreateOrEditCreditNotePaymentDetailDto[] = [];
  paymentDetail: CreateOrEditCreditNotePaymentDetailDto = new CreateOrEditCreditNotePaymentDetailDto();
  address: CreditNoteAddressDto = new CreditNoteAddressDto();
  billingReferenceIds: string[] = [];
  irnNos: string[] = [];
  basicForm: FormGroup;
  invoiceCount = 0;
  editItemIndex: number;
  editQuantity: number;
  originalInvoiceItems: CreateOrEditCreditNoteItemDto[];
  isSaving: boolean;
  editWithoutBillingRefId = false;
  prchaseEntryDate: any;
  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _CreditNoteServiceProxy: CreditNoteServiceProxy,
    private _salesInvoiceServiceProxy: SalesInvoicesServiceProxy,
    private _masterCountriesServiceProxy: CountryServiceProxy,
    private _tenantbasicdetailsServiceProxy: TenantBasicDetailsServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService,
    private _dateTimeService: DateTimeService,
    private _sessionService: AppSessionService,
    private _router: Router
  ) {
    super(injector);
    this.CreditForm();
  }
  CreditForm() {
    this.basicForm = this.fb.group({
      Name: ['', Validators.required],
      Buildno: ['', [Validators.required,
      Validators.maxLength(4),
      Validators.pattern('^[0-9]*$')]],
      street: ['', Validators.required],
      Neighbourhood: ['', Validators.required],
      pin: ['', [Validators.required,
      Validators.maxLength(5),
      Validators.pattern('^[0-9]*$')]],
      city: ['', Validators.required],
      state: ['', Validators.required],
      nationality: ['', Validators.required],
      ContactNo: ['', [Validators.pattern('^[0-9]*$'), Validators.required]],
      vatid: ['', [
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
    this.address.countryCode = null;
    this.getCountriesDropdown();
    this.create();
    this.getProducts();
    this.onSelectCustomer();
    this.invoiceSummary.totalAmountWithVAT = 0;
    this.invoiceSummary.totalAmountWithoutVAT = 0;
    this.invoiceSummary.sumOfInvoiceLineNetAmount = 0;
    this.invoiceSummary.netInvoiceAmount = 0;
    this.discount = 0;
    this.invoiceItem.quantity = 0;
    this.invoiceItem.vatRate = 15;
    this.invoiceItem.discountPercentage = 0;
    this.supplier.registrationName = this._sessionService.tenant.name;
    this.supplier.contactPerson.email = this._sessionService.user.emailAddress;
    this.invoice.customerId = '567';
    this.invoiceSummary.sumOfInvoiceLineNetAmountCurrency = 'SAR';
    this.invoice.location = 'Ind';
  
  }
  //get countries from master data
  getCountriesDropdown() {
    this._masterCountriesServiceProxy.getAll('', '', '', '', '', undefined, undefined, '', '', undefined, undefined, undefined, undefined, 200)
      .subscribe(result => {
        this.countries = result.items;
      });
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
          data[i] = (data[i].irnNo + ' ' + (data[i].issueDate).toString());
        }
        this.billingReferenceIds = data;
      }
    });
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
  onSelectInvoice(event): void {
    this._salesInvoiceServiceProxy.getsalesdetails(event.split(' ')[0],undefined).subscribe((data) => {
      this.invoice.billingReferenceId = data[0].irnNo;
      this.invoice.invoiceNumber = data[0].irnNo;
      this.dateofsupply = data[0].issueDate;
      this.customer.registrationName = data[0].name;
      this.customer.contactPerson.contactNumber = data[0].contactNumber;
      this.customer.contactPerson.email = data[0].email;
      this.customer.vatid = data[0].vatid;
      this.address.buildingNo = data[0].buildingNo;
      this.address.street = data[0].street?.trim();
      this.address.additionalStreet = data[0].additionalStreet?.trim();
      this.address.city = data[0].city?.trim();
      this.address.state = data[0].state;
      this.address.countryCode = data[0].countryCode;
      this.address.postalCode = data[0].postalCode;
      this.address.neighbourhood = data[0].neighbourhood?.trim();
      this.prchaseEntryDate = data[0].issueDate;
    });
    this.invoiceItems = [];
    this._salesInvoiceServiceProxy.getsalesitemdetail(event.split(' ')[0],'sales').subscribe((data) => {
      for (let i = 0; i < data.length; i++) {
        this.invoiceItem.costPrice = data[i].costPrice;
        this.invoiceItem.description = data[i].description;
        this.invoiceItem.name = data[i].name;
        this.invoiceItem.currencyCode="SAR";
        this.invoiceItem.discountAmount = data[i].discountAmount;
        this.invoiceItem.discountPercentage = data[i].discountPercentage;
        this.invoiceItem.grossPrice = data[i].grossPrice;
        this.invoiceItem.lineAmountInclusiveVAT = data[i].lineAmountInclusiveVAT;
        this.invoiceItem.quantity = data[i].quantity;
        this.invoiceItem.unitPrice = data[i].unitPrice;
        this.invoiceItem.netPrice = data[i].netPrice;
        this.invoiceItem.vatAmount = data[i].vatAmount;
        this.invoiceItem.vatRate = data[i].vatRate;
        this.invoiceItem.vatCode =  data[i].vatRate === 15 ? 'S' : 'Z';
        this.invoiceItems.push(this.invoiceItem);
        this.originalInvoiceItems = JSON.parse(JSON.stringify(this.invoiceItems));
        this.invoiceCount = this.invoiceItems.length;
        this.invoiceItem = new CreateOrEditSalesInvoiceItemDto();
      }
      this.calculateInvoiceSummary();
    });
  }
  //--------------------------------------auto complete ends---------------------------------------------------------
  getProducts(): void {
  }
  updateProduct(i: number) {
  }

  filterProduct() {
  }

  create() {
    this.customer.contactPerson = new CreateOrEditCreditNoteContactPersonDto();
    this.supplier.contactPerson = new CreateOrEditCreditNoteContactPersonDto();
    this.supplier.address = new CreateOrEditCreditNoteAddressDto();

  }

  //add item to invoiceitems
  addItem() {
    if (this.invoiceItem.quantity > this.editQuantity && this.invoiceCount !== 0 && this.editMode) {
      this.invoiceItem.quantity = this.editQuantity;
      this.message.warn('Quantity cannot exceed ' + this.editQuantity);
      return;
    }
    if (this.invoiceItem.quantity <= 0) {
      this.message.warn('Quantity cannot be less than or equal to 0 ');
      return;
    }
    if (this.invoiceItem.unitPrice <= 0) {
      this.message.warn('Rate cannot be less than or equal to 0 ');
      return;
    }
    if (this.invoiceItem.discountPercentage < 0 || this.invoiceItem.discountPercentage > 100) {
      this.message.warn('Discount  must be in between 0 to 100');
      return;
    }
    if (this.invoiceItem.name == null || this.invoiceItem.description == null || this.invoiceItem.quantity == null
      || this.invoiceItem.quantity == null || this.invoiceItem.unitPrice == null || this.invoiceItem.discountPercentage == null) {
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
    this.invoiceItem.discountAmount = this.invoiceItem.quantity * this.invoiceItem.unitPrice * this.invoiceItem.discountPercentage / 100.0;
    this.invoiceItem.vatAmount = ((this.invoiceItem.quantity * this.invoiceItem.unitPrice) - this.invoiceItem.discountAmount) * this.invoiceItem.vatRate / 100.0;
    this.invoiceItem.lineAmountInclusiveVAT = ((this.invoiceItem.quantity * this.invoiceItem.unitPrice) - this.invoiceItem.discountAmount) + this.invoiceItem.vatAmount;
    this.invoiceItem.netPrice = this.invoiceItem.lineAmountInclusiveVAT - this.invoiceItem.vatAmount;
    this.invoiceItem.currencyCode = 'SAR';
    this.invoiceItem.identifier = 'Sales';
    this.invoiceItem.vatCode = this.invoiceItem.vatRate === 15 ? 'S' : 'Z';
    this.invoiceItem.uom = 'test';
    console.log(this.invoiceItems, 'iiii');
    if (this.editMode) {
      this.invoiceItems[this.editItemIndex] = this.invoiceItem;
    } else {
      this.invoiceItems.push(this.invoiceItem);
    }
    this.invoiceItem = new CreateOrEditSalesInvoiceItemDto();
    this.calculateInvoiceSummary();
    this.invoiceItem.quantity = 0;
    this.invoiceItem.vatRate = 15;
    this.invoiceItem.discountPercentage = 0;
    this.editMode = false;
  }

  deleteItem(index: number) {
    if(this.invoiceItems.length==1)
    {
      this.notify.error(this.l('Credit Note must have atleast one line item'));

    }
    else{
    this.invoiceItems.splice(index, 1);
    this.calculateInvoiceSummary();
    }
  }

  editItem(index: number) {
    this.invoiceItem = this.invoiceItems[index];
    this.editItemIndex = index;
    this.editMode = true;
    if (this.invoice.billingReferenceId != null || this.invoice.billingReferenceId !== undefined) {
      this.editWithoutBillingRefId = false;
    } else {
      this.editWithoutBillingRefId = true;
    }
    this.editQuantity = this.originalInvoiceItems[index].quantity;
    this.calculateInvoiceSummary();
  }

  clearItem() {
    this.invoiceItem = new CreateOrEditSalesInvoiceItemDto();
    this.invoiceItem.discountPercentage = 0;
    this.editMode = false;
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
      this.invoiceSummary.netInvoiceAmount += (invoiceItem.quantity * invoiceItem.unitPrice);
      this.discount += invoiceItem.discountAmount;
      this.invoiceSummary.totalVATAmount = this.invoiceSummary.totalAmountWithVAT - this.invoiceSummary.totalAmountWithoutVAT;

    });

  }

  fillDummy() {
    this.invoice.status = 'Paid';
    this.invoice.paymentType = 'Cash';
    this.invoice.invoiceNumber = '-1';
    this.invoice.invoiceCurrencyCode = 'SAR';
    if (this.dateofsupply === undefined || this.dateofsupply == null){
      this.invoice.dateOfSupply = this.invoice.issueDate;
    } else {
      this.invoice.dateOfSupply = this.dateofsupply;
    }
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

  save() {
    if (this.isFormValid()) {
      this.isSaving = true;
      if (this.invoiceItems.length <= 0) {
        this.notify.error(this.l('Please add at least one item to save.'));
        this.isSaving = false;
        return;
      }
      if (this.invoice.billingReferenceId === undefined || this.invoice.billingReferenceId == null) {
        this.notify.error(this.l('Please enter Reference Invoice Number.'));
        this.isSaving = false;
        return;
      }
      if (!this.invoice.additional_Info?.trim()) {
        this.notify.error(this.l('Please enter the reason for credit note.'));
        this.isSaving = false;
        return;
      }

      if ((this.issueDate.toString() >  DateTime.now().toString())){
        this.notify.error(this.l('Credit Note Date Should not be greater than Current Date.'));
        this.isSaving = false;

        return;
      }
      this.invoice.invoiceSummary = this.invoiceSummary;
      this.invoice.items = this.invoiceItems;
      console.log(this.invoice.items, 'ttttt');

      this.invoice.supplier = [this.supplier];
      this.invoice.buyer = [this.customer];
      try {
        this.invoice.issueDate = this.parseDate(this.issueDate.toString());
        if (this.editaddressMode === true)  {
          if (this.invoice.issueDate < this.parseDate(this.prchaseEntryDate.toString())) {
            this.notify.error(this.l('Credit Note date should not be greater than the purchase entry date.'));
            this.isSaving = false;
            return;
          }
        }
      } catch (e) {
        this.notify.error(this.l('Please enter valid issue date.'));
        this.isSaving = false;
        return;
      }
      this.invoice.invoiceNotes = this.invoice.additional_Info;
      this.invoice.buyer[0].address = this.address;
      this.invoice.supplier[0].address = this.supplier.address;

      this.fillDummy();
      this._CreditNoteServiceProxy.createCreditNote(this.invoice).subscribe((result) => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.isSaving = false;
        this._router.navigate(['/app/main/transactions'], { state: { tabvaule: 'Credit Note' } });
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
  refresh() {
    //reload page
    location.reload();
  }

}

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { GetCountryForViewDto, CountryServiceProxy, CreditNoteServiceProxy, CreateOrEditCreditNoteItemDto, CreateOrEditDebitNotePartyDto, CreateOrEditSalesInvoiceItemDto, CreateOrEditCreditNoteSummaryDto, CreateOrEditDebitNoteVATDetailDto, CreateOrEditDebitNoteDiscountDto, CreateOrEditDebitNotePaymentDetailDto, CreateOrEditDebitNoteContactPersonDto, DebitNoteAddressDto, CreateOrEditSalesInvoiceAddressDto, SalesInvoicesServiceProxy, CreateOrEditCreditNoteDto, CreditNoteAddressDto, DebitNotesServiceProxy, CreateOrEditDebitNoteDto, DebitNotePartiesServiceProxy, CreateOrEditDebitNoteItemDto, CreateOrEditDebitNoteSummaryDto, CreateOrEditDebitNoteAddressDto, InvoiceDto, CreateOrEditPurchaseDebitNoteDto, CreateOrEditPurchaseDebitNotePartyDto, CreateOrEditPurchaseDebitNoteItemDto, CreateOrEditPurchaseDebitNoteSummaryDto, CreateOrEditPurchaseDebitNoteVATDetailDto, CreateOrEditPurchaseDebitNoteDiscountDto, CreateOrEditPurchaseDebitNotePaymentDetailDto, CreateOrEditPurchaseDebitNoteAddressDto, CreateOrEditPurchaseDebitNoteContactPersonDto, PurchaseDebitNoteServiceProxy, PurchaseCreditNoteServiceProxy, PurchaseDebitNoteAddressDto } from '@shared/service-proxies/service-proxies';
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
import { FormGroup, FormBuilder, Validators } from '@angular/forms';




@Component({
  templateUrl: './createPurchaseDebitNote.component.html',
  styleUrls: ['./createPurchaseDebitNote.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class CreatePurchaseDebitNoteComponent extends AppComponentBase {


  date = new Date();
  month = (this.date.getMonth() + 1).toString().length > 1 ? this.date.getMonth() + 1 : '0' + (this.date.getMonth() + 1);
  day = (this.date.getDate()).toString().length > 1 ? this.date.getDate() : '0' + (this.date.getDate());
  maxDate = this.date.getFullYear() + '-' + this.month + '-' + this.day;
  editMode = false;
  dateofsupply: DateTime;
  issueDate = new Date().toISOString().slice(0, 10);
  editaddressMode = false;
  profileType = '';

  discount = 0.0;
  prchaseEntryDate: any;
  invoice: CreateOrEditPurchaseDebitNoteDto = new CreateOrEditPurchaseDebitNoteDto();
  supplier: CreateOrEditPurchaseDebitNotePartyDto = new CreateOrEditPurchaseDebitNotePartyDto();
  buyer: CreateOrEditPurchaseDebitNotePartyDto = new CreateOrEditPurchaseDebitNotePartyDto();
  customer: CreateOrEditPurchaseDebitNotePartyDto = new CreateOrEditPurchaseDebitNotePartyDto();
  countries: GetCountryForViewDto[] = [];

  invoiceItems: CreateOrEditPurchaseDebitNoteItemDto[] = [];
  invoiceItem: CreateOrEditPurchaseDebitNoteItemDto = new CreateOrEditPurchaseDebitNoteItemDto();

  invoiceSummary: CreateOrEditPurchaseDebitNoteSummaryDto = new CreateOrEditPurchaseDebitNoteSummaryDto();
  vatDetails: CreateOrEditPurchaseDebitNoteVATDetailDto[] = [];
  vatDetail: CreateOrEditPurchaseDebitNoteVATDetailDto = new CreateOrEditPurchaseDebitNoteVATDetailDto();

  discountDetails: CreateOrEditPurchaseDebitNoteDiscountDto[] = [];
  discountDetail: CreateOrEditPurchaseDebitNoteDiscountDto = new CreateOrEditPurchaseDebitNoteDiscountDto();

  paymentDetails: CreateOrEditPurchaseDebitNotePaymentDetailDto[] = [];
  paymentDetail: CreateOrEditPurchaseDebitNotePaymentDetailDto = new CreateOrEditPurchaseDebitNotePaymentDetailDto();
  address: CreateOrEditPurchaseDebitNoteAddressDto = new CreateOrEditPurchaseDebitNoteAddressDto();
  isSaving: boolean;
  billingReferenceIds: string[] = [];
  irnNos: string[] = [];
  basicForm: FormGroup;
    constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _CreditNoteServiceProxy: DebitNotesServiceProxy,
    private _salesInvoiceServiceProxy: SalesInvoicesServiceProxy,
    private _PurchaseDebitNoteServiceProxy: PurchaseDebitNoteServiceProxy,
    private _PurchaseCreditNoteServiceProxy: PurchaseCreditNoteServiceProxy,
    private _notifyService: NotifyService,
    private _masterCountriesServiceProxy: CountryServiceProxy,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService,
    private _dateTimeService: DateTimeService,
    private _sessionService: AppSessionService,
    private _router: Router
  ) {
    super(injector);
    this.debitForm();
  }

  debitForm() {
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

  focusVatPerInput(): void {
    setTimeout(() => {
      document.getElementById('itemVATPercent').focus();
    }, 100);
  }
  //ngoninit

  ngOnInit(): void {
    this.address.countryCode = null;
    this.getCountriesDropdown();
    this.create();
    this.getProducts();

    this.invoiceSummary.totalAmountWithVAT = 0;
    this.invoiceSummary.totalAmountWithoutVAT = 0;
    this.invoiceSummary.sumOfInvoiceLineNetAmount = 0;
    this.invoiceSummary.netInvoiceAmount = 0;
    this.discount = 0;
    this.invoiceItem.quantity = 0;
    this.invoiceItem.discountPercentage = 0;
    this.invoiceItem.vatRate = 15;
    this.buyer.registrationName = this._sessionService.tenant.name;
    this.buyer.vatid = '5672345';
    this.buyer.contactPerson.email = this._sessionService.user.emailAddress;
    this.buyer.contactPerson.contactNumber = '567898084';
    this.invoice.customerId = '567';
    this.invoiceSummary.sumOfInvoiceLineNetAmountCurrency = 'SAR';
    this.invoice.location = 'Ind';
}
  //--------------------------------------auto complete starts---------------------------------------------------------

  filterCustomers(event): void {
  }

  onSelectCustomer(event): void {
  }
  changedata(event) {
  }
  //--------------------------------------auto complete ends---------------------------------------------------------


  //get countries from master data
  getCountriesDropdown() {
    this._masterCountriesServiceProxy.getAll('', '', '', '', '', undefined, undefined, '', '', undefined, undefined, undefined, undefined, 200)
      .subscribe(result => {
        this.countries = result.items;
        console.log(this.countries);
      });
  }
  getProducts(): void {
  }


  updateProduct(i: number) {  }

  filterProduct() {
     }


  create() {
    this.customer.contactPerson = new CreateOrEditPurchaseDebitNoteContactPersonDto();
    this.buyer.contactPerson = new CreateOrEditPurchaseDebitNoteContactPersonDto();
    this.supplier.contactPerson = new CreateOrEditPurchaseDebitNoteContactPersonDto();
  }
  addItem() {

    if (this.invoiceItem.quantity <= 0) {
      this.message.warn('Quantity should be more than 0 ');
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
    if (this.invoiceItem.name == null || this.invoiceItem.description == null || this.invoiceItem.quantity == null
      || this.invoiceItem.quantity == null || this.invoiceItem.unitPrice == null || this.invoiceItem.discountPercentage == null) {
      this.notify.error(this.l('Please fill all required fields to add item to invoice.'));
      return;
    }
    if (this.invoiceItem.vatRate != 15 && this.invoiceItem.vatRate != 0) {
      this.notify.error(this.l('VAT rate error'));
      return;
    }

    if (this.address.countryCode != 'SA' && this.invoiceItem.vatRate != 0) {
      this.notify.error(this.l('Exports are exempt from VAT'));
      return;
    }

    this.invoiceItem.discountAmount = this.invoiceItem.quantity * this.invoiceItem.unitPrice * this.invoiceItem.discountPercentage / 100.0;
    this.invoiceItem.vatAmount = (this.invoiceItem.quantity * this.invoiceItem.unitPrice - this.invoiceItem.discountAmount) * this.invoiceItem.vatRate / 100.0;
    this.invoiceItem.lineAmountInclusiveVAT = (this.invoiceItem.quantity * this.invoiceItem.unitPrice - this.invoiceItem.discountAmount) + this.invoiceItem.vatAmount;
    this.invoiceItem.netPrice = this.invoiceItem.lineAmountInclusiveVAT - this.invoiceItem.vatAmount;
    this.invoiceItem.currencyCode = 'SAR';
    this.invoiceItem.identifier = 'Sales';
    this.invoiceItem.vatCode = this.invoiceItem.vatRate === 15 ? 'S' : 'Z';
    this.invoiceItem.uom = 'test';
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
  }


  calculateInvoiceSummary() {


    this.invoiceSummary.totalAmountWithVAT = 0;
    this.invoiceSummary.totalAmountWithoutVAT = 0;
    this.invoiceSummary.sumOfInvoiceLineNetAmount = 0;
    this.invoiceSummary.netInvoiceAmount = 0;
    this.discount = 0;

    this.invoiceItems.forEach(invoiceItem => {
      console.log(invoiceItem, this.invoiceSummary);
      this.invoiceSummary.totalAmountWithVAT += invoiceItem.lineAmountInclusiveVAT;
      this.invoiceSummary.totalAmountWithoutVAT += (invoiceItem.quantity * invoiceItem.unitPrice - invoiceItem.discountAmount);
      this.invoiceSummary.sumOfInvoiceLineNetAmount += invoiceItem.netPrice;
      this.invoiceSummary.netInvoiceAmount += (invoiceItem.quantity * invoiceItem.unitPrice);
      this.discount += invoiceItem.discountAmount;
    });
  }
  fillDummy() {
    this.invoice.status = 'Paid';
    this.invoice.paymentType = 'Cash';
    if (this.dateofsupply === undefined || this.dateofsupply == null){
      this.invoice.dateOfSupply = this.invoice.issueDate;
    } else {
      this.invoice.dateOfSupply = this.dateofsupply;
    }
    if (this.editaddressMode === true) {
      this.invoice.invoiceNumber = this.invoice.invoiceNumber;
    } else{
      this.invoice.invoiceNumber = '-1';
    }
    this.invoice.invoiceCurrencyCode = 'SAR';
    this.invoice.invoiceSummary.currencyCode = 'SAR';
    this.invoice.invoiceSummary.paidAmountCurrency = 'SAR';
    this.invoice.invoiceSummary.payableAmountCurrency = 'SAR';
    this.invoice.invoiceSummary.netInvoiceAmountCurrency = 'SAR';
    this.invoice.invoiceSummary.totalAmountWithoutVATCurrency = 'SAR';


    this.invoice.items.forEach(item => {
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
    } date.setMinutes(parseInt(time.split(':')[1]));
    date.setSeconds(parseInt(time.split(':')[2]));
    dateString = date.toISOString();


    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());

    }
    return null;
  }

  //--------------------------------------auto complete starts---------------------------------------------------------

  filterInvoices(event): void {
    this._salesInvoiceServiceProxy.getPurchaseSuggestions(event.query).subscribe((data) => {
      if (data == null || data === undefined || data.length === 0) {
        this.invoice.billingReferenceId = event.query;
        this.editaddressMode = false;
      } else{
        this.editaddressMode = true;
      for (let i = 0; i < data.length; i++) {
        data[i] = (data[i].id + ' ' + (data[i].issueDate).toString());
      }

      this.billingReferenceIds = data;
    }
    });

  }

  onSelectInvoice(event): void {
    this._salesInvoiceServiceProxy.getPurchaseDetails(event.split(' ')[0]).subscribe((data) => {
      this.prchaseEntryDate = data[0].issueDate;
      this.invoice.billingReferenceId = data[0].id;
      this.invoice.invoiceNumber = data[0].id;
      this.customer.registrationName = data[0].name;
      this.customer.contactPerson.contactNumber = data[0].contactNumber;
      this.customer.contactPerson.email = data[0].email;
      this.address.buildingNo = data[0].buildingNo;
      this.address.street = data[0].street?.trim();
      this.address.additionalStreet = data[0].additionalStreet?.trim();
      this.address.city = data[0].city?.trim();
      this.address.state = data[0].state;
      this.address.countryCode = data[0].countryCode;
      this.address.postalCode = data[0].postalCode;
      this.address.neighbourhood = data[0].neighbourhood?.trim();
      this.customer.vatid = data[0].vatid;
      this.dateofsupply = data[0].issueDate;
    });
  }

  //--------------------------------------auto complete ends---------------------------------------------------------

  save() {
    if (this.isFormValid()) {
      this.isSaving = true;
      this.invoice.invoiceSummary = this.invoiceSummary;
      this.invoice.items = this.invoiceItems;
      this.invoice.supplier = this.customer;
      try {
        this.invoice.issueDate = this.parseDate(this.issueDate.toString());
        if (this.editaddressMode === true) {
          if (this.invoice.issueDate < this.parseDate(this.prchaseEntryDate.toString())) {
            this.notify.error(this.l('Debit Note date should not be greater then the purchase entry date'));
            this.isSaving = false;
            return;
          }
        }

      } catch (e) {
        this.notify.error(this.l('Please enter valid issue date.'));
        this.isSaving = false;
        return;

      }

      if (!this.invoice.additional_Info?.trim()) {
        this.notify.error(this.l('Please enter the reason for debit note.'));
        this.isSaving = false;
        return;
      }
      if (this.invoiceItems.length <= 0) {
        this.notify.error(this.l('Please add at least one item to save.'));
        this.isSaving = false;

        return;
      }
      this.invoice.supplier.address = new CreateOrEditPurchaseDebitNoteAddressDto();
      this.invoice.supplier.address = this.address;
      this.invoice.buyer = this.buyer;
      this.fillDummy();
      this.invoice.buyer.address = new PurchaseDebitNoteAddressDto();
        this._PurchaseDebitNoteServiceProxy.createPurchaseDebitNote(this.invoice).subscribe((result) => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.editMode = false;
        this.isSaving = false;
        this._router.navigate(['/app/main/sales/purchaseTransaction'], {state: {tabvaule: 'Debit Note'}});
      });
    } else {
      this.notify.error(this.l('Please fill all the required fields'));
    }

  }
  download(id, uid) {
    let pdfUrl = AppConsts.pdfUrl + '/InvoiceFiles/' + this._sessionService.tenantId + '/' + uid + '/' + uid + '_' + id + '.pdf';
    window.open(pdfUrl);
  }
  refresh(){
    location.reload();
    }
}

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
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
    templateUrl: './createDebitNote.component.html',
    styleUrls: ['./createDebitNote.component.css'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class CreateDebitNoteComponent extends AppComponentBase {
    date = new Date();
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
    countries: GetCountryForViewDto[] = [];

    invoiceItems: CreateOrEditDebitNoteItemDto[] = [];
    invoiceItem: CreateOrEditDebitNoteItemDto = new CreateOrEditDebitNoteItemDto();

    invoiceSummary: CreateOrEditDebitNoteSummaryDto = new CreateOrEditDebitNoteSummaryDto();
    vatDetails: CreateOrEditDebitNoteVATDetailDto[] = [];
    vatDetail: CreateOrEditDebitNoteVATDetailDto = new CreateOrEditDebitNoteVATDetailDto();

    discountDetails: CreateOrEditDebitNoteDiscountDto[] = [];
    discountDetail: CreateOrEditDebitNoteDiscountDto = new CreateOrEditDebitNoteDiscountDto();

    paymentDetails: CreateOrEditDebitNotePaymentDetailDto[] = [];
    paymentDetail: CreateOrEditDebitNotePaymentDetailDto = new CreateOrEditDebitNotePaymentDetailDto();

    //product: CreateOrEditProductDto = new CreateOrEditProductDto();

    //products: GetProductForViewDto[] = [];
    //filteredProducts: GetProductForViewDto[] = [];
    //customers:GetCustomerAutofill[]=new Array<GetCustomerAutofill>();
    //filteredCustomers:GetCustomerAutofill[];

    address: CreateOrEditDebitNoteAddressDto = new CreateOrEditDebitNoteAddressDto();
    isSaving: boolean;

    billingReferenceIds: string[] = [];
    irnNos: string[] = [];
    basicForm: FormGroup;
    prchaseEntryDate: any;
    editaddressMode: boolean;
    //salesInvoice: GetInvoiceDto = new GetInvoiceDto();

    constructor(
        injector: Injector,
        private fb: FormBuilder,
        //private _customerServiceProxy: CustomerServiceProxy,
        //private _productsServiceProxy: ProductsServiceProxy,
        private _CreditNoteServiceProxy: DebitNotesServiceProxy,
        //private _invoiceHeaderServiceProxy: InvoiceHeadersServiceProxy,
        private _salesInvoiceServiceProxy: SalesInvoicesServiceProxy,
        private _tenantbasicdetailsServiceProxy: TenantBasicDetailsServiceProxy,
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
            Buildno: ['', [Validators.required, Validators.maxLength(4), Validators.pattern('^[0-9]*$')]],
            street: ['', Validators.required],
            Neighbourhood: ['', Validators.required],
            pin: ['', [Validators.required, Validators.maxLength(5), Validators.pattern('^[0-9]*$')]],
            city: ['', Validators.required],
            state: ['', Validators.required],
            nationality: ['', Validators.required],
            // Email:['',Validators.required],
            ContactNo: ['', [Validators.required, Validators.pattern('^[0-9]*$')]],
            vatid: [
                '',
                [
                    Validators.required,
                    Validators.maxLength(15),
                    Validators.minLength(15),
                    Validators.pattern('^3[0-9]*3$'),
                ],
            ],
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
        this.address.countryCode = null;
        this.getCountriesDropdown();
        this.create();
        this.getProducts();
        this.onSelectCustomer();
        this.invoiceSummary.totalAmountWithVAT = 0;
        this.invoiceSummary.totalAmountWithoutVAT = 0;
        this.invoiceSummary.sumOfInvoiceLineNetAmount = 0;
        this.invoiceSummary.netInvoiceAmount = 0;
        this.invoiceSummary.totalVATAmount = 0;
        this.discount = 0;
        this.invoiceItem.quantity = 0;
        this.invoiceItem.discountPercentage = 0;
        this.invoiceItem.vatRate = 15;
        this.supplier.registrationName = this._sessionService.tenant.name;
        this.supplier.contactPerson.email = this._sessionService.user.emailAddress;
        this.invoice.customerId = '567';
        this.invoiceSummary.sumOfInvoiceLineNetAmountCurrency = 'SAR';
       
    }

    //--------------------------------------auto complete starts---------------------------------------------------------

    filterCustomers(event): void {
        //   this._customerServiceProxy.getCustomers(event.query).subscribe((data) => {
        //     this.filteredCustomers = data;
        // });
    }

    onSelectCustomer() {
        this._tenantbasicdetailsServiceProxy.getTenantById(this._sessionService.tenant.id).subscribe((data) => {
          //supplier
          this.supplier.address.city=data[0].city;
          this.supplier.address.state=data[0].state;
          this.supplier.address.countryCode = data[0].country;
          this.supplier.address.postalCode = data[0].postalCode;
          this.supplier.address.additionalNo=data[0].additionalNo;
          this.supplier.address.additionalStreet=data[0].additionalStreet;
          this.supplier.address.buildingNo=data[0].buildingNo;
          this.supplier.address.neighbourhood=data[0].neighbourhood;
          this.supplier.address.street=data[0].street;
          this.supplier.vatid=data[0].vatid;
          this.supplier.crNumber=data[0].crNumber;
          this.supplier.contactPerson.contactNumber=data[0].contactNumber;
          console.log(this.supplier.address,'sss')

        });
      }
    changedata(event) {
        // this.customer.registrationName=event.registrationName
    }
    //--------------------------------------auto complete ends---------------------------------------------------------

    //get countries from master data
    getCountriesDropdown() {
        this._masterCountriesServiceProxy
            .getAll('', '', '', '', '', undefined, undefined, '', '', undefined, undefined, undefined, undefined, 200)
            .subscribe((result) => {
                this.countries = result.items;
                console.log(this.countries);
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
        this.supplier.address= new CreateOrEditDebitNoteAddressDto();
        // this.invoiceItem.
    }

    //add item to invoiceitems
    addItem() {
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
        if (this.invoiceItem.vatRate !== 15 && this.invoiceItem.vatRate !== 0) {
            this.notify.error(this.l('VAT rate error'));
            return;
        }

        if (this.address.countryCode !== 'SA' && this.invoiceItem.vatRate !== 0) {
            this.notify.error(this.l('Exports are exempt from VAT'));
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

        this.invoiceItems.forEach((invoiceItem) => {
            console.log(invoiceItem, this.invoiceSummary);
            this.invoiceSummary.totalAmountWithVAT += invoiceItem.lineAmountInclusiveVAT;
            this.invoiceSummary.totalAmountWithoutVAT +=
                invoiceItem.quantity * invoiceItem.unitPrice - invoiceItem.discountAmount;
            this.invoiceSummary.sumOfInvoiceLineNetAmount += invoiceItem.netPrice;
            this.invoiceSummary.netInvoiceAmount += invoiceItem.quantity * invoiceItem.unitPrice;
            this.discount += invoiceItem.discountAmount;
        });
        this.invoiceSummary.totalVATAmount = this.invoiceSummary.totalAmountWithVAT-this.invoiceSummary.totalAmountWithoutVAT;

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

    onSelectInvoice(event): void {
        this._salesInvoiceServiceProxy.getsalesdetails(event.split(' ')[0],undefined).subscribe((data) => {
            // this.salesInvoice = data;
            // this.invoiceItems = this.salesInvoice.itemData;
            this.prchaseEntryDate = data[0].issueDate;
            this.invoice.billingReferenceId = data[0].irnNo;
            this.invoice.invoiceNumber = data[0].irnNo;
            // this.invoiceSummary = this.salesInvoice.invoicesummary;
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
            // this.customer.contactPerson.email = this.salesInvoice.customerEmail;
            // this.customer.contactPerson.contactNumber = this.salesInvoice.conatctNo;
            //   this.invoice.billingReferenceId=event.split(" ")[0];
            // this.originalInvoiceItems = JSON.parse(JSON.stringify(this.invoiceItems));
            // this.invoiceCount = this.invoiceItems.length;
            // this.calculateInvoiceSummary();
        });
    }

    //--------------------------------------auto complete ends---------------------------------------------------------

    save() {
        if (this.isFormValid()) {
            this.isSaving = true;
            this.invoice.invoiceSummary = this.invoiceSummary;
            this.invoice.items = this.invoiceItems;
            this.invoice.supplier = [this.supplier];
            this.invoice.buyer = [this.customer];
            console.log('ss', this.invoice.billingReferenceId);

            try {
                this.invoice.issueDate = this.parseDate(this.issueDate.toString());
                if (this.editaddressMode === true) {
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
            if (this.invoice.billingReferenceId === undefined || this.invoice.billingReferenceId == null) {
                this.notify.error(this.l('Please enter Reference Invoice Number.'));
                this.isSaving = false;
                return;
            }

            if (this.issueDate.toString() > DateTime.now().toString()) {
                this.notify.error(this.l('Debit Note Date Should not be greater than Current Date.'));
                this.isSaving = false;

                return;
            }

            this.invoice.invoiceNotes = this.invoice.additional_Info;
            //this.invoice.buyer.address = new CreateOrEditDebitNoteAddressDto();
            this.invoice.buyer[0].address = this.address;
            this.invoice.supplier[0].address=this.supplier.address;

            this.fillDummy();
            console.log(this.invoice);
            this._CreditNoteServiceProxy.createDebitNote(this.invoice).subscribe((result) => {
                // this._CreditNoteServiceProxy.insertDebitReportData(Number(result.invoiceId)).subscribe((result) => {
                // });
                this.notify.success(this.l('SavedSuccessfully'));
                this.editMode = false;
                this.isSaving = false;
                this._router.navigate(['/app/main/transactions'], { state: { tabvaule: 'Debit Note' } });
                this.download(result.invoiceId, result.uuid);
            });
        } else {
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
}

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CountryServiceProxy,GetCountryForViewDto, SalesInvoicesServiceProxy, CreateOrEditSalesInvoiceDto,CreateOrEditPurchaseEntryPartyDto, CreateOrEditSalesInvoiceItemDto, CreateOrEditSalesInvoiceSummaryDto, CreateOrEditPurchaseEntryVATDetailDto, CreateOrEditPurchaseEntryDiscountDto, CreateOrEditPurchaseEntryPaymentDetailDto, CreateOrEditPurchaseEntryContactPersonDto, PurchaseEntryAddressDto, CreateOrEditSalesInvoiceAddressDto, SalesInvoiceAddressDto, PurchaseEntriesServiceProxy, CreateOrEditPurchaseEntryDto, CreateOrEditPurchaseEntrySummaryDto, CreateOrEditPurchaseEntryItemDto } from '@shared/service-proxies/service-proxies';
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
import { FormBuilder, FormGroup, Validators } from '@angular/forms';


@Component({
  templateUrl: './createPurchaseEntry.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class CreatePurchaseEntryComponent extends AppComponentBase {

  date=new Date();
  month= (this.date.getMonth()+1).toString().length>1?this.date.getMonth()+1:"0"+(this.date.getMonth()+1)
  day= (this.date.getDate()).toString().length>1?this.date.getDate():"0"+(this.date.getDate())
  maxDate = this.date.getFullYear()+"-"+this.month+"-"+this.day;
  editMode: boolean = false;
  issueDate:Date;

  profileType: string = "";

  discount:number=0.0

  invoice: CreateOrEditPurchaseEntryDto = new CreateOrEditPurchaseEntryDto();
  supplier: CreateOrEditPurchaseEntryPartyDto = new CreateOrEditPurchaseEntryPartyDto();
  customer:CreateOrEditPurchaseEntryPartyDto = new CreateOrEditPurchaseEntryPartyDto();


  invoiceItems:CreateOrEditPurchaseEntryItemDto[] = [];
  invoiceItem: CreateOrEditPurchaseEntryItemDto = new CreateOrEditPurchaseEntryItemDto();
  countries: GetCountryForViewDto[] = [];

  invoiceSummary:CreateOrEditPurchaseEntrySummaryDto = new CreateOrEditPurchaseEntrySummaryDto();
  vatDetails:CreateOrEditPurchaseEntryVATDetailDto[] = [];
  vatDetail:CreateOrEditPurchaseEntryVATDetailDto = new CreateOrEditPurchaseEntryVATDetailDto();

  discountDetails:CreateOrEditPurchaseEntryDiscountDto[] = [];
  discountDetail:CreateOrEditPurchaseEntryDiscountDto = new CreateOrEditPurchaseEntryDiscountDto();

  paymentDetails:CreateOrEditPurchaseEntryPaymentDetailDto[] = [];
  paymentDetail:CreateOrEditPurchaseEntryPaymentDetailDto = new CreateOrEditPurchaseEntryPaymentDetailDto();

  //product: CreateOrEditProductDto = new CreateOrEditProductDto();

  //products: GetProductForViewDto[] = [];
  //filteredProducts: GetProductForViewDto[] = [];

  address: SalesInvoiceAddressDto = new SalesInvoiceAddressDto();
  isSaving: boolean;
  basicForm: FormGroup;



 
  constructor(
    injector: Injector,
    private fb: FormBuilder,
    //private _customerServiceProxy: CustomerServiceProxy,
    //private _productsServiceProxy: ProductsServiceProxy,
    private _salesInvoiceServiceProxy: PurchaseEntriesServiceProxy,
    private _invoiceHeaderServiceProxy: SalesInvoicesServiceProxy,
    private _masterCountriesServiceProxy: CountryServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService,
    private _dateTimeService: DateTimeService,
    private _sessionService: AppSessionService

  ) {
    super(injector);
    this.PurchaseForm();
  }
  PurchaseForm() {
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
      nationality: ['', [Validators.required] ],
      ContactNo: ['', [Validators.required, Validators.pattern("^[0-9]*$")] ],
      vatid:['', [
       ]]
    })
    this.basicForm.get('nationality').valueChanges.subscribe(val => {
      if (this.basicForm.get('nationality').value=="SA") { // for setting validations
        this.basicForm.get('vatid').addValidators(  [Validators.required ,
               Validators.minLength(15),
            Validators.maxLength(15),
          Validators.pattern("^3[0-9]*3$")]);
      } 
      if (this.basicForm.get('nationality').value!="SA") { // for clearing validations
        this.basicForm.get('vatid').setValidators([Validators.required]);
      }
      this.basicForm.get('vatid').updateValueAndValidity();
  });
  }

  isFormValid() {
    return (this.basicForm.valid);
  }
//ngoninit

  ngOnInit(): void {
    this.getCountriesDropdown();
    this.create();
    this.getProducts();

    this.invoiceSummary.totalAmountWithVAT=0;
    this.invoiceSummary.totalAmountWithoutVAT=0 ;
    this.invoiceSummary.totalVATAmount=0;
    
    this.invoiceSummary.sumOfInvoiceLineNetAmount=0;
    this.invoiceSummary.netInvoiceAmount=0;
    this.discount=0;


this.invoiceItem.discountPercentage=0;
    this.supplier.registrationName=this._sessionService.tenancyName
    this.supplier.vatid="5672345"
    this.supplier.contactPerson.email=this._sessionService.user.emailAddress
   this.supplier.contactPerson.contactNumber="567898084"
    this.invoice.customerId="567"
//this.customer.vatid="-"
    //get all invoice headers
    this._invoiceHeaderServiceProxy.getAll("", "", "", undefined, undefined, undefined,undefined, "", "", "", "", "", undefined, undefined, "", "", "", "", 0,0, 0, "", "","","","","",0, 1234567).subscribe((result) => {
      console.log(result);
    });


  }


       //get countries from master data
       getCountriesDropdown() {
        this._masterCountriesServiceProxy.getAll("","","","","",undefined,undefined,"","",undefined,undefined,undefined,undefined,200)
          .subscribe(result => {
            this.countries = result.items;
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
      //  this.invoiceItem.costPrice = this.product.costPrice;
      //  this.invoiceItem.description = this.product.description;
      //  this.invoiceItem.uom = this.product.uom;

  }

  filterProduct() {
    // let name = this.product.name
    // console.log(name,this.products);
    // this.filteredProducts = JSON.parse(JSON.stringify(this.products)).filter(function (str) { 
    //   return str.product.name.indexOf(name||"") === -1; });
    //   console.log(this.filteredProducts);
  }


  create() {
    this.customer.contactPerson = new CreateOrEditPurchaseEntryContactPersonDto();
    this.supplier.contactPerson = new CreateOrEditPurchaseEntryContactPersonDto();
  // this.invoiceItem.
  }

  //add item to invoiceitems
  addItem() {
    if(this.invoiceItem.quantity<0 ){
      this.message.warn("Quantity cannot be less than 0 ");
      return;
    }
    if(this.invoiceItem.unitPrice<0 ){
      this.message.warn("Rate cannot be less than 0 ");
      return;
    }
    if(this.invoiceItem.discountPercentage<0 || this.invoiceItem.discountPercentage>100 ){
      this.message.warn("Discount must be in between 0 to 100");
      return;
    }
    if (this.invoiceItem.name == null || this.invoiceItem.description == null || this.invoiceItem.quantity == null 
      || this.invoiceItem.quantity == null || this.invoiceItem.unitPrice == null || this.invoiceItem.discountPercentage == null) {
        this.notify.error(this.l('Please fill all required fields to add item to invoice.'));
        return;
    }
    if (this.invoiceItem.vatRate != 15 && this.invoiceItem.vatRate != 0) {
      this.notify.error(this.l('VAT Rate should be 0 or 15'));
      return;
    }
    
 this.invoiceItem.discountAmount = this.invoiceItem.quantity * this.invoiceItem.unitPrice * this.invoiceItem.discountPercentage /100.0
 this.invoiceItem.vatAmount = (this.invoiceItem.quantity * this.invoiceItem.unitPrice - this.invoiceItem.discountAmount)* this.invoiceItem.vatRate /100.0
 this.invoiceItem.lineAmountInclusiveVAT = (this.invoiceItem.quantity * this.invoiceItem.unitPrice - this.invoiceItem.discountAmount) + this.invoiceItem.vatAmount
 this.invoiceItem.netPrice = this.invoiceItem.lineAmountInclusiveVAT-this.invoiceItem.vatAmount

this.invoiceItem.currencyCode="SAR"
this.invoiceItem.identifier="Purchase"
this.invoiceItem.uom="1"
this.invoiceItem.vatCode= this.invoiceItem.vatRate==15?"S":"Z"

this.invoiceItems.push(this.invoiceItem);
    this.invoiceItem = new CreateOrEditPurchaseEntryItemDto();
this.calculateInvoiceSummary()
}

deleteItem(index: number) {
this.invoiceItems.splice(index, 1);

this.calculateInvoiceSummary()
}

clearItem(){
  this.invoiceItem = new CreateOrEditPurchaseEntryItemDto();
 // this.product = new CreateOrEditProductDto();
  this.invoiceItem.discountPercentage=0;
}


calculateInvoiceSummary(){
 
 
  this.invoiceSummary.totalAmountWithVAT=0
    this.invoiceSummary.totalAmountWithoutVAT=0 
    this.invoiceSummary.sumOfInvoiceLineNetAmount=0
    this.invoiceSummary.netInvoiceAmount=0
    this.discount=0

  this.invoiceItems.forEach(invoiceItem=>{
    console.log(invoiceItem,this.invoiceSummary)
    this.invoiceSummary.totalAmountWithVAT+=invoiceItem.lineAmountInclusiveVAT
    this.invoiceSummary.totalAmountWithoutVAT+= (invoiceItem.quantity * invoiceItem.unitPrice - invoiceItem.discountAmount)
    this.invoiceSummary.sumOfInvoiceLineNetAmount+=invoiceItem.netPrice
    this.invoiceSummary.netInvoiceAmount += (invoiceItem.quantity * invoiceItem.unitPrice)
    this.discount+= invoiceItem.discountAmount
    this.invoiceSummary.totalVATAmount=this.invoiceSummary.totalAmountWithVAT-this.invoiceSummary.totalAmountWithoutVAT
  })


}




fillDummy(){
  
this.invoice.status="Paid"
this.invoice.paymentType="Cash"
this.invoice.dateOfSupply = this.invoice.issueDate
this.invoice.invoiceNumber="-1"
this.invoice.invoiceCurrencyCode="SAR"
this.invoice.buyer.crNumber="1234567890"
this.invoice.supplier.crNumber="1234567890"
this.invoice.invoiceSummary.currencyCode="SAR"
this.invoice.invoiceSummary.paidAmountCurrency="SAR"
this.invoice.invoiceSummary.payableAmountCurrency="SAR"
this.invoice.invoiceSummary.netInvoiceAmountCurrency="SAR"
this.invoice.invoiceSummary.totalAmountWithoutVATCurrency="SAR"

this.invoice.supplier.contactPerson.name = this.invoice.supplier.contactPerson.name == null ? "Admin" : this.invoice.supplier.contactPerson.name;
this.invoice.supplier.contactPerson.contactNumber = this.invoice.supplier.contactPerson.contactNumber == null ? "567898084" : this.invoice.supplier.contactPerson.contactNumber;
this.invoice.supplier.address.city = this.invoice.supplier.address.city == null ? "Bengaluru" : this.invoice.supplier.address.city;
this.invoice.supplier.contactPerson.type = this.customer.contactPerson.type == null ? "NA" : this.customer.contactPerson.type;
this.invoice.supplier.address.type = this.invoice.supplier.address.type == null ? "NA" : this.invoice.supplier.address.type;
this.invoice.supplier.address.additionalNo = this.invoice.supplier.address.additionalNo == null ? "Gurappa Ave" : this.invoice.supplier.address.additionalNo;
this.invoice.supplier.address.countryCode = this.invoice.supplier.address.countryCode == null ? "UAE" : this.invoice.supplier.address.countryCode;
this.invoice.supplier.address.postalCode = this.invoice.supplier.address.postalCode == null ? "560025" : this.invoice.supplier.address.postalCode;
this.invoice.supplier.address.state = this.invoice.supplier.address.state == null ? "Karnataka" : this.invoice.supplier.address.state;
this.invoice.supplier.address.street = this.invoice.supplier.address.street == null ? "Diamond House, OFF" : this.invoice.supplier.address.street;
this.invoice.supplier.address.buildingNo = this.invoice.supplier.address.buildingNo == null ? "11" : this.invoice.supplier.address.buildingNo;
this.invoice.supplier.address.neighbourhood = this.invoice.supplier.address.neighbourhood == null ? "Ashok Nagar" : this.invoice.supplier.address.neighbourhood;

this.invoice.buyer.contactPerson.name = this.customer.registrationName == null ? "NA" : this.customer.registrationName;
this.invoice.buyer.contactPerson.type = this.customer.contactPerson.type == null ? "NA" : this.customer.contactPerson.type;
this.invoice.buyer.address.type = this.address.type == null ? "NA" : this.address.type;
this.invoice.buyer.address.additionalNo = this.address.additionalNo == null ? "NA" : this.address.additionalNo;
this.invoice.buyer.address.city = this.invoice.buyer.address.city == null ? "NA" : this.invoice.buyer.address.city;
this.invoice.buyer.address.countryCode = this.invoice.buyer.address.countryCode == null ? "NA" : this.invoice.buyer.address.countryCode;
this.invoice.buyer.address.postalCode = this.invoice.buyer.address.postalCode == null ? "NA" : this.invoice.buyer.address.postalCode;
this.invoice.buyer.address.state = this.invoice.buyer.address.state == null ? "NA" : this.invoice.buyer.address.state;
this.invoice.buyer.address.street = this.invoice.buyer.address.street == null ? "NA" : this.invoice.buyer.address.street;
this.invoice.buyer.address.state = this.invoice.buyer.address.state == null ? "NA" : this.invoice.buyer.address.state;
this.invoice.buyer.address.buildingNo = this.invoice.buyer.address.buildingNo == null ? "NA" : this.invoice.buyer.address.buildingNo;
this.invoice.buyer.address.neighbourhood = this.invoice.buyer.address.neighbourhood == null ? "NA" : this.invoice.buyer.address.neighbourhood;


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


  save(){
    if(this.isFormValid())
    {

    this.isSaving= true;
    if (this.invoiceItems.length<=0) {
      this.notify.error(this.l('Please add at least one item to save.'));
      this.isSaving= false;
      return;
  }
     this.invoice.invoiceSummary = this.invoiceSummary
     this.invoice.items = this.invoiceItems
     this.invoice.supplier = this.supplier
     this.invoice.buyer = this.customer
    //this.invoice.issueDate = this.parseDate(this.issueDate.toString())
          try {
        this.invoice.issueDate = this.parseDate(this.issueDate.toString())
      } catch (e) {
        this.notify.error(this.l('Please enter valid issue date.'));
        this.isSaving = false;
        return

      }
    this.invoice.supplier.address= new CreateOrEditSalesInvoiceAddressDto()
this.invoice.buyer.address= this.address

    this.fillDummy()
    console.log(this.invoice)
    this._salesInvoiceServiceProxy.createPurchaseEntry(this.invoice).subscribe((result) => {
      // this._salesInvoiceServiceProxy.insertPurchaseReportData(Number(result.invoiceId)).subscribe((result) => {
      // });
        this.notify.success(this.l('SavedSuccessfully'));
        this.editMode = false;
        this.isSaving= false;

        window.location.reload();

    });

  }

else{
  this.notify.error(this.l('Please fill all the required fields'));      

}

  }
}
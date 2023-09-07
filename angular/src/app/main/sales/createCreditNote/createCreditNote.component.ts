import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GetCountryForViewDto,CountryServiceProxy, CreditNoteServiceProxy, CreateOrEditCreditNoteItemDto,CreateOrEditCreditNotePartyDto, CreateOrEditSalesInvoiceItemDto, CreateOrEditCreditNoteSummaryDto, CreateOrEditCreditNoteVATDetailDto, CreateOrEditCreditNoteDiscountDto, CreateOrEditCreditNotePaymentDetailDto, CreateOrEditCreditNoteContactPersonDto, CreditNoteAddressDto, CreateOrEditSalesInvoiceAddressDto,SalesInvoicesServiceProxy , CreateOrEditCreditNoteDto } from '@shared/service-proxies/service-proxies';
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
  styleUrls:['./createCreditNote.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class CreateCreditNoteComponent extends AppComponentBase {

date=new Date();
month= (this.date.getMonth()+1).toString().length>1?this.date.getMonth()+1:"0"+(this.date.getMonth()+1)
day= (this.date.getDate()).toString().length>1?this.date.getDate():"0"+(this.date.getDate())
  maxDate = this.date.getFullYear()+"-"+this.month+"-"+this.day;
  editMode: boolean = false;
  dateofsupply: DateTime;
  issueDate=new Date().toISOString().slice(0, 10);;

  profileType: string = "";

  discount:number=0.0

  invoice: CreateOrEditCreditNoteDto = new CreateOrEditCreditNoteDto();
  supplier: CreateOrEditCreditNotePartyDto = new CreateOrEditCreditNotePartyDto();
  customer:CreateOrEditCreditNotePartyDto = new CreateOrEditCreditNotePartyDto();
  countries: GetCountryForViewDto[] = [];

  invoiceItems:CreateOrEditCreditNoteItemDto[] = [];
  invoiceItem: CreateOrEditCreditNoteItemDto = new CreateOrEditCreditNoteItemDto();

  invoiceSummary:CreateOrEditCreditNoteSummaryDto = new CreateOrEditCreditNoteSummaryDto();
  vatDetails:CreateOrEditCreditNoteVATDetailDto[] = [];
  vatDetail:CreateOrEditCreditNoteVATDetailDto = new CreateOrEditCreditNoteVATDetailDto();

  discountDetails:CreateOrEditCreditNoteDiscountDto[] = [];
  discountDetail:CreateOrEditCreditNoteDiscountDto = new CreateOrEditCreditNoteDiscountDto();

  paymentDetails:CreateOrEditCreditNotePaymentDetailDto[] = [];
  paymentDetail:CreateOrEditCreditNotePaymentDetailDto = new CreateOrEditCreditNotePaymentDetailDto();

  //product: CreateOrEditProductDto = new CreateOrEditProductDto();

  //products: GetProductForViewDto[] = [];
  //filteredProducts: GetProductForViewDto[] = [];

  address: CreditNoteAddressDto = new CreditNoteAddressDto();
  billingReferenceIds: string[] = [];
  irnNos: string[] = [];
  //salesInvoice: GetInvoiceDto = new GetInvoiceDto();
  basicForm: FormGroup;
  invoiceCount:number=0
  editItemIndex: number;
  editQuantity: number;
  originalInvoiceItems: CreateOrEditCreditNoteItemDto[];
  isSaving: boolean;
  editWithoutBillingRefId : boolean = false;
 
  constructor(
    injector: Injector,
    private fb: FormBuilder,
    //private _customerServiceProxy: CustomerServiceProxy,
    //private _productsServiceProxy: ProductsServiceProxy,
    private _CreditNoteServiceProxy: CreditNoteServiceProxy,
    //private _invoiceHeaderServiceProxy: InvoiceHeadersServiceProxy,
    private _salesInvoiceServiceProxy: SalesInvoicesServiceProxy,
    private _masterCountriesServiceProxy: CountryServiceProxy,
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
      //basic Form
              Name: ['', Validators.required ],
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
              ContactNo: ['', [Validators.pattern("^[0-9]*$"), Validators.required] ],
              vatid:['', [
                Validators.maxLength(15),
                Validators.minLength(15),
              Validators.pattern("^3[0-9]*3$")]]
            });
  }

   //check if form is valid
    isFormValid() {
        return(this.basicForm.valid);
    }
//ngoninit

  ngOnInit(): void {
    this.address.countryCode=null;
    this.getCountriesDropdown();
    this.create();
    this.getProducts();
    
    this.invoiceSummary.totalAmountWithVAT = 0
    this.invoiceSummary.totalAmountWithoutVAT = 0
    this.invoiceSummary.sumOfInvoiceLineNetAmount = 0
    this.invoiceSummary.netInvoiceAmount = 0
    this.discount = 0
    this.invoiceItem.quantity = 0
    this.invoiceItem.vatRate=15;
    this.invoiceItem.discountPercentage=0;
    this.supplier.registrationName=this._sessionService.tenant.name;
    this.supplier.vatid="5672345"
    this.supplier.contactPerson.email=this._sessionService.user.emailAddress
   this.supplier.contactPerson.contactNumber="567898084"
    this.invoice.customerId="567"
    this.invoiceSummary.sumOfInvoiceLineNetAmountCurrency ="SAR"
this.invoice.location="Ind",
    //get all invoice headers
    this._salesInvoiceServiceProxy.getAll("", "", "", undefined, undefined, undefined,undefined, "", "", "", "", "", undefined, undefined, "", "", "", "", 0,0, 0, "", "","","","","",0, 1234567).subscribe((result) => {
      // console.log(result);
    });


  } 

     //get countries from master data
     getCountriesDropdown() {
      this._masterCountriesServiceProxy.getAll("","","","","",undefined,undefined,"","",undefined,undefined,undefined,undefined,200)
        .subscribe(result => {
          this.countries = result.items;
        });
    }


          //--------------------------------------auto complete starts---------------------------------------------------------

          filterInvoices(event): void 
          {
            this._salesInvoiceServiceProxy.getInvoiceSuggestions(event.query).subscribe((data) => {
              if(data== null || data == undefined || data.length==0)
              {
                this.invoice.billingReferenceId = event.query
              }
              else{

              for(var i=0;i<data.length;i++)
              {
                data[i]=(data[i].irnNo+' '+(data[i].issueDate).toString());
              }

              this.billingReferenceIds = data;
            }  
              

              //event.query.split(" ")[0]
   
          });
          }
        
          onSelectInvoice(event): void
          {
            this._salesInvoiceServiceProxy.getsalesdetails(event.split(" ")[0]).subscribe((data) => {
              this.invoice.billingReferenceId =data[0].irnNo
              this.invoice.invoiceNumber=data[0].irnNo;
              this.dateofsupply=data[0].issueDate;
              this.customer.registrationName = data[0].name;
              this.customer.contactPerson.contactNumber=data[0].contactNumber;
              this.customer.contactPerson.email=data[0].email;
              this.customer.vatid=data[0].vatid;
              this.address.buildingNo = data[0].buildingNo;
              this.address.street = data[0].street?.trim();
              this.address.additionalStreet = data[0].additionalStreet?.trim();
              this.address.city = data[0].city?.trim();
              this.address.state = data[0].state;
              this.address.countryCode = data[0].countryCode;
              this.address.postalCode = data[0].postalCode;
              this.address.neighbourhood = data[0].neighbourhood?.trim();
              
             // this.customer.vatid = this..vat;
              // this.customer.contactPerson.email = this.salesInvoice.customerEmail;
              // this.customer.contactPerson.contactNumber = this.salesInvoice.conatctNo;
              //   this.invoice.billingReferenceId=event.split(" ")[0];
              // this.originalInvoiceItems = JSON.parse(JSON.stringify(this.invoiceItems));
              // this.invoiceCount = this.invoiceItems.length;
              // this.calculateInvoiceSummary();
           });
           this.invoiceItems=[];
           this._salesInvoiceServiceProxy.getsalesitemdetail(event.split(" ")[0]).subscribe((data) => {
            for(var i=0;i<data.length;i++)
            {

              this.invoiceItem.costPrice=data[i].costPrice;
              this.invoiceItem.description=data[i].description;
              this.invoiceItem.name=data[i].name;
              this.invoiceItem.discountAmount=data[i].discountAmount;
              this.invoiceItem.discountPercentage=data[i].discountPercentage;
              this.invoiceItem.grossPrice=data[i].grossPrice;
              this.invoiceItem.lineAmountInclusiveVAT=data[i].lineAmountInclusiveVAT;
              this.invoiceItem.quantity=data[i].quantity
              this.invoiceItem.unitPrice=data[i].unitPrice;
              this.invoiceItem.netPrice=data[i].netPrice;
              this.invoiceItem.vatAmount=data[i].vatAmount;
              this.invoiceItem.vatRate=data[i].vatRate


              this.invoiceItems.push(this.invoiceItem)
              this.originalInvoiceItems = JSON.parse(JSON.stringify(this.invoiceItems));
              this.invoiceCount = this.invoiceItems.length;
              this.invoiceItem = new CreateOrEditSalesInvoiceItemDto();

            }
            this.calculateInvoiceSummary()

         });
          }
        
                  //--------------------------------------auto complete ends---------------------------------------------------------
        


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
    // this.filteredProducts = JSON.parse(JSON.stringify(this.products)).filter(function (str) { 
    //   return str.product.name.indexOf(name||"") === -1; });
    //   console.log(this.filteredProducts);
  }


  create() {
    this.customer.contactPerson = new CreateOrEditCreditNoteContactPersonDto();
  this.supplier.contactPerson = new CreateOrEditCreditNoteContactPersonDto();

  }

  //add item to invoiceitems
  addItem() {  
    // if(this.invoiceItems.length + 1 > this.invoiceCount && this.invoiceCount != 0 && !this.editMode ){
    //   this.message.warn("You can't add more items");
    //   return;
    // }
    if(this.invoiceItem.quantity>this.editQuantity && this.invoiceCount != 0 && this.editMode ){
      this.invoiceItem.quantity=this.editQuantity;
      this.message.warn("Quantity cannot exceed "+this.editQuantity);
      return;
    }
    if(this.invoiceItem.quantity<0 ){
      this.message.warn("Quantity cannot be less than 0 ");
      return;
    }
    if(this.invoiceItem.unitPrice<0 ){
      this.message.warn("Rate cannot be less than 0 ");
      return;
    }
    if(this.invoiceItem.discountPercentage<0 || this.invoiceItem.discountPercentage>100 ){
      this.message.warn("Discount  must be in between 0 to 100");
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
    if(this.address.countryCode!='SA' && this.invoiceItem.vatRate!=0)
    {
      this.notify.error(this.l('Exports are exempt from VAT'));
      return;
    }
 this.invoiceItem.discountAmount = this.invoiceItem.quantity * this.invoiceItem.unitPrice * this.invoiceItem.discountPercentage /100.0
 this.invoiceItem.vatAmount = ((this.invoiceItem.quantity * this.invoiceItem.unitPrice) - this.invoiceItem.discountAmount)* this.invoiceItem.vatRate /100.0
 this.invoiceItem.lineAmountInclusiveVAT = ((this.invoiceItem.quantity * this.invoiceItem.unitPrice) - this.invoiceItem.discountAmount) + this.invoiceItem.vatAmount
 this.invoiceItem.netPrice = this.invoiceItem.lineAmountInclusiveVAT-this.invoiceItem.vatAmount;
this.invoiceItem.currencyCode="SAR"
this.invoiceItem.identifier="Sales"
this.invoiceItem.vatCode=this.invoiceItem.vatRate==15?"S":"Z"
this.invoiceItem.uom="test"
if(this.editMode){
  this.invoiceItems[this.editItemIndex] = this.invoiceItem;
}else{
this.invoiceItems.push(this.invoiceItem);
}
    this.invoiceItem = new CreateOrEditSalesInvoiceItemDto();

this.calculateInvoiceSummary()
this.invoiceItem.quantity = 0
this.invoiceItem.vatRate=15;
this.invoiceItem.discountPercentage=0;
this.editMode = false;
}

deleteItem(index: number) {
this.invoiceItems.splice(index, 1);
this.calculateInvoiceSummary()
}

editItem(index: number) {
  this.invoiceItem = this.invoiceItems[index];
  this.editItemIndex = index;
  //this.invoiceItems.splice(index, 1);
  this.editMode=true
  if(this.invoice.billingReferenceId!=null || this.invoice.billingReferenceId!=undefined)
  {
    this.editWithoutBillingRefId = false;
  }else{
    this.editWithoutBillingRefId = true;
  }
  this.editQuantity = this.originalInvoiceItems[index].quantity;
  this.calculateInvoiceSummary()
  }

clearItem(){
  this.invoiceItem = new CreateOrEditSalesInvoiceItemDto();
  //this.product = new CreateOrEditProductDto();
  this.invoiceItem.discountPercentage=0;
  this.editMode = false;
}


calculateInvoiceSummary(){
 
 
  this.invoiceSummary.totalAmountWithVAT=0
    this.invoiceSummary.totalAmountWithoutVAT=0 
    this.invoiceSummary.sumOfInvoiceLineNetAmount=0
    this.invoiceSummary.netInvoiceAmount=0
    this.discount=0

  this.invoiceItems.forEach(invoiceItem=>{
    this.invoiceSummary.totalAmountWithVAT+=invoiceItem.lineAmountInclusiveVAT
    this.invoiceSummary.totalAmountWithoutVAT+= (invoiceItem.quantity * invoiceItem.unitPrice - invoiceItem.discountAmount)
    this.invoiceSummary.sumOfInvoiceLineNetAmount+=invoiceItem.netPrice
    this.invoiceSummary.netInvoiceAmount += (invoiceItem.quantity * invoiceItem.unitPrice)
    this.discount+= invoiceItem.discountAmount
  })


}




fillDummy(){
this.invoice.status="Paid"
this.invoice.paymentType="Cash"
// this.invoice.dateOfSupply = "12/3/21"
this.invoice.invoiceNumber='-1';
this.invoice.invoiceCurrencyCode="SAR"
this.invoice.dateOfSupply=this.dateofsupply;

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
// this.invoice.buyer.crNumber="1234567890"
// this.invoice.supplier.crNumber="1234567890"
// this.invoice.invoiceSummary.currencyCode="SAR"
// this.invoice.invoiceSummary.paidAmountCurrency="SAR"
// this.invoice.invoiceSummary.payableAmountCurrency="SAR"
// this.invoice.invoiceSummary.netInvoiceAmountCurrency="SAR"
// this.invoice.invoiceSummary.totalAmountWithoutVATCurrency="SAR"
// this.invoice.invoiceSummary.sumOfInvoiceLineNetAmountCurrency="SAR"
// this.invoice.supplier.contactPerson.name="test"
// this.invoice.buyer.contactPerson.name="test"
// this.invoice.supplier.contactPerson.type="test"
// this.invoice.buyer.contactPerson.type="test"


// this.invoice.supplier.address.type="test"
// this.invoice.supplier.address.additionalNo="test"
// this.invoice.supplier.address.city="test"
// this.invoice.supplier.address.countryCode="test"
// this.invoice.supplier.address.postalCode="test"
// this.invoice.supplier.address.state="test"
// this.invoice.supplier.address.street="test"
// this.invoice.supplier.address.state="test"
// this.invoice.supplier.address.buildingNo="test"
// this.invoice.supplier.address.neighbourhood="test"

// this.invoice.invoiceSummary.id=null
// this.invoice.buyer.id=null
// this.invoice.buyer.address.id=null
// this.invoice.supplier.id=null
// this.invoice.supplier.address.id=null
// this.invoice.supplier.contactPerson.id=null;
// this.invoice.buyer.contactPerson.id=null;


// this.invoice.items.forEach(item => {
//   item.id = null;
// });




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
    this.isSaving = true;
    if (this.invoiceItems.length<=0) {
      this.notify.error(this.l('Please add at least one item to save.'));
      this.isSaving = false;
      return;
  }
  if (this.invoice.billingReferenceId == undefined ||  this.invoice.billingReferenceId == null) {
    this.notify.error(this.l('Please enter Reference Invoice Number.'));
    this.isSaving = false;
    return;
}
  if(!this.invoice.additional_Info?.trim()){
    this.notify.error(this.l('Please enter the reason for credit note.'));
    this.isSaving = false;
    return;
  }
    this.invoice.invoiceSummary = this.invoiceSummary
     this.invoice.items = this.invoiceItems
    this.invoice.supplier = this.supplier
     this.invoice.buyer = this.customer


    try{
    this.invoice.issueDate = this.parseDate(this.issueDate.toString())
    }catch(e){
      this.notify.error(this.l('Please enter valid issue date.'));
      this.isSaving = false;

      return;
    }

   this.invoice.buyer.address= new CreateOrEditSalesInvoiceAddressDto()
 this.invoice.buyer.address= this.address
    this.invoice.supplier.address= new CreateOrEditSalesInvoiceAddressDto()

    this.fillDummy()
    // console.log(this.invoice)
    this._CreditNoteServiceProxy.createCreditNote(this.invoice).subscribe((result) => {
      // this._CreditNoteServiceProxy.insertCreditReportData(Number(result.invoiceId)).subscribe((result) => {
      // });
        this.notify.success(this.l('SavedSuccessfully'));
        this.isSaving = false;
        this._router.navigate(['/app/main/transactions'],{state: {tabvaule:'Credit Note'}});
     this.download(result.invoiceId,result.uuid)

    });
  }
  else{
    this.notify.error(this.l('Please fill all the required fields'));

  }

  }
download(id,uid){
  //window.location.reload();
  var pdfUrl = AppConsts.pdfUrl + '/InvoiceFiles/'+this._sessionService.tenantId+"/"+uid+"/"+uid+"_"+id+".pdf";
  window.open(pdfUrl)
}
  refresh(){
  //reload page
  location.reload();


  }

}
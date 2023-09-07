import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetSectorForViewDto, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { profile } from 'console';
import {CustomerComponent} from '@app/main/customers/customers.component'
import {Location} from '@angular/common';
import {CustomerDocumentsDto,
  GetTaxCategoryForViewDto,
  TaxCategoryServiceProxy,
  CreateOrEditCustomerTaxDetailsDto,
  GetInvoiceTypeForViewDto,
  TransactionCategoryServiceProxy,
  InvoiceTypeServiceProxy,
  BusinessOperationalModelServiceProxy,
  GetBusinessOperationalModelForViewDto,
  GetTransactionCategoryForViewDto,
  CreateOrEditCustomerDocumentsDto,CustomerAddressDto,SectorServiceProxy,GetDocumentMasterForViewDto,ConstitutionServiceProxy,GetConstitutionForViewDto,DocumentMasterDto,DocumentMasterServiceProxy,CountryServiceProxy,GetCountryForViewDto,CustomersesServiceProxy,CreateOrEditCustomersDto} from '@shared/service-proxies/service-proxies';



import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { CustomerModule } from '@app/main/customers/customers.module';


@Component({
    selector:'app-Individual',
  templateUrl: './Individual.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class IndividualComponent extends AppComponentBase {
  @Input() id : number;
  editMode: boolean = true;

  profileType: string = "Individual";

  customer: CreateOrEditCustomersDto = new CreateOrEditCustomersDto();
  vatid:number;
  address: CustomerAddressDto = new CustomerAddressDto();

  Documents: CustomerDocumentsDto = new CustomerDocumentsDto();

  Documentitems: CreateOrEditCustomerDocumentsDto[] = [];
  Documentitem: CreateOrEditCustomerDocumentsDto = new CreateOrEditCustomerDocumentsDto();
  taxdetails:CreateOrEditCustomerTaxDetailsDto= new CreateOrEditCustomerTaxDetailsDto();
  doctype:GetDocumentMasterForViewDto[]=[];
  transtype:GetTransactionCategoryForViewDto[]=[];
  operationaltype:GetBusinessOperationalModelForViewDto[]=[];
  taxcat:GetTaxCategoryForViewDto[]=[];
  invoicetype:GetInvoiceTypeForViewDto[]=[];
  countries: GetCountryForViewDto[] = [];
  constitutionTypes: GetConstitutionForViewDto[] = [];
  sectors: GetSectorForViewDto[] = [];
  companyRequiredForm: FormGroup;
  individualRequiredForm: FormGroup;
  addressRequiredForm: FormGroup;
  isSaving: boolean;
  basicForm: FormGroup;
  loadindividualdt:boolean=false;
  loadindividualadd:boolean=false;
  loadIndividualReg:boolean=false;
  uniqueidvat:string;

  

  tenantForm() {
    this.basicForm = this.fb.group({
      //basic Form
              Name: ['', Validators.required ],
              Buildno: ['', [Validators.required,
                Validators.maxLength(4),
              Validators.pattern("^[0-9]*$") ]],
              AdditionalBno: ['', [] ],
              street: ['', Validators.required ],
              Additnalstreet: ['', [] ],
              Neighbourhood:['',Validators.required],
              pin: ['', [Validators.required,
                Validators.maxLength(5),
            Validators.pattern("^[0-9]*$") ] ],
              city: ['', Validators.required ],
              state: ['', Validators.required ],
              nationality: ['', Validators.required ],
              ContactNo: ['', Validators.required ],
              vatid:['', [
                Validators.maxLength(15),
                Validators.minLength(15),
              Validators.pattern("^3[0-9]*3$"),
            Validators.required]]
            });

            this.companyRequiredForm = this.fb.group({
              //basic Form
              operatingModel: ['', Validators.required ],
              businessCategory: ['', Validators.required ],
              businesssupplies: ['', Validators.required ],
              vatcategory: ['', Validators.required ],
              invoiceType: ['', Validators.required ]
                    });
  }

    isFormValid() {
        return(this.basicForm.valid);
    }
 
  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _customerServiceProxy: CustomersesServiceProxy,
    private _masterCountriesServiceProxy: CountryServiceProxy,
    private _masterDocumentsServiceProxy: DocumentMasterServiceProxy,
    private _masterInvoiceTypeServiceProxy: InvoiceTypeServiceProxy,
   private _masterSectorServiceProxy: SectorServiceProxy,
    private _masterConstitutionServiceProxy: ConstitutionServiceProxy,
    private _masterBusinessCategoryServiceProxy: TransactionCategoryServiceProxy,
    private _masterBusinessOperationalModelServiceProxy: BusinessOperationalModelServiceProxy,
    private _masterTaxCategoryServiceProxy: TaxCategoryServiceProxy,

    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService,
    private _dateTimeService: DateTimeService,
    private _router: Router,
    private _location: Location
  ) {
    super(injector);
    this.tenantForm();
  }




  ngOnInit(): void {
    this.getCountriesDropdown();
    this.getdoctypeDropdown();
    this.getinvoicetypeDropdown();
    this.getbusinessCategoryDropdown();
    this.getopertaionalDropdown();
    this.gettaxcategoryDropdown();
     this.getSector();
     this.getConstitutionType();
     this.getGender();
     this.getTitle();
     this.getAddressType();
     this.getFileType();
      this.getIdentifier();
      this.loadindividualdt=true;
      this.profileType='Individual';
      this.Documentitem.documentTypeCode=" ";
    this.show(this.id);
   }
 
   show(CustomerId?: number): void {
    if (!CustomerId) {
    } else {
      this._customerServiceProxy.getCustomerForEdit(CustomerId).subscribe((result) => {
        console.log(result);
        this.customer.id=CustomerId;
        this.customer.name=result[0].name;
        this.customer.nationality=result[0].nationality;
        this.customer.contactNumber=result[0].contactNumber;
        this.customer.emailID=result[0].emailID;
        this.address.buildingNo=result[0].buildingNo;
        this.address.additionalNo=result[0].additionalNo;
        this.address.street=result[0].street;
        this.address.additionalStreet=result[0].additionalStreet;
        this.address.city=result[0].city;
        this.address.state=result[0].state;
        this.address.postalCode=result[0].postalCode;
        this.address.neighbourhood=result[0].neighbourhood;
        this.taxdetails.businessCategory=result[0].businessCategory;
        this.taxdetails.businessSupplies=result[0].businessSupplies;
        this.taxdetails.salesVATCategory=result[0].salesVATCategory;
        this.taxdetails.invoiceType=result[0].invoiceType;
        this.taxdetails.operatingModel=result[0].operatingModel;
        for(var i=0;i<result.length;i++)
        {
          if(result[i].documentTypeCode=='VAT')
          {
            this.uniqueidvat=result[i].docunique;
            this.vatid=result[i].documentNumber;
          }
          else
          {
          this.Documentitem.uniqueId=result[i].docunique
          this.Documentitem.documentName=result[i].documentName;
          this.Documentitem.documentNumber=result[i].documentNumber;
          this.Documentitem.documentTypeCode=result[i].documentTypeCode;
          this.Documentitem.doumentDate=result[i].doumentDate;
          this.Documentitems.push(this.Documentitem);
          }
          this.Documentitem = new CreateOrEditCustomerDocumentsDto();
        }

        console.log(result,'e');
      });
    }
  }


   getCountriesDropdown() {
     this._masterCountriesServiceProxy.getAll("","","","","",undefined,undefined,"","",undefined,undefined,undefined,undefined,200)
       .subscribe(result => {
         this.countries = result.items;
       });
   }

   getdoctypeDropdown() {
    this._masterDocumentsServiceProxy.getAll("","","","",undefined,undefined,undefined,undefined,200)
      .subscribe(result => {
        this.doctype = result.items;
      });
  }

  getbusinessCategoryDropdown() {
    this._masterBusinessCategoryServiceProxy.getAll("","","","",undefined,undefined,undefined,200)
      .subscribe(result => {
        this.transtype = result.items;
      });
  }

  getopertaionalDropdown() {
    this._masterBusinessOperationalModelServiceProxy.getAll("","","","",undefined,undefined,undefined,200)
      .subscribe(result => {
        this.operationaltype = result.items;
      });
  }

  gettaxcategoryDropdown() {
    this._masterTaxCategoryServiceProxy.getAll("","","","",undefined,undefined,undefined,undefined,undefined,200)
      .subscribe(result => {
        this.taxcat = result.items;
      });
  }


  getinvoicetypeDropdown() {
    this._masterInvoiceTypeServiceProxy.getAll("","","","",undefined,undefined,undefined,200)
      .subscribe(result => {
        this.invoicetype = result.items;
      });
  }
 
   getSector() {
          this._masterSectorServiceProxy.getAll("","","",undefined,undefined,undefined,undefined,undefined,undefined,undefined,undefined,undefined,undefined,undefined,undefined)

       .subscribe(result => {
         this.sectors = result.items;
       });
   }
 
   getConstitutionType() {
     this._masterConstitutionServiceProxy.getAll("","","",undefined,undefined,undefined,undefined,undefined)
       .subscribe(result => {
         this.constitutionTypes = result.items;
       });
   }
   
   getGender() {
   }
 
   getTitle() {
   }
 
     getAddressType() {
     }
 
     getFileType() {
     }
 
     getIdentifier() {
     }
   
  
 
     mapFormtoDto() {
      this.customer.name = this.basicForm.get('Name').value || " ";
      this.customer.legalName ='NA'; //this.individualRequiredForm.get('firstName').value || " ";
      this.customer.tenantType = 'Individual';//this.individualRequiredForm.get('firstName').value || " ";
      this.customer.constitutionType = 'NA' //this.individualRequiredForm.get('firstName').value || " ";
      this.customer.contactNumber = //this.individualRequiredForm.get('firstName').value || " ";
      this.customer.contactPerson = this.basicForm.get('ContactNo').value || " ";
      //this.customer.emailID = this.individualRequiredForm.get('firstName').value || " ";
      this.customer.nationality = this.basicForm.get('nationality').value || " ";
      this.customer.designation = 'NA' //this.individualRequiredForm.get('firstName').value || " ";
      //this.customer.documents.documentNumber=this.basicForm.get('vatid').value || " ";
      this.address.additionalNo = this.basicForm.get('AdditionalBno').value || " ";
      this.address.additionalStreet = this.basicForm.get('Additnalstreet').value || " ";
      this.address.street = this.basicForm.get('street').value || " ";
      this.address.postalCode = this.basicForm.get('pin').value || " ";
      this.address.state = this.basicForm.get('state').value || " ";
      this.address.buildingNo = this.basicForm.get('Buildno').value || " ";
      this.address.city = this.basicForm.get('city').value || " ";
      this.address.countryCode=this.basicForm.get('nationality').value || " ";
      this.address.neighbourhood=this.basicForm.get('Neighbourhood').value || " ";

      if(this.customer.id>0){
      this.address.customerID=(this.customer.id).toString();
      }

      }
      mapDtotoForm() {
        this.customer.name = this.basicForm.get('Name').value || " ";

  
        }
    

   
   
    create() {
      this.mapFormtoDto();
        this.editMode = true;
      }
 
 
   save(){
    this.isSaving = true;
    if(!this.customer.id)
    {

    if(this.isFormValid()){
     this.create();
     console.log(this.Documentitems,'yy');
     for(var j=0;j<this.Documentitems.length;j++)
     {
      this.Documentitems[j].doumentDate=this.parseDate(this.Documentitems[j].doumentDate.toString())
    }
    this.Documentitem.uniqueId=this.uniqueidvat;
    this.Documentitem.documentNumber=this.vatid.toString();
    this.Documentitem.documentTypeCode='VAT'
    this.Documentitem.documentName='VAT'
    this.Documentitems.push(this.Documentitem);
     this.customer.address=this.address;
     this.customer.documents=this.Documentitems;
     this.customer.taxdetails=this.taxdetails;
     console.log(this.customer);
     this._customerServiceProxy.createCustomer(this.customer).subscribe(() => {
         this.notify.success(this.l('SavedSuccessfully'));
         this.editMode = true;
         this.gotoCustomer();
       
     });


    }else{
      this.notify.error(this.l('Please fill all the required fields'));
    }
  }
  else{
    if(this.isFormValid()){
       this.create();
       console.log(this.Documentitems,'ff')
       for(var j=0;j<this.Documentitems.length;j++)
       {
        this.Documentitems[j].doumentDate=this.parseDate(this.Documentitems[j].doumentDate.toString())
      }
      console.log(this.Documentitems,'f1')

      this.Documentitem.uniqueId=this.uniqueidvat;
      this.Documentitem.documentNumber=this.vatid.toString();
      this.Documentitem.documentTypeCode='VAT'
      this.Documentitem.documentName='VAT'
      this.Documentitems.push(this.Documentitem);
      console.log(this.Documentitems,'f2')

       this.customer.address=this.address;
       this.customer.documents=this.Documentitems;
       this.customer.taxdetails=this.taxdetails;
       console.log(this.customer);
       this._customerServiceProxy.upadateCustomer(this.customer) .subscribe(() => {
           this.notify.success(this.l('UpdatedSuccessfully'));
           this.editMode = true;
           this.gotoCustomer();
         
       });
  
  
      }else{
        this.notify.error(this.l('Please fill all the required fields'));
      }
  }

   this.isSaving = false;
   }

   gotoCustomer(): void {
    this._location.back();
    
  }

   reloadCurrentPage(){
    Swal.fire({
      title: 'Are you sure want clear data?',
      text: "You won't be able to revert this!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, clear it!'
    }).then((result) => {
      if (result.isConfirmed) {
      window.location.reload();
      }
    })  
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
  updateGrid(k:number){
    Swal.fire({
      title: "Do you want to overwrite the existing document details?",
      text: "",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, overwrite it!'
    }).then((result) => {
      if (result.isConfirmed) {
        Swal.fire(
          'Updated!',
          'Document details has been updated.',
          'success'
        )
        console.log(k)
        this.Documentitems[k].documentNumber=this.Documentitem?.documentNumber;
        this.Documentitems[k].doumentDate=this.Documentitem?.doumentDate;
        this.Documentitems[k].documentName=this.Documentitem?.documentName;       
        this.Documentitem = new CreateOrEditCustomerDocumentsDto();

      }
    })
   
  }


   additem(){
    if (!(this.Documentitem.documentName && this.Documentitem.documentNumber && this.Documentitem.doumentDate
      && this.Documentitem.documentTypeCode)) {

      this.notify.error(this.l('Please fill all  document details to add a document.'));
      return;
    }

    var UPDATED=false
    if(this.Documentitems.length>0)
    {
      for(var k=0;k<this.Documentitems.length;k++)
      {
          if(this.Documentitems[k].documentTypeCode==this.Documentitem.documentTypeCode)
          {
            this.updateGrid(k);
            UPDATED=true;
            break;
          }
          
      }
    
    }
    if(!UPDATED){
      this.Documentitems.push(this.Documentitem);
      this.Documentitem = new CreateOrEditCustomerDocumentsDto();
    }
   }


   clearitem(){
    this.Documentitem = new CreateOrEditCustomerDocumentsDto();
   }
   
   deleteItem(index: number) {
    this.Documentitems.splice(index, 1); 
  }
   loadIndividual(){
    this.loadindividualdt=true;
    this.loadindividualadd=false;
    this.loadIndividualReg=false;
   }

   loadAdd(){
    this.loadindividualdt=false;
    this.loadindividualadd=true;
    this.loadIndividualReg=false;
   }

   loadReg(){
    this.loadindividualdt=false;
    this.loadindividualadd=false;
    this.loadIndividualReg=true;
   }

   
  }



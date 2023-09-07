import { AppConsts } from '@shared/AppConsts';

import { Component, Injector, ViewEncapsulation, ViewChild, Output, EventEmitter } from '@angular/core';

import { ActivatedRoute, Router } from '@angular/router';

//import { DetailsServiceProxy, GetDetailForViewDto } from '@shared/service-proxies/service-proxies';

import { AppComponentBase } from '@shared/common/app-component-base';

import { appModuleAnimation } from '@shared/animations/routerTransition';

import { filter as _filter } from 'lodash-es';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { Table } from 'primeng/table';
import { ConstitutionServiceProxy, CountryServiceProxy, CreateOrEditTenantAddressDto, CreateOrEditTenantBasicDetailsDto, CreateOrEditTenantDocumentsDto, CustomersesServiceProxy,GetConstitutionForViewDto,GetCountryForViewDto,GetCustomersForViewDto, TenantBasicDetailsServiceProxy } from '@shared/service-proxies/service-proxies';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { Location } from '@angular/common';


@Component({
  selector:'tenantdetails',
 templateUrl: './tenantdetails.component.html',
encapsulation: ViewEncapsulation.None,
 animations: [appModuleAnimation()],

})

export class TenantDetailsComponent extends AppComponentBase {


  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
 //customers: GetCustomersForViewDto[] = [];
 customers:any[]=[];
 active = false;
 saving = false;
 filterText = '';
 searchQuery='';
 advancedFiltersAreShown = false;
 tenantid:number;
 type:string;
 nameFilter = '';
 descriptionFilter = '';
 codeFilter = '';
 isActiveFilter = -1;
 basicForm: FormGroup;
 constitutionTypes: GetConstitutionForViewDto[] = [];
 countries: GetCountryForViewDto[] = [];
 loadindividualdt:boolean=false;
 loadindividualadd:boolean=false;
 loadIndividualReg:boolean=false;
 tenants:CreateOrEditTenantBasicDetailsDto=new CreateOrEditTenantBasicDetailsDto();
 address:CreateOrEditTenantAddressDto=new CreateOrEditTenantAddressDto();
 documents:CreateOrEditTenantDocumentsDto=new CreateOrEditTenantDocumentsDto();


constructor(
 injector: Injector,
private _CustomerDetailsServiceProxy: CustomersesServiceProxy,
private _activatedRoute: ActivatedRoute,
private _location:Location,
private _masterCountriesServiceProxy:CountryServiceProxy,
private _masterConstitutionServiceProxy: ConstitutionServiceProxy,
private _tenantbasicdetailsServiceProxy : TenantBasicDetailsServiceProxy,
private _sessionService: AppSessionService,
private fb: FormBuilder,

) {
 super(injector);
 this.tenantForm();
 this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
}

ngAfterViewInit(): void {
  this.primengTableHelper.adjustScroll(this.dataTable);
}

tenantForm() {
    this.basicForm = this.fb.group({
      //basic Form
      businessCategory: ['', [] ],
      ConstitutionType: ['', [] ],
      TenantType: ['',[]],
      OperationalModel: ['', []],
      TurnoverSlab: ['', []],
      EmailID: ['', []],
      Designation: ['', [] ],
      VATID: ['', [] ],
      ParentEntityName: ['', [] ],
      LegalRepresentative: ['', [] ],
      ParentEntityCountryCode: ['', [] ],
      LastReturnFiled: ['', []],
      VATReturnFillingFrequency: ['', [] ],
      ContactPerson: ['', [] ],
      crnumber: ['', [] ],

      
              Buildno: ['', [Validators.required,
                Validators.maxLength(4),
              Validators.pattern("^[0-9]*$") ]],
              AdditionalBno: ['', Validators.required ],
              street: ['', Validators.required ],
              Additnalstreet: ['', Validators.required ],
              pin: ['', [Validators.required,
                Validators.maxLength(5),
            Validators.pattern("^[0-9]*$") ] ],
              city: ['', Validators.required ],
              state: ['', Validators.required ],
              nationality: ['', Validators.required ],
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

ngOnInit() {
  this.type="Update";
    this.loadindividualdt=true
    this.tenantid =  this._sessionService.tenantId
   //this.show(this.tenantid);
    this.getCountriesDropdown();
    this.getConstitutionType();

}


show(tenantId: number): void {
    this.active = true;



    this._tenantbasicdetailsServiceProxy.getTenantById(this.tenantid).subscribe((data) => {
        this.tenants.vatid = data[0].vatid?.trim();
        this.tenants.businessCategory = data[0].businessCategory?.trim();      
         this.tenants.contactPerson = data[0].contactPerson?.trim();
         this.tenants.contactNumber = data[0].contactNumber?.trim();
         this.tenants.constitutionType =data[0].constitutionType?.trim();
         this.tenants.tenantType=data[0].tenantType?.trim();
         this.tenants.turnoverSlab=data[0].turnoverslab?.trim();
         this.tenants.designation=data[0].designation?.trim();
         this.tenants.emailID=data[0].emailID?.trim();
         this.tenants.lastReturnFiled=data[0].lastReturnFiled?.trim();
         this.tenants.legalRepresentative=data[0].legalrepresentative?.trim();
         this.tenants.nationality=data[0].nationality?.trim();
         this.tenants.operationalModel=data[0].operationalModel?.trim();
         this.tenants.parentEntityCountryCode=data[0].parententitycountrycode?.trim();
         this.tenants.parentEntityName=data[0].parententityname?.trim();
         this.tenants.vatReturnFillingFrequency=data[0].vatReturnFillingFrequency?.trim();
        this.address.buildingNo = data[0].buildingNo;
        this.address.additionalBuildingNumber = data[0].additionalBuildingNumber;
        this.address.street = data[0].street?.trim();
        this.address.additionalStreet = data[0].additionalStreet?.trim();
        this.address.city = data[0].city?.trim();
        this.address.state = data[0].state;
        this.address.country = data[0].country;

        this.address.postalCode = data[0].postalCode;
        this.address.neighbourhood = data[0].neighbourhood?.trim();  
        this.documents.documentNumber=data[0].documentNumber;
    this.tenants.address=this.address;
// this.tenants.documents=this.documents;   
  });
}


update(){
    // this.tenants.address=this.address;
    // this.tenants.documents=this.documents;
    // this._tenantbasicdetailsServiceProxy.upadateTenant(this.tenants)
    // .pipe(finalize(()=>(
    //     this.saving = false

    // ))) 
    // .subscribe(() => {
    //     this.notify.success(this.l('UpdatedSuccessfully'));
    // });
}

getConstitutionType() {
    this._masterConstitutionServiceProxy.getAll("","","",undefined,undefined,undefined,undefined,undefined)
      .subscribe(result => {
        this.constitutionTypes = result.items;
      });
  }

getCountriesDropdown() {
    this._masterCountriesServiceProxy.getAll("","","","","",undefined,undefined,"","",undefined,undefined,undefined,undefined,200)
      .subscribe(result => {
        this.countries = result.items;
      });
  }


  close(): void {
    //this.active = false;
    //this.modal.hide();
    this._location.back();

}

search(){
}
resetFilters(): void {
  this.filterText = '';
  this.nameFilter = '';

 
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
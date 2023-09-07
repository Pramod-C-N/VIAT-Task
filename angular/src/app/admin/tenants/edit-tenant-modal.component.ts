import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { FormGroup,Validators,FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    CommonLookupServiceProxy,
    SubscribableEditionComboboxItemDto,
    TenantEditDto,
    TenantServiceProxy,
    CountryServiceProxy,
    GetCountryForViewDto,
    GetConstitutionForViewDto,
    ConstitutionServiceProxy,
    TenantBasicDetailsServiceProxy,
    CreateOrEditTenantBasicDetailsDto,
    CreateOrEditTenantAddressDto,
    CreateOrEditTenantDocumentsDto,
    GetBusinessTurnoverSlabForViewDto,
    BusinessTurnoverSlabServiceProxy,
    GetBusinessOperationalModelForViewDto,
    BusinessOperationalModelServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { timeStamp } from 'console';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { threadId } from 'worker_threads';
import { Location } from '@angular/common';

@Component({
    selector: 'editTenantModal',
    templateUrl: './edit-tenant-modal.component.html',
})
export class EditTenantModalComponent extends AppComponentBase {
    @ViewChild('nameInput', { static: true }) nameInput: ElementRef;
    @ViewChild('editModal', { static: true }) modal: ModalDirective;
    @ViewChild('SubscriptionEndDateUtc') subscriptionEndDateUtc: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    isUnlimited = false;
    subscriptionEndDateUtcIsValid = false;
    type:string;
    tenant: TenantEditDto = undefined;
    currentConnectionString: string;
    editions: SubscribableEditionComboboxItemDto[] = [];
    isSubscriptionFieldsVisible = false;
    tenantid:number;
    constitutionTypes: GetConstitutionForViewDto[] = [];
    basicForm: FormGroup;
    isSelectedEditionFree = false;
    tenantAdminPasswordRepeat = '';
    countries: GetCountryForViewDto[] = [];
    loadindividualdt:boolean=false;
    loadindividualadd:boolean=false;
    loadIndividualReg:boolean=false;
    tenants:CreateOrEditTenantBasicDetailsDto=new CreateOrEditTenantBasicDetailsDto();
    address:CreateOrEditTenantAddressDto=new CreateOrEditTenantAddressDto();
    documents:CreateOrEditTenantDocumentsDto=new CreateOrEditTenantDocumentsDto();
    turnoverslabs:GetBusinessTurnoverSlabForViewDto[]=[];
    OperationalModels:GetBusinessOperationalModelForViewDto[]=[];


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

    constructor(
        injector: Injector,
        private fb: FormBuilder,
        private _location:Location,
        private _router: Router,
        private _tenantService: TenantServiceProxy,
        private _commonLookupService: CommonLookupServiceProxy,
        private _dateTimeService: DateTimeService,
        private _activatedRoute: ActivatedRoute,
        private _masterCountriesServiceProxy:CountryServiceProxy,
        private _masterConstitutionServiceProxy: ConstitutionServiceProxy,
        private _tenantbasicdetailsServiceProxy : TenantBasicDetailsServiceProxy,
        private _BusinessTurnoverSlabServiceProxy :BusinessTurnoverSlabServiceProxy,
        private _BusinessOperationalModelServiceProxy :BusinessOperationalModelServiceProxy



    ) {
        super(injector);
        this.tenantForm();
    }


    ngOnInit(): void {
        this.type="Update";
        this.tenantid=this._activatedRoute.snapshot.queryParams['id'];

        this.show(this.tenantid);
        this.getCountriesDropdown();
        this.getConstitutionType();
        this.getTurnoverslabDropdown();
        this.getoperationalModelDropdown();



        this.loadindividualdt=true;
    }
    getoperationalModelDropdown() {
        this._BusinessOperationalModelServiceProxy.getAll(undefined,"","",undefined,undefined,undefined,undefined,200)
          .subscribe(result => {
            this.OperationalModels = result.items;
          });
      }

    getTurnoverslabDropdown() {
        this._BusinessTurnoverSlabServiceProxy.getAll(undefined,"","",undefined,undefined,undefined,undefined,200)
          .subscribe(result => {
            this.turnoverslabs = result.items;
          });
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

    show(tenantId: number): void {
        this.active = true;

        this._commonLookupService.getEditionsForCombobox(false).subscribe((editionsResult) => {
            this.editions = editionsResult.items;
            let notSelectedEdition = new SubscribableEditionComboboxItemDto();
            notSelectedEdition.displayText = this.l('NotAssigned');
            notSelectedEdition.value = '';
            this.editions.unshift(notSelectedEdition);

            this._tenantService.getTenantForEdit(tenantId).subscribe((tenantResult) => {
                this.tenant = tenantResult;
                this.currentConnectionString = tenantResult.connectionString;
                this.tenant.editionId = this.tenant.editionId || 0;
                this.isUnlimited = !this.tenant.subscriptionEndDateUtc;
                this.subscriptionEndDateUtcIsValid =
                    this.isUnlimited || this.tenant.subscriptionEndDateUtc !== undefined;
                //this.modal.show();
                this.toggleSubscriptionFields();

            });
        });

    //     this._tenantbasicdetailsServiceProxy.getTenantById(this.tenantid).subscribe((data) => {
    //         console.log(data)
    //         this.tenants.vatid = data[0].vatid?.trim();
    //         this.tenants.businessCategory = data[0].businessCategory?.trim();      
    //          this.tenants.contactPerson = data[0].contactPerson?.trim();
    //          this.tenants.contactNumber = data[0].contactNumber?.trim();
    //          this.tenants.constitutionType =data[0].constitutionType?.trim();
    //          this.tenants.tenantType=data[0].tenantType?.trim();
    //          this.tenants.turnoverSlab=data[0].turnoverslab?.trim();
    //          this.tenants.designation=data[0].designation?.trim();
    //          this.tenants.emailID=data[0].emailID?.trim();
    //          this.tenants.lastReturnFiled=data[0].lastReturnFiled?.trim();
    //          this.tenants.legalRepresentative=data[0].legalrepresentative?.trim();
    //          this.tenants.nationality=data[0].nationality?.trim();
    //          this.tenants.operationalModel=data[0].operationalModel?.trim();
    //          this.tenants.parentEntityCountryCode=data[0].parententitycountrycode?.trim();
    //          this.tenants.parentEntityName=data[0].parententityname?.trim();
    //          this.tenants.vatReturnFillingFrequency=data[0].vatReturnFillingFrequency?.trim();
    //         this.address.buildingNo = data[0].buildingNo;
    //         this.address.additionalBuildingNumber = data[0].additionalBuildingNumber;
    //         this.address.street = data[0].street?.trim();
    //         this.address.additionalStreet = data[0].additionalStreet?.trim();
    //         this.address.city = data[0].city?.trim();
    //         this.address.state = data[0].state;
    //         this.address.country = data[0].country;

    //         this.address.postalCode = data[0].postalCode;
    //         this.address.neighbourhood = data[0].neighbourhood?.trim();  
    //         this.documents.documentNumber=data[0].documentNumber;
    //     this.tenants.address=this.address;
    // this.tenants.documents=this.documents;     });
    }

    onShown(): void {
        document.getElementById('Name').focus();
    }

    subscriptionEndDateChange(e): void {
        this.subscriptionEndDateUtcIsValid = (e && e.date !== false) || this.isUnlimited;
    }

    selectedEditionIsFree(): boolean {
        if (!this.tenant.editionId) {
            return true;
        }

        let selectedEditions = _filter(this.editions, { value: this.tenant.editionId + '' });
        if (selectedEditions.length !== 1) {
            return true;
        }

        let selectedEdition = selectedEditions[0];
        return selectedEdition.isFree;
    }

    save(): void {
        this.saving = true;
        if (this.tenant.editionId === 0) {
            this.tenant.editionId = null;
        }

        if (this.isUnlimited) {
            this.tenant.isInTrialPeriod = false;
        }

        // take selected date as UTC
        if (this.isUnlimited || !this.tenant.editionId) {
            this.tenant.subscriptionEndDateUtc = null;
        } else {
            this.tenant.subscriptionEndDateUtc = this.tenant.subscriptionEndDateUtc.toLocal();
        }

        this._tenantService
            .updateTenant(this.tenant)
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
               // this.close();
                this.modalSave.emit(null);
            });
    }

    updatedetails(tenants){
        this.tenants.id=this.tenantid;
        this.tenants.address=this.address;
        // this.tenants.documents=this.documents;
        this._tenantbasicdetailsServiceProxy.upadateTenant(tenants)
        .pipe(finalize(()=>(
            this.saving = false

        ))) 
        .subscribe(() => {
            this.notify.success(this.l('UpdatedSuccessfully'));
            this._location.back();
        });
    }
    close(): void {
        //this.active = false;
        //this.modal.hide();
        this._location.back();

    }

    onEditionChange(): void {
        if (this.selectedEditionIsFree()) {
            this.tenant.isInTrialPeriod = false;
        }

        this.toggleSubscriptionFields();
    }

    onUnlimitedChange(): void {
        if (this.isUnlimited) {
            this.tenant.subscriptionEndDateUtc = null;
            this.subscriptionEndDateUtcIsValid = true;
            this.tenant.isInTrialPeriod = false;
        } else {
            if (!this.tenant.subscriptionEndDateUtc) {
                this.subscriptionEndDateUtcIsValid = false;
            }
        }
    }

    toggleSubscriptionFields() {
        if (this.tenant.editionId > 0) {
            this.isSubscriptionFieldsVisible = true;
        } else {
            this.isSubscriptionFieldsVisible = false;
        }
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

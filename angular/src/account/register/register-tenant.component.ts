import { AfterViewInit, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

import { Location } from '@angular/common';

import {
    EditionSelectDto,
    PasswordComplexitySetting,
    ProfileServiceProxy,
    RegisterTenantOutput,
    TenantRegistrationServiceProxy,
    PaymentPeriodType,
    SubscriptionPaymentGatewayType,
    SubscriptionStartType,
    EditionPaymentType,
    CountryServiceProxy,
    GetCountryForViewDto,
    GetConstitutionForViewDto,
    ConstitutionServiceProxy,
    TenantBasicDetailsServiceProxy,
    CreateOrEditTenantBasicDetailsDto,
    CreateOrEditTenantAddressDto,
    TenantServiceProxy,
    CreateTenantInput,
    SubscribableEditionComboboxItemDto,
    CommonLookupServiceProxy,
    CreateOrEditTenantDocumentsDto
    
} from '@shared/service-proxies/service-proxies';
import { RegisterTenantModel } from './register-tenant.model';
import { TenantRegistrationHelperService } from './tenant-registration-helper.service';
import { finalize, catchError } from 'rxjs/operators';
import { ReCaptchaV3Service } from 'ngx-captcha';
import { FormBuilder, FormGroup ,Validators} from '@angular/forms';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { firstValueFrom } from 'rxjs';

@Component({
    selector:'Register-Tenant',
    templateUrl: './register-tenant.component.html',
    animations: [accountModuleAnimation()],
})
export class RegisterTenantComponent extends AppComponentBase implements OnInit, AfterViewInit {
    privacyEnabled=false;
    Signing=false;
    model: RegisterTenantModel = new RegisterTenantModel();
    passwordComplexitySetting: PasswordComplexitySetting = new PasswordComplexitySetting();
    subscriptionStartType = SubscriptionStartType;
    editionPaymentType: EditionPaymentType;
    paymentPeriodType = PaymentPeriodType;
    selectedPaymentPeriodType: PaymentPeriodType = PaymentPeriodType.Monthly;
    subscriptionPaymentGateway = SubscriptionPaymentGatewayType;
    paymentId = '';
    recaptchaSiteKey: string = AppConsts.recaptchaSiteKey;
    basicForm: FormGroup;
    countries: GetCountryForViewDto[] = [];
    loadindividualdt:boolean=false;
    loadindividualadd:boolean=false;
    loadIndividualReg:boolean=false;
    tenants:CreateOrEditTenantBasicDetailsDto=new CreateOrEditTenantBasicDetailsDto();
    address:CreateOrEditTenantAddressDto=new CreateOrEditTenantAddressDto();
    documents:CreateOrEditTenantDocumentsDto=new CreateOrEditTenantDocumentsDto();
    constitutionTypes: GetConstitutionForViewDto[] = [];
    setRandomPassword = true;
    tenant: CreateTenantInput;
    active = false;
    saving = false;
    useHostDb = true;
    editions: SubscribableEditionComboboxItemDto[] = [];
    isUnlimited = false;
    isSubscriptionFieldsVisible = false;
    isSelectedEditionFree = false;
    tenantAdminPasswordRepeat = '';
    fromOnboardingPage=false;
    profilePicture = AppConsts.appBaseUrl + '/assets/common/images/logo-vita.jpg';
    profilelogo = AppConsts.appBaseUrl + '/assets/common/images/agency.png';
    @ViewChild('modal' , { static: false }) modal: ModalDirective;




    tenantForm() {
        this.basicForm = this.fb.group({
              Buildno: ['', [Validators.required,
                    Validators.maxLength(4),
                  Validators.pattern("^[0-9]*$") ]],
                  AdditionalBno: ['',[
                  Validators.pattern("^[0-9]*$") ]  ],
                  street: ['', Validators.required ],

                  companycode: ['', []],
                crnumber: ['', [Validators.pattern("^[0-9]*$"),Validators.maxLength(10),Validators.minLength(10)] ],
                  companyname: ['', [Validators.required,Validators.maxLength(68)] ],
                  adminusername: ['', [Validators.required,Validators.maxLength(64),Validators.pattern("[a-zA-Z][a-zA-Z ]+")] ],
                  adminsurname: ['', [Validators.required,Validators.maxLength(64),Validators.pattern("[a-zA-Z][a-zA-Z ]+")] ],
                  adminemail:['', [Validators.required,Validators.maxLength(256)] ],
                  Additnalstreet: ['', []],
                  pin: ['', [Validators.required,
                    Validators.maxLength(5),
                Validators.pattern("^[0-9]*$") ] ],
                nationality: ['', Validators.required ],
                  city: ['', Validators.required ],
                  state: ['', [Validators.required,Validators.pattern("[a-zA-Z][a-zA-Z ]+")] ],
                  ContactNo: ['', [Validators.required,
                Validators.maxLength(12),
            Validators.minLength(12)] ],
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
        private _router: Router,
        private _tenantRegistrationService: TenantRegistrationServiceProxy,
        private _profileService: ProfileServiceProxy,
        private _tenantRegistrationHelper: TenantRegistrationHelperService,
        private _activatedRoute: ActivatedRoute,
        private _tenantService: TenantServiceProxy,
        private _reCaptchaV3Service: ReCaptchaV3Service,
        private _masterCountriesServiceProxy:CountryServiceProxy,
        private _masterConstitutionServiceProxy: ConstitutionServiceProxy,
        private _tenantbasicdetailsServiceProxy : TenantBasicDetailsServiceProxy,
        private _commonLookupService: CommonLookupServiceProxy,
        private _dateTimeService: DateTimeService,
        private _location:Location


    ) {
        super(injector);
        this.tenantForm();
    }

    get useCaptcha(): boolean {
        return this.setting.getBoolean('App.TenantManagement.UseCaptchaOnRegistration');
    }


    ngOnInit() {
        window.scroll(0,0);
        this.model.editionId = this._activatedRoute.snapshot.queryParams['editionId'];
        this.editionPaymentType = this._activatedRoute.snapshot.queryParams['editionPaymentType'];
        this.fromOnboardingPage = this._activatedRoute.snapshot.queryParams['fromOnboardingPage'];

        if (this.model.editionId) {
            this.model.subscriptionStartType = this._activatedRoute.snapshot.queryParams['subscriptionStartType'];
        }

        //Prevent to create tenant in a tenant context
        if (this.appSession.tenant != null) {
            this._router.navigate(['account/login']);
            return;
        }

        this._profileService.getPasswordComplexitySetting().subscribe((result) => {
           
            this.passwordComplexitySetting = result.setting;
            
        });

        this.loadindividualdt=true;
        this.tenants.nationality=null;
        this.address.country="SA";
        this.tenants.constitutionType=null;
        this.tenants.tenantType=null;
        this.tenants.vatReturnFillingFrequency=null;
        //this.getCountriesDropdown();


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
            console.log(result.items);
          });
      }

    ngAfterViewInit() {
        if (this.model.editionId) {
            this._tenantRegistrationService.getEdition(this.model.editionId).subscribe((result: EditionSelectDto) => {
                this.model.edition = result;
            });
        }
    }

    
    // mapFormtoDto() {
    //     this.tenants.tenantType = this.basicForm.get('TenantType').value || " ";
    //     this.tenants.businessCategory =this.basicForm.get('businessCategory').value || " ";
    //     this.tenants.constitutionType = this.basicForm.get('ConstitutionType').value || " ";
    //     this.tenants.contactNumber = //this.individualRequiredForm.get('firstName').value || " ";
    //     this.tenants.contactPerson = this.basicForm.get('ContactNo').value || " ";
    //     this.tenants.operationalModel = this.basicForm.get('OperationalModel').value || " ";
    //     this.tenants.turnoverSlab = this.basicForm.get('turnoverSlab').value || " ";
    //     this.tenants.emailID = this.basicForm.get('emailID').value || " ";
    //     this.tenants.nationality = this.basicForm.get('nationality').value || " ";
    //     this.tenants.designation = this.basicForm.get('designation').value || " ";
    //     this.tenants.vatid = this.basicForm.get('vatid').value || " ";
    //     this.tenants.parentEntityName = this.basicForm.get('parentEntityName').value || " ";
    //     this.tenants.legalRepresentative = this.basicForm.get('legalRepresentative').value || " ";
    //     this.tenants.parentEntityCountryCode = this.basicForm.get('parentEntityCountryCode').value || " ";
    //     this.tenants.lastReturnFiled = this.basicForm.get('lastReturnFiled').value || " ";
    //     this.tenants.vatReturnFillingFrequency = this.basicForm.get('vatReturnFillingFrequency').value || " ";

    //     this.address.additionalBuildingNumber = this.basicForm.get('AdditionalBno').value || " ";
    //     this.address.additionalStreet = this.basicForm.get('Additnalstreet').value || " ";
    //     this.address.street = this.basicForm.get('street').value || " ";
    //     this.address.postalCode = this.basicForm.get('pin').value || " ";
    //     this.address.state = this.basicForm.get('state').value || " ";
    //     this.address.buildingNo = this.basicForm.get('Buildno').value || " ";
    //     this.address.city = this.basicForm.get('city').value || " ";
    //     this.address.countryCode=this.basicForm.get('nationality').value || " ";
  
    //     }


    async isEmailRegistered(){
        //return false 
       var res = await firstValueFrom(this._tenantRegistrationService.checkIfEmailExists(this.model.adminEmailAddress,true))
        return res
    }
    async isvatRegistered(){
        //return false 
        if((this.tenants.vatid).charAt(10)!='1')
        {
       var res = await firstValueFrom(this._tenantRegistrationService.checkIfVatExists(this.tenants.vatid,true))
       return res;
        }
        else
        {
            return res;
        }
    }
    async save(): Promise<void> {
      
if(this.isFormValid())
{
    
    this.model.tenancyName="dummy_1"
    if((this.tenants.vatid==null || this.tenants.vatid==undefined )&&(
        this.documents.documentNumber==null || this.documents.documentNumber==undefined))
        {
        this.notify.error(this.l('Please fill either CR number or VAT number to save.'));
        }
    else if(this.model.adminPassword==null || this.model.adminPassword==undefined)
    {
        this.notify.error(this.l('Password is Required'));
    }
    else if(this.model.passwordRepeat==null || this.model.passwordRepeat==undefined)
        {
            console.log(this.model.passwordRepeat)
            this.notify.error(this.l('Confirm Password is Required '));

        }
    else if(this.model.passwordRepeat!=this.model.adminPassword)
        {
            this.notify.error(this.l('Passwords do not match '));
        }
        else if(await this.isEmailRegistered()){
            this.notify.error(this.l('Entered Email Address is already registered'));
          return null;
        }
        else if(await this.isvatRegistered()){
            this.notify.error(this.l('Entered VAT Number  already exists'));
          return null;
        }
    
    else
    {
        let recaptchaCallback = (token: string) => {
            this.saving = true;
            this.Signing=true;
            this.model.captchaResponse = token;
            this._tenantRegistrationService
                .registerTenant(this.model)
                .pipe(
                    finalize(() => {
                        (this.saving = false),
                        (this.Signing=false)
                    })
                )
                .subscribe((result: RegisterTenantOutput) => {
                    this.notify.success(this.l('SuccessfullyRegistered'));
                    
                    this._tenantRegistrationHelper.registrationResult = result;
                    if (parseInt(this.model.subscriptionStartType.toString()) === SubscriptionStartType.Paid) {
                        this._router.navigate(['account/buy'], {
                            queryParams: {
                                tenantId: result.tenantId,
                                editionId: this.model.editionId,
                                subscriptionStartType: this.model.subscriptionStartType,
                                editionPaymentType: this.editionPaymentType,
                            },

                        });
                    } else {
                        //this._router.navigate(['account/register-tenant-result']);
                        this.savenew();
                    }
                });
        };
    
        

        if (this.useCaptcha) {
            this._reCaptchaV3Service.execute(this.recaptchaSiteKey, 'register_tenant', (token) => {
                recaptchaCallback(token);
            });
        } else {
            recaptchaCallback(null);
        }
    }


    }
    else
    {

        this.notify.error(this.l('Please fill all required field'));

    }
}
    show() {
        this.active = true;
        this.init();

        this._profileService.getPasswordComplexitySetting().subscribe((result) => {
            this.passwordComplexitySetting = result.setting;
           
        });
    }

    onShown(): void {
        document.getElementById('TenancyName').focus();
    }

    init(): void {
        this.tenant = new CreateTenantInput();
        this.tenant.isActive = true;
        this.tenant.shouldChangePasswordOnNextLogin = true;
        this.tenant.sendActivationEmail = true;
        this.tenant.editionId = 0;
        this.tenant.isInTrialPeriod = false;

        this._commonLookupService.getEditionsForCombobox(false).subscribe((result) => {
            this.editions = result.items;

            let notAssignedItem = new SubscribableEditionComboboxItemDto();
            notAssignedItem.value = '';
            notAssignedItem.displayText = this.l('NotAssigned');

            this.editions.unshift(notAssignedItem);

            this._commonLookupService.getDefaultEditionName().subscribe((getDefaultEditionResult) => {
                let defaultEdition = _filter(this.editions, { displayText: getDefaultEditionResult.name });
                if (defaultEdition && defaultEdition[0]) {
                    this.tenant.editionId = parseInt(defaultEdition[0].value);
                    this.toggleSubscriptionFields();
                }
            });
        });
    }

    getEditionValue(item): number {
        return parseInt(item.value);
    }

    selectedEditionIsFree(): boolean {
        let selectedEditions = _filter(this.editions, { value: this.tenant.editionId.toString() }).map((u) =>
            Object.assign(new SubscribableEditionComboboxItemDto(), u)
        );

        if (selectedEditions.length !== 1) {
            this.isSelectedEditionFree = true;
        }

        let selectedEdition = selectedEditions[0];
        this.isSelectedEditionFree = selectedEdition.isFree;
        return this.isSelectedEditionFree;
    }

    subscriptionEndDateIsValid(): boolean {
        if (this.tenant.editionId <= 0) {
            return true;
        }

        if (this.isUnlimited) {
            return true;
        }

        if (!this.tenant.subscriptionEndDateUtc) {
            return false;
        }

        return this.tenant.subscriptionEndDateUtc !== undefined;
    }

    savenew(): void {  
        // if(this.isFormValid()){

        // this.saving = true;
        // this.tenants.address=this.address;

        // if (this.setRandomPassword) {
        //     this.tenant.adminPassword = null;
        // }

        // if (this.tenant.editionId === 0) {
        //     this.tenant.editionId = null;
        // }

        // if (this.isUnlimited) {
        //     this.tenant.isInTrialPeriod = false;
        // }

        // if (this.isUnlimited || this.tenant.editionId <= 0) {
        //     this.tenant.subscriptionEndDateUtc = null;
        // } else {
        //     this.tenant.subscriptionEndDateUtc = this._dateTimeService.toUtcDate(this.tenant.subscriptionEndDateUtc);
        // }

        // this.tenants.documents=this.documents;        
        this.tenants.address=this.address;
        this.tenants.tenantType='Individual'
            this._tenantRegistrationService
            .createTenant(this.tenants)
            .pipe(finalize(() => (this.saving = false,
                (this.Signing=false))))
            .subscribe(() => {
              //  this.notify.info(this.l('SavedSuccessfully'));
               this.close();
            //this.modalSave.emit(null);
                //this._router.navigate(['/app/admin/tenants']);
               this._router.navigate(['account/register-tenant-result']);         
               });
    //     }
    //     else{

    //         this.notify.error(this.l('Please fill both Basic and Address Information to save.'));
    //         this.saving = false;
    //         return;
        
    //   }
            
    }
    close(): void {
        this.active = false;
        this.tenantAdminPasswordRepeat = '';
      // this.modal.hide();
    }

    onEditionChange(): void {
        this.tenant.isInTrialPeriod = this.tenant.editionId > 0 && !this.selectedEditionIsFree();
        this.toggleSubscriptionFields();
    }

    toggleSubscriptionFields() {
        this.isSelectedEditionFree = this.selectedEditionIsFree();
        if (this.tenant.editionId <= 0 || this.isSelectedEditionFree) {
            this.isSubscriptionFieldsVisible = false;

            if (this.isSelectedEditionFree) {
                this.isUnlimited = true;
            } else {
                this.isUnlimited = false;
            }
        } else {
            this.isSubscriptionFieldsVisible = true;
        }
    }

    onIsUnlimitedChange() {
        if (this.isUnlimited) {
            this.tenant.isInTrialPeriod = false;
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

back(){
    this._location.back();
}
isSelectEditionPage(): boolean {
    return this._router.url.indexOf('/account/select-edition') >= 0;
}
}

import { Component, EventEmitter, Injector, NgZone, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup ,ValidationErrors,Validators} from '@angular/forms';
import { Router } from '@angular/router';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    CommonLookupServiceProxy,
    CreateTenantInput,
    PasswordComplexitySetting,
    ProfileServiceProxy,
    TenantServiceProxy,
    SubscribableEditionComboboxItemDto,
    CountryServiceProxy,
    GetCountryForViewDto,
    GetConstitutionForViewDto,
    ConstitutionServiceProxy,
    TenantBasicDetailsServiceProxy,
    CreateOrEditTenantBasicDetailsDto,
    CreateOrEditTenantAddressDto,
    CreateOrEditTenantDocumentsDto,
    BusinessTurnoverSlabServiceProxy,
    GetBusinessTurnoverSlabForViewDto,
    BusinessOperationalModelServiceProxy,
    GetBusinessOperationalModelForViewDto,
    TenantRegistrationServiceProxy
} from '@shared/service-proxies/service-proxies';
import { filter as _filter } from 'lodash-es';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { Location } from '@angular/common';
import { AppConsts } from '@shared/AppConsts';
import { IFormattedUserNotification, UserNotificationHelper } from '@app/shared/layout/notifications/UserNotificationHelper';
import { NotificationServiceProxy } from '@shared/service-proxies/service-proxies';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { forEach as _forEach } from 'lodash-es';
import { UserNotification } from '@shared/service-proxies/service-proxies';
import { HttpClient } from '@angular/common/http';
import { FileUpload } from 'primeng/fileupload';
import { ImportBatchDatasServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { TenantSettingsModule } from '../settings/tenant-settings.module';
import { firstValueFrom } from 'rxjs';

@Component({
    selector: 'createTenantModal',
    templateUrl: './create-tenant-modal.component.html',
})
export class CreateTenantModalComponent extends AppComponentBase {
    @ViewChild('createModal', { static: true }) modal: ModalDirective;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    setRandomPassword = true;
    useHostDb = true;
    editions: SubscribableEditionComboboxItemDto[] = [];
    tenant: CreateTenantInput;
    passwordComplexitySetting: PasswordComplexitySetting = new PasswordComplexitySetting();
    isUnlimited = false;
    fileName: string;
    type:string;
    uploadUrl: string;
    isSubscriptionFieldsVisible = false;
    constitutionTypes: GetConstitutionForViewDto[] = [];
    basicForm: FormGroup;
    isSelectedEditionFree = false;
    tenantAdminPasswordRepeat = '';
    countries: GetCountryForViewDto[] = [];
    turnoverslabs:GetBusinessTurnoverSlabForViewDto[]=[];
    OperationalModels:GetBusinessOperationalModelForViewDto[]=[];
    loadindividualdt:boolean=false;
    loadindividualadd:boolean=false;
    loadIndividualReg:boolean=false;
    tenants:CreateOrEditTenantBasicDetailsDto=new CreateOrEditTenantBasicDetailsDto();
    address:CreateOrEditTenantAddressDto=new CreateOrEditTenantAddressDto();
    documents:CreateOrEditTenantDocumentsDto=new CreateOrEditTenantDocumentsDto();
    notifications: IFormattedUserNotification[] = [];
unreadNotificationCount = 0;
filedate:string;


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
                  ContactNo: ['', Validators.required ],
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
        private _location: Location,
        private _tenantService: TenantServiceProxy,
        private _commonLookupService: CommonLookupServiceProxy,
        private _profileService: ProfileServiceProxy,
        private _dateTimeService: DateTimeService,
        private _masterCountriesServiceProxy:CountryServiceProxy,
        private _masterConstitutionServiceProxy: ConstitutionServiceProxy,
        private _tenantbasicdetailsServiceProxy : TenantBasicDetailsServiceProxy,
        private _notificationService: NotificationServiceProxy,
        private _userNotificationHelper: UserNotificationHelper,
        private _tenantRegistrationService: TenantRegistrationServiceProxy,
        public _zone: NgZone,
        private _httpClient: HttpClient,
        private _fileUpload: ImportBatchDatasServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _BusinessTurnoverSlabServiceProxy :BusinessTurnoverSlabServiceProxy,
        private _BusinessOperationalModelServiceProxy :BusinessOperationalModelServiceProxy



    ) {
        super(injector);
        this.tenantForm();
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/TenantFileUpload/ImportFromExcel';


    }


    ngOnInit(): void {
        this.type="Create";
        this.show();
        this.getCountriesDropdown();
        this.getConstitutionType();
        this.loadNotifications();
        this.registerToEvents();
        this.getTurnoverslabDropdown();
        this.getoperationalModelDropdown();

        this.loadindividualdt=true;

    }

    loadNotifications(): void {
        if (UrlHelper.isInstallUrl(location.href)) {
            return;
        }
    
        this._notificationService.getUserNotifications(undefined, undefined, undefined, 3, 0).subscribe((result) => {
            this.unreadNotificationCount = result.unreadCount;
            this.notifications = [];
            _forEach(result.items, (item: UserNotification) => {
                this.notifications.push(this._userNotificationHelper.format(<any>item));
    
                //this is used for syncronizing the getUploadData method
               //if(this._userNotificationHelper.format(<any>item).text== "AllCreditNoteSuccessfullyImportedFromExcel" || this._userNotificationHelper.format(<any>item).text== "ClickToSeeInvalidCreditNote")
               if(this._userNotificationHelper.format(<any>item).text.startsWith("Tenant File Upload") )
               this.getUploadedData(this.fileName);
            });
            console.log(this.notifications);
        });
    }
    
    
    registerToEvents() {
        let self = this;
    
        function onNotificationReceived(userNotification) {
            self._userNotificationHelper.show(userNotification);
            self.loadNotifications();
        }
    
        this.subscribeToEvent('abp.notifications.received', (userNotification) => {
            self._zone.run(() => {
                onNotificationReceived(userNotification);
            });
        });
    
        function onNotificationsRefresh() {
            self.loadNotifications();
        }
    
        this.subscribeToEvent('app.notifications.refresh', () => {
            self._zone.run(() => {
                onNotificationsRefresh();
            });
        });
    
    }
    getDate(){
        var date= new Date();
        this.filedate= date.getFullYear().toString()+date.getMonth().toString()+date.getDay().toString()+date.getHours().toString()+date.getMinutes().toString()+date.getSeconds().toString()
      }

    uploadExcel(data: { files: File }): void {
        const formData: FormData = new FormData();
        this.getDate();
          const file = data.files[0];
          const newFileName = this.filedate+"_"+file.name;
          //const newFileName = 'StandardFileUpload'+"_"+this.randomString(5, '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ') + '.' + fileExtension;
          this.fileName = newFileName;
          const newFile = new File([file], newFileName, { type: file.type });
          formData.append('file', newFile);
    console.log("called")
        this._httpClient
            .post<any>(this.uploadUrl, formData)
            .pipe(finalize(() => this.excelFileUpload.clear()))
            .subscribe((response) => {
                if (response.success) {
                    this.notify.success(this.l('ImportTenantProcessStart'));
                } else if (response.error != null) {
                    this.notify.error(this.l('ImportTenantUploadFailed'));
                }
            });
    }
    
    getUploadedData(fileName:string): void {
     
    //   this._salesInvoicesAppService.getSalesBatchData(fileName).subscribe((result) => {
    //     console.log(result);
    //     if(result && result.length>0)
    //     this.invoices = result;
    //     this.batchid=result[0]?.batchId;
    //   });
      }
    
    onUploadExcelError(): void {
        this.notify.error(this.l('ImportCreditNoteUploadFailed'));
    }
    
    downloadInvalidExcel(): void {
    }
    
    exportToExcel(batchId:number): void {
      this._fileUpload
        .getInvalidRecordsToExcel(batchId,this.fileName)
        .subscribe((result) => {
          this._fileDownloadService.downloadTempFile(result);
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

      getTurnoverslabDropdown() {
        this._BusinessTurnoverSlabServiceProxy.getAll(undefined,"","",undefined,undefined,undefined,undefined,200)
          .subscribe(result => {
            this.turnoverslabs = result.items;
          });
      }

      getoperationalModelDropdown() {
        this._BusinessOperationalModelServiceProxy.getAll(undefined,"","",undefined,undefined,undefined,undefined,200)
          .subscribe(result => {
            this.OperationalModels = result.items;
          });
      }
    show() {
        this.active = true;
        this.init();

        this._profileService.getPasswordComplexitySetting().subscribe((result) => {
            this.passwordComplexitySetting = result.setting;
           this.modal.show();
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

    mapFormtoDto() {
        this.tenants.tenantType = this.basicForm.get('TenantType').value || " ";
        this.tenants.businessCategory =this.basicForm.get('businessCategory').value || " ";
        this.tenants.constitutionType = this.basicForm.get('ConstitutionType').value || " ";
        this.tenants.contactNumber = //this.individualRequiredForm.get('firstName').value || " ";
        this.tenants.contactPerson = this.basicForm.get('ContactNo').value || " ";
        this.tenants.operationalModel = this.basicForm.get('OperationalModel').value || " ";
        this.tenants.turnoverSlab = this.basicForm.get('turnoverSlab').value || " ";
        this.tenants.emailID = this.basicForm.get('emailID').value || " ";
        this.tenants.nationality = this.basicForm.get('nationality').value || " ";
        this.tenants.designation = this.basicForm.get('designation').value || " ";
        this.tenants.vatid = this.basicForm.get('vatid').value || " ";
        this.tenants.parentEntityName = this.basicForm.get('parentEntityName').value || " ";
        this.tenants.legalRepresentative = this.basicForm.get('legalRepresentative').value || " ";
        this.tenants.parentEntityCountryCode = this.basicForm.get('parentEntityCountryCode').value || " ";
        this.tenants.lastReturnFiled = this.basicForm.get('lastReturnFiled').value || " ";
        this.tenants.vatReturnFillingFrequency = this.basicForm.get('vatReturnFillingFrequency').value || " ";
        this.documents.documentType = 'CRN'
        this.documents.documentNumber = this.basicForm.get('crnumber').value || " ";

        this.address.additionalBuildingNumber = this.basicForm.get('AdditionalBno').value || " ";
        this.address.additionalStreet = this.basicForm.get('Additnalstreet').value || " ";
        this.address.street = this.basicForm.get('street').value || " ";
        this.address.postalCode = this.basicForm.get('pin').value || " ";
        this.address.state = this.basicForm.get('state').value || " ";
        this.address.buildingNo = this.basicForm.get('Buildno').value || " ";
        this.address.city = this.basicForm.get('city').value || " ";
        this.address.countryCode=this.basicForm.get('nationality').value || " ";
  
        }

    async save(tenants): Promise<void> {
        // if(this.isFormValid()){

        this.saving = true;
        this.tenants.address=this.address;
        // this.tenants.documents=this.documents;

        if (this.setRandomPassword) {
            this.tenant.adminPassword = null;
        }

        if (this.tenant.editionId === 0) {
            this.tenant.editionId = null;
        }

        if (this.isUnlimited) {
            this.tenant.isInTrialPeriod = false;
        }

        if (this.isUnlimited || this.tenant.editionId <= 0) {
            this.tenant.subscriptionEndDateUtc = null;
        } else {
            this.tenant.subscriptionEndDateUtc = this._dateTimeService.toUtcDate(this.tenant.subscriptionEndDateUtc);
        }
        if(await this.isEmailRegistered()){
            this.notify.error(this.l('Entered Email Address is already registered'));
            return null;
        }
        this._tenantService
            .createTenant(this.tenant)
            .pipe(finalize(() => (
                this. savedetails(tenants)
                )))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
               // this.close();
               // this.modalSave.emit(null);
            });

            
    //     }
    //     else{

    //         this.notify.error(this.l('Please fill both Basic and Address Information to save.'));
    //         this.saving = false;
    //         return;
        
    //   }
    }


    savedetails(tenants){
        this._tenantbasicdetailsServiceProxy
            .createTenant(tenants)
            .pipe(finalize(() => (this.saving = false)))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
               // this.close();
               // this.modalSave.emit(null);
                //this._router.navigate(['/app/admin/tenants']);
                this._location.back();
            });
    }
    close(): void {
        this.active = false;
        this.tenantAdminPasswordRepeat = '';
        //this.modal.hide();
        this._location.back();

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

       async isEmailRegistered(){
        //return false 
       var res = await firstValueFrom(this._tenantRegistrationService.checkIfEmailExists(this.tenant.adminEmailAddress,true))
        return res
    }
}

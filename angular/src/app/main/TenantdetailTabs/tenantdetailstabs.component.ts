import { AppConsts } from '@shared/AppConsts';
import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild,
    Input,
    Output,
    EventEmitter,
    ElementRef,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
//import { DetailsServiceProxy, GetDetailForViewDto } from '@shared/service-proxies/service-proxies'
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { Table } from 'primeng/table';
import {
    BusinessOperationalModelServiceProxy,
    BusinessTurnoverSlabServiceProxy,
    ConstitutionServiceProxy,
    CountryServiceProxy,
    CreateOrEditTenantAddressDto,
    CreateOrEditTenantBasicDetailsDto,
    CreateOrEditTenantDocumentsDto,
    CustomersesServiceProxy,
    DocumentMasterServiceProxy,
    GetBusinessOperationalModelForViewDto,
    GetBusinessTurnoverSlabForViewDto,
    GetConstitutionForViewDto,
    GetCountryForViewDto,
    GetCustomersForViewDto,
    GetDocumentMasterForViewDto,
    GetInvoiceTypeForViewDto,
    GetTaxCategoryForViewDto,
    GetTransactionCategoryForViewDto,
    InvoiceTypeServiceProxy,
    SectorServiceProxy,
    TaxCategoryServiceProxy,
    TenantBasicDetailsServiceProxy,
    TenantRegistrationServiceProxy,
    TenantSettingsServiceProxy,
    TransactionCategoryServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { Location } from '@angular/common';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { firstValueFrom } from 'rxjs';
import { GlobalConstsCustomService } from '@shared/customService/global-consts-service';

@Component({
    selector: 'Tenantdetailtabs',
    templateUrl: './tenantdetailstabs.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class TenantDetailsTabsComponent extends AppComponentBase {
    vitamenu = AppConsts.vitaMenu;
    @Output() state = new EventEmitter<any>();
    @Input() tenantid: number;
    @Input() type: string;
    @Input() tenant: [];
    @Input() Ctenant: [];
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('uploadLogoInputLabel') uploadLogoInputLabel: ElementRef;

    customers: any[] = [];  
    vatid: string;
    active = false;
    saving = false;
    filterText = '';
    searchQuery = '';
    advancedFiltersAreShown = false;
    //tenantid:number;
    nameFilter = '';
    descriptionFilter = '';
    codeFilter = '';
    isActiveFilter = -1;
    basicForm: FormGroup;
    constitutionTypes: GetConstitutionForViewDto[] = [];
    countries: GetCountryForViewDto[] = [];
    loadindividualdt = false;
    loadindividualadd = false;
    loadIndividualReg = false;
    loadIndividualpat = false;
    tenants: CreateOrEditTenantBasicDetailsDto = new CreateOrEditTenantBasicDetailsDto();
    address: CreateOrEditTenantAddressDto = new CreateOrEditTenantAddressDto();
    documents: CreateOrEditTenantDocumentsDto = new CreateOrEditTenantDocumentsDto();
    turnoverslabs: GetBusinessTurnoverSlabForViewDto[] = [];
    unicoredocs:CreateOrEditTenantDocumentsDto[]=[];
    OperationalModels: GetBusinessOperationalModelForViewDto[] = [];
    remoteServiceBaseUrl = AppConsts.remoteServiceBaseUrl;
    logoUploader: FileUploader;
    customCssUploader: FileUploader;
    doctype: GetDocumentMasterForViewDto[] = [];
    transtype: GetTransactionCategoryForViewDto[] = [];
    taxcat: GetTaxCategoryForViewDto[] = [];
    invoicetype: GetInvoiceTypeForViewDto[] = [];
    purchasevat: any[];
    salesvat: any[];
    businesspurchase: any[];
    businesssales: any[];
    profileType: string;
    constitutiontype: string;

    constructor(
        injector: Injector,
        private _CustomerDetailsServiceProxy: CustomersesServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _location: Location,
        private _masterCountriesServiceProxy: CountryServiceProxy,
        private _masterConstitutionServiceProxy: ConstitutionServiceProxy,
        private _tenantbasicdetailsServiceProxy: TenantBasicDetailsServiceProxy,
        private _sessionService: AppSessionService,
        private _BusinessTurnoverSlabServiceProxy: BusinessTurnoverSlabServiceProxy,
        private _BusinessOperationalModelServiceProxy: BusinessOperationalModelServiceProxy,
        private _tenantSettingsService: TenantSettingsServiceProxy,
        private _masterDocumentsServiceProxy: DocumentMasterServiceProxy,
        private _masterInvoiceTypeServiceProxy: InvoiceTypeServiceProxy,
        private _masterSectorServiceProxy: SectorServiceProxy,
        private _masterBusinessCategoryServiceProxy: TransactionCategoryServiceProxy,
        private _masterTaxCategoryServiceProxy: TaxCategoryServiceProxy,
        private _tenantRegistrationService: TenantRegistrationServiceProxy,
        private _GlobalConstsCustomService: GlobalConstsCustomService,

        private fb: FormBuilder,
        private _tokenService: TokenService
    ) {
        super(injector);
        if(this.vitamenu == true)
        {
        this.tenantForm();
        }
        else
        {
            this.tenantunicoreForm();
        }
        this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
    }

    // eslint-disable-next-line @angular-eslint/use-lifecycle-interface
    ngAfterViewInit(): void {
        this.primengTableHelper.adjustScroll(this.dataTable);
    }


    ngOnInit() {

        if(this._sessionService.tenantId != undefined || this._sessionService.tenantId != null)
        {
        this.initUploaders();
        }
        this.loadindividualdt = true;
        //this.tenantid =  this._sessionService.tenantId
        this.tenantid=this._sessionService.tenantId;
        this._GlobalConstsCustomService.data$.subscribe(e=>{
            this.vitamenu=e.isVita;
        })
        if(this.vitamenu == true)
        {
            if(this.type=='Update')
            {
            this.show(this.tenantid);
            }
        }
        else
        {
            if (this.type === 'Update') {
                this.show(this.tenantid);
            }
            else
            {
                this.profileType='Individual'
            }
        }
        this.getCountriesDropdown();
        this.getConstitutionType();
        this.getTurnoverslabDropdown();
        this.getbusinessCategoryDropdown();
        this.getoperationalModelDropdown();
        this.getsalesvatdropdown();
        this.getbusinessPurchasedropdown();
        this.getbusinesssuppliesdropdown();
        this.getinvoicetypeDropdown();
        this.getpurchasevatdropdown();

    }


    tenantForm() {
        this.basicForm = this.fb.group({
            //basic Form
            businessCategory: ['', []],
            ConstitutionType: ['', []],
            TenantType: ['', []],
            OperationalModel: ['', []],
            TurnoverSlab: ['', []],
            EmailID: ['', []],
            Designation: ['', []],
            VATID: ['', []],
            ParentEntityName: ['', []],
            LegalRepresentative: ['', []],
            ParentEntityCountryCode: ['', []],
            LastReturnFiled: ['', []],
            VATReturnFillingFrequency: ['', []],
            ContactPerson: ['', []],
            crnumber: ['', [Validators.pattern('^[0-9]*$'), Validators.minLength(10), Validators.maxLength(10)]],

            Buildno: ['', [Validators.required, Validators.maxLength(4), Validators.pattern('^[0-9]*$')]],
            AdditionalBno: ['', [Validators.pattern('^[0-9]*$')]],
            street: ['', Validators.required],
            Additnalstreet: ['', []],
            pin: ['', [Validators.required, Validators.maxLength(5), Validators.pattern('^[0-9]*$')]],
            city: ['', Validators.required],
            state: ['', [Validators.pattern('[a-zA-Z][a-zA-Z ]+'), Validators.required]],
            ContactNo: [
                '',
                [
                    Validators.required,
                    Validators.minLength(12),
                    Validators.maxLength(12),
                    Validators.pattern('^[0-9]*$'),
                ],
            ],
            nationality: ['', Validators.required],
            vatid: ['', [Validators.maxLength(15), Validators.minLength(15), Validators.pattern('^3[0-9]*3$')]],
        });
    }
    //check if form is valid
    isFormValid() {
        return this.basicForm.valid;
    }
    // eslint-disable-next-line @angular-eslint/use-lifecycle-interface


    tenantunicoreForm() {
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
          crnumber: ['', [Validators.pattern("^[0-9]*$"),
        Validators.minLength(10),Validators.maxLength(10)] ],
                  Buildno: ['', [Validators.required,
                    Validators.maxLength(4),
                  Validators.pattern("^[0-9]*$") ]],
                  AdditionalBno: ['',[
                  Validators.pattern("^[0-9]*$")]   ],              
                  street: ['', Validators.required ],
                  Additnalstreet: ['', [] ],
                  pin: ['', [Validators.required,
                    Validators.maxLength(5),
                Validators.pattern("^[0-9]*$") ] ],
                  city: ['', Validators.required ],
                  state: ['',[Validators.pattern("[a-zA-Z][a-zA-Z ]+") ,Validators.required] ],
                  ContactNo: ['', [Validators.required,
                    Validators.minLength(12),
                    Validators.maxLength(12),
                    Validators.pattern("^[0-9]*$")]],
                  nationality: ['', Validators.required ],
                  vatid:['', [
                    Validators.maxLength(15),
                    Validators.minLength(15),
                  Validators.pattern("^3[0-9]*3$")]]
                });
      }
    
       //check if form is valid
        isunicoreFormValid() {
            return(this.basicForm.valid);
        }
    getoperationalModelDropdown() {
        this._BusinessOperationalModelServiceProxy
            .getAll(undefined, '', '', undefined, undefined, undefined, undefined, 200)
            .subscribe((result) => {
                this.OperationalModels = result.items;
            });
    }
    getTurnoverslabDropdown() {
        this._BusinessTurnoverSlabServiceProxy
            .getAll(undefined, '', '', undefined, undefined, undefined, undefined, 200)
            .subscribe((result) => {
                this.turnoverslabs = result.items;
            });
    }
    // getdoctypeDropdown() {
    //  this._masterDocumentsServiceProxy.getAll("","","","",undefined,undefined,undefined,undefined,200)
    //    .subscribe(result => {
    //      this.doctype = result.items;
    //    });
    // }
    getbusinessCategoryDropdown() {
        this._masterBusinessCategoryServiceProxy
            .getAll('', '', '', '', undefined, undefined, undefined, 200)
            .subscribe((result) => {
                this.transtype = result.items;
            });
    }
    getpurchasevatdropdown() {
        this._tenantbasicdetailsServiceProxy.getpurchasevatdropdown().subscribe((result) => {
            this.purchasevat = result;
        });
    }
    getsalesvatdropdown() {
        this._tenantbasicdetailsServiceProxy.getsalesvatdropdown().subscribe((result) => {
            this.salesvat = result;
        });
    }
    getbusinessPurchasedropdown() {
        this._tenantbasicdetailsServiceProxy.getbusinessPurchasedropdown().subscribe((result) => {
            this.businesspurchase = result;
            console.log(this.businesspurchase, 'dwd');
        });
    }
    getbusinesssuppliesdropdown() {
        this._tenantbasicdetailsServiceProxy.getbusinesssuppliesdropdown().subscribe((result) => {
            this.businesssales = result;
        });
    }

    getopertaionalDropdown() {
        this._BusinessOperationalModelServiceProxy
            .getAll('', '', '', '', undefined, undefined, undefined, 200)
            .subscribe((result) => {
                this.OperationalModels = result.items;
            });
    }
    gettaxcategoryDropdown() {
        this._masterTaxCategoryServiceProxy
            .getAll('', '', '', '', undefined, undefined, undefined, undefined, undefined, 200)
            .subscribe((result) => {
                this.taxcat = result.items;
            });
    }
    getinvoicetypeDropdown() {
        this._masterInvoiceTypeServiceProxy
            .getAll('', '', '', '', undefined, undefined, undefined, 200)
            .subscribe((result) => {
                this.invoicetype = result.items;
            });
    }
    show(tenantId: number): void {
        
        this.active = true;
        this._tenantbasicdetailsServiceProxy.getTenantById(tenantId).subscribe((data) => {
            this.tenants.vatid = data[0]?.vatid;
            this.tenants.website=data[0]?.website;
            this.tenants.faxNo=data[0]?.faxNo;
            this.tenants.constitutionType=data[0]?.constitutionType;
            this.tenants.tenantType=data[0]?.tenantType;
            this.tenants.emailID=data[0].emailID?.trim();
            this.tenants.nationality = data[0].nationality?.trim();
            this.tenants.contactNumber = data[0].contactNumber?.trim();
            this.address.buildingNo = data[0]?.buildingNo;
            this.address.additionalBuildingNumber = data[0]?.additionalBuildingNumber;
            this.address.street = data[0]?.street?.trim();
            this.address.additionalStreet = data[0]?.additionalStreet?.trim();
            this.address.city = data[0]?.city?.trim();
            this.address.state = data[0]?.state;
            this.address.country = data[0]?.country;
            this.address.postalCode = data[0]?.postalCode;
            this.address.neighbourhood = data[0]?.neighbourhood?.trim();
            this.documents.documentNumber = data[0].documentNumber;
            this.tenants.address = this.address;
            this.profileType=this.tenants.tenantType;
            this.constitutiontype=this.tenants.constitutionType;
            this.vatid=data[0]?.vatid;

        });


    }
    async updatedetails(tenants) {
        this.tenants = tenants;
            if (
                (this.tenants.vatid == null || this.tenants.vatid === undefined || !this.tenants.vatid) &&
                (this.documents.documentNumber === null ||
                    this.documents.documentNumber === undefined ||
                    !this.documents.documentNumber)
            ) {
                this.notify.error(this.l('Please fill either CR number or VAT ID to save.'));
                return null;

            } else {
                if (this.type === 'Update') {
                    this.tenants.id = this.tenantid;
                    this.tenants.address = this.address;
                    this._tenantbasicdetailsServiceProxy
                        .upadateTenant(this.tenants)
                        .pipe(finalize(() => (this.saving = false)))
                        .subscribe(() => {
                            this.notify.success(this.l('UpdatedSuccessfully'));
                            this.state.emit();
                        });
                } else {
                    this.savedetails();
                }
            }
    }
    savedetails() {
            this.state.emit(this.tenants);
            // this._tenantbasicdetailsServiceProxy
            //     .createTenant(this.tenants)
            //     .pipe(finalize(() => (this.saving = false)))
            //     .subscribe(() => {
            //         this.notify.info(this.l('SavedSuccessfully'));

            //        // this.close();
            //        // this.modalSave.emit(null);
            //         //this._router.navigate(['/app/admin/tenants']);
            //         this._location.back();
            //     });
    }
    getConstitutionType() {
        this._masterConstitutionServiceProxy
            .getAll('', '', '', undefined, undefined, undefined, undefined, undefined)
            .subscribe((result) => {
                this.constitutionTypes = result.items;
            });
    }
    getCountriesDropdown() {
        this._masterCountriesServiceProxy
            .getAll('', '', '', '', '', undefined, undefined, '', '', undefined, undefined, undefined, undefined, 200)
            .subscribe((result) => {
                this.countries = result.items;
            });
    }
    close(): void {
        //this.active = false;
        //this.modal.hide();
        this._location.back();
    }
    search() {}
    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
    }
    loadIndividual() {
        this.loadindividualdt = true;
        this.loadindividualadd = false;
        this.loadIndividualReg = false;
        this.loadIndividualpat = false;

    }
    loadAdd() {
        this.loadindividualdt = false;
        this.loadindividualadd = true;
        this.loadIndividualReg = false;
        this.loadIndividualpat = false;

    }
    loadReg() {
        this.loadindividualdt = false;
        this.loadindividualadd = false;
        this.loadIndividualReg = true;
        this.loadIndividualpat = false;

    }
    loadpat() {
        this.loadindividualdt = false;
        this.loadindividualadd = false;
        this.loadIndividualReg = false;
        this.loadIndividualpat = true;

    }
    //upload logo
    initUploaders(): void {
        this.logoUploader = this.createUploader('/TenantCustomization/UploadLogo', (result) => {
            this.appSession.tenant.logoFileType = result.fileType;
           this.appSession.tenant.logoId = result.id;
        });

        this.customCssUploader = this.createUploader('/TenantCustomization/UploadCustomCss', (result) => {
            this.appSession.tenant.customCssId = result.id;

            let oldTenantCustomCss = document.getElementById('TenantCustomCss');
            if (oldTenantCustomCss) {
                oldTenantCustomCss.remove();
            }

            let tenantCustomCss = document.createElement('link');
            tenantCustomCss.setAttribute('id', 'TenantCustomCss');
            tenantCustomCss.setAttribute('rel', 'stylesheet');
            tenantCustomCss.setAttribute(
                'href',
                AppConsts.remoteServiceBaseUrl +
                    '/TenantCustomization/GetCustomCss?tenantId=' +
                    this.appSession.tenant.id
            );
            document.head.appendChild(tenantCustomCss);
        });

}
    createUploader(url: string, success?: (result: any) => void): FileUploader {
        const uploader = new FileUploader({ url: AppConsts.remoteServiceBaseUrl + url });

        uploader.onAfterAddingFile = (file) => {
            file.withCredentials = false;
        };

        uploader.onSuccessItem = (item, response, status) => {
            const ajaxResponse = <IAjaxResponse>JSON.parse(response);
            if (ajaxResponse.success) {
                this.notify.info(this.l('SavedSuccessfully'));
                if (success) {
                    success(ajaxResponse.result);
                }
            } else {
                this.message.error(ajaxResponse.error.message);
            }
        };

        const uploaderOptions: FileUploaderOptions = {};
        uploaderOptions.authToken = 'Bearer ' + this._tokenService.getToken();
        uploaderOptions.removeAfterUpload = true;
        uploader.setOptions(uploaderOptions);
        return uploader;
    }

    onUploadLogoInputChange(files: FileList) {
        this.uploadLogoInputLabel.nativeElement.innerText = Array.from(files)
            .map((f) => f.name)
            .join(', ');
    }
    uploadLogo(): void {
        this.logoUploader.uploadAll();
    }
    clearLogo(): void {
        this._tenantSettingsService.clearLogo().subscribe(() => {
            this.appSession.tenant.logoFileType = null;
            this.appSession.tenant.logoId = null;
            this.notify.info(this.l('ClearedSuccessfully'));
        });
    }


    //unicore
    async updateunicoredetails(){
        if(this.isunicoreFormValid())
      {
        if((this.tenants.vatid==null || this.tenants.vatid==undefined ||  !this.tenants.vatid)&&(
          this.documents.documentNumber==null || this.documents.documentNumber==undefined || !this.documents.documentNumber))
          {
          this.notify.error(this.l('Please fill either CR number or VAT number to save.'));
      
          }
          else if(await this.isvatRegistered()){
            this.notify.error(this.l('Entered VAT Number  already exists'));
          return null;
        }
          else{
        if(this.type=='Update')
        {
          this.tenants.id=this.tenantid;
          this.tenants.address=this.address;
          this.documents.documentType='CRN';
          this.unicoredocs.push(this.documents);
          this.tenants.documents=this.unicoredocs;
          this._tenantbasicdetailsServiceProxy.upadateTenant(this.tenants)
          .pipe(finalize(()=>(
              this.saving = false
      
          ))) 
          .subscribe(() => {
              this.notify.success(this.l('UpdatedSuccessfully'));
              this.state.emit();
      
          });
      }else
      {
        this.saveunicoredetails();
      }
      }
      }
      else{
        this.notify.error(this.l('Please fill all required basic and address details'));
      
      }
      }

      saveunicoredetails(){

        if(this.isFormValid())
        {
          this.tenants.address=this.address;
          this.state.emit(this.tenants);
          }  else{
        
            this.notify.error(this.l('Please fill all required basic and address details'));
        
          }
      }

      async isvatRegistered(){
        //return false 
        if((this.tenants.vatid).charAt(10)!='1')
        {
            if(this.vatid != this.tenants.vatid)
            {
           var res = await firstValueFrom(this._tenantRegistrationService.checkIfVatExists(this.tenants.vatid,true))
            return res
            }
            else
            {
                return false;
            }
        }
        else
        {
            return res;
        }
    }
}





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
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { Table } from 'primeng/table';
import Swal from 'sweetalert2/dist/sweetalert2.js';

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
    TenantSettingsServiceProxy,
    TransactionCategoryServiceProxy,
    CreateOrEditTenantShareHoldersDto,
    CreateOrEditTenantBusinessPurchaseDto,
    CreateOrEditTenantBusinessSuppliesDto,
    CreateOrEditTenantPurchaseVatCateoryDto,
    CreateOrEditTenantSupplyVATCategoryDto,
    DesignationServiceProxy,
    GetDesignationForViewDto,
} from '@shared/service-proxies/service-proxies';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { Location } from '@angular/common';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';

@Component({
    selector: 'ConsoritiumTenant',
    templateUrl: './ConsoritiumTenant.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ConsoritiumTenantComponent extends AppComponentBase {
    @Output() state = new EventEmitter<any>();
    @Input() tenantid: number;
    @Input() type: string;
    @Input() tenant: [];
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('uploadLogoInputLabel') uploadLogoInputLabel: ElementRef;
    customers: any[] = [];
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
    Documentitem: CreateOrEditTenantDocumentsDto = new CreateOrEditTenantDocumentsDto();
    Documentitems: CreateOrEditTenantDocumentsDto[] = [];
    BusinessPurchase: CreateOrEditTenantBusinessPurchaseDto = new CreateOrEditTenantBusinessPurchaseDto();
    businessSupplies: CreateOrEditTenantBusinessSuppliesDto = new CreateOrEditTenantBusinessSuppliesDto();
    purchaseVatCateory: CreateOrEditTenantPurchaseVatCateoryDto = new CreateOrEditTenantPurchaseVatCateoryDto();
    supplyVATCategory: CreateOrEditTenantSupplyVATCategoryDto = new CreateOrEditTenantSupplyVATCategoryDto();
    documents: CreateOrEditTenantDocumentsDto = new CreateOrEditTenantDocumentsDto();
    turnoverslabs: GetBusinessTurnoverSlabForViewDto[] = [];
    OperationalModels: GetBusinessOperationalModelForViewDto[] = [];
    remoteServiceBaseUrl = AppConsts.remoteServiceBaseUrl;
    logoUploader: FileUploader;
    customCssUploader: FileUploader;
    doctype: GetDocumentMasterForViewDto[] = [];
    designationtype: GetDesignationForViewDto[] = [];
    transtype: GetTransactionCategoryForViewDto[] = [];
    taxcat: GetTaxCategoryForViewDto[] = [];
    invoicetype: GetInvoiceTypeForViewDto[] = [];
    purchasevat: any[];
    salesvat: any[];
    businesspurchase: any[];
    businesssales: any[];
    profileType = 'Individual';
    partnerShareHolderItems: CreateOrEditTenantShareHoldersDto[] = [];
    partnerShareHolderItem: CreateOrEditTenantShareHoldersDto = new CreateOrEditTenantShareHoldersDto();
    ishost = true;
    constructor(
        injector: Injector,
        private _CustomerDetailsServiceProxy: CustomersesServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _location: Location,
        private _masterCountriesServiceProxy: CountryServiceProxy,
        private _masterConstitutionServiceProxy: ConstitutionServiceProxy,
        private _tenantbasicdetailsServiceProxy: TenantBasicDetailsServiceProxy,
        private _designationServiceProxy: DesignationServiceProxy,
        private _sessionService: AppSessionService,
        private _BusinessTurnoverSlabServiceProxy: BusinessTurnoverSlabServiceProxy,
        private _BusinessOperationalModelServiceProxy: BusinessOperationalModelServiceProxy,
        private _tenantSettingsService: TenantSettingsServiceProxy,
        private _masterDocumentsServiceProxy: DocumentMasterServiceProxy,
        private _masterInvoiceTypeServiceProxy: InvoiceTypeServiceProxy,
        private _masterSectorServiceProxy: SectorServiceProxy,
        private _masterBusinessCategoryServiceProxy: TransactionCategoryServiceProxy,
        private _masterTaxCategoryServiceProxy: TaxCategoryServiceProxy,
        private fb: FormBuilder,
        private _tokenService: TokenService
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
    isFormValid() {
        return this.basicForm.valid;
    }
    ngOnInit() {
        if (this._sessionService.tenantId !== undefined || this._sessionService.tenantId == null) {
            this.ishost = false;
            this.initUploaders();
        }
        this.tenants.tenantType = 'Company';
        this.tenants.constitutionType = 'Consortium';
        this.tenants.businessCategory = '';
        this.tenants.operationalModel = '';
        this.tenants.turnoverSlab = '';
        this.tenants.designation = '';
        this.address.country = '';
        this.purchaseVatCateory.vatCategoryName = '';
        this.supplyVATCategory.vatCategoryName = '';
        this.businessSupplies.businessSupplies = '';
        this.BusinessPurchase.businessPurchase = '';
        this.tenants.parentEntityCountryCode = '';
        this.tenants.vatReturnFillingFrequency = '';
        this.loadindividualdt = true;
        this.tenantid = this._sessionService.tenantId;
        if (this.type === 'Update') {
            this.show(this.tenantid);
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
        this.getdoctypeDropdown();
        this.getdesignationDropdown();
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
    getdoctypeDropdown() {
        this._masterDocumentsServiceProxy
            .getAll('', '', '', '', undefined, undefined, undefined, undefined, 200)
            .subscribe((result) => {
                this.doctype = result.items;
            });
    }
    getdesignationDropdown() {
        this._designationServiceProxy
            .getAll('', '', '', '', undefined, undefined, undefined, 200)
            .subscribe((result) => {
                this.designationtype = result.items;
            });
    }
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
            this.tenants.emailID = data[0].emailID?.trim();
            this.tenants.nationality = data[0].nationality?.trim();
            this.tenants.contactNumber = data[0].contactNumber?.trim();
            this.tenants.contactPerson = data[0].contactPerson?.trim();
            this.tenants.designation = data[0].designation?.trim();
            this.tenants.emailID = data[0].emailID?.trim();
            this.tenants.businessCategory = data[0].businessCategory?.trim();
            this.tenants.operationalModel = data[0].operationalModel?.trim();
            this.tenants.turnoverSlab = data[0].turnoverSlab?.trim();
            this.tenants.lastReturnFiled = data[0].lastReturnFiled?.trim();
            this.tenants.vatReturnFillingFrequency = data[0].vatReturnFillingFrequency?.trim();
            this.address.buildingNo = data[0]?.buildingNo;
            this.address.additionalBuildingNumber = data[0]?.additionalBuildingNumber;
            this.address.street = data[0]?.street?.trim();
            this.address.additionalStreet = data[0]?.additionalStreet?.trim();
            this.address.city = data[0]?.city?.trim();
            this.address.state = data[0]?.state;
            this.address.country = data[0]?.country;
            this.address.postalCode = data[0]?.postalCode;
            this.address.neighbourhood = data[0]?.neighbourhood?.trim();
            this.documents.documentNumber = data[0].documentNumber?.trim();

            this.BusinessPurchase.businessPurchase = data[0].businessPurchase?.trim();
            this.businessSupplies.businessSupplies = data[0].businessSupplies?.trim();
            this.supplyVATCategory.vatCategoryName = data[0].vatCategoryName?.trim();
            this.purchaseVatCateory.vatCategoryName = data[0].vatCategoryName?.trim();

            for (let i = 0; i < data.length; i++) {
                this.Documentitem.docUniqueId = data[i].docunique;
                this.Documentitem.docUniqueId = data[i].docunique;
                this.Documentitem.documentId = data[i].documentId;
                this.Documentitem.documentNumber = data[i].documentNumber;
                this.Documentitem.documentType = data[i].documentType;
                this.Documentitem.registrationDate = data[i].registrationDate;
                this.Documentitems.push(this.Documentitem);
                this.Documentitem = new CreateOrEditTenantDocumentsDto();
            }
            this.tenants.address = this.address;
            this.tenants.documents = this.Documentitems;
        });

        this._tenantbasicdetailsServiceProxy.getTenantpartnerinfoById(tenantId).subscribe((patdata) => {
            for (let i = 0; i < patdata.length; i++) {
                this.partnerShareHolderItem.shareUniqueId = patdata[0].patunique;
                this.partnerShareHolderItem.shareUniqueId = patdata[0].patunique;
                this.partnerShareHolderItem.partnerName = patdata[i].partnerName;
                this.partnerShareHolderItem.constitutionName = patdata[i].constitutionName;
                this.partnerShareHolderItem.domesticName = patdata[i].domesticName;
                this.partnerShareHolderItem.designation = patdata[i].designation;
                this.partnerShareHolderItem.capitalAmount = patdata[i].capitalAmount;
                this.partnerShareHolderItem.capitalShare = patdata[i].capitalShare;
                this.partnerShareHolderItem.profitShare = patdata[i].profitShare;
                this.partnerShareHolderItem.representativeName = patdata[i].representativeName;
                this.partnerShareHolderItem.nationality = patdata[i].nationality;
                this.partnerShareHolderItems.push(this.partnerShareHolderItem);
                this.partnerShareHolderItem = new CreateOrEditTenantShareHoldersDto();
            }
        });
    }
    updatedetails() {
        if (
            (this.tenants.vatid == null || this.tenants.vatid === undefined || !this.tenants.vatid) &&
            (this.documents.documentNumber === null ||
                this.documents.documentNumber === undefined ||
                !this.documents.documentNumber)
        ) {
            this.notify.error(this.l('Please fill either CR number or VAT ID to save.'));
        } else {
            if (this.type === 'Update') {
                this.tenants.id = this.tenantid;
                this.tenants.address = this.address;
                this.tenants.documents = this.Documentitems;
                this.tenants.businessPurchase = this.BusinessPurchase;
                this.tenants.businessSupplies = this.businessSupplies;
                this.tenants.purchaseVatCateory = this.purchaseVatCateory;
                this.tenants.supplyVATCategory = this.supplyVATCategory;
                this.tenants.partnerShareHolders = this.partnerShareHolderItems;
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
        if (this.isFormValid()) {
            this.tenants.address = this.address;
            this.tenants.documents = this.Documentitems;
            this.tenants.businessPurchase = this.BusinessPurchase;
            this.tenants.businessSupplies = this.businessSupplies;
            this.tenants.purchaseVatCateory = this.purchaseVatCateory;
            this.tenants.supplyVATCategory = this.supplyVATCategory;
            this.tenants.partnerShareHolders = this.partnerShareHolderItems;
            this.state.emit(this.tenants);
        } else {
            this.notify.error(this.l('Please fill all required basic and address details'));
        }
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

    additem() {
        if (
            !(
                this.Documentitem.documentId &&
                this.Documentitem.documentNumber &&
                this.Documentitem.registrationDate &&
                this.Documentitem.documentType
            )
        ) {
            this.notify.error(this.l('Please fill all  document details to add a document.'));
            return;
        }

        let UPDATED = false;
        if (this.Documentitems.length > 0) {
            for (let k = 0; k < this.Documentitems.length; k++) {
                if (this.Documentitems[k].documentType === this.Documentitem.documentType) {
                    this.updateGrid(k);
                    UPDATED = true;
                    break;
                }
            }
        }
        if (!UPDATED) {
            this.Documentitems.push(this.Documentitem);
            this.Documentitem = new CreateOrEditTenantDocumentsDto();
        }
    }

    clearitem() {
        this.Documentitem = new CreateOrEditTenantDocumentsDto();
    }

    deleteItem(index: number) {
        this.Documentitems.splice(index, 1);
    }

    updateGrid(k: number) {
        Swal.fire({
            title: 'Do you want to overwrite the existing document details?',
            text: '',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, overwrite it!',
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire('Updated!', 'Document details has been updated.', 'success');
                this.Documentitems[k].documentNumber = this.Documentitem?.documentNumber;
                this.Documentitems[k].registrationDate = this.Documentitem?.registrationDate;
                this.Documentitems[k].documentId = this.Documentitem?.documentId;
                this.Documentitem = new CreateOrEditTenantDocumentsDto();
            }
        });
    }

    addpatitem() {
        if (
            !(
                this.partnerShareHolderItem.partnerName &&
                this.partnerShareHolderItem.constitutionName &&
                this.partnerShareHolderItem.nationality &&
                this.partnerShareHolderItem.representativeName &&
                this.partnerShareHolderItem.designation &&
                this.partnerShareHolderItem.capitalAmount &&
                this.partnerShareHolderItem.capitalShare &&
                this.partnerShareHolderItem.profitShare
            )
        ) {
            this.notify.error(this.l('Please fill all  Information details to add a document.'));
            return;
        }
        let PATUPDATED = false;
        if (this.partnerShareHolderItems.length > 0) {
            for (let k = 0; k < this.partnerShareHolderItems.length; k++) {
                if (
                    this.partnerShareHolderItems[k].partnerName === this.partnerShareHolderItem.partnerName &&
                    this.partnerShareHolderItems[k].designation === this.partnerShareHolderItem.designation
                ) {
                    this.updatepatGrid(k);
                    PATUPDATED = true;
                    break;
                }
            }
        }
        if (!PATUPDATED) {
            this.partnerShareHolderItems.push(this.partnerShareHolderItem);
            this.partnerShareHolderItem = new CreateOrEditTenantShareHoldersDto();
        }
    }

    clearpatitem() {
        this.partnerShareHolderItem = new CreateOrEditTenantShareHoldersDto();
    }

    deletepatItem(index: number) {
        this.partnerShareHolderItems.splice(index, 1);
    }

    updatepatGrid(k: number) {
        Swal.fire({
            title: 'Do you want to overwrite the existing partner details?',
            text: '',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, overwrite it!',
            cancelButtonText: 'Continue',
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire('Updated!', 'Partner details has been updated.', 'success');
                this.partnerShareHolderItems[k].partnerName = this.partnerShareHolderItem?.partnerName;
                this.partnerShareHolderItems[k].nationality = this.partnerShareHolderItem?.nationality;
                this.partnerShareHolderItems[k].designation = this.partnerShareHolderItem?.designation;
                this.partnerShareHolderItems[k].capitalAmount = this.partnerShareHolderItem?.capitalAmount;
                this.partnerShareHolderItems[k].capitalShare = this.partnerShareHolderItem?.capitalShare;
                this.partnerShareHolderItems[k].profitShare = this.partnerShareHolderItem?.profitShare;
                this.partnerShareHolderItem = new CreateOrEditTenantShareHoldersDto();
            } else {
                this.partnerShareHolderItems.push(this.partnerShareHolderItem);
                this.partnerShareHolderItem = new CreateOrEditTenantShareHoldersDto();
            }
        });
    }
}

import { Component, Injector, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy, VendorsesServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { Location } from '@angular/common';
import { CustomerAddressDto, ConstitutionServiceProxy, GetConstitutionForViewDto, CountryServiceProxy, GetCountryForViewDto, CustomersesServiceProxy, CreateOrEditCustomersDto } from '@shared/service-proxies/service-proxies';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
@Component({
  selector: 'createoreditvendors',
  templateUrl: './createVendors.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class CreateVendorComponent extends AppComponentBase {
  editMode: boolean = true;
  profileType: string = "Individual";
  customer: CreateOrEditCustomersDto = new CreateOrEditCustomersDto();
  address: CustomerAddressDto = new CustomerAddressDto();
  countries: GetCountryForViewDto[] = [];
  constitutionTypes: GetConstitutionForViewDto[] = [];
  companyRequiredForm: FormGroup;
  individualRequiredForm: FormGroup;
  addressRequiredForm: FormGroup;
  isSaving: boolean;
  commonForm: FormGroup;
  loadIndividualdt: boolean = false;
  customerId: number;
  constitutionType: string;
  tenattype: string;
  customerForm() {
    this.companyRequiredForm = this.fb.group({
      companyName: ['', Validators.required],
      companyEmail: ['', Validators.required],
      companyTel: ['', Validators.required],
      companyConstitution: ['', Validators.required],
      companyVat: ['', [Validators.required,
      Validators.minLength(15),
      Validators.maxLength(15),
      Validators.pattern("^3[0-9]*3$")]],
      companyGVat: ['', [
        Validators.minLength(15),
        Validators.maxLength(15),
        Validators.pattern("^3[0-9]*3$")]]
    });
    this.individualRequiredForm = this.fb.group({
      firstName: ['', Validators.required],
      middleName: [''],
      lastName: [''],
      title: ['', Validators.required],
      email: ['',],
      mobileNo: ['',],
      gender: ['',],
    });
    this.addressRequiredForm = this.fb.group({
      buildingNo: ['', []],
      street: ['', []],
      neighbourhood: ['', []],
      city: ['', []],
      country: ['', Validators.required],
      state: ['', []],
      postalCode: ['', []],
    });
    this.commonForm = this.fb.group({
      profileType: ['Individual', []],
    });
    this.commonForm.get('profileType').valueChanges.subscribe(val => {
      this.profileType = this.commonForm.get('profileType').value      
      if (this.commonForm.get('profileType').value == "Individual") { // for setting validations
        this.addressRequiredForm.get('buildingNo').setValidators([]);
        this.addressRequiredForm.get('street').setValidators([]);
        this.addressRequiredForm.get('neighbourhood').setValidators([]);
        this.addressRequiredForm.get('city').setValidators([]);
        this.addressRequiredForm.get('state').setValidators([]);
        this.addressRequiredForm.get('country').setValidators([Validators.required]);
        this.addressRequiredForm.get('postalCode').setValidators([]);
        this.individualRequiredForm.get('firstName').setValidators([Validators.required]);
        this.individualRequiredForm.get('middleName').setValidators([]);
        this.individualRequiredForm.get('lastName').setValidators([]);
        this.individualRequiredForm.get('title').setValidators([Validators.required]);
        this.individualRequiredForm.get('email').setValidators([]);
        this.individualRequiredForm.get('mobileNo').setValidators([]);
        this.individualRequiredForm.get('gender').setValidators([]);
      } else {
        this.addressRequiredForm.get('buildingNo').setValidators([Validators.required,
        Validators.maxLength(4),
        Validators.pattern("^[0-9]*$")]);
        this.addressRequiredForm.get('street').setValidators([Validators.required]);
        this.addressRequiredForm.get('neighbourhood').setValidators([Validators.required]);
        this.addressRequiredForm.get('city').setValidators([Validators.required]);
        this.addressRequiredForm.get('country').setValidators([Validators.required]);
        this.addressRequiredForm.get('state').setValidators([Validators.required]);
        this.addressRequiredForm.get('postalCode').setValidators([Validators.required,
        Validators.maxLength(5),
        Validators.minLength(4),
        Validators.pattern("^[0-9]*$")]);
        this.individualRequiredForm.get('firstName').setValidators([Validators.required]);
        this.individualRequiredForm.get('middleName').setValidators([Validators.required]);
        this.individualRequiredForm.get('lastName').setValidators([Validators.required]);
        this.individualRequiredForm.get('title').setValidators([Validators.required]);
        this.individualRequiredForm.get('email').setValidators([Validators.required]);
        this.individualRequiredForm.get('mobileNo').setValidators([Validators.required]);
        this.individualRequiredForm.get('gender').setValidators([Validators.required]);
      }
      this.addressRequiredForm.get('buildingNo').updateValueAndValidity();
      this.addressRequiredForm.get('street').updateValueAndValidity();
      this.addressRequiredForm.get('neighbourhood').updateValueAndValidity();
      this.addressRequiredForm.get('city').updateValueAndValidity();
      this.addressRequiredForm.get('country').updateValueAndValidity();
      this.addressRequiredForm.get('postalCode').updateValueAndValidity();
      this.addressRequiredForm.get('state').updateValueAndValidity();
      this.individualRequiredForm.get('firstName').updateValueAndValidity();
      this.individualRequiredForm.get('middleName').updateValueAndValidity();
      this.individualRequiredForm.get('lastName').updateValueAndValidity();
      this.individualRequiredForm.get('title').updateValueAndValidity();
      this.individualRequiredForm.get('email').updateValueAndValidity();
      this.individualRequiredForm.get('mobileNo').updateValueAndValidity();
      this.individualRequiredForm.get('gender').updateValueAndValidity();
    });
  }
  isFormValid() {
    if (this.profileType == "Individual") {      
      return this.individualRequiredForm.valid && this.addressRequiredForm.valid;
    } else {      
      return this.companyRequiredForm.valid && this.addressRequiredForm.valid && this.individualRequiredForm.valid;
    }
  }
  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _customerServiceProxy: VendorsesServiceProxy,
    private _masterCountriesServiceProxy: CountryServiceProxy,
    private _masterConstitutionServiceProxy: ConstitutionServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _location: Location
  ) {
    super(injector);
    this.customerForm();
  }
  ngOnInit(): void {
    this.getCountriesDropdown();
    this.getSector();
    this.getConstitutionType();
    this.getGender();
    this.getTitle();
    this.getAddressType();
    this.getFileType();
    this.getIdentifier();
    this.profileType = 'Individual'
    this.customerId = this._activatedRoute.snapshot.queryParams['id'];
    this.constitutionType = this._activatedRoute.snapshot.queryParams['constitutionType'];
    this.tenattype = this._activatedRoute.snapshot.queryParams['tenantType'];
    this.profileType = this.tenattype?.trim() || 'Individual';
    this.show();
  }
  getCountriesDropdown() {
    this._masterCountriesServiceProxy.getAll("", "", "", "", "", undefined, undefined, "", "", undefined, undefined, undefined, undefined, undefined)
      .subscribe(result => {
        this.countries = result.items;
      });
  }
  getSector() {
  }
  getConstitutionType() {
    this._masterConstitutionServiceProxy.getAll("", "", "", undefined, undefined, undefined, undefined, undefined)
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
    this.customer.name = this.individualRequiredForm.get('firstName').value || " ";
    this.customer.legalName = this.individualRequiredForm.get('firstName').value || " ";
    this.customer.tenantType = this.individualRequiredForm.get('firstName').value || " ";
    this.customer.constitutionType = this.individualRequiredForm.get('companyConstitution').value || " ";
    this.customer.contactNumber = this.individualRequiredForm.get('firstName').value || " ";
    this.customer.contactPerson = this.individualRequiredForm.get('firstName').value || " ";
    this.customer.emailID = this.individualRequiredForm.get('firstName').value || " ";
    this.customer.nationality = this.individualRequiredForm.get('firstName').value || " ";
    this.customer.designation = this.individualRequiredForm.get('firstName').value || " ";
  }
  show(): void {
    this.customer.id = this.customerId;
    this.customer.constitutionType = this.constitutionType;
    this.profileType = this.tenattype?.trim() || 'Individual';
  }
  create() {
    this.mapFormtoDto();
    this.editMode = true;
  }
  save() {
    this.isSaving = true;
  }
  gotoCustomer(): void {
    this._location.back();
  }
  changestate() {
    this.customer.constitutionType = '';
  }
  loadIndividual() {
    this.loadIndividualdt = true;
  }
}

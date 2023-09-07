import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { SalesInvoicesServiceProxy, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
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
import {CustomerAddressDto,ConstitutionServiceProxy,GetConstitutionForViewDto,CountryServiceProxy,GetCountryForViewDto,CustomersesServiceProxy,CreateOrEditCustomersDto} from '@shared/service-proxies/service-proxies';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';


@Component({
  selector:'MasterConfiguration',
  templateUrl: './masterConfiguration.component.html',
  styleUrls: ['./masterConfiguration.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class MasterConfigurationComponent extends AppComponentBase {

  profileType: string = "Individual";
  selectedOption: string;
  validstatus: boolean;
  customerId:number;
  FinYearData: any;
  constitutionType:string;
  activeFinYear: string;
  tenattype:string;
  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _customerServiceProxy: CustomersesServiceProxy,
    private _masterCountriesServiceProxy: CountryServiceProxy,
    private _salesInvoiceServiceProxy: SalesInvoicesServiceProxy,
    private _masterConstitutionServiceProxy: ConstitutionServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService,
    private _dateTimeService: DateTimeService,
    private _router: Router,
    private _location: Location
  ) {
    super(injector);
  }
  ngOnInit(): void {
    this.getFinancialYear();
    this.handleButtonClick(undefined);
    this.activeFinYear = this.FinYearData[0].finYear;
    //console.log(this.FinYearData,'this.FinYearData')
    //this.findActiveFinYear();
   }

   findActiveFinYear() {
    console.log(event,'Finyear1')
    const activeFinYearObj = this.FinYearData.find(year => year.isActive === true);
    if (this.FinYearData.length > 0) {
      this.activeFinYear = activeFinYearObj.activeYear;
    } else if (this.FinYearData.length = 0) {
      this.activeFinYear = this.FinYearData[0].finYear;
    }
  }

   handleButtonClick(option: string) {
    this._salesInvoiceServiceProxy.updateMasterValidation(option)
    .subscribe((result) => {
      console.log(result,'result')
      this.validstatus = result;
      if(this.validstatus == true)
      {
        console.log(this.validstatus,'validstatus')
        this.selectedOption = 'Active';
      }
      else
      {
        console.log(this.validstatus,'invalidstatus')
        this.selectedOption = 'Inactive';
      }
    })
  }

  getFinancialYear(){
    this._salesInvoiceServiceProxy.getFinancialYear().subscribe((result) => 
    {
      this.FinYearData = result;
      this.findActiveFinYear();
    })
  }

  updateFinYear(event?: string){
    this._salesInvoiceServiceProxy.updateFinancialYear(event).subscribe((result) => 
    {
      console.log(event,'Finyear')
      this.getFinancialYear();
    });
    }
}

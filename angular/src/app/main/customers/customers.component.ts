import { AppConsts } from '@shared/AppConsts';

import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';

import { ActivatedRoute, Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';

import { appModuleAnimation } from '@shared/animations/routerTransition';

import { filter as _filter } from 'lodash-es';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { Table } from 'primeng/table';
import { CustomersesServiceProxy,GetCustomersForViewDto } from '@shared/service-proxies/service-proxies';
import {CreateCustomerComponent} from './createCustomers/createCustomers.component'

@Component({
 templateUrl: './customers.component.html',
encapsulation: ViewEncapsulation.None,
 animations: [appModuleAnimation()],

})

export class CustomerComponent extends AppComponentBase {
  @ViewChild('CreateCustomerModule', { static: true }) CreateCustomerModule: CreateCustomerComponent;

  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
 customers:any[]=[];
 filterText = '';
 searchQuery='';
 advancedFiltersAreShown = false;

 nameFilter = '';
 descriptionFilter = '';
 codeFilter = '';
 isActiveFilter = -1;

constructor(
 injector: Injector,
private _CustomerDetailsServiceProxy: CustomersesServiceProxy,
private _activatedRoute: ActivatedRoute,
) {
 super(injector);
 this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
}
ngAfterViewInit(): void {
  this.primengTableHelper.adjustScroll(this.dataTable);
}

ngOnInit(event? : LazyLoadEvent): void {

  this.GetCustomerByPage(null,"");
}



getCustomer(event?: LazyLoadEvent) {
  if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
 
      }
    }
      this.primengTableHelper.showLoadingIndicator();

        this._CustomerDetailsServiceProxy
            .getAll(
              this.filterText,
              '',
              '',
              this.nameFilter,
              '',
              '',
              '',
              '',
               '',
               '',
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
  }


GetCustomerByPage(event,searchQuery){
this._CustomerDetailsServiceProxy.getCustomerData().pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
  .subscribe((result) => {  
    console.log("here",result);
    this.customers = result;     
   });
}

search(){
console.log(this.searchQuery)
}
resetFilters(): void {
  this.filterText = '';
  this.nameFilter = '';

  this.getCustomer();
}
}
﻿import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';
import { ImportBatchDatasServiceProxy } from '@shared/service-proxies/service-proxies';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { type } from 'os';

@Component({
  selector: 'app-errorlist',
  templateUrl: './importstandardfilesErrorLists.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
  styleUrls: ['./importstandardfilesErrorLists.component.less'],
})
export class ImportstandardfilesErrorListsComponent extends AppComponentBase {
  @Input() batchid: number;
  @Input() fileName: string;
  @Input() Type: String;
  @Input() Deletepage:  boolean;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxBatchidFilter: number;
  maxBatchidFilterEmpty: number;
  minBatchidFilter: number;
  minBatchidFilterEmpty: number;
  errormsg: string;
  disable: boolean;
  hide: boolean;
  labels: any[];
  errorlist: any[] = [];
  columns: any[] = [
    { field: 'number', header: 'Number' },
    { field: 'date', header: 'Date' },
    { field: 'name', header: 'Name' },
    { field: 'errormessage', header: 'Error Message' },
    { field: 'status', header: 'Status' }
  ];
  selectCount = 0;
  isSelectAll = false;
  para: number;
  constructor(
    injector: Injector,
    private _importBatchDatasServiceProxy: ImportBatchDatasServiceProxy,
    private route: ActivatedRoute,
    private _fileDownloadService: FileDownloadService,
  ) {
    super(injector);
    this.batchid = this.route.snapshot.params['id'];
  }

  // eslint-disable-next-line @angular-eslint/use-lifecycle-interface
  ngOnInit(): void {
    if (this.Deletepage === undefined){
      this.Deletepage = false;
    };
    this.disable = true;
    this.GetData();
  }

  GetData() {

    if (this.Type === 'Master') {
      this._importBatchDatasServiceProxy.getMasterReportDataByID(this.batchid, this.para).subscribe(result => {
        this.labels = Object.keys(result[0]);
        for (let i = 0; i < result.length; i++) {
          result[i]['isSelected'] = false;
        }
        this.errorlist = result;
        let override = this.errorlist.filter(e => e.isOverride === 1);
        if (override.length) {
          this.hide = true;
        } else{
          this.hide = false;
        }
      });
    } else {

      this._importBatchDatasServiceProxy.execgetdataSP(this.batchid, this.para).subscribe(result => {
        this.labels = Object.keys(result[0]);
        for (let i = 0; i < result.length; i++) {
          result[i]['isSelected'] = false;
        }
        this.errorlist = result;
        let override = this.errorlist.filter(e => e.isOverride === 1);
        if (override.length) {
          this.hide = true;
        } else{
          this.hide = false;
        }
      });
    }

  }
  exportToExcel(batchId: number): void {
    if (this.Type === 'Master') {
      this.para = 1;
      this.fileName = 'MasterBatchDetailedReport' + '(' + this.batchid + ').xlsx';
      this._importBatchDatasServiceProxy
        .getMasterErrorListToExcel(this.fileName, this.batchid, this.para)
        .subscribe((result) => {
          this._fileDownloadService.downloadTempFile(result);
        });
    } else {
      this.para = 1;
      this.fileName = 'TransactionBatchDetailedReport' + '(' + this.batchid + ').xlsx';
      this._importBatchDatasServiceProxy
        .getErrorListToExcel(this.fileName, this.batchid, this.para)
        .subscribe((result) => {
          this._fileDownloadService.downloadTempFile(result);
        });
    }
  }
  selectAll() {
    this.disable = false;
    for (let i = 0; i < this.errorlist.length; i++) {
      if (this.isSelectAll) {
        this.errorlist[i]['isSelected'] = false;
      } else {
        if (this.errorlist[i]['isOverride'] === 1) {
          this.errorlist[i]['isSelected'] = true;
        }
      }
      this.selectCount = this.errorlist.length;
    }
  }
  checkAllSelected(currentValue) {
    this.disable = false;
    if (currentValue === true) {
      this.selectCount++;
    } else {
      this.selectCount--;
    }
    this.isSelectAll = this.selectCount === this.errorlist.length;
  }

  Override() {
    debugger;
    let selectedData = [];
    for (let i = 0; i < this.errorlist.length; i++) {
      if (this.errorlist[i]['isSelected'] === true && this.errorlist[i]['isOverride'] === 1) {
        selectedData.push({
          uniqueId: this.errorlist[i]['uniqueId'],
        });
      }
    }
    let selectedDataString = JSON.stringify(selectedData);
    if (this.Type === 'Master') {
      console.log(selectedDataString,this.batchid,'override')
      this._importBatchDatasServiceProxy.overrideErrorsMasters(this.batchid, selectedData).subscribe(result => {
        if (result) {
          this.notify.success(this.l('Override Success'));
          this.disable = true;
          this.GetData();
        } else {
          this.notify.error(this.l('Override Failed'));
        }
      });
    } else {
      console.log(selectedDataString,this.batchid,'override')
      console.log(selectedData,this.batchid,'override not json')
      this._importBatchDatasServiceProxy.overrideErrors(this.batchid, selectedData).subscribe(result => {
        if (result) {
          this.notify.success(this.l('Override Success'));
          this.disable = true;
          this.GetData();
        } else {
          this.notify.error(this.l('Override Failed'));
        }
      });
    }
  }
}

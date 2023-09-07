

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { finalize } from 'rxjs/operators';
import { FileDownloadService } from '@shared/utils/file-download.service';

interface Invoice {
  issueDate: DateTime;
  count: number;
  vatAmount: number;
  standardRated: number;
  zeroRated: number;
  exports: number;
  privateHealth: number;
  exempt: number;
  vatRate: number;
  totalAmount: number;
}

@Component({
  templateUrl: './CustomerReport.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
  styleUrls: ['./Customerreport.component.less'],
})

export class CustomerReportComponent extends AppComponentBase {
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];

  tenantId: Number;
  tenantName: string;
  fromDate: Date;
  toDate: Date;
  detailedInvoices: any[] = [];
  invoices: any[] = [];
  vatAmount = 0;
  totalAmount = 0;
  count = 0;
  taxable = 0;
  zeroRated = 0;
  health = 0;
  export = 0;
  exempt = 0;
  isdisable = false;
  OutofScope = 0;

  columns: any[] = [
    { field: 'name', header: 'Name' },
    { field: 'constitutionType', header: 'Constitution Type' },
    { field: 'country', header: 'Country' },
    { field: 'contactNumber', header: 'Contact Number' },
    { field: 'documentNumber', header: 'Document Number' }];

  exportColumns: any[] = this.columns.map(col => ({ title: col.header, dataKey: col.field }));
  _selectedColumns: any[] = this.columns;

  constructor(
    injector: Injector,
    private _ReportServiceProxy: ReportServiceProxy,
    private _sessionService: AppSessionService,
    private _dateTimeService: DateTimeService,
    private _fileDownloadService: FileDownloadService,

  ) {
    super(injector);
  }
  @Input() get selectedColumns(): any[] {
    return this._selectedColumns;
  }

  set selectedColumns(val: any[]) {
    this._selectedColumns = this.columns.filter(col => val.includes(col));
  }

  ngOnInit(): void {
    this.getCustomerReport();
    this.tenantId = this._sessionService.tenantId;
    this.tenantName = this._sessionService.tenancyName;
  }

  ResetData() {
    this.vatAmount = 0;
    this.export = 0;
    this.taxable = 0;
    this.totalAmount = 0;
    this.health = 0;
    this.exempt = 0;
    this.zeroRated = 0;
    this.count = 0;
    this.OutofScope = 0;
  }

  getCustomerReport() {
    this.ResetData();
    this.isdisable = true;
    this._ReportServiceProxy.getMasterReport('CUSTOMER')
      .pipe(finalize(() => this.isdisable = false))
      .subscribe((result) => {
        this.invoices = result;
        this.invoices.forEach(element => {
          this.vatAmount += element.vatAmount;
          this.totalAmount += element.totalAmount;
          this.taxable += element.taxableAmount;
          this.zeroRated += element.zeroRated;
          this.health += element.healthCare;
          this.exempt += element.exempt;
          this.export += element.exports;
          this.OutofScope += element.outofScope;
          this.count += parseInt(element.invoicenumber);
        });
      });
  }

  parseDate(dateString: string): DateTime {
    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());
    }
    return null;
  }

  getDate(dateString: string): string {
    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString()).toISODate();
    }
    return null;
  }
  addFooter(li: any[]): any[] {
    li.push({
      invoiceDate: 'Total',
      taxableAmount: this.taxable,
      zeroRated: this.zeroRated,
      exempt: this.exempt,
      exports: this.export,
      outofScope: this.OutofScope,
      vatAmount: this.vatAmount,
      totalAmount: this.totalAmount
    });
    return li;
  }

  removeFooter(li: any[]): any[] {
    li.pop();
    return li;
  }
  downloadExcel(li: any[]) {
    this._ReportServiceProxy.getReportExcel(this.tenantName, 'Customer Matser Report', 'CUSTOMER')
      .subscribe((result) => {
        this._fileDownloadService.downloadTempFile(result);
      });
  }
}

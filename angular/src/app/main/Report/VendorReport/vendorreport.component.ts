

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input, forwardRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { finalize } from 'rxjs/operators';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { Table } from 'primeng/table';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
  templateUrl: './vendorReport.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
  styleUrls: ['./vendorreport.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => VendorReportComponent),  // replace name as appropriate
      multi: true
    }
  ]
})
export class VendorReportComponent extends AppComponentBase {
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];

  tenantId: Number;
  tenantName: string;
  fromDate: Date;
  toDate: Date;
  invoices: any[] = [];
  selectedInvoices: any[] = [];
  code: string;
  vatAmount = 0;
  totalAmount = 0;
  taxableAmount = 0;
  zeroRated = 0;
  health = 0;
  export = 0;
  exempt = 0;
  Gov = 0;
  OutofScope = 0;
  vatrate = 0;
  isdisable = false;

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
    this.getSalesDetailedReport();
    this.code = 'VATSAL000';
    this.tenantId = this._sessionService.tenantId;
    this.tenantName = this._sessionService.tenancyName;
  }

  ResetData() {
    this.vatAmount = 0;
    this.export = 0;
    this.taxableAmount = 0;
    this.totalAmount = 0;
    this.health = 0;
    this.exempt = 0;
    this.zeroRated = 0;
    this.Gov = 0;
    this.OutofScope = 0;
  }

  getSalesDetailedReport() {
    this.ResetData();
    this.isdisable = true;
    this._ReportServiceProxy.getMasterReport('VENDOR')
      .pipe(finalize(() => this.isdisable = false))
      .subscribe((result) => {
        this.invoices = result;
        this.invoices.forEach(element => {
          this.vatAmount = this.vatAmount + element.vatAmount;
          this.export = this.export + element.exports;
          this.taxableAmount = this.taxableAmount + element.taxableAmount;
          this.totalAmount = this.totalAmount + element.totalAmount;
          this.exempt = this.exempt + element.exempt;
          this.zeroRated += element.zeroRated;
          this.Gov += element.govtTaxableAmt;
          this.OutofScope += element.outofScope;
          this.vatrate += element.vatrate;
        });
      });
  }

  parseDate(dateString: string): DateTime {
    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());
    }
    return null;
  }
  clear(table: Table) {
    table.clear();
  }

  addFooter(li: any[]): any[] {
    li.push({ invoiceDate: 'Total', taxableAmount: this.taxableAmount, govtTaxableAmt: this.Gov, zeroRated: this.zeroRated, exempt: this.exempt, exports: this.export, outofScope: this.OutofScope, vatAmount: this.vatAmount, totalAmount: this.totalAmount });
    return li;
  }

  removeFooter(li: any[]): any[] {
    li.pop();
    return li;
  }

  downloadExcel(li: any[]) {
    this._ReportServiceProxy.getReportExcel(this.tenantName, 'Vendor Matser Report', 'VENDOR')
      .subscribe((result) => {
        this._fileDownloadService.downloadTempFile(result);
      });
  }
}

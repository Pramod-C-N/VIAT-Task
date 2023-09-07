

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { finalize } from 'rxjs/operators';
import { BsDatepickerConfig, BsDatepickerViewMode } from 'ngx-bootstrap/datepicker';
import { format } from 'path';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
  selector: 'demo-datepicker-min-mode',
  templateUrl: './detailedReport.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
  styleUrls: ['./detailedReport.component.less'],
})
export class DetailedReportComponent extends AppComponentBase {
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
  bsValue: Date = new Date();
  minMode: BsDatepickerViewMode = 'month'; // change for month:year
  bsConfig: Partial<BsDatepickerConfig>;
  tenantId: Number;
  tenantName: string;
  fromDate: Date;
  toDate: Date;
  data: any[] = [];
  nosType: any;
  vatAmount = 0;
  totalAmount = 0;
  taxable = 0;
  customPaid = 0;
  custom = 0;
  code : string;
  withholdingtaxamount = 0;
  taxrate = 0;
  totalamountPaid = 0;
  isdisable = false;
  columns: any[] = [
    { field: 'slno', header: 'Sl No' },
    { field: 'typeofpayments', header: 'Type of Payments' },
    { field: 'nameofPayee', header: 'Name of the Payee' },
    { field: 'country', header: 'Country' },
    { field: 'paymentdate', header: 'Payment Date' },
    { field: 'totalamountPaid', header: 'Total Amount Paid (SAR)' },
    { field: 'taxrate', header: 'Tax Rate' },
    { field: 'withholdingtaxamount', header: 'Withholding Tax Amount' },
    {field: 'standardRate', header: 'Standard Rate'},
    {field: 'dtTrate', header: 'DTT Rate'},
    {field: 'affiliationStatus', header: 'Affiliation Status'},
    {field: 'obtainedrequireddocuments', header: 'Obtained Required Documents'}];

  exportColumns: any[] = this.columns.map(col => ({ title: col.header, dataKey: col.field }));
  _selectedColumns: any[] = this.columns;

  constructor(
    injector: Injector,
    private _sessionService: AppSessionService,
    private _dateTimeService: DateTimeService,
    private _WHTReportsServiceProxy: ReportServiceProxy,
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

  ResetData() {
    this.vatAmount = 0;
    this.totalAmount = 0;
    this.taxable = 0;
    this.customPaid = 0;
    this.custom = 0;
    this.totalamountPaid = 0;
    this.taxrate = 0;
    this.withholdingtaxamount = 0;
  }

  ngOnInit(): void {
    this.tenantId = this._sessionService.tenantId;
    this.tenantName = this._sessionService.tenancyName;
    this.bsConfig = Object.assign({}, {
      minMode: this.minMode
    });
    this.code = "natureofservice";
    this.getReportType();
  }

  getSalesDetailedReport() {
    this.isdisable = true;
    this.ResetData();
    this._WHTReportsServiceProxy.getDetailedData(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()),this.code)
      .pipe(finalize(() => this.isdisable = false))
      .subscribe((result) => {
        this.data = result;
        this.data.forEach(element => {
          this.withholdingtaxamount += element.withholdingtaxamount;
          this.taxrate += element.taxrate;
          this.totalamountPaid += element.totalamountPaid;
        });
      });
  }

  parseDate(dateString: string): DateTime {
    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());
    }
  }

  addFooter(li: any[]): any[] {
    li.push({
      slno: 'Total',
      withholdingtaxamount: this.withholdingtaxamount,
      taxrate: this.taxrate,
      totalamountPaid: this.totalamountPaid
    });
    return li;
  }

  removeFooter(li: any[]): any[] {
    li.pop();
    return li;
  }

  downloadExcel() {
    this._WHTReportsServiceProxy.getWhtDetailedReportToExcel(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()), 'WHTDetailedReport', this.tenantName,this.code, false).subscribe((result) => {
      this._fileDownloadService.downloadTempFile(result);
    });
  }

  getReportType() {

    this._WHTReportsServiceProxy.getNatureOfServiceDropdown().subscribe((result) => {
      console.log(result,'nos');
        this.nosType = result;
    });
}
getnosData(event?: string) {
    this.code = event;
    this.getSalesDetailedReport();
    console.log(event);
}
}

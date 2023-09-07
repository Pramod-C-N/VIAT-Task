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
  templateUrl: './returnReport.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
  styleUrls: ['./returnReport.component.less'],
})
export class ReturnReportComponent extends AppComponentBase {
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
  bsValue: Date = new Date();
  minMode: BsDatepickerViewMode = 'month'; // change for month:year
  bsConfig: Partial<BsDatepickerConfig>;
  tenantId: Number;
  tenantName: string;
  fromDate: Date;
  toDate: Date;
  data: any[] = [];
  vatAmount = 0;
  totalAmount = 0;
  taxable = 0;
  customPaid = 0;
  custom = 0;
  isdisable = false;
  taxDue = 0;
  taxrate = 0;
  totalamountPaid = 0;
  columns: any[] = [
    { field: 'slno', header: 'Sl Number' },
    { field: 'typeofpayments', header: 'Type of Payment' },
    { field: 'nameofPayee', header: 'Name of the Payee(from which tax is withheld)' },
    { field: 'paymentdate', header: 'Payment Date' },
    { field: 'totalamountPaid', header: 'Total Amount Paid (SAR)' },
    { field: 'taxrate', header: 'Tax Rate' },
    { field: 'taxDue', header: 'Tax Due' }];
  exportColumns: any[] = this.columns.map(col => ({ title: col.header, dataKey: col.field }));
  _selectedColumns: any[] = this.columns;

  constructor(
    injector: Injector,
    private _sessionService: AppSessionService,
    private _dateTimeService: DateTimeService,
    private _WHTReportServiceProxy: ReportServiceProxy,
    private _fileDownloadService: FileDownloadService,
  ) {
    super(injector);
  }

  @Input() get selectedColumns(): any[] {
    return this._selectedColumns;
  }

  ResetData() {
    this.vatAmount = 0;
    this.totalAmount = 0;
    this.taxable = 0;
    this.customPaid = 0;
    this.custom = 0;
    this.totalamountPaid = 0;
    this.taxrate = 0;
    this.taxDue = 0;
  }

  ngOnInit(): void {
    this.tenantId = this._sessionService.tenantId;
    this.tenantName = this._sessionService.tenancyName;
    this.bsConfig = Object.assign({}, {
      minMode: this.minMode
    });
  }

  getSalesDetailedReport() {
    this.isdisable = true;
    this.ResetData();
    this._WHTReportServiceProxy.getPayemntReturn(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
      .pipe(finalize(() => this.isdisable = false))
      .subscribe((result) => {
        this.data = result;
        this.data.forEach(element => {
          this.taxDue += element.taxDue;
          this.taxrate += element.taxrate;
          this.totalamountPaid += element.totalamountPaid;
        });
      });
  }

  parseDate(dateString: string): DateTime {
    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());
    }
    return null;
  }

  addFooter(li: any[]): any[] {
    li.push({
      slno: 'Total',
      totalamountPaid: this.totalamountPaid,
      taxrate: this.taxrate,
      taxDue: this.taxDue
    });
    return li;
  }

  removeFooter(li: any[]): any[] {
    li.pop();
    return li;
  }

  downloadExcel() {
    this._WHTReportServiceProxy.getWhtReturnReportToExcel(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()), 'WHTReturnReport', this.tenantName, false).subscribe((result) => {
      this._fileDownloadService.downloadTempFile(result);
    });
  }
}

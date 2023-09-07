
import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VatReportDto, ReportServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
  templateUrl: './overheadsreconciliationreport.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
  styleUrls: ['./overheadsreconciliationreport.component.less'],
})
export class overheadsreconciliationreportComponent extends AppComponentBase {
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];

  tenantId: Number;
  tenantName: string;
  fromDate: Date;
  toDate: Date;
  data: VatReportDto[] = [new VatReportDto(), new VatReportDto(), new VatReportDto(), new VatReportDto(), new VatReportDto(),
  new VatReportDto(), new VatReportDto(), new VatReportDto(), new VatReportDto(), new VatReportDto(), new VatReportDto(),
  new VatReportDto(), new VatReportDto(), new VatReportDto(), new VatReportDto(), new VatReportDto()];
  vatAmount = 0;
  totalAmount = 0;
  netAmount = 0;
  searched = false;

  constructor(
    injector: Injector,
    private _VatReportServiceProxy: ReportServiceProxy,
    private _sessionService: AppSessionService,
    private _dateTimeService: DateTimeService,
    private _fileDownloadService: FileDownloadService,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.tenantId = this._sessionService.tenantId;
    this.tenantName = this._sessionService.tenancyName;
  }
  getSalesDetailedReport() {
    this._VatReportServiceProxy.getOverheadsReconciliationReport(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString())).subscribe((result) => {
      this.data = result;
      this.searched = true;
    });
  }
  downloadExcel() {
    this._VatReportServiceProxy.getOverheadsReconciliationReportToExcel(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()), 'OverheadsReconciliationReport', this.tenantName, false).subscribe((result) => {
      this._fileDownloadService.downloadTempFile(result);
    });
  }
  parseDate(dateString: string): DateTime {
    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());
    }
    return null;
  }
}

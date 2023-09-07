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
    templateUrl: './overridereport.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
    styleUrls: ['./overridereport.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => OverrideReportComponent), // replace name as appropriate
            multi: true,
        },
    ],
})
export class OverrideReportComponent extends AppComponentBase {
    public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
    tenantId: Number;
    tenantName: string;
    fromDate: Date;
    toDate: Date;
    invoices: any[] = [];
    selectedInvoices: any[] = [];
    code = '';
    isdisable = false;
    reportType: any;

    columns: any[] = [
        { field: 'number', header: 'Number' },
        { field: 'batchNo', header: 'Batch No' },
        { field: 'date', header: 'Date' },
        { field: 'name', header: 'Name' },
        { field: 'errorMessage', header: 'Error Message' },
        { field: 'overriderName', header: 'Overrider Name' },
        { field: 'overrideDate', header: 'Override Date' },
        { field: 'natureofOverride', header: 'Nature of Override' },
    ];
    exportColumns: any[] = this.columns.map((col) => ({ title: col.header, dataKey: col.field }));
    _selectedColumns: any[] = this.columns;
    constructor(
        injector: Injector,
        private _ReportServiceProxy: ReportServiceProxy,
        private _sessionService: AppSessionService,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }
    @Input() get selectedColumns(): any[] {
        return this._selectedColumns;
    }
    set selectedColumns(val: any[]) {
        this._selectedColumns = this.columns.filter((col) => val.includes(col));
    }
    ngOnInit(): void {
        this.tenantId = this._sessionService.tenantId;
        this.tenantName = this._sessionService.tenancyName;
        this.code = 'VATSAL000';
        this.getReportType();
    }
    getSalesDetailedReport() {
        this.isdisable = true;
        this._ReportServiceProxy
            .getOverrideReport(
                this.parseDate(this.dateRange[0].toString()),
                this.parseDate(this.dateRange[1].toString()),
                this.code
            )
            .pipe(finalize(() => (this.isdisable = false)))
            .subscribe((result) => {
                this.invoices = result;
            });
    }
    parseDate(dateString: string): DateTime {
        if (dateString) {
            return DateTime.fromISO(new Date(dateString).toISOString());
        }
        return null;
    }
    downloadExcel(li: any[]) {
        this._ReportServiceProxy
            .getOverrideReportToExcel(
                this.parseDate(this.dateRange[0].toString()),
                this.parseDate(this.dateRange[1].toString()),
                this.code,
                'OverrideOptionExercisedReport',
                this.tenantName,
                false
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
    getReportType() {
        this._ReportServiceProxy.getReportType('OVR').subscribe((result) => {
          this.reportType = result;
        });
      }
}

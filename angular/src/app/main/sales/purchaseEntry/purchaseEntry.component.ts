// @ts-nocheck

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    SalesInvoiceServiceProxy,
    GetInvoiceDto,
    PurchaseEntriesServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTime } from 'luxon';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TableModule } from 'primeng/table';
@Component({
    templateUrl: './purchaseEntry.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class PurchaseEntryComponent extends AppComponentBase {
    public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
    invoices: ReportDto[] = [];
    tenantId: Number;
    tenantName: String;
    columns: any[] = [
        { field: 'invoiceId', header: 'Purchase Id' },
        { field: 'invoiceDate', header: 'Purchase Date' },
        { field: 'customerName', header: 'Customer Name' },
        { field: 'contactNo', header: 'Contact No' },
        { field: 'amount', header: 'Amount' },
    ];
    exportColumns: any[] = this.columns.map((col) => ({ title: col.header, dataKey: col.field }));
    _selectedColumns: any[] = this.columns;
    constructor(
        injector: Injector,
        private _invoiceServiceProxy: PurchaseEntriesServiceProxy,
        private _sessionService: AppSessionService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    @Input() get selectedColumns(): any[] {
        return this._selectedColumns;
    }
    set selectedColumns(val: any[]) {
        //restore original order
        this._selectedColumns = this.columns.filter((col) => val.includes(col));
    }
    // eslint-disable-next-line @angular-eslint/use-lifecycle-interface
    ngOnInit(): void {
        this.tenantId = this._sessionService.tenantId == null ? 0 : this._sessionService.tenantId;
        this.tenantName = this._sessionService.tenancyName;
        this.getSalesData();
    }
    getSalesData() {
        this._invoiceServiceProxy
            .getPurchaseData(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
            .subscribe((result) => {
                console.log(result);
                this.invoices = result;
            });
    }
    parseDate(dateString: string): DateTime {
        if (dateString) {
            return DateTime.fromISO(new Date(dateString).toISOString());
        }
        return null;
    }
    //format date
    formatDate(date) {
        let d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2) {
            month = '0' + month;
        }

        if (day.length < 2) {
            day = '0' + day;
        }

        return [year, month, day].join('-');
    }
}

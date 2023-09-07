// @ts-nocheck

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    SalesInvoicesServiceProxy,
    GetInvoiceDto,
    CreditNoteServiceProxy,
    DebitNotesServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTime } from 'luxon';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { Location } from '@angular/common';
import { ReportGridColumns } from '@app/shared/vita-components/report-grid/report-grid';

@Component({
    selector: 'transactions',
    templateUrl: './transactions.component.html',
    styleUrls: ['./transactions.component.css'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class TransactionsComponent extends AppComponentBase {
    @Input() hideHeader: boolean = false;
    public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
    invoices: ReportDto[] = [];
    tenantId: Number;
    tenantName: String;
    pdfUrl = AppConsts.pdfUrl + '/InvoiceFiles';
    tab: string = 'Sales Invoice';
    theme: string = 'sales';
    createUrl = '/app/main/sales/createSalesInvoice';
    creditColumns: ReportGridColumns[] = [
        { field: 'invoiceId', header: 'Credit Note Number' },
        { field: 'invoiceDate', header: 'Credit Note Date' },
        { field: 'referenceNumber', header: 'Invoice Number' },
        { field: 'customerName', header: 'Customer Name' },
        { field: 'contactNo', header: 'Contact Number' },
        {
            field: 'amount',
            header: 'Amount (SAR)',
            align: 'right',
            sortable: true,
            type: 'Decimal',
        },
        {
            field: 'download',
            header: 'Download Invoice',
            type: 'Download',
        },
    ];
    debitColumns: ReportGridColumns[] = [
        { field: 'invoiceId', header: 'Debit Note Number' },
        { field: 'invoiceDate', header: 'Debit Note Date' },
        { field: 'referenceNumber', header: 'Invoice Number' },
        { field: 'customerName', header: 'Customer Name' },
        { field: 'contactNo', header: 'Contact Number' },
        {
            field: 'amount',
            header: 'Amount (SAR)',
            align: 'right',
            sortable: true,
            type: 'Decimal',
        },
        {
            field: 'download',
            header: 'Download Invoice',
            type: 'Download',
        },
    ];
    salesColumns: ReportGridColumns[] = [
        { field: 'invoiceId', header: 'Invoice Number' },
        { field: 'invoiceDate', header: 'Invoice Date' },
        { field: 'customerName', header: 'Customer Name' },
        { field: 'contactNo', header: 'Contact Number' },
        {
            field: 'amount',
            header: 'Amount (SAR)',
            align: 'right',
            sortable: true,
            type: 'Decimal',
        },
        {
            field: 'download',
            header: 'Download Invoice',
            type: 'Download',
        },
    ];
    columns: any[] = this.salesColumns;

    constructor(
        injector: Injector,

        private _sessionService: AppSessionService,
        private _salesServiceProxy: SalesInvoicesServiceProxy,
        private _creditServiceProxy: CreditNoteServiceProxy,
        private _debitServiceProxy: DebitNotesServiceProxy,
        private _dateTimeService: DateTimeService,
        private location: Location
    ) {
        super(injector);
    }

    //ngoninit

    ngOnInit(): void {
        this.tenantId = this._sessionService.tenantId == null ? 0 : this._sessionService.tenantId;
        this.tenantName = this._sessionService.tenancyName;
        this.getSalesData();
        this.tab = this.location.getState().tabvaule != undefined ? this.location.getState().tabvaule : 'Sales Invoice';
    }

    addPdfUrl() {
        this.invoices.forEach((e) => {
            e['download'] =
                this.pdfUrl +
                '/' +
                this.tenantId +
                '/' +
                e.uniqueIdentifier +
                '/' +
                e.uniqueIdentifier +
                '_' +
                e.invoiceId +
                '.pdf';
        });
    }

    getSalesData() {
        this._salesServiceProxy
            .getSalesData(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
            .subscribe((result) => {
                console.log(result);
                this.invoices = result;
                this.addPdfUrl();
            });
    }

    getCreditData() {
        this._creditServiceProxy
            .getCreditData(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
            .subscribe((result) => {
                console.log(result);
                this.invoices = result;
                this.addPdfUrl();
            });
    }

    getDebitData() {
        this._debitServiceProxy
            .getDebitData(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
            .subscribe((result) => {
                console.log(result);
                this.invoices = result;
                this.addPdfUrl();
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
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('-');
    }

    changeTab(tab: string) {
        this.tab = tab;
        if (tab == 'Sales Invoice') {
            this.getSalesData();
            this.columns = this.salesColumns;
            this.createUrl = '/app/main/sales/createSalesInvoice';
            this.theme = 'sales';
        } else if (tab == 'Credit Note') {
            this.getCreditData();
            this.columns = this.creditColumns;
            this.createUrl = '/app/main/sales/createCreditNote';
            this.theme = 'debit';
        } else {
            this.getDebitData();
            this.columns = this.debitColumns;
            this.createUrl = '/app/main/sales/createDebitNote';
            this.theme = 'credit';
        }
    }

    searchData() {
        this.changeTab(this.tab);
    }
}

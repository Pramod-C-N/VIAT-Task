// @ts-nocheck

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    SalesInvoicesServiceProxy,
    GetInvoiceDto,
    CreditNoteServiceProxy,
    DebitNotesServiceProxy,
    PurchaseEntriesServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTime } from 'luxon';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { Location } from '@angular/common';

@Component({
    selector: 'transactions',
    templateUrl: './purchaseTransaction.component.html',
    styleUrls: ['./purchaseTransaction.component.css'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class PurchaseTransactionComponent extends AppComponentBase {

    @Input() hideHeader = false;
    public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
    invoices: ReportDto[] = [];
    tenantId: Number;
    tenantName: String;
    pdfUrl = AppConsts.pdfUrl + '/InvoiceFiles';
    tab = 'Sales Invoice';
    createUrl = '/app/main/sales/createPurchaseEntry';
    creditColumns: any[] = [
        { field: 'invoiceId', header: 'Credit Note Number' },
        { field: 'invoiceDate', header: 'Credit Note Date' },
        { field: 'customerName', header: 'Customer Name' },
        { field: 'contactNo', header: 'Contact Number' },
        { field: 'amount', header: 'Amount (SAR)' },
    ];
    debitColumns: any[] = [
        { field: 'invoiceId', header: 'InvoiceId' },
        { field: 'invoiceDate', header: 'Invoice Date' },
        { field: 'referenceNumber', header: 'Reference Number' },
        { field: 'customerName', header: 'Customer Name' },
        { field: 'contactNo', header: 'Contact No' },
        { field: 'amount', header: 'Amount' },
    ];
    salesColumns: any[] = [
        { field: 'invoiceId', header: 'Purchase Id' },
        { field: 'invoiceDate', header: 'Purchase Date' },
        { field: 'customerName', header: 'Customer Name' },
        { field: 'contactNo', header: 'Contact No' },
        { field: 'amount', header: 'Amount' },
    ];
    columns: any[] = this.salesColumns;

    constructor(
        injector: Injector,
        private _invoiceServiceProxy: PurchaseEntriesServiceProxy,
        private _debitNoteProxy: DebitNotesServiceProxy,
        private _sessionService: AppSessionService,
        private _salesServiceProxy: SalesInvoicesServiceProxy,
        private _creditServiceProxy: CreditNoteServiceProxy,
        private _debitServiceProxy: DebitNotesServiceProxy,
        private _dateTimeService: DateTimeService,
        private location: Location
    ) {
        super(injector);
    }
    // eslint-disable-next-line @angular-eslint/use-lifecycle-interface
    ngOnInit(): void {
        this.tenantId = this._sessionService.tenantId == null ? 0 : this._sessionService.tenantId;
        this.tenantName = this._sessionService.tenancyName;
        this.getSalesData();
        this.tab = this.location.getState().tabvaule !== undefined ? this.location.getState().tabvaule : 'Sales Invoice';
    }

    getSalesData() {
        this._invoiceServiceProxy
            .getPurchaseData(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
            .subscribe((result) => {
                this.invoices = result;
            });
    }

    getDebitData() {
        this._debitNoteProxy.getDebitData(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString())).subscribe((result) => {
            this.invoices = result;
        });
    }

    getCreditData() {
        this._creditServiceProxy
            .getCreditData(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
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

    changeTab(tab: string) {
        this.tab = tab;
        if (tab === 'Sales Invoice') {
            this.getSalesData();
            this.columns = this.salesColumns;
            this.createUrl = '/app/main/sales/createPurchaseEntry';
        } else if (tab === 'Credit Note') {
            this.getCreditData();
            this.columns = this.creditColumns;
            this.createUrl = '/app/main/sales/createCreditNote';
        } else {
            this.getDebitData();
            this.columns = this.debitColumns;
            this.createUrl = '/app/main/sales/createDebitNote';
        }
    }

    searchData() {
        this.changeTab(this.tab);
    }
}

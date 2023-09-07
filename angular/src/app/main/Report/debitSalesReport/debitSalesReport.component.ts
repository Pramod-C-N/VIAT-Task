// @ts-nocheck

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    SalesInvoicesServiceProxy,
    GetInvoiceDto,
    CreditNoteServiceProxy,
    DebitNotesServiceProxy,
    ReportServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTime } from 'luxon';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { finalize } from 'rxjs/operators';
import { FileDownloadService } from '@shared/utils/file-download.service';


@Component({
    templateUrl: './debitSalesReport.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
    styleUrls: ['./debitSalesReport.component.css']
})
export class DebitSalesReportComponent extends AppComponentBase {
    public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
    invoices: ReportDto[] = [];
    tenantId: Number;
    code: string;
    tenantName: String;
    pdfUrl = AppConsts.pdfUrl + '/InvoiceFiles';
    tab = 'Detailed';
    checkboxValue: any;


    daywiseColumns: any[] = [
        { field: 'invoiceDate', header: 'Debit Note Date' },
        { field: 'invoicenumber', header: 'Debit Note Count' },
        { field: 'taxableAmount', header: 'Taxable Amount' },
        { field: 'vatAmount', header: 'VAT Amount' },
        { field: 'zeroRated', header: 'Zero Rated' },
        { field: 'exempt', header: 'Exempt' },
        { field: 'exports', header: 'Exports' },
        { field: 'outofScope', header: 'Out of Scope' },
        { field: 'govtTaxableAmt', header: 'Govt Taxable Amount' },
        { field: 'totalAmount', header: 'Total Amount' },
    ];

    detailedColumns: any[] = [
        { field: 'irnNo', header: 'Debit Note Number' },
        { field: 'customerName', header: 'Customer Name' },
        { field: 'invoiceDate', header: 'Debit Note Date' },
        { field: 'invoicenumber', header: 'Invoice Number' },
        { field: 'taxableAmount', header: 'Taxable Amount' },
        { field: 'vatRate', header: 'VAT Rate' },
        { field: 'vatAmount', header: 'VAT Amount' },
        { field: 'totalAmount', header: 'Total Amount' },        
        { field: 'zeroRated', header: 'Zero Rated' },
        { field: 'exempt', header: 'Exempt' },
        { field: 'exports', header: 'Exports' },
        { field: 'outofScope', header: 'Out of Scope' },
        { field: 'govtTaxableAmt', header: 'Govt Taxable Amount' },
    ];
    isdisable = false;

    daywiseFooter: any = {
        vatAmount: 0,
        totalAmount: 0,
        count: 0,
        taxable: 0,
        zeroRated: 0,
        health: 0,
        Gov: 0,
        export: 0,
        exempt: 0,
        OutofScope: 0,
    };

    detailedFooter: any = {
        vatAmount: 0,
        totalAmount: 0,
        taxableAmount: 0,
        zeroRated: 0,
        health: 0,
        export: 0,
        exempt: 0,
        Gov: 0,
        OutofScope: 0,
        vatrate: 0,
        customerName: ''
    };

    columns: any[] = this.detailedColumns;

    constructor(
        injector: Injector,

        private _sessionService: AppSessionService,
        private _reportService: ReportServiceProxy,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService

    ) {
        super(injector);
    }

    //ngoninit

    ngOnInit(): void {
        this.code = 'DNSAL000';
        this.tenantId = this._sessionService.tenantId == null ? 0 : this._sessionService.tenantId;
        this.tenantName = this._sessionService.tenant.name;
       // this.getDebitDetailedReport();
    }

    ResetData() {
        this.daywiseFooter = {
            vatAmount: 0,
            totalAmount: 0,
            count: 0,
            taxable: 0,
            zeroRated: 0,
            health: 0,
            Gov: 0,
            export: 0,
            exempt: 0,
            OutofScope: 0,
        };

        this.detailedFooter = {
            vatAmount: 0,
            totalAmount: 0,
            taxableAmount: 0,
            zeroRated: 0,
            health: 0,
            export: 0,
            exempt: 0,
            Gov: 0,
            OutofScope: 0,
            vatrate: 0,
            customerName: ''
        };
    }

    getDebitSalesDaywiseReport(handleDisable: boolean = false) {

        if (handleDisable) {
this.isdisable = true;
}
        this._reportService
            .getDebitSalesDaywiseReport(
                this.parseDate(this.dateRange[0].toString()),
                this.parseDate(this.dateRange[1].toString())
            )
            .pipe(finalize(() => (this.isdisable = false)))
            .subscribe((result) => {
                console.log(result);
                this.invoices = result;
                this.ResetData();
                this.invoices.forEach((element) => {
                    this.daywiseFooter.vatAmount += element.vatAmount;
                    this.daywiseFooter.totalAmount += element.totalAmount;
                    this.daywiseFooter.taxable += element.taxableAmount;
                    this.daywiseFooter.zeroRated += element.zeroRated;
                    this.daywiseFooter.health += element.healthCare;
                    this.daywiseFooter.exempt += element.exempt;
                    this.daywiseFooter.export += element.exports;
                    this.daywiseFooter.OutofScope += element.outofScope;
                    this.daywiseFooter.Gov += element.govtTaxableAmt;
                    this.daywiseFooter.count += parseInt(element.invoicenumber);
                });
            });
    }

    getDebitDetailedReport(handleDisable: boolean = false) {

        if (handleDisable) {
this.isdisable = true;
}
        this._reportService
            .getDebitNotePeriodicalReport(
                this.parseDate(this.dateRange[0].toString()),
                this.parseDate(this.dateRange[1].toString()),
                this.code
            )
            .pipe(finalize(() => (this.isdisable = false)))
            .subscribe((result) => {
                console.log(result);
                this.invoices = result;
                this.ResetData();
                this.invoices.forEach((element) => {
                    this.detailedFooter.vatAmount += element.vatAmount;
                    this.detailedFooter.export += element.exports;
                    this.detailedFooter.taxableAmount += element.taxableAmount;
                    this.detailedFooter.totalAmount += element.totalAmount;
                    this.detailedFooter.exempt += element.exempt;
                    this.detailedFooter.zeroRated += element.zeroRated;
                    this.detailedFooter.Gov += element.govtTaxableAmt;
                    this.detailedFooter.OutofScope += element.outofScope;
                    this.detailedFooter.vatrate += element.vatrate;
                    this.detailedFooter.customerName += element.customerName;
                });
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


    changeEvent(event: any) {
        if (this.tab == 'Detailed') {
            this.changeTab('Daywise');
            this.tab = 'Daywise';
        } else {
            this.changeTab('Detailed');
            this.tab = 'Detailed';
        }
          }
    changeTab(tab: string) {
        this.tab = tab;
        if (tab == 'Daywise') {
            this.getDebitSalesDaywiseReport();
            this.columns = this.daywiseColumns;
        } else if (tab == 'Detailed') {
            this.getDebitDetailedReport();
            this.columns = this.detailedColumns;
        }
    }

    downloadExcel() {
        if (this.tab == 'Daywise') {
            this._reportService
                .getDebitDaywiseToExcel(
                    this.parseDate(this.dateRange[0].toString()),
                    this.parseDate(this.dateRange[1].toString()),
                    this.code,
                    'DebitDaywiseReport',
                    this.tenantName,
                    false
                )
                .subscribe((result) => {
                    this._fileDownloadService.downloadTempFile(result);
                });
        } else if (this.tab == 'Detailed') {
            this._reportService
                .getDebitDetailedToExcel(
                    this.parseDate(this.dateRange[0].toString()),
                    this.parseDate(this.dateRange[1].toString()),
                    this.code,
                    'DebitDetailedReport',
                    this.tenantName,
                    false
                )
                .subscribe((result) => {
                    this._fileDownloadService.downloadTempFile(result);
                });
        }
    }
    searchData(){
        this.changeTab(this.tab);
    }
}

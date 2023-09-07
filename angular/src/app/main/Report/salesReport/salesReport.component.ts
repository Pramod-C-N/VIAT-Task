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
    templateUrl: './salesReport.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
    styleUrls: ['./salesReport.component.css'],
})
export class SalesReportComponent extends AppComponentBase {
    public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
    invoices: ReportDto[] = [];
    code: string;
    tenantId: Number;
    tenantName: String;
    checkboxValue: any;
    reportType: any;
    pdfUrl = AppConsts.pdfUrl + '/InvoiceFiles';
    tab = 'Detailed';
    daywiseColumns: any[] = [
        { field: 'invoiceDate', header: 'Invoice Date' },
        { field: 'invoicenumber', header: 'Invoice Count' },
        { field: 'taxableAmount', header: 'Taxable Amount' },
        { field: 'zeroRated', header: 'Zero Rated' },
        { field: 'exempt', header: 'Exempt' },
        { field: 'govtTaxableAmt', header: 'Govt Taxable Amount' },
        { field: 'exports', header: 'Exports' },
        { field: 'outofScope', header: 'Out of Scope' },
        { field: 'vatAmount', header: 'VAT Amount' },
        { field: 'totalAmount', header: 'Total Amount' },
    ];

    detailedColumns: any[] = [
        { field: 'invoiceDate', header: 'Invoice Date' },
        { field: 'invoicenumber', header: 'Invoice Number' },
        { field: 'customerName', header: 'Customer Name' },
        { field: 'taxableAmount', header: 'Taxable Amount' },
        { field: 'govtTaxableAmt', header: 'Govt Taxable Amount' },
        { field: 'zeroRated', header: 'Zero Rated' },
        { field: 'exempt', header: 'Exempt' },
        { field: 'exports', header: 'Exports' },
        { field: 'outofScope', header: 'Out of Scope' },
        { field: 'vatrate', header: 'VAT Rate' },
        { field: 'vatAmount', header: 'VAT Amount' },
        { field: 'totalAmount', header: 'Total Amount' },
    ];
    detailedDescriptionColumns: any[] = [
        { field: 'invoiceDate', header: 'Invoice Date' },
        // { field: 'irnNo', header: 'IRN Number' }, 
        { field: 'description', header: 'Description' },
        { field: 'invoicenumber', header: 'Invoice Number' },
        { field: 'taxableAmount', header: 'Taxable Amount' },
        { field: 'govtTaxableAmt', header: 'Govt Taxable Amount' },
        { field: 'zeroRated', header: 'Zero Rated' },
        { field: 'exempt', header: 'Exempt' },
        { field: 'exports', header: 'Exports' },
        { field: 'outofScope', header: 'Out of Scope' },
        { field: 'vatrate', header: 'VAT Rate' },
        { field: 'vatAmount', header: 'VAT Amount' },
        { field: 'totalAmount', header: 'Total Amount' },
    ];

    isdisable = false;

    daywiseFooter: any = {
      vatAmount: 0,
      totalAmount: 0,
      count : 0,
      taxable : 0,
      zeroRated  : 0,
      health : 0,
      export : 0,
      exempt  : 0,
      Gov: 0,
      OutofScope: 0,
    };

    detailedFooter: any = {
      vatAmount:  0,
      totalAmount:  0,
      taxableAmount:  0,
      zeroRated:  0,
      health:  0,
      export:  0,
      exempt:  0,
      Gov: 0,
      OutofScope: 0,
      vatrate: 0
    };

    columns: any[] = this.detailedColumns;

    constructor(
        injector: Injector,

        private _sessionService: AppSessionService,
        private _reportService: ReportServiceProxy,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService,

    ) {
        super(injector);
    }

    //ngoninit

    ngOnInit(): void {
        this.code = 'VATSAL000';
        this.getReportType();
        this.tenantId = this._sessionService.tenantId == null ? 0 : this._sessionService.tenantId;
        this.tenantName = this._sessionService.tenant.name;
       // this.getSalesDetailedReport();
    }

    ResetData(){
      this.daywiseFooter = {
        vatAmount: 0,
        totalAmount: 0,
        count : 0,
        taxable : 0,
        zeroRated  : 0,
        health : 0,
        export : 0,
        exempt  : 0,
        Gov: 0,
        OutofScope: 0,
      };

      this.detailedFooter = {
        vatAmount:  0,
        totalAmount:  0,
        taxableAmount:  0,
        zeroRated:  0,
        health:  0,
        export:  0,
        exempt:  0,
        Gov: 0,
        OutofScope: 0,
        vatrate: 0
      };
    }

    getSalesDaywiseReport(handleDisable: boolean= false) {

        if (handleDisable) {
this.isdisable = true;
}
        this._reportService
            .getSalesDayWiseReport(
                this.parseDate(this.dateRange[0].toString()),
                this.parseDate(this.dateRange[1].toString())
            )
            .pipe(finalize(() => (this.isdisable = false)))
            .subscribe((result) => {
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
                    this.daywiseFooter.Gov += element.govtTaxableAmt;
                    this.daywiseFooter.OutofScope += element.outofScope;
                    this.daywiseFooter.count += parseInt(element.invoicenumber);


                });
            });
    }

    getSalesDetailedReport(handleDisable: boolean= false) {
      if (handleDisable) {
this.isdisable = true;
}
      this._reportService.getSalesDetailedReport(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()), this.code)
      .pipe(finalize(() => this.isdisable = false))
      .subscribe((result) => {
        this.invoices = result;
        this.ResetData();
        this.invoices.forEach(element => {
          this.detailedFooter.vatAmount += element.vatAmount;
          this.detailedFooter.export +=  element.exports;
          this.detailedFooter.taxableAmount +=  element.taxableAmount;
          this.detailedFooter.totalAmount +=  element.totalAmount;
          this.detailedFooter.exempt +=  element.exempt;
          this.detailedFooter.zeroRated += element.zeroRated;
          this.detailedFooter.Gov += element.govtTaxableAmt;
          this.detailedFooter.OutofScope += element.outofScope;
          this.detailedFooter.vatrate += element.vatrate;
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
        this.ResetData();
        if (tab === 'Daywise') {
            this.getSalesDaywiseReport();
            this.columns = this.daywiseColumns;
        } else if (tab === 'Detailed') {
            if (this.code === 'VATSAL000'){
                this.columns = this.detailedColumns;
            } else{
                this.columns = this.detailedDescriptionColumns;
            }
            this.getSalesDetailedReport();
        }
    }
    getReportType() {
        this._reportService.getReportType('SAL').subscribe((result) => {
          this.reportType = result;
        });
      }

    downloadExcel() {
      if (this.tab == 'Daywise'){
        this._reportService
            .getSalesDaywiseToExcel(
                this.parseDate(this.dateRange[0].toString()),
                this.parseDate(this.dateRange[1].toString()),
                'SalesDaywiseReport',
                this.tenantName,
                false
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
          } else if (this.tab == 'Detailed'){
            this._reportService
            .getSalesDetailedToExcel(
                this.parseDate(this.dateRange[0].toString()),
                this.parseDate(this.dateRange[1].toString()),
                this.code,
                'SalesDetailedReport',
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

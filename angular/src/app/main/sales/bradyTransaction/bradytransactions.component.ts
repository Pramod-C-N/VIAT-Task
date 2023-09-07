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
    selector: 'bradytransactions',
    templateUrl: './bradytransactions.component.html',
    styleUrls: ['./bradytransactions.component.css'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class BradyTransactionsComponent extends AppComponentBase {
    date = new Date();
    month =
        (this.date.getMonth() + 1).toString().length > 1 ? this.date.getMonth() + 1 : '0' + (this.date.getMonth() + 1);
    day = this.date.getDate().toString().length > 1 ? this.date.getDate() : '0' + this.date.getDate();
    maxDate = this.date.getFullYear() + '-' + this.month + '-' + this.day;
    @ViewChild('modalterms', { static: false }) modal: ModalDirective;
    @Input() hideHeader: boolean = false;
    public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
    invoices: ReportDto[] = [];
    tenantId: Number;
    tenantName: String;
    pdfUrl = AppConsts.pdfUrl + '/InvoiceFiles';
    tab: string = 'Sales Invoice';
    theme: string = 'sales';
    createUrl = '/app/main/sales/createSalesInvoice';
    zatca_status: any[] = [];
    status= false;
    shippedToCode: String;
    buyerCode: String;
    ReferenceNumber: String;
    purchaseOrderNo: String;
    salesOrderNo: String;
    customerName: String;
    creationDate: String;
    em: any;
    wm: string;
    bradycreditColumns: ReportGridColumns[] = [
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
        { field: 'senttozatca', header: 'Sent to ZATCA' },
        { field: 'zatcastatus', header: 'ZATCA Status' },
        {
            field: 'download',
            header: 'Download Invoice',
            type: 'Download',
        },
    ];
    bradydebitColumns: ReportGridColumns[] = [
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
        { field: 'senttozatca', header: 'Sent to ZATCA' },
        { field: 'zatcastatus', header: 'ZATCA Status' },
        {
            field: 'download',
            header: 'Download Invoice',
            type: 'Download',
        },
    ];

    bradysalesColumns: ReportGridColumns[] = [
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
        { field: 'senttozatca', header: 'Sent to ZATCA' },
        { field: 'zatcastatus', header: 'ZATCA Status' },
        {
            field: 'download',
            header: 'Download Invoice',
            type: 'Download',
        },
    ];
    columns: any[] = this.bradysalesColumns;
    advancedFiltersAreShown = false;
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

    // eslint-disable-next-line @angular-eslint/use-lifecycle-interface
    ngOnInit(): void {
        this.tenantId = this._sessionService.tenantId == null ? 0 : this._sessionService.tenantId;
        this.tenantName = this._sessionService.tenancyName;
        this.getSalesData();
        this.tab = this.location.getState().tabvaule != undefined ? this.location.getState().tabvaule : 'Sales Invoice';
        this.changeTab(this.tab);
    }

    addPdfUrl() {
        // if (tab === 'Sales Invoice') {
            this.invoices.forEach((e) => {
                e['download'] = e.pdfurl;
            });
        // }
        // else{
        // this.invoices.forEach((e) => {
        //     e['download'] =
        //         this.pdfUrl +
        //         '/' +
        //         this.tenantId +
        //         '/' +
        //         e.uniqueIdentifier +
        //         '/' +
        //         e.uniqueIdentifier +
        //         '_' +
        //         e.invoiceId +
        //         '.pdf';
        // });
    // }
    }

    getSalesData() {
        this._salesServiceProxy
            .getSalesData(this.parseDate(this.dateRange[0].toString()), 
            this.parseDate(this.dateRange[1].toString()),
            this.creationDate,this.customerName,
            this.salesOrderNo,this.purchaseOrderNo,
            this.ReferenceNumber,this.buyerCode,this.shippedToCode)
            .subscribe((result) => {
                this.invoices = result;
                this.zatca_status = [];
                this.invoices.forEach((e) => {
                    this.zatca_status.push(e['zatcastatus']);
                    if (e?.zatcastatus?.length > 0) {
                        if (e['amount'] > 1000) {
                            e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.clearanceStatus;
                            if(e['zatcastatus']==='NOT_CLEARED')
                            {
                                e['zatcastatus']='false';
                            }
                            else
                            {
                                e['zatcastatus']='true';
                            }
                        } else {
                            e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.reportingStatus;
                            if(e['zatcastatus']==='NOT_REPORTED')
                            {
                                e['zatcastatus']='false';
                            }
                            else
                            {
                                e['zatcastatus']='true';
                            }
                        }


                    }
                    else{
                        e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.complianceInvoiceResponse;
                        if(e['zatcastatus']==='NOT_REPORTED')
                        {
                            e['zatcastatus']='false';
                        }
                        else    
                        {
                            e['zatcastatus']='true';
                        }
                    }
                });
                this.addPdfUrl();
            });
    }

    getCreditData() {
        this._creditServiceProxy
            .getCreditData(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
            .subscribe((result) => {
                this.invoices = result;
                this.zatca_status = [];
                this.invoices.forEach((e) => {
                    this.zatca_status.push(e['zatcastatus']);
                    if (e?.zatcastatus?.length > 0) {
                        if (e['amount'] > 1000) {
                            e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.clearanceStatus;
                            if(e['zatcastatus']==='NOT_CLEARED')
                            {
                                e['zatcastatus']='false';
                            }
                            else
                            {
                                e['zatcastatus']='true';
                            }
                        } else {
                            e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.reportingStatus;
                            if(e['zatcastatus']==='NOT_REPORTED')
                            {
                                e['zatcastatus']='false';
                            }
                            else
                            {
                                e['zatcastatus']='true';
                            }
                        }


                    }
                    else{
                        e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.complianceInvoiceResponse;
                        if(e['zatcastatus']==='NOT_REPORTED')
                        {
                            e['zatcastatus']='false';
                        }
                        else    
                        {
                            e['zatcastatus']='true';
                        }
                    }
                });
                this.addPdfUrl();
            });
    }

    getDebitData() {
        this._debitServiceProxy
            .getDebitData(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
            .subscribe((result) => {
                this.invoices = result;
                this.zatca_status = [];
                this.invoices.forEach((e) => {
                    this.zatca_status.push(e['zatcastatus']);
                    if (e?.zatcastatus?.length > 0) {
                        if (e['amount'] > 1000) {
                            e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.clearanceStatus;
                            if(e['zatcastatus']==='NOT_CLEARED')
                            {
                                e['zatcastatus']='false';
                            }
                            else
                            {
                                e['zatcastatus']='true';
                            }
                        } else {
                            e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.reportingStatus;
                            if(e['zatcastatus']==='NOT_REPORTED')
                            {
                                e['zatcastatus']='false';
                            }
                            else
                            {
                                e['zatcastatus']='true';
                            }
                        }


                    }
                    else{
                        e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.complianceInvoiceResponse;
                        if(e['zatcastatus']==='NOT_REPORTED')
                        {
                            e['zatcastatus']='false';
                        }
                        else    
                        {
                            e['zatcastatus']='true';
                        }
                    }
                });

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
            this.columns = this.bradysalesColumns;
            this.createUrl = '/app/main/sales/createSalesInvoiceBrady';
            this.theme = 'sales';
        } else if (tab == 'Credit Note') {
            this.getCreditData();
            this.columns = this.bradycreditColumns;
            this.createUrl = '/app/main/sales/createCreditNoteBrady';
            this.theme = 'debit';
        } else {
            this.getDebitData();
            this.columns = this.bradydebitColumns;
            this.createUrl = '/app/main/sales/createDebitNoteBrady';
            this.theme = 'credit';
        }
    }

    searchData() {
        
        this.changeTab(this.tab);
    }

    resolveUrl(event: any) {
        this._salesServiceProxy.generatePdf(event).subscribe((e) => {
            console.log(e);
        });
        console.log(event);
    }

    modaltermsshow(i: number) {

        this.wm = JSON.parse(this.zatca_status[i])?.validationResults.warningMessages[i];
        this.em = JSON.parse(this.zatca_status[i]);
        this.modal.show();
        

    }
}

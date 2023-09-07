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
import * as FileSaver from 'file-saver';
import * as ExcelJS from 'exceljs';
import { GlobalConstsCustomService } from '@shared/customService/global-consts-service';
import { ViewInvoiceComponent } from '../ViewInvoice/viewInvoice.component';

@Component({
    selector: 'transactions',
    templateUrl: './transactions.component.html',
    styleUrls: ['./transactions.component.css'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class TransactionsComponent extends AppComponentBase {
    date = new Date();
    month =
        (this.date.getMonth() + 1).toString().length > 1 ? this.date.getMonth() + 1 : '0' + (this.date.getMonth() + 1);
    day = this.date.getDate().toString().length > 1 ? this.date.getDate() : '0' + this.date.getDate();
    maxDate = this.date.getFullYear() + '-' + this.month + '-' + this.day;
    @ViewChild('modalterms', { static: false }) modal: ModalDirective;
    @ViewChild('modalView' , { static: false }) modalView: ModalDirective;

    @Input() hideHeader: boolean = false;
    public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
    invoices: ReportDto[] = [];
    globalFilter: string;
    tenantId: Number;
    tenantName: String;
    pdfUrl = AppConsts.pdfUrl + '/InvoiceFiles';
    tab: string = 'Sales Invoice';
    theme: string = 'sales';
    type: string;
    createUrl = '';
    viewInvoiceUrl= '';
    id:number;
    zatca_status: any[] = [];
    status= false;
    shippedToCode: String;
    IRNo:string;
    createdBy:string;
    buyerCode: String;
    ReferenceNumber: String;
    purchaseOrderNo: String;
    salesOrderNo: String;
    customerName: String;
    creationDate: String;
    loader:false;
    irnNo: string='0';
    em: any;
    wm: string;
    fields:string[][]=[];
    headers:string[]=[];
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
        { field: 'senttozatca', header: 'Sent to ZATCA' },
        { field: 'zatcastatus', header: 'ZATCA Status' },
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
        { field: 'senttozatca', header: 'Sent to ZATCA' },
        { field: 'zatcastatus', header: 'ZATCA Status' },
        {
            field: 'download',
            header: 'Download Invoice',
            type: 'Download',
        },
    ];
    // salesColumns: ReportGridColumns[] = [
    //     { field: 'invoiceId', header: 'Invoice Number' },
    //     { field: 'invoiceDate', header: 'Invoice Date' },
    //     { field: 'customerName', header: 'Customer Name' },
    //     { field: 'contactNo', header: 'Contact Number' },
    //     {
    //         field: 'amount',
    //         header: 'Amount (SAR)',
    //         align: 'right',
    //         sortable: true,
    //         type: 'Decimal',
    //     },
    //     { field: 'senttozatca', header: 'Sent to ZATCA' },
    //     { field: 'zatcastatus', header: 'ZATCA Status' },
    //     {
    //         field: 'download',
    //         header: 'Download Invoice',
    //         type: 'Download',
    //     },
    // ];
    columns: any[] = [];
    viewShow:boolean=false;
    advancedFiltersAreShown = false;
    exportColumns: string[] = [];
    searchableFields:any[]=[];
    viewtab:string='Sales';
    from:string;
    constructor(
        injector: Injector,
        private _sessionService: AppSessionService,
        private _salesServiceProxy: SalesInvoicesServiceProxy,
        private _creditServiceProxy: CreditNoteServiceProxy,
        private _debitServiceProxy: DebitNotesServiceProxy,
        private _dateTimeService: DateTimeService,
        private location: Location,
        private _globalConstsService: GlobalConstsCustomService
    ) {
        super(injector);
        this._globalConstsService.data$.subscribe((e) => {
            this.isVita = e.isVita;
        });

        this._globalConstsService.tenantType$.subscribe((e: string) => {
            this.isServices = e.includes('S');
            this.isGoods = e.includes('G');
        });
        
    }

    //ngoninit

    // eslint-disable-next-line @angular-eslint/use-lifecycle-interface
    ngOnInit(): void {
        this.from="UI";
        this.viewShow=false;
        this.tenantId = this._sessionService.tenantId == null ? 0 : this._sessionService.tenantId;
        this.tenantName = this._sessionService.tenancyName;
        this.viewInvoiceUrl='/app/main/sales/viewInvoice'
        this.createUrl = this.tenantName.toLowerCase()=='brady'?  '/app/main/sales/createSalesInvoiceBrady':'/app/main/sales/createSalesInvoice';
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
            this.ReferenceNumber,this.buyerCode,this.shippedToCode,this.IRNo,this.createdBy)
            .subscribe((result: any[]) => {
                this.invoices = result;
                console.log(result, 'wasd');
                this.zatca_status = [];
                if (this.invoices.length > 0) {
                  this.columns = Object.keys(this.invoices[0]).map(header => ({
                    field: header,
                    header: header.toUpperCase().replaceAll('_',' '),
                    sortable: true,
                  }));
                  this.columns = this.columns.filter((column) => column.header !== 'ADDITIONALDATA2');
                  console.log(this.columns, 'headers');
                  this.searchableFields=this.columns.map(val=>val.field)
                } else {
                  this.headers = [];
                  this.fields = [];
                }
              });
        this.addPdfUrl();
    }

    getCreditData() {
        this._creditServiceProxy
            .getCreditData(this.parseDate(this.dateRange[0].toString()), 
            this.parseDate(this.dateRange[1].toString()),
            this.creationDate,this.customerName,
            this.salesOrderNo,this.purchaseOrderNo,
            this.ReferenceNumber,this.buyerCode,this.shippedToCode,this.IRNo,this.createdBy)
            .subscribe((result) => {
                this.invoices = result;
                this.zatca_status = [];
                if (this.invoices.length > 0) {
                    this.columns = Object.keys(this.invoices[0]).map(header => ({
                      field: header,
                      header: header.toUpperCase().replaceAll('_',' '),
                      sortable: true,
                    }));
                    console.log(this.columns,'wrefds')
                    this.columns = this.columns.filter((column) => column.header !== 'ADDITIONALDATA2');
                    this.columns = this.columns.filter((column) => column.header !== 'INVOICE NUMBER');
                    console.log(this.columns, 'headers');
                    this.searchableFields=this.columns.map(val=>val.field)
                  } else {
                    this.headers = [];
                    this.fields = [];
                  }
                // this.invoices.forEach((e) => {
                //     this.zatca_status.push(e['zatcastatus']);
                //     if (e?.zatcastatus?.length > 0) {
                //         if (e['amount'] > 1000) {
                //             e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.clearanceStatus;
                //             if(e['zatcastatus']==='NOT_CLEARED')
                //             {
                //                 e['zatcastatus']='false';
                //             }
                //             else
                //             {
                //                 e['zatcastatus']='true';
                //             }
                //         } else {
                //             e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.reportingStatus;
                //             if(e['zatcastatus']==='NOT_REPORTED')
                //             {
                //                 e['zatcastatus']='false';
                //             }
                //             else
                //             {
                //                 e['zatcastatus']='true';
                //             }
                //         }


                //     }
                //     else{
                //         e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.complianceInvoiceResponse;
                //         if(e['zatcastatus']==='NOT_REPORTED')
                //         {
                //             e['zatcastatus']='false';
                //         }
                //         else    
                //         {
                //             e['zatcastatus']='true';
                //         }
                //     }
                // });
                this.addPdfUrl();
            });
    }

    getDebitData() {
        this._debitServiceProxy
            .getDebitData(this.parseDate(this.dateRange[0].toString()), 
            this.parseDate(this.dateRange[1].toString()),
            this.creationDate,this.customerName,
            this.salesOrderNo,this.purchaseOrderNo,
            this.ReferenceNumber,this.buyerCode,this.shippedToCode,this.IRNo,this.createdBy)
            .subscribe((result) => {
                this.invoices = result;
                this.zatca_status = [];
                if (this.invoices.length > 0) {
                    this.columns = Object.keys(this.invoices[0]).map(header => ({
                      field: header,
                      header: header.toUpperCase().replaceAll('_',' '),
                      sortable: true,
                    }));
                    this.columns = this.columns.filter((column) => column.header !== 'ADDITIONALDATA2');
                    this.columns = this.columns.filter((column) => column.header !== 'INVOICE NUMBER');
                    console.log(this.columns, 'headers');
                    this.searchableFields=this.columns.map(val=>val.field)
                  } else {
                    this.headers = [];
                    this.fields = [];
                  }
                // this.invoices.forEach((e) => {
                //     this.zatca_status.push(e['zatcastatus']);
                //     if (e?.zatcastatus?.length > 0) {
                //         if (e['amount'] > 1000) {
                //             e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.clearanceStatus;
                //             if(e['zatcastatus']==='NOT_CLEARED')
                //             {
                //                 e['zatcastatus']='false';
                //             }
                //             else
                //             {
                //                 e['zatcastatus']='true';
                //             }
                //         } else {
                //             e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.reportingStatus;
                //             if(e['zatcastatus']==='NOT_REPORTED')
                //             {
                //                 e['zatcastatus']='false';
                //             }
                //             else
                //             {
                //                 e['zatcastatus']='true';
                //             }
                //         }


                //     }
                //     else{
                //         e['zatcastatus'] = JSON.parse(e['zatcastatus'])?.complianceInvoiceResponse;
                //         if(e['zatcastatus']==='NOT_REPORTED')
                //         {
                //             e['zatcastatus']='false';
                //         }
                //         else    
                //         {
                //             e['zatcastatus']='true';
                //         }
                //     }
                // });

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
            // this.columns = this.salesColumns;
            this.createUrl = this.isServices ? ('/app/main/sales/createSalesInvoiceProfessional'):( this.tenantName.toLowerCase()=='brady'?  '/app/main/sales/createSalesInvoiceBrady':'/app/main/sales/createSalesInvoice');
            this.theme = 'sales';
        } else if (tab == 'Credit Note') {
            this.getCreditData();
            //this.columns = this.creditColumns;
            this.createUrl = this.isServices ? ('/app/main/sales/createCreditNoteProfessional'):( this.tenantName.toLowerCase()=='brady'?  '/app/main/sales/createCreditNoteBrady':'/app/main/sales/createCreditNote');
            this.theme = 'credit';
        } else {
            this.getDebitData();
            //this.columns = this.debitColumns;
            this.createUrl = this.isServices ? ('/app/main/sales/createDebitNoteProfessional'):( this.tenantName.toLowerCase()=='brady'?  '/app/main/sales/createDebitNoteBrady':'/app/main/sales/createDebitNote');
            this.theme = 'debit';
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

    
    modalViewShow(irnno: number,viewtype: string){
        this.id=irnno;
        this.viewShow=true;
        if(viewtype == 'isDraft'){
            this.type='Draft';
        }
        else{
            this.type=this.theme;
        }
        this.modalView.show();
    }

    clearData(): void {
        
        this.creationDate = '';
        this.customerName = '';
        this.salesOrderNo = '';
        this.purchaseOrderNo = '';
        this.ReferenceNumber = '';
        this.buyerCode = '';
        this.shippedToCode = '';
        this.IRNo='';
        this.createdBy='';
        this.getSalesData();
    }

    viewInvoice(id){
        this.router.navigate([this.viewInvoiceUrl,id]);

    }
    exportExcel() {
        const workbook = new ExcelJS.Workbook();
        const worksheet = workbook.addWorksheet('Data');
        // Add headers
        const fields = this.columns.filter(a => a.field != "pdf").map(a=>a.field)
        worksheet.addRow(this.columns.filter(a => a.header != "PDF").map(a=>a.header));

        // Add data
        this.invoices.forEach((item) => {
            const row: any = [];
            fields.forEach((col) => {
                console.log(col,'col')
                row.push(item[col]);
            });
            worksheet.addRow(row);
        });
        workbook.xlsx.writeBuffer().then((buffer: any) => {
            const blob = new Blob([buffer], {
                type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
            });
            FileSaver.saveAs(blob, `${'vita_' + Date.now()}.xlsx`);
        });
    }
    convert(irnno: string){
        this.irnNo=irnno;
        var title = '';
        if (this.tab === 'Sales Invoice') {
            title = 'Are You Sure To Convert This Draft To Sales Invoice?';
          } else if (this.tab === 'Credit Note') {
            title = 'Are You Sure To Convert This Draft To Credit Note Invoice?';
          } else {
            title = 'Are You Sure To Convert This Draft To Debit Note Invoice?';
          }
        Swal.fire({
            title,
            text: '',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Convert it!',
        }).then((result) => {
            if (result.isConfirmed) {
                this.loader=true;
                this._salesServiceProxy.createInvoiceFromDraft(irnno).subscribe( {
                next: (result) => {
                    this.loader=false;
                    this.notify.success(this.l('ConvertedSuccessfully'));
                    this.changeTab(this.tab);
                    this.loader=false;
                  },
                  error: (err) => {
                    this.loader=false;
                  },
                  complete: () => console.log('complete')
                });
            }
            else{
                this.loader=false
            }
        });
    }

    Modalhide(){
        this.modalView.hide();
        this.viewShow=false;
    }
}

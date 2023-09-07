// @ts-nocheck


import { Component, OnInit, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import {  SalesInvoicesServiceProxy, TenantDashboardServiceProxy } from '@shared/service-proxies/service-proxies';
import { DateTime } from 'luxon';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DashboardChartBase } from '../dashboard-chart-base';
import { WidgetComponentBaseComponent } from '../widget-component-base';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTimeCustomService } from '@shared/customService/date-time-service';
import { finalize } from 'rxjs';
import { AppConsts } from '@shared/AppConsts';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { Table } from 'primeng/table';
import { saveAs } from 'file-saver';
import * as FileSaver from 'file-saver';
import * as ExcelJS from 'exceljs';
import { error } from 'console';
import { futimesSync } from 'fs';
import { key } from 'localforage';
import { values } from 'lodash-es';


class DashboardTopStats extends DashboardChartBase {
    items: any = {};
    init(results) {
        this.items = results;
        this.hideLoading();
    }
}

@Component({
    selector: 'app-einvc-tab-stats',
    templateUrl: './widget-Einvc-tab.component.html',
    styleUrls: ['./widget-Einvc-tab.component.css'],
    encapsulation : ViewEncapsulation.None

})
export class WidgetEinvcComponent   implements OnInit {
   // public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
   @ViewChild(Table) private dataTable: Table;
   dashboardTopStats: DashboardTopStats;
    selectedDate:[DateTime, DateTime]=[DateTime.local().startOf('month'), DateTime.local().endOf('month')];
    loading: boolean = false;
    items: any = {};
    type:string='Sales';
    totalrecords:number=0;
    valid:number=0;
    invalid:number=0;
    zatcapp:number=0;
    zatcawar:number=0;
    zatcarej:number=0;
    zatcasub:number=0;
    loader:boolean=false;
    Totalrecords:boolean=true;
    em:any;
    wm:string;
    count:string;
    irnNo: string ='0';
    invoicereferencenumber: string;
    invoicereferncedate: string;
    purchaseOrderNo: string;
    customername: string;
    activestatus: string;
    currency: string;
    payernumber: string;
    salesordernumber: string;
    shiptonumber: string;
    IRNo:string;
    createdBy:string;
    viewShow:boolean=false;
    status= true;
    labels: any[] = [];
    labelcolor: any[] = [];
    reportname : string;
    messages:any[]=[];
    loader=false;
    modeltype:string;
    searchableFields:any[]=[];
    id:number;
    advancedFiltersAreShown = false;
    @ViewChild('modalterms' , { static: false }) modal: ModalDirective;
    @ViewChild('modalView' , { static: false }) modalView: ModalDirective;


     getData(selectedDate: any) {
    }
    
  
    loadTopStatsData(results) {
            this.dashboardTopStats.init(results);
    }
    tenantId: Number;
    invoices:any;
    invoicesparsed:any;
    colors:any[]=[];
    tenantName: String;
    pdfUrl = AppConsts.pdfUrl + '/InvoiceFiles';
    tab: string = 'Sales Invoice';
    createUrl = '/app/main/sales/createSalesInvoice';
    creditColumns: any[] = [
        { field: 'invoiceId', header: 'Credit Note Number' },
        { field: 'invoiceDate', header: 'Credit Note Date' },
        { field: 'referenceNumber', header: 'Invoice Number' },
        { field: 'customerName', header: 'Customer Name' },
        { field: 'validation', header: 'Validations' },
        { field: 'xml', header: 'XML Generation' },
        { field: 'invoicegen', header: 'Invoice Generation' },
        { field: 'ZATCAsub', header: 'ZATCA Submission' },
        { field: 'ZATCAapp', header: 'ZATCA Status' },
    ];
    debitColumns: any[] = [
        { field: 'invoiceId', header: 'Debit Note Number' },
        { field: 'invoiceDate', header: 'Debit Note Date' },
        { field: 'referenceNumber', header: 'Invoice Number' },
        { field: 'customerName', header: 'Customer Name' },
        { field: 'validation', header: 'Validations' },
        { field: 'xml', header: 'XML Generation' },
        { field: 'invoicegen', header: 'Invoice Generation' },
        { field: 'ZATCAsub', header: 'ZATCA Submission' },
        { field: 'ZATCAapp', header: 'ZATCA Status' },
   
    ];
    salesColumns: any[] = [
        { field: 'invoiceId', header: 'Invoice Id' },
        { field: 'invoiceDate', header: 'Invoice Date' },
        { field: 'customerName', header: 'Customer Name' },
        { field: 'validation', header: 'Validations' },
        { field: 'xml', header: 'XML Generation' },
        { field: 'invoicegen', header: 'Invoice Generation' },
        { field: 'ZATCAsub', header: 'ZATCA Submission' },
        { field: 'ZATCAapp', header: 'ZATCA Status' },
        // { field: 'ZATCAwar', header: 'ZATCA Approved with warning' },
           // { field: 'ZATCArej', header: 'ZATCA Rejected' },
        // { field: 'invcarch', header: 'Invoice Archived' }
    ];
    columns: any[] = [];
    errors: any[]=[]
    products!: any[];
    zatca_status:any[]=[];
    fields:string[][]=[];
    headers:string[]=[];
    viewtype:string;
    from:string;
    constructor(
        injector: Injector,
        private _salesInvoiceServiceProxy: SalesInvoicesServiceProxy,
        private _dateTimeService: DateTimeService,
        private _dateTimeCustomService:DateTimeCustomService,
        private _sessionService: AppSessionService,
        private modalService: BsModalService

    ) {
       // super(injector);
    }
    
    //ngoninit

    ngOnInit(): void {
        this.from="FileUpload"
        this.viewShow=false;
         this._dateTimeCustomService.data$.subscribe(e=>{
            if(e)
            this.selectedDate = e;
            this.getSalesData();
            this.tenantId =  this._sessionService.tenantId;
        })
    }


    resetdata(){
        this.invoicereferencenumber ='';
        this.invoicereferncedate='' ;
        this.purchaseOrderNo='' ;
        this.customername='' ;
        this.activestatus='' ;
        this.currency='' ;
        this.payernumber='' ;
        this.salesordernumber='' ;
        this.shiptonumber ='' ;
        this.IRNo='';
        this.createdBy='';
        this.getSalesData();
    }

    getSalesData() {        
        
        this.loader=true;
        this.resetfilter();
        this._salesInvoiceServiceProxy
            .getintegrationdashboarddataasJson(this.selectedDate[0],this.selectedDate[1],this.type,
                this.invoicereferencenumber,
                this.invoicereferncedate,
                this.purchaseOrderNo,
                this.customername,
                this.activestatus,
                this.currency,
                this.payernumber,
                this.salesordernumber,
                this.shiptonumber,
                this.IRNo,
                this.createdBy)
            .pipe(finalize(() => this.loader=false))            
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
                  this.columns = this.columns.filter((column) => column.header !== 'IRNNO');
                  
                  this.errors = this.columns.filter((column) => column.header == 'ERRORS');
                  console.log(this.invoices);
                  this.columns = this.columns.filter((column) => column.header !== 'ERRORS');
                  console.log(this.columns, 'headers');
                  this.searchableFields=this.columns.map(val=>val.field);
                } else {
                  this.headers = [];
                  this.fields = [];
                }
            });

    }

    getCreditData() {
        // this._creditServiceProxy
        //     .getCreditData(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
        //     .subscribe((result) => {
        //         this.invoices = result;
        //     });
    }

    getDebitData() {
        // this._debitServiceProxy
        //     .getDebitData(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
        //     .subscribe((result) => {
        //         this.invoices = result;
        //     });
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
            this.type='Sales'
            this.resetfilter();
            this.invoices=[]=[];
            this.getSalesData();
            // this.columns = this.salesColumns;
            this.createUrl = '/app/main/sales/createSalesInvoice';
        } else if (tab == 'Credit Note') {
            this.type='Credit'
            this.resetfilter();
            this.invoices=[]=[];
            this.getSalesData();
            this.columns = this.creditColumns;
            this.createUrl = '/app/main/sales/createCreditNote';
        } else {
            this.type='Debit'
            this.resetfilter();
            this.invoices=[]=[];
            this.getSalesData();
            this.columns = this.debitColumns;
            this.createUrl = '/app/main/sales/createDebitNote';
        }
    }

    searchData(){
        this.changeTab(this.tab)
    }

    Process()
    {
        this.resetdata();
        // this._salesInvoiceServiceProxy
        // .processdashboarddata(this.selectedDate[0],this.selectedDate[1],this.type)
        // .pipe(finalize(() => this.loader=false))            
        // .subscribe((result) => {
        //     this.invoices = result;
        //     for(var i=0;i<this.invoices.length;i++){
        //         var status = this.invoices[i].status
        //         this.invoices[i].validation=false

        //         if(status>1){
        //             this.invoices[i].validation=true
        //         } if(status>2){
        //             this.invoices[i].xml=true
        //         }
        //         if(status>3){
        //             this.invoices[i].invoicegen = true
        //             this.valid++;
        //         }
        //         if(this.invoices[i].validation==true)
        //         {
        //             this.invoices[i].ZATCAsub=true;

        //         if(this.invoices[i].ZATCAsub==false ){
        //             this.invoices[i].ZATCAapp =0
        //         }
        //         else
        //     {
        //         if(this.invoices[i].zatcAstatus==1 ){
        //             this.invoices[i].ZATCAapp =1
        //             this.zatcapp++;
        //         } 
        //         if(this.invoices[i].zatcAstatus==2 ){
        //             this.invoices[i].ZATCAapp = 2
        //             this.zatcawar++;
        //         }  
        //         if(this.invoices[i].zatcAstatus==3 ){
        //             this.invoices[i].ZATCAapp = 3
        //             this.zatcarej++;
        //         }   
        //     }     
        // }
                         
        //         this.totalrecords=this.invoices.length;
        //     }
            
        // });
    }


    modaltermsshow(i:number){
        this.em = this.invoices[i].errors 
        this.modal.show();
    }

    modalViewShow(irnno: number,viewtype: string){
        this.id=irnno;
        this.viewShow=true;
        if(viewtype == 'isDraft'){
            this.viewtype='Draft';
        }
        else{
            this.viewtype=this.type;
        }
        this.modalView.show();
    }

    exportToCsv(){
     
        const replacer = (key, value) => value === null ? '' : value; // specify how you want to handle null values here
        const header = this.labels.filter(a=>a!='download');

        let csv = this.invoices.map(row => header.map(fieldName => JSON.stringify(row[fieldName], replacer)).join(','));
        csv.unshift(header.join(','));
        let csvArray = csv.join('\r\n');

        var blob = new Blob([csvArray], {type: 'text/csv' })
        saveAs(blob, this.reportname+DateTime.local().toFormat('yyyy-MM-dd')+".csv");  
      }
    
    
    resetfilter(){
        if(this.dataTable != undefined)
        {
        this.count='';
        this.dataTable.filter('','validation','contains');
        this.dataTable.filter('','ZATCAapp','contains');
        }

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

    filter(dt,value,validation,condition,count){
        if(dt != undefined)
        {
        this.count=count;
        dt.filter('','validation',condition);
        dt.filter('','ZATCAapp',condition);
        dt.filter(value,validation,condition);
        }
    }

    convert(irnno: string){
        this.irnNo=irnno;
        console.log(this.irnNo,'this.irnNo');
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
                this._salesInvoiceServiceProxy.createInvoiceFromDraft(irnno).subscribe( {
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


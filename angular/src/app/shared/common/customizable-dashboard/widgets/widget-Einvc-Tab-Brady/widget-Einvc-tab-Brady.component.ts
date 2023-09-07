import { Component, OnInit, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import {  SalesInvoicesServiceProxy, TenantDashboardServiceProxy } from '@shared/service-proxies/service-proxies';
import { DateTime } from 'luxon';
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
import { error } from 'console';
import { futimesSync } from 'fs';

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
export class WidgetEinvcComponent  implements OnInit {
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
    status= true;
    labels: any[] = [];
    labelcolor: any[] = [];
    reportname : string;
    messages:any[]=[];
    modeltype:string;
    @ViewChild('modalterms' , { static: false }) modal: ModalDirective;

     getData(selectedDate: any) {
    }
    
  
    loadTopStatsData(results) {
            this.dashboardTopStats.init(results);
    }
    tenantId: Number;
    invoices:any[]=[];
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
    columns: any[] = this.salesColumns;
    zatca_status:any[]=[]
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
         this._dateTimeCustomService.data$.subscribe(e=>{
            if(e)
            this.selectedDate = e;
            this.getSalesData();
            this.tenantId =  this._sessionService.tenantId;
        })
    }


    resetdata(){
        this.totalrecords=0;
        this.valid=0;
        this.invalid=0;
        this.zatcapp=0;
        this.zatcawar=0;
        this.zatcarej=0;
        this.zatcasub=0;
    }
    getSalesData() {        
        this.resetdata();
        this.loader=true;
        this.resetfilter();
        this._salesInvoiceServiceProxy
        .getintegrationdashboradcolor(this.selectedDate[0],this.selectedDate[1],this.type)
        .pipe(finalize(() => this.loader=false))            
        .subscribe((results) => {
            this.messages = results;
            this.colors=results[0];
        });
        this._salesInvoiceServiceProxy
            .getintegrationdashboraddata(this.selectedDate[0],this.selectedDate[1],this.type)
            .pipe(finalize(() => this.loader=false))            
            .subscribe((result) => {
                this.zatca_status=[]
                this.labels=[]
                this.invoices = result;
                console.log(this.invoices, 'ww');
                this.invoices.forEach(e=>{
                        this.zatca_status.push(e['zatca Status'])
                        if(e['zatca Status'] != " ")
                        {
                            if (e['amount'] > 1000) {
                                e['zatca Status'] = JSON.parse(e['zatca Status'])?.clearanceStatus;
                                if(e['zatca Status']==='NOT_CLEARED')
                                {
                                    e['zatca Status']='false';
                                }
                                else
                                {
                                    e['zatca Status']='true';
                                }
                            } else {
                                e['zatca Status'] = JSON.parse(e['zatca Status'])?.reportingStatus;
                                if(e['zatca Status']==='NOT_REPORTED')
                                {
                                    e['zatca Status']='false';
                                }
                                else
                                {
                                    e['zatca Status']='true';
                                }
                            }     
                 }
                 else
                 {
                    e['zatca Status']=' ';
                 }  
                })
                if(result?.length>0)
                {
                this.labels = Object.keys(result[0]).filter(a=>a!='amount');;
                }
                this.reportname=this.type+'dashboardreport';
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
            this.columns = this.salesColumns;
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


    modaltermsshow(i:number,type:string){
        if(type=='data')
        {
        this.modeltype=type;
        this.em=this.messages[i].error;
        this.em = this.em.substring(0, this.em.length) 
        this.modal.show();
        }
        else
        {
            this.modeltype=type;
            this.wm= JSON.parse(this.zatca_status[i])?.validationResults.warningMessages[i];
            this.em= JSON.parse(this.zatca_status[i]);
            this.modal.show();
        }
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

    filter(dt,value,validation,condition,count){
        if(dt != undefined)
        {
        this.count=count;
        dt.filter('','validation',condition);
        dt.filter('','ZATCAapp',condition);
        dt.filter(value,validation,condition);
        }
    }
}


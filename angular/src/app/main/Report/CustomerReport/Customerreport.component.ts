

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { finalize } from 'rxjs/operators';
import { FileDownloadService } from '@shared/utils/file-download.service';


//create an interface for the data
interface Invoice {
  issueDate: DateTime;
  count: number;
  vatAmount: number;
  standardRated: number;
  zeroRated: number;
  exports: number;
  privateHealth: number;
  exempt: number;
  vatRate: number;
  totalAmount: number;
}

@Component({
  templateUrl: './CustomerReport.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})

export class CustomerReportComponent extends AppComponentBase {
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];

  tenantId: Number;
  tenantName: string;
  fromDate: Date;
  toDate: Date;
  detailedInvoices: any[] = [];
  invoices: any[] = [];
  vatAmount: number=0;
  totalAmount:number=0;
  count: number = 0;
  taxable: number = 0;
  zeroRated: number = 0;
  health: number = 0;
  export: number = 0;
  exempt: number = 0;
  isdisable:boolean=false;
  OutofScope:number=0;


  columns: any[]= [
    { field: 'name', header: 'Name' },
    { field: 'constitutionType', header: 'Constitution Type' },
    { field: 'country', header: 'Country' },
    {field:"contactNumber",header:"Contact Number"},
    {field:"documentNumber",header:"Document Number"}];

 exportColumns: any[] = this.columns.map(col => ({title: col.header, dataKey: col.field}));
 _selectedColumns: any[] = this.columns;


  @Input() get selectedColumns(): any[] {
    return this._selectedColumns;
  }

 set selectedColumns(val: any[]) {
    //restore original order
    this._selectedColumns = this.columns.filter(col => val.includes(col));
  }
    //---
  constructor(  
    injector: Injector,
    private _ReportServiceProxy: ReportServiceProxy,
    private _sessionService: AppSessionService,
    private _dateTimeService: DateTimeService,
    private _fileDownloadService: FileDownloadService,

    ) {
    super(injector);
  }
  

//ngoninit

  ngOnInit(): void {
this.getCustomerReport();
   this.tenantId =  this._sessionService.tenantId
  this.tenantName =  this._sessionService.tenancyName
  }

//    groupBy(list, keyGetter) {
//     const map = new Map();
//     list.forEach((item) => {
//       console.log(map);
//          const key = keyGetter(item);
//          let inv:Invoice={
//           issueDate:item.invoiceheader.issueDate,
//           count:1,
//           vatAmount:item.invoicesummary.totalAmountWithVAT - item.invoicesummary.totalAmountWithoutVAT,
//           standardRated:item.invoicesummary.totalAmountWithoutVAT,
//           zeroRated:0,
//           exports:0,
//           privateHealth:0,
//           exempt:0,
//           vatRate:15,
//           totalAmount:item.invoicesummary.sumOfInvoiceLineNetAmount
//         };
//          const collection = map.get(key);
//          if (!collection) {
          
//              map.set(key, inv);
//          } else {
//           let temp = map.get(key);
//           temp.count++;
//           temp.vatAmount+= item.invoicesummary.totalAmountWithVAT - item.invoicesummary.totalAmountWithoutVAT;
//           temp.totalAmount+=item.invoicesummary.sumOfInvoiceLineNetAmount;
//           temp.standardRated+=item.invoicesummary.totalAmountWithoutVAT ;
//           map.set(key, temp);
//          }
//     });
//     return Array.from(map.values());
// }

ResetData() {
  this.vatAmount = 0;
  this.export = 0;
  this.taxable = 0;
  this.totalAmount = 0;
  this.health = 0;
  this.exempt = 0;
  this.zeroRated=0
  this.count=0;
  this.OutofScope=0;

}

  getCustomerReport(){
    this.ResetData();
    this.isdisable=true;
      this._ReportServiceProxy.getMasterReport("CUSTOMER")
      .pipe(finalize(() => this.isdisable=false))
      .subscribe((result) => {
      console.log(result);
      this.invoices = result;
  //     const grouped = this.groupBy(this.detailedInvoices, a => this.getDate(a.invoiceheader.issueDate.toString()));
  //  this.invoices = grouped
  //     console.log(grouped);

      this.invoices.forEach(element => {
          this.vatAmount += element.vatAmount;
          this.totalAmount += element.totalAmount;
          this.taxable += element.taxableAmount;
          this.zeroRated += element.zeroRated;
          this.health += element.healthCare;
          this.exempt += element.exempt;
          this.export += element.exports;
          this.OutofScope+=element.outofScope;
          this.count+=parseInt(element.invoicenumber);
        
      });
    
    });
  }

  parseDate(dateString: string): DateTime {
    if (dateString) {
        return   DateTime.fromISO(new Date(dateString).toISOString());
        
    }
    return null;
  }
  
  getDate(dateString: string): string {
    if (dateString) {
        return   DateTime.fromISO(new Date(dateString).toISOString()).toISODate();
        
    }
    return null;
  }
  addFooter(li:any[]):any[]{
    li.push({invoiceDate:"Total",
    taxableAmount:this.taxable,
    zeroRated:this.zeroRated,
    exempt:this.exempt,
    exports:this.export,
    outofScope:this.OutofScope,
    vatAmount:this.vatAmount,
    totalAmount:this.totalAmount})
   console.log(li);
    return li;
  }

  removeFooter(li:any[]):any[]{
    console.log(li);
    li.pop();
    return li;
   }
   downloadExcel(li:any[]){
    this._ReportServiceProxy.getReportExcel(this.tenantName,'Customer Matser Report',"CUSTOMER")
    .subscribe((result) => {
        this._fileDownloadService.downloadTempFile(result);
    });
  }
}
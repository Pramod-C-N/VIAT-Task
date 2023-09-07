

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
  templateUrl: './debitnotepurchasedaywise.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})

export class DebitNotePurchaseDaywiseReportComponent extends AppComponentBase {
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
  outofScope:number=0;
  customs:number=0;
  exise:number=0;
  otherChargesPaid:number=0;


  columns: any[]= [
    //{ field: 'irnNo', header: 'IRN No' }
    {field:'invoiceDate', header: 'DN Date ' },
    { field: 'invoicenumber', header: 'DN Count' },
    //{field:"referenceNo",header:"Reference No"},
    {field:"purchasecategory",header:"Purchase Category"},
    {field:"taxableAmount",header:"Taxable Amount"},
    {field:"zeroRated",header:"Zero Rated"},
    {field:"exempt",header:"Exempt"},
    {field:"outofScope",header:"Out of Scope"},
    {field:"importVATCustoms",header:"Import Subject to VAT paid at Customs"},
    {field:"vatDeffered",header:"Import Subject to Deferred VAT"},
    {field:"importsatRCM",header:"Import Subject to RCM"},
    {field:"customsPaid",header:"Customs Paid"},
    {field:"exciseTaxPaid",header:"Excise Tax Paid"},
    {field:"otherChargesPaid",header:"Other Charges Paid"},
    {field:"vatrate",header:"VAT Rate"},
    {field:'vatAmount', header: 'VAT Amount' },
    {field:"totalAmount",header:"Total Amount"}];

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
    private _SalesDaywiseProxy: ReportServiceProxy,
    private _sessionService: AppSessionService,
    private _dateTimeService: DateTimeService,
    private _fileDownloadService: FileDownloadService,

    ) {
    super(injector);
  }
  

//ngoninit

  ngOnInit(): void {

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
  this.outofScope=0;
  this.exise=0
    this.customs=0
    this.otherChargesPaid=0

}

getDebitNotePurchaseDaywiseReport(){
    this.ResetData();
    this.isdisable=true;
      this._SalesDaywiseProxy.getDebitPurchaseDayWiseReport(this.parseDate(this.dateRange[0].toString()),this.parseDate(this.dateRange[1].toString()))
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
          this.outofScope+=element.outofScope;
          this.exise+=element.exciseTaxPaid;
          this.customs+=element.customsPaid;
          this.otherChargesPaid+=element.otherChargesPaid;
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
    li.push({irnNo:"Total",
    taxableAmount:this.taxable,
    zeroRated:this.zeroRated,
    exempt:this.exempt,
    exports:this.export,
    outofScope:this.outofScope,
    vatAmount:this.vatAmount,
    exciseTaxPaid:this.exise,
    customsPaid:this.customs,
    otherChargesPaid:this.otherChargesPaid,
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
    this._SalesDaywiseProxy.getDaywiseDebitPurchaseToExcel(this.parseDate(this.dateRange[0].toString()),this.parseDate(this.dateRange[1].toString()),'DebitNote(Purchase)DaywiseReport',this.tenantName,false)
.subscribe((result) => {
    this._fileDownloadService.downloadTempFile(result);
});}
}
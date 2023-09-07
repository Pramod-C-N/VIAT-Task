import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {ReportServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { finalize } from 'rxjs/operators';
import { FileDownloadService } from '@shared/utils/file-download.service';


@Component({
  templateUrl: './debitNoteReport.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class DebitNoteReportComponent extends AppComponentBase {
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
  code:string;
  tenantId: Number;
  tenantName: string;
  fromDate: Date;
  toDate: Date;
  invoices: any[] = [];
  vatAmount: number=0;
  totalAmount: number=0;
  taxable: number=0;
  zeroRated: number=0;
  exempt: number=0;
  isdisable:boolean=false;
  Gov:number=0;
  export:number=0;

  columns: any[]= [
    { field: 'invoiceDate', header: 'Debit Note Date' },
    { field: 'irnNo', header: 'Debit Note No' },
    {field:"invoicenumber",header:"Invoice Number"},
    {field:"referenceNo",header:"Reference Number"},
    {field:"taxableAmount",header:"Taxable Amount"},
    {field:"govtTaxableAmt",header:"Govt Taxable Amount"},
    {field:"zeroRated",header:"Zero Rated"},
    {field:"exports",header:"Exports"},
    {field:"exempt",header:"Exempt"},
    {field:"vatRate",header:"VAT Rate"},
    { field: 'vatAmount', header: 'VAT Amount' },
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
  constructor(
    injector: Injector,
    private _debitNoteProxy: ReportServiceProxy,
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
  this.code="debitsales"
  

 
  }

  ResetData() {
    this.taxable=0
    this.vatAmount=0
    this.zeroRated=0
    this.exempt=0
    this.totalAmount=0
    this.Gov=0
    this.export=0
  }

  getDebitNoteReport(){

    this.ResetData();
    this.isdisable=true;
    this._debitNoteProxy.getDebitNotePeriodicalReport(this.parseDate(this.dateRange[0].toString()),this.parseDate(this.dateRange[1].toString()))
    .pipe(finalize(() => this.isdisable=false))
    .subscribe((result) => {
      console.log(result);
      this.invoices = result;
      this.invoices.forEach(element => {
        this.taxable+=element.taxableAmount
        this.vatAmount+=element.vatAmount
        this.zeroRated+=element.zeroRated
        this.exempt+=element.exempt
        this.totalAmount+=element.totalAmount
        this.Gov+=element.govtTaxableAmt
        this.export+=element.exports
      });
    });
  }

  parseDate(dateString: string): DateTime {
    if (dateString) {
        return   DateTime.fromISO(new Date(dateString).toISOString());
        
    }
    return null;
  }
  addFooter(li:any[]):any[]{
    li.push({invoiceDate:"Total",
    taxableAmount:this.taxable,
    govtTaxableAmt:this.Gov,
    zeroRated:this.zeroRated,
    exempt:this.exempt,
    exports:this.export,
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
    this._debitNoteProxy.getDebitDetailedToExcel(this.parseDate(this.dateRange[0].toString()),this.parseDate(this.dateRange[1].toString()),this.code,'DebitNote(Sales)DetailedReport',this.tenantName,false)
.subscribe((result) => {
    this._fileDownloadService.downloadTempFile(result);
});
}

}
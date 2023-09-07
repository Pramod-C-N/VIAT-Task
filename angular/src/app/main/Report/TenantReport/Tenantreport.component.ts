import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { finalize } from 'rxjs/operators';
import { ReportServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
  templateUrl: './TenantReport.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class TenantReportComponent extends AppComponentBase {
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];

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
  outofScope:number=0;
  vatrate:number=0;
  export:number=0;
  Gov:number=0;
  code:string;
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
    this.getTenantReport();
    this.code="VATCNS000"
   this.tenantId =  this._sessionService.tenantId
  this.tenantName =  this._sessionService.tenancyName
  
  

 
  }

  ResetData() {
    this.taxable=0
    this.vatAmount=0
    this.zeroRated=0
    this.exempt=0
    this.totalAmount=0
    this.outofScope=0
    this.vatrate=0
    this.export=0
    this.Gov=0
  }

  getTenantReport(){

    this.ResetData();
    this.isdisable=true;
    this._ReportServiceProxy.getMasterReport("TENANT")
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
        this.outofScope+=element.outofScope
        this.vatrate+=element.vatrate
        this.export+=element.exports
        this.Gov+=element.govtTaxableAmt

      });
      console.log(this.taxable);

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
    outofScope:this.outofScope,
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
    this._ReportServiceProxy.getReportExcel(this.tenantName,'Tenant Matser Report',"TENANT")
    .subscribe((result) => {
        this._fileDownloadService.downloadTempFile(result);
    });
  }

}
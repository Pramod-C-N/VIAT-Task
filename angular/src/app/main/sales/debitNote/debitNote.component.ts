import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {DebitNotesServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
  templateUrl: './debitNote.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class DebitNoteComponent extends AppComponentBase {
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];

  pdfUrl = AppConsts.pdfUrl + '/InvoiceFiles';
  invoices: any[] = [];
  tenantId:number;
  tenantName:string;
  columns: any[]= [
    { field: 'invoiceId', header: 'InvoiceId' },
    { field: 'invoiceDate', header: 'Invoice Date' },
    { field: 'referenceNumber', header: 'Reference Number' },
    {field:"customerName",header:"Customer Name"},
    {field:"contactNo",header:"Contact No"},
    {field:"amount",header:"Amount"}];

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
    private _debitNoteProxy: DebitNotesServiceProxy,
    private _sessionService: AppSessionService,
    private _dateTimeService: DateTimeService
    ) {
    super(injector);
  }
  
  getSalesData(){
    // this._debitNoteProxy.getDebitData(this.parseDate(this.dateRange[0].toString()),this.parseDate(this.dateRange[1].toString())).subscribe((result) => {
    //   console.log(result)
    //   this.invoices = result;
    //   // this.invoices.forEach(element => {
    //   //   element.invoiceheader.issueDate = new Date(element.invoiceheader.issueDate).toLocaleDateString();
    //   // });
    // });
  }
  parseDate(dateString: string): DateTime {
    if (dateString) {
        return   DateTime.fromISO(new Date(dateString).toISOString());
        
    }
    return null;
  }

//ngoninit

  ngOnInit(): void {
    this.tenantId = this._sessionService.tenantId==null?0:this._sessionService.tenantId;
  this.tenantName=this._sessionService.tenancyName;
  this.getSalesData();

 
  }



}
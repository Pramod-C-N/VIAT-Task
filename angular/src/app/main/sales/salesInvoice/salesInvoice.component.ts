// @ts-nocheck

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild,Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SalesInvoicesServiceProxy, GetInvoiceDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTime } from 'luxon';
import { AppSessionService } from '@shared/common/session/app-session.service';
@Component({
  templateUrl: './salesInvoice.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class SalesInvoiceComponent extends AppComponentBase {


  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];

  invoices: ReportDto[] = [];
  tenantId: Number;
  tenantName: String;
  pdfUrl = AppConsts.pdfUrl + '/InvoiceFiles';

  columns: any[]= [
    { field: 'invoiceId', header: 'Invoice Id' },
    { field: 'invoiceDate', header: 'InvoiceDate' },
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
    
    private _sessionService: AppSessionService,
private _invoiceServiceProxy: SalesInvoicesServiceProxy,
private _dateTimeService: DateTimeService

    ) {
    super(injector);
  }

//ngoninit

  ngOnInit(): void {
    this.tenantId = this._sessionService.tenantId==null?0:this._sessionService.tenantId;
    this.tenantName =  this._sessionService.tenancyName
    this.getSalesData()
    

   
  }

  getSalesData(){
    this._invoiceServiceProxy.getSalesData(this.parseDate(this.dateRange[0].toString()),this.parseDate(this.dateRange[1].toString())).subscribe((result) => {
      console.log(result)
      this.invoices = result;
      // this.invoices.forEach(element => {
      //   element.invoiceheader.issueDate = new Date(element.invoiceheader.issueDate).toLocaleDateString();
      // });
    });
  }
  parseDate(dateString: string): DateTime {
    if (dateString) {
        return   DateTime.fromISO(new Date(dateString).toISOString());
        
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



}
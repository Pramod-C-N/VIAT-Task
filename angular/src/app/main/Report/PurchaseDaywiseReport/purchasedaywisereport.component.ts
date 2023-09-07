// @ts-nocheck

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTime } from 'luxon';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { FileDownloadService } from '@shared/utils/file-download.service';


@Component({
  templateUrl: './purchasedaywisereport.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class purchaseEntryDaywiseSummaryComponent extends AppComponentBase {

  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];

  tenantId: Number;
  tenantName: String;
  fromDate: Date;
  toDate: Date;
  pdate: Date;
  invoices: any[] = [];
  vatAmount = 0;
  totalAmount = 0;
  tcount = 0;
  tAmount = 0;
  taxable = 0;
  customPaid = 0;
  custom = 0;
  zero = 0;
  exempt = 0;
  import = 0;
  rcm = 0;
  excisetax = 0;
  other = 0;
  OutofScope = 0;

  columns: any[] = [
    { field: 'invoiceDate', header: 'Purchase Date' },
    { field: 'invoiceNumber', header: 'Purchase Count' },
    {field: 'taxableAmount', header: 'Taxable Amount'},
    {field: 'exempt', header: 'Exempt'},
    {field: 'importsatCustoms', header: 'Imports at Customs'},
    {field: 'importsatRCM', header: 'Imports at RCM'},
    {field: 'zeroRated', header: 'Zero Rated'},
    {field: 'purchaseCategory', header: 'Category'},
    {field: 'customsPaid', header: 'Customs Paid'},
    {field: 'exciseTaxPaid', header: 'Excise Tax Paid'},
    {field: 'otherChargesPaid', header: 'Other Charges Paid'},
    {field: 'outofScope', header: 'Out of Scope'},
    {field: 'vatDeffered', header: 'VAT Deffered'},
    {field: 'rcmApplicable', header: 'RCM Applicable'},
    {field: 'vatAmount', header: 'VAT Amount'},
    {field: 'totalAmount', header: 'Total Amount'},
];

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
    private _PurchaseEntryServiceProxy: ReportServiceProxy,
    // private _Trn_PurchaseDataAggregationsServiceProxy:Trn_PurchaseDataAggregationsServiceProxy,
    private _sessionService: AppSessionService,
    private _dateTimeService: DateTimeService,
    private _fileDownloadService: FileDownloadService,
    ) {
    super(injector);
  }

  ResetData(){
    this.vatAmount = 0;
    this.totalAmount = 0;
    this.taxable = 0;
    this.customPaid = 0;
    this.custom = 0;
    this.tAmount = 0;
    this.tcount = 0;
    this.other = 0;
    this.rcm = 0;
    this.import = 0;
    this.excisetax = 0;
    this.exempt = 0;
    this.zero = 0;
    this.OutofScope = 0;
  }

//ngoninit

  ngOnInit(): void {

   this.tenantId =  this._sessionService.tenantId;
  this.tenantName =  this._sessionService.tenancyName;


  }

  getPurchaseEntryDaywiseReport(){
    this.ResetData();
      this._PurchaseEntryServiceProxy.getPurchaseDaywiseReport(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString())).subscribe((result) => {
      console.log(result);
      this.invoices = result;
      this.invoices.forEach(element => {
          // this.vatAmount += element.vatAmount;
          // this.totalAmount += element.totalAmount;
          // this.taxable += element.taxableAmount;
          // this.customPaid += element.customsPaid;
           this.custom += element.customsPaid;
          this.vatAmount += element.vatAmount;
          this.taxable += element.taxableAmount;
          this.tAmount += element.totalAmount;
          this.tcount += parseInt(element.invoiceNumber);
          //this.count+=parseInt(element.invoiceNumber);
          this.other += element.otherChargesPaid;
          this.rcm += element.importsatRCM;
          this.import += element.importsatCustoms;
          this.excisetax += element.exciseTaxPaid;
          this.exempt += element.exempt;
          this.zero += element.zeroRated;
          this.OutofScope += element.outofScope;

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

  exportToCsv(){

    const replacer = (key, value) => value === null ? '' : value; // specify how you want to handle null values here
    const header = Object.keys(this.invoices[0]);
    let csv = this.invoices.map(row => header.map(fieldName => JSON.stringify(row[fieldName], replacer)).join(','));
    csv.unshift(header.join(','));
    let csvArray = csv.join('\r\n');

    let blob = new Blob([csvArray], {type: 'text/csv' });
    saveAs(blob, 'Purchase Entry Daywise Report ' + DateTime.local().toFormat('yyyy-MM-dd') + '.csv');
  }
  addFooter(li: any[]): any[]{
    li.push({invoiceDate: 'Total',
    taxableAmount: this.taxable,
    zeroRated: this.zero,
    importsatCustoms: this.import,
    exempt: this.exempt,
    customsPaid: this.customPaid,
    otherChargesPaid: this.other,
    importsatRCM: this.rcm,
    exciseTaxPaid: this.excisetax,
    vatAmount: this.vatAmount
    , totalAmount: this.totalAmount});
   console.log(li);
    return li;
  }

  removeFooter(li: any[]): any[]{
    console.log(li);
    li.pop();
    return li;
   }

   downloadExcel(li: any[]){
    this._PurchaseEntryServiceProxy.getDaywisePurchaseToExcel(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()), 'PurchaseEntryDaywiseReport', this.tenantName, false)
    .subscribe((result) => {
        this._fileDownloadService.downloadTempFile(result);
    });
  }
}

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
  templateUrl: './debitnotepurchasedetailedreport.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
  styleUrls: ['./debitnotepurchasedetailedreport.component.css'],
})
export class DebitNotePurchaseDetailedReportComponent extends AppComponentBase {
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
  code: string;
  tenantId: Number;
  tenantName: string;
  fromDate: Date;
  toDate: Date;
  invoices: any[] = [];
  vatAmount = 0;
  totalAmount = 0;
  taxable = 0;
  zeroRated = 0;
  exempt = 0;
  isdisable = false;
  outofScope = 0;
  importVATCustoms = 0;
  vatDeffered = 0;
  importsatRCM = 0;
  vatrate = 0;
  export = 0;
  Gov = 0;
  customs = 0;
  exise = 0;
  otherChargesPaid = 0;
  tab = 'Detailed';
  checkboxValue: any;

  detailedwiseFooter: any = {
    taxable: 0,
    vatAmount: 0,
    zeroRated: 0,
    exempt: 0,
    totalAmount: 0,
    outofScope: 0,
    vatrate: 0,
    export: 0,
    Gov: 0,
    exise: 0,
    customs: 0,
    otherChargesPaid: 0,
    chargesIncludingVAT: 0,
    importVATCustoms: 0,
    vatDeffered: 0,
    importsatRCM: 0
  };

  daywiseFooter: any = {
    vatAmount: 0,
    export: 0,
    taxable: 0,
    totalAmount: 0,
    health: 0,
    exempt: 0,
    zeroRated: 0,
    count: 0,
    outofScope: 0,
    exise: 0,
    customs: 0,
    otherChargesPaid: 0,
    importVATCustoms: 0,
    vatDeffered: 0,
    importsatRCM: 0
  };

  detailedColumns: any[] = [
    // { field: 'irnNo', header: 'IRN No' },
    { field: 'invoicenumber', header: 'Debit Note Number' },
    { field: 'vendorName', header: 'Vendor Name' },
    { field: 'invoiceDate', header: 'Debit Note Date ' },
    { field: 'referenceNo', header: 'Purchase Number' },
    { field: 'purchasecategory', header: 'Purchase Category' },
    { field: 'taxableAmount', header: 'Taxable Amount' },
    { field: 'vatrate', header: 'VAT Rate' },
    { field: 'vatAmount', header: 'VAT Amount' },
    { field: 'totalAmount', header: 'Total Amount' },
    { field: 'zeroRated', header: 'Zero Rated' },
    { field: 'exempt', header: 'Exempt' },
    { field: 'outofScope', header: 'Out of Scope' },
    { field: 'importVATCustoms', header: 'Import VAT Customs' },
    { field: 'vatDeffered', header: 'VAT Deffered' },
    { field: 'importsatRCM', header: 'Imports at RCM' },
    { field: 'customsPaid', header: 'Customs Paid' },
    { field: 'exciseTaxPaid', header: 'Excise Tax Paid' },
    { field: 'otherChargesPaid', header: 'Other Charges Paid' },
    {field: 'chargesIncludingVAT', header: 'Charges Including VAT'}];
    dayColumns: any[] = [
      {field: 'invoiceDate', header: 'Debit Note Date ' },
      {field: 'invoicenumber', header: 'Debit Note Count' },
      {field: 'purchasecategory', header: 'Purchase Category'},
      {field: 'taxableAmount', header: 'Taxable Amount'},
      {field: 'zeroRated', header: 'Zero Rated'},
      {field: 'exempt', header: 'Exempt'},
      {field: 'outofScope', header: 'Out of Scope'},
      {field: 'importVATCustoms', header: 'Import VAT Customs'},
      {field: 'vatDeffered', header: 'VAT Deffered'},
      {field: 'importsatRCM', header: 'Imports at RCM'},
      {field: 'customsPaid', header: 'Customs Paid'},
      {field: 'exciseTaxPaid', header: 'Excise Tax Paid'},
      {field: 'otherChargesPaid', header: 'Other Charges Paid'},
      //{field: 'vatrate', header: 'VAT Rate'},
      {field: 'vatAmount', header: 'VAT Amount' },
      {field: 'totalAmount', header: 'Total Amount'}];
      columns: any[] = this.detailedColumns;
  constructor(
    injector: Injector,
    private _creditNoteProxy: ReportServiceProxy,
    private _sessionService: AppSessionService,
    private _dateTimeService: DateTimeService,
    private _fileDownloadService: FileDownloadService,

  ) {
    super(injector);
  }

  //ngoninit

  ngOnInit(): void {

    this.tenantId = this._sessionService.tenantId;
    this.tenantName = this._sessionService.tenancyName;
    //this.code = 'debitdetailed';
  }

  ResetData() {
      this.detailedwiseFooter = {
        taxable: 0,
        vatAmount: 0,
        zeroRated: 0,
        exempt: 0,
        totalAmount: 0,
        outofScope: 0,
        vatrate: 0,
        export: 0,
        Gov: 0,
        customs: 0,
        exise: 0,
        otherChargesPaid: 0,
        chargesIncludingVAT: 0,
        importVATCustoms: 0,
        vatDeffered: 0,
        importsatRCM: 0
      };
      this.daywiseFooter = {
        vatAmount: 0,
        export: 0,
        taxable: 0,
        totalAmount: 0,
        health: 0,
        exempt: 0,
        zeroRated: 0,
        count: 0,
        outofScope: 0,
        exise: 0,
        customs: 0,
        otherChargesPaid: 0,
        importVATCustoms: 0,
        vatDeffered: 0,
        importsatRCM: 0
      };
  }

  getDebititNotePurchaseDetailedReport() {
    this.isdisable = true;
    this._creditNoteProxy.getDebitPurchaseDetailedReport(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
      .pipe(finalize(() => this.isdisable = false))
      .subscribe((result) => {
        this.invoices = result;
        this.ResetData();
        this.invoices.forEach(element => {
          this.detailedwiseFooter.taxable += element.taxableAmount;
          this.detailedwiseFooter.vatAmount += element.vatAmount;
          this.detailedwiseFooter.zeroRated += element.zeroRated;
          this.detailedwiseFooter.exempt += element.exempt;
          this.detailedwiseFooter.totalAmount += element.totalAmount;
          this.detailedwiseFooter.outofScope += element.outofScope;
          this.detailedwiseFooter.importVATCustoms += element.importVATCustoms;
          this.detailedwiseFooter.vatDeffered += element.vatDeffered;
          this.detailedwiseFooter.importsatRCM += element.importsatRCM;
          this.detailedwiseFooter.vatrate += element.vatrate;
          this.detailedwiseFooter.export += element.exports;
          this.detailedwiseFooter.Gov += element.govtTaxableAmt;
          this.detailedwiseFooter.exise += element.exciseTaxPaid;
          this.detailedwiseFooter.customs += element.customsPaid;
          this.detailedwiseFooter.otherChargesPaid += element.otherChargesPaid;
          this.detailedwiseFooter.chargesIncludingVAT += element.chargesIncludingVAT;
        });
      });
  }
  getDebitNotePurchaseDaywiseReport(){
    this.isdisable = true;
      this._creditNoteProxy.getDebitPurchaseDayWiseReport(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
      .pipe(finalize(() => this.isdisable = false))
      .subscribe((result) => {
      this.invoices = result;
      this.ResetData();
      this.invoices.forEach(element => {
          this.daywiseFooter.vatAmount += element.vatAmount;
          this.daywiseFooter.totalAmount += element.totalAmount;
          this.daywiseFooter.taxable += element.taxableAmount;
          this.daywiseFooter.zeroRated += element.zeroRated;
          this.daywiseFooter.health += element.healthCare;
          this.daywiseFooter.exempt += element.exempt;
          this.daywiseFooter.export += element.exports;
          this.daywiseFooter.outofScope += element.outofScope;
          this.daywiseFooter.importVATCustoms += element.importVATCustoms;
          this.daywiseFooter.vatDeffered += element.vatDeffered;
          this.daywiseFooter.importsatRCM += element.importsatRCM;
          this.daywiseFooter.exise += element.exciseTaxPaid;
          this.daywiseFooter.customs += element.customsPaid;
          this.daywiseFooter.otherChargesPaid += element.otherChargesPaid;
          this.daywiseFooter.count += parseInt(element.invoicenumber);
      });
    });
  }

  parseDate(dateString: string): DateTime {
    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());
    }
    return null;
  }

  addFooter(li: any[]): any[] {
    li.push({
      irnNo: 'Total',
      taxableAmount: this.taxable,
      zeroRated: this.zeroRated,
      exempt: this.exempt,
      exports: this.export,
      outofScope: this.outofScope,
      vatAmount: this.vatAmount,
      exciseTaxPaid: this.exise,
      customsPaid: this.customs,
      otherChargesPaid: this.otherChargesPaid,
      totalAmount: this.totalAmount
    });
    return li;
  }

  removeFooter(li: any[]): any[] {
    li.pop();
    return li;
  }

  downloadExcel(li: any[]) {
    if (this.tab === 'Detailed') {
    this._creditNoteProxy.getDebitPurchaseToExcel(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()), this.code, 'DebitNote(Purchase)DetailedReport', this.tenantName, false)
      .subscribe((result) => {
        this._fileDownloadService.downloadTempFile(result);
      });
    } else{
      this._creditNoteProxy.getDaywiseDebitPurchaseToExcel(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()), 'DebitNote(Purchase)DaywiseReport', this.tenantName, false)
      .subscribe((result) => {
          this._fileDownloadService.downloadTempFile(result);
      });
    }
  }
  changeEvent(event: any) {
    if (this.tab === 'Detailed') {
      this.changeTab('Daywise');
      this.tab = 'Daywise';
    } else {
      this.changeTab('Detailed');
      this.tab = 'Detailed';
    }
  }
  changeTab(tab: string) {
    this.tab = tab;
    if (tab === 'Daywise') {
      this.columns = this.dayColumns;
       this.getDebitNotePurchaseDaywiseReport();
    } else if (tab === 'Detailed') {
      this.columns = this.detailedColumns;
      this.getDebititNotePurchaseDetailedReport();

    }
  }
  searchData() {
    this.changeTab(this.tab);
  }
}

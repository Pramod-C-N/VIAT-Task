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
import { GlobalConstsCustomService } from '@shared/customService/global-consts-service';

@Component({
  templateUrl: './purchaseDetailedReport.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
  styleUrls: ['./PurchasedetailedReport.component.css'],
})

export class PurchaseDetailedReportComponent extends AppComponentBase {
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
  tenantId: Number;
  tenantName: string;
  fromDate: Date;
  toDate: Date;
  invoices: any[] = [];
  isdisable = false;
  code: string;
  reportType: any;
  tab = 'Detailed';
  vitamenu: boolean = AppConsts.vitaMenu;
  checkboxValue: any;

  detailedColumns: any[] = [
    { field: 'invoiceNumber', header: 'Purchase Number' },
    { field: 'vendorName', header: 'Vendor Name' },
    { field: 'invoiceDate', header: 'Purchase Date' },
    { field: 'purchaseCategory', header: 'Purchase Category' },
    { field: 'taxableAmount', header: 'Taxable Amount' },
    { field: 'vatrate', header: 'VAT Rate' },
    { field: 'vatAmount', header: 'VAT Amount' },
    { field: 'totalAmount', header: 'Total Amount' },
    { field: 'zeroRated', header: 'Zero Rated' },
    { field: 'exempt', header: 'Exempt' },
    { field: 'outofScope', header: 'Out of Scope' },
    //{ field: 'importsatCustoms', header: 'Imports At Customs' },
    { field: 'importVATCustoms', header: 'Import VAT Customs' },
    { field: 'vatDeffered', header: 'VAT Deffered' },
    { field: 'importsatRCM', header: 'Imports at RCM' },
    { field: 'customsPaid', header: 'Customs Paid' },
   // { field: 'rcmApplicable', header: 'RCM Applicable' },
    { field: 'exciseTaxPaid', header: 'Excise Tax Paid' },
    { field: 'otherChargesPaid', header: 'Other Charges Paid' },
    {field: 'chargesIncludingVAT', header: 'Charges Including VAT'}];
  daywiseColumns: any[] = [
    { field: 'invoiceDate', header: 'Purchase Date' },
    { field: 'invoiceNumber', header: 'Purchase Count' },
    { field: 'taxableAmount', header: 'Taxable Amount' },
    { field: 'exempt', header: 'Exempt' },
    { field: 'importsatCustoms', header: 'Imports at Customs' },
    { field: 'importsatRCM', header: 'Imports at RCM' },
    { field: 'zeroRated', header: 'Zero Rated' },
    { field: 'purchaseCategory', header: 'Category' },
    { field: 'customsPaid', header: 'Customs Paid' },
    { field: 'exciseTaxPaid', header: 'Excise Tax Paid' },
    { field: 'otherChargesPaid', header: 'Other Charges Paid' },
    { field: 'outofScope', header: 'Out of Scope' },
    { field: 'vatDeffered', header: 'VAT Deffered' },
    { field: 'rcmApplicable', header: 'RCM Applicable' },
    { field: 'vatAmount', header: 'VAT Amount' },
    { field: 'totalAmount', header: 'Total Amount' },
  ];

  detailedDescriptionColumns: any[] = [
    { field: 'invoiceDate', header: 'Purchase Date' },
    { field: 'description', header: 'Description' },
    { field: 'vendorName', header: 'Vendor Name' },
    { field: 'invoiceNumber', header: 'Purchase Number' },
    { field: 'taxableAmount', header: 'Taxable Amount' },
    { field: 'zeroRated', header: 'Zero Rated' },
    { field: 'exempt', header: 'Exempt' },
    { field: 'outofScope', header: 'Out of Scope' },
  //  { field: 'importsatCustoms', header: 'Imports At Customs' },
    { field: 'importVATCustoms', header: 'Imports At Customs' },
    { field: 'importsatRCM', header: 'Imports at RCM' },
    { field: 'purchaseCategory', header: 'Purchase Category' },
    { field: 'vatDeffered', header: 'VAT Deffered' },
    { field: 'rcmApplicable', header: 'RCM Applicable' },
    { field: 'customsPaid', header: 'Customs Paid' },
    { field: 'exciseTaxPaid', header: 'Excise Tax Paid' },
    { field: 'otherChargesPaid', header: 'Other Charges Paid' },
    { field: 'vatAmount', header: 'VAT Amount' },
    { field: 'totalAmount', header: 'Total Amount' },
  ];

  daywiseFooter: any = {
    vatAmount: 0,
    totalAmount: 0,
    taxable: 0,
    customPaid: 0,
    other: 0,
    rcm: 0,
    import: 0,
    excisetax: 0,
    exempt: 0,
    zero: 0,
    outofscope: 0,
  };
  detailedwiseFooter: any = {
    vatAmount: 0,
    totalAmount: 0,
    taxable: 0,
    customPaid: 0,
    other: 0,
    rcm: 0,
    import: 0,
    excisetax: 0,
    exempt: 0,
    zero: 0,
    outofscope: 0,
    chargesIncludingVAT: 0,
    vatDeffered: 0
  };
  columns: any[] = this.detailedColumns;
  constructor(
    injector: Injector,
    private _PurchaseEntryServiceProxy: ReportServiceProxy,
    private _sessionService: AppSessionService,
    private _dateTimeService: DateTimeService,
    private _fileDownloadService: FileDownloadService,
    private _GlobalConstsCustomService: GlobalConstsCustomService,

  ) {
    super(injector);
  }
  ResetData() {
    this.detailedwiseFooter = {
      vatAmount: 0,
      totalAmount: 0,
      taxable: 0,
      customPaid: 0,
      other: 0,
      rcm: 0,
      import: 0,
      excisetax: 0,
      exempt: 0,
      zero: 0,
      outofscope: 0,
      chargesIncludingVAT: 0,
      vatDeffered:0
    };
    this.daywiseFooter = {
      custom: 0,
      vatAmount: 0,
      taxable: 0,
      tAmount: 0,
      tcount: 0,
      other: 0,
      rcm: 0,
      import: 0,
      excisetax: 0,
      exempt: 0,
      zero: 0,
      OutofScope: 0,
    };
  }

  ngOnInit(): void {
    this.code = 'VATPUR000';
    this.tenantId = this._sessionService.tenantId;
    this.tenantName = this._sessionService.tenancyName;
    this.getReportType();
    //this.getPurchaseDetailedReport();
    this._GlobalConstsCustomService.data$.subscribe(e=>{
      this.vitamenu=e.isVita;
  })
  }

  getPurchaseDetailedReport() {
    if (this.code === undefined) {
      this.code = 'VATPUR000';
    }
    this.isdisable = true;
    this._PurchaseEntryServiceProxy.getPurchaseDetailedReport(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()), this.code)
      .pipe(finalize(() => this.isdisable = false))
      .subscribe((result) => {
        this.ResetData();
        this.invoices = result;
        this.invoices.forEach(element => {
          this.detailedwiseFooter.vatAmount += element.vatAmount;
          this.detailedwiseFooter.totalAmount += element.totalAmount;
          this.detailedwiseFooter.taxable += element.taxableAmount;
          this.detailedwiseFooter.customPaid += element.customsPaid;
          this.detailedwiseFooter.other += element.otherChargesPaid;
          this.detailedwiseFooter.rcm += element.importsatRCM;
          this.detailedwiseFooter.vatDeffered += element.vatDeffered;
        //  this.detailedwiseFooter.import += element.importsatCustoms;
          this.detailedwiseFooter.import += element.importVATCustoms;
          this.detailedwiseFooter.excisetax += element.exciseTaxPaid;
          this.detailedwiseFooter.exempt += element.exempt;
          this.detailedwiseFooter.zero += element.zeroRated;
          this.detailedwiseFooter.outofscope += element.outofScope;
          this.detailedwiseFooter.chargesIncludingVAT += element.chargesIncludingVAT;

        });
      });
  }

  getPurchaseDayReport() {
    this.ResetData();
    this.isdisable = true;
    this._PurchaseEntryServiceProxy.getPurchaseDaywiseReport(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
      .pipe(finalize(() => this.isdisable = false))
      .subscribe((result) => {
        this.invoices = result;
        this.invoices.forEach(element => {
          this.daywiseFooter.custom += element.customsPaid;
          this.daywiseFooter.vatAmount += element.vatAmount;
          this.daywiseFooter.taxable += element.taxableAmount;
          this.daywiseFooter.tAmount += element.totalAmount;
          this.daywiseFooter.tcount += parseInt(element.invoiceNumber);
          this.daywiseFooter.other += element.otherChargesPaid;
          this.daywiseFooter.rcm += element.importsatRCM;
          this.daywiseFooter.import += element.importsatCustoms;
          this.daywiseFooter.excisetax += element.exciseTaxPaid;
          this.daywiseFooter.exempt += element.exempt;
          this.daywiseFooter.zero += element.zeroRated;
          this.daywiseFooter.OutofScope += element.outofScope;
        });
      });
  }
  parseDate(dateString: string): DateTime {
    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());
    }
    return null;
  }
  removeFooter(li: any[]): any[] {
    li.pop();
    return li;
  }

  downloadExcel(li: any[]) {
    if (this.tab === 'Detailed') {
      this._PurchaseEntryServiceProxy.getPurchaseToExcel(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()), this.code, 'PurchaseEntryDetailedReport', this.tenantName, false)
        .subscribe((result) => {
          this._fileDownloadService.downloadTempFile(result);
        });
    } else {
      this._PurchaseEntryServiceProxy.getDaywisePurchaseToExcel(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()), 'PurchaseEntryDaywiseReport', this.tenantName, false)
        .subscribe((result) => {
          this._fileDownloadService.downloadTempFile(result);
        });
    }
  }

  getReportType() {
    this._PurchaseEntryServiceProxy.getReportType('PUR').subscribe((result) => {
      this.reportType = result;
    });
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
      this.columns = this.daywiseColumns;
      this.getPurchaseDayReport();
    } else if (tab === 'Detailed') {
      if (this.code === 'VATPUR002' || this.code === 'VATPUR008') {
        this.columns = this.detailedDescriptionColumns;
      } else {
        this.columns = this.detailedColumns;
      }
      this.getPurchaseDetailedReport();
    }
  }
  searchData() {
    this.changeTab(this.tab);
  }
}


import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VatReportDto, ReportServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as FileSaver from 'file-saver';
import * as ExcelJS from 'exceljs';
import { GlobalConstsCustomService } from '@shared/customService/global-consts-service';

@Component({
  templateUrl: './overHeadGapAnalysisReport.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
  styleUrls: ['./overHeadGapAnalysisReport.component.css'],
})
export class overHeadGapAnalysisReportComponent extends AppComponentBase {
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];

  tenantId: Number;
  tenantName: string;
  fromDate: Date;
  toDate: Date;
  vatAmount = 0;
  totalAmount = 0;
  previousyear = [];
  netAmount = 0;
  searched = false;
  months = [
    'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
    'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
  ];
  cols = [
    'col1', 'col2', 'col3', 'col4', 'col5', 'col6',
    'col7', 'col8', 'col9', 'col10', 'col11', 'col12'
  ];
  year = new Date().getFullYear().toString().slice(-2);
  year1 = new Date().getFullYear();
  data: any[];

  constructor(
    injector: Injector,
    private _VatReportServiceProxy: ReportServiceProxy,
    private _sessionService: AppSessionService,
    private _dateTimeService: DateTimeService,
    private _fileDownloadService: FileDownloadService,
    private _localStorage: GlobalConstsCustomService,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.tenantId = this._sessionService.tenantId;
    this.tenantName = this._sessionService.tenancyName;
    this.getPreviousYearData()
    this.getOverheadGapAnalysisReport();
    this.getFinancialYear();
  }

  getFinancialYear() {
    //this._localStorage.setCache("dat",)
    this._VatReportServiceProxy.getFinancialYearData().subscribe((result) => {
      //console.log(result[0].effectiveFromDate.slice(2,4),"result");
      this.year = result[0].effectiveFromDate.slice(2,4);
      this.year1 = result[0].effectiveFromDate.slice(0,4);
    });
  }

  getOverheadGapAnalysisReport() {
    this._VatReportServiceProxy.getOverHeadGapAnalysisReport(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString())).subscribe((result) => {
      console.log(result,'gapanalysis');
      this.data = result;
      console.log(this.data,'gapreport');
      this.searched = true;
    });
  }

  getPreviousYearData() {
    this._VatReportServiceProxy.getPreviousYearData().subscribe((result) => {
      console.log(result,'previousyear');
      this.previousyear = result[0].apportionmentSupplies
      this.year1 = result[0].finYear;
    });
  }

  refreshVal() {
    this._VatReportServiceProxy.refereshSalesSummaryData().subscribe((result) => {
      return result;
    });
  }

  parseDate(dateString: string): DateTime {
    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());
    }
    return null;
  }

  getStyle(style: number): { [key: string]: any } {
    let dynamicStyles: { [key: string]: any } = {};

    switch (style) {
        case 1:
            dynamicStyles = { 'font-weight': 'bold' , 'text-decoration': 'underline'};
            break;
        case 2:
            dynamicStyles = { 'font-weight': 'bold' };
            break;
        case 3:
            dynamicStyles = { 'font-style': 'italic' };
            break;
        default:
            break;
    }

    return dynamicStyles;
}

async exportExcel() {
  const workbook = new ExcelJS.Workbook();
  const worksheet = workbook.addWorksheet('OverheadGapAnalysis');
  // Add headers
  const topheader = worksheet.addRow(['Overhead Gap Analysis Report']);
  topheader.font = { bold: true };
  worksheet.mergeCells('A1:N1');
  topheader.getCell(1).alignment = { horizontal: 'center' };


  const header = ['Particulars', ...this.months.map(month => `${month} - ${this.year}`), 'Total'];
  const headerRow = worksheet.addRow(header);
  headerRow.font = { bold: true };
  // const fields = this.data.filter(a => a.field != "pdf").map(a=>a.field)
  // worksheet.addRow(this.data.filter(a => a.header != "PDF").map(a=>a.header));
  for (const item of this.data) {
    const row = [item.particulars, ...this.cols.map(col => item[col]), item.amount];
    const itemRow = worksheet.addRow(row);
    itemRow.getCell(1).font = {bold : true};
  }

  const buffer = await workbook.xlsx.writeBuffer();

  const blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'OverheadGapAnalysis.xlsx';
    a.click();

    URL.revokeObjectURL(url);
  // Add data
  // console.log(this.data,'data')
  // this.data.forEach((item) => {
  //     const row: any = [];
  //     fields.forEach((col) => {
  //         console.log(col,'col')
  //         row.push(item[col]);
  //     });
  //     worksheet.addRow(row);
  // });
  // workbook.xlsx.writeBuffer().then((buffer: any) => {
  //     const blob = new Blob([buffer], {
  //         type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
  //     });
  //     FileSaver.saveAs(blob, `${'vita_' + Date.now()}.xlsx`);
  };

}

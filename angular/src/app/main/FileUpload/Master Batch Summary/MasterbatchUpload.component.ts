import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
//import {InvoiceHeadersServiceProxy, GetInvoiceHeaderForViewDto,InvoiceSummariesServiceProxy, GetInvoiceSummaryForViewDto, GetCreditNoteHeaderForViewDto, GetCreditNoteSummaryForViewDto, CreditNoteServiceServiceProxy, CreditNoteSummariesServiceProxy, CreditNoteHeadersServiceProxy, GetInvoiceDto, StandardFileReport, ImportStandardFilesServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';

import { HttpClient } from '@angular/common/http';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { finalize } from 'rxjs/operators';
import { FileUpload } from 'primeng/fileupload';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ImportBatchDatasServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './MasterbatchUpload.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
  styleUrls: ['./MasterbatchUpload.component.less'],
})
export class MasterBatchUploadComponent extends AppComponentBase {
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
  // eslint-disable-next-line @typescript-eslint/member-ordering
  @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;

  invoices: any[] = [];
  tenantId: Number;
  tenantName: String;
  fromDate: Date;
  toDate: Date;
  uploadUrl: string;
  setview = false;
  batchid: number;
  fileName: string;
  type: string;

  constructor(
    injector: Injector,
    private _httpClient: HttpClient,
    private _sessionService: AppSessionService,
    private _fileUpload: ImportBatchDatasServiceProxy,
    private _fileDownloadService: FileDownloadService,
    private _dateTimeService: DateTimeService
  ) {
    super(injector);

  }

  // eslint-disable-next-line @angular-eslint/use-lifecycle-interface
  ngOnInit(): void {
    this.tenantId = this._sessionService.tenantId;
    this.tenantName = this._sessionService.tenancyName;
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/StandardFileUpload/ImportFromExcel';
    this.fromDate = new Date();
    this.toDate = new Date();
    this.getBatchUploadData();
    this.setview = false;
  }
  getBatchUploadData() {
    this._fileUpload.getMasterReportData(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString())).subscribe((result) => {
      console.log(result);
      this.invoices = result;
      this.batchid = result[0]?.batchId;
      this.fileName = result[1]?.fileName;
    });
  }

  searchData() {
    this.getBatchUploadData();
  }

  view(batchid, filename) {
    this.setview = true;
    this.batchid = batchid;
    this.fileName = filename;
    this.type = 'Master';
  }

  uploadExcel(data: { files: File }): void {
    const formData: FormData = new FormData();
    const file = data.files[0];
    file.FileName = file.FileName + '_' + this.tenantId;
    formData.append('file', file, file.name);

    this._httpClient
      .post<any>(this.uploadUrl, formData)
      .pipe(finalize(() => this.excelFileUpload.clear()))
      .subscribe((response) => {
        if (response.success) {
          this.notify.success(this.l('ImportSalesInvoiceProcessStart'));
        } else if (response.error != null) {
          this.notify.error(this.l('ImportSalesInvoiceUploadFailed'));
        }
      });
  }




  onUploadExcelError(): void {
    this.notify.error(this.l('ImportSalesInvoiceUploadFailed'));
  }
  downloadInvalidExcel(id): void {
  }

  exportToExcel(fileName: string, batchId: number): void {
    this._fileUpload
      .getMasterInvalidRecordsToExcel(batchId, fileName)
      .subscribe((result) => {
        this._fileDownloadService.downloadTempFile(result);
      });
  }


  parseDate(dateString: string): DateTime {
    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());
    }
    return null;
  }

}

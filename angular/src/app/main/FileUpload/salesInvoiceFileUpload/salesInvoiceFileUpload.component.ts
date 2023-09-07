import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, NgZone } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ImportBatchDatasServiceProxy, GetCreditNoteSummaryForViewDto, NotificationServiceProxy, UserNotification, SalesInvoicesServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { IFormattedUserNotification, UserNotificationHelper } from '@app/shared/layout/notifications/UserNotificationHelper';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { forEach as _forEach } from 'lodash-es';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

interface RecievedData {
  fileName: string;
  fileDate: DateTime;
  batchId: number;
  valid: number;
  invalid: number;
  total: number;
}
@Component({
  templateUrl: './salesInvoiceFileUpload.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
  styleUrls: ['./salesInvoiceFileUpload.component.less'],
})
export class SalesInvoiceFileUploadComponent extends AppComponentBase {
  @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];

  uploadUrl: string;
  tenantId: number;
  tenantName: String;
  invoices: any[] = [];
  fileName: string;
  filedate: string;
  batchid: number;
  fromdate: DateTime;
  todate: DateTime;
  notifications: IFormattedUserNotification[] = [];
  unreadNotificationCount = 0;
  isLoading = false;
  constructor(
    injector: Injector,
    private _httpClient: HttpClient,
    private _sessionService: AppSessionService,
    private _fileUpload: ImportBatchDatasServiceProxy,
    private _fileDownloadService: FileDownloadService,
    private _notificationService: NotificationServiceProxy,
    private _userNotificationHelper: UserNotificationHelper,
    private _salesInvoicesAppService: SalesInvoicesServiceProxy,
    public _zone: NgZone,
    private _dateTimeService: DateTimeService
  ) {
    super(injector);
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/SalesFileUpload/ImportFromExcel';
  }

  // eslint-disable-next-line @angular-eslint/use-lifecycle-interface
  ngOnInit(): void {
    this.tenantId = this._sessionService.tenantId;
    this.tenantName = this._sessionService.tenancyName;
    this.loadNotifications();
    this.registerToEvents();
  }
  //----------------------begin notification service-------------------------------
  loadNotifications(): void {
    if (UrlHelper.isInstallUrl(location.href)) {
      return;
    }
    this._notificationService.getUserNotifications(undefined, undefined, undefined, 3, 0).subscribe((result) => {
      this.unreadNotificationCount = result.unreadCount;
      this.notifications = [];
      _forEach(result.items, (item: UserNotification) => {
        this.notifications.push(this._userNotificationHelper.format(<any>item));
        if (this._userNotificationHelper.format(<any>item).text.startsWith('Sales File Upload')) {
          this.getUploadedData(this.fileName);
        }
      });
    });
  }

  registerToEvents() {
    let self = this;
    function onNotificationReceived(userNotification) {
      self._userNotificationHelper.show(userNotification);
      self.loadNotifications();
    }
    this.subscribeToEvent('abp.notifications.received', (userNotification) => {
      self._zone.run(() => {
        onNotificationReceived(userNotification);
        this.isLoading = false;
      });
    });
    function onNotificationsRefresh() {
      self.loadNotifications();
    }
    this.subscribeToEvent('app.notifications.refresh', () => {
      self._zone.run(() => {
        onNotificationsRefresh();
      });
    });
  }
  //----------------------end notification service-------------------------------
  randomString(length: number, chars: string): string {
    let result = '';
    for (let i = length; i > 0; --i) {
      result += chars[Math.floor(Math.random() * chars.length)];
    }
    return result;
  }
  uploadExcel(data: { files: File }): void {
    //rename file with a unique id
    const file = data.files[0];
    this.getDate();
    const newFileName = this.filedate + '_' + file.name;
    this.fileName = newFileName;
    const newFile = new File([file], newFileName, { type: file.type });
    const formData = new FormData();
    formData.append('file', newFile);
    formData.append('fromdate', this.fromdate.toString());
    formData.append('todate', this.todate.toString());
    this._httpClient
      .post<any>(this.uploadUrl, formData)
      .pipe(finalize(() => this.excelFileUpload.clear()))
      .subscribe((response) => {
        if (response.success) {
          this.isLoading = true;
          this.notify.success(this.l('ImportSalesInvoiceProcessStart'));
        } else if (response.error != null) {
          this.notify.error(this.l('ImportSalesInvoiceUploadFailed'));
        }
      });
  }

  // eslint-disable-next-line @typescript-eslint/member-ordering
  setview = false;
  view() {
    this.setview = true;
  }
  getUploadedData(fileName: string): void {
    this._salesInvoicesAppService.getSalesBatchData(fileName).subscribe((result) => {
      console.log(result);
      if (result && result.length > 0) {
        this.invoices = result;
      }
      this.batchid = result[0]?.batchId;
    });
  }

  onUploadExcelError(): void {
    this.notify.error(this.l('ImportSalesInvoiceUploadFailed'));
  }

  downloadInvalidExcel(): void {
  }

  getDate() {
    let date = new Date();
    this.fromdate = this.parseDate(this.dateRange[0].toString());
    this.todate = this.parseDate(this.dateRange[1].toString());
    this.filedate = date.getFullYear().toString() + date.getMonth().toString() + date.getDay().toString() + date.getHours().toString() + date.getMinutes().toString() + date.getSeconds().toString();
  }

  parseDate(dateString: string): DateTime {
    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());
    }
    return null;
  }
  exportToExcel(batchId: number): void {
    this._fileUpload
      .getInvalidRecordsToExcel(batchId, this.fileName)
      .subscribe((result) => {
        this._fileDownloadService.downloadTempFile(result);
      });
  }
}

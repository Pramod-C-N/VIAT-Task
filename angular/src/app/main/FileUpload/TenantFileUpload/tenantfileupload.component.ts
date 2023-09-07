import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, NgZone } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TenantBasicDetailsServiceProxy, CustomersesServiceProxy, SalesInvoicesServiceProxy, NotificationServiceProxy, UserNotification, ImportBatchDatasServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { IFormattedUserNotification, UserNotificationHelper } from '@app/shared/layout/notifications/UserNotificationHelper';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { forEach as _forEach } from 'lodash-es';

@Component({
  templateUrl: './tenantfileupload.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
  styleUrls: ['./tenantfileupload.component.less'],

})

export class TenantUploadComponent extends AppComponentBase {
  @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
  invoices: any = [];
  uploadUrl: string;
  tenantId: Number;
  tenantName: String;
  fileName: string;
  notifications: IFormattedUserNotification[] = [];
  unreadNotificationCount = 0;
  filedate: string;
  batchid: number;
  type: string;
  isLoading = false;
  constructor(
    injector: Injector,
    private _httpClient: HttpClient,
    private _sessionService: AppSessionService,
    private _notificationService: NotificationServiceProxy,
    private _userNotificationHelper: UserNotificationHelper,
    public _zone: NgZone,
    private _fileUpload: ImportBatchDatasServiceProxy,
    private _fileDownloadService: FileDownloadService,
    private _TenantServiceProxy: TenantBasicDetailsServiceProxy
  ) {
    super(injector);
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/TenantFileUpload/ImportFromExcel';
  }

  // eslint-disable-next-line @angular-eslint/use-lifecycle-interface
  ngOnInit(): void {
    this.tenantId = this._sessionService.tenantId;
    this.tenantName = this._sessionService.tenancyName;
    this.loadNotifications();
    this.registerToEvents();
  }

  loadNotifications(): void {
    if (UrlHelper.isInstallUrl(location.href)) {
      return;
    }

    this._notificationService.getUserNotifications(undefined, undefined, undefined, 3, 0).subscribe((result) => {
      this.unreadNotificationCount = result.unreadCount;
      this.notifications = [];
      _forEach(result.items, (item: UserNotification) => {
        this.notifications.push(this._userNotificationHelper.format(<any>item));
        if (this._userNotificationHelper.format(<any>item).text.startsWith('Tenant File Upload')) {
          this.getUploadedData(this.fileName);
        }
      });
    });
  }

  getUploadedData(fileName: string): void {
    this._TenantServiceProxy.getTenantBatchData(fileName).subscribe((result) => {
      if (result && result.length > 0) {
        this.invoices = result;
      }
      this.batchid = result[0]?.batchId;
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

  uploadExcel(data: { files: File }): void {
    const formData: FormData = new FormData();
    this.getDate();
    const file = data.files[0];
    const newFileName = this.filedate + '_' + file.name;
    this.fileName = newFileName;
    const newFile = new File([file], newFileName, { type: file.type });
    formData.append('file', newFile);
    this._httpClient
      .post<any>(this.uploadUrl, formData)
      .pipe(finalize(() => this.excelFileUpload.clear()))
      .subscribe((response) => {
        if (response.success) {
          this.isLoading = true;
          this.notify.success(this.l('ImportTenantProcessStart'));
        } else if (response.error != null) {
          this.notify.error(this.l('ImportTenantUploadFailed'));
        }
      });
  }

  onUploadExcelError(): void {
    this.notify.error(this.l('ImportTenantUploadFailed'));
  }

  downloadInvalidExcel(): void {
  }

  // eslint-disable-next-line @typescript-eslint/member-ordering
  setview = false;
  view() {
    this.setview = true;
    this.type = 'Master';
  }
  getDate() {
    let date = new Date();
    this.filedate = date.getFullYear().toString() + date.getMonth().toString() + date.getDay().toString() + date.getHours().toString() + date.getMinutes().toString() + date.getSeconds().toString();
  }

  exportToExcel(batchId: number): void {
    this._fileUpload
      .getMasterInvalidRecordsToExcel(batchId, this.fileName)
      .subscribe((result) => {
        this._fileDownloadService.downloadTempFile(result);
      });
  }
}

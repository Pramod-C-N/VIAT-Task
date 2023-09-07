import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, NgZone } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    ImportBatchDatasServiceProxy,
    GetCreditNoteSummaryForViewDto,
    NotificationServiceProxy,
    UserNotification,
    SalesInvoicesServiceProxy,
    FileMappingPost,
    TenantConfigurationServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import {
    IFormattedUserNotification,
    UserNotificationHelper,
} from '@app/shared/layout/notifications/UserNotificationHelper';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { forEach as _forEach } from 'lodash-es';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { LabelsConfig } from '@abylle/ng-file-mapper/src/app/drop-down-mapper/columns/columns.component';
import { NotifyService } from 'abp-ng2-module';

interface RecievedData {
    fileName: string;
    fileDate: DateTime;
    batchId: number;
    valid: number;
    invalid: number;
    total: number;
}
@Component({
    templateUrl: './file-mapper.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
    styleUrls: ['./file-mapper.component.less'],
})
export class FileMapperComponent extends AppComponentBase {
    labelsConfig: LabelsConfig = {
        availableColumns: 'Our fields',
    };
    availableColumns = [];
    mappings = [];
    isLoading:boolean=true
    // mappings = [
    //     {
    //         uploadedFields: ['Invoice Type'],
    //         fieldForMapping: 'Invoice Type',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Invoice Type'],
    //     },
    //     {
    //         uploadedFields: ['TransType'],
    //         fieldForMapping: 'TransType',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['TransType'],
    //     },
    //     {
    //         uploadedFields: ['IRN Number'],
    //         fieldForMapping: 'IRN Number',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['IRN Number'],
    //     },
    //     {
    //         uploadedFields: ['Invoice Number *'],
    //         fieldForMapping: 'Invoice Number *',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Invoice Number *'],
    //     },
    //     {
    //         uploadedFields: ['Invoice Issue Date*'],
    //         fieldForMapping: 'Invoice Issue Date*',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Invoice Issue Date*'],
    //     },
    //     {
    //         uploadedFields: ['Invoice Issue Time *'],
    //         fieldForMapping: 'Invoice Issue Time *',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Invoice Issue Time *'],
    //     },
    //     {
    //         uploadedFields: ['Invoice currency code *'],
    //         fieldForMapping: 'Invoice currency code *',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Invoice currency code *'],
    //     },
    //     {
    //         uploadedFields: ['Purchase Order ID'],
    //         fieldForMapping: 'Purchase Order ID',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Purchase Order ID'],
    //     },
    //     {
    //         uploadedFields: ['Contract ID'],
    //         fieldForMapping: 'Contract ID',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Contract ID'],
    //     },
    //     {
    //         uploadedFields: ['Supply Date'],
    //         fieldForMapping: 'Supply Date',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Supply Date'],
    //     },
    //     {
    //         uploadedFields: ['Supply End Date'],
    //         fieldForMapping: 'Supply End Date',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Supply End Date'],
    //     },
    //     {
    //         uploadedFields: ['Buyer Master Code'],
    //         fieldForMapping: 'Buyer Master Code',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Buyer Master Code'],
    //     },
    //     {
    //         uploadedFields: ['Buyer Name'],
    //         fieldForMapping: 'Buyer Name',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Buyer Name'],
    //     },
    //     {
    //         uploadedFields: ['Buyer VAT number'],
    //         fieldForMapping: 'Buyer VAT number',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Buyer VAT number'],
    //     },
    //     {
    //         uploadedFields: ['Buyer Contact'],
    //         fieldForMapping: 'Buyer Contact',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Buyer Contact'],
    //     },
    //     {
    //         uploadedFields: ['Buyer Country Code'],
    //         fieldForMapping: 'Buyer Country Code',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Buyer Country Code'],
    //     },
    //     {
    //         uploadedFields: ['Invoice line identifier *'],
    //         fieldForMapping: 'Invoice line identifier *',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Invoice line identifier *'],
    //     },
    //     {
    //         uploadedFields: ['Item Master Code'],
    //         fieldForMapping: 'Item Master Code',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Item Master Code'],
    //     },
    //     {
    //         uploadedFields: ['Item name'],
    //         fieldForMapping: 'Item name',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Item name'],
    //     },
    //     {
    //         uploadedFields: ['Invoiced quantity unit of measure'],
    //         fieldForMapping: 'Invoiced quantity unit of measure',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Invoiced quantity unit of measure'],
    //     },
    //     {
    //         uploadedFields: ['Item gross price'],
    //         fieldForMapping: 'Item gross price',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Item gross price'],
    //     },
    //     {
    //         uploadedFields: ['Item price discount'],
    //         fieldForMapping: 'Item price discount',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Item price discount'],
    //     },
    //     {
    //         uploadedFields: ['Item net price*'],
    //         fieldForMapping: 'Item net price*',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Item net price*'],
    //     },
    //     {
    //         uploadedFields: ['Invoiced quantity '],
    //         fieldForMapping: 'Invoiced quantity ',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Invoiced quantity '],
    //     },
    //     {
    //         uploadedFields: ['Invoice line net amount '],
    //         fieldForMapping: 'Invoice line net amount ',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Invoice line net amount '],
    //     },
    //     {
    //         uploadedFields: ['Invoiced item VAT category code*'],
    //         fieldForMapping: 'Invoiced item VAT category code*',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Invoiced item VAT category code*'],
    //     },
    //     {
    //         uploadedFields: ['Invoiced item VAT rate*'],
    //         fieldForMapping: 'Invoiced item VAT rate*',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Invoiced item VAT rate*'],
    //     },
    //     {
    //         uploadedFields: ['VAT exemption reason Code'],
    //         fieldForMapping: 'VAT exemption reason Code',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['VAT exemption reason Code'],
    //     },
    //     {
    //         uploadedFields: ['VAT exemption reason '],
    //         fieldForMapping: 'VAT exemption reason ',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['VAT exemption reason '],
    //     },
    //     {
    //         uploadedFields: ['VAT line amount*'],
    //         fieldForMapping: 'VAT line amount*',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['VAT line amount*'],
    //     },
    //     {
    //         uploadedFields: ['Line amount inclusive VAT*'],
    //         fieldForMapping: 'Line amount inclusive VAT*',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Line amount inclusive VAT*'],
    //     },
    //     {
    //         uploadedFields: ['Advance Rcpt Amount Adjusted'],
    //         fieldForMapping: 'Advance Rcpt Amount Adjusted',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Advance Rcpt Amount Adjusted'],
    //     },
    //     {
    //         uploadedFields: ['VAT on Advance Receipt Amount Adjusted'],
    //         fieldForMapping: 'VAT on Advance Receipt Amount Adjusted',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['VAT on Advance Receipt Amount Adjusted'],
    //     },
    //     {
    //         uploadedFields: ['Advance Receipt Reference Number'],
    //         fieldForMapping: 'Advance Receipt Reference Number',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Advance Receipt Reference Number'],
    //     },
    //     {
    //         uploadedFields: ['Payment Means'],
    //         fieldForMapping: 'Payment Means',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Payment Means'],
    //     },
    //     {
    //         uploadedFields: ['Payment Terms'],
    //         fieldForMapping: 'Payment Terms',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Payment Terms'],
    //     },
    //     {
    //         uploadedFields: ['Buyer Type'],
    //         fieldForMapping: 'Buyer Type',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Buyer Type'],
    //     },
    //     {
    //         uploadedFields: ['Comments'],
    //         fieldForMapping: 'Comments',
    //         defaultValue: null,
    //         transform: [],
    //         dataType: 'string',
    //         combination: ['Comments'],
    //     },
    // ];
    id: number = -1;
    mappingList: any[] = [];
    activeIndex: number;
    constructor(
        injector: Injector,
        private _httpClient: HttpClient,
        private _sessionService: AppSessionService,
        private _tenantConfiguration: TenantConfigurationServiceProxy,
        private _salesInvoicesAppService: SalesInvoicesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit() {
        this._salesInvoicesAppService.getFileMappings().subscribe((e) => {
            console.log(e);
            if (e) {
                this.mappingList = e;
            }
        });

        this._tenantConfiguration.getTenantConfigurationByTransactionType('General').subscribe(e=>{
            console.log(e)
            this.availableColumns = JSON.parse(e?.tenantConfiguration?.additionalData3??'[]')
            this.isLoading=false
        })
    }

    editMapping(m: any,activeIndex:number) {
        this.mappings = JSON.parse(m.mapping);
        this.id = m.id;
        this.activeIndex = activeIndex
        // console.log('i was here', m.id);
    }

    deleteMapping(m: any) {
        this._salesInvoicesAppService.deleteFileMapping(m.id).subscribe((e: any) => {
            if (e) this.notify.success(this.l('Successfully Deleted'));
            else this.notify.error(this.l('Something went wrong'));
            setTimeout(() => {
                window.location.reload();
            }, 1500);
        });
    }

    getMappings(output: any) {
        console.log(output);
        let body = new FileMappingPost();
        body.id = this.id;
        body.type = output?.additionalData?.transactionType ?? 'Sales';
        body.json = JSON.stringify(output.mappings);
        body.name = output?.additionalData?.mappingName;
        console.log(this.mappingList,this.id)
        body.isActive = this.id == -1 ? true : this.mappingList[this.activeIndex].isActive;
        this._salesInvoicesAppService.createOrUpdateFileMappings(body).subscribe((e) => {
            if (e && this.id != -1) this.notify.success(this.l('Successfully Updated'));
            else if (e && this.id == -1) this.notify.success(this.l('Successfully Created'));
            else this.notify.error(this.l('Something went wrong'));
            this.id = -1;
            setTimeout(() => {
                window.location.reload();
            }, 1500);
        });
    }
}

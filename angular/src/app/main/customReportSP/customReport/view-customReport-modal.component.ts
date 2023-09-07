import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetCustomReportForViewDto, CustomReportDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewCustomReportModal',
    templateUrl: './view-customReport-modal.component.html',
})
export class ViewCustomReportModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetCustomReportForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetCustomReportForViewDto();
        this.item.customReport = new CustomReportDto();
    }

    show(item: GetCustomReportForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}

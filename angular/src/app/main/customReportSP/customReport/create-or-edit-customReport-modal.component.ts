import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CustomReportServiceProxy, CreateOrEditCustomReportDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditCustomReportModal',
    templateUrl: './create-or-edit-customReport-modal.component.html',
})
export class CreateOrEditCustomReportModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    customReport: CreateOrEditCustomReportDto = new CreateOrEditCustomReportDto();

    constructor(
        injector: Injector,
        private _customReportServiceProxy: CustomReportServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(customReportId?: number): void {
        if (!customReportId) {
            this.customReport = new CreateOrEditCustomReportDto();
            this.customReport.id = customReportId;

            this.active = true;
            this.modal.show();
        } else {
            this._customReportServiceProxy.getCustomReportForEdit(customReportId).subscribe((result) => {
                this.customReport = result.customReport;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._customReportServiceProxy
            .createOrEdit(this.customReport)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}

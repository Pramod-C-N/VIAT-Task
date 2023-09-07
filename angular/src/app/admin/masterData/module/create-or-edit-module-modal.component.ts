import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ModuleServiceProxy, CreateOrEditModuleDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditModuleModal',
    templateUrl: './create-or-edit-module-modal.component.html',
})
export class CreateOrEditModuleModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    module: CreateOrEditModuleDto = new CreateOrEditModuleDto();

    constructor(
        injector: Injector,
        private _moduleServiceProxy: ModuleServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(moduleId?: number): void {
        if (!moduleId) {
            this.module = new CreateOrEditModuleDto();
            this.module.id = moduleId;

            this.active = true;
            this.modal.show();
        } else {
            this._moduleServiceProxy.getModuleForEdit(moduleId).subscribe((result) => {
                this.module = result.module;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._moduleServiceProxy
            .createOrEdit(this.module)
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

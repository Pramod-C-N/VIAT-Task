﻿import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { BusinessProcessServiceProxy, CreateOrEditBusinessProcessDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-businessProcess.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditBusinessProcessComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    businessProcess: CreateOrEditBusinessProcessDto = new CreateOrEditBusinessProcessDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('BusinessProcess'), '/app/main/masterData/businessProcess'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _businessProcessServiceProxy: BusinessProcessServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(businessProcessId?: number): void {
        if (!businessProcessId) {
            this.businessProcess = new CreateOrEditBusinessProcessDto();
            this.businessProcess.id = businessProcessId;

            this.active = true;
        } else {
            this._businessProcessServiceProxy.getBusinessProcessForEdit(businessProcessId).subscribe((result) => {
                this.businessProcess = result.businessProcess;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._businessProcessServiceProxy
            .createOrEdit(this.businessProcess)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/businessProcess']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._businessProcessServiceProxy
            .createOrEdit(this.businessProcess)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.businessProcess = new CreateOrEditBusinessProcessDto();
            });
    }
}

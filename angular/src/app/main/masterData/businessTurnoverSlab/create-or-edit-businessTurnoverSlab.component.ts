import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    BusinessTurnoverSlabServiceProxy,
    CreateOrEditBusinessTurnoverSlabDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-businessTurnoverSlab.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditBusinessTurnoverSlabComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    businessTurnoverSlab: CreateOrEditBusinessTurnoverSlabDto = new CreateOrEditBusinessTurnoverSlabDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('BusinessTurnoverSlab'), '/app/main/masterData/businessTurnoverSlab'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _businessTurnoverSlabServiceProxy: BusinessTurnoverSlabServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(businessTurnoverSlabId?: number): void {
        if (!businessTurnoverSlabId) {
            this.businessTurnoverSlab = new CreateOrEditBusinessTurnoverSlabDto();
            this.businessTurnoverSlab.id = businessTurnoverSlabId;

            this.active = true;
        } else {
            this._businessTurnoverSlabServiceProxy
                .getBusinessTurnoverSlabForEdit(businessTurnoverSlabId)
                .subscribe((result) => {
                    this.businessTurnoverSlab = result.businessTurnoverSlab;

                    this.active = true;
                });
        }
    }

    save(): void {
        this.saving = true;

        this._businessTurnoverSlabServiceProxy
            .createOrEdit(this.businessTurnoverSlab)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/businessTurnoverSlab']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._businessTurnoverSlabServiceProxy
            .createOrEdit(this.businessTurnoverSlab)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.businessTurnoverSlab = new CreateOrEditBusinessTurnoverSlabDto();
            });
    }
}

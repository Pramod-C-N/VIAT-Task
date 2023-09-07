import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    BusinessOperationalModelServiceProxy,
    CreateOrEditBusinessOperationalModelDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-businessOperationalModel.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditBusinessOperationalModelComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    businessOperationalModel: CreateOrEditBusinessOperationalModelDto = new CreateOrEditBusinessOperationalModelDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('BusinessOperationalModel'), '/app/main/masterData/businessOperationalModel'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _businessOperationalModelServiceProxy: BusinessOperationalModelServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(businessOperationalModelId?: number): void {
        if (!businessOperationalModelId) {
            this.businessOperationalModel = new CreateOrEditBusinessOperationalModelDto();
            this.businessOperationalModel.id = businessOperationalModelId;

            this.active = true;
        } else {
            this._businessOperationalModelServiceProxy
                .getBusinessOperationalModelForEdit(businessOperationalModelId)
                .subscribe((result) => {
                    this.businessOperationalModel = result.businessOperationalModel;

                    this.active = true;
                });
        }
    }

    save(): void {
        this.saving = true;

        this._businessOperationalModelServiceProxy
            .createOrEdit(this.businessOperationalModel)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/businessOperationalModel']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._businessOperationalModelServiceProxy
            .createOrEdit(this.businessOperationalModel)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.businessOperationalModel = new CreateOrEditBusinessOperationalModelDto();
            });
    }
}

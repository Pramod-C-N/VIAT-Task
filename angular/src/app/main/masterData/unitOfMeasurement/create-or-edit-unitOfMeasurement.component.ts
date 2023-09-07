import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    UnitOfMeasurementServiceProxy,
    CreateOrEditUnitOfMeasurementDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-unitOfMeasurement.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditUnitOfMeasurementComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    unitOfMeasurement: CreateOrEditUnitOfMeasurementDto = new CreateOrEditUnitOfMeasurementDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('UnitOfMeasurement'), '/app/main/masterData/unitOfMeasurement'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _unitOfMeasurementServiceProxy: UnitOfMeasurementServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(unitOfMeasurementId?: number): void {
        if (!unitOfMeasurementId) {
            this.unitOfMeasurement = new CreateOrEditUnitOfMeasurementDto();
            this.unitOfMeasurement.id = unitOfMeasurementId;

            this.active = true;
        } else {
            this._unitOfMeasurementServiceProxy.getUnitOfMeasurementForEdit(unitOfMeasurementId).subscribe((result) => {
                this.unitOfMeasurement = result.unitOfMeasurement;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._unitOfMeasurementServiceProxy
            .createOrEdit(this.unitOfMeasurement)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/unitOfMeasurement']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._unitOfMeasurementServiceProxy
            .createOrEdit(this.unitOfMeasurement)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.unitOfMeasurement = new CreateOrEditUnitOfMeasurementDto();
            });
    }
}

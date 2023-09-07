import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    PlaceOfPerformanceServiceProxy,
    CreateOrEditPlaceOfPerformanceDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-placeOfPerformance.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditPlaceOfPerformanceComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    placeOfPerformance: CreateOrEditPlaceOfPerformanceDto = new CreateOrEditPlaceOfPerformanceDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('PlaceOfPerformance'), '/app/main/masterData/placeOfPerformance'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _placeOfPerformanceServiceProxy: PlaceOfPerformanceServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(placeOfPerformanceId?: number): void {
        if (!placeOfPerformanceId) {
            this.placeOfPerformance = new CreateOrEditPlaceOfPerformanceDto();
            this.placeOfPerformance.id = placeOfPerformanceId;

            this.active = true;
        } else {
            this._placeOfPerformanceServiceProxy
                .getPlaceOfPerformanceForEdit(placeOfPerformanceId)
                .subscribe((result) => {
                    this.placeOfPerformance = result.placeOfPerformance;

                    this.active = true;
                });
        }
    }

    save(): void {
        this.saving = true;

        this._placeOfPerformanceServiceProxy
            .createOrEdit(this.placeOfPerformance)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/placeOfPerformance']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._placeOfPerformanceServiceProxy
            .createOrEdit(this.placeOfPerformance)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.placeOfPerformance = new CreateOrEditPlaceOfPerformanceDto();
            });
    }
}

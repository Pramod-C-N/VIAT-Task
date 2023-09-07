import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { NatureofServicesServiceProxy, CreateOrEditNatureofServicesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-natureofServices.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditNatureofServicesComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    natureofServices: CreateOrEditNatureofServicesDto = new CreateOrEditNatureofServicesDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('NatureofServices'), '/app/main/masterData/natureofServices'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _natureofServicesServiceProxy: NatureofServicesServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(natureofServicesId?: number): void {
        if (!natureofServicesId) {
            this.natureofServices = new CreateOrEditNatureofServicesDto();
            this.natureofServices.id = natureofServicesId;

            this.active = true;
        } else {
            this._natureofServicesServiceProxy.getNatureofServicesForEdit(natureofServicesId).subscribe((result) => {
                this.natureofServices = result.natureofServices;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._natureofServicesServiceProxy
            .createOrEdit(this.natureofServices)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/natureofServices']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._natureofServicesServiceProxy
            .createOrEdit(this.natureofServices)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.natureofServices = new CreateOrEditNatureofServicesDto();
            });
    }
}

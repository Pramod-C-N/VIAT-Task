import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ErrorGroupServiceProxy, CreateOrEditErrorGroupDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-errorGroup.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditErrorGroupComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    errorGroup: CreateOrEditErrorGroupDto = new CreateOrEditErrorGroupDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('ErrorGroup'), '/app/main/masterData/errorGroup'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _errorGroupServiceProxy: ErrorGroupServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(errorGroupId?: number): void {
        if (!errorGroupId) {
            this.errorGroup = new CreateOrEditErrorGroupDto();
            this.errorGroup.id = errorGroupId;

            this.active = true;
        } else {
            this._errorGroupServiceProxy.getErrorGroupForEdit(errorGroupId).subscribe((result) => {
                this.errorGroup = result.errorGroup;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._errorGroupServiceProxy
            .createOrEdit(this.errorGroup)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/errorGroup']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._errorGroupServiceProxy
            .createOrEdit(this.errorGroup)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.errorGroup = new CreateOrEditErrorGroupDto();
            });
    }
}

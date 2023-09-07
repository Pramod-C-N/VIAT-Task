import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ErrorTypeServiceProxy, CreateOrEditErrorTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-errorType.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditErrorTypeComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    errorType: CreateOrEditErrorTypeDto = new CreateOrEditErrorTypeDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('ErrorType'), '/app/main/masterData/errorType'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _errorTypeServiceProxy: ErrorTypeServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(errorTypeId?: number): void {
        if (!errorTypeId) {
            this.errorType = new CreateOrEditErrorTypeDto();
            this.errorType.id = errorTypeId;

            this.active = true;
        } else {
            this._errorTypeServiceProxy.getErrorTypeForEdit(errorTypeId).subscribe((result) => {
                this.errorType = result.errorType;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._errorTypeServiceProxy
            .createOrEdit(this.errorType)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/errorType']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._errorTypeServiceProxy
            .createOrEdit(this.errorType)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.errorType = new CreateOrEditErrorTypeDto();
            });
    }
}

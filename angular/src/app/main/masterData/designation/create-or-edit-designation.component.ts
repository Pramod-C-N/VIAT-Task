import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { DesignationServiceProxy, CreateOrEditDesignationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-designation.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditDesignationComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    designation: CreateOrEditDesignationDto = new CreateOrEditDesignationDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Designation'), '/app/main/masterData/designation'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _designationServiceProxy: DesignationServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(designationId?: number): void {
        if (!designationId) {
            this.designation = new CreateOrEditDesignationDto();
            this.designation.id = designationId;

            this.active = true;
        } else {
            this._designationServiceProxy.getDesignationForEdit(designationId).subscribe((result) => {
                this.designation = result.designation;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._designationServiceProxy
            .createOrEdit(this.designation)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/designation']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._designationServiceProxy
            .createOrEdit(this.designation)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.designation = new CreateOrEditDesignationDto();
            });
    }
}

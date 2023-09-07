import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { GenderServiceProxy, CreateOrEditGenderDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-gender.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditGenderComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    gender: CreateOrEditGenderDto = new CreateOrEditGenderDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Gender'), '/app/main/masterData/gender'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _genderServiceProxy: GenderServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(genderId?: number): void {
        if (!genderId) {
            this.gender = new CreateOrEditGenderDto();
            this.gender.id = genderId;

            this.active = true;
        } else {
            this._genderServiceProxy.getGenderForEdit(genderId).subscribe((result) => {
                this.gender = result.gender;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._genderServiceProxy
            .createOrEdit(this.gender)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/gender']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._genderServiceProxy
            .createOrEdit(this.gender)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.gender = new CreateOrEditGenderDto();
            });
    }
}

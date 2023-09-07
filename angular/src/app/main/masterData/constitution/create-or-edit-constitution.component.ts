import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ConstitutionServiceProxy, CreateOrEditConstitutionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-constitution.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditConstitutionComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    constitution: CreateOrEditConstitutionDto = new CreateOrEditConstitutionDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Constitution'), '/app/main/masterData/constitution'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _constitutionServiceProxy: ConstitutionServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(constitutionId?: number): void {
        if (!constitutionId) {
            this.constitution = new CreateOrEditConstitutionDto();
            this.constitution.id = constitutionId;

            this.active = true;
        } else {
            this._constitutionServiceProxy.getConstitutionForEdit(constitutionId).subscribe((result) => {
                this.constitution = result.constitution;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._constitutionServiceProxy
            .createOrEdit(this.constitution)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/constitution']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._constitutionServiceProxy
            .createOrEdit(this.constitution)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.constitution = new CreateOrEditConstitutionDto();
            });
    }
}

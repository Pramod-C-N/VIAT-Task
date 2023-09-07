import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { OrganisationTypeServiceProxy, CreateOrEditOrganisationTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-organisationType.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditOrganisationTypeComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    organisationType: CreateOrEditOrganisationTypeDto = new CreateOrEditOrganisationTypeDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('OrganisationType'), '/app/main/masterData/organisationType'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _organisationTypeServiceProxy: OrganisationTypeServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(organisationTypeId?: number): void {
        if (!organisationTypeId) {
            this.organisationType = new CreateOrEditOrganisationTypeDto();
            this.organisationType.id = organisationTypeId;

            this.active = true;
        } else {
            this._organisationTypeServiceProxy.getOrganisationTypeForEdit(organisationTypeId).subscribe((result) => {
                this.organisationType = result.organisationType;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._organisationTypeServiceProxy
            .createOrEdit(this.organisationType)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/organisationType']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._organisationTypeServiceProxy
            .createOrEdit(this.organisationType)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.organisationType = new CreateOrEditOrganisationTypeDto();
            });
    }
}

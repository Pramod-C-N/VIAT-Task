import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { AffiliationServiceProxy, CreateOrEditAffiliationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-affiliation.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditAffiliationComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    affiliation: CreateOrEditAffiliationDto = new CreateOrEditAffiliationDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Affiliation'), '/app/main/masterData/affiliation'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _affiliationServiceProxy: AffiliationServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(affiliationId?: number): void {
        if (!affiliationId) {
            this.affiliation = new CreateOrEditAffiliationDto();
            this.affiliation.id = affiliationId;

            this.active = true;
        } else {
            this._affiliationServiceProxy.getAffiliationForEdit(affiliationId).subscribe((result) => {
                this.affiliation = result.affiliation;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._affiliationServiceProxy
            .createOrEdit(this.affiliation)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/affiliation']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._affiliationServiceProxy
            .createOrEdit(this.affiliation)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.affiliation = new CreateOrEditAffiliationDto();
            });
    }
}

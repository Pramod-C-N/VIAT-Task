import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ExemptionReasonServiceProxy, CreateOrEditExemptionReasonDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-exemptionReason.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditExemptionReasonComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    exemptionReason: CreateOrEditExemptionReasonDto = new CreateOrEditExemptionReasonDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('ExemptionReason'), '/app/main/masterData/exemptionReason'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _exemptionReasonServiceProxy: ExemptionReasonServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(exemptionReasonId?: number): void {
        if (!exemptionReasonId) {
            this.exemptionReason = new CreateOrEditExemptionReasonDto();
            this.exemptionReason.id = exemptionReasonId;

            this.active = true;
        } else {
            this._exemptionReasonServiceProxy.getExemptionReasonForEdit(exemptionReasonId).subscribe((result) => {
                this.exemptionReason = result.exemptionReason;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._exemptionReasonServiceProxy
            .createOrEdit(this.exemptionReason)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/exemptionReason']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._exemptionReasonServiceProxy
            .createOrEdit(this.exemptionReason)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.exemptionReason = new CreateOrEditExemptionReasonDto();
            });
    }
}

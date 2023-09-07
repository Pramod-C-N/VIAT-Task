import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { AllowanceReasonServiceProxy, CreateOrEditAllowanceReasonDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-allowanceReason.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditAllowanceReasonComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    allowanceReason: CreateOrEditAllowanceReasonDto = new CreateOrEditAllowanceReasonDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('AllowanceReason'), '/app/main/masterData/allowanceReason'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _allowanceReasonServiceProxy: AllowanceReasonServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(allowanceReasonId?: number): void {
        if (!allowanceReasonId) {
            this.allowanceReason = new CreateOrEditAllowanceReasonDto();
            this.allowanceReason.id = allowanceReasonId;

            this.active = true;
        } else {
            this._allowanceReasonServiceProxy.getAllowanceReasonForEdit(allowanceReasonId).subscribe((result) => {
                this.allowanceReason = result.allowanceReason;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._allowanceReasonServiceProxy
            .createOrEdit(this.allowanceReason)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/allowanceReason']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._allowanceReasonServiceProxy
            .createOrEdit(this.allowanceReason)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.allowanceReason = new CreateOrEditAllowanceReasonDto();
            });
    }
}

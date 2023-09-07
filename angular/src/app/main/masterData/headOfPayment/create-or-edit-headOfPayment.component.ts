import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { HeadOfPaymentServiceProxy, CreateOrEditHeadOfPaymentDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-headOfPayment.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditHeadOfPaymentComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    headOfPayment: CreateOrEditHeadOfPaymentDto = new CreateOrEditHeadOfPaymentDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('HeadOfPayment'), '/app/main/masterData/headOfPayment'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _headOfPaymentServiceProxy: HeadOfPaymentServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(headOfPaymentId?: number): void {
        if (!headOfPaymentId) {
            this.headOfPayment = new CreateOrEditHeadOfPaymentDto();
            this.headOfPayment.id = headOfPaymentId;

            this.active = true;
        } else {
            this._headOfPaymentServiceProxy.getHeadOfPaymentForEdit(headOfPaymentId).subscribe((result) => {
                this.headOfPayment = result.headOfPayment;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._headOfPaymentServiceProxy
            .createOrEdit(this.headOfPayment)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/headOfPayment']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._headOfPaymentServiceProxy
            .createOrEdit(this.headOfPayment)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.headOfPayment = new CreateOrEditHeadOfPaymentDto();
            });
    }
}

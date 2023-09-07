import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { PaymentMeansServiceProxy, CreateOrEditPaymentMeansDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-paymentMeans.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditPaymentMeansComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    paymentMeans: CreateOrEditPaymentMeansDto = new CreateOrEditPaymentMeansDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('PaymentMeans'), '/app/main/masterData/paymentMeans'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _paymentMeansServiceProxy: PaymentMeansServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(paymentMeansId?: number): void {
        if (!paymentMeansId) {
            this.paymentMeans = new CreateOrEditPaymentMeansDto();
            this.paymentMeans.id = paymentMeansId;

            this.active = true;
        } else {
            this._paymentMeansServiceProxy.getPaymentMeansForEdit(paymentMeansId).subscribe((result) => {
                this.paymentMeans = result.paymentMeans;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._paymentMeansServiceProxy
            .createOrEdit(this.paymentMeans)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/paymentMeans']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._paymentMeansServiceProxy
            .createOrEdit(this.paymentMeans)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.paymentMeans = new CreateOrEditPaymentMeansDto();
            });
    }
}

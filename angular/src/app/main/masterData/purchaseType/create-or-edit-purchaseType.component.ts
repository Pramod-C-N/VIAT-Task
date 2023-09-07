import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { PurchaseTypeServiceProxy, CreateOrEditPurchaseTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-purchaseType.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditPurchaseTypeComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    purchaseType: CreateOrEditPurchaseTypeDto = new CreateOrEditPurchaseTypeDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('PurchaseType'), '/app/main/masterData/purchaseType'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _purchaseTypeServiceProxy: PurchaseTypeServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(purchaseTypeId?: number): void {
        if (!purchaseTypeId) {
            this.purchaseType = new CreateOrEditPurchaseTypeDto();
            this.purchaseType.id = purchaseTypeId;

            this.active = true;
        } else {
            this._purchaseTypeServiceProxy.getPurchaseTypeForEdit(purchaseTypeId).subscribe((result) => {
                this.purchaseType = result.purchaseType;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._purchaseTypeServiceProxy
            .createOrEdit(this.purchaseType)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/purchaseType']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._purchaseTypeServiceProxy
            .createOrEdit(this.purchaseType)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.purchaseType = new CreateOrEditPurchaseTypeDto();
            });
    }
}

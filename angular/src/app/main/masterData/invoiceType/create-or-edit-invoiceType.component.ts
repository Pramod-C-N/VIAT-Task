import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { InvoiceTypeServiceProxy, CreateOrEditInvoiceTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-invoiceType.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditInvoiceTypeComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    invoiceType: CreateOrEditInvoiceTypeDto = new CreateOrEditInvoiceTypeDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('InvoiceType'), '/app/main/masterData/invoiceType'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _invoiceTypeServiceProxy: InvoiceTypeServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(invoiceTypeId?: number): void {
        if (!invoiceTypeId) {
            this.invoiceType = new CreateOrEditInvoiceTypeDto();
            this.invoiceType.id = invoiceTypeId;

            this.active = true;
        } else {
            this._invoiceTypeServiceProxy.getInvoiceTypeForEdit(invoiceTypeId).subscribe((result) => {
                this.invoiceType = result.invoiceType;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._invoiceTypeServiceProxy
            .createOrEdit(this.invoiceType)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/invoiceType']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._invoiceTypeServiceProxy
            .createOrEdit(this.invoiceType)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.invoiceType = new CreateOrEditInvoiceTypeDto();
            });
    }
}

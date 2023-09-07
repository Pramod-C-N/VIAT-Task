import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { InvoiceCategoryServiceProxy, CreateOrEditInvoiceCategoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-invoiceCategory.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditInvoiceCategoryComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    invoiceCategory: CreateOrEditInvoiceCategoryDto = new CreateOrEditInvoiceCategoryDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('InvoiceCategory'), '/app/main/masterData/invoiceCategory'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _invoiceCategoryServiceProxy: InvoiceCategoryServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(invoiceCategoryId?: number): void {
        if (!invoiceCategoryId) {
            this.invoiceCategory = new CreateOrEditInvoiceCategoryDto();
            this.invoiceCategory.id = invoiceCategoryId;

            this.active = true;
        } else {
            this._invoiceCategoryServiceProxy.getInvoiceCategoryForEdit(invoiceCategoryId).subscribe((result) => {
                this.invoiceCategory = result.invoiceCategory;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._invoiceCategoryServiceProxy
            .createOrEdit(this.invoiceCategory)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/invoiceCategory']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._invoiceCategoryServiceProxy
            .createOrEdit(this.invoiceCategory)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.invoiceCategory = new CreateOrEditInvoiceCategoryDto();
            });
    }
}

import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TaxCategoryServiceProxy, CreateOrEditTaxCategoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-taxCategory.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditTaxCategoryComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    taxCategory: CreateOrEditTaxCategoryDto = new CreateOrEditTaxCategoryDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('TaxCategory'), '/app/main/masterData/taxCategory'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _taxCategoryServiceProxy: TaxCategoryServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(taxCategoryId?: number): void {
        if (!taxCategoryId) {
            this.taxCategory = new CreateOrEditTaxCategoryDto();
            this.taxCategory.id = taxCategoryId;

            this.active = true;
        } else {
            this._taxCategoryServiceProxy.getTaxCategoryForEdit(taxCategoryId).subscribe((result) => {
                this.taxCategory = result.taxCategory;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._taxCategoryServiceProxy
            .createOrEdit(this.taxCategory)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/taxCategory']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._taxCategoryServiceProxy
            .createOrEdit(this.taxCategory)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.taxCategory = new CreateOrEditTaxCategoryDto();
            });
    }
}

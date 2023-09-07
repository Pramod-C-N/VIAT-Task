import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TaxSubCategoryServiceProxy, CreateOrEditTaxSubCategoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-taxSubCategory.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditTaxSubCategoryComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    taxSubCategory: CreateOrEditTaxSubCategoryDto = new CreateOrEditTaxSubCategoryDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('TaxSubCategory'), '/app/main/masterData/taxSubCategory'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _taxSubCategoryServiceProxy: TaxSubCategoryServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(taxSubCategoryId?: number): void {
        if (!taxSubCategoryId) {
            this.taxSubCategory = new CreateOrEditTaxSubCategoryDto();
            this.taxSubCategory.id = taxSubCategoryId;

            this.active = true;
        } else {
            this._taxSubCategoryServiceProxy.getTaxSubCategoryForEdit(taxSubCategoryId).subscribe((result) => {
                this.taxSubCategory = result.taxSubCategory;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._taxSubCategoryServiceProxy
            .createOrEdit(this.taxSubCategory)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/taxSubCategory']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._taxSubCategoryServiceProxy
            .createOrEdit(this.taxSubCategory)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.taxSubCategory = new CreateOrEditTaxSubCategoryDto();
            });
    }
}

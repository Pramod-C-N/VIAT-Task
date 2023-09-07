import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    TransactionCategoryServiceProxy,
    CreateOrEditTransactionCategoryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-transactionCategory.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditTransactionCategoryComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    transactionCategory: CreateOrEditTransactionCategoryDto = new CreateOrEditTransactionCategoryDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('TransactionCategory'), '/app/main/masterData/transactionCategory'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _transactionCategoryServiceProxy: TransactionCategoryServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(transactionCategoryId?: number): void {
        if (!transactionCategoryId) {
            this.transactionCategory = new CreateOrEditTransactionCategoryDto();
            this.transactionCategory.id = transactionCategoryId;

            this.active = true;
        } else {
            this._transactionCategoryServiceProxy
                .getTransactionCategoryForEdit(transactionCategoryId)
                .subscribe((result) => {
                    this.transactionCategory = result.transactionCategory;

                    this.active = true;
                });
        }
    }

    save(): void {
        this.saving = true;

        this._transactionCategoryServiceProxy
            .createOrEdit(this.transactionCategory)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/transactionCategory']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._transactionCategoryServiceProxy
            .createOrEdit(this.transactionCategory)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.transactionCategory = new CreateOrEditTransactionCategoryDto();
            });
    }
}

﻿import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    TransactionCategoryServiceProxy,
    GetTransactionCategoryForViewDto,
    TransactionCategoryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-transactionCategory.component.html',
    animations: [appModuleAnimation()],
})
export class ViewTransactionCategoryComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetTransactionCategoryForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('TransactionCategory'), '/app/main/masterData/transactionCategory'),
        new BreadcrumbItem(this.l('TransactionCategory') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _transactionCategoryServiceProxy: TransactionCategoryServiceProxy
    ) {
        super(injector);
        this.item = new GetTransactionCategoryForViewDto();
        this.item.transactionCategory = new TransactionCategoryDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(transactionCategoryId: number): void {
        this._transactionCategoryServiceProxy
            .getTransactionCategoryForView(transactionCategoryId)
            .subscribe((result) => {
                this.item = result;
                this.active = true;
            });
    }
}

import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    InvoiceCategoryServiceProxy,
    GetInvoiceCategoryForViewDto,
    InvoiceCategoryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-invoiceCategory.component.html',
    animations: [appModuleAnimation()],
})
export class ViewInvoiceCategoryComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetInvoiceCategoryForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('InvoiceCategory'), '/app/main/masterData/invoiceCategory'),
        new BreadcrumbItem(this.l('InvoiceCategory') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _invoiceCategoryServiceProxy: InvoiceCategoryServiceProxy
    ) {
        super(injector);
        this.item = new GetInvoiceCategoryForViewDto();
        this.item.invoiceCategory = new InvoiceCategoryDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(invoiceCategoryId: number): void {
        this._invoiceCategoryServiceProxy.getInvoiceCategoryForView(invoiceCategoryId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

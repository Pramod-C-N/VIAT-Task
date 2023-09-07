import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    InvoiceTypeServiceProxy,
    GetInvoiceTypeForViewDto,
    InvoiceTypeDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-invoiceType.component.html',
    animations: [appModuleAnimation()],
})
export class ViewInvoiceTypeComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetInvoiceTypeForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('InvoiceType'), '/app/main/masterData/invoiceType'),
        new BreadcrumbItem(this.l('InvoiceType') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _invoiceTypeServiceProxy: InvoiceTypeServiceProxy
    ) {
        super(injector);
        this.item = new GetInvoiceTypeForViewDto();
        this.item.invoiceType = new InvoiceTypeDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(invoiceTypeId: number): void {
        this._invoiceTypeServiceProxy.getInvoiceTypeForView(invoiceTypeId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

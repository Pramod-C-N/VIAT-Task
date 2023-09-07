import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    PurchaseTypeServiceProxy,
    GetPurchaseTypeForViewDto,
    PurchaseTypeDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-purchaseType.component.html',
    animations: [appModuleAnimation()],
})
export class ViewPurchaseTypeComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetPurchaseTypeForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('PurchaseType'), '/app/main/masterData/purchaseType'),
        new BreadcrumbItem(this.l('PurchaseType') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _purchaseTypeServiceProxy: PurchaseTypeServiceProxy
    ) {
        super(injector);
        this.item = new GetPurchaseTypeForViewDto();
        this.item.purchaseType = new PurchaseTypeDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(purchaseTypeId: number): void {
        this._purchaseTypeServiceProxy.getPurchaseTypeForView(purchaseTypeId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

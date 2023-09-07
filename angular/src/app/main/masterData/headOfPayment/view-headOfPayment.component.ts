import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    HeadOfPaymentServiceProxy,
    GetHeadOfPaymentForViewDto,
    HeadOfPaymentDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-headOfPayment.component.html',
    animations: [appModuleAnimation()],
})
export class ViewHeadOfPaymentComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetHeadOfPaymentForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('HeadOfPayment'), '/app/main/masterData/headOfPayment'),
        new BreadcrumbItem(this.l('HeadOfPayment') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _headOfPaymentServiceProxy: HeadOfPaymentServiceProxy
    ) {
        super(injector);
        this.item = new GetHeadOfPaymentForViewDto();
        this.item.headOfPayment = new HeadOfPaymentDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(headOfPaymentId: number): void {
        this._headOfPaymentServiceProxy.getHeadOfPaymentForView(headOfPaymentId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

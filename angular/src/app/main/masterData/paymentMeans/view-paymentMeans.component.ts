import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    PaymentMeansServiceProxy,
    GetPaymentMeansForViewDto,
    PaymentMeansDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-paymentMeans.component.html',
    animations: [appModuleAnimation()],
})
export class ViewPaymentMeansComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetPaymentMeansForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('PaymentMeans'), '/app/main/masterData/paymentMeans'),
        new BreadcrumbItem(this.l('PaymentMeans') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _paymentMeansServiceProxy: PaymentMeansServiceProxy
    ) {
        super(injector);
        this.item = new GetPaymentMeansForViewDto();
        this.item.paymentMeans = new PaymentMeansDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(paymentMeansId: number): void {
        this._paymentMeansServiceProxy.getPaymentMeansForView(paymentMeansId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

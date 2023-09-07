import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    BusinessTurnoverSlabServiceProxy,
    GetBusinessTurnoverSlabForViewDto,
    BusinessTurnoverSlabDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-businessTurnoverSlab.component.html',
    animations: [appModuleAnimation()],
})
export class ViewBusinessTurnoverSlabComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetBusinessTurnoverSlabForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('BusinessTurnoverSlab'), '/app/main/masterData/businessTurnoverSlab'),
        new BreadcrumbItem(this.l('BusinessTurnoverSlab') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _businessTurnoverSlabServiceProxy: BusinessTurnoverSlabServiceProxy
    ) {
        super(injector);
        this.item = new GetBusinessTurnoverSlabForViewDto();
        this.item.businessTurnoverSlab = new BusinessTurnoverSlabDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(businessTurnoverSlabId: number): void {
        this._businessTurnoverSlabServiceProxy
            .getBusinessTurnoverSlabForView(businessTurnoverSlabId)
            .subscribe((result) => {
                this.item = result;
                this.active = true;
            });
    }
}

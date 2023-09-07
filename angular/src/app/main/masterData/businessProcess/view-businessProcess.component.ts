import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    BusinessProcessServiceProxy,
    GetBusinessProcessForViewDto,
    BusinessProcessDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-businessProcess.component.html',
    animations: [appModuleAnimation()],
})
export class ViewBusinessProcessComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetBusinessProcessForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('BusinessProcess'), '/app/main/masterData/businessProcess'),
        new BreadcrumbItem(this.l('BusinessProcess') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _businessProcessServiceProxy: BusinessProcessServiceProxy
    ) {
        super(injector);
        this.item = new GetBusinessProcessForViewDto();
        this.item.businessProcess = new BusinessProcessDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(businessProcessId: number): void {
        this._businessProcessServiceProxy.getBusinessProcessForView(businessProcessId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    BusinessOperationalModelServiceProxy,
    GetBusinessOperationalModelForViewDto,
    BusinessOperationalModelDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-businessOperationalModel.component.html',
    animations: [appModuleAnimation()],
})
export class ViewBusinessOperationalModelComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetBusinessOperationalModelForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('BusinessOperationalModel'), '/app/main/masterData/businessOperationalModel'),
        new BreadcrumbItem(this.l('BusinessOperationalModel') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _businessOperationalModelServiceProxy: BusinessOperationalModelServiceProxy
    ) {
        super(injector);
        this.item = new GetBusinessOperationalModelForViewDto();
        this.item.businessOperationalModel = new BusinessOperationalModelDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(businessOperationalModelId: number): void {
        this._businessOperationalModelServiceProxy
            .getBusinessOperationalModelForView(businessOperationalModelId)
            .subscribe((result) => {
                this.item = result;
                this.active = true;
            });
    }
}

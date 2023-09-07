import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    TenantTypeServiceProxy,
    GetTenantTypeForViewDto,
    TenantTypeDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-tenantType.component.html',
    animations: [appModuleAnimation()],
})
export class ViewTenantTypeComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetTenantTypeForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('TenantType'), '/app/main/masterData/tenantType'),
        new BreadcrumbItem(this.l('TenantType') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _tenantTypeServiceProxy: TenantTypeServiceProxy
    ) {
        super(injector);
        this.item = new GetTenantTypeForViewDto();
        this.item.tenantType = new TenantTypeDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(tenantTypeId: number): void {
        this._tenantTypeServiceProxy.getTenantTypeForView(tenantTypeId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

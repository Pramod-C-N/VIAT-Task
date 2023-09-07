import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    ReasonCNDNServiceProxy,
    GetReasonCNDNForViewDto,
    ReasonCNDNDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-reasonCNDN.component.html',
    animations: [appModuleAnimation()],
})
export class ViewReasonCNDNComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetReasonCNDNForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('ReasonCNDN'), '/app/main/masterData/reasonCNDN'),
        new BreadcrumbItem(this.l('ReasonCNDN') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _reasonCNDNServiceProxy: ReasonCNDNServiceProxy
    ) {
        super(injector);
        this.item = new GetReasonCNDNForViewDto();
        this.item.reasonCNDN = new ReasonCNDNDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(reasonCNDNId: number): void {
        this._reasonCNDNServiceProxy.getReasonCNDNForView(reasonCNDNId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    ExemptionReasonServiceProxy,
    GetExemptionReasonForViewDto,
    ExemptionReasonDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-exemptionReason.component.html',
    animations: [appModuleAnimation()],
})
export class ViewExemptionReasonComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetExemptionReasonForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('ExemptionReason'), '/app/main/masterData/exemptionReason'),
        new BreadcrumbItem(this.l('ExemptionReason') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _exemptionReasonServiceProxy: ExemptionReasonServiceProxy
    ) {
        super(injector);
        this.item = new GetExemptionReasonForViewDto();
        this.item.exemptionReason = new ExemptionReasonDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(exemptionReasonId: number): void {
        this._exemptionReasonServiceProxy.getExemptionReasonForView(exemptionReasonId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

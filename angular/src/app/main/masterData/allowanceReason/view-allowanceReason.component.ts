import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    AllowanceReasonServiceProxy,
    GetAllowanceReasonForViewDto,
    AllowanceReasonDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-allowanceReason.component.html',
    animations: [appModuleAnimation()],
})
export class ViewAllowanceReasonComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetAllowanceReasonForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('AllowanceReason'), '/app/main/masterData/allowanceReason'),
        new BreadcrumbItem(this.l('AllowanceReason') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _allowanceReasonServiceProxy: AllowanceReasonServiceProxy
    ) {
        super(injector);
        this.item = new GetAllowanceReasonForViewDto();
        this.item.allowanceReason = new AllowanceReasonDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(allowanceReasonId: number): void {
        this._allowanceReasonServiceProxy.getAllowanceReasonForView(allowanceReasonId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

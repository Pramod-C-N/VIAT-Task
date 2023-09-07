import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    DesignationServiceProxy,
    GetDesignationForViewDto,
    DesignationDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-designation.component.html',
    animations: [appModuleAnimation()],
})
export class ViewDesignationComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetDesignationForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Designation'), '/app/main/masterData/designation'),
        new BreadcrumbItem(this.l('Designation') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _designationServiceProxy: DesignationServiceProxy
    ) {
        super(injector);
        this.item = new GetDesignationForViewDto();
        this.item.designation = new DesignationDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(designationId: number): void {
        this._designationServiceProxy.getDesignationForView(designationId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

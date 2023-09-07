import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    ErrorGroupServiceProxy,
    GetErrorGroupForViewDto,
    ErrorGroupDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-errorGroup.component.html',
    animations: [appModuleAnimation()],
})
export class ViewErrorGroupComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetErrorGroupForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('ErrorGroup'), '/app/main/masterData/errorGroup'),
        new BreadcrumbItem(this.l('ErrorGroup') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _errorGroupServiceProxy: ErrorGroupServiceProxy
    ) {
        super(injector);
        this.item = new GetErrorGroupForViewDto();
        this.item.errorGroup = new ErrorGroupDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(errorGroupId: number): void {
        this._errorGroupServiceProxy.getErrorGroupForView(errorGroupId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

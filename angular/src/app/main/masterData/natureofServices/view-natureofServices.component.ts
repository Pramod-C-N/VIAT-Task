import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    NatureofServicesServiceProxy,
    GetNatureofServicesForViewDto,
    NatureofServicesDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-natureofServices.component.html',
    animations: [appModuleAnimation()],
})
export class ViewNatureofServicesComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetNatureofServicesForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('NatureofServices'), '/app/main/masterData/natureofServices'),
        new BreadcrumbItem(this.l('NatureofServices') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _natureofServicesServiceProxy: NatureofServicesServiceProxy
    ) {
        super(injector);
        this.item = new GetNatureofServicesForViewDto();
        this.item.natureofServices = new NatureofServicesDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(natureofServicesId: number): void {
        this._natureofServicesServiceProxy.getNatureofServicesForView(natureofServicesId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

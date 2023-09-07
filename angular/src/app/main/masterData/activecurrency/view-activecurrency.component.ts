import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    ActivecurrencyServiceProxy,
    GetActivecurrencyForViewDto,
    ActivecurrencyDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-activecurrency.component.html',
    animations: [appModuleAnimation()],
})
export class ViewActivecurrencyComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetActivecurrencyForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Activecurrency'), '/app/main/masterData/activecurrency'),
        new BreadcrumbItem(this.l('Activecurrency') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _activecurrencyServiceProxy: ActivecurrencyServiceProxy
    ) {
        super(injector);
        this.item = new GetActivecurrencyForViewDto();
        this.item.activecurrency = new ActivecurrencyDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(activecurrencyId: number): void {
        this._activecurrencyServiceProxy.getActivecurrencyForView(activecurrencyId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

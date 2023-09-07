import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    AffiliationServiceProxy,
    GetAffiliationForViewDto,
    AffiliationDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-affiliation.component.html',
    animations: [appModuleAnimation()],
})
export class ViewAffiliationComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetAffiliationForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Affiliation'), '/app/main/masterData/affiliation'),
        new BreadcrumbItem(this.l('Affiliation') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _affiliationServiceProxy: AffiliationServiceProxy
    ) {
        super(injector);
        this.item = new GetAffiliationForViewDto();
        this.item.affiliation = new AffiliationDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(affiliationId: number): void {
        this._affiliationServiceProxy.getAffiliationForView(affiliationId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

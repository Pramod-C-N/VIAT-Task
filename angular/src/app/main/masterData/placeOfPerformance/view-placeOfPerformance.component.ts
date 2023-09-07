import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    PlaceOfPerformanceServiceProxy,
    GetPlaceOfPerformanceForViewDto,
    PlaceOfPerformanceDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-placeOfPerformance.component.html',
    animations: [appModuleAnimation()],
})
export class ViewPlaceOfPerformanceComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetPlaceOfPerformanceForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('PlaceOfPerformance'), '/app/main/masterData/placeOfPerformance'),
        new BreadcrumbItem(this.l('PlaceOfPerformance') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _placeOfPerformanceServiceProxy: PlaceOfPerformanceServiceProxy
    ) {
        super(injector);
        this.item = new GetPlaceOfPerformanceForViewDto();
        this.item.placeOfPerformance = new PlaceOfPerformanceDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(placeOfPerformanceId: number): void {
        this._placeOfPerformanceServiceProxy.getPlaceOfPerformanceForView(placeOfPerformanceId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

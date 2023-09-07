import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    UnitOfMeasurementServiceProxy,
    GetUnitOfMeasurementForViewDto,
    UnitOfMeasurementDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-unitOfMeasurement.component.html',
    animations: [appModuleAnimation()],
})
export class ViewUnitOfMeasurementComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetUnitOfMeasurementForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('UnitOfMeasurement'), '/app/main/masterData/unitOfMeasurement'),
        new BreadcrumbItem(this.l('UnitOfMeasurement') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _unitOfMeasurementServiceProxy: UnitOfMeasurementServiceProxy
    ) {
        super(injector);
        this.item = new GetUnitOfMeasurementForViewDto();
        this.item.unitOfMeasurement = new UnitOfMeasurementDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(unitOfMeasurementId: number): void {
        this._unitOfMeasurementServiceProxy.getUnitOfMeasurementForView(unitOfMeasurementId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

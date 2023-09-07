import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { SectorServiceProxy, GetSectorForViewDto, SectorDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-sector.component.html',
    animations: [appModuleAnimation()],
})
export class ViewSectorComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetSectorForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Sector'), '/app/main/masterData/sector'),
        new BreadcrumbItem(this.l('Sector') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _sectorServiceProxy: SectorServiceProxy
    ) {
        super(injector);
        this.item = new GetSectorForViewDto();
        this.item.sector = new SectorDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(sectorId: number): void {
        this._sectorServiceProxy.getSectorForView(sectorId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

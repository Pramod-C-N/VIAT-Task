import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { SectorServiceProxy, CreateOrEditSectorDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-sector.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditSectorComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    sector: CreateOrEditSectorDto = new CreateOrEditSectorDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Sector'), '/app/main/masterData/sector'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _sectorServiceProxy: SectorServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(sectorId?: number): void {
        if (!sectorId) {
            this.sector = new CreateOrEditSectorDto();
            this.sector.id = sectorId;

            this.active = true;
        } else {
            this._sectorServiceProxy.getSectorForEdit(sectorId).subscribe((result) => {
                this.sector = result.sector;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._sectorServiceProxy
            .createOrEdit(this.sector)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/sector']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._sectorServiceProxy
            .createOrEdit(this.sector)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.sector = new CreateOrEditSectorDto();
            });
    }
}

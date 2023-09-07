import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TenantTypeServiceProxy, CreateOrEditTenantTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-tenantType.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditTenantTypeComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    tenantType: CreateOrEditTenantTypeDto = new CreateOrEditTenantTypeDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('TenantType'), '/app/main/masterData/tenantType'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _tenantTypeServiceProxy: TenantTypeServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(tenantTypeId?: number): void {
        if (!tenantTypeId) {
            this.tenantType = new CreateOrEditTenantTypeDto();
            this.tenantType.id = tenantTypeId;

            this.active = true;
        } else {
            this._tenantTypeServiceProxy.getTenantTypeForEdit(tenantTypeId).subscribe((result) => {
                this.tenantType = result.tenantType;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._tenantTypeServiceProxy
            .createOrEdit(this.tenantType)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/tenantType']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._tenantTypeServiceProxy
            .createOrEdit(this.tenantType)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.tenantType = new CreateOrEditTenantTypeDto();
            });
    }
}

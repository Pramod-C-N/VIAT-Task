import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ReasonCNDNServiceProxy, CreateOrEditReasonCNDNDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-reasonCNDN.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditReasonCNDNComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    reasonCNDN: CreateOrEditReasonCNDNDto = new CreateOrEditReasonCNDNDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('ReasonCNDN'), '/app/main/masterData/reasonCNDN'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _reasonCNDNServiceProxy: ReasonCNDNServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(reasonCNDNId?: number): void {
        if (!reasonCNDNId) {
            this.reasonCNDN = new CreateOrEditReasonCNDNDto();
            this.reasonCNDN.id = reasonCNDNId;

            this.active = true;
        } else {
            this._reasonCNDNServiceProxy.getReasonCNDNForEdit(reasonCNDNId).subscribe((result) => {
                this.reasonCNDN = result.reasonCNDN;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._reasonCNDNServiceProxy
            .createOrEdit(this.reasonCNDN)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/reasonCNDN']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._reasonCNDNServiceProxy
            .createOrEdit(this.reasonCNDN)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.reasonCNDN = new CreateOrEditReasonCNDNDto();
            });
    }
}

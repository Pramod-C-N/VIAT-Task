import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ActivecurrencyServiceProxy, CreateOrEditActivecurrencyDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-activecurrency.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditActivecurrencyComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    activecurrency: CreateOrEditActivecurrencyDto = new CreateOrEditActivecurrencyDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Activecurrency'), '/app/main/masterData/activecurrency'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _activecurrencyServiceProxy: ActivecurrencyServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(activecurrencyId?: number): void {
        if (!activecurrencyId) {
            this.activecurrency = new CreateOrEditActivecurrencyDto();
            this.activecurrency.id = activecurrencyId;

            this.active = true;
        } else {
            this._activecurrencyServiceProxy.getActivecurrencyForEdit(activecurrencyId).subscribe((result) => {
                this.activecurrency = result.activecurrency;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._activecurrencyServiceProxy
            .createOrEdit(this.activecurrency)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/activecurrency']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._activecurrencyServiceProxy
            .createOrEdit(this.activecurrency)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.activecurrency = new CreateOrEditActivecurrencyDto();
            });
    }
}

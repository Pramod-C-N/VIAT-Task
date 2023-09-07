import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CountryServiceProxy, CreateOrEditCountryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-country.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditCountryComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    country: CreateOrEditCountryDto = new CreateOrEditCountryDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Country'), '/app/main/masterData/country'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _countryServiceProxy: CountryServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(countryId?: number): void {
        if (!countryId) {
            this.country = new CreateOrEditCountryDto();
            this.country.id = countryId;

            this.active = true;
        } else {
            this._countryServiceProxy.getCountryForEdit(countryId).subscribe((result) => {
                this.country = result.country;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._countryServiceProxy
            .createOrEdit(this.country)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/country']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._countryServiceProxy
            .createOrEdit(this.country)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.country = new CreateOrEditCountryDto();
            });
    }
}

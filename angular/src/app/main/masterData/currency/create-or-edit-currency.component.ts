import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CurrencyServiceProxy, CreateOrEditCurrencyDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-currency.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditCurrencyComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    currency: CreateOrEditCurrencyDto = new CreateOrEditCurrencyDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Currency'), '/app/main/masterData/currency'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _currencyServiceProxy: CurrencyServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(currencyId?: number): void {
        if (!currencyId) {
            this.currency = new CreateOrEditCurrencyDto();
            this.currency.id = currencyId;

            this.active = true;
        } else {
            this._currencyServiceProxy.getCurrencyForEdit(currencyId).subscribe((result) => {
                this.currency = result.currency;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._currencyServiceProxy
            .createOrEdit(this.currency)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/currency']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._currencyServiceProxy
            .createOrEdit(this.currency)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.currency = new CreateOrEditCurrencyDto();
            });
    }
}

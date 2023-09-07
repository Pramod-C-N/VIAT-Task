import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { FinancialYearServiceProxy, CreateOrEditFinancialYearDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-financialYear.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditFinancialYearComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    financialYear: CreateOrEditFinancialYearDto = new CreateOrEditFinancialYearDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('FinancialYear'), '/app/main/masterData/financialYear'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _financialYearServiceProxy: FinancialYearServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(financialYearId?: number): void {
        if (!financialYearId) {
            this.financialYear = new CreateOrEditFinancialYearDto();
            this.financialYear.id = financialYearId;

            this.active = true;
        } else {
            this._financialYearServiceProxy.getFinancialYearForEdit(financialYearId).subscribe((result) => {
                this.financialYear = result.financialYear;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._financialYearServiceProxy
            .createOrEdit(this.financialYear)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/financialYear']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._financialYearServiceProxy
            .createOrEdit(this.financialYear)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.financialYear = new CreateOrEditFinancialYearDto();
            });
    }
}

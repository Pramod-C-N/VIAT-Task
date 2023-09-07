import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    FinancialYearServiceProxy,
    GetFinancialYearForViewDto,
    FinancialYearDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-financialYear.component.html',
    animations: [appModuleAnimation()],
})
export class ViewFinancialYearComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetFinancialYearForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('FinancialYear'), '/app/main/masterData/financialYear'),
        new BreadcrumbItem(this.l('FinancialYear') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _financialYearServiceProxy: FinancialYearServiceProxy
    ) {
        super(injector);
        this.item = new GetFinancialYearForViewDto();
        this.item.financialYear = new FinancialYearDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(financialYearId: number): void {
        this._financialYearServiceProxy.getFinancialYearForView(financialYearId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

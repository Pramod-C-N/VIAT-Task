import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    TaxCategoryServiceProxy,
    GetTaxCategoryForViewDto,
    TaxCategoryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-taxCategory.component.html',
    animations: [appModuleAnimation()],
})
export class ViewTaxCategoryComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetTaxCategoryForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('TaxCategory'), '/app/main/masterData/taxCategory'),
        new BreadcrumbItem(this.l('TaxCategory') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _taxCategoryServiceProxy: TaxCategoryServiceProxy
    ) {
        super(injector);
        this.item = new GetTaxCategoryForViewDto();
        this.item.taxCategory = new TaxCategoryDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(taxCategoryId: number): void {
        this._taxCategoryServiceProxy.getTaxCategoryForView(taxCategoryId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

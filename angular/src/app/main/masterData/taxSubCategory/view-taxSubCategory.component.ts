import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    TaxSubCategoryServiceProxy,
    GetTaxSubCategoryForViewDto,
    TaxSubCategoryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-taxSubCategory.component.html',
    animations: [appModuleAnimation()],
})
export class ViewTaxSubCategoryComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetTaxSubCategoryForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('TaxSubCategory'), '/app/main/masterData/taxSubCategory'),
        new BreadcrumbItem(this.l('TaxSubCategory') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _taxSubCategoryServiceProxy: TaxSubCategoryServiceProxy
    ) {
        super(injector);
        this.item = new GetTaxSubCategoryForViewDto();
        this.item.taxSubCategory = new TaxSubCategoryDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(taxSubCategoryId: number): void {
        this._taxSubCategoryServiceProxy.getTaxSubCategoryForView(taxSubCategoryId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

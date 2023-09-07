import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { CurrencyServiceProxy, GetCurrencyForViewDto, CurrencyDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-currency.component.html',
    animations: [appModuleAnimation()],
})
export class ViewCurrencyComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetCurrencyForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Currency'), '/app/main/masterData/currency'),
        new BreadcrumbItem(this.l('Currency') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _currencyServiceProxy: CurrencyServiceProxy
    ) {
        super(injector);
        this.item = new GetCurrencyForViewDto();
        this.item.currency = new CurrencyDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(currencyId: number): void {
        this._currencyServiceProxy.getCurrencyForView(currencyId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

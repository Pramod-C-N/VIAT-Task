import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { CountryServiceProxy, GetCountryForViewDto, CountryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-country.component.html',
    animations: [appModuleAnimation()],
})
export class ViewCountryComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetCountryForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Country'), '/app/main/masterData/country'),
        new BreadcrumbItem(this.l('Country') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _countryServiceProxy: CountryServiceProxy
    ) {
        super(injector);
        this.item = new GetCountryForViewDto();
        this.item.country = new CountryDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(countryId: number): void {
        this._countryServiceProxy.getCountryForView(countryId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

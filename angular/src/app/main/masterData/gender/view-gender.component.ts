import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GenderServiceProxy, GetGenderForViewDto, GenderDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-gender.component.html',
    animations: [appModuleAnimation()],
})
export class ViewGenderComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetGenderForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Gender'), '/app/main/masterData/gender'),
        new BreadcrumbItem(this.l('Gender') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _genderServiceProxy: GenderServiceProxy
    ) {
        super(injector);
        this.item = new GetGenderForViewDto();
        this.item.gender = new GenderDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(genderId: number): void {
        this._genderServiceProxy.getGenderForView(genderId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

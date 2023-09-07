import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    ConstitutionServiceProxy,
    GetConstitutionForViewDto,
    ConstitutionDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-constitution.component.html',
    animations: [appModuleAnimation()],
})
export class ViewConstitutionComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetConstitutionForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Constitution'), '/app/main/masterData/constitution'),
        new BreadcrumbItem(this.l('Constitution') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _constitutionServiceProxy: ConstitutionServiceProxy
    ) {
        super(injector);
        this.item = new GetConstitutionForViewDto();
        this.item.constitution = new ConstitutionDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(constitutionId: number): void {
        this._constitutionServiceProxy.getConstitutionForView(constitutionId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

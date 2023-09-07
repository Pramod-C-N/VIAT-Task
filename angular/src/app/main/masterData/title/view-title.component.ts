import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { TitleServiceProxy, GetTitleForViewDto, TitleDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-title.component.html',
    animations: [appModuleAnimation()],
})
export class ViewTitleComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetTitleForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Title'), '/app/main/masterData/title'),
        new BreadcrumbItem(this.l('Title') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _titleServiceProxy: TitleServiceProxy
    ) {
        super(injector);
        this.item = new GetTitleForViewDto();
        this.item.title = new TitleDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(titleId: number): void {
        this._titleServiceProxy.getTitleForView(titleId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

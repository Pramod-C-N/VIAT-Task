import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ErrorTypeServiceProxy, GetErrorTypeForViewDto, ErrorTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-errorType.component.html',
    animations: [appModuleAnimation()],
})
export class ViewErrorTypeComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetErrorTypeForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('ErrorType'), '/app/main/masterData/errorType'),
        new BreadcrumbItem(this.l('ErrorType') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _errorTypeServiceProxy: ErrorTypeServiceProxy
    ) {
        super(injector);
        this.item = new GetErrorTypeForViewDto();
        this.item.errorType = new ErrorTypeDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(errorTypeId: number): void {
        this._errorTypeServiceProxy.getErrorTypeForView(errorTypeId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    DocumentMasterServiceProxy,
    GetDocumentMasterForViewDto,
    DocumentMasterDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-documentMaster.component.html',
    animations: [appModuleAnimation()],
})
export class ViewDocumentMasterComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetDocumentMasterForViewDto;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('DocumentMaster'), '/app/main/masterData/documentMaster'),
        new BreadcrumbItem(this.l('DocumentMaster') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _documentMasterServiceProxy: DocumentMasterServiceProxy
    ) {
        super(injector);
        this.item = new GetDocumentMasterForViewDto();
        this.item.documentMaster = new DocumentMasterDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(documentMasterId: number): void {
        this._documentMasterServiceProxy.getDocumentMasterForView(documentMasterId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}

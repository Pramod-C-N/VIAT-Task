import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { DocumentMasterServiceProxy, CreateOrEditDocumentMasterDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-documentMaster.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditDocumentMasterComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    documentMaster: CreateOrEditDocumentMasterDto = new CreateOrEditDocumentMasterDto();

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('DocumentMaster'), '/app/main/masterData/documentMaster'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _documentMasterServiceProxy: DocumentMasterServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(documentMasterId?: number): void {
        if (!documentMasterId) {
            this.documentMaster = new CreateOrEditDocumentMasterDto();
            this.documentMaster.id = documentMasterId;

            this.active = true;
        } else {
            this._documentMasterServiceProxy.getDocumentMasterForEdit(documentMasterId).subscribe((result) => {
                this.documentMaster = result.documentMaster;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._documentMasterServiceProxy
            .createOrEdit(this.documentMaster)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/documentMaster']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._documentMasterServiceProxy
            .createOrEdit(this.documentMaster)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.documentMaster = new CreateOrEditDocumentMasterDto();
            });
    }
}

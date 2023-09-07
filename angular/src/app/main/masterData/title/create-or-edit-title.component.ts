import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TitleServiceProxy, CreateOrEditTitleDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-title.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditTitleComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    title: CreateOrEditTitleDto = new CreateOrEditTitleDto();

    breadcrumbs: BreadcrumbItem[] = [
       // new BreadcrumbItem(this.l('Title'), '/app/main/masterData/title'),
       // new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _titleServiceProxy: TitleServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(titleId?: number): void {
        if (!titleId) {
            this.title = new CreateOrEditTitleDto();
            this.title.id = titleId;

            this.active = true;
        } else {
            this._titleServiceProxy.getTitleForEdit(titleId).subscribe((result) => {
                this.title = result.title;

                this.active = true;
            });
        }
    }

    save(): void {
        this.saving = true;

        this._titleServiceProxy
            .createOrEdit(this.title)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/masterData/title']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._titleServiceProxy
            .createOrEdit(this.title)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.title = new CreateOrEditTitleDto();
            });
    }
}

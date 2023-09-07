import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModuleServiceProxy, ModuleDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditModuleModalComponent } from './create-or-edit-module-modal.component';

import { ViewModuleModalComponent } from './view-module-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './module.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ModuleComponent extends AppComponentBase {
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('createOrEditModuleModal', { static: true }) createOrEditModuleModal: CreateOrEditModuleModalComponent;
    @ViewChild('viewModuleModal', { static: true }) viewModuleModal: ViewModuleModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxModuleIdFilter: number;
    maxModuleIdFilterEmpty: number;
    minModuleIdFilter: number;
    minModuleIdFilterEmpty: number;
    moduleNameFilter = '';
    maxStatusFilter: number;
    maxStatusFilterEmpty: number;
    minStatusFilter: number;
    minStatusFilterEmpty: number;
    maxTenantIdFilter: number;
    maxTenantIdFilterEmpty: number;
    minTenantIdFilter: number;
    minTenantIdFilterEmpty: number;

    _entityTypeFullName = 'vita.MasterData.Module';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _moduleServiceProxy: ModuleServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.entityHistoryEnabled = this.setIsEntityHistoryEnabled();
    }

    private setIsEntityHistoryEnabled(): boolean {
        let customSettings = (abp as any).custom;
        return (
            this.isGrantedAny('Pages.Administration.AuditLogs') &&
            customSettings.EntityHistory &&
            customSettings.EntityHistory.isEnabled &&
            _filter(
                customSettings.EntityHistory.enabledEntities,
                (entityType) => entityType === this._entityTypeFullName
            ).length === 1
        );
    }

    getModule(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._moduleServiceProxy
            .getAll(
                this.filterText,
                this.maxModuleIdFilter == null ? this.maxModuleIdFilterEmpty : this.maxModuleIdFilter,
                this.minModuleIdFilter == null ? this.minModuleIdFilterEmpty : this.minModuleIdFilter,
                this.moduleNameFilter,
                this.maxStatusFilter == null ? this.maxStatusFilterEmpty : this.maxStatusFilter,
                this.minStatusFilter == null ? this.minStatusFilterEmpty : this.minStatusFilter,
                this.maxTenantIdFilter == null ? this.maxTenantIdFilterEmpty : this.maxTenantIdFilter,
                this.minTenantIdFilter == null ? this.minTenantIdFilterEmpty : this.minTenantIdFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createModule(): void {
        this.createOrEditModuleModal.show();
    }

    showHistory(module: ModuleDto): void {
        this.entityTypeHistoryModal.show({
            entityId: module.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: '',
        });
    }

    deleteModule(module: ModuleDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._moduleServiceProxy.delete(module.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxModuleIdFilter = this.maxModuleIdFilterEmpty;
        this.minModuleIdFilter = this.maxModuleIdFilterEmpty;
        this.moduleNameFilter = '';
        this.maxStatusFilter = this.maxStatusFilterEmpty;
        this.minStatusFilter = this.maxStatusFilterEmpty;
        this.maxTenantIdFilter = this.maxTenantIdFilterEmpty;
        this.minTenantIdFilter = this.maxTenantIdFilterEmpty;

        this.getModule();
    }
}

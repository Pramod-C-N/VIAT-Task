﻿import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CurrencyServiceProxy, CurrencyDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';

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
    templateUrl: './currency.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class CurrencyComponent extends AppComponentBase {
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    descriptionFilter = '';
    codeFilter = '';
    numericCodeFilter = '';
    minorUnitFilter = '';
    countryFilter = '';
    isActiveFilter = -1;

    _entityTypeFullName = 'vita.MasterData.Currency';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _currencyServiceProxy: CurrencyServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _router: Router,
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

    getCurrency(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._currencyServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.descriptionFilter,
                this.codeFilter,
                this.numericCodeFilter,
                this.minorUnitFilter,
                this.countryFilter,
                this.isActiveFilter,
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

    createCurrency(): void {
        this._router.navigate(['/app/main/masterData/currency/createOrEdit']);
    }

    showHistory(currency: CurrencyDto): void {
        this.entityTypeHistoryModal.show({
            entityId: currency.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: '',
        });
    }

    deleteCurrency(currency: CurrencyDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._currencyServiceProxy.delete(currency.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._currencyServiceProxy
            .getCurrencyToExcel(
                this.filterText,
                this.nameFilter,
                this.descriptionFilter,
                this.codeFilter,
                this.numericCodeFilter,
                this.minorUnitFilter,
                this.countryFilter,
                this.isActiveFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.descriptionFilter = '';
        this.codeFilter = '';
        this.numericCodeFilter = '';
        this.minorUnitFilter = '';
        this.countryFilter = '';
        this.isActiveFilter = -1;

        this.getCurrency();
    }
}

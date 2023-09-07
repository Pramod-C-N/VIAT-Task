import { Component, ElementRef, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective,Validators } from '@angular/forms';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTime } from 'luxon';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { finalize } from 'rxjs/operators';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { GlobalConstsCustomService } from '@shared/customService/global-consts-service';
import { AnyARecord } from 'dns';

@Component({
    selector: 'filter-component',
    templateUrl: './filter.component.html',
    styleUrls: ['./filter.component.css'],
})
export class filterComponent {
    @Input() fields: any;
    formName: FormGroup;
    searchText: string;
    @Output() searchData = new EventEmitter<any>();
    @Output() clearData = new EventEmitter<string>();
    @Output() filterCriteria = new EventEmitter<any>();


    filterName='any';
    public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
 
    constructor(private formgroupDirective: FormGroupDirective,
        private _dateTimeService: DateTimeService,
        private el: ElementRef,
        private fb: FormBuilder,) {            
    }


    ngOnInit() {
        this.formName = this.formgroupDirective.control;
    }

    ngAfterViewInit() {}

    search(){
        this.searchData.emit({'filterName':this.filterName,'searchText':this.searchText,'dateRange':this.dateRange});
    }

    clear(){
        this.clearData.emit();
        this.searchText='';
        this.filterName='any';
    }


}

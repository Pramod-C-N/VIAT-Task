import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { ModalDirective } from 'ngx-bootstrap/modal';

type Error = {
    code: number;
    message: string;
    type: number;
    field?: string;
};

type FileUploadResult = {
    id: number;
    status: string;
    lastModifiedBy: string;
    lastModifiedOn: string;
    errors: Error[];
    data: {};
};

type ErrorView = {
    code: number;
    message: string;
    type: number;
    field?: string;
    recordId: number[];
};

@Component({
    selector: 'file-upload-output',
    templateUrl: './fileUploadOutput.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
    styleUrls: ['./fileUploadOutput.component.less'],
})
export class FileUploadOutputComponent extends AppComponentBase {
    result: FileUploadResult[] = [
        {
            id: 1,
            status: 'Success',
            lastModifiedBy: 'Shubham',
            lastModifiedOn: '18/5/2023 12:11',
            errors: [],
            data: { invoiceNumber: 123, VatRate: '15' },
        },
        {
            id: 2,
            status: 'Failure',
            lastModifiedBy: 'Shubham',
            lastModifiedOn: '18/5/2023 12:11',
            errors: [
                { code: 456, message: 'Invalid vat rate', type: 1, field: 'VatRate' },
                { code: 7, message: 'Invalid issue date', type: 2 },
                { code: 90, message: 'Vat amount is incorrect', type: 3 },
            ],
            data: { invoiceNumber: 123, VatRate: '15' },
        },
        {
            id: 3,
            status: 'Success',
            lastModifiedBy: 'Shubham',
            lastModifiedOn: '18/5/2023 12:11',
            errors: [],
            data: { invoiceNumber: 123, VatRate: '15' },
        },
        {
            id: 4,
            status: 'Success',
            lastModifiedBy: 'Shubham',
            lastModifiedOn: '18/5/2023 12:11',
            errors: [],
            data: { invoiceNumber: 123, VatRate: '15' },
        },
    ];

    errorView: ErrorView[] = [];
    filteredResult: FileUploadResult[] = [];
    filterType: number = 0;
    selectedData: any = {
        errors: [],
        data: [],
        id: null,
    };

    data: string[] = [];

    @ViewChild('modal', { static: false }) modal: ModalDirective;

    constructor(
        injector: Injector,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.prepareErrorView();
    }

    doubleClickTimer: any;

    prepareErrorView() {
      //this.errorView.
    }

    checkIfError(field: any) {
        return this.result.findIndex((a) => a.errors.findIndex((e) => e.field == field) != -1) != -1;
    }
    viewRecords(type: number) {
        if (type != 0)
            this.filteredResult = this.result.filter((a) => a.errors.filter((e) => e.type == type).length > 0);
        else this.filteredResult = this.result;
    }

    viewErrors(id: number): void {
        this.doubleClickTimer = setTimeout(() => {
            this.getErrors(id);
        }, 300);
    }
    viewData(id: number): void {
        clearTimeout(this.doubleClickTimer);
        this.doubleClickTimer = undefined;
        this.getData(id);
    }
    getErrors(id: number) {
        if (!this.doubleClickTimer) return;
        this.selectedData.errors = this.result.find((a) => a.id == id)?.errors.sort((a, b) => a.type - b.type) || [];
        this.selectedData.id = id;
        console.log(this.selectedData);
    }
    getData(id: number) {
        this.data = [];
        this.selectedData.data = this.result.find((a) => a.id == id)?.data;
        this.data = Object.keys(this.selectedData.data);
        this.selectedData.id = id;
        this.modal.show();
    }
}

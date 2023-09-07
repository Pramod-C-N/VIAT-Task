import { HttpClient } from '@angular/common/http';
import { Component, ViewEncapsulation, Input, SimpleChanges, ElementRef, Output, EventEmitter } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable, catchError, map, of } from 'rxjs';
import * as FileSaver from 'file-saver';
import * as ExcelJS from 'exceljs';

@Component({
    selector: 'report-grid',
    templateUrl: './report-grid.component.html',
    styleUrls: ['./report-grid.component.css'],
    animations: [appModuleAnimation()],
})
export class ReportGridComponent {
    @Output('onResolveUrl') onResolveUrl: EventEmitter<any> = new EventEmitter();
    @Input() columns: any[] = [];
    @Input() showFooter: boolean = true;
    @Input() footerPageWise: boolean = true;
    @Input() searchFields: string[] = [];
    @Input() data: any[] = [];
    @Input() exportColumns: string[] = [];
    @Input() theme: string = 'sales';
    @Input() decimalConfig: ReportGridDecimalConfig = {
        format: 'en',
        precision: 2,
    };
    @Input() integerConfig: ReportGridIntegerConfig = {
        format: 'en',
    };
    @Input() dateConfig: ReportGridDateConfig = {
        format: 'en-GB',
    };
    @Input() dateTimeConfig: ReportGridDateTimeConfig = {
        format: 'en-GB',
    };
    @Input() urlConfig: ReportGridURLConfig[] = [
        {
            type: 'Download',
            src: '/assets/common/images/pdf_icon.svg',
            innerHtml:
                ' <a ' +
                ' class="btn btn-active-color-primary btn-sm me-2 p-0">' +
                ' <span class="svg-icon svg-icon-3 m-0">' +
                '    <img class="pdf-md" ' +
                `          src="@src" />` +
                '  </span>' +
                '   </a>',
        },
    ];

    footer: string[] = [];
    rows: number = 10;
    first: number = 0;

    constructor(private httpClient: HttpClient, private elementRef: ElementRef, private sanitizer: DomSanitizer) {}

    ngOnInit(): void {}

    ngOnChanges(changes: SimpleChanges) {
        console.log(changes);
        this.prepareFooter();
        this.prepareSearchable();
    }

    public fileExists(url: string): Observable<boolean> {
        console.log(url);
        return this.httpClient.get(url).pipe(
            map(() => true),
            catchError(() => of(false))
        );
    }

    public resolvePath(url: string) {
        window.open(url, '_blank');
        this.fileExists(url).subscribe((e) => {
            if (e) {
                console.log(e);
            } else {
                this.onResolveUrl.emit(url);
            }
        });
    }

    prepareData(val: any, type: string): any {
        if (type == 'Download') {
            let config = this.urlConfig?.find((a) => a.type == type);
            return config.innerHtml.replace('@href', val).replace('@src', config.src);
        } else if (type == 'Decimal') {
            return Number(parseFloat(val).toFixed(this.decimalConfig.precision)).toLocaleString(
                this.decimalConfig.format,
                { minimumFractionDigits: this.decimalConfig.precision }
                // {style: 'currency', currency: 'SAR', minimumFractionDigits: this.decimalConfig.precision}
            );
        } else if (type == 'Integer') {
            return Number(parseFloat(val).toFixed(0)).toLocaleString(this.integerConfig.format);
        } else if (type == 'Date') {
            var d = new Date(val);
            return d.toLocaleDateString(this.dateConfig.format);
        } else if (type == 'DateTime') {
            var d = new Date(val);
            return d.toLocaleString(this.dateTimeConfig.format);
        } else {
            return val;
        }
    }

    prepareFooter() {
        this.footer = [];

        let sliceFrom = this.first;
        let sliceTo = this.first + this.rows > this.data.length ? this.data.length : this.first + this.rows;
        if (!this.footerPageWise) {
            sliceFrom = 0;
            sliceTo = this.data.length;
        }

        console.log(this.data.slice(sliceFrom, sliceTo), sliceFrom, sliceTo);

        this.columns.forEach((c) => {
            if (c.footer) {
                if (c.footer.type == 'label') this.footer.push(c.footer.value);
                else if (c.footer.type == 'sum')
                    this.footer.push(
                        this.prepareData(
                            this.data
                                .slice(sliceFrom, sliceTo)
                                .reduce((a, b) => a + b[c.field], 0)
                                .toString() || 0,
                            c.type
                        )
                    );
                else if (c.footer.type == 'count')
                    this.footer.push(
                        this.data
                            .slice(sliceFrom, sliceTo)
                            .filter((a) => a != null || a != undefined || a != '')
                            .length.toString()
                    );
                else this.footer.push('');
            } else this.footer.push('');
        });
    }

    prepareSearchable() {
        this.searchFields = this.columns.filter((a: ReportGridColumns) => a.searchable).map((a) => a.field);
    }

    paginate(event) {
        this.rows = event.rows;
        this.first = event.first;
        this.prepareFooter();
        console.log(event);
    }

    exportExcel() {
        const workbook = new ExcelJS.Workbook();
        const worksheet = workbook.addWorksheet('Data');
        console.log(this.exportColumns,this.columns.filter(a=>this.exportColumns.includes(a.field)))
        // Add headers
        const headers = this.columns.filter(a=>this.exportColumns.includes(a.field)).map(a=>a.header)
        worksheet.addRow(headers);

        // Add data
        this.data.forEach((item) => {
            const row: any = [];
            this.exportColumns.forEach((col) => {
                row.push(item[col]);
            });
            worksheet.addRow(row);
        });
        workbook.xlsx.writeBuffer().then((buffer: any) => {
            const blob = new Blob([buffer], {
                type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
            });
            FileSaver.saveAs(blob, `${'vita_' + Date.now()}.xlsx`);
        });
    }
}

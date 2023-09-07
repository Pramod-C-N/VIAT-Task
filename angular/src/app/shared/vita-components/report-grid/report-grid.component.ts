import { Component, ViewEncapsulation, Input, SimpleChanges } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';


@Component({
    selector: 'report-grid',
    templateUrl: './report-grid.component.html',
    styleUrls: ['./report-grid.component.css'],
    animations: [appModuleAnimation()],
})
export class ReportGridComponent {
    @Input() columns:any[]=[]
    // ReportGridColumns[];
    @Input() data: any[]=[];
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
                ' <a target="_blank" href="@href"' +
                ' class="btn btn-active-color-primary btn-sm me-2 p-0">' +
                ' <span class="svg-icon svg-icon-3 m-0">' +
                '    <img class="pdf-md" ' +
                `          src="@src" />` +
                '  </span>' +
                '   </a>',
        },
    ];

    
    ngOnInit(): void {}

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
}

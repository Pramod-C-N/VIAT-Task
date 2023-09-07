import { Component, OnInit, Injector } from '@angular/core';
import { forEach as _forEach } from 'lodash-es';
import { SalesInvoicesServiceProxy, SalesSummaryDatePeriod, TenantDashboardServiceProxy } from '@shared/service-proxies/service-proxies';
import { DashboardChartBase } from '../dashboard-chart-base';
import { WidgetComponentBaseComponent } from '../widget-component-base';
import { DateTime } from 'luxon';

class SalesSummaryChart extends DashboardChartBase {
    totalSalesAmount = 0;
    totalVatAmount=0;
    salesAmount = 0;
    vatAmount = 0;
    fromDate:DateTime 
    toDate:DateTime 
    selectedDatePeriod: SalesSummaryDatePeriod;

    data = [];

    constructor(private _dashboardService: SalesInvoicesServiceProxy) {
        super();
    }

    init(salesSummaryData,salesAmount,vatAmount): void {
            
        this.setChartData(salesSummaryData);

        this.salesAmount = salesAmount;
        this.vatAmount = vatAmount;
        this.totalSalesAmount = salesAmount;
        this.totalVatAmount = vatAmount;


        this.hideLoading();
    }

    setChartData(items): void {
        let sales = [];
        let vat = [];

        this.salesAmount = 0;
        this.vatAmount = 0;
        this.totalSalesAmount = 0;
        this.totalVatAmount = 0;

        items.forEach((item) => {
            this.salesAmount += item.salesAmount;
            this.vatAmount += item.vatAmount;
            sales.push({ name: item.invoiceDate.split('T')[0], value: item.salesAmount });
            vat.push({ name: item.invoiceDate.split('T')[0], value: item.vatAmount });
        })

        this.data = [
            {
                name: 'Sales',
                series: sales,
            },
            {
                name: 'VAT',
                series: vat,
            },
        ];

    }

    reload(datePeriod) {
         if (this.selectedDatePeriod === datePeriod) {
            this.hideLoading();
            return;
        }

        this.selectedDatePeriod = datePeriod;
       let type="";
        if(this.selectedDatePeriod === 1) {
            this.fromDate = DateTime.local().startOf('day');
            this.toDate = DateTime.local().endOf('day');
            type="Daily";
        }else if(this.selectedDatePeriod === 2) {
            this.fromDate = DateTime.local().startOf('week');
            this.toDate = DateTime.local().endOf('week');
            type="Weekly";
            console.log(this.fromDate,this.toDate)
        }else if(this.selectedDatePeriod === 3) {
            this.fromDate = DateTime.local().startOf('month');
            this.toDate = DateTime.local().endOf('month');
            type="Monthly";
           
        }

        this.showLoading();
        this._dashboardService.getSummaryDashboardData(this.fromDate,this.toDate,type).subscribe((result) => {
             this.setChartData(result);
            this.hideLoading();
        });
    }
}

@Component({
    selector: 'app-widget-sales-summary',
    templateUrl: './widget-sales-summary.component.html',
    styleUrls: ['./widget-sales-summary.component.css'],
})
export class WidgetSalesSummaryComponent extends WidgetComponentBaseComponent implements OnInit {
    salesSummaryChart: SalesSummaryChart;
    appSalesSummaryDateInterval = SalesSummaryDatePeriod;

    constructor(injector: Injector, private _salesInvoiceServiceProxy: SalesInvoicesServiceProxy) {
        super(injector);
        this.salesSummaryChart = new SalesSummaryChart(this._salesInvoiceServiceProxy);
    }

    ngOnInit(): void {
        this.subDateRangeFilter();

        this.runDelayed(() => {
            this.salesSummaryChart.reload(SalesSummaryDatePeriod.Daily);
        });
    }

    onDateRangeFilterChange = (dateRange) => {
        this.runDelayed(() => {
            this.salesSummaryChart.reload(SalesSummaryDatePeriod.Daily);
        });
    };

    subDateRangeFilter() {
        this.subscribeToEvent('app.dashboardFilters.dateRangePicker.onDateChange', this.onDateRangeFilterChange);
    }
}
import { Component, OnInit, Injector } from '@angular/core';
import { SalesInvoicesServiceProxy, TenantDashboardServiceProxy } from '@shared/service-proxies/service-proxies';
import { DateTime, NumberUnitLength } from 'luxon';
import { DashboardChartBase } from '../dashboard-chart-base';
import { WidgetComponentBaseComponent } from '../widget-component-base';
import { ApexOptions } from 'ng-apexcharts';
import { getCSSVariableValue } from '@metronic/app/kt/_utils';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTimeCustomService } from '@shared/customService/date-time-service';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { GlobalConstsCustomService } from '@shared/customService/global-consts-service';

class DashboardTopStats extends DashboardChartBase {
    items: any = {};

    init(results) {
        this.items = results;
        this.hideLoading();
    }
}

@Component({
    selector: 'app-widget-top-stats',
    templateUrl: './widget-top-stats.component.html',
    styleUrls: ['./widget-top-stats.component.css'],
})
export class WidgetTopStatsComponent extends WidgetComponentBaseComponent implements OnInit {
    dashboardTopStats: DashboardTopStats;
    selectedDate:[DateTime, DateTime]=[DateTime.local().startOf('month'), DateTime.local().endOf('month')];
    public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
    loading: boolean = false;
    items: any = {};
    height: number;
    totalamount: number;
    totalvatamount: number;
    total: number;
    color = 'primary';
    creditcolor = 'success';
    debitcolor = 'warning';
    chartOptions: any = {};
    debitchartOptions: any = {};
    creditchartOptions: any = {};
    totalchartOptions: any = {};
    labelColor: string;
    baseColor: string;
    lightColor: string;
    clabelColor: string;
    cbaseColor: string;
    clightColor: string;
    dlabelColor: string;
    dbaseColor: string;
    dlightColor: string;
    tenantId: Number;
    isVita: boolean = false;
    isGoods: boolean = false;
    isServices: boolean = false;
    chartSize = 100;
    chartLine = 15;
    chartRotate = 145;
    salescreateUrl = '/app/main/sales/createSalesInvoice';
    creditcreateUrl = '/app/main/sales/createSalesInvoice';
    debitcreateUrl = '/app/main/sales/createSalesInvoice';

        constructor(
        injector: Injector,
        private _tenantDashboardServiceProxy: TenantDashboardServiceProxy,
        private _dateTimeService: DateTimeService,
        private _dateTimeCustomService:DateTimeCustomService,
        private _salesInvoiceServiceProxy: SalesInvoicesServiceProxy,
        private _sessionService: AppSessionService,
        private _globalConstsService: GlobalConstsCustomService


    ) {
        super(injector);
        this.dashboardTopStats = new DashboardTopStats();
        this._globalConstsService.data$.subscribe((e) => {
            this.isVita = e.isVita;
        });

        this._globalConstsService.tenantType$.subscribe((e: string) => {
            this.isServices = e.includes('S');
            this.isGoods = e.includes('G');
        });
    }

    ngOnInit() {
      this._dateTimeCustomService.data$.subscribe(e=>{
        console.log(e,"here")
        if(e){
        this.selectedDate = e;
        this.getData(e);
        }else{
            console.log("else")
            this.getData(this.selectedDate);
        }
        this.tenantId =  this._sessionService.tenantId;
        this.salescreateUrl = this.isServices ? ('/app/main/sales/createSalesInvoiceProfessional'):( this._sessionService.tenancyName.toLowerCase()=='brady'?  '/app/main/sales/createSalesInvoiceBrady':'/app/main/sales/createSalesInvoice');
        this.creditcreateUrl = this.isServices ? ('/app/main/sales/createCreditNoteProfessional'):( this._sessionService.tenancyName.toLowerCase()=='brady'?  '/app/main/sales/createCreditNoteBrady':'/app/main/sales/createCreditNote');
        this.debitcreateUrl = this.isServices ? ('/app/main/sales/createDebitNoteProfessional'):( this._sessionService.tenancyName.toLowerCase()=='brady'?  '/app/main/sales/createDebitNoteBrady':'/app/main/sales/createDebitNote');
    })        
    this.height = 150;
        this.labelColor = getCSSVariableValue('--kt-gray-800');
        this.baseColor = getCSSVariableValue('--kt-' + this.color);
        this.lightColor = getCSSVariableValue('--kt-' + this.color + '-light');
        this.clabelColor = getCSSVariableValue('--kt-gray-800');
        this.cbaseColor = '#1BC5BD';
        this.clightColor = getCSSVariableValue('--kt-' + this.creditcolor + '-light');
        this.dlabelColor = getCSSVariableValue('--kt-gray-800');
        this.dbaseColor = getCSSVariableValue('--kt-' + this.debitcolor);
        this.dlightColor = getCSSVariableValue('--kt-' + this.debitcolor + '-light');
        this.chartOptions = getChartOptions(this.height, this.labelColor, this.baseColor, this.lightColor);
        this.creditchartOptions = getCreditChartOptions(
            this.height,
            this.clabelColor,
            this.cbaseColor,
            this.clightColor
        );
        this.debitchartOptions = getDebitChartOptions(this.height, this.dlabelColor, this.dbaseColor, this.dlightColor);
    }

    //----------------------custom implementation-----------------------------

    parseDate(dateString: string): DateTime {
      if (dateString) {
          return DateTime.fromISO(new Date(dateString).toISOString());
      }
      return null;
  }     
    getData(selectedDate:any) {
        console.log(selectedDate,'sss')

        this._salesInvoiceServiceProxy.getStatsDashboardData(selectedDate[0],selectedDate[1]).subscribe((result) => {
            console.log(result,"erroror")
            result.forEach((e) => {
                    console.log(result, 'ds');
                    this.totalamount = result[3].amount;
                    this.totalvatamount = result[3].vatAmount;
                    this.total = result[3].totalAmount;
                    this.items[e.type] = e;
                });
                this.loadTopStatsData(this.items);
                initChart(
                    this.chartSize,
                    this.chartLine,
                    this.chartRotate,
                    this.totalamount,
                    this.totalvatamount,
                    this.total
                );
            });
    }

    loadTopStatsData(results) {
        this.dashboardTopStats.init(results);
    }
}
function getChartOptions(height: number, labelColor: string, baseColor: string, lightColor: string): ApexOptions {
    return {
        series: [
            {
                name: 'Net Profit',
                data: [15, 20, 15, 20],
            },
        ],
        chart: {
            fontFamily: 'inherit',
            type: 'area',
            height: height,
            toolbar: {
                show: false,
            },
            zoom: {
                enabled: false,
            },
            sparkline: {
                enabled: true,
            },
        },
        plotOptions: {},
        fill: {
            type: 'solid',
            opacity: 1,
        },
        stroke: {
            curve: 'smooth',
            show: true,
            width: 3,
            colors: [baseColor],
        },
        xaxis: {
            categories: ['Feb', 'Mar', 'Apr', 'May'],
            axisBorder: {
                show: false,
            },
        },
        yaxis: {
            min: 0,
            max: 35,
            labels: {
                show: false,
                style: {
                    colors: labelColor,
                    fontSize: '12px',
                },
            },
        },
        states: {
            normal: {
                filter: {
                    type: 'none',
                },
            },
        },

        colors: [lightColor],
        markers: {
            colors: [lightColor],
            strokeColors: [baseColor],
            strokeWidth: 3,
        },
    };
}

function getCreditChartOptions(height: number, labelColor: string, baseColor: string, lightColor: string): ApexOptions {
    return {
        series: [
            {
                name: 'Net Profit',
                data: [15, 20, 15, 20],
            },
        ],
        chart: {
            fontFamily: 'inherit',
            type: 'area',
            height: height,
            toolbar: {
                show: false,
            },
            zoom: {
                enabled: false,
            },
            sparkline: {
                enabled: true,
            },
        },
        plotOptions: {},
        fill: {
            type: 'solid',
            opacity: 1,
        },
        stroke: {
            curve: 'smooth',
            show: true,
            width: 3,
            colors: [baseColor],
        },
        xaxis: {
            categories: ['Feb', 'Mar', 'Apr', 'May'],
            axisBorder: {
                show: false,
            },
        },
        yaxis: {
            min: 0,
            max: 35,
            labels: {
                show: false,
                style: {
                    colors: labelColor,
                    fontSize: '12px',
                },
            },
        },
        states: {
            normal: {
                filter: {
                    type: 'none',
                },
            },
            active: {
                allowMultipleDataPointsSelection: false,
                filter: {
                    type: 'none',
                    value: 0,
                },
            },
        },

        colors: [lightColor],
        markers: {
            colors: [lightColor],
            strokeColors: [baseColor],
            strokeWidth: 3,
        },
    };
}

function getDebitChartOptions(height: number, labelColor: string, baseColor: string, lightColor: string): ApexOptions {
    return {
        series: [
            {
                name: 'Net Profit',
                data: [15, 20, 15, 20],
            },
        ],
        chart: {
            fontFamily: 'inherit',
            type: 'area',
            height: height,
            toolbar: {
                show: false,
            },
            zoom: {
                enabled: false,
            },
            sparkline: {
                enabled: true,
            },
        },
        plotOptions: {},
        fill: {
            type: 'solid',
            opacity: 1,
        },
        stroke: {
            curve: 'smooth',
            show: true,
            width: 3,
            colors: [baseColor],
        },
        xaxis: {
            categories: ['Feb', 'Mar', 'Apr', 'May'],
            axisBorder: {
                show: false,
            },
        },
        yaxis: {
            min: 0,
            max: 35,
            labels: {
                show: false,
                style: {
                    colors: labelColor,
                    fontSize: '12px',
                },
            },
        },
        states: {
            normal: {
                filter: {
                    type: 'none',
                },
            },
            active: {
                allowMultipleDataPointsSelection: false,
                filter: {
                    type: 'none',
                    value: 0,
                },
            },
        },

        colors: [lightColor],
        markers: {
            colors: [lightColor],
            strokeColors: [baseColor],
            strokeWidth: 3,
        },
    };
}

const initChart = function (
    chartSize: number = 70,
    chartLine: number = 11,
    chartRotate: number = 145,
    amount: number,
    vatamount: number,
    totalamount: number
) {
    const el = document.getElementById('kt_card_widget_17_chart');

    if (!el) {
        return;
    }

    if (el.hasChildNodes()) {
        return;
    }

    var options = {
        size: chartSize,
        lineWidth: chartLine,
        rotate: chartRotate,
        //percent:  el.getAttribute('data-kt-percent') ,
    };

    const canvas = document.createElement('canvas');
    const span = document.createElement('span');

    // @ts-ignore
    if (typeof G_vmlCanvasManager !== 'undefined') {
        // @ts-ignore
        G_vmlCanvasManager.initElement(canvas);
    }

    const ctx = canvas.getContext('2d');
    canvas.width = canvas.height = options.size;

    el.appendChild(span);
    el.appendChild(canvas);

    // @ts-ignore
    ctx.translate(options.size / 2, options.size / 2); // change center
    // @ts-ignore
    ctx.rotate((-1 / 2 + options.rotate / 180) * Math.PI); // rotate -90 deg

    //imd = ctx.getImageData(0, 0, 240, 240);
    const radius = (options.size - options.lineWidth) / 2;

    const drawCircle = function (color: string, lineWidth: number, percent: number) {
        //percent = Math.min(Math.max(0, percent || 1), 1);
        console.log(percent, 'percent');

        if (!ctx) {
            return;
        }

        ctx.beginPath();
        ctx.arc(0, 0, radius, 0, Math.PI * 2 * percent, false);
        ctx.strokeStyle = color;
        ctx.lineCap = 'round'; // butt, round or square
        ctx.lineWidth = lineWidth;
        ctx.stroke();
    };
    // Init
    drawCircle(getCSSVariableValue('--kt-success'), options.lineWidth, amount / amount);
    drawCircle(getCSSVariableValue('--kt-primary'), options.lineWidth, vatamount / (amount + vatamount));
};

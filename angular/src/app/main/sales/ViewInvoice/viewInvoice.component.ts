// @ts-nocheck

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    SalesInvoicesServiceProxy,
    TenantBasicDetailsServiceProxy,
    GetInvoiceDto,
    CreateOrEditSalesInvoiceSummaryDto,
    CreateOrEditSalesInvoiceItemDto,
    CreateOrEditSalesInvoiceDto,
    CreateOrEditSalesInvoicePartyDto,
    CreateOrEditSalesInvoiceAddressDto
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTime } from 'luxon';
import { AppSessionService } from '@shared/common/session/app-session.service';
@Component({
    selector:'viewInvoice',
    templateUrl: './viewInvoice.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./viewInvoice.component.css'],
    animations: [appModuleAnimation()],
})
export class ViewInvoiceComponent extends AppComponentBase {
    public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
    @Input() id: number;
    @Input() type: string;
    @Input() theme: string;
    @Input() from: string;

    invoices: ReportDto[] = [];
    invoice: CreateOrEditSalesInvoiceDto = new CreateOrEditSalesInvoiceDto();
    customer: CreateOrEditSalesInvoicePartyDto = new CreateOrEditSalesInvoicePartyDto();
    address: CreateOrEditSalesInvoiceAddressDto = new CreateOrEditSalesInvoiceAddressDto();
    invoiceItems: CreateOrEditSalesInvoiceItemDto[] = [];
    invoiceItem: CreateOrEditSalesInvoiceItemDto = new CreateOrEditSalesInvoiceItemDto();
    invoiceSummary: CreateOrEditSalesInvoiceSummaryDto = new CreateOrEditSalesInvoiceSummaryDto();
    totalChargeVATAmount: number;
    shipmentRegName: string;
    ShipmentContctName:string;
    ShipmentContact:string;
    ShipmentEmail: string;
    ShipmentVAT: string; 
    ChargeVATAmount: number;
    dateofsupply: DateTime;
    Cusemail: string;
    CuscontactNumber: string;
    billtoattn: string;
    Customeradd1: string;
    Customeradd2: string;
    Customeradd3: string;
    Customeradd4: string;
    tenantcontact:number;
    tenantemail:number;
    tenantcr:number;
    tenantadd: string;
    tenantadd2: string;
    tenantvat:string;
    exchangeRateVal: number;
    exchangerate: any[] = [];
    tenantdata: any[] = [];
    othercharges: any[] = [];
    othercharge: number;
    otherchargename: string;
    shipmentAdd: string;
    shipmentAdd1: string;
    shipmentAdd2: string;
    shipmentAdd3: string;
    totalcharge: number;
    discount: number;
    bankinfo: string;
    weareyourvendor: string;
    tenantId: Number;
    tenantName: String;
    transactionurl:string;
    shipment: any[] = [];
    additionalData: any[] = [];
    pdfUrl = AppConsts.pdfUrl + '/InvoiceFiles';
    header:string;
    columns: any[] = [
        { field: 'invoiceId', header: 'Invoice Id' },
        { field: 'invoiceDate', header: 'InvoiceDate' },
        { field: 'customerName', header: 'Customer Name' },
        { field: 'contactNo', header: 'Contact No' },
        { field: 'amount', header: 'Amount' },
    ];
    exportColumns: any[] = this.columns.map((col) => ({ title: col.header, dataKey: col.field }));
    _selectedColumns: any[] = this.columns;

    @Input() get selectedColumns(): any[] {
        return this._selectedColumns;
    }

    set selectedColumns(val: any[]) {
        //restore original order
        this._selectedColumns = this.columns.filter((col) => val.includes(col));
    }

    constructor(
        injector: Injector,

        private _sessionService: AppSessionService,
        private _invoiceServiceProxy: SalesInvoicesServiceProxy,
        private _tenantbasicdetailsServiceProxy: TenantBasicDetailsServiceProxy,
        private _dateTimeService: DateTimeService,
        private route: ActivatedRoute,
        private router: Router,

    ) {
        super(injector);
    }

    //ngoninit

    ngOnInit(): void {
        this.gettenantdetails();
        this.route.paramMap.subscribe((paramMap) => {
            //this.id = paramMap.get('id');
            //this.type=paramMap.get('type');
            if((this.type).toLowerCase() == 'sales')
            {
                this.header='Sales Invoice';
            }
            else if((this.type).toLowerCase()=='credit')
            {
                this.header='Credit Note';
            }
            else if((this.type).toLowerCase()=='debit'){
                this.header='Debit Note';
            }
            else{
                this.header='Draft'
            }
            this.getInvoiceData(this.id);
            this.getsalesitemdetail(this.id,this.type);
        });
        this.tenantId = this._sessionService.tenantId == null ? 0 : this._sessionService.tenantId;
        this.tenantName = this._sessionService.tenancyName;
        this.othercharge = 0;
        this.transactionurl="/app/main/sales/transactions";
    }

    getInvoiceData(id) {
        this._invoiceServiceProxy.viewInvoice(id.toString(),this.type).subscribe((result) => {
            this.invoices = result;
            this.getCustomerInfo(result);
        });
    }
    parseDate(dateString: string): DateTime {
        if (dateString) {
            return DateTime.fromISO(new Date(dateString).toISOString());
        }
        return null;
    }
    //format date
    formatDate(date) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('-');
    }
    getCustomerInfo(data) {
        this.invoice.billingReferenceId = data[0].billingReferenceId;
        this.invoice.invoiceNumber = data[0].irnNo;
        this.invoice.invoiceNotes = data[0].invoiceNotes;
        this.invoice.invoiceCurrencyCode = data[0].invoiceCurrencyCode;
        this.dateofsupply = data[0].issueDate;
        this.customer.registrationName = data[0].registrationName;
        this.CuscontactNumber = data[0].contactNumber;
        this.Cusemail = data[0].email;
        this.customer.vatid = data[0].vat;
        this.customer.customerId=data[0].customerId;
        this.Customeradd1 = data[0].cusAdd1;
        this.Customeradd2 = data[0].cusAdd2;
        this.Customeradd3 = data[0].cusAdd3;
        this.Customeradd4 = data[0].cusAdd4;
        this.shipment = JSON.parse(data[0].shipment);
        this.exchangerate = JSON.parse(data[0].additionalData1);
        this.exchangeRateVal = this.exchangerate[0].exchangeRate;
        this.additionalData = JSON.parse(data[0].additionalData);
        console.log(this.additionalData,'add')
        this.weareyourvendor=data[0].youvendor;
        this.bankinfo = this.additionalData[0].bank_information.replace(/\n/g, ' ');
        if(this.shipment.length>0)
        {
        this.shipmentAdd = this.shipment[0].address.buildingNo.concat(' ').concat(this.shipment[0].address.street);
        this.shipmentAdd1 = this.shipment[0].address.neighbourhood;
        this.shipmentAdd2 = this.shipment[0].address.city.concat(' ').concat(this.shipment[0].address.state);
        this.shipmentAdd3 = this.shipment[0].address.countryCode
            .concat(' ')
            .concat(this.shipment[0].address.postalCode);
        }
        this.customer.otherDocumentTypeId = data[0].buyerType;
        this.billtoattn = data[0].billtoAttn;
        this.address.buildingNo = data[0].buildingNo;
        this.address.street = data[0].street?.trim();
        this.address.additionalStreet = data[0].additionalStreet?.trim();
        this.address.city = data[0].city?.trim();
        this.address.state = data[0].state;
        this.address.countryCode = data[0].countryCode;
        this.address.postalCode = data[0].postalCode;
        this.address.neighbourhood = data[0].neighbourhood?.trim();
    }

    getsalesitemdetail(id,type) {
        this._invoiceServiceProxy.getsalesitemdetail(id,type).subscribe((data) => {
            for (let i = 0; i < data.length; i++) {
                if (data[i].isOtherCharges == false) {
                    this.invoiceItem.costPrice = data[i].costPrice;
                    this.invoiceItem.description = data[i].description;
                    this.invoiceItem.excemptionReasonCode = data[i].excemptionReasonCode;
                    this.invoiceItem.excemptionReasonText = data[i].excemptionReasonText;
                    this.invoiceItem.name = data[i].name;
                    this.invoiceItem.currencyCode = 'SAR';
                    this.invoiceItem.discountAmount = data[i].discountAmount;
                    this.invoiceItem.discountPercentage = data[i].discountPercentage;
                    this.invoiceItem.grossPrice = data[i].grossPrice;
                    this.invoiceItem.lineAmountInclusiveVAT = data[i].lineAmountInclusiveVAT;
                    this.invoiceItem.quantity = data[i].quantity;
                    this.invoiceItem.unitPrice = data[i].unitPrice;
                    this.invoiceItem.netPrice = data[i].netPrice;
                    this.invoiceItem.vatAmount = data[i].vatAmount;
                    this.invoiceItem.vatRate = data[i].vatRate;
                    this.invoiceItem.vatCode = data[i].vatRate === 15 ? 'S' : 'Z';
                    this.invoiceItems.push(this.invoiceItem);
                    // this.originalInvoiceItems = JSON.parse(JSON.stringify(this.invoiceItems));
                    this.invoiceCount = this.invoiceItems.length;
                    this.invoiceItem = new CreateOrEditSalesInvoiceItemDto();
                } else {
                    this.othercharges.push(data[i]);
                    // this.othercharge+=this.data[i].unitPrice;
                }
            }

            this.calculateInvoiceSummary(this.invoiceSummary, this.invoiceItems, this.othercharges);
            this.otherchargevatcalc();
        });
    }
    otherchargevatcalc() {
      this.totalChargeVATAmount = 0;
        for (let i = 0; i < this.othercharges.length; i++) {
            if (this.invoiceItems.filter((p) => p.vatAmount > 0).length > 0) {
                this.chargeVATAmount = this.othercharges[i].unitPrice * 0.15;
                this.totalChargeVATAmount += this.chargeVATAmount;
            } else {
                this.totalChargeVATAmount = 0;
            }
        }
    }
    calculateInvoiceSummary(summary, items, charges) {
        summary.totalAmountWithVAT = 0;
        summary.totalAmountWithoutVAT = 0;
        summary.sumOfInvoiceLineNetAmount = 0;
        summary.netInvoiceAmount = 0;
        summary.totalVATAmount = 0;
        this.discount = 0;
        this.totalcharge = 0;
        charges.forEach((charge) => {
            this.totalcharge += charge.unitPrice;
        });
        items.forEach((invoiceItem) => {
            summary.totalAmountWithVAT += invoiceItem.lineAmountInclusiveVAT;
            summary.totalAmountWithoutVAT += invoiceItem.quantity * invoiceItem.unitPrice - invoiceItem.discountAmount;
            summary.sumOfInvoiceLineNetAmount += invoiceItem.netPrice;
            summary.netInvoiceAmount += invoiceItem.quantity * invoiceItem.unitPrice;
            this.discount += invoiceItem.discountAmount;
            summary.totalVATAmount = summary.totalAmountWithVAT - summary.totalAmountWithoutVAT;
        });
    }

    gettenantdetails() {
        this._tenantbasicdetailsServiceProxy.getTenantById(this._sessionService.tenant.id).subscribe((data) => {
            this.tenantdata = data;
            this.tenantadd = data[0].tenantdd;
            this.tenantadd2 = data[0].tenAdd2;
            this.tenantcontact =data[0].contactNumber;
            this.tenantemail=data[0].emailID;
            this.tenantcr=data[0].documentNumber;
            this.tenantvat=data[0].vatid;
        });
    }

    formatKey(key: string): string {
        return key
          .replace(/_/g, ' ')
          .replace(/\b\w/g, firstLetter => firstLetter.toUpperCase());
      }

      back()
      {
        this.router.navigate(['app/main/sales/transactions'], { state: { tabvaule: 'Sales Invoice' } });
      }
}

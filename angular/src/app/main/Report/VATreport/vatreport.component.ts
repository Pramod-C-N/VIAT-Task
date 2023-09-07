
import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VatReportDto, ReportServiceProxy, VatCalculationReportDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
@Component({
  templateUrl: './vatreport.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
  styleUrls: ['./vatreport.component.less'],
})
export class VatReportNewComponent extends AppComponentBase {
  @ViewChild('modalterms' , { static: false }) modal: ModalDirective;
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
  tenantId: Number;
  tenantName: string;
  fromDate: Date;
  toDate: Date;
  data: VatReportDto[] = [new VatReportDto(), new VatReportDto(), new VatReportDto(), new VatReportDto(), new VatReportDto(),
  new VatReportDto(), new VatReportDto(), new VatReportDto(), new VatReportDto(), new VatReportDto(), new VatReportDto(),
  new VatReportDto(), new VatReportDto(), new VatReportDto(), new VatReportDto(), new VatReportDto()];

  vatAmount = 0;
  totalAmount = 0;
  netAmount = 0;
  searched = false;
  showtooltip: string;
  daywiseColumns: any[] = [
    { field: 'slNo', header: 'Sl.No' },
    { field: 'issueDate', header: 'Issue Date' },
    { field: 'netAmount', header: 'Net Amount' },
    { field: 'vatAmount', header: 'Vat Amount' },
    { field: 'totalAmount', header: 'Total Amount' }
  ];

  FieldNo: any = {
    A1: 'A1',
    B1: 'B1',
    C1: 'C1',
    A1D1: 'A1.1',
    B1D1: 'B1.1',
    C1D1: 'C1.1',
    A1D2: 'A1.2',
    B1D2: 'B1.2',
    C1D2: 'C1.2',
    A2: 'A2',
    B2: 'B2',
    C2: 'C2',
    A3: 'A3',
    B3: 'B3',
    C3: 'C3',
    A4: 'A4',
    B4: 'B4',
    C4: 'C4',
    A5: 'A5',
    B5: 'B5',
    C5: 'C5',
    A7: 'A7',
    B7: 'B7',
    C7: 'C7',
    A7D1: 'A7.1',
    B7D1: 'B7.1',
    C7D1: 'C7.1',
    A8: 'A8',
    B8: 'B8',
    C8: 'C8',
    A8D1: 'A8.1',
    B8D1: 'B8.1',
    C8D1: 'C8.1',
    A9: 'A9',
    B9: 'B9',
    C9: 'C9',
    A9D1: 'A9.1',
    B9D1: 'B9.1',
    C9D1: 'C9.1',
    A10: 'A10',
    B10: 'B10',
    C10: 'C10',
    A11: 'A11',
    B11: 'B11',
    C11: 'C11',
    A12: 'A12',
    B12: 'B12',
    C12: 'C12',
    A13: 'A13',
    B13: 'B13',
    C13: 'C13',
    C14: 'C14',
    C15: 'C15',
    C16: 'C16',
 };
 columns: any[] = this.daywiseColumns;
 invoices: VatCalculationReportDto[] = [];
  constructor(
    injector: Injector,
    private _VatReportServiceProxy: ReportServiceProxy,
    private _sessionService: AppSessionService,
    private _dateTimeService: DateTimeService,
    private _fileDownloadService: FileDownloadService,
    private modalService: BsModalService
  ) {
    super(injector);
  }

  ngOnInit(): void {

    this.tenantId = this._sessionService.tenantId;
    this.tenantName = this._sessionService.tenant.name;
  }

  getDetails(fieldNumber: string){
    this.modal.show();
    this._VatReportServiceProxy.getVatDescription( fieldNumber, this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString())).subscribe((result) => {
      this.invoices = result;
      this.searched = true;
    });

  }
  getSalesDetailedReport() {

    this._VatReportServiceProxy.getAllNew(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString())).subscribe((result) => {
      this.data = result;
      this.searched = true;
    });
  }
  downloadExcel() {
    this._VatReportServiceProxy.getVatReportToExcel(this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()), 'VATReturnReport', this.tenantName, false).subscribe((result) => {
      this._fileDownloadService.downloadTempFile(result);
    });
  }

  parseDate(dateString: string): DateTime {
    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());
    }
    return null;
  }
}

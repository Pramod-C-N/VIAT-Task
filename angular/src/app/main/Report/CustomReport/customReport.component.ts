

import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportServiceProxy,CustomReportServiceProxy,GetCustomReportForViewDto,PagedResultDtoOfGetCustomReportForViewDto} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter, result } from 'lodash-es';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { finalize } from 'rxjs/operators';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { saveAs } from 'file-saver';

@Component({
  templateUrl: './customReport.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class CustomReportComponent extends AppComponentBase {
  public dateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];

  tenantId: Number;
  tenantName: String;
  fromDate: Date;
  toDate: Date;
  reportName: string ="";
  reports: PagedResultDtoOfGetCustomReportForViewDto = new PagedResultDtoOfGetCustomReportForViewDto();
  //reports:any[]=[];
  labels: any[] = [];
  data:any[] = [];
  footer:any[] = [];
  
  isdisable:boolean=false;

  constructor(
    injector: Injector,
    private _SalesDetailedProxy: ReportServiceProxy,
    private _sessionService: AppSessionService,
    private _dateTimeService: DateTimeService,
    private _customReportProxy: CustomReportServiceProxy,
    private _fileDownloadService: FileDownloadService,
  ) {
    super(injector);
  }


  //ngoninit

  ngOnInit(): void {
  this.getReportName();
    this.tenantId = this._sessionService.tenantId
    this.tenantName = this._sessionService.tenancyName
  }


  getReportName(){

    this._customReportProxy.getAll("","","","",0,undefined) .subscribe((result) => {
      console.log(result);
      this.reports=result;

   });


   

  }


  getSalesDetailedReport() {

    this.isdisable=true;
    this._SalesDetailedProxy.execSP(this.reportName,this.parseDate(this.dateRange[0].toString()), this.parseDate(this.dateRange[1].toString()))
    .pipe(finalize(() => this.isdisable=false))
    .subscribe((result) => {
      console.log(result);
      this.data=result;
      if(result.length>0)
      this.labels = Object.keys(result[0]);
      this.createFooter()
    });
  }

  checkDecimal(){
    var temp=this.data[0]
    //loop through the object
    for (var key in temp) {
      if (temp.hasOwnProperty(key)) {
       //check if the value is a float
        if(!isNaN(parseFloat(temp[key]))){
          //if it is a float then add it to the footer
          this.footer.push({label:key,decimal:true,value:0});
      }else{
        //if not then add it to the footer
        this.footer.push({label:key,decimal:false,value:""});
      }
    }
  }
}

createFooter(){
  this.footer=[];
  this.checkDecimal();
  console.log(this.footer)
  
  for(var i in this.labels){
    for(var j in this.data){
      if(this.footer[i].decimal==true){
        this.footer[i].value+=parseFloat(this.data[j][this.labels[i]]);
      }
    }
  }
  
  console.log(this.footer)
}

  parseDate(dateString: string): DateTime {
    if (dateString) {
      return DateTime.fromISO(new Date(dateString).toISOString());

    }
    return null;
  }

  exportToCsv(){
     
    const replacer = (key, value) => value === null ? '' : value; // specify how you want to handle null values here
    const header = this.labels;
    let csv = this.data.map(row => header.map(fieldName => JSON.stringify(row[fieldName], replacer)).join(','));
    csv.unshift(header.join(','));
    let csvArray = csv.join('\r\n');

    var blob = new Blob([csvArray], {type: 'text/csv' })
    saveAs(blob, this.reportName+DateTime.local().toFormat('yyyy-MM-dd')+".csv");  
  }





}
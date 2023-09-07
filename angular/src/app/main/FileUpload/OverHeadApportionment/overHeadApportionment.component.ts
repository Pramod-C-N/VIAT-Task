import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import {CustomerAddressDto,ConstitutionServiceProxy,GetConstitutionForViewDto,CountryServiceProxy,GetCountryForViewDto,CustomersesServiceProxy,CreateOrEditCustomersDto, OverheadApportionmentServiceProxy, OverheadApportionmentCurrentDataDetailedDTO, OverheadApportionmentPreviousDataDTO, ReportServiceProxy} from '@shared/service-proxies/service-proxies';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { parseDate } from 'ngx-bootstrap/chronos';


@Component({
  selector:'OverHeadApportionment',
  templateUrl: './overHeadApportionment.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class OverHeadApportionmentComponent extends AppComponentBase {
  loadprevui = false;
  loadcurui = false;
  showMenu = false;
  activeMenu = 'menu1';
  tab = 'Detailed' ;
  commonForm: FormGroup;
  loadIndividualdt:boolean=false;
  inputValues: string[] = [];
  customerId:number;
  currentMonthIndex: number = new Date().getMonth();
  currentYear: number = new Date().getFullYear();
  previousYear: number = this.currentYear - 1;
  checkboxValue: any;
  month: number = 12;
  constitutionType:string;
  detailedColumns = true;
  summaryColumns = false;
  previousYearPercentage: number;
  OverHeadApportionment:OverheadApportionmentPreviousDataDTO=new OverheadApportionmentPreviousDataDTO()
  OverHeadApportionmentList:OverheadApportionmentPreviousDataDTO[]=[]



  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _VatReportServiceProxy: ReportServiceProxy,
    private _overheadService: OverheadApportionmentServiceProxy,
    private cd:ChangeDetectorRef
  ) {
    super(injector);
  }
  ngOnInit(): void {
    this.populateOverheadList();
    this.loadprevui = true;
    this.loadcurui = false;
    this.tab = 'Detailed' ;
    this.getFinancialYear();
    this.getPreviousData();
   }
   loadIndividual(){
    this.loadIndividualdt=true;
  }
  
  loadpreviousui() {
    this.loadprevui = true;
    this.getPreviousData();
    this.loadcurui = false;
}

loadcurrentui() {
    this.loadprevui = false;
    this.loadcurui = true;
    this.detailedColumns = true;
    this.summaryColumns = false;
    this.getDetailedData();
    this.tab = 'Detailed';
}

getFinancialYear() {
  //this._localStorage.setCache("dat",)
  this._VatReportServiceProxy.getFinancialYearData().subscribe((result) => {
    //console.log(result[0].effectiveFromDate.slice(2,4),"result");
    //this.year = result[0].effectiveFromDate.slice(2,4);
    this.currentYear = result[0].effectiveFromDate.slice(0,4);
    this.previousYear = parseInt(result[0].effectiveFromDate.slice(0,4)) - 1 ;
    console.log(this.previousYear,'this.previousYear');
  });
}

changeEvent(event: any) {
  if(this.tab == 'Detailed')
  {
      this.changeTab('Summary');
      this.detailedColumns = false;
      this.summaryColumns = true;
      this.getSummaryData()
      this.tab='Summary';
  }
  else
  {
      this.changeTab('Detailed');
      this.detailedColumns = true;
      this.summaryColumns = false;
      this.getDetailedData();
      this.tab='Detailed';
  }
  }

  changeTab(tab: string) {
    this.tab = tab;
    if (tab == 'Summary') {
        this.summaryColumns  = true;
        this.detailedColumns = false;
    } else if (tab == 'Detailed') {
        this.detailedColumns = true;
        this.summaryColumns = false;
    }
}

range(count: number): number[] {
  return Array.from({ length: count }, (_, i) => i + 1);
}

getMonthName(index: number): string {
  const months = [
    'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Total'
  ];

  //return months[index - 1] || '';
  const yearInYYFormat = this.currentYear.toString().slice(-2); // Get the current year in YY format

  let t= `${months[index]}-${yearInYYFormat}`; // Return "Jan-23" (YY format) or "Jan-2023" (YYYY format)
  return t? t: 'Total'
}

populateOverheadList(){
  for(let i=0;i<12;i++){
    let temp  = new OverheadApportionmentPreviousDataDTO();
    temp.exemptPurchase=0;
    temp.exemptSupplies=0;
    temp.taxablePurchase=0;
    temp.taxableSupplies=0;
    temp.exemptTaxablePurchase=0;
    temp.exemptTaxableSupplies=0;
    temp.percentageofTaxable=0;
    temp.date = (i).toString();
    temp.type = "Detailed"
    this.OverHeadApportionmentList.push(temp)
  }
}

onSaveSummaryData(){
  this.OverHeadApportionment.type="Summary";
  this.checkData();
  if (this.OverHeadApportionment.taxableSupplies == 0 || this.OverHeadApportionment.exemptSupplies == 0 ) this.OverHeadApportionment.percentageofTaxable = 0;
  else this.OverHeadApportionment.percentageofTaxable = parseFloat(((this.OverHeadApportionment.taxableSupplies/(this.OverHeadApportionment.taxableSupplies+this.OverHeadApportionment.exemptSupplies))*100).toFixed(2));
  this._overheadService.createOverheadApportionmentCurrentDataSummary(this.OverHeadApportionment).subscribe(e=>{
    e? this.notify.success("Saved Successfully") : this.notify.error("Something went wrong")
  })
}

onSavePreviousData(){
  this.OverHeadApportionment.type="Previous";
  this.checkData();
  if (this.OverHeadApportionment.taxableSupplies == 0 || this.OverHeadApportionment.exemptSupplies == 0 ) this.OverHeadApportionment.percentageofTaxable = 0;
  else this.OverHeadApportionment.percentageofTaxable = parseFloat(((this.OverHeadApportionment.taxableSupplies/(this.OverHeadApportionment.taxableSupplies+this.OverHeadApportionment.exemptSupplies))*100).toFixed(2));
  this._overheadService.createOverheadApportionmentPreviousData(this.OverHeadApportionment).subscribe(e=>{
    e? this.notify.success("Saved Successfully") : this.notify.error("Something went wrong")
  })
}

onSaveDetailedData(){
  this.OverHeadApportionment.type="Detailed";
  this.checkData();
  this._overheadService.createOverheadApportionmentCurrentDataDetailed(this.OverHeadApportionmentList).subscribe(e=>{
    e? this.notify.success("Saved Successfully") : this.notify.error("Something went wrong")
  })
}

getSummaryData(){
  this._overheadService.getOverheadApportionmentCurrentDataSummary().subscribe(e=>{
    if(e?.length>0){
      this.OverHeadApportionment.exemptPurchase = parseFloat(e[0].exemptPurchase);
      this.OverHeadApportionment.taxablePurchase = parseFloat(e[0].taxablePurchase);
      this.OverHeadApportionment.exemptSupplies = parseFloat(e[0].exemptSupplies);
      this.OverHeadApportionment.taxableSupplies = parseFloat(e[0].taxableSupplies);
      this.OverHeadApportionment.exemptTaxablePurchase = parseFloat(e[0].exemptTaxablePurchase);
      this.OverHeadApportionment.exemptTaxableSupplies = parseFloat(e[0].exemptTaxableSupplies);
      this.OverHeadApportionment.percentageofTaxable = parseFloat(e[0].percentageofTaxable);
      this.OverHeadApportionment.type = e[0].type;
      this.OverHeadApportionment.date = e[0].date;
    }
    else{
      this.OverHeadApportionment.exemptPurchase = 0;
      this.OverHeadApportionment.taxablePurchase = 0;
      this.OverHeadApportionment.exemptSupplies = 0;
      this.OverHeadApportionment.taxableSupplies = 0;
      this.OverHeadApportionment.exemptTaxablePurchase = 0;
      this.OverHeadApportionment.exemptTaxableSupplies = 0;
      this.OverHeadApportionment.percentageofTaxable = 0;
      this.OverHeadApportionment.type = e[0].type;
      this.OverHeadApportionment.date = e[0].date;
    }
  })
}

getPreviousData(){
  this._overheadService.getOverheadApportionmentPreviousData().subscribe(e=>{
    if(e?.length>0){
    this.OverHeadApportionment.exemptPurchase = parseFloat(e[0].exemptPurchase);
    this.OverHeadApportionment.taxablePurchase = parseFloat(e[0].taxablePurchase);
    this.OverHeadApportionment.exemptSupplies = parseFloat(e[0].exemptSupplies);
    this.OverHeadApportionment.taxableSupplies = parseFloat(e[0].taxableSupplies);
    this.OverHeadApportionment.exemptTaxablePurchase = parseFloat(e[0].exemptTaxablePurchase);
    this.OverHeadApportionment.exemptTaxableSupplies = parseFloat(e[0].exemptTaxableSupplies);
    this.OverHeadApportionment.percentageofTaxable = parseFloat(e[0].percentageofTaxable);
    this.previousYearPercentage = this.OverHeadApportionment.percentageofTaxable;
    this.OverHeadApportionment.type = e[0].type;
    this.OverHeadApportionment.date =e[0].date;
    }
    else{
      this.OverHeadApportionment.exemptPurchase = 0;
      this.OverHeadApportionment.taxablePurchase = 0;
      this.OverHeadApportionment.exemptSupplies = 0;
      this.OverHeadApportionment.taxableSupplies = 0;
      this.OverHeadApportionment.exemptTaxablePurchase = 0;
      this.OverHeadApportionment.exemptTaxableSupplies = 0;
      this.OverHeadApportionment.percentageofTaxable = 0;
      this.OverHeadApportionment.type = e[0].type;
      this.OverHeadApportionment.date = e[0].date;
    }
  })
}

getDetailedData(){
  this._overheadService.getOverheadApportionmentCurrentDataDetailed("Detailed").subscribe(e=>{
    if(e?.length>0){
      this.OverHeadApportionmentList = []
      for(let i = 0 ; i<e.length ; i++){
        let temp = new OverheadApportionmentCurrentDataDetailedDTO()
        temp.exemptPurchase = parseFloat(e[i].exemptPurchase);
        temp.taxablePurchase = parseFloat(e[i].taxablePurchase);
        temp.exemptSupplies = parseFloat(e[i].exemptSupplies);
        temp.taxableSupplies = parseFloat(e[i].taxableSupplies);
        temp.exemptTaxablePurchase = parseFloat(e[i].exemptTaxablePurchase);
        temp.exemptTaxableSupplies = parseFloat(e[i].exemptTaxableSupplies);
        temp.percentageofTaxable = parseFloat(e[i].percentageofTaxable);
        temp.type = e[i].type;
        temp.date = e[i].date;
    this.OverHeadApportionmentList.push(temp)
    }
    console.log(this.OverHeadApportionmentList,'this.OverHeadApportionmentList')
    this.cd.detectChanges()
  }
  })
}

calculate(val,index,field){
  if(field=='exemptSupplies')
this.OverHeadApportionmentList[index].exemptSupplies = val
else if(field=='taxableSupplies')
this.OverHeadApportionmentList[index].taxableSupplies = val
else if(field=='taxablePurchase')
this.OverHeadApportionmentList[index].taxablePurchase = val
else if(field=='exemptPurchase')
this.OverHeadApportionmentList[index].exemptPurchase = val

this.OverHeadApportionmentList[index].exemptTaxableSupplies = this.OverHeadApportionmentList[index].exemptSupplies+this.OverHeadApportionmentList[index].taxableSupplies
this.OverHeadApportionmentList[index].exemptTaxablePurchase = this.OverHeadApportionmentList[index].exemptPurchase+this.OverHeadApportionmentList[index].taxablePurchase

this.OverHeadApportionmentList[11].exemptPurchase = this.OverHeadApportionmentList.slice(0,11).map(a => a.exemptPurchase).reduce(this.sumutility,0);
this.OverHeadApportionmentList[11].taxablePurchase = this.OverHeadApportionmentList.slice(0,11).map(a => a.taxablePurchase).reduce(this.sumutility,0);
this.OverHeadApportionmentList[11].exemptSupplies = this.OverHeadApportionmentList.slice(0,11).map(a => a.exemptSupplies).reduce(this.sumutility,0);
this.OverHeadApportionmentList[11].taxableSupplies = this.OverHeadApportionmentList.slice(0,11).map(a => a.taxableSupplies).reduce(this.sumutility,0);
this.OverHeadApportionmentList[11].exemptTaxablePurchase = this.OverHeadApportionmentList.slice(0,11).map(a => a.exemptTaxablePurchase).reduce(this.sumutility,0);
this.OverHeadApportionmentList[11].exemptTaxableSupplies = this.OverHeadApportionmentList.slice(0,11).map(a => a.exemptTaxableSupplies).reduce(this.sumutility,0);
this.OverHeadApportionmentList[11].exemptPurchase = this.OverHeadApportionmentList.slice(0,11).map(a => a.exemptPurchase).reduce(this.sumutility,0);
}

private sumutility(x,y){
  return x+y;
}

checkData(){
  if (this.OverHeadApportionment.taxableSupplies == null) this.OverHeadApportionment.taxableSupplies = 0;
  if (this.OverHeadApportionment.exemptSupplies == null) this.OverHeadApportionment.exemptSupplies = 0;
  if (this.OverHeadApportionment.taxablePurchase == null) this.OverHeadApportionment.taxablePurchase = 0;
  if (this.OverHeadApportionment.exemptPurchase == null) this.OverHeadApportionment.exemptPurchase = 0;
  if (this.OverHeadApportionment.exemptTaxablePurchase == null) this.OverHeadApportionment.exemptTaxablePurchase = 0;
  if (this.OverHeadApportionment.exemptTaxableSupplies == null) this.OverHeadApportionment.exemptTaxableSupplies = 0;
  
  for(let i = 0 ; i<12 ; i++){
  if (this.OverHeadApportionmentList[i].taxableSupplies == null) this.OverHeadApportionmentList[i].taxableSupplies = 0;
  if (this.OverHeadApportionmentList[i].exemptSupplies == null) this.OverHeadApportionmentList[i].exemptSupplies = 0;
  if (this.OverHeadApportionmentList[i].taxablePurchase == null) this.OverHeadApportionmentList[i].taxablePurchase = 0;
  if (this.OverHeadApportionmentList[i].exemptPurchase == null) this.OverHeadApportionmentList[i].exemptPurchase = 0;
  if (this.OverHeadApportionmentList[i].exemptTaxablePurchase == null) this.OverHeadApportionmentList[i].exemptTaxablePurchase = 0;
  if (this.OverHeadApportionmentList[i].exemptTaxableSupplies == null) this.OverHeadApportionmentList[i].exemptTaxableSupplies = 0;
  }
}

}

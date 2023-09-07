import { Component, Injector, ViewEncapsulation, ViewChild, ChangeDetectorRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { filter as _filter } from 'lodash-es';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { OverheadApportionmentCurrentDataDetailedDTO, OverheadApportionmentPreviousDataDTO, OverheadApportionmentServiceProxy } from '@shared/service-proxies/service-proxies';


@Component({
    selector:'nominalSupplies',
    templateUrl: './nominalSupplies.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
  })

export class NominalSuppliesComponent extends AppComponentBase {
currentMonthIndex: number = new Date().getMonth();
currentYear: number = new Date().getFullYear();
previousYear: number = this.currentYear - 1;
month: number = 12;
OverHeadApportionment:OverheadApportionmentPreviousDataDTO=new OverheadApportionmentPreviousDataDTO()
OverHeadApportionmentList:OverheadApportionmentPreviousDataDTO[]=[]

constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _overheadService: OverheadApportionmentServiceProxy,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.populateOverheadList();
    this.getDetailedData();
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
      temp.type = "Nominal"
      this.OverHeadApportionmentList.push(temp)
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
  onSaveData(){
    this.OverHeadApportionment.type="Nominal";
    this.checkData();
    this._overheadService.createOverheadApportionmentCurrentDataDetailed(this.OverHeadApportionmentList).subscribe(e=>{
        e? this.notify.success("Saved Successfully") : this.notify.error("Something went wrong")
      })
  }

  getDetailedData(){
    this._overheadService.getOverheadApportionmentCurrentDataDetailed("Nominal").subscribe(e=>{
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
      this.cd.detectChanges()
    }
    })
  }

  calculate(val,index,field){
  if(field=='taxableSupplies')
  this.OverHeadApportionmentList[index].taxableSupplies = val 

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
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AppConsts } from '@shared/AppConsts';
import { TenantConfigurationServiceProxy } from '@shared/service-proxies/service-proxies';


export enum CachedData{
  vita_countries,
  vita_currencies
}

interface Consts  {
    isVita: boolean;
};

@Injectable()
export class GlobalConstsCustomService {


   data = new BehaviorSubject<Consts>({
    isVita: false,
  });

  data$ = this.data.asObservable();

  tenantType = new BehaviorSubject<string>('');

  tenantType$ = this.tenantType.asObservable();

  constructor(private _tenantConfigurations: TenantConfigurationServiceProxy) {
    _tenantConfigurations.getTenantConfigurationByTransactionType('General').subscribe(e=>{
      this.tenantType.next(e.tenantConfiguration.additionalData1 ?? 'GS')
    })
  }


  setData(newData: Consts){
    this.data.next(newData);
    if (newData.isVita){
      localStorage.setItem('isVita', 'true');
    }else{
      localStorage.setItem('isVita', 'false');
    }
    AppConsts.vitaMenu = newData.isVita;
  }

  setTenantType(tenantType:string){
    this.tenantType.next(tenantType);
  }

  getCache(data:CachedData){
    return JSON.parse(localStorage.getItem(data.toString()) || "null")
  }

  setCache(data:CachedData,val:any){
    localStorage.setItem(data.toString(),JSON.stringify(val))
  }

}

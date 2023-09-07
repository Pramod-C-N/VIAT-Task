import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    CustomersesServiceProxy,
    CustomersDto,
    SalesInvoicesServiceProxy,
    CreateOrEditSalesInvoiceDto,
    CreateOrEditSalesInvoicePartyDto,
    CreateOrEditSalesInvoiceItemDto,
    CreateOrEditSalesInvoiceSummaryDto,
    CreateOrEditSalesInvoiceVATDetailDto,
    CreateOrEditSalesInvoiceDiscountDto,
    CreateOrEditSalesInvoicePaymentDetailDto,
    CreateOrEditSalesInvoiceContactPersonDto,
    SalesInvoiceAddressDto,
    CreateOrEditSalesInvoiceAddressDto,
    CountryServiceProxy,
    GetCountryForViewDto,
    TenantBasicDetailsServiceProxy,
    CreateOrEditTenantBasicDetailsDto,
    GenerateXmlServiceProxy,
    ZatcaComplianceCSIDResponse,
    ZatcaDigitalSignature,
    XMLSignInput,
    Phase2RequestDto,
    ComplianceAPIError,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { throws } from 'assert';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { DateTime } from 'luxon';
import { Theme2ThemeUiSettingsComponent } from '@app/admin/ui-customization/theme2-theme-ui-settings.component';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BlockList } from 'net';

@Component({
    templateUrl: './phase-2.component.html',
    styleUrls: ['./phase-2.component.css'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class Phase2Component extends AppComponentBase {
  csidInfo: ZatcaComplianceCSIDResponse= new ZatcaComplianceCSIDResponse();
  signInfo: ZatcaDigitalSignature= new ZatcaDigitalSignature();
  phase2Input: Phase2RequestDto= new Phase2RequestDto();
  otp: string;
  csid:string;
  vatid:string;
  showXml:boolean=false;
  showStatus:Boolean=false;
  isSimplified:boolean=false;
  xml:string;
  clearenceStatus:string;
  complainceStatus:string;
  reportingStatus: string;
  reportingEm:any;
  em:any;
  clearenceEm:any;
    constructor(
        injector: Injector,
        private _xmlAppService: GenerateXmlServiceProxy,
        private _sessionService: AppSessionService,
        private router: Router
    ) {
        super(injector);
    }

    ngOnInit(){
     // this.sendOTP()
     this.vatid="300075588700003";
    }

    sendOTP(otp){
      this._xmlAppService.getCSID(otp).subscribe(e=>{
        console.log(e.item1)
        console.log(e.item2)
        this.showXml=true;
        this.phase2Input.success=e.item1.success;
        this.phase2Input.sign=e.item2;
        this.phase2Input.success.errors=[];
        this.csid=this.phase2Input?.success?.binarySecurityToken;
      })
    }

    sendToZatca(){
       this.phase2Input.xmlInput =new XMLSignInput();
       this.phase2Input.xmlInput.xml = this.xml;
       this.phase2Input.xmlInput.uuid= '16e78469-64af-406d-9cfd-895e724198f0';
       this.phase2Input.xmlInput.xml_uuid= "16e78469-64af-406d-9cfd-895e724198f0";
       this.phase2Input.xmlInput.qrPath = "";
       this.phase2Input.xmlInput.sellerName="Saudi Glass";
       this.phase2Input.xmlInput.vatId = "300075588700003";
       if(this.isSimplified)
       {
       this.phase2Input.xmlInput.totalAmount = 900.00;
       }
       else{
        this.phase2Input.xmlInput.totalAmount = 1001.00;

       }
       this.phase2Input.xmlInput.vatAmount = 150;
       this.phase2Input.xmlInput.issueDate = DateTime.now();
       this.phase2Input.xmlInput.xmlPath = "";
       this.phase2Input.xmlInput.pathToSave = "";
       this.phase2Input.xmlInput.xmlModel = null;
       this.phase2Input.xmlInput.irnno = 12;
       this.phase2Input.xmlInput.tenantId = 131;
       this.phase2Input.xmlInput.isPhase1  = false;      
       this._xmlAppService.sendToZatca(this.phase2Input).subscribe(e=>{
        console.log(e.item1)
        console.log(e.item2)
        console.log(e.item3)
        this.showStatus=true;
        this.csid=this.csidInfo?.success?.binarySecurityToken
        this.complainceStatus=e?.item2?.clearanceStatus;
        this.clearenceStatus=e?.item3?.clearanceStatus;
        this.reportingStatus=e?.item1?.reportingStatus;
        this.clearenceEm=e?.item3?.validationResults.errorMessages;
        this.reportingEm=e?.item1?.validationResults.errorMessages;
        this.em=e?.item2?.validationResults.errorMessages;
        
        console.log(this.em);
      })
    }
}

// import { NgModule } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';

// import { SalesDetailedReportRoutingModule } from './salesdetailedreport-routing.module';


// @NgModule({
//   schemas: [
//     NO_ERRORS_SCHEMA,
//     CUSTOM_ELEMENTS_SCHEMA
//   ],
//   declarations: [],
//   imports: [
//     CommonModule,
//     SalesDetailedReportRoutingModule
//   ]
// })
// export class SalesDetailedReportModule { }
import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { VendorReportRoutingModule } from './vendorreport-routing.module';
import { VendorReportComponent } from './vendorreport.component';
import {MultiSelectModule} from 'primeng/multiselect';

@NgModule({
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: [VendorReportComponent],
  imports: [AppSharedModule, VendorReportRoutingModule, AdminSharedModule,MultiSelectModule],
})
export class VendorReportModule {}


import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { PurchaseEntryRoutingModule } from './purchaseEntry-routing.module';
import { PurchaseEntryComponent } from './purchaseEntry.component';
import { TableModule } from 'primeng/table';
import {BsDatepickerModule} from 'ngx-bootstrap/datepicker';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
@NgModule({
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: [PurchaseEntryComponent],
  imports: [AppSharedModule, PurchaseEntryRoutingModule, AdminSharedModule,TableModule,BsDatepickerModule],
})
export class PurchaseEntryModule {}

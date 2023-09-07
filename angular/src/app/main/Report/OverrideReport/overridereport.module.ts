import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { OverrideReportRoutingModule } from './overridereport-routing.module';
import { OverrideReportComponent } from './overridereport.component';
import { MultiSelectModule } from 'primeng/multiselect';

@NgModule({
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: [OverrideReportComponent],
  imports: [AppSharedModule, OverrideReportRoutingModule, AdminSharedModule, MultiSelectModule],
})
export class OverrideReportModule { }

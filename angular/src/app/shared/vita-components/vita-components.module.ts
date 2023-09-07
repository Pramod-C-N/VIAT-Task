import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { ReportGridComponent } from './report-grid/report-grid.component';
@NgModule({
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: [ReportGridComponent],
  imports: [AppSharedModule, AdminSharedModule],
  exports:[ReportGridComponent]
})
export class VitaComponentsModule {}

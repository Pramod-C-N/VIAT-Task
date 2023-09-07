import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreditNoteReportRoutingModule } from './creditNoteReport-routing.module';
import { CreditNoteReportComponent } from './creditNoteReport.component';
@NgModule({
  declarations: [CreditNoteReportComponent],
  imports: [AppSharedModule, CreditNoteReportRoutingModule, AdminSharedModule],
})
export class CreditNoteReportModule {}

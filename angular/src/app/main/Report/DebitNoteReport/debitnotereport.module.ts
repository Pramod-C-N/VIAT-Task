import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DebitNoteReportRoutingModule } from './debitNoteReport-routing.module';
import { DebitNoteReportComponent } from './debitNoteReport.component';
@NgModule({
  declarations: [DebitNoteReportComponent],
  imports: [AppSharedModule, DebitNoteReportRoutingModule, AdminSharedModule],
})
export class DebitNoteReportModule {}

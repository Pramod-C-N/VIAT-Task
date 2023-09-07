import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { SalesReportRoutingModule } from './salesReport-routing.module';
import { SalesReportComponent } from './salesReport.component';
import { InvoiceComponentsModule } from '@app/shared/invoice-components/invoice-components.module';
import { FormGroupDirective } from '@angular/forms';

@NgModule({
  declarations: [SalesReportComponent],
  providers: [FormGroupDirective],
  imports: [AppSharedModule, SalesReportRoutingModule, AdminSharedModule,InvoiceComponentsModule],
})
export class SalesReportModule {}

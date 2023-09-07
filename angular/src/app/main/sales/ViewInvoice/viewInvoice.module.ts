import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ViewInvoiceRoutingModule } from './viewInvoice-routing.module';
import { ViewInvoiceComponent } from './viewInvoice.component';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { FormGroupDirective } from '@angular/forms';
import { AppCommonModule } from '@app/shared/common/app-common.module';
@NgModule({
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: [ViewInvoiceComponent],
  imports: [AppSharedModule, ViewInvoiceRoutingModule, AdminSharedModule,AppCommonModule],
  providers: [FormGroupDirective],
  exports:[ViewInvoiceComponent]

})
export class ViewInvoiceModule {}

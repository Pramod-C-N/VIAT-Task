import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreateDebitNotebradyRoutingModule } from './createDebitNotebrady-routing.module';
import { CreateDebitNotebradyComponent } from './createDebitNotebrady.component';
import { VitaComponentsModule } from '@app/shared/vita-components/vita-components.module';
import { InvoiceComponentsModule } from '@app/shared/invoice-components/invoice-components.module';
@NgModule({
  declarations: [CreateDebitNotebradyComponent],
  imports: [AppSharedModule, CreateDebitNotebradyRoutingModule, AdminSharedModule, VitaComponentsModule,InvoiceComponentsModule],
})
export class CreateDebitNotebradyModule {}

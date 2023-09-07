import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreateDebitNoteProfRoutingModule } from './createDebitNoteProf-routing.module';
import { CreateDebitNoteProfComponent } from './createDebitNoteProf.component';
import { InvoiceComponentsModule } from '@app/shared/invoice-components/invoice-components.module';
import { VitaComponentsModule } from '@app/shared/vita-components/vita-components.module';
@NgModule({
  declarations: [CreateDebitNoteProfComponent],
  imports: [AppSharedModule, CreateDebitNoteProfRoutingModule, AdminSharedModule, VitaComponentsModule,InvoiceComponentsModule],
})
export class CreateDebitNoteProfModule {}

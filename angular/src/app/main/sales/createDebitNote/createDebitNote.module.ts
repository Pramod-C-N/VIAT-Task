import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreateDebitNoteRoutingModule } from './createDebitNote-routing.module';
import { CreateDebitNoteComponent } from './createDebitNote.component';
@NgModule({
  declarations: [CreateDebitNoteComponent],
  imports: [AppSharedModule, CreateDebitNoteRoutingModule, AdminSharedModule],
})
export class CreateDebitNoteModule {}

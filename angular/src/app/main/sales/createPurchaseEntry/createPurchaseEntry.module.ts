import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreatePurchaseEntryRoutingModule } from './createPurchaseEntry-routing.module';
import { CreatePurchaseEntryComponent } from './createPurchaseEntry.component';
@NgModule({
  declarations: [CreatePurchaseEntryComponent],
  imports: [AppSharedModule, CreatePurchaseEntryRoutingModule, AdminSharedModule],
})
export class CreatePurchaseEntryModule {}

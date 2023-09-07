import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { PurchaseTransactionRoutingModule } from './purchaseTransaction-routing.module';
import { PurchaseTransactionComponent } from './purchaseTransaction.component';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
@NgModule({
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: [PurchaseTransactionComponent],
  imports: [AppSharedModule, PurchaseTransactionRoutingModule, AdminSharedModule],
  exports: [PurchaseTransactionComponent]
})
export class PurchaseTransactionModule {}

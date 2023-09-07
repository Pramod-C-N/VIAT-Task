import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CustomerRoutingModule } from './customers-routing.module';
import { CustomerComponent } from './customers.component';
import { CreateCustomerModule } from './createCustomers/createCustomers.module';
@NgModule({
  declarations: [CustomerComponent],
  imports: [AppSharedModule, CustomerRoutingModule, AdminSharedModule,CreateCustomerModule],
})
export class CustomerModule {}

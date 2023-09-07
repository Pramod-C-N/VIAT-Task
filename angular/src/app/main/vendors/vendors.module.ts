import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { VendorRoutingModule } from './vendors-routing.module';
import { VendorComponent } from './vendors.component';
import { CreateVendorModule } from './createVendors/createVendors.module';
@NgModule({
  declarations: [VendorComponent],
  imports: [AppSharedModule, VendorRoutingModule, AdminSharedModule,CreateVendorModule],
})
export class VendorModule {}

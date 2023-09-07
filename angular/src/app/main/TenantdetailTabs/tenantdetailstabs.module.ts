import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TenantDetailsTabsComponent } from './tenantdetailstabs.component';
import { TenantDetailTabsRoutingModule } from './tenantdetailstabs.routing.module';
import { EditTenantModalComponent } from '@app/admin/tenants/edit-tenant-modal.component';

@NgModule({
  declarations: [TenantDetailsTabsComponent
 ],
  imports: [AppSharedModule, TenantDetailTabsRoutingModule, AdminSharedModule],
  exports:[TenantDetailsTabsComponent]
})
export class TenantDetailsTabsModule {}

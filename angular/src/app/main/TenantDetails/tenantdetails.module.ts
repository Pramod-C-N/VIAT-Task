import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TenantDetailsComponent } from './tenantdetails.component';
import { TenantDetailRoutingModule } from './tenantdetails.routing.module';
import { TenantDetailsTabsModule } from '@app/main/TenantdetailTabs/tenantdetailstabs.module';
import { TenantDetailsTabsComponent } from '../TenantdetailTabs/tenantdetailstabs.component';

@NgModule({
  declarations: [TenantDetailsComponent
 ],
  imports: [AppSharedModule, TenantDetailRoutingModule, AdminSharedModule,TenantDetailsTabsModule],

})
export class TenantDetailsModule {}

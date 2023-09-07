import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TenantTypeRoutingModule } from './tenantType-routing.module';
import { TenantTypeComponent } from './tenantType.component';
import { CreateOrEditTenantTypeComponent } from './create-or-edit-tenantType.component';
import { ViewTenantTypeComponent } from './view-tenantType.component';

@NgModule({
    declarations: [TenantTypeComponent, CreateOrEditTenantTypeComponent, ViewTenantTypeComponent],
    imports: [AppSharedModule, TenantTypeRoutingModule, AdminSharedModule],
})
export class TenantTypeModule {}

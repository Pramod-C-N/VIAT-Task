import { NgModule } from '@angular/core';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TenantsRoutingModule } from './tenants-routing.module';
import { CreateTenantModalComponent } from './create-tenant-modal.component';
import { EditTenantModalComponent } from './edit-tenant-modal.component';
import { TenantFeaturesModalComponent } from './tenant-features-modal.component';
import { TenantsComponent } from './tenants.component';
import { TenantDetailsTabsModule } from '@app/main/TenantdetailTabs/tenantdetailstabs.module';

@NgModule({
    declarations: [
        CreateTenantModalComponent,
        EditTenantModalComponent,
        TenantFeaturesModalComponent,
        TenantsComponent ,
    ],
    imports: [AppSharedModule, AdminSharedModule, TenantsRoutingModule, TenantDetailsTabsModule
    ],
})
export class TenantsModule {}

import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TenantDetailsTabsComponent } from './tenantdetailstabs.component';
import { TenantDetailTabsRoutingModule } from './tenantdetailstabs.routing.module';
import { EditTenantModalComponent } from '@app/admin/tenants/edit-tenant-modal.component';
import { IndividualTenantComponent } from './IndivdualTenant/IndividualTenant.component';
import { GeneralTenantComponent } from './CompanyTenant/General/GeneralTenant.component';
import { LLPTenantComponent } from './CompanyTenant/LLP/LLPTenant.component';
import { LLCTenantComponent } from './CompanyTenant/LLC/LLCTenant.component';
import { JSCTenantComponent } from './CompanyTenant/JSC/JSCTenant.component';
import { ForeignTenantComponent } from './CompanyTenant/Foreign/ForeignTenant.component';
import { PETenantComponent } from './CompanyTenant/PE/PETenant.component';
import { NRCTenantComponent } from './CompanyTenant/NRC/NRCTenant.component';
import { GovernmentTenantComponent } from './CompanyTenant/Government/GovernmentTenant.component';
import { ConsoritiumTenantComponent } from './CompanyTenant/Consoritium/ConsoritiumTenant.component';

@NgModule({
    declarations: [
        TenantDetailsTabsComponent,
        IndividualTenantComponent,
        GeneralTenantComponent,
        LLPTenantComponent,
        LLCTenantComponent,
        JSCTenantComponent,
        ForeignTenantComponent,
        PETenantComponent,
        NRCTenantComponent,
        GovernmentTenantComponent,
        ConsoritiumTenantComponent
    ],
    imports: [AppSharedModule, TenantDetailTabsRoutingModule, AdminSharedModule],
    exports: [TenantDetailsTabsComponent],
})
export class TenantDetailsTabsModule {}
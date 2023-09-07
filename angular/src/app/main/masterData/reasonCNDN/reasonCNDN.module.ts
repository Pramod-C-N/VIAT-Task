import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ReasonCNDNRoutingModule } from './reasonCNDN-routing.module';
import { ReasonCNDNComponent } from './reasonCNDN.component';
import { CreateOrEditReasonCNDNComponent } from './create-or-edit-reasonCNDN.component';
import { ViewReasonCNDNComponent } from './view-reasonCNDN.component';

@NgModule({
    declarations: [ReasonCNDNComponent, CreateOrEditReasonCNDNComponent, ViewReasonCNDNComponent],
    imports: [AppSharedModule, ReasonCNDNRoutingModule, AdminSharedModule],
})
export class ReasonCNDNModule {}

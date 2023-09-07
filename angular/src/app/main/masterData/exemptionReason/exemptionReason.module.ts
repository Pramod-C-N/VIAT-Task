import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ExemptionReasonRoutingModule } from './exemptionReason-routing.module';
import { ExemptionReasonComponent } from './exemptionReason.component';
import { CreateOrEditExemptionReasonComponent } from './create-or-edit-exemptionReason.component';
import { ViewExemptionReasonComponent } from './view-exemptionReason.component';

@NgModule({
    declarations: [ExemptionReasonComponent, CreateOrEditExemptionReasonComponent, ViewExemptionReasonComponent],
    imports: [AppSharedModule, ExemptionReasonRoutingModule, AdminSharedModule],
})
export class ExemptionReasonModule {}

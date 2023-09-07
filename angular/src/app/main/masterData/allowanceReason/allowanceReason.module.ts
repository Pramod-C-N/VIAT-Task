import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AllowanceReasonRoutingModule } from './allowanceReason-routing.module';
import { AllowanceReasonComponent } from './allowanceReason.component';
import { CreateOrEditAllowanceReasonComponent } from './create-or-edit-allowanceReason.component';
import { ViewAllowanceReasonComponent } from './view-allowanceReason.component';

@NgModule({
    declarations: [AllowanceReasonComponent, CreateOrEditAllowanceReasonComponent, ViewAllowanceReasonComponent],
    imports: [AppSharedModule, AllowanceReasonRoutingModule, AdminSharedModule],
})
export class AllowanceReasonModule {}

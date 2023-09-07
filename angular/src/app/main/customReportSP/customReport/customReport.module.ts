import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CustomReportRoutingModule } from './customReport-routing.module';
import { CustomReportComponent } from './customReport.component';
import { CreateOrEditCustomReportModalComponent } from './create-or-edit-customReport-modal.component';
import { ViewCustomReportModalComponent } from './view-customReport-modal.component';

@NgModule({
    declarations: [CustomReportComponent, CreateOrEditCustomReportModalComponent, ViewCustomReportModalComponent],
    imports: [AppSharedModule, CustomReportRoutingModule, AdminSharedModule],
})
export class CustomReportModule {}

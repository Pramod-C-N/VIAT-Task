import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessProcessRoutingModule } from './businessProcess-routing.module';
import { BusinessProcessComponent } from './businessProcess.component';
import { CreateOrEditBusinessProcessComponent } from './create-or-edit-businessProcess.component';
import { ViewBusinessProcessComponent } from './view-businessProcess.component';

@NgModule({
    declarations: [BusinessProcessComponent, CreateOrEditBusinessProcessComponent, ViewBusinessProcessComponent],
    imports: [AppSharedModule, BusinessProcessRoutingModule, AdminSharedModule],
})
export class BusinessProcessModule {}

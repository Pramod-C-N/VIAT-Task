import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DesignationRoutingModule } from './designation-routing.module';
import { DesignationComponent } from './designation.component';
import { CreateOrEditDesignationComponent } from './create-or-edit-designation.component';
import { ViewDesignationComponent } from './view-designation.component';

@NgModule({
    declarations: [DesignationComponent, CreateOrEditDesignationComponent, ViewDesignationComponent],
    imports: [AppSharedModule, DesignationRoutingModule, AdminSharedModule],
})
export class DesignationModule {}

import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { OrganisationTypeRoutingModule } from './organisationType-routing.module';
import { OrganisationTypeComponent } from './organisationType.component';
import { CreateOrEditOrganisationTypeComponent } from './create-or-edit-organisationType.component';
import { ViewOrganisationTypeComponent } from './view-organisationType.component';

@NgModule({
    declarations: [OrganisationTypeComponent, CreateOrEditOrganisationTypeComponent, ViewOrganisationTypeComponent],
    imports: [AppSharedModule, OrganisationTypeRoutingModule, AdminSharedModule],
})
export class OrganisationTypeModule {}

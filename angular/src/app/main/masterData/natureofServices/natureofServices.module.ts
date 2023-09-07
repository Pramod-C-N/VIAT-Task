import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { NatureofServicesRoutingModule } from './natureofServices-routing.module';
import { NatureofServicesComponent } from './natureofServices.component';
import { CreateOrEditNatureofServicesComponent } from './create-or-edit-natureofServices.component';
import { ViewNatureofServicesComponent } from './view-natureofServices.component';

@NgModule({
    declarations: [NatureofServicesComponent, CreateOrEditNatureofServicesComponent, ViewNatureofServicesComponent],
    imports: [AppSharedModule, NatureofServicesRoutingModule, AdminSharedModule],
})
export class NatureofServicesModule {}

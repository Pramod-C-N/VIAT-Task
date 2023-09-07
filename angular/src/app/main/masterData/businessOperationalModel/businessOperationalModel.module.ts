import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessOperationalModelRoutingModule } from './businessOperationalModel-routing.module';
import { BusinessOperationalModelComponent } from './businessOperationalModel.component';
import { CreateOrEditBusinessOperationalModelComponent } from './create-or-edit-businessOperationalModel.component';
import { ViewBusinessOperationalModelComponent } from './view-businessOperationalModel.component';

@NgModule({
    declarations: [
        BusinessOperationalModelComponent,
        CreateOrEditBusinessOperationalModelComponent,
        ViewBusinessOperationalModelComponent,
    ],
    imports: [AppSharedModule, BusinessOperationalModelRoutingModule, AdminSharedModule],
})
export class BusinessOperationalModelModule {}

import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessTurnoverSlabRoutingModule } from './businessTurnoverSlab-routing.module';
import { BusinessTurnoverSlabComponent } from './businessTurnoverSlab.component';
import { CreateOrEditBusinessTurnoverSlabComponent } from './create-or-edit-businessTurnoverSlab.component';
import { ViewBusinessTurnoverSlabComponent } from './view-businessTurnoverSlab.component';

@NgModule({
    declarations: [
        BusinessTurnoverSlabComponent,
        CreateOrEditBusinessTurnoverSlabComponent,
        ViewBusinessTurnoverSlabComponent,
    ],
    imports: [AppSharedModule, BusinessTurnoverSlabRoutingModule, AdminSharedModule],
})
export class BusinessTurnoverSlabModule {}

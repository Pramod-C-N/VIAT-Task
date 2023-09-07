import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AffiliationRoutingModule } from './affiliation-routing.module';
import { AffiliationComponent } from './affiliation.component';
import { CreateOrEditAffiliationComponent } from './create-or-edit-affiliation.component';
import { ViewAffiliationComponent } from './view-affiliation.component';

@NgModule({
    declarations: [AffiliationComponent, CreateOrEditAffiliationComponent, ViewAffiliationComponent],
    imports: [AppSharedModule, AffiliationRoutingModule, AdminSharedModule],
})
export class AffiliationModule {}

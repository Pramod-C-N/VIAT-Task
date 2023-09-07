import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ConstitutionRoutingModule } from './constitution-routing.module';
import { ConstitutionComponent } from './constitution.component';
import { CreateOrEditConstitutionComponent } from './create-or-edit-constitution.component';
import { ViewConstitutionComponent } from './view-constitution.component';

@NgModule({
    declarations: [ConstitutionComponent, CreateOrEditConstitutionComponent, ViewConstitutionComponent],
    imports: [AppSharedModule, ConstitutionRoutingModule, AdminSharedModule],
})
export class ConstitutionModule {}

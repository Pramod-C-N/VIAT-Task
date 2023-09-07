import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { GenderRoutingModule } from './gender-routing.module';
import { GenderComponent } from './gender.component';
import { CreateOrEditGenderComponent } from './create-or-edit-gender.component';
import { ViewGenderComponent } from './view-gender.component';

@NgModule({
    declarations: [GenderComponent, CreateOrEditGenderComponent, ViewGenderComponent],
    imports: [AppSharedModule, GenderRoutingModule, AdminSharedModule],
})
export class GenderModule {}

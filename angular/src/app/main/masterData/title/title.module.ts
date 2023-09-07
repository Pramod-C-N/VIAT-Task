import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TitleRoutingModule } from './title-routing.module';
import { TitleComponent } from './title.component';
import { CreateOrEditTitleComponent } from './create-or-edit-title.component';
import { ViewTitleComponent } from './view-title.component';

@NgModule({
    declarations: [TitleComponent, CreateOrEditTitleComponent, ViewTitleComponent],
    imports: [AppSharedModule, TitleRoutingModule, AdminSharedModule],
})
export class TitleModule {}

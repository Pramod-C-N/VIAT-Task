import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { SectorRoutingModule } from './sector-routing.module';
import { SectorComponent } from './sector.component';
import { CreateOrEditSectorComponent } from './create-or-edit-sector.component';
import { ViewSectorComponent } from './view-sector.component';

@NgModule({
    declarations: [SectorComponent, CreateOrEditSectorComponent, ViewSectorComponent],
    imports: [AppSharedModule, SectorRoutingModule, AdminSharedModule],
})
export class SectorModule {}

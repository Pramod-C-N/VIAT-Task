import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { OverHeadApportionmentComponentRoutingModule } from './overHeadApportionment-routing.module';
import { OverHeadApportionmentComponent } from './overHeadApportionment.component';

@NgModule({
  declarations: [ OverHeadApportionmentComponent ],
  imports: [AppSharedModule, OverHeadApportionmentComponentRoutingModule, AdminSharedModule],
  exports: [OverHeadApportionmentComponent]
})
export class OverHeadApportionmentModule {}

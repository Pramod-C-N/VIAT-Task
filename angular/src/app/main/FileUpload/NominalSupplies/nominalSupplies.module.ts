import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { NominalSuppliesRoutingModule } from './nominalSupplies-routing.module';
import { NominalSuppliesComponent } from './nominalSupplies.component';

@NgModule({
  declarations: [ NominalSuppliesComponent ],
  imports: [AppSharedModule, NominalSuppliesRoutingModule, AdminSharedModule],
  exports: [NominalSuppliesComponent]
})
export class NominalSuppliesModule {}
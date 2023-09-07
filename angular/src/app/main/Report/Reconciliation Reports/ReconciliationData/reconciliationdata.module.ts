import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { reconciliationdataRoutingModule } from './reconciliationData-routing.module';
import { reconciliationdataComponent } from './reconciliationdatacomponent';
@NgModule({
  declarations: [reconciliationdataComponent],
  imports: [AppSharedModule, reconciliationdataRoutingModule, AdminSharedModule],
  exports:[reconciliationdataComponent]
})
export class reconciliationdataModule {}

import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { MasterConfigurationRoutingModule } from './masterConfiguration-routing.module';
import { MasterConfigurationComponent } from './masterConfiguration.component';
import { FormGroupDirective, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [ MasterConfigurationComponent ],
  imports: [AppSharedModule, MasterConfigurationRoutingModule, AdminSharedModule],
  exports: [MasterConfigurationComponent],

})
export class MasterConfigurationModule {}

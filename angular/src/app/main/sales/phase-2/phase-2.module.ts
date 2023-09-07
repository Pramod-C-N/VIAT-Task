import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { Phase2RoutingModule } from './phase-2-routing.module';
import { Phase2Component } from './phase-2.component';
@NgModule({
  declarations: [Phase2Component],
  imports: [AppSharedModule, Phase2RoutingModule, AdminSharedModule],
})
export class Phase2Module {}

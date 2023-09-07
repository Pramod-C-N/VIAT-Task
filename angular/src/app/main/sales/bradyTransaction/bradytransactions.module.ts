import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BradyTransactionsRoutingModule } from './bradytransactions-routing.module';
import { BradyTransactionsComponent } from './bradytransactions.component';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { VitaComponentsModule } from '@app/shared/vita-components/vita-components.module';

@NgModule({
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: [BradyTransactionsComponent],
  imports: [AppSharedModule, BradyTransactionsRoutingModule, AdminSharedModule,VitaComponentsModule],
  exports:[BradyTransactionsComponent]
})
export class BradyTransactionsModule {}

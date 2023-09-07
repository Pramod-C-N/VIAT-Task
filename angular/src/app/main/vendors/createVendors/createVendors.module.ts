import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreateVendorRoutingModule } from './createVendors-routing.module';
import { CreateVendorComponent } from './createVendors.component';
import {IndividualComponent} from './IndividualCustomer/IndividualCustomer.Component'
import {GovernmentComponent} from './Company/Government/Government.component'
import {GeneralComponent} from './Company/General/General.component'
import { LLPComponent } from './Company/LLP/LLP.Component';
import { LLCComponent } from './Company/LLC/LLC.Component';
import { JSCompanyComponent } from './Company/JSC/JSC.Component';
import { ForeignComponent } from './Company/Foreign/Foreign.Component';
import { PEComponent } from './Company/PE/PE.Component';
import { ConsortiumComponent } from './Company/Consortium/Consortium.Component';
import { NRCComponent } from './Company/NRC/NRC.Component';

@NgModule({
  declarations: [
    NRCComponent,
    ConsortiumComponent,
    PEComponent,ForeignComponent,JSCompanyComponent,LLCComponent,LLPComponent,GovernmentComponent,
    IndividualComponent,CreateVendorComponent,
    GeneralComponent
  ],
  imports: [AppSharedModule, CreateVendorRoutingModule, AdminSharedModule],
})
export class CreateVendorModule {}

import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AccountSharedModule } from '@account/shared/account-shared.module';
import { RegisterTenantComponent } from './register-tenant.component';
import { RegisterTenantRoutingModule } from './register-tenant-routing.module';
import { PasswordModule } from 'primeng/password';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {LanguageswitchModule} from '@account/language-switch.module'


@NgModule({
    imports: [AppSharedModule, AccountSharedModule, RegisterTenantRoutingModule, PasswordModule,FormsModule,  
        ReactiveFormsModule ,LanguageswitchModule  ],
    declarations: [RegisterTenantComponent],
})
export class RegisterTenantModule {}

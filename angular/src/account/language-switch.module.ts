import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { LanguageSwitchComponent } from './language-switch.component';
import { EditTenantModalComponent } from '@app/admin/tenants/edit-tenant-modal.component';

@NgModule({
  declarations: [LanguageSwitchComponent
 ],
  imports: [AppSharedModule, AdminSharedModule],
  exports:[LanguageSwitchComponent]
})
export class LanguageswitchModule {}

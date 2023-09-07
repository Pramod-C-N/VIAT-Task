import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { FileUploadOutputRoutingModule } from './fileUploadOutput-routing.module';
import { FileUploadOutputComponent } from './fileUploadOutput.component';

@NgModule({
  declarations: [
    FileUploadOutputComponent
  ],
  imports: [AppSharedModule, FileUploadOutputRoutingModule, AdminSharedModule],
  exports: [FileUploadOutputComponent]
})
export class FileUploadOutputModule {}

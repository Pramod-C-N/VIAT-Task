import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { ReportGridComponent } from './report-grid/report-grid.component';
import { DynamicErrorComponent } from './dynamic-field/dynamic-error/dynamic-error.component';
import { DynamicFieldComponent } from './dynamic-field/dynamic-field.component';
import { DynamicSelectComponent } from './dynamic-field/dynamic-select/dynamic-select.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DynamicInputComponent } from './dynamic-field/dynamic-input/dynamic-input.component';
import { DynamicTextAreaComponent } from './dynamic-field/dynamic-text-area/dynamic-text-area.component';
@NgModule({
    schemas: [NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA],
    declarations: [
        ReportGridComponent,
        DynamicFieldComponent,
        DynamicInputComponent,
        DynamicTextAreaComponent,
        DynamicSelectComponent,
        DynamicErrorComponent,
    ],
    imports: [FormsModule, ReactiveFormsModule, AppSharedModule, AdminSharedModule],
    exports: [
        ReportGridComponent,
        DynamicFieldComponent,
        DynamicInputComponent,
        DynamicTextAreaComponent,
        DynamicSelectComponent,
        DynamicErrorComponent,
    ],
})
export class VitaComponentsModule {}

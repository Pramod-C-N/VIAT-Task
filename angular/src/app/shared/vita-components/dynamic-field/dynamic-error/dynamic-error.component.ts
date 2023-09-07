import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormGroupDirective } from '@angular/forms';

@Component({
    selector: 'vita-dynamic-error',
    templateUrl: './dynamic-error.component.html',
    styleUrls: ['./dynamic-error.component.css'],
})
export class DynamicErrorComponent implements OnInit {
    formName: FormGroup;
    @Input() field: any;
    fieldName: string;
    formGroupName: string;

    constructor(private formgroupDirective: FormGroupDirective) {}

    ngOnInit() {
        this.fieldName = this.field?.fieldName;
        this.formGroupName = this.field?.formGroupName;
        if (this.formGroupName != '') this.formName = this.getChildControls(this.formGroupName);
        else this.formName = this.formgroupDirective.control;
    }

    getChildControls(child: string) {
        return this.formgroupDirective.control.get(child) as FormGroup;
    }
}

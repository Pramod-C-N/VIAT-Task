import { Component, ElementRef, Input, OnInit } from '@angular/core';
import { FormGroup, FormGroupDirective } from '@angular/forms';

@Component({
    selector: 'vita-dynamic-text-area',
    templateUrl: './dynamic-text-area.component.html',
    styleUrls: ['./dynamic-text-area.component.css'],
})
export class DynamicTextAreaComponent {
    @Input() field: any;
    formName: FormGroup;

    constructor(private formgroupDirective: FormGroupDirective, private el: ElementRef) {
        this.formName = formgroupDirective.control;
    }

    ngOnInit() {}

    ngAfterViewInit() {}
}

declare type ReportGridColumns= {
    field: string;
    header: string;
    align: string ;
    total: boolean ;
    sortable: boolean ;
    type: string ;
    transform: (val: number | string) => number | string;
    footer? : {
        type:string,
        value?:any
    }
    filter?:string
    searchable?: boolean 
}

declare type ReportGridDecimalConfig ={
    precision: number;
    format: string;
}

declare type ReportGridIntegerConfig ={
    format: string;
}

declare type ReportGridDateConfig ={
    format: string;
}

declare type ReportGridDateTimeConfig ={
    format: string;
}


declare type ReportGridURLConfig= {
    type:string;
    src:string;
    innerHtml:string
}

import { Injectable } from '@angular/core';
import { DateTime } from 'luxon';
import { BehaviorSubject } from 'rxjs';



@Injectable()
export class DateTimeCustomService {
  private data = new BehaviorSubject<any>({
    selectedDate:[DateTime.local().startOf('month'), DateTime.local().endOf('month')]
  });

  data$ = this.data.asObservable();

  setDate(selectedDate:any){
    console.log(selectedDate)
    this.data.next(selectedDate)
  }

  
}

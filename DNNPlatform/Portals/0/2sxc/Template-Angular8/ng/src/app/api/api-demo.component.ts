import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Data } from '@2sic.com/dnn-sxc-angular';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-api-demo',
  templateUrl: './api-demo.component.html',
  styleUrls: ['./api-demo.component.scss']
})
export class ApiDemoComponent implements OnInit {

  apiMessage$: Observable<string>;
  nameMessage$: Observable<string>;
  numbers$: Observable<number[]>;
  something$: Observable<Something>;

  constructor(data: Data) {
    // simple version for just a quick call, not re-using the api object
    this.numbers$ = data.api$<number[]>('simple/Numbers');

    // version for using the api-object many times;
    const simple = data.api('simple');

    // short call version - without parameters
    this.apiMessage$ = simple.get<string>('hello');

      // short call version - with parameters
    this.nameMessage$ = simple.get<string>('hello', new HttpParams().set('name', 'Michael'));

    this.something$ = simple.get<Something>('Something', new HttpParams().set('name', 'Samuel Jackson'));
   }

  ngOnInit() {
  }

}

interface Something {
  Name: string;
  Birthday: Date;
}

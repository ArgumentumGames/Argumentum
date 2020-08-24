import { Component, OnInit } from '@angular/core';
import { Context } from '@2sic.com/dnn-sxc-angular';

@Component({
  selector: 'app-title',
  templateUrl: './title.component.html',
  styleUrls: ['./title.component.scss']
})
export class TitleComponent implements OnInit {

  constructor(
    public context: Context
  ) { }

  ngOnInit() {
  }

}

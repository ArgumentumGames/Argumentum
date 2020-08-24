import { Component, Input } from '@angular/core';
import { Person } from '../models/person.model';
import { PersonsService } from '../persons.service';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-person-list',
    templateUrl: './person-list.component.html',
})
export class PersonListComponent {
  persons: Observable<Person[]>;

  @Input()
  set bu(value: string) {
    this.personSvc.BuFilter.next(value);
  }

  constructor(
      public personSvc: PersonsService,
  ) {
    this.persons = personSvc.Persons;
  }

}

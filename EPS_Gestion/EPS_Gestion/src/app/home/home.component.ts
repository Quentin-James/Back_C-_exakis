import { Component, OnInit } from '@angular/core';
import { MockStudentName } from './studentservice';

@Component({
  selector: 'app-home',
  standalone: true,
  template: `
  <section>
    <form>
      <input type="text" placeholder="Filter by city">
      <button class="primary" type="button">Search</button>
    </form>
    <ul>
      <li *ngFor="let student of students">
        {{ student.firstName }} {{ student.lastName }}
      </li>
    </ul>
  </section>
  `,
  styleUrls: ['./home/homehome.component.scss']
})
export class HomeComponent implements OnInit {
  students: { firstName: string, lastName: string }[] = [];

  constructor(private mockStudentName: MockStudentName) { }

  ngOnInit() {
    this.students = this.mockStudentName.getStudents();

  }
}


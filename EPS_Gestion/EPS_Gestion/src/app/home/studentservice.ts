import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MockStudentName {
  private firstNames = ['Jean', 'Jacque', 'Jacquie', 'Jacqueline', 'Jeaquot'];
  private lastNames = ['Michel', 'Moulin', 'Moulinex', 'Moule', 'Marmite'];

  private getRandomElement(arr: string[]): string {
    return arr[Math.floor(Math.random() * arr.length)];
  }

  private generateRandomStudent() {
    return {
      firstName: this.getRandomElement(this.firstNames),
      lastName: this.getRandomElement(this.lastNames)
    };
  }

  getStudents(count: number = 10) {
    const students = [];
    for (let i = 0; i < count; i++) {
      students.push(this.generateRandomStudent());
    }
    return students;
  }
}

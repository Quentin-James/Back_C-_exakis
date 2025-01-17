import { Component } from '@angular/core';
import { HomeComponent } from './home/home.component'; 

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    HomeComponent,

  ],
  template: `
  <main>
    <header class="brand-name">
      <img class="brand-logo" src="/assets/logo_sport.jpg" alt="logo" aria-hidden="true">
    </header>
    <section class="content">
      <app-home></app-home>
    </section>
  </main>
`,
  styleUrls: ['./home/home.component.scss']
})
export class AppComponent {
  title = 'EPS_Gestion';
}


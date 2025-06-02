import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="dashboard">
      <h2>Welcome to FarmSystem Dashboard 🌿</h2>
      <p>This is your main control panel.</p>
    </div>
  `,
  styles: [`
    .dashboard {
      padding: 2rem;
      text-align: center;
    }
  `]
})
export class DashboardComponent {}

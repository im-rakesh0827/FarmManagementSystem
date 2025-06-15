import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '@shared/services/user.service';
import { User } from '@shared/models/user.model';
import { AuthService } from '@app/shared/services/auth.service';
import { LoaderService } from '@shared/services/loader.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user!: User;
  error: string = '';

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private loaderService: LoaderService) { }

  ngOnInit(): void {
    this.loaderService.show();
    const email = this.authService.getEmailFromLocalStorage();

    if (email) {
    this.loaderService.hide();
      this.userService.getProfile(email).subscribe({
        next: (res) => this.user = res,
        error: () => this.error = 'Failed to load profile.'
      });
    } else {
      this.error = 'User is not authenticated.';
    }
  }
}

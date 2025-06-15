import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; // ✅ Required for *ngIf
import { LoaderService } from '../services/loader.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-loader',
  standalone: true,
  imports: [CommonModule], // ✅ Add CommonModule here for *ngIf to work
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss']
})
export class LoaderComponent implements OnInit {
  isLoading$!: Observable<boolean>;

  constructor(private readonly loaderService: LoaderService) {}

  ngOnInit(): void {
    this.isLoading$ = this.loaderService.isLoading$;
  }
}

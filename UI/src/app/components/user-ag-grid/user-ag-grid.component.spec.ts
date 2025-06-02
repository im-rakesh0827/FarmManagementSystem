import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAgGridComponent } from './user-ag-grid.component';

describe('UserAgGridComponent', () => {
  let component: UserAgGridComponent;
  let fixture: ComponentFixture<UserAgGridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserAgGridComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserAgGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

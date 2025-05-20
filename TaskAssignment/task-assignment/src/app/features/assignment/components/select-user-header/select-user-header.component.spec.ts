import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectUserHeaderComponent } from './select-user-header.component';

describe('SelectUserHeaderComponent', () => {
  let component: SelectUserHeaderComponent;
  let fixture: ComponentFixture<SelectUserHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SelectUserHeaderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SelectUserHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { fakeAsync, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvanceRealComponent } from './avance-real.component';

describe('AvanceRealComponent', () => {
  let component: AvanceRealComponent;
  let fixture: ComponentFixture<AvanceRealComponent>;

  beforeEach(fakeAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ AvanceRealComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvanceRealComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormBitacoraComponent } from './form-bitacora.component';

describe('FormBitacoraComponent', () => {
  let component: FormBitacoraComponent;
  let fixture: ComponentFixture<FormBitacoraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormBitacoraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormBitacoraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

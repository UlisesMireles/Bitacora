import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrosCalendarioComponent } from './registros-calendario.component';

describe('RegistrosCalendarioComponent', () => {
  let component: RegistrosCalendarioComponent;
  let fixture: ComponentFixture<RegistrosCalendarioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrosCalendarioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrosCalendarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

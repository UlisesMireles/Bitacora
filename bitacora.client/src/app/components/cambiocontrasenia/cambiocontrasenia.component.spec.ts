import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CambiocontraseniaComponent } from './cambiocontrasenia.component';

describe('CambiocontraseniaComponent', () => {
  let component: CambiocontraseniaComponent;
  let fixture: ComponentFixture<CambiocontraseniaComponent>;

  beforeEach(async () => {
    TestBed.configureTestingModule({
      declarations: [CambiocontraseniaComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CambiocontraseniaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

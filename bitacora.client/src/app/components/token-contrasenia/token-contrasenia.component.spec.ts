import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TokenContraseniaComponent } from './token-contrasenia.component';

describe('TokenContraseniaComponent', () => {
  let component: TokenContraseniaComponent;
  let fixture: ComponentFixture<TokenContraseniaComponent>;

  beforeEach(async() => {
    TestBed.configureTestingModule({
      declarations: [ TokenContraseniaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TokenContraseniaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

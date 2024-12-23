import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoriaDocumentiComponent } from './categoria-documenti.component';

describe('CategoriaDocumentiComponent', () => {
  let component: CategoriaDocumentiComponent;
  let fixture: ComponentFixture<CategoriaDocumentiComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoriaDocumentiComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CategoriaDocumentiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

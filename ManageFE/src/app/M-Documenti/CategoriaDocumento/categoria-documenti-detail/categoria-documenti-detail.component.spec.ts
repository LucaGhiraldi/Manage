import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoriaDocumentiDetailComponent } from './categoria-documenti-detail.component';

describe('CategoriaDocumentiDetailComponent', () => {
  let component: CategoriaDocumentiDetailComponent;
  let fixture: ComponentFixture<CategoriaDocumentiDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoriaDocumentiDetailComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CategoriaDocumentiDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

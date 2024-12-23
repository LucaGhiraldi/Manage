import { TestBed } from '@angular/core/testing';

import { CategoriaDocumentiService } from './categoria-documenti.service';

describe('CategoriaDocumentiService', () => {
  let service: CategoriaDocumentiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CategoriaDocumentiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

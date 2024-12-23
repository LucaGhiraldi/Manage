import { TestBed } from '@angular/core/testing';

import { FileDocumentiService } from './file-documenti.service';

describe('FileDocumentiService', () => {
  let service: FileDocumentiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FileDocumentiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { AssignmentValidationService } from './assignment-validation.service';

describe('AssignmentValidationService', () => {
  let service: AssignmentValidationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AssignmentValidationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

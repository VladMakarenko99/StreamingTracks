import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SoundtrackListComponent } from './soundtrack-list.component';

describe('SoundtrackListComponent', () => {
  let component: SoundtrackListComponent;
  let fixture: ComponentFixture<SoundtrackListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SoundtrackListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SoundtrackListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

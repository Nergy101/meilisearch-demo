import { Injectable, effect, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  public loading = signal(false);

  constructor() {
    effect(() => {
      if (this.loading()) {
        console.info('--- Loading started ---');
      } else {
        console.info('--- Loading finished ---');
      }
    });
  }
}

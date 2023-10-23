import { Injectable, WritableSignal, effect, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  public loading: WritableSignal<boolean | undefined> = signal(undefined);

  constructor() {
    effect(() => {
      if (this.loading() === true) {
        console.info('--- Loading started ---');
      } else if (this.loading() === false) {
        console.info('--- Loading finished ---');
      } else {
        // console.info('--- Loading not started or finished yet ---');
      }
    });
  }
}

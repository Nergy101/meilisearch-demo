import {
  ChangeDetectionStrategy,
  Component,
  effect,
  OnInit,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { SearchBarComponent } from './search-bar/search-bar.component';
import { ResultCardComponent } from './result-card/result-card.component';
import { LoadingComponent } from './loading/loading.component';
import { debounceTime, tap } from 'rxjs';
import { MoviesService } from './movies.service';
import { LoadingService } from './loading.service';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    CommonModule,
    MatCardModule,
    MatDividerModule,
    MatSnackBarModule,
    SearchBarComponent,
    ResultCardComponent,
    LoadingComponent,
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent implements OnInit {
  title = 'meilisearch-client-demo';

  searchTerm = signal('Story');
  searchOptions = signal<string[]>([]);

  constructor(
    public readonly loadingService: LoadingService,
    public readonly moviesService: MoviesService,
    private readonly snackbar: MatSnackBar
  ) {
    // apiState is OK
    effect(() => {
      if (this.moviesService.moviesApiState() == '✅') {
        this.snackbar.open('Movies-API returned Healthy✅!', 'Dismiss', {
          horizontalPosition: 'end',
          verticalPosition: 'bottom',
        });
      }
    });

    // get movies every time we change the searchTerm with debounce
    var searchTermDebounce = toObservable(this.searchTerm);
    searchTermDebounce
      .pipe(
        takeUntilDestroyed(), // takes care of cleaning up the subscription
        tap(() => this.loadingService.loading.set(true)),
        debounceTime(300),
        tap(async (val) => {
          await this.moviesService.getMovies(val);
        })
      )
      .subscribe();

    // change the dropdown-items everytime the movies-set changes
    effect(
      () => {
        this.searchOptions.set(
          this.moviesService.movies().map((m) => `${m.title} (${m.year})`)
        );
      },
      { allowSignalWrites: true } // allows writing to the signal this.searchOptions
    );
  }

  async ngOnInit(): Promise<void> {
    // polls repeatedly until API response is Healthy
    this.moviesService.checkHealth();
  }

  async onSearchTermChange(searchTerm: string): Promise<void> {
    this.searchTerm.set(searchTerm);
  }
}

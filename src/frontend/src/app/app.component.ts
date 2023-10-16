import { Component, effect, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { SearchBarComponent } from './search-bar/search-bar.component';
import { ResultCardComponent } from './result-card/result-card.component';
import { LoadingComponent } from './loading/loading.component';
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { catchError, interval, lastValueFrom, map, of, switchMap, tap, timer } from "rxjs";
import { Movie } from "./models/movie";
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    RouterOutlet,
    MatCardModule,
    MatDividerModule,
    SearchBarComponent,
    ResultCardComponent,
    LoadingComponent,
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'meilisearch-client-demo';
  baseUrl = environment.apiUrl;
  loading = true;

  apiState = signal<'✅' | '❌' | '❌🔁' | '🔁'>('🔁');
  moviesCount = signal<number>(0);
  movies = signal<Movie[]>([]);
  searchTerm = signal('Story');
  searchOptions = signal<string[]>([]);

  intervalId: any = undefined;

  constructor(private readonly httpClient: HttpClient) {
    effect(async () => {
      this.loading = true;
      await this.setMovies(this.searchTerm());
      this.loading = false;
    }, { allowSignalWrites: true });

    effect(() => {
      this.searchOptions.set(this.movies().map(m => `${m.title} (${m.year})`));
    }, { allowSignalWrites: true });
  }

  async ngOnInit(): Promise<void> {
    this.loading = true;
    const fetchData = async () => {
      lastValueFrom(this.httpClient.get(`${this.baseUrl}/health`, { responseType: 'text' }))
        .then(async state => {
          if (state == "Healthy") {
            this.apiState.set('✅');

            await this.setMovies(this.searchTerm());

            const totalMoviesCount = Number.parseInt(await lastValueFrom(this.httpClient.get(`${this.baseUrl}/movies/count`, { responseType: 'text' })));
            this.moviesCount.set(totalMoviesCount);
            this.loading = false;
            clearInterval(this.intervalId);
          } else {
            this.apiState.set('❌');
          }
        }).catch(_ => {
          this.apiState.set('❌🔁');
          this.loading = true;
        });

    }
    fetchData();

    this.intervalId = setInterval(fetchData, 3000);
  }

  async onSearchTermChange(searchTerm: string): Promise<void> {
    this.searchTerm.set(searchTerm);
  }

  private async setMovies(searchTerm: string): Promise<void> {
    const movies: Movie[] = await lastValueFrom(
      this.httpClient.get<Movie[]>(`${this.baseUrl}/search?query=${searchTerm}`)
    );
    this.movies.set(movies);
  }
}

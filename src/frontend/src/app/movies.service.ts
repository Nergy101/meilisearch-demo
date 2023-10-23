import { Injectable, Signal, computed, effect, signal } from '@angular/core';
import { Movie } from './models/movie';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { LoadingService } from './loading.service';

@Injectable({
  providedIn: 'root',
})
export class MoviesService {
  public moviesApiState = signal<'✅' | '❌' | '❌🔁' | '🔁'>('🔁');
  public totalMoviesCount = signal<number>(0);
  public movies = signal<Movie[]>([]);

  public moviesCount: Signal<number>;

  private baseUrl = environment.apiUrl;
  private intervalId: any = undefined;

  constructor(
    private readonly httpClient: HttpClient,
    private readonly loadingService: LoadingService
  ) {
    // clear interval once apiState is OK
    effect(() => {
      const state = this.moviesApiState();
      console.info(`Movies API responded: ${state}`);

      if (state == '✅') {
        clearInterval(this.intervalId);
      }
    });

    this.moviesCount = computed(() => this.movies().length);
  }

  public async checkHealth(): Promise<void> {
    if (this.moviesApiState() == '✅') {
      return;
    }

    this.moviesApiState.set('❌🔁');
    this.loadingService.loading.set(true);
    const check = async () => {
      try {
        const state = await lastValueFrom(
          this.httpClient.get(`${this.baseUrl}/health`, {
            responseType: 'text',
          })
        );
        if (state == 'Healthy') {
          this.moviesApiState.set('✅');

          // set total amount of movies once from API
          const totalMoviesCount = Number.parseInt(
            await lastValueFrom(
              this.httpClient.get(`${this.baseUrl}/movies/count`, {
                responseType: 'text',
              })
            )
          );
          this.totalMoviesCount.set(totalMoviesCount);

          // initial search
          this.getMovies('Story');
        } else {
          this.moviesApiState.set('❌');
        }
      } catch (_) {}
    };

    this.intervalId = setInterval(check, 3000);
  }

  public async getMovies(searchTerm: string): Promise<void> {
    if (this.moviesApiState() != '✅') {
      return;
    }

    try {
      this.loadingService.loading.set(true);
      const movies: Movie[] = await lastValueFrom(
        this.httpClient.get<Movie[]>(
          `${this.baseUrl}/movies/search?query=${searchTerm}`
        )
      );
      this.movies.set(movies);
      this.loadingService.loading.set(false);
    } catch (_) {
      this.checkHealth();
    }
  }
}

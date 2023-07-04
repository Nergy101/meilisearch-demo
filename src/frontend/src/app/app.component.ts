import {Component, effect, OnInit, signal} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterOutlet} from '@angular/router';
import {MatCardModule} from '@angular/material/card';
import {MatDividerModule} from '@angular/material/divider';
import {SearchBarComponent} from './search-bar/search-bar.component';
import {ResultCardComponent} from './result-card/result-card.component';
import {LoadingComponent} from './loading/loading.component';
import {HttpClient, HttpClientModule} from "@angular/common/http";
import {lastValueFrom} from "rxjs";
import {Movie} from "./models/movie";

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
  baseUrl = 'http://localhost:5171'
  loading = false;

  movies = signal<Movie[]>([]);
  searchTerm = signal('Story');
  searchOptions = signal<string[]>([]);

  constructor(private readonly httpClient: HttpClient) {
    effect(async () => {
      await this.setMovies(this.searchTerm())
    }, {allowSignalWrites: true})

    effect(() => {
      this.searchOptions.set(this.movies().map(m => `${m.title} (${m.year})`))
    }, {allowSignalWrites: true})
  }

  async ngOnInit(): Promise<void> {
  }

  async onSearchTermChange(searchTerm: string): Promise<void> {
    this.searchTerm.set(searchTerm);
  }

  private async setMovies(searchTerm: string): Promise<void> {
    const movies: Movie[] = await lastValueFrom(
      this.httpClient.get<Movie[]>(`${this.baseUrl}/search/${searchTerm}`)
    );

    this.movies.set(movies);
  }
}

import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { SearchBarComponent } from './search-bar/search-bar.component';
import { ResultCardComponent } from './result-card/result-card.component';
import { LoadingComponent } from './loading/loading.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
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

  loading = false;
  movies = signal([]);

  searchTerm = 'Hero';

  ngOnInit(): void {
    this.movies.set(
      JSON.parse(
        `[
{
"id": "Id287455e4-abcd-4156-a9f5-fe0ee2415855",
"title": "Titlebfe38eea-b54b-47e6-b7e4-0953c54b028d",
"genres": [
"42e4e671-fa3c-440a-85a2-7746b71f0796",
"ec604093-aedb-487b-a24b-d2cb01d0ce83",
"0dbd6bd8-44ff-4b87-8ace-2b367c61643f"
]
},
{
"id": "Id45f66eb0-a757-439a-abcd-409feb018dab",
"title": "Title2d6c7a82-82d5-4bdd-b028-2164faf85cdd",
"genres": [
"2b052de2-0ae0-4b6c-b8b9-1ea2031deb4f",
"cf5f425f-6169-4f8c-bfac-22b93c3e2c7c",
"2814f10c-1d91-41f6-aa45-35bf173b6f6b"
]
},
{
"id": "Id2d6cd9a9-8cf6-475f-abcd-51ed69018052",
"title": "Title70d8badc-20c3-47d7-8f18-2a7b459be22d",
"genres": [
"fc4847ae-b543-41b0-96df-b910f6a7f841",
"d0ed3417-44ed-4178-8fb3-5d82c5471133",
"412b15b4-d3d2-48cd-b2c2-a467c62235bd"
]},
{
"id": "Id2d6cd9a9-8cf6-475f-abcd-51ed69018052",
"title": "Title70d8badc-20c3-47d7-8f18-2a7b459be22d",
"genres": [
"fc4847ae-b543-41b0-96df-b910f6a7f841",
"d0ed3417-44ed-4178-8fb3-5d82c5471133",
"412b15b4-d3d2-48cd-b2c2-a467c62235bd"
]
}]`
      )
    );
  }
}

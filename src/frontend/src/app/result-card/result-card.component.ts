import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { Movie } from '../models/movie';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'result-card',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatCardModule,
    MatButtonModule,
    FormsModule,
  ],
  templateUrl: './result-card.component.html',
  styleUrls: ['./result-card.component.scss'],
})
export class ResultCardComponent {
  @Input()
  item?: Movie;

  @Output()
  onHide = new EventEmitter<number>();

  public onSearch(): void {
    const googleSearchURL = `https://www.google.com/search?q=${this.item?.title} ${this.item?.year} movie`;

    window.location.href = googleSearchURL;
  }

  public onHideClicked(): void {
    this.onHide.emit(this.item?.id);
  }
}

import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {CommonModule} from '@angular/common';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {FormControl, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {Observable, from, map, startWith, tap} from 'rxjs';

@Component({
  selector: 'search-bar',
  standalone: true,
  imports: [
    CommonModule,
    MatAutocompleteModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss'],
})
export class SearchBarComponent implements OnInit {

  @Input()
  options: string[] = ['Super Hero', 'A Hero', 'Heroes beating villains!'];

  @Input()
  searchTerm = '';

  @Output()
  searchTermChange = new EventEmitter<string>();

  myControl: FormControl<string | null> | undefined;


  filteredOptions: Observable<string[]> = from([]);

  ngOnInit() {
    this.myControl = new FormControl(this.searchTerm);

    this.filteredOptions = this.myControl.valueChanges
      .pipe(
        tap((val) => {
          this.searchTerm = val ?? '';
          this.searchTermChange.emit(val ?? '');
        })
      )
      .pipe(
        startWith(''),
        map((value) => this._filter(value || ''))
      );
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.options.filter((option) =>
      option.toLowerCase().includes(filterValue)
    );
  }
}

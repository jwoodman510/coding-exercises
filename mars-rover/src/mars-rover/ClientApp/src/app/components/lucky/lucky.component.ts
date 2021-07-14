import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { delay, finalize, map, switchMap, tap } from 'rxjs/operators';
import { MarsRoverService } from '../../services';

@Component({
  selector: 'app-lucky',
  templateUrl: './lucky.component.html',
  styleUrls: ['./lucky.component.scss']
})
export class LuckyComponent implements OnInit {
  readonly imgSrc$: Observable<string>;
  readonly details$: Observable<string>;
  readonly loading$: Observable<boolean>;

  private readonly _details = new BehaviorSubject<string>('');
  private readonly _imgSrc = new BehaviorSubject<string>('');
  private readonly _loading = new BehaviorSubject<boolean>(true);

  constructor(private roverService: MarsRoverService) {
    this.imgSrc$ = this._imgSrc.asObservable();
    this.loading$ = this._loading.asObservable();
    this.details$ = this._details.asObservable();
  }

  ngOnInit(): void {
    this.testMyLuck();
  }

  testMyLuck(): void {
    of(true).pipe(
      tap(x => this._loading.next(x)),
      switchMap(() => this.getRandomImgSrc()),
      tap(x => this._imgSrc.next(x)),
      finalize(() => this._loading.next(false))
    ).subscribe();
  }

  private getRandomImgSrc(): Observable<string> {
    return this.roverService.getRovers().pipe(
      map(rovers => this.getRandomArrayElement(rovers)),
      map(rover => ({ rover: rover.name, date: this.getRandomDateString() })),
      tap(x => this._details.next(`${x.rover}: ${x.date}`)),
      switchMap(x => this.roverService.getPhotos(x.rover, x.date)),
      map(photos => this.getRandomArrayElement(photos)),
      map(photo => photo ? photo.imgSrc : '')
    );
  }

  private getRandomArrayElement<T>(array: Array<T>): T {
    return array[Math.floor(Math.random() * array.length)];
  }

  private getRandomDateString(): string {
    const start = new Date(2018, 0, 1);
    const end = new Date();
    

    const date = new Date(start.getTime() + Math.random() * (end.getTime() - start.getTime()));

    return `${date.getMonth() + 1}-${date.getDate()}-${date.getFullYear()}`;
  }
}

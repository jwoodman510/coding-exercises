import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Photo, Rover } from '../models';

@Injectable()
export class MarsRoverService {
  private readonly _baseUrl: string;

  constructor(private http: HttpClient) {
    this._baseUrl = `api/v1/rover`;
  }

  getRovers(): Observable<Array<Rover>> {
    return this.http.get<Array<Rover>>(this._baseUrl);
  }

  getPhotos(rover: string, date: string): Observable<Array<Photo>> {
    return this.http.get<Array<Photo>>(`${this._baseUrl}/photos?rover=${rover}&date=${date}`);
  }
}

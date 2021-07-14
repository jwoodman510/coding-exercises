import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Rover } from '../models';

@Injectable()
export class GalleryService {
  private readonly _baseUrl: string;

  constructor(private http: HttpClient) {
      this._baseUrl = `api/v1/gallery`;
  }

  get(): Observable<any> {
    return this.http.get<Array<Rover>>(this._baseUrl);
  }
}

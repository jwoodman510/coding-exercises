import { Component } from '@angular/core';
import { GalleryService } from 'src/app/services';
import { Observable } from 'rxjs';
import { Rover } from 'src/app/models/rover';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss']
})
export class GalleryComponent {
  readonly rovers$: Observable<Array<Rover>>;

  constructor(private galleryService: GalleryService) {
    this.rovers$ = this.galleryService.get();
  }
}

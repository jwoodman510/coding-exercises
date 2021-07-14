import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { GalleryComponent, LuckyComponent } from './components';
import { GalleryService, MarsRoverService } from './services';

@NgModule({
  declarations: [
    AppComponent,
    GalleryComponent,
    LuckyComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', pathMatch: 'full', redirectTo: '/gallery' },
      { path: 'gallery', component: GalleryComponent, pathMatch: 'full' },
      { path: 'lucky', component: LuckyComponent, pathMatch: 'full' }
    ])
  ],
  providers: [GalleryService, MarsRoverService],
  bootstrap: [AppComponent]
})
export class AppModule { }

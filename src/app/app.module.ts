import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BoardComponent } from './game/board/board.component';
import { SquareComponent } from './game/square/square.component';
import { InputService } from './services/input.service';
import { MenuComponent } from './menu/menu.component';
import { GameService } from './services/game.service';
import { SignalRService } from './services/signalr.service';
import { SignalRController } from './signalr.controller';

@NgModule({
  declarations: [
    AppComponent,
    BoardComponent,
    SquareComponent,
    MenuComponent
  ],
  imports: [
    BrowserModule, HttpClientModule
  ],
  providers: [InputService, GameService, SignalRService, SignalRController],
  bootstrap: [AppComponent]
})
export class AppModule { }

import { Component, OnInit } from '@angular/core';
import { Observable, pipe, pluck, take } from 'rxjs';
import { BoardState, GameState, WhichBoard } from './game/GameVariables';
import { GameService } from './services/game.service';
import { SignalRService } from './services/signalr.service';

@Component({
   selector: 'app-root',
   templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
   title = 'app';
   connId = '';

   //gameState$?: Observable<GameState>;
   boardLeft$?: Observable<BoardState>;
   boardRight$?: Observable<BoardState>;

   constructor(
      private signalRService: SignalRService,
      private gameService: GameService
   ) {

   }

   ngOnInit() {

      this.signalRService.startConnection();
      this.getBoardState();
      //this.getGameState();

   }

   getBoardState() {
      this.boardLeft$ = this.gameService.getLeftBoardState();
      this.boardRight$ = this.gameService.getRightBoardState();
   }

   //getGameState() {
   //   this.gameState$ = this.gameService.getGameState()
   //}
}

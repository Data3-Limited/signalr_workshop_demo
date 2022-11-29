import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, catchError, Observable, tap } from "rxjs";
import { BoardState, Coord, GameState, PlayerType, WhichBoard } from "../game/GameVariables";

@Injectable({
   providedIn: 'root'
})
export class GameService {
   API_BASE = 'https://localhost:7045/api';

   private player$: BehaviorSubject<PlayerType> = new BehaviorSubject<PlayerType>(PlayerType.spectator);
   private BoardLeft$: BehaviorSubject<BoardState> = new BehaviorSubject<BoardState>({});
   private BoardRight$: BehaviorSubject<BoardState> = new BehaviorSubject<BoardState>({});

   constructor(private httpClient: HttpClient) {
      //this.player$.pipe(tap(console.log)).subscribe();
   }

   setBoardState(player: PlayerType, state: any): void {
      this.player$.next(player);

      switch (player) {
         case PlayerType.player1:
         case PlayerType.player2:
            this.BoardRight$.next({ ...state, drawBoats: true });
            break;
         case PlayerType.spectator:
            this.BoardLeft$.next(state.left);
            this.BoardRight$.next(state.right);
            break;
      }
   }

   attackSquare(target: PlayerType, state: any, successful: boolean) {
      switch (this.player$.getValue()) {
         case PlayerType.player1:
            if (target == PlayerType.player1) {
               this.BoardRight$.next({ ...state, drawBoats: true });
            } else {
               this.BoardLeft$.next({ ...state, drawBoats: false });
            }
            break;
         case PlayerType.player2:
            if (target == PlayerType.player2) {
               this.BoardRight$.next({ ...state, drawBoats: true });
            } else {
               this.BoardLeft$.next({ ...state, drawBoats: false });
            }
            break;
         case PlayerType.spectator:
            if (target == PlayerType.player1) {
               this.BoardLeft$.next({ ...state, drawBoats: true });
            } else {
               this.BoardRight$.next({ ...state, drawBoats: true });
            }
            break;
      }
   }

   getLeftBoardState(): Observable<BoardState> {
      return this.BoardLeft$;
   }

   getRightBoardState(): Observable<BoardState> {
      return this.BoardRight$
   }

   resetBoard(): void {
      let blankBoard: BoardState = {
         squares: []
      }
      this.BoardLeft$.next(blankBoard);
      this.BoardRight$.next(blankBoard);
   }
}
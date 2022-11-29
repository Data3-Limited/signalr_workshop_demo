export interface BoardSize {
  width: number;
  height: number;
}

export interface Coord {
  x: number;
  y: number;
}

export enum squareStatus {
   untouched,
   miss,
   hit,
   boat
}

export interface SquareState {
   coord: Coord,
   status: squareStatus
}

export interface BoardState {
   player?: PlayerType;
   squares?: SquareState[];
}

export enum PlayerType {
   player1,
   player2,
   spectator
}

export interface GameState {
   playerType?: PlayerType,
   leftBoard?: BoardState,
   rightBoard?: BoardState
}

export enum WhichBoard {
   left,
   right
}
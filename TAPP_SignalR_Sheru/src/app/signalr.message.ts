import { Coord } from "./game/GameVariables"

export enum SignalRClientMethods {
   reset,
   join,
   attack
}

export enum SignalRServerMethods {
   error,
   info,
   reset,
   joined,
   attack,
   attackResponse,
   getBoardState
}

export enum PlayerTypes {
   player1,
   player2,
   spectator
}

export interface MessageJoin {
   type: PlayerTypes
}

export interface MessageAttack {
   coord: Coord
}

export interface SignalRMessage {
   Method: SignalRClientMethods,
   Message: any
}

export interface SignalRServerMessage {
   method: SignalRServerMethods,
   message: any
}
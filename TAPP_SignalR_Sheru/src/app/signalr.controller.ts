/***********************************************************
 * 
 *  This class is simply to centralise any outgoing signalr commands.
 *  For the purposes of the workshop demonstration, this should
 *  make SignalR communication slightly easier to track.
 * 
 **********************************************************/

import { Injectable } from '@angular/core';
import { Coord, PlayerType } from './game/GameVariables';
import { SignalRService } from './services/signalr.service';
import { SignalRClientMethods, SignalRMessage } from './signalr.message';

@Injectable({ providedIn: 'root' })
export class SignalRController {
   constructor(
      private signalRService: SignalRService,
   ) { }

   sendReset(): void {
      this.signalRService.sendMessage(this.createMessage(SignalRClientMethods.reset, ''));
   }

   sendAttack(coordinates: Coord): void {
      this.signalRService.sendMessage(this.createMessage(SignalRClientMethods.attack, coordinates));
   }

   sendJoin(player: PlayerType): void {
      this.signalRService.sendMessage(this.createMessage(SignalRClientMethods.join, player));
   }

   /*
    * Helper methods
    * 
    */
   createMessage(type: SignalRClientMethods, message: any): SignalRMessage {
      return {
         Method: type,
         Message: message
      }
   }
}

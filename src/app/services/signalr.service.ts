import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { PlayerType } from '../game/GameVariables';
import { SignalRMessage, SignalRServerMessage, SignalRServerMethods } from '../signalr.message';
import { GameService } from './game.service';

@Injectable({ providedIn: 'root' })
export class SignalRService {
   constructor(
      private gameService: GameService
   ) { }
    connected = false;

    hubConnection: signalR.HubConnection | undefined;

    startConnection = async () => {
        this.hubConnection = await new signalR.HubConnectionBuilder()
            .withUrl('https://localhost:7045/LiveUpdateHub', {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets
            }).build();

        this.hubConnection
            .start()
            .then(() => {
              console.log("SignalR connection successful");
              console.log(this.hubConnection);
                // Receive messages from server once connection is established
               this.hubConnection?.on("FromServer", (payload: SignalRServerMessage) => {
                  switch (payload.method) {
                     case SignalRServerMethods.info:
                        console.log("[INFO]: " + payload.message);
                        break;
                     case SignalRServerMethods.error:
                        console.log("[ERROR]: " + payload.message);
                        break;
                     case SignalRServerMethods.attack:
                        break;
                     case SignalRServerMethods.attackResponse:
                         this.gameService.attackSquare(payload.message.target, payload.message.boardState, payload.message.successful);
                         break;
                      case SignalRServerMethods.getBoardState:
                         break;
                      case SignalRServerMethods.joined:
                         this.gameService.setBoardState(payload.message.type, payload.message.boardState);
                         break;
                      case SignalRServerMethods.reset:
                         this.gameService.resetBoard();
                         break;
                   }
                })
              this.connected = true;
            })
        .catch(err => console.log("Error while starting connection: " + err));

    }

    sendMessage(payload: SignalRMessage) {
        this.hubConnection?.invoke("FromClient", JSON.stringify(payload))
            .catch(err => console.error(err));
    }

}

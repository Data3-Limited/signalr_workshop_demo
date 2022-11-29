import { Component, OnDestroy, OnInit } from '@angular/core';
import { PlayerType } from '../game/GameVariables';
import { InputService } from '../services/input.service';
import { SignalRController } from '../signalr.controller';

@Component({
   selector: 'app-menu',
   templateUrl: './menu.component.html',
   styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

   constructor(
      private signalRController: SignalRController,
      private dataService: InputService,
   ) { }

   ngOnInit() {
   }

   btnClick(btn: string) {
      this.dataService.buttonClicked(btn);
      switch (btn) {
         case 'reset':
            this.signalRController.sendReset();
            break;
         case 'p1':
            this.signalRController.sendJoin(PlayerType.player1);
            break;
         case 'p2':
            this.signalRController.sendJoin(PlayerType.player2);
            break;
         case 'spectator':
            this.signalRController.sendJoin(PlayerType.spectator);
            break;
      }
   }

}

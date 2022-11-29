import { Component, EventEmitter, HostBinding, HostListener, Input, OnInit, Output } from '@angular/core';
import { InputService } from '../../services/input.service';
import * as gameVars from '../GameVariables';

@Component({
   selector: 'app-square',
   templateUrl: './square.component.html',
   styleUrls: ['./square.component.css']
})
export class SquareComponent implements OnInit {
   @Input() coord: gameVars.Coord = { x: 0, y: 0 };

   @HostBinding('class.hit') statusHit: boolean = false;
   @HostBinding('class.miss') statusMiss: boolean = false;
   @HostBinding('class.untouched') statusUntouched: boolean = true;
   @HostBinding('class.boat') statusBoat: boolean = false;

   @HostListener("click") onClick() {
      this.dataService.squareClicked(this);
   }

   squareContent!: string;

   constructor(private dataService: InputService) { }

   ngOnInit() {
      this.resetSquareContent();
   }

   public setSquareStatus(status: gameVars.squareStatus) {

      if (status == this.getStatus()) return;

      // Set class and content
      this.statusUntouched = false;
      this.statusHit = false;
      this.statusMiss = false;
      this.statusBoat = false;

      switch (status) {
         case gameVars.squareStatus.untouched:
            this.resetSquareContent();
            this.statusUntouched = true;
            break;
         case gameVars.squareStatus.hit:
            this.setSquareContent('*');
            this.statusHit = true;
            break;
         case gameVars.squareStatus.miss:
            this.setSquareContent('');
            this.statusMiss = true;
            break;
         case gameVars.squareStatus.boat:
            this.squareContent = 'B'
            this.statusBoat = true;
            break;
      }
   }

   public resetSquareContent(): void {
      this.squareContent = `${this.coord.x},${this.coord.y}`;
   }

   public setSquareContent(content: string) {
      this.squareContent = content;
   }

   public getStatus(): gameVars.squareStatus {
      if (this.statusBoat) return gameVars.squareStatus.boat;
      if (this.statusHit) return gameVars.squareStatus.hit;
      if (this.statusMiss) return gameVars.squareStatus.miss;
      if (this.statusUntouched) return gameVars.squareStatus.untouched;
      return -1;
   }

   public getCoord(): gameVars.Coord {
      return this.coord;
   }

}

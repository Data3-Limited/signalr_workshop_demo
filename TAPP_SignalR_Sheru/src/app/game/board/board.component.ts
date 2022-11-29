import { AfterContentInit, Component, ContentChildren, Input, OnChanges, OnInit, QueryList, SimpleChange, SimpleChanges, ViewChildren } from '@angular/core';
import { InputService } from '../../services/input.service';
import { BoardSize, Coord } from '../GameVariables';
import { SquareComponent } from '../square/square.component';
import * as gameVars from '../GameVariables';
import { SignalRController } from '../../signalr.controller';
import { SignalRMessage } from '../../signalr.message';

@Component({
   selector: 'app-board',
   templateUrl: './board.component.html',
   styleUrls: ['./board.component.css']
})
export class BoardComponent implements OnInit, OnChanges, AfterContentInit {

   @Input() boardState?: gameVars.BoardState | null;

   @ViewChildren('square') squares!: QueryList<SquareComponent>;

   Dimensions: BoardSize = { width: 10, height: 10 }

   constructor(
      private dataService: InputService,
      private signalRController: SignalRController
   ) { }


   ngOnInit(): void {
   }

   ngAfterContentInit() {
      this.dataService.squareClickEmitter.subscribe((val) => {
         let test = this.findSquare(val);
         if (test != null) {
            this.signalRController.sendAttack(test.getCoord());
         }
      });
   }

   ngOnChanges(changes: SimpleChanges): void {
      if (changes['boardState'].currentValue.squares) {
         this.resetBoard();
         for (let square of changes['boardState'].currentValue.squares) {
            let test = this.findSquareCoord(square.coord);
            if (test != null) {
               if (square.status != gameVars.squareStatus.boat) {
                  test.setSquareStatus(square.status);
               }
               if (square.status == gameVars.squareStatus.boat && changes['boardState'].currentValue.drawBoats) {
                  test.setSquareStatus(square.status);
               }
            }
         }
      }

   }

   findSquare(square: SquareComponent): SquareComponent | void {
      return this.squares.find(s => s == square);
   }

   findSquareCoord(coord: Coord): SquareComponent | void {
      return this.squares.find(square => square.coord.x == coord.x && square.coord.y == coord.y);
   }

   resetBoard(): void {
      for (let x = 0; x < this.Dimensions.width; x++) {
         for (let y = 0; y < this.Dimensions.height; y++) {
            let square = this.findSquareCoord(this.BuildCoord(x, y));
            if (square != null) square.setSquareStatus(gameVars.squareStatus.untouched);
         }
      }
   }

   BuildCoord = (x: number, y: number): Coord => {
      return { x: x, y: y };
   }
}

import { EventEmitter, Injectable } from "@angular/core";
import { Coord } from "../game/GameVariables";
import { SquareComponent } from "../game/square/square.component";

@Injectable()
export class InputService {

   // Emitters
   squareClickEmitter = new EventEmitter<SquareComponent>();
   buttonClickEmitter = new EventEmitter<string>();

   // Events
   squareClicked(square: SquareComponent) {
      this.squareClickEmitter.emit(square);
   }

   buttonClicked(btn: string) {
      this.buttonClickEmitter.emit(btn);
   }
}
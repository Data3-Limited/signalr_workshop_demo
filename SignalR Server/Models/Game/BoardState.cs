using SignalR_Server.Interfaces;
using TAPP_SignalR.Models;
using static SignalR_Server.GameValues;

namespace SignalR_Server.Models.Game
{
   public class BoardState : IBoardState
   {

      public PlayerTypes? player { get; set; }
      public List<SquareState> Squares { get; set; } = new();

      public BoardState()
      {

      }

      public void AddBoats(IEnumerable<Boat> boats)
      {
         Squares = new List<SquareState>();
         foreach (Boat boat in boats)
         {
            foreach (Coord coord in boat.Coords)
            {
               AddSquare(coord, SquareStatus.boat);
            }
         }
      }

      private void AddSquare(Coord coord, SquareStatus newStatus)
      {
         Squares.Add(new SquareState(coord, newStatus));
      }

      private void RemoveSquare(Coord coord)
      {
         Squares.RemoveAll(s => s.Coord == coord);
      }

      private SquareState? FindSquare(Coord coord)
      {
         return Squares.Where(s => s.Coord.x == coord.x && s.Coord.y == coord.y).FirstOrDefault();
      }

      public void SetSquareStatus(Coord coord, SquareStatus newStatus)
      {
         var square = FindSquare(coord);
         if (square != null) square.Status = newStatus;
         else AddSquare(coord, newStatus);
      }

      public SquareStatus GetSquareStatus(Coord coord)
      {
         var square = FindSquare(coord);
         if(square != null) return square.Status;
         return SquareStatus.untouched;
      }

      public bool Attack(Coord coord)
      {
         if(GetSquareStatus(coord) == SquareStatus.boat)
         {
            SetSquareStatus(coord, SquareStatus.hit);
            return true;
         }
         SetSquareStatus(coord, SquareStatus.miss);
         return false;
      }

      public void Reset()
      {
         player = null;
         Squares.Clear();
      }
   }
}

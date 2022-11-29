using SignalR_Server.Interfaces;
using static SignalR_Server.GameValues;

namespace SignalR_Server.Models.Game
{
   public class SquareState
   {
      public Coord Coord { get; set; }
      public SquareStatus Status { get; set; }

      public SquareState(Coord coord, SquareStatus status)
      {
         Coord = coord;
         Status = status;
      }
   }
}

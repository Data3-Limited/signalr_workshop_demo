using static SignalR_Server.GameValues;

namespace SignalR_Server.Models.Game
{
   public class Boat
   {
      public IEnumerable<Coord> Coords { get; set; }

      public Boat(List<Coord> coords)
      {
         Coords = coords;
      }
   }
}

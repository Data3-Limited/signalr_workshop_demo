using SignalR_Server.Interfaces;

namespace SignalR_Server.Models.Game
{
   public class Coord : ICoord
   {
      public int x { get; set; }
      public int y { get; set; }

      public Coord(int x, int y)
      {
         this.x = x;
         this.y = y;
      }

      public override string ToString() => $"({x},{y})";
   }

}

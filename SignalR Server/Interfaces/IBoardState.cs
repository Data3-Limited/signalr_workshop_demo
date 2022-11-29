using SignalR_Server;
using SignalR_Server.Models.Game;
using TAPP_SignalR.Models;
using static SignalR_Server.GameValues;

namespace SignalR_Server.Interfaces
{
   public interface IBoardState
   {
      PlayerTypes? player { get; set; }
      void SetSquareStatus(Coord coord, GameValues.SquareStatus newStatus);

      void Reset();

      bool Attack(Coord coord);

      SquareStatus GetSquareStatus(Coord coord);
   }
}

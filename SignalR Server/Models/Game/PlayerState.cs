using SignalR_Server.Interfaces;
using TAPP_SignalR.Models;

namespace SignalR_Server.Models.Game
{
   public class PlayerState : IPlayerState
   {
      private readonly IBoatGenerator _boatGenerator;
      public string? ConnectionId { get; set; }
      public BoardState BoardState { get; set; }

      public PlayerState(IBoatGenerator boatGenerator)
      {
         _boatGenerator = boatGenerator;
         BoardState = new();
      }

      public bool Join(string connId, PlayerTypes player)
      {
         if (ConnectionId == null)
         {
            ConnectionId = connId;
            BoardState.Reset();
            BoardState.AddBoats(_boatGenerator.generateBoats());
            BoardState.player = player;
            return true;
         }

         return false;
      }

      public IBoardState GetBoardState()
      {
         return BoardState;
      }
   }
}

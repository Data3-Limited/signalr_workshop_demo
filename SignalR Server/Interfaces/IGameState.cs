using TAPP_SignalR.Models;

namespace SignalR_Server.Interfaces
{
   public interface IGameState
   {
      /// <summary>
      /// Attempts to join as player. Returns false if unable to join as player.
      /// </summary>
      /// <param name="type">Player 1 or Player 2</param>
      /// <param name="connectionId">SignalR connection ID</param>
      /// <returns>True if player connected, false if not</returns>
      bool Join(PlayerTypes type, string connectionId);
      bool JoinSpectator(string connectionId);

      bool Attack(PlayerTypes player, ICoord coord);

      void Reset();

      bool IsPlayer(string connId);

      IBoardState GetPlayerBoard(PlayerTypes player);
      PlayerTypes GetPlayerType(string connId);
      string? GetPlayerConnectionId(PlayerTypes player);
      List<string> GetSpectators();

   }
}

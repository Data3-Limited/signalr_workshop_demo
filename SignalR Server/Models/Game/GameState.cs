using SignalR_Server.GameLogic;
using SignalR_Server.Interfaces;
using TAPP_SignalR.Models;

namespace SignalR_Server.Models.Game
{
   public class GameState : IGameState
   {
      public Dictionary<PlayerTypes, PlayerState> Players { get; set; } = new();
      public List<string> Spectators { get; set; } = new List<string>();

      private IBoatGenerator boatGenerator = new BoatGenerator();

      public GameState()
      {

         Players.Add(PlayerTypes.player1, new(boatGenerator));
         Players.Add(PlayerTypes.player2, new(boatGenerator));
      }

      public bool Attack(PlayerTypes player, ICoord coord)
         => Players[player].BoardState.Attack((Coord)coord);


      public bool Join(PlayerTypes player, string connectionId)
      {
         // Connection id is already connected as a player
         if (Players[PlayerTypes.player1].ConnectionId == connectionId || Players[PlayerTypes.player2].ConnectionId == connectionId)
            return false;
         return Players[player].Join(connectionId, player);
      }

      public bool JoinSpectator(string connectionId)
      {
         if(!Spectators.Contains(connectionId))
         {
            Spectators.Add(connectionId);
            return true;
         }

         return false;
      }

      public void Reset()
      {
         Players[PlayerTypes.player1].ConnectionId = null;
         Players[PlayerTypes.player1].BoardState.Reset();

         Players[PlayerTypes.player2].ConnectionId = null;
         Players[PlayerTypes.player2].BoardState.Reset();

         Spectators.Clear();
      }

      public bool IsPlayer(string connId)
         => Players[PlayerTypes.player1].ConnectionId == connId || Players[PlayerTypes.player2].ConnectionId == connId;

      public IBoardState GetPlayerBoard(PlayerTypes player)
      {
         return Players[player].BoardState;
      }

      public List<string> GetSpectators()
      {
         return Spectators;
      }

      public PlayerTypes GetPlayerType(string connId)
      {
         if (Players[PlayerTypes.player1].ConnectionId == connId) return PlayerTypes.player1;
         if (Players[PlayerTypes.player2].ConnectionId == connId) return PlayerTypes.player2;
         return PlayerTypes.spectator;         
      }

      public string? GetPlayerConnectionId(PlayerTypes player)
      {
         return Players[player].ConnectionId;
      }
   }
}

using Microsoft.AspNetCore.SignalR;
using TAPP_SignalR.Models;
using System.Text.Json;
using SignalR_Server.Models.Game;
using SignalR_Server.GameLogic;
using SignalR_Server.Interfaces;
using static SignalR_Server.GameValues;

namespace TAPP_SignalR.Hubs
{
   public static class Channels
   {
      public static readonly string spectator = "spectator";
      public static readonly string players = "players";
   }
   public class LiveUpdateHub : Hub
   {
      private readonly IGameState _gameState;
      private string MessageFromServer = "fromServer";

      public LiveUpdateHub(IGameState gameState)
      {
         _gameState = gameState;
      }

      /*
       * Helper methods to send TO the client
       */
      public Task Broadcast(SignalRServerMessage message)
          => Clients.All.SendAsync(MessageFromServer, message);

      public Task BroadcastAllExceptSender(SignalRServerMessage message)
         => Clients.AllExcept(this.Context.ConnectionId).SendAsync(MessageFromServer, message);

      public Task BroadcastOthers(List<string> users, SignalRServerMessage message)
         => Clients.AllExcept(users).SendAsync(MessageFromServer, message);

      public Task ReplySender(SignalRServerMessage message)
          => Clients.Clients(this.Context.ConnectionId).SendAsync(MessageFromServer, message);

      public Task SendMessageToUser(string user, SignalRServerMessage message)
          => Clients.Clients(user).SendAsync(MessageFromServer, message);

      public Task SendGroup(string group, SignalRServerMessage message)
         => Clients.Group(group).SendAsync(MessageFromServer, message);

      /*
       * Methods which receive FROM the client
       */
      public void FromClient(string message)
      {
         var messageObj = JsonSerializer.Deserialize<SignalRClientMessage>(message);
         try
         {
            switch (messageObj?.Method)
            {
               case SignalRClientMethods.reset:
                  HandleReset();
                  break;
               case SignalRClientMethods.join:
                  HandleJoin(JsonSerializer.Deserialize<PlayerTypes>(messageObj.Message));
                  break;
               case SignalRClientMethods.attack:
                  HandleAttack(JsonSerializer.Deserialize<Coord>(messageObj.Message));
                  break;
            }
         }
         catch (Exception)
         {
            ReplySender(Message(SignalRServerMethods.error, "What the heck did you just send me?"));
         }
      }


      /*
       * Helper methods
       */

      private static SignalRServerMessage Message(SignalRServerMethods method, dynamic message)
      {
         return new SignalRServerMessage(method, message);
      }


      /// <summary>
      /// Assign the user to the requested player/channel
      /// </summary>
      /// <param name="type">This will contain what the user is trying to join as</param>
      private void HandleJoin(PlayerTypes type)
      {

         if (type == PlayerTypes.spectator)
         {
            if(_gameState.IsPlayer(Context.ConnectionId))
            {
               // Players can't be spectators!
               ReplySender(Message(SignalRServerMethods.error, "You are already a player, stop trying to cheat!"));
            } else
            {
               if (_gameState.JoinSpectator(Context.ConnectionId)) {

                  // Join as spectator
                  Groups.AddToGroupAsync(Context.ConnectionId, Channels.spectator);

                  // Let everybody know
                  var boardState = new
                  {
                     left = (BoardState)_gameState.GetPlayerBoard(PlayerTypes.player1),
                     right = (BoardState)_gameState.GetPlayerBoard(PlayerTypes.player2),
                  };
                  ReplySender(Message(SignalRServerMethods.joined, new { type, boardState }));
                  Broadcast(Message(SignalRServerMethods.info, "New spectator joined"));
               }
               else
               {
                  // User is already a spectator
                  ReplySender(Message(SignalRServerMethods.error, $"You are already a spectator!"));
               }
            }
            return;
         }

         if (_gameState.Join(type, Context.ConnectionId))
         {
            // Successfully joined as chosen player
            Groups.AddToGroupAsync(Context.ConnectionId, Channels.players);

            // Send messages
            ReplySender(Message(SignalRServerMethods.joined, new { type, boardState = (BoardState)_gameState.GetPlayerBoard(type) }));
            Broadcast(Message(SignalRServerMethods.info, $"{type} has joined."));
            // Let everybody know
            var boardState = new
            {
               left = (BoardState)_gameState.GetPlayerBoard(PlayerTypes.player1),
               right = (BoardState)_gameState.GetPlayerBoard(PlayerTypes.player2),
            };
            SendGroup(Channels.spectator,Message(SignalRServerMethods.joined, new { type = PlayerTypes.spectator, boardState }));
         }
         else
         {
            // Wasn't allowed to join as requested player for whatever reason
            ReplySender(Message(SignalRServerMethods.error, $"You cannot join as {type}"));
         }

         //CheckState();
      }

      private void HandleReset()
      {
         Broadcast(Message(SignalRServerMethods.reset, ""));
         Broadcast(Message(SignalRServerMethods.info, "Game reset!"));

         // Clear all Signalr groups
         var spectators = _gameState.GetSpectators();
         spectators.ForEach(spectator =>
         {
            Groups.RemoveFromGroupAsync(spectator, Channels.spectator);
         });

         Groups.RemoveFromGroupAsync(Channels.players, _gameState.GetPlayerConnectionId(PlayerTypes.player1));
         Groups.RemoveFromGroupAsync(Channels.players, _gameState.GetPlayerConnectionId(PlayerTypes.player2));

         _gameState.Reset();
      }

      private void HandleAttack(Coord coord)
      {
         PlayerTypes player = _gameState.GetPlayerType(Context.ConnectionId);
         PlayerTypes target = PlayerTypes.player1; // Messy code but oh well :)
         bool successful = false;

         switch(player)
         {
            case PlayerTypes.player1:
               successful = _gameState.Attack(PlayerTypes.player2, coord);
               target = PlayerTypes.player2;
               break;
            case PlayerTypes.player2:
               successful = _gameState.Attack(PlayerTypes.player1, coord);
               target = PlayerTypes.player1;
               break;
         }

         SendGroup(Channels.spectator, Message(SignalRServerMethods.attackResponse, new { target = target, boardState = (BoardState)_gameState.GetPlayerBoard(target), successful = successful }));
         SendGroup(Channels.players, Message(SignalRServerMethods.attackResponse, new { target = target, boardState = (BoardState)_gameState.GetPlayerBoard(target), successful = successful }));
      }

      // Used for debugging
      private void CheckState()
      {
         Broadcast(Message(SignalRServerMethods.info, JsonSerializer.Serialize((GameState)_gameState)));
      }

   }
}

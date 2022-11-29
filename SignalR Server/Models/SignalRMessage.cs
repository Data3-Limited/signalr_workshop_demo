namespace TAPP_SignalR.Models
{
   public enum SignalRClientMethods
   {
      reset,
      join,
      attack
   }

   public enum SignalRServerMethods
   {
      error,
      info,
      reset,
      joined,
      attack,
      attackResponse,
      getBoardState
   }

   public enum PlayerTypes
   {
      player1,
      player2,
      spectator
   }

   public class SignalRClientMessage
   {
      public SignalRClientMethods Method { get; set; }
      public dynamic? Message { get; set; }
   }

   public class SignalRServerMessage
   {
      public SignalRServerMethods Method { get; set; }
      public dynamic? Message { get; set; }

      public SignalRServerMessage(SignalRServerMethods method, dynamic message)
      {
         Method = method;
         Message = message;
      }
   }
}

namespace SignalR_Server
{
   public static class GameValues
   {
      public const int BoardWidth = 10;
      public const int BoardHeight = 10;

      public enum SquareStatus
      {
         untouched,
         miss,
         hit,
         boat
      }

      public enum BoatLength
      {
         Short = 2,
         Medium = 3,
         Long = 4,
         Kraken = 5
      }

   }
}

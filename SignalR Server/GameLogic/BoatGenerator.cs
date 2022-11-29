using SignalR_Server.Interfaces;
using SignalR_Server.Models.Game;
using static SignalR_Server.GameValues;

namespace SignalR_Server.GameLogic
{
   public class BoatGenerator : IBoatGenerator
   {
      private enum BoatDirection
      {
         Horizontal,
         Vertical
      }

      private static readonly Random _random = new Random();
      private static readonly object syncLock = new object();
      public static int RandomNumber(int min, int max)
      {
         lock (syncLock)
         {
            return _random.Next(min, max);
         }
      }

      public BoatGenerator()
      {

      }

      public IEnumerable<Boat> generateBoats()
      {
         // init board state
         List<Boat> boats = new();

         // generate a boat for each length of boat in BoatLength
         foreach (int len in Enum.GetValues(typeof(BoatLength)))
         {
            boats.Add(generateBoat(boats, len));
         }
         return boats;
      }

      public Boat generateBoat(List<Boat> boats, int boatLength)
      {
         int maxX, maxY;


         bool clearCoordinates = false;
         BoatDirection direction = RandomNumber(0, 100) < 50 ? BoatDirection.Horizontal : BoatDirection.Vertical;
         List<Coord> coordList = new();

         while (!clearCoordinates)
         {
            // get possible starting area depending on direction
            switch (direction)
            {
               case BoatDirection.Horizontal:
                  maxX = BoardWidth - 1 - boatLength;
                  maxY = BoardHeight - 1;
                  break;
               case BoatDirection.Vertical:
                  maxX = BoardWidth - 1;
                  maxY = BoardHeight - 1 - boatLength;
                  break;
               default:
                  maxX = 0;
                  maxY = 0;
                  break;
            }

            int startX = RandomNumber(0, maxX);
            int startY = RandomNumber(0, maxY);

            int coordX = startX;
            int coordY = startY;

            coordList.Clear();
            coordList.Add(new Coord(coordX, coordY));

            // generate remaining coordinates for the boat and its given length
            for (int i = 1; i < boatLength; i++)
            {
               switch (direction)
               {
                  case BoatDirection.Horizontal:
                     coordX++;
                     break;
                  case BoatDirection.Vertical:
                     coordY++;
                     break;
               }

               coordList.Add(new Coord(coordX, coordY));
            }
            clearCoordinates = true;
            foreach (Boat boat in boats)
            {
               foreach (Coord coord in boat.Coords)
               {
                  foreach (Coord coord1 in coordList)
                  {
                     if (coord.x == coord1.x && coord.y == coord1.y) clearCoordinates = false;
                  }
               }
            }

         }

         return new Boat(coordList);
      }
   }
}

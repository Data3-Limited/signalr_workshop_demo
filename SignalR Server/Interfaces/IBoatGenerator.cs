using SignalR_Server.Models.Game;

namespace SignalR_Server.Interfaces
{
   public interface IBoatGenerator
   {
      IEnumerable<Boat> generateBoats();
   }
}

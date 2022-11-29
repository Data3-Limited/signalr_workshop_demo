using Microsoft.AspNetCore.Mvc;
using SignalR_Server.GameLogic;
using SignalR_Server.Models.Game;

namespace SignalR_Server.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class GameController : ControllerBase
   {

      private readonly ILogger<GameController> _logger;
      public GameState _gameState;

      public GameController(ILogger<GameController> logger)
      {
         _logger = logger;
      }

      [HttpGet]
      [Route("reset")]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameState))]
      public IActionResult Get()
      {
         //BoatGenerator generator = new BoatGenerator();

         //BoardState boardStateMine = new ();
         //BoardState BoardStateOpponent = new ();

         //_gameState = new GameState(boardStateMine, BoardStateOpponent);

         return Ok("hi");
      }
      
   }
}

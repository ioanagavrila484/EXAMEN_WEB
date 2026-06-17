using EXAMEN_WEB_ASP.Server.Data;
using EXAMEN_WEB_ASP.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace EXAMEN_WEB_ASP.Server.Controllers
{
    [ApiController]
    [Route("api/recipestep")]
    public class RecipeStepController : ControllerBase
    {
        private readonly RecipeStepRepo _repo;

        public RecipeStepController(RecipeStepRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] RecipeStep step)
        {
            try
            {
                await _repo.InsertAsync(step);
                return Ok(new { message = "Step added" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

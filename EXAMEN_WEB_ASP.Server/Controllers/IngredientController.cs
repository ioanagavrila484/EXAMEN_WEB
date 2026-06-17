using EXAMEN_WEB_ASP.Server.Data;
using Microsoft.AspNetCore.Mvc;

namespace EXAMEN_WEB_ASP.Server.Controllers
{
    [ApiController]
    [Route("api/ingredient")]
    public class IngredientController : ControllerBase
    {
        private readonly IngredientRepo _repo;

        public IngredientController(IngredientRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await _repo.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

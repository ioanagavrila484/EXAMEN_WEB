using EXAMEN_WEB_ASP.Server.Data;
using EXAMEN_WEB_ASP.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace EXAMEN_WEB_ASP.Server.Controllers
{
    [ApiController]
    [Route("api/recipe")]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeRepo _repo;

        public RecipeController(RecipeRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Recipe recipe)
        {
            try
            {
                var id = await _repo.InsertAsync(recipe);
                return Ok(new { id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repo.DeleteAsync(id);
                return Ok(new { message = "Deleted" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

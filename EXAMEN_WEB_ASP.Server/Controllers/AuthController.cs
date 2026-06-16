using EXAMEN_WEB_ASP.Server.Data;
using EXAMEN_WEB_ASP.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace EXAMEN_WEB_ASP.Server.Controllers
{
    [ApiController]
    [Route("api/auth")] // ruta de baza = /api/auth
    public class AuthController : ControllerBase
    {
        private readonly UtilizatorRepo _utilizatorRepo;

        public AuthController(UtilizatorRepo utilizatorRepo)
        {
            _utilizatorRepo = utilizatorRepo;
        }

        // POST /api/auth/login?userName=Ioana%20Gavrila
        // Primeste numele DIRECT din URL (query string), fara body/JSON.
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string userName)
        {
            // Daca nu vine niciun nume -> 400, nu 500.
            if (string.IsNullOrWhiteSpace(userName))
                return BadRequest(new { message = "Numele este obligatoriu" });

            try
            {
                var found = await _utilizatorRepo.LoginAsync(userName);

                if (found is null)
                    return Unauthorized(new { message = "Utilizator inexistent" });

                return Ok(new { id = found.Id, userName = found.UserName });
            }
            catch (Exception ex)
            {
                // Orice eroare (ex: conexiune SQL) -> 500 cu mesaj clar, nu crash anonim.
                return StatusCode(500, new { message = "Eroare la login", detail = ex.Message });
            }
        }

        /*
        =================================================================
          SINTEZA LEGATURA FE <-> BE (Backend / C#)
        =================================================================
          - Fiecare metoda de aici este un ENDPOINT pe care il apeleaza FE.
          - [Route("api/auth")] -> ruta de baza este /api/auth.
          - [HttpGet]/[HttpPost]/[HttpPut] decid ce metoda HTTP din Angular
            (http.get/post/put) ajunge aici.
          - [FromBody] = datele vin din body-ul request-ului (JSON).
          - {id} in ruta = valoare luata din URL.
          - Intoarce Ok(...), NotFound(), BadRequest(), Unauthorized() etc.
          - NU uita sa inregistrezi Repo-ul in Program.cs:
                builder.Services.AddScoped<UtilizatorRepo>();

          ---- TEMPLATE GET (returneaza lista catre FE) -----------------
          [HttpGet("all")]
          public async Task<IActionResult> GetAll()
          {
              var items = await _utilizatorRepo.GetAllAsync();
              return Ok(items);
          }

          ---- TEMPLATE POST (creeaza ceva pe baza body-ului) -----------
          [HttpPost("create")]
          public async Task<IActionResult> Create([FromBody] Utilizator item)
          {
              await _utilizatorRepo.InsertAsync(item);
              return Ok(new { message = "Adaugat" });
          }

          ---- TEMPLATE PUT (update dupa id din URL) --------------------
          [HttpPut("{id}")]
          public async Task<IActionResult> Update(int id, [FromBody] Utilizator item)
          {
              await _utilizatorRepo.UpdateAsync(id, item);
              return Ok(new { message = "Actualizat" });
          }
        =================================================================
        */
    }
}

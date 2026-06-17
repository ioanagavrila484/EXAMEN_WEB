using EXAMEN_WEB_ASP.Server.Models;
using Microsoft.Data.SqlClient;

namespace EXAMEN_WEB_ASP.Server.Data
{
    public class IngredientRepo
    {
        private readonly string _connectionString;

        public IngredientRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<Ingredient>> GetAllAsync()
        {
            var list = new List<Ingredient>();
            const string sql = "SELECT Id, NameIngredient, Unit, CaloriesPer100g FROM Ingredient";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Ingredient
                {
                    Id = reader.GetInt32(0),
                    NameIngredient = reader.GetString(1),
                    Unit = reader.GetString(2),
                    CaloriesPer100g = reader.GetInt32(3)
                });
            }
            return list;
        }
    }
}

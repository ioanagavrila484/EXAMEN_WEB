using EXAMEN_WEB_ASP.Server.Models;
using Microsoft.Data.SqlClient;

namespace EXAMEN_WEB_ASP.Server.Data
{
    public class RecipeRepo
    {
        private readonly string _connectionString;

        public RecipeRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<int> InsertAsync(Recipe recipe)
        {
            const string sql =
                "INSERT INTO Recipe(userId, title, TotalCalories) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@userId, @title, @totalCalories)";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userId", recipe.UserId);
            command.Parameters.AddWithValue("@title", recipe.Title);
            command.Parameters.AddWithValue("@totalCalories", recipe.TotalCalories);

            var result = await command.ExecuteScalarAsync();
            return (int)result!;
        }

        public async Task DeleteAsync(int id)
        {
            const string sql =
                "DELETE FROM RecipeStep WHERE recipeId = @id; " +
                "DELETE FROM Recipe WHERE Id = @id";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }
    }
}

using EXAMEN_WEB_ASP.Server.Models;
using Microsoft.Data.SqlClient;

namespace EXAMEN_WEB_ASP.Server.Data
{
    public class RecipeStepRepo
    {
        private readonly string _connectionString;

        public RecipeStepRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task InsertAsync(RecipeStep step)
        {
            const string sql =
                "INSERT INTO RecipeStep(recipeId, StepNumber, DescriptionStep, IngredientsIds) " +
                "VALUES (@recipeId, @stepNumber, @descriptionStep, @ingredientsIds)";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@recipeId", step.RecipeId);
            command.Parameters.AddWithValue("@stepNumber", step.StepNumber);
            command.Parameters.AddWithValue("@descriptionStep", step.DescriptionStep);
            command.Parameters.AddWithValue("@ingredientsIds", step.IngredientsIds);
            await command.ExecuteNonQueryAsync();
        }
    }
}

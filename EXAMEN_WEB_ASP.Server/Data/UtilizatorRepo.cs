using EXAMEN_WEB_ASP.Server.Models;
using Microsoft.Data.SqlClient;

namespace EXAMEN_WEB_ASP.Server.Data
{
    // Repo = clasa care vorbeste cu baza de date (SQL).
    // Aici scrii interogarile SQL (SELECT / INSERT / UPDATE).
    public class UtilizatorRepo
    {
        private readonly string _connectionString;

        public UtilizatorRepo(IConfiguration configuration)
        {
            // Connection string-ul vine din appsettings.json -> "ConnectionStrings:DefaultConnection"
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // ====================== LOGIN ======================
        // Cauta un utilizator dupa UserName. Daca exista -> il intoarce, altfel -> null.
        public async Task<Utilizator?> LoginAsync(string userName)
        {
            const string sql =
                "SELECT Id, UserName FROM Utilizator " +
                "WHERE UserName = @userName";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            // Parametrii (@userName) previn SQL injection
            command.Parameters.AddWithValue("@userName", userName);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Utilizator
                {
                    Id = reader.GetInt32(0),       // coloana 0 = Id
                    UserName = reader.GetString(1) // coloana 1 = UserName
                };
            }

            return null; // nu s-a gasit niciun utilizator
        }

        /*
        =================================================================
          TEMPLATE-URI (DECOMENTEAZA SI ADAPTEAZA LA TABELA TA)
        =================================================================

        ---- GET ALL (citeste toata lista din tabela) -------------------
        public async Task<List<Utilizator>> GetAllAsync()
        {
            var lista = new List<Utilizator>();
            const string sql = "SELECT Id, UserName FROM Utilizator";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Utilizator
                {
                    Id = reader.GetInt32(0),
                    UserName = reader.GetString(1)
                });
            }
            return lista;
        }

        ---- INSERT (adauga un rand nou in tabela) ----------------------
        public async Task InsertAsync(Utilizator u)
        {
            const string sql =
                "INSERT INTO Utilizator(UserName) VALUES (@userName)";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userName", u.UserName);
            await command.ExecuteNonQueryAsync(); // pt INSERT/UPDATE/DELETE
        }

        ---- UPDATE (modifica un rand existent dupa Id) -----------------
        public async Task UpdateAsync(int id, Utilizator u)
        {
            const string sql =
                "UPDATE Utilizator SET UserName = @userName WHERE Id = @id";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userName", u.UserName);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }
        =================================================================
        */
    }
}

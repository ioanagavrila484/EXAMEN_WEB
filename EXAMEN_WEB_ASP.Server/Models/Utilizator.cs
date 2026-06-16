namespace EXAMEN_WEB_ASP.Server.Models
{
    // Modelul = oglinda tabelei din SQL.
    // Proprietatile trebuie sa corespunda coloanelor din tabela Utilizator:
    //   Id       INT
    //   UserName VARCHAR(100)
    public class Utilizator
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}

namespace EXAMEN_WEB_ASP.Server.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int TotalCalories { get; set; }
    }
}

namespace EXAMEN_WEB_ASP.Server.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string NameIngredient { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public int CaloriesPer100g { get; set; }
    }
}

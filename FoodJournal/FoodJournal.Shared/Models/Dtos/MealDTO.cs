namespace FoodJournal.Shared.Models.Dtos
{
    public record MealGetDTOWithFoods(int Id, string Name, IEnumerable<FoodGetDTO> Foods);
    public record MealGetDTO(int Id, string Name);
    public record MealCreateDTO(string Name, IEnumerable<FoodGetDTO> Foods);
    public record MealUpdateDTO(int Id, string Name, IEnumerable<FoodGetDTO>? Foods = null);
}

using FoodJournal.Shared.Enums;

namespace FoodJournal.Shared.Models.Dtos
{
    public record MealGetDTOWithFoods(int Id, string Name, MealType MealType, IEnumerable<FoodGetDTO> Foods);
    public record MealGetDTO(int Id, string Name, MealType MealType);
    public record MealCreateDTO(string Name, MealType MealType, IEnumerable<FoodGetDTO> Foods);
    public record MealUpdateDTO(int Id, string Name, MealType MealType, IEnumerable<FoodGetDTO>? Foods = null);
}

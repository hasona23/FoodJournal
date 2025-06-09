using FoodJournal.Shared.Enums;

namespace FoodJournal.Shared.Models.Dtos
{
    public record MealGetDTOWithFoods(int Id, string Name, bool IsStarred, DateTime DateEaten, MealType MealType, IEnumerable<FoodGetDTO> Foods);
    public record MealGetDTO(int Id, string Name, bool IsStarred, DateTime DateEaten, MealType MealType);
    public record MealCreateDTO(string Name, bool IsStarred, DateTime DateEaten, MealType MealType, IEnumerable<FoodGetDTO> Foods);
    public record MealUpdateDTO(int Id, string Name, bool IsStarred, DateTime DateEaten, MealType MealType, IEnumerable<FoodGetDTO>? Foods = null);
}

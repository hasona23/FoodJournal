using FoodJournal.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace FoodJournal.Shared.Models.Dtos
{
    public record MealGetDTOWithFoods(int Id, string Name, MealType MealType, IEnumerable<FoodGetDTO> Foods);
    public record MealGetDTO(int Id, string Name, MealType MealType);
    public record MealCreateDTO([MinLength(3, ErrorMessage = "Too Short"), MaxLength(16, ErrorMessage = "TooLong")]
    string Name, MealType MealType, IEnumerable<FoodGetDTO> Foods);
    public record MealUpdateDTO(int Id, string Name, MealType MealType, IEnumerable<FoodGetDTO>? Foods = null);
}

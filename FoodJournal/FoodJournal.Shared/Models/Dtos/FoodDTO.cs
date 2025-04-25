namespace FoodJournal.Shared.Models.Dtos
{
    public record FoodGetDTOWithMeals(int Id, string Name, IEnumerable<MealGetDTO> Meals);
    public record FoodGetDTO(int Id, string Name);
    public record FoodCreateDTO(string Name);
    public record FoodUpdateDTO(int Id, string Name);
}


using FoodJournal.Shared.Models.Dtos;

namespace FoodJournal.Shared.Models
{
    public class Food
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; } = string.Empty;


        public virtual List<Meal> Meals { get; set; }
        public IEnumerable<MealGetDTO> GetMealsDTO()
        {
            return Meals.Select(m => new MealGetDTO(m.Id, m.Name));
        }
    }
}

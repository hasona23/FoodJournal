using FoodJournal.Shared.Models.Dtos;
using System.ComponentModel.DataAnnotations;

namespace FoodJournal.Shared.Models
{
    public class Food
    {
        public int Id { get; set; }
        [Required, MinLength(3), MaxLength(16)]
        public string Name { get; set; }


        public virtual List<Meal> Meals { get; set; }
        public IEnumerable<MealGetDTO> GetMealsDTO()
        {
            return Meals.Select(m => new MealGetDTO(m.Id, m.Name, m.IsStarred, m.DateEaten, m.MealType));
        }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}

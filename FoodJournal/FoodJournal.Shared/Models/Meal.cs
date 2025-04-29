using FoodJournal.Shared.Enums;
using FoodJournal.Shared.Models.Dtos;

namespace FoodJournal.Shared.Models
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; } = string.Empty;
        public MealType MealType { get; set; }

        //Navigation Property
        public virtual List<Food> Foods { get; set; }

        public IEnumerable<FoodGetDTO> GetFoodsDTO()
        {
            return Foods.Select(f => new FoodGetDTO(f.Id, f.Name));
        }
    }
}

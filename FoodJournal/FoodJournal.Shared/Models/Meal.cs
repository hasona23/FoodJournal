using FoodJournal.Shared.Enums;
using FoodJournal.Shared.Models.Dtos;
using System.ComponentModel.DataAnnotations;

namespace FoodJournal.Shared.Models
{
    public class Meal
    {
        public int Id { get; set; }

        [MinLength(3, ErrorMessage = "Too Short"), MaxLength(16, ErrorMessage = "TooLong")]
        public string Name { get; set; }

        public bool IsStarred { get; set; }
        public DateTime DateEaten { get; set; }
        public MealType MealType { get; set; }

        //Navigation Property
        public virtual List<Food> Foods { get; set; }

        public IEnumerable<FoodGetDTO> GetFoodsDTO()
        {
            return Foods.Select(f => new FoodGetDTO(f.Id, f.Name));
        }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}

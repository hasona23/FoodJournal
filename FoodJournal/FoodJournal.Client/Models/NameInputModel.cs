using System.ComponentModel.DataAnnotations;

namespace FoodJournal.Client.Models;

public class NameInputModel
{
    public const int MaxNameLength = 16;
    public const int MinNameLength = 2;
    [Required]
    [StringLength(maximumLength: MaxNameLength, MinimumLength = MinNameLength, ErrorMessage = "Name must be between 2 and 16 in length")]
    public string Name { get; set; } = string.Empty;
    public bool IsValid => Name.Length <= MaxNameLength && Name.Length >= MinNameLength;
}

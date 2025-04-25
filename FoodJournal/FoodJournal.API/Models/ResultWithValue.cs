namespace FoodJournal.API.Models;

public record ResultWithValue<T>(T? Value, Result Response) where T : class
{
    public static ResultWithValue<T> Success(T value) => new ResultWithValue<T>(value, Result.Success());
    public static ResultWithValue<T> Error(Result response) => new ResultWithValue<T>(null, response);
    public bool IsSuccess => Value != null && Response == Result.Success();

}

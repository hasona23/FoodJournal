namespace FoodJournal.API.Models
{
    public record Result(string Message)
    {
        public static Result Success() => new Result(string.Empty);
        public static Result Fail(string msg) => new Result(msg);
        public static Result NotFoundError() => new Result("NotFound");
        public bool IsSuccess() => string.IsNullOrEmpty(Message);
    }

}

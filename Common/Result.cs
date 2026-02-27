namespace Warehouse.Common
{
    public class Result<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public T? Data { get; private set; }

        public static Result<T> Ok(T data, string message = "Operation completed successfully.")
            => new() { Success = true, Data = data, Message = message };

        public static Result<T> Failure(string message)
            => new() { Success = false, Message = message };
    }
}

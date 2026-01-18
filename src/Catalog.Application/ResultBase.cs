namespace Catalog.Application
{
    public class ResultBase<T>
    {
        public bool IsSuccess { get; }
        public string? Message { get; }
        public T? Value { get; }

        private ResultBase(T? value, string message)
        {
            IsSuccess = value != null;
            Message = message;
            Value = value;
        }

        public static ResultBase<T> Ok(T? value, string message = "") => new(value, message);
        public static ResultBase<T> Failure(string error) => new(default, error);
    }
}

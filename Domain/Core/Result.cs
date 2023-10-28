namespace Domain.Core;

public class Result<T>
{
    public bool IsScucess { get; set; }

    public T? Value { get; set; }

    public string Error { get; set; } = string.Empty;

    public static Result<T> Success(T value) => new() { IsScucess = true, Value = value };

    public static Result<T> Failure(string error) => new() { IsScucess = false, Error = error };
}


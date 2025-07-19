namespace PetFamily.Domain.Shared.Entities
{
    public class Result
    {
        public Result(bool isSuccess, string? error)
        {
            if (isSuccess && error != null)
                throw new InvalidOperationException();

            if (!isSuccess && error == null)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public string? Error { get; set; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public static Result Success() => new(true, null);

        public static Result Failure(string error) => new(false, error);

        public static implicit operator Result(string error) => new(false, error);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;

        public Result(TValue value, bool isSuccess, string? error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("The value of a failure result can not be accessed.");

        public static Result<TValue> Success(TValue value) => new Result<TValue>(value, true, null);
        public static new Result<TValue> Failure(string error) => new Result<TValue>(default!, false, error);

        public static implicit operator Result<TValue>(TValue value) => new(value, true, null);
        public static implicit operator Result<TValue>(string? error) => new(default!, false, error);
    }
    
    public class UnitResult<TError>
    {
        private readonly TError? _error;

        public UnitResult(bool isSuccess, TError? error)
        {
            IsSuccess = isSuccess;
            _error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public TError? Error => IsFailure ? _error : throw new InvalidOperationException("The value of a failure result can not be accessed.");

        public static UnitResult<TError> Success() => new UnitResult<TError>(true, default!);
        public static UnitResult<TError> Failure(TError error) => new UnitResult<TError>(false, error);

        public static implicit operator UnitResult<TError>(TError? error) => new(true, error);
    }

    public class Result<TValue, TError>
    {
        private readonly TValue _value;
        private readonly TError? _error;

        public Result(TValue value, bool isSuccess, TError? error)
        {
            _value = value;
            IsSuccess = isSuccess;
            _error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("The value of a failure result can not be accessed.");
        public TError? Error => IsFailure ? _error : throw new InvalidOperationException("The value of a failure result can not be accessed.");

        public static Result<TValue, TError> Success(TValue value) => new Result<TValue, TError>(value, true, default!);
        public static Result<TValue, TError> Failure(TError error) => new Result<TValue, TError>(default!, false, error);

        public static implicit operator Result<TValue, TError>(TValue value) => new(value, true, default!);
        public static implicit operator Result<TValue, TError>(TError? error) => new(default!, false, error);
    }
}

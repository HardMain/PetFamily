namespace SharedKernel.Failures
{
    public class Result
    {
        public Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
                throw new InvalidOperationException();

            if (!isSuccess && error == null)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public Error Error { get; set; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);

        public static implicit operator Result(Error error) => new(false, error);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;

        public Result(TValue value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("The value of a failure result can not be accessed.");

        public static Result<TValue> Success(TValue value) => new Result<TValue>(value, true, Error.None);
        public static new Result<TValue> Failure(Error error) => new Result<TValue>(default!, false, error);

        public static implicit operator Result<TValue>(TValue value) => Success(value);
        
        public static implicit operator Result<TValue>(Error error) => Failure(error);
    }
    public class UnitResult<TError>
    {
        private readonly TError _error;

        public UnitResult(bool isSuccess, TError error)
        {
            _error = error;
            IsSuccess = isSuccess;
        }
        
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public TError Error => IsFailure ? _error : throw new InvalidOperationException("The error of a successful result cannot be accessed.");

        public static UnitResult<TError> Success() => new UnitResult<TError>(true, default!);
        public static UnitResult<TError> Failure(TError error) => new UnitResult<TError>(false, error);

        public static implicit operator UnitResult<TError>(TError error) => Failure(error);
    }

    public class Result<TValue, TError>
    {
        private readonly TValue _value;
        private readonly TError _error;

        public Result(TValue value, bool isSuccess, TError error)
        {
            _value = value;
            _error = error;
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("The value of a failure result can not be accessed.");
        public TError Error => IsFailure ? _error : throw new InvalidOperationException("The error of a successful result cannot be accessed.");

        public static Result<TValue, TError> Success(TValue value) => new Result<TValue, TError>(value, true, default!);
        public static Result<TValue, TError> Failure(TError error) => new Result<TValue, TError>(default!, false, error);

        public static implicit operator Result<TValue, TError>(TValue value) => Success(value);
        public static implicit operator Result<TValue, TError>(TError error) => Failure(error);
    }
}
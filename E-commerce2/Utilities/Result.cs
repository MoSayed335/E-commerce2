namespace ECommerce2.Utilities
{
    /// <summary>
    /// Wrapper موحّد للنتائج بدل ما نرمي Exceptions على أخطاء متوقعة
    /// (زي "المخزون مش كفاية" أو "الكوبون منتهي") - الـ Controller بس هو اللي يقرر
    /// يترجم الفشل لأنهي HTTP Status Code، والـ Service مش مسؤول عن ده (SRP).
    /// </summary>
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? Error { get; }

        private Result(bool isSuccess, T? value, string? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Failure(string error) => new(false, default, error);
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }

        private Result(bool isSuccess, string? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);
        public static Result Failure(string error) => new(false, error);
    }
}

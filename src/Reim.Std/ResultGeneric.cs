namespace Reim.Std;

// T: Value
// E: Error
public sealed partial class Result<T, E> {

    private readonly IResult _imp;

    private interface IResult {
        U Match<U>(Func<T, U> ok, Func<E, U> err);
    }

    private Result(IResult imp) {
        _imp = imp;
    }

    internal static Result<T, E> Ok(T ok) {
        return new Result<T, E>(new Ok0(ok));
    }

    internal static Result<T, E> Err(E err) {
        return new Result<T, E>(new Err0(err));
    }

    private sealed class Ok0(T success) : IResult {
        public U Match<U>(
            Func<T, U> onSuccess,
            Func<E, U> onFailure
        ) {
            return onSuccess(success);
        }
    }

    private sealed class Err0(E failure) : IResult {
        public U Match<U>(
            Func<T, U> onSuccess,
            Func<E, U> onFailure
        ) {
            return onFailure(failure);
        }
    }

    // --- For simpler access ---
    public static implicit operator Result<T, E>(T value) => Ok(value);
    public static implicit operator Result<T, E>(E value) => Err(value);
}

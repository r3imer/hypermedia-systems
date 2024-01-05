namespace Reim.Std;

public static partial class Result {
    public static Result<T, E> Ok<T, E>(this T ok) => ok;
    public static Result<T, E> Err<T, E>(this E err) => err;
}



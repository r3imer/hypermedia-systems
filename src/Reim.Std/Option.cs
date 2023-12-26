namespace Reim.Std;

public static partial class Option {
    public static Option<T> Some<T>(T some) => some;
    public static Option<T> None<T>() => Option<T>.None();
}

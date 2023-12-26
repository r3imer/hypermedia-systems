namespace Reim.Std;

public static partial class Option {

    public static Option<T> From<T>(T? x) => x switch {
        null => None<T>(),
        var y => Some(y)
    };

    public static Option<T> ToOption<T>(this Nullable<T> x) where T : struct => x switch {
        null => None<T>(),
        var y => Some(y.Value)
    };

    public static Option<T> ToOption<T>(this T? x) => x switch {
        null => None<T>(),
        var y => Some<T>((T)y)
    };

    public static T? ToNullable<T>(this Option<T> x) => x.Match(
        y => y,
        () => default(T?)
    );

}

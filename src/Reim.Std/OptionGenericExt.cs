namespace Reim.Std;

public sealed partial class Option<T> {

    public T Expect(string msg) => Match(
        some: x => x,
        none: () => throw new NoneException(msg)
    );

    public Option<U> Map<U>(Func<T, Option<U>> map) {
        return _imp.Match(map, Option.None<U>);
    }

    public U Match<U>(Func<T, U> some, Func<U> none) {
        return _imp.Match(some, none);
    }
    public Result<T, E> OkOr<E>(E err) => Match(
        some: x => x,
        none: () => Result.Err<T, E>(err)
    );

    public T Unwrap() => Match(
        some: x => x,
        none: () => throw new NoneException()
    );

    public Option<T> Inspect(Action<T> func) {
        _ = Map(x => { func(x); return Some(x); });
        return this;
    }
}

//public Option<U> Select<U>(
//    Func<T,U> select
//) {
//    return Match(
//        x => Option.Some<U>(select(x))
//    );
//}

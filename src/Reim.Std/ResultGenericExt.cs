namespace Reim.Std;

public sealed partial class Result<T, E> {

    public Option<E> Err() => Match(
        ok: _ => Option.None<E>(),
        err: x => x
    );

    public T Expect(string msg) => Match(
        ok: x => x,
        err: x => throw new ErrException<E>(x, msg)
    );

    public Result<U, E> Map<U>(Func<T, Result<U, E>> map) {
        return _imp.Match(map, Result.Err<U, E>);
    }

    public Result<T, U> MapErr<U>(Func<E, Result<T, U>> map) {
        return _imp.Match(x => x, map);
    }

    public Result<U, E[]> MapToArray<U>(Func<T, Result<U, E[]>> map) {
        return _imp.Match(map, x => new E[] { x });
    }

    public U Match<U>(Func<T, U> ok, Func<E, U> err) {
        return _imp.Match(ok, err);
    }
    public Option<T> Ok() => Match(
        ok: x => x,
        err: _ => Option.None<T>()
    );

    public Result<T1, E> Select<T1>(
        Func<T, T1> selector
    ) {
        return SelectOk(selector);
    }

    public Result<T1, E1> SelectBoth<T1, E1>(
        Func<T, T1> selectOk,
        Func<E, E1> selectErr
    ) {
        return Match(
            s => Result.Ok<T1, E1>(selectOk(s)),
            f => Result.Err<T1, E1>(selectErr(f))
        );
    }

    public Result<T, E1> SelectErr<E1>(
        Func<E, E1> selectErr
    ) {
        return SelectBoth(x => x, selectErr);
    }

    public Result<T1, E> SelectOk<T1>(
        Func<T, T1> selectOk
    ) {
        return SelectBoth(selectOk, x => x);
    }

    public T Unwrap() => Match(
        ok: x => x,
        err: x => throw new ErrException<E>(x)
    );

    public Result<T, E> Inspect(Action<T> func) {
        _ = SelectOk(x => { func(x); return x; });
        return this;
    }
}


namespace Reim.Std;

public static partial class Result {

    public static Result<U, E[]> Success<T, U, E>(
        this Result<T, E> r, Func<T, U> func
    ) {
        return r.Match<Result<U, E[]>>(
            ok => func(ok),
            err => new E[] { err }
        );
    }

    public static Result<U, E[]> Success<T1, T2, U, E>(
        this (Result<T1, E>, Result<T2, E>) r, Func<T1, T2, U> func
    ) {
        return r.Item1.Match(
            ok1 => r.Item2.Match<Result<U, E[]>>(
                ok2 => func(ok1, ok2),
                err2 => new E[] { err2 }
            ),
            err1 => r.Item2.Match<Result<U, E[]>>(
                _ => new E[] { err1 },
                err2 => new E[] { err1, err2 }
            )
        );
    }

    public static Result<U, E[]> Success<T1, T2, U, E>(
        this (Result<T1, E[]>, Result<T2, E[]>) r, Func<T1, T2, U> func
    ) {
        return r.Item1.Match(
            ok1 => r.Item2.Match<Result<U, E[]>>(
                ok2 => func(ok1, ok2),
                err2 => err2
            ),
            err1 => r.Item2.Match<Result<U, E[]>>(
                _ => err1,
                err2 => err1.Concat(err2).ToArray()
            )
        );
    }

    public static Result<U, E[]> Success<T1, T2, T3, U, E>(
        this (Result<T1, E>, Result<T2, E>, Result<T3, E>) r, Func<T1, T2, T3, U> func
    ) {
        return (r.Item1, r.Item2).Success((r1, r2) => (r1, r2)).Match(
            ok => r.Item3.Match<Result<U, E[]>>(
                ok3 => func(ok.r1, ok.r2, ok3),
                err3 => new E[] { err3 }
            ),
            err12 => r.Item3.Match<Result<U, E[]>>(
                _ => err12,
                err3 => err12.Append(err3).ToArray()
            )
        );
    }

    public static Result<U, E[]> Success<T1, T2, T3, U, E>(
        this (Result<T1, E[]>, Result<T2, E[]>, Result<T3, E[]>) r, Func<T1, T2, T3, U> func
    ) {
        return (r.Item1, r.Item2).Success((r1, r2) => (r1, r2)).Match(
            ok => r.Item3.Match<Result<U, E[]>>(
                ok3 => func(ok.r1, ok.r2, ok3),
                err3 => err3
            ),
            err12 => r.Item3.Match<Result<U, E[]>>(
                _ => err12,
                err3 => err12.Concat(err3).ToArray()
            )
        );
    }

    public static Result<U, E[]> Success<T1, T2, T3, T4, U, E>(
        this (Result<T1, E>, Result<T2, E>, Result<T3, E>, Result<T4, E>) r, Func<T1, T2, T3, T4, U> func
    ) {
        return (
            (r.Item1, r.Item2).Success((r1, r2) => (r1, r2)),
            (r.Item3, r.Item4).Success((r3, r4) => (r3, r4))
        ).Success(
            (a, b) => func(a.r1, a.r2, b.r3, b.r4)
        );
    }

    public static Result<U, E[]> Success<T1, T2, T3, T4, U, E>(
        this (Result<T1, E[]>, Result<T2, E[]>, Result<T3, E[]>, Result<T4, E[]>) r, Func<T1, T2, T3, T4, U> func
    ) {
        return (
            (r.Item1, r.Item2).Success((r1, r2) => (r1, r2)),
            (r.Item3, r.Item4).Success((r3, r4) => (r3, r4))
        ).Success(
            (a, b) => func(a.r1, a.r2, b.r3, b.r4)
        );
    }
}

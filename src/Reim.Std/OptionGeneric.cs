namespace Reim.Std;

// T: Value
public sealed partial class Option<T> {

    private readonly IOption _imp;

    private interface IOption {
        U Match<U>(Func<T, U> onSome, Func<U> onNone);
    }

    private Option(IOption option) {
        _imp = option;
    }

    internal static Option<T> Some(T some) {
        return new Option<T>(new Some0(some));
    }

    internal static Option<T> None() {
        return new Option<T>(new None0());
    }

    private sealed class Some0(T some) : IOption {
        public U Match<U>(
            Func<T, U> onSome,
            Func<U> onNone
        ) {
            return onSome(some);
        }
    }

    private sealed class None0 : IOption {
        public None0() { }
        public U Match<U>(
            Func<T, U> onSome,
            Func<U> onNone
        ) {
            return onNone();
        }
    }

    // --- For simpler access ---
    public static implicit operator Option<T>(T value) => Some(value);
}

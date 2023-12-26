namespace Reim.Std;

public class NoneException : Exception {
    public NoneException() : base() { }
    public NoneException(string msg) : base(msg) { }
    public NoneException(string msg, Exception ie) : base(msg, ie) { }
}

public class ErrException : Exception {
    public ErrException() : base() { }
    public ErrException(string msg) : base(msg) { }
    public ErrException(string msg, Exception ie) : base(msg, ie) { }
}

public class ErrException<E> : ErrException {
    public E Err { get; }
    public ErrException(E e) : base() { Err = e; }
    public ErrException(E e, string msg) : base(msg) { Err = e; }
    public ErrException(E e, string msg, Exception ie) : base(msg, ie) { Err = e; }
}

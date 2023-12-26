namespace Reim.Std.Domain;

//public interface IId<I> {
//    I Id { get; }
//}

//public interface IDomain<T> : IId<T> {
//    Task<bool> Save(IRepo<IDomain<T>,T> repo) => repo.Save(this);
//}

public interface IForm<T, I, E> {
    Task<Result<T, E>> Create(IValidater<T> repo, I id);
}

public interface IValidater<T> {
    public Task<bool> Single(Func<T, bool> fn);
    public Task<bool> All(Func<T, bool> fn);
}

// TODO: with error by Load and Save
public interface IRepo<T, I> : IValidater<T> {
    public Task<T?> Load(I id);
    public Task<bool> Save(T data); // TODO?: split add and update ?
    public Task<I> NextId();
}

public record Errors {
    private readonly IDictionary<string, string> _dic;
    public Errors(IDictionary<string, string> dic) {
        _dic = dic;
    }
    public string? this[string key] {
        get => _dic.TryGetValue(key, out var value) ? value : null;
    }
}

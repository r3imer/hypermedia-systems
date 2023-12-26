using Reim.Std;
using Reim.Std.Domain;

namespace Reim.Htmx.Web;

public static class ContactExt {
    public static ContactForm ToForm(this Contact x) => new(
        x.first,
        x.last,
        x.phone,
        x.email
    );
}

public record HtmlContent(string Html);

public record ContactForm(
    string? first_name = null,
    string? last_name = null,
    string? phone = null,
    string? email = null
) : IForm<Contact, int, Errors> {

    public async Task<Result<Contact, Errors>> Create(IValidater<Contact> repo, int id) {
        var errors = new Dictionary<string, string>();

        if (string.IsNullOrEmpty(first_name)) {
            errors["first_name"] = "First Name is Required!";
        }
        if (string.IsNullOrEmpty(last_name)) {
            errors["last_name"] = "Last Name is Required!";
        }
        if (string.IsNullOrEmpty(email)) {
            errors["email"] = "Email is Required!";
        } else if (await repo.Single(x => x.email == email && x.id != id)) {
            errors["email"] = "Email not Unique!";
        }

        if (errors.Count == 0) {
            return new Contact(
                id,
                first_name!,
                last_name!,
                email!,
                phone
            );
        } else {
            return new Errors(errors);
        }
    }
}

public record Contact(
    int id,
    string first,
    string last,
    string email,
    string? phone
);

public record Error {
    public required IDictionary<string, string> data { private get; init; }
    public string? this[string index] {
        get => data.TryGetValue(index, out var value) ? value : null;
    }
}

public record ContactsRequest(
    Contact[] contacts,
    string? q
);

public class ContactsRepo(Contact[] list) : IRepo<Contact, int> {

    private readonly List<Contact> _db = [.. list];
    private int _counter = list.Length;

    public Contact[] All() => _db.ToArray();
    public Contact[] Search(string text) {
        var compare = StringComparison.InvariantCultureIgnoreCase;
        return _db.Where(x =>
            (x.first?.Contains(text, compare) ?? false) ||
            (x.last?.Contains(text, compare) ?? false) ||
            (x.phone?.Contains(text, compare) ?? false) ||
            (x.email?.Contains(text, compare) ?? false)
        ).ToArray();
    }

    public Task<Contact?> Load(int id)
        => Task.FromResult(_db.Where(x => x.id == id).FirstOrDefault());

    public bool Delete(int id) {
        var index = _db.FindIndex(x => x.id == id);
        if (index == -1) {
            return false;
        }
        _db.RemoveAt(index);
        return true;
    }

    public Task<bool> Save(Contact data) {
        var index = _db.FindIndex(x => x.id == data.id);
        if (index == -1) {
            _db.Add(data);
        } else {
            _db[index] = data;
        }
        return Task.FromResult(true);
    }

    public Task<int> NextId() {
        return Task.FromResult(_counter++);
    }

    public Task<bool> Single(Func<Contact, bool> fn) {
        return Task.FromResult(_db.Any(fn));
    }

    public Task<bool> All(Func<Contact, bool> fn) {
        return Task.FromResult(_db.All(fn));
    }
}

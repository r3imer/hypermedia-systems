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

public record ContactDto(
    string? first_name = null,
    string? last_name = null,
    string? phone = null,
    string? email = null,
    int? id = null
);

public record ContactForm(
    string? first_name = null,
    string? last_name = null,
    string? phone = null,
    string? email = null
);

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
    string? q,
    int page,
    int pageSize
);

//public class ContactsRepo(Contact[] list) : IRepo<Contact, int> {
public class ContactsRepo(Contact[] list) {

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

    public Contact? Load(int id)
        => _db.Where(x => x.id == id).FirstOrDefault();

    public bool Delete(int id) {
        var index = _db.FindIndex(x => x.id == id);
        if (index == -1) {
            return false;
        }
        _db.RemoveAt(index);
        return true;
    }

    private bool Save(Contact data) {
        var index = _db.FindIndex(x => x.id == data.id);
        if (index == -1) {
            _db.Add(data);
        } else {
            _db[index] = data;
        }
        return true;
    }

    public string? ValidateEmail(string? email, int? id = null) {
        if (string.IsNullOrEmpty(email)) {
            return "Email is Required!";
        } else if (_db.Any(x => x.email == email && id is not null && id != x.id)) {
            return "Email not Unique!";
        }
        return null;
    }

    private static Errors Validate(ContactForm x) {
        var errors = new Errors();

        if (string.IsNullOrEmpty(x.first_name)) {
            errors["first_name"] = "First Name is Required!";
        }
        if (string.IsNullOrEmpty(x.last_name)) {
            errors["last_name"] = "Last Name is Required!";
        }

        return errors;
    }

    public Result<Contact, Errors> Create(ContactForm x) {
        var errors = Validate(x);
        errors["email"] = ValidateEmail(x.email);

        if (errors.IsEmpty()) {
            var c = new Contact(
                _counter++,
                x.first_name!,
                x.last_name!,
                x.email!,
                x.phone
            );
            if (Save(c)) {
                return c;
            }
            errors["save"] = "Could not Save to Database"; // TODO
        }

        return errors;
    }

    public Result<Contact, Errors> Update(ContactForm x, int id) {
        var errors = Validate(x);
        errors["email"] = ValidateEmail(x.email, id);

        if (errors.IsEmpty()) {
            var c = new Contact(
                id,
                x.first_name!,
                x.last_name!,
                x.email!,
                x.phone
            );
            if (Save(c)) {
                return c;
            }
            errors["save"] = "Could not Save to Database"; // TODO
        }

        return errors;
    }
}

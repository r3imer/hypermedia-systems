namespace Reim.Htmx.Web;

public record HtmlContent(string Html);

public record ContactDto(
    string? first_name,
    string? last_name,
    string? phone,
    string? email
);

public class Contact {
    public int id { get; set; }
    public string? first { get; set; }
    public string? last { get; set; }
    public string? phone { get; set; }
    public string? email { get; set; }
    public Dictionary<string, string> errors { get; set; } = [];
}

public record ContactsRequest(
    Contact[] contacts,
    string? q
);

public class ContactsRepo(Contact[] list) {

    private List<Contact> db = [.. list];
    private int _counter = list.Length;

    public Contact[] All() => db.ToArray();
    public Contact[] Search(string text) => db
        .Where(x =>
            (x.first?.Contains(text) ?? false) ||
            (x.last?.Contains(text) ?? false) ||
            (x.phone?.Contains(text) ?? false) ||
            (x.email?.Contains(text) ?? false)
        ).ToArray();

    public Contact? Get(int id) => db.Where(x => x.id == id).FirstOrDefault();

    public bool Add(Contact contact) {
        contact.id = _counter;
        _counter++;
        db.Add(contact);
        return true;
    }
}

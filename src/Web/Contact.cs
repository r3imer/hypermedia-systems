namespace Reim.Htmx.Web;

public record HtmlContent(string Html);

public class Contact {
    public required int id { get; set; }
    public required string? first { get; set; }
    public required string? last { get; set; }
    public required string? phone { get; set; }
    public required string? email { get; set; }
    public string[] errors { get; set; } = Array.Empty<string>();
}

public record ContactsRequest(
    Contact[] contacts,
    string? q
);

public class ContactsRepo(Contact[] list) {

    private List<Contact> db = [.. list];

    public Contact[] All() => db.ToArray();
    public Contact[] Search(string text) => db
        .Where(x =>
            (x.first?.Contains(text) ?? false) ||
            (x.last?.Contains(text) ?? false) ||
            (x.phone?.Contains(text) ?? false) ||
            (x.email?.Contains(text) ?? false)
        ).ToArray();
}

using System.Text.Json;

namespace Reim.Htmx.Web;

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


//public class DB {
//    public List<Contact> Contacts = new();

//    public void Load() {
//        var file = File.ReadAllText("contacts.json");
//        Contacts = JsonSerializer.Deserialize<Contact[]>(file)?.ToList() ?? new();
//    }

//    public void Save() {
//        File.WriteAllText("contacts.json", JsonSerializer.Serialize(Contacts));
//    }
//}

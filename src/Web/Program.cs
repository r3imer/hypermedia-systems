using Reim.Htmx.Web;
using System.Text.Json;

var bldr = WebApplication.CreateBuilder(args);
var serv = bldr.Services;

//serv.AddSingleton<DB>();
serv.AddSingleton<ContactsRepo>(_ => new(JsonSerializer.Deserialize<Contact[]>(File.ReadAllText("contacts.json")) ?? Array.Empty<Contact>()));

var app = bldr.Build();

app.MapGet("/",
() =>
    //Results.Extensions.RazorSlice("/Slices/Hello.cshtml", DateTime.Now)
    Results.Redirect("/contacts")
);

app.MapGet("/contacts",
(string? q, ContactsRepo db) => {
    var contacts = (q is null) switch {
        false => db.Search(q),
        true => db.All(),
    };
    return Results.Extensions.RazorSlice("/Slices/Index", contacts);
});

app.Run();

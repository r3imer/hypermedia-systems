using Reim.Htmx.Web;
using System.Text.Json;

var bldr = WebApplication.CreateBuilder(args);
var serv = bldr.Services;

//serv.AddSingleton<DB>();
serv.AddSingleton<ContactsRepo>(_ => new(JsonSerializer.Deserialize<Contact[]>(File.ReadAllText("contacts.json")) ?? Array.Empty<Contact>()));

var app = bldr.Build();

app.UseStaticFiles();

app.MapGet("/",
() =>
    //Results.Extensions.RazorSlice("/slices/test")
    Results.Redirect("/contacts")
);

//app.MapPost("/clicked",
//() =>
//    "clicked"
//);

app.MapGet("/contacts",
(string? q, ContactsRepo db) => {
    var contacts = (q is null) switch {
        false => db.Search(q),
        true => db.All(),
    };
    return Results.Extensions.RazorSlice("/slices/index", new ContactsRequest(contacts, q));
});

app.Run();

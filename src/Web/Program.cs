using RazorSlices;
using Reim.Htmx.Web;
using System.Text.Json;

var bldr = WebApplication.CreateBuilder(args);
var serv = bldr.Services;

//serv.AddSingleton<DB>();
serv.AddSingleton<ContactsRepo>(_
    => new(JsonSerializer.Deserialize<Contact[]>(
        File.ReadAllText("contacts2.json")) ?? []
    )
);

var app = bldr.Build();

app.UseStaticFiles();

app.MapGet("/",
() =>
    Results.Redirect("/contacts")
);

app.MapGet("/contacts",
async (HttpContext ctxt, string? q, ContactsRepo db) => {
    var contacts = (q is null) switch {
        false => db.Search(q),
        true => db.All(),
    };

    var slice = RazorSlice.Create(
        "slices/contacts",
        new ContactsRequest(contacts, q)
    );

    var body = await slice.RenderAsync();

    // TODO: BROKEN
    var html = RazorSlice.Create(
        "/slices/page",
        body
    );

    ctxt.Response.StatusCode = StatusCodes.Status200OK;
    ctxt.Response.ContentType = "text/html; charset=utf-8";
    
    return html.RenderAsync(ctxt.Response.Body);

});

app.Run();
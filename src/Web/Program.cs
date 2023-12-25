using Reim.Htmx.Web;
using Reim.Htmx.Web.Template;
using System.Text.Json;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Reim.Web;

var bldr = WebApplication.CreateBuilder(args);
var serv = bldr.Services;

serv.AddAntiforgery();

//serv.AddSingleton<DB>();
serv.AddSingleton<ContactsRepo>(_
    => new(JsonSerializer.Deserialize<Contact[]>(
        File.ReadAllText("contacts2.json")) ?? []
    )
);

serv.AddHttpLogging(opts => {
    opts.LoggingFields =
        HttpLoggingFields.Duration |
        HttpLoggingFields.ResponsePropertiesAndHeaders |
        HttpLoggingFields.Request
        //HttpLoggingFields.All
        ;
});

var app = bldr.Build();
var log = app.Services.GetService<ILogger<Program>>();

//app.UseMiddleware<CatchExceptions>();
app.UseStaticFiles();
app.UseHttpLogging();
app.UseAntiforgery();

app.Use((ctxt,next) => {
    try {
        return next(ctxt);
    } catch (Exception ex) {
        log.LogError(ex, "Something went wrong");
        ctxt.Response.StatusCode = 500;
        ctxt.Response.WriteAsync("Something went wrong");
        return Task.CompletedTask;
    }
});

app.MapGet("/",
() =>
    Results.Redirect("/contacts")
);

app.MapGet("/contacts",
(string? q, ContactsRepo db) => {
    var contacts = (q is null) switch {
        false => db.Search(q),
        true => db.All(),
    };

    var tmp = new ContactsRequest(contacts, q);
    var html = tmp.ToIndex().ToLayout();

    return html.AsHtml();
});

app.MapGet("/contacts/new",
() => {
    return new Contact().ToNew().ToLayout().AsHtml();
});

app.MapPost("/contacts/new",
([FromForm] ContactDto contact, ContactsRepo db) => {
    var c = contact.To();
    if(db.Add(c)) {
        Flashes.Add("Created New Contact!");
        return Results.Redirect("/contacts");
    } else {
        return c.ToNew().ToLayout().AsHtml();
    }
})
.DisableAntiforgery();

app.MapGet("/contacts/{id}",
(int id, ContactsRepo db) => {
    var contact = db.Get(id);
    if (contact is null) {
        Flashes.Add($"Contact '{id}' not found");
        return Results.Redirect("/contacts");
    }
    return contact.ToShow().ToLayout().AsHtml();
});


app.Run();

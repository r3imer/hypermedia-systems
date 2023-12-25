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
var log = app.Services.GetService<ILogger<Program>>()!;

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
    var html = tmp.HtmlIndex().HtmlLayout();

    return html.AsHtml();
});

app.MapGet("/contacts/new",
() => {
    return new Contact().HtmlNew().HtmlLayout().AsHtml();
});

app.MapPost("/contacts/new",
([FromForm] ContactDto contact, ContactsRepo db) => {
    var c = contact.To();
    if(db.Add(c)) {
        Flashes.Add("Created New Contact!");
        return Results.Redirect("/contacts");
    } else {
        return c.HtmlNew().HtmlLayout().AsHtml();
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
    return contact.HtmlShow().HtmlLayout().AsHtml();
});

app.MapGet("/contacts/{id}/edit",
(int id, ContactsRepo db) => {
    var contact = db.Get(id);
    if (contact is null) {
        Flashes.Add($"Contact '{id}' not found");
        return Results.Redirect("/contacts");
    }
    return contact.HtmlEdit().HtmlLayout().AsHtml();
});

app.MapPost("/contacts/{id}/edit",
(int id, [FromForm] ContactDto contact, ContactsRepo db) => {
    var c = db.Get(id); 
    if (c is null) {
        Flashes.Add($"Contact '{id}' not found");
        return Results.Redirect("/contacts");
    }
    if (c.Update(contact)) {
        Flashes.Add("Updated Contact!");
        return Results.Redirect($"/contacts/{id}");
    }
    return c.HtmlEdit().HtmlLayout().AsHtml();
})
.DisableAntiforgery();

app.MapPost("/contacts/{id}/delete",
(int id, ContactsRepo db) => {
    if (db.Delete(id)) {
        Flashes.Add("Delete Contact!");
    } else {
        Flashes.Add($"Contact '{id}' not found");
    }
    return Results.Redirect("/contacts");
})
.DisableAntiforgery();

app.Run();

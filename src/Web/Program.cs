using Reim.Htmx.Web;
using Reim.Htmx.Web.Template;
using System.Text.Json;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;

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

app.Use((ctxt, next) => {
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
    return new ContactForm().HtmlNew(null).HtmlLayout().AsHtml();
});

app.MapPost("/contacts/new",
async ([FromForm] ContactForm contact, ContactsRepo db) => {
    var c = await contact.Create(db, await db.NextId());
    return await c.Match(
        async ok => {
            if (await db.Save(ok)) {
                Flashes.Add("Created New Contact!");
                return Results.Redirect("/contacts");
            } else {
                Flashes.Add("Problem by Saving to Database!");
                return ok.ToForm().HtmlNew(null).HtmlLayout().AsHtml();
            }
        },
        async err => {
            return contact.HtmlNew(err).HtmlLayout().AsHtml();
        }
    );
})
.DisableAntiforgery();

app.MapGet("/contacts/{id}",
async (int id, ContactsRepo db) => {
    var contact = await db.Load(id);
    if (contact is null) {
        Flashes.Add($"Contact '{id}' not found");
        return Results.Redirect("/contacts");
    }
    return contact.ToForm().HtmlShow(id).HtmlLayout().AsHtml();
});

app.MapGet("/contacts/{id}/edit",
async (int id, ContactsRepo db) => {
    var contact = await db.Load(id);
    if (contact is null) {
        Flashes.Add($"Contact '{id}' not found");
        return Results.Redirect("/contacts");
    }
    return contact.ToForm().HtmlEdit(id, null).HtmlLayout().AsHtml();
});

app.MapPost("/contacts/{id}/edit",
async (int id, [FromForm] ContactForm contact, ContactsRepo db) => {
    var old = await db.Load(id);
    if (old is null) {
        Flashes.Add($"Contact '{id}' not found");
        return Results.Redirect("/contacts");
    }
    var c = await contact.Create(db, id);
    return await c.Match(
        async ok => {
            if (await db.Save(ok)) {
                Flashes.Add("Updated Contact!");
                return Results.Redirect($"/contacts/{id}");
            } else {
                Flashes.Add("Problem by Saving to Database!");
                return ok.ToForm().HtmlEdit(id,null).HtmlLayout().AsHtml();
            }
        },
        async err => {
            return contact.HtmlEdit(id, err).HtmlLayout().AsHtml();
        }
    );
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

using Microsoft.AspNetCore.Mvc;
using Reim.Htmx.Web.Template;
using Reim.Http;

namespace Reim.Htmx.Web;

public class Endpoint {  };

public static class EndpointsWeb {
    public static IEndpointRouteBuilder MapWebEndpoints(this IEndpointRouteBuilder x) {


        x.MapGet("/",
        (string? q, ContactsRepo db) => {
            var contacts = (q is null) switch {
                false => db.Search(q),
                true => db.All(),
            };

            var tmp = new ContactsRequest(contacts, q);
            var html = tmp.HtmlIndex().HtmlLayout();

            return html.AsHtml();
        });

        x.MapGet("/new",
        () => {
            return new ContactForm().HtmlNew(null).HtmlLayout().AsHtml();
        });

        x.MapPost("/new",
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
        });

        x.MapGet("/{id}",
        async (int id, ContactsRepo db) => {
            var contact = await db.Load(id);
            if (contact is null) {
                Flashes.Add($"Contact '{id}' not found");
                return Results.Redirect("/contacts");
            }
            return contact.ToForm().HtmlShow(id).HtmlLayout().AsHtml();
        });

        x.MapGet("/{id}/edit",
        async (int id, ContactsRepo db) => {
            var contact = await db.Load(id);
            if (contact is null) {
                Flashes.Add($"Contact '{id}' not found");
                return Results.Redirect("/contacts");
            }
            return contact.ToForm().HtmlEdit(id, null).HtmlLayout().AsHtml();
        });

        x.MapPost("/{id}/edit",
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
                        return ok.ToForm().HtmlEdit(id, null).HtmlLayout().AsHtml();
                    }
                },
                async err => {
                    return contact.HtmlEdit(id, err).HtmlLayout().AsHtml();
                }
            );
        });

        x.MapDelete("/{id}",
        (int id, ContactsRepo db, HttpContext ctxt) => {
            if (db.Delete(id)) {
                Flashes.Add("Delete Contact!");
            } else {
                Flashes.Add($"Contact '{id}' not found");
            }
            //var r = TypedResults.StatusCode(303);
            ctxt.Response303("/contacts");
        });

        return x;
    }
}

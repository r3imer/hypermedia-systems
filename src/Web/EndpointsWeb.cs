using Microsoft.AspNetCore.Mvc;
using Reim.Htmx.Web.Template;
using Reim.Http;

namespace Reim.Htmx.Web;

public class Endpoint {  };

public static class EndpointsWeb {
    public static IEndpointRouteBuilder MapWebEndpoints(this IEndpointRouteBuilder x) {

        x.MapGet("/",
        async (
            ContactsRepo db,
            [FromHeader(Name = "HX-Trigger")] string? trigger,
            string? q,
            int page = 0
        ) => {
            var query = new QueryContacts(q, page);

            var c = await db.Query(query);

            return trigger switch {
                "search" => c.HtmlRows().AsHtml(),
                _ => c.HtmlIndex().HtmlLayout().AsHtml(),
            };
        });

        x.MapGet("/count",
        async (ContactsRepo db) => {
            await Task.Delay(400);
            var total = (await db.All()).Length;
            return $"({total} total Contacts)".AsHtml();
        });

        x.MapGet("/new",
        () => {
            return new ContactDto().HtmlNew(null).HtmlLayout().AsHtml();
        });

        x.MapPost("/new",
        ([FromForm] ContactForm contact, ContactsRepo db) => {
            var c = db.Create(contact);
            return c.Match(
                ok => {
                    Flashes.Add("Created New Contact!");
                    return Results.Redirect("/contacts");
                },
                err => {
                    return contact.ToDto(null).HtmlNew(err).HtmlLayout().AsHtml();
                }
            );
        });

        x.MapGet("/{id}",
        (int id, ContactsRepo db) => {
            var contact = db.Load(id);
            if (contact is null) {
                Flashes.Add($"Contact '{id}' not found");
                return Results.Redirect("/contacts");
            }
            return contact.ToDto().HtmlShow().HtmlLayout().AsHtml();
        });

        x.MapGet("/{id}/edit",
        (int id, ContactsRepo db) => {
            var contact = db.Load(id);
            if (contact is null) {
                Flashes.Add($"Contact '{id}' not found");
                return Results.Redirect("/contacts");
            }
            return contact.ToDto().HtmlEdit(null).HtmlLayout().AsHtml();
        });

        x.MapPost("/{id}/edit",
        (int id, [FromForm] ContactForm contact, ContactsRepo db) => {
            var old = db.Load(id);
            if (old is null) {
                Flashes.Add($"Contact '{id}' not found");
                return Results.Redirect("/contacts");
            }
            var c = db.Update(contact, id);
            return c.Match(
                ok => {
                    Flashes.Add("Updated Contact!");
                    return Results.Redirect($"/contacts/{id}");
                },
                err => {
                    return contact.ToDto(id).HtmlEdit(err).HtmlLayout().AsHtml();
                }
            );
        });

        x.MapGet("/{id}/email",
        (ContactsRepo db, int id, string? email = null) => {
            var r = db.ValidateEmail(email, id);
            return r.AsHtml();
        });

        x.MapDelete("/{id}",
        (int id, ContactsRepo db, HttpContext ctxt) => {
            if (db.Delete(id)) {
                Flashes.Add("Delete Contact!");
            } else {
                Flashes.Add($"Contact '{id}' not found");
            }
            ctxt.Response303("/contacts");
        });

        return x;
    }
}

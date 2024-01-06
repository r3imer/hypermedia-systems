using Microsoft.AspNetCore.Mvc;
using Reim.Htmx.Archiver;
using Reim.Htmx.Web.Template;
using Reim.Http;

namespace Reim.Htmx.Web;

public class Endpoint {  };

public static class EndpointsWeb {
    public static IEndpointRouteBuilder MapWebEndpoints(this IEndpointRouteBuilder x) {

        x.MapGet("/",
        (
            ContactsRepo db,
            IArchiver ar,
            [FromHeader(Name = "HX-Trigger")] string? trigger,
            string? q,
            int page = 0
        ) => {
            var query = new QueryContacts(q, page);

            var c = db.Query(query);

            return trigger switch {
                "search" => c.HtmlRows().AsHtml(),
                _ => c.HtmlIndex(ar).HtmlLayout().AsHtml(),
            };
        });

        x.MapDelete("/",
        (
            [FromForm] ContactDeleteForm form,
            ContactsRepo db,
            IArchiver ar
        ) => {
            _ = form.selected_contact_ids.Select(x => db.Delete(x)).ToArray();
            Flashes.Add("Deleted Contacts!");
            Contacts c = db.Query(new());
            return c.HtmlIndex(ar).HtmlLayout().AsHtml();
        });

        x.MapGet("/count",
        (ContactsRepo db) => {
            var total = db.All().Length;
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
        async (
            [FromHeader(Name = "HX-Trigger")] string? trigger,
            int id,
            ContactsRepo db,
            HttpContext ctxt
        ) => {
            var found = db.Delete(id);
            if (found && trigger == "delete-btn") {
                Flashes.Add("Delete Contact!");
                ctxt.Response303("/contacts");
            } else {
                await ctxt.Response200Html("");
            }
        });

        x.MapGet("/archive",
        (IArchiver a) => {
            return a.HtmlArchiver().AsHtml();
        });

        x.MapPost("/archive",
        (IArchiver a) => {
            a.run();
            return a.HtmlArchiver().AsHtml();
        });

        return x;
    }
}

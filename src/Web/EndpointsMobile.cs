using Microsoft.AspNetCore.Mvc;
using Reim.Htmx.Archiver;
using Reim.Htmx.Web.Template;
using Reim.Http;
using Reim.Std.Domain;

namespace Reim.Htmx.Web;

public static class EndpointsMobile {
    public static IEndpointRouteBuilder MapMobileEndpoints(this IEndpointRouteBuilder x) {

        //x.MapGet("/",
        //() => {
        //    return Hxml.HalloWorld().AsHxml();
        //});

        x.MapGet("/",
        (
            ContactsRepo db,
            IArchiver ar,
            string? rows_only,
            string? q,
            int page = 0
        ) => {
            var query = new QueryContacts(q, page);

            var c = db.Query(query);

            return rows_only switch {
                "true" => c.HxmlRows().AsHxml(),
                _ => c.HxmlIndex(ar).HxmlLayout().AsHxml(),
            };
        });

        //x.MapDelete("/",
        //(
        //    [FromForm] ContactDeleteForm form,
        //    ContactsRepo db,
        //    IArchiver ar
        //) => {
        //    _ = form.selected_contact_ids.Select(x => db.Delete(x)).ToArray();
        //    Flashes.Add("Deleted Contacts!");
        //    Contacts c = db.Query(new());
        //    return c.HtmlIndex(ar).HtmlLayout().AsHtml();
        //});

        //x.MapGet("/count",
        //(ContactsRepo db) => {
        //    var total = db.All().Length;
        //    return $"({total} total Contacts)".AsHtml();
        //});

        //x.MapGet("/new",
        //() => {
        //    return new ContactDto().HtmlNew(null).HtmlLayout().AsHtml();
        //});

        //x.MapPost("/new",
        //([FromForm] ContactForm contact, ContactsRepo db) => {
        //    var c = db.Create(contact);
        //    return c.Match(
        //        ok => {
        //            Flashes.Add("Created New Contact!");
        //            return Results.Redirect("/contacts");
        //        },
        //        err => {
        //            return contact.ToDto(null).HtmlNew(err).HtmlLayout().AsHtml();
        //        }
        //    );
        //});

        x.MapGet("/{id}",
        (int id, ContactsRepo db) => {
            var contact = db.Load(id);
            if (contact is null) {
                return Results.NotFound();
            }
            return contact?.ToDto().HxmlShow().HxmlLayout().AsHxml();
        });

        x.MapGet("/{id}/edit",
        (int id, ContactsRepo db) => {
            var contact = db.Load(id);
            if (contact is null) {
                return Results.NotFound();
            }
            return contact?.ToDto().HxmlEdit(null, false).HxmlLayout().AsHxml();
        });

        x.MapPost("/{id}/edit",
        (int id, [FromForm] ContactForm contact, ContactsRepo db) => {
            var old = db.Load(id);
            if (old is null) {
                return Results.NotFound();
            }
            var c = db.Update(contact, id);
            var (err, saved) = c.Match<(Errors?, bool)>(
                ok => (null, true),
                err => (err, false)
            );
            return contact.ToDto(id).HxmlFields(err, saved).AsHxml();
        });

        //x.MapGet("/{id}/email",
        //(ContactsRepo db, int id, string? email = null) => {
        //    var r = db.ValidateEmail(email, id);
        //    return r.AsHtml();
        //});

        x.MapPost("/{id}/delete",
        (
            int id,
            ContactsRepo db
        ) => {
            var success = db.Delete(id);
            return success.HxmlDeleted().AsHxml();
        });

        //x.MapGet("/archive",
        //(IArchiver a) => {
        //    return a.HtmlArchiver().AsHtml();
        //});

        //x.MapPost("/archive",
        //(IArchiver a) => {
        //    a.run();
        //    return a.HtmlArchiver().AsHtml();
        //});

        //x.MapDelete("/archive",(IArchiver a) => {
        //    a.reset();
        //    return a.HtmlArchiver().AsHtml();
        //});

        //x.MapGet("/archive/file",(IArchiver a) => {
        //    return Results.File(a.archive_file(), fileDownloadName: "archive.json");
        //});


        return x;
    }
}

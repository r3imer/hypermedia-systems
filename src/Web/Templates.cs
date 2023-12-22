namespace Reim.Htmx.Web;

public static class Templates {
    public static string ToHtml(this string[] x) => string.Join("\n", x);
    public static string ToHtml(this IEnumerable<string> x) => string.Join("\n", x);
    public static IResult AsHtml(this string x) => Results.Content(x, "text/html; charset=utf-8");

    public static string ToHtml(this ContactsRequest x) => $"""
        <form action="/contacts" method="get" class="tool-bar">
          <label for="search">Search Term</label>
          <input id="search" type="search" name="q" value="{x.q}" />
          <input type="submit" value="Search" />
        </form>
        <table>
          <thead>
            <tr>
              <th></th>
              <th>First</th>
              <th>Last</th>
              <th>Phone</th>
              <th>Email</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {x.contacts.Select(ToHtml).ToHtml()}
          </tbody>
        </table>
        """;

    public static string ToHtml(this Contact x) => $"""
        <tr>
          <td>
            <input type="checkbox" name="selected_contact_ids" value="{x.id}" x-model="selected">
          </td>
          <td>{x.first}</td>
          <td>{x.last}</td>
          <td>{x.phone}</td>
          <td>{x.email}</td>
          <td>
            <div data-overflow-menu>
              <button type="button" aria-haspopup="menu" aria-controls="contact-menu-{x.id}">
                Options
              </button>
              <div role="menu" hidden id="contact-menu-{x.id}">
                <a role="menuitem" href="/contacts/{x.id}/edit">Edit</a>
                <a role="menuitem" href="/contacts/{x.id}">View</a>
                <a role="menuitem" href="#"
                   hx-delete="/contacts/{x.id}"
                   hx-confirm="Are you sure you want to delete this contact?"
                   hx-swap="outerHTML swap:1s"
                   hx-target="closest tr">Delete</a>
              </div>
            </div>
          </td>
        </tr>
        """;

    public static string ToPage(this string x) => $"""
        <!doctype html>
        <html lang="en">
        <head>
          <title>Contact App</title>
          <link rel="stylesheet" href="/static/site.css">
          <script src="/static/js/htmx-1.8.0.js"></script>
          <script src="/static/js/_hyperscript-0.9.7.js"></script>
          <script src="/static/js/rsjs-menu.js" type="module"></script>
          <script defer src="https://unpkg.com/alpinejs@3/dist/cdn.min.js"></script>
        </head>
        <body hx-boost="true">
          <main>
            <header>
              <h1>
                <all-caps>contacts.app</all-caps>
                <sub-title>A Demo Contacts Application</sub-title>
              </h1>
            </header>
              {x}
            </main>
        </body>
        </html>
        """;
}

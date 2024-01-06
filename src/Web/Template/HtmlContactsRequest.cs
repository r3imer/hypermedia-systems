using Reim.Htmx.Archiver;

namespace Reim.Htmx.Web.Template;

public static partial class Template {

    public static string HtmlArchiver(this IArchiver a) {
        int p = (int) (a.progress() * 100);
        return $$"""
            <div id="archive-ui" hx-target="this" hx-swap="outerHTML">
                {{ a.status() switch {
                    Status.Waiting => $$"""
                        <button hx-post="/contacts/archive">
                            Download Contact Archive
                        </button>
                        """,
                    Status.Running => $$"""
                        <div hx-get="/contacts/archive" hx-trigger="load delay:500ms">
                            Creating Archive...
                            <div class="progress" >
                                <div class="progress-bar" role="progressbar"
                                     aria-valuenow="{{ p }}"
                                     style="width:{{ p }}%"></div>
                            </div>
                        </div>
                        """,
                    Status.Complete =>
                        "",
                }}}
            </div>
            """;
    }

    public static string HtmlIndex(this Contacts a, IArchiver b) => $$"""
        <p>{{ b.HtmlArchiver() }}</p>
        <form action="/contacts" method="get" class="tool-bar">
            <label for="search">Search Term</label>
            <input id="search" type="search" name="q" value="{{ a.q.q }}"
                   hx-get="/contacts"
                   hx-target="tbody"
                   hx-select="tbody tr"
                   hx-push-url="true"
                   hx-indicator="#spinner"
                   hx-trigger="keyup delay:300ms changed"/>
            <input type="submit" value="Search" />
            <img style="height: 26px" id="spinner" class="htmx-indicator" src="/static/img/spinning-circles.svg"/>
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
                    {{ a.HtmlRows() }}
                </tbody>
                <button hx-delete="/contacts"
                        hx-confirm="Are you sure you want to delete these contacts?"
                        hx-target="body">
                    Delete Selected Contacts
                </button>
            </table>
        </form>
        <p>
            <a href="/contacts/new">Add Contact</a>
            <span hx-get="/contacts/count" hx-trigger="load">
                <img style="height: 20px" id="spinner" class="htmx-indicator" src="/static/img/spinning-circles.svg"/>
            </span>
        </p>
        """;

    public static string HtmlRows(this Contacts a) => a.arr.Select(b => $$"""
        <tr>
            <td><input type="checkbox" name="selected_contact_ids" value="{{ b.id }}"></td>
            <td>{{ b.first }}</td>
            <td>{{ b.last }}</td>
            <td>{{ b.phone }}</td>
            <td>{{ b.email }}</td>
            <td>
                <a href="/contacts/{{ b.id }}/edit">Edit</a>
                <a href="/contacts/{{ b.id }}">View</a>
                <a href="#" hx-delete="/contacts/{{ b.id }}"
                   hx-swap="outerHTML swap:1s"
                   hx-confirm="Are you sure you want to delete this contact?"
                   hx-target="closest tr">Delete</a>
            </td>
        </tr>
        """).Join() + ( a.arr.Length == Const.PAGE_SIZE ? $$"""
            <tr>
                <td colspan="5" style="text-align: center">
                    <button hx-target="closest tr"
                            hx-swap="outerHTML"
                            hx-select="tbody > tr"
                            hx-get="/contacts?page={{ a.q.page + 1 }}&q={{ a.q.q }}">
                        Load More
                    </button>
                </td>
            </tr>
        """ : "");

}

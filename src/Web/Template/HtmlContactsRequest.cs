namespace Reim.Htmx.Web.Template;

public static partial class Template {

    public static string HtmlIndex(this Contacts a) => $$"""
        <form action="/contacts" method="get" class="tool-bar">
            <label for="search">Search Term</label>
            <input id="search" type="search" name="q" value="{{ a.q.q }}"
                   hx-get="/contacts"
                   hx-target="tbody"
                   hx-select="tbody tr"
                   hx-push-url="true"
                   hx-trigger="keyup delay:300ms changed"/>
            <input type="submit" value="Search" />
        </form>
        <table>
            <thead>
                <tr>
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
        </table>
        <p>
            <a href="/contacts/new">Add Contact</a>
        </p>
        """;

    public static string HtmlRows(this Contacts a) => a.arr.Select(b => $$"""
        <tr>
            <td>{{ b.first }}</td>
            <td>{{ b.last }}</td>
            <td>{{ b.phone }}</td>
            <td>{{ b.email }}</td>
            <td>
                <a href="/contacts/{{ b.id }}/edit">Edit</a>
                <a href="/contacts/{{ b.id }}">View</a>
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

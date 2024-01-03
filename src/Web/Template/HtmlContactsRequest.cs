namespace Reim.Htmx.Web.Template;

public static partial class Template {

    public static string HtmlIndex(this ContactsRequest x) => $$"""
        <form action="/contacts?size={{ x.pageSize }}" method="get" class="tool-bar">
          <label for="search">Search Term</label>
          <input id="search" type="search" name="q" value="{{ x.q }}"
                 hx-get="/contacts?size={{ x.pageSize }}"
                 hx-target="tbody"
                 hx-select="tbody tr"
                 hx-push-url="true"
                 hx-trigger="search, keyup delay:300ms changed"/>
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
            {{ x.HtmlRows() }}
          </tbody>
        </table>

        <p>
            <a href="/contacts/new">Add Contact</a>
        </p>
        """;

    public static string HtmlRows(this ContactsRequest x) => $$"""
            {{ x.contacts.Select(x => x.ToDto().HtmlRow()).ToHtml() }}
            {{( x.contacts.Length == x.pageSize ? $$"""
                <tr>
                    <td colspan="5" style="text-align: center">
                        <button hx-target="closest tr"
                                hx-swap="outerHTML"
                                hx-select="tbody > tr"
                                hx-get="/contacts?page={{ x.page + 1 }}&size={{ x.pageSize }}&q={{ x.q }}">
                          Load More
                        </button>
                    </td>
                </tr>
                """ : ""
            )}}
        """;

}

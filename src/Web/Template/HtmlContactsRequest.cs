namespace Reim.Htmx.Web.Template;

public static partial class Template {

    public static string HtmlIndex(this ContactsRequest x) => $$"""
        <form action="/contacts" method="get" class="tool-bar">
          <label for="search">Search Term</label>
          <input id="search" type="search" name="q" value="{{ x.q }}"/>
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
            {{ x.contacts.Select(x => x.ToDto().HtmlRow()).ToHtml() }}
          </tbody>
        </table>

        <div>
            <span style="float: right">
                {{ (x.page >= 1
                    ? $$"""<a href="/contacts?page={{ x.page - 1 }}&size={{ x.pageSize }}">Previous</a>""" : ""
                )}}
                {{ (x.contacts.Length == x.pageSize
                    ? $$"""<a href="/contacts?page={{ x.page + 1 }}&size={{ x.pageSize }}">Next</a>""" : ""
                )}}
            </span>
        </div>

        <p>
            <a href="/contacts/new">Add Contact</a>
        </p>
        """;

}

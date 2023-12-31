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

        <p>
            <a href="/contacts/new">Add Contact</a>
        </p>
        """;

}

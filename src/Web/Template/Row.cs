namespace Reim.Htmx.Web.Template;

public static partial class Template {

    public static string ToRow(this Contact x) => $"""
        <tr>
          <td>{x.first}</td>
          <td>{x.last}</td>
          <td>{x.phone}</td>
          <td>{x.email}</td>
          <td>
              <a href="/contacts/{x.id}/edit">Edit</a>
              <a href="/contacts/{x.id}">View</a>
          </td>
        </tr>
        """;

}

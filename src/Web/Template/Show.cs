namespace Reim.Htmx.Web.Template;

public static partial class Template {

    public static string ToShow(this Contact x) => $"""
        <h1>{x.first} {x.last}</h1>

        <div>
          <div>Phone: {x.phone}</div>
          <div>Email: {x.email}</div>
        </div>

        <p>
          <a href="/contacts/{x.id}/edit">Edit</a>
          <a href="/contacts">Back</a>
        </p>
        """;

}

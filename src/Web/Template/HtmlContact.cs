namespace Reim.Htmx.Web.Template;

public static partial class Template {

    public static string HtmlNew(this Contact x) => $$"""
        <form action="/contacts/new" method="post">
            <fieldset>
                <legend>Contact Values</legend>
                <p>
                    <label for="email">Email</label>
                    <input name="email" id="email" type="email" placeholder="Email" value="{{x.email ?? ""}}">
                    <span class="error">{{x.errors.GetValueOrDefault("email")}}</span>
                </p>
                <p>
                    <label for="first_name">First Name</label>
                    <input name="first_name" id="first_name" type="text" placeholder="First Name" value="{{x.first ?? ""}}">
                    <span class="error">{{x.errors.GetValueOrDefault("first")}}</span>
                </p>
                <p>
                    <label for="last_name">Last Name</label>
                    <input name="last_name" id="last_name" type="text" placeholder="Last Name" value="{{x.last ?? ""}}">
                    <span class="error">{{x.errors.GetValueOrDefault("last")}}</span>
                </p>
                <p>
                    <label for="phone">Phone</label>
                    <input name="phone" id="phone" type="text" placeholder="Phone" value="{{x.phone ?? ""}}">
                    <span class="error">{{x.errors.GetValueOrDefault("phone")}}</span>
                </p>
                <button>Save</button>
            </fieldset>
        </form>

        <p>
            <a href="/contacts">Back</a>
        </p>
        """;

    public static string HtmlShow(this Contact x) => $$"""
        <h1>{{x.first}} {{x.last}}</h1>

        <div>
          <div>Phone: {{x.phone}}</div>
          <div>Email: {{x.email}}</div>
        </div>

        <p>
          <a href="/contacts/{{x.id}}/edit">Edit</a>
          <a href="/contacts">Back</a>
        </p>
        """;

    public static string HtmlRow(this Contact x) => $$"""
        <tr>
          <td>{{x.first}}</td>
          <td>{{x.last}}</td>
          <td>{{x.phone}}</td>
          <td>{{x.email}}</td>
          <td>
              <a href="/contacts/{{x.id}}/edit">Edit</a>
              <a href="/contacts/{{x.id}}">View</a>
          </td>
        </tr>
        """;

}

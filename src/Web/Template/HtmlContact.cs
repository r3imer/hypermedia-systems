using Reim.Std.Domain;

namespace Reim.Htmx.Web.Template;

public static partial class Template {

    private static string HtmlForm(this ContactDto x, Errors? errors) => $$"""
        <fieldset>
            <legend>Contact Values</legend>
            <p>
                <label for="email">Email</label>
                <input name="email" id="email" type="email"
                       hx-get="/contacts/{{ x.id }}/email"
                       hx-target="next .error"
                       hx-trigger="keyup delay:500ms changed"
                       placeholder="Email" value="{{ x.email }}">
                <span class="error">{{ errors?["email"] }}</span>
            </p>
            <p>
                <label for="first_name">First Name</label>
                <input name="first_name" id="first_name" type="text"
                       placeholder="First Name" value="{{ x.first_name }}">
                <span class="error">{{ errors?["first_name"] }}</span>
            </p>
            <p>
                <label for="last_name">Last Name</label>
                <input name="last_name" id="last_name" type="text"
                       placeholder="Last Name" value="{{ x.last_name }}">
                <span class="error">{{ errors?["last_name"] }}</span>
            </p>
            <p>
                <label for="phone">Phone</label>
                <input name="phone" id="phone" type="text"
                       placeholder="Phone" value="{{ x.phone }}">
                <span class="error">{{ errors?["phone"] }}</span>
            </p>
            <button>Save</button>
        </fieldset>
        """;

    public static string HtmlNew(this ContactDto x, Errors? errors) => $$"""
        <form action="/contacts/new" method="post">
            {{ x.HtmlForm(errors) }}
        </form>
        <p>
            <a href="/contacts">Back</a>
        </p>
        """;

    public static string HtmlEdit(this ContactDto x, Errors? errors) => $$"""
        <form action="/contacts/{{ x.id }}/edit" method="post">
            {{ x.HtmlForm(errors) }}
        </form>
        <button hx-delete="/contacts/{{ x.id }}"
                hx-target="body"
                hx-confirm="Are you sure you want to delete this contact?"
                hx-push-url="true">
            Delete Contact
        </button>
        <p>
            <a href="/contacts/">Back</a>
        </p>
        """;

    public static string HtmlShow(this ContactDto x) => $$"""
        <h1>{{ x.first_name }} {{ x.last_name }}</h1>
        <div>
          <div>Phone: {{ x.phone }}</div>
          <div>Email: {{ x.email }}</div>
        </div>
        <p>
          <a href="/contacts/{{ x.id }}/edit">Edit</a>
          <a href="/contacts">Back</a>
        </p>
        """;

    public static string HtmlRow(this ContactDto x) => $$"""
        <tr>
          <td>{{ x.first_name }}</td>
          <td>{{ x.last_name }}</td>
          <td>{{ x.phone }}</td>
          <td>{{ x.email }}</td>
          <td>
              <a href="/contacts/{{ x.id }}/edit">Edit</a>
              <a href="/contacts/{{ x.id }}">View</a>
          </td>
        </tr>
        """;

}

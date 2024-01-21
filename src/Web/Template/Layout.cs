namespace Reim.Htmx.Web.Template;

public static class Flashes {
    private static List<string> s_list = [];
    public static string[] Get() {
        var arr = s_list.ToArray();
        s_list = [];
        return arr;
    }
    public static void Add(string flash) { s_list.Add(flash); }
}

public static partial class Template {

    public static string HtmlLayout(this string x) => $$"""
        <!doctype html>
        <html lang="en">
        <head>
            <title>Contact App</title>
            <link rel="stylesheet" href="/static/missing-1.1.1.css">
            <link rel="stylesheet" href="/static/site.css">
            <script src="/static/js/htmx-1.9.10.js"></script>
            <script src="/static/js/_hyperscript-0.9.12.js"></script>
            <script src="/static/js/rsjs-menu.js" type="module"></script>
            <script defer src="https://unpkg.com/alpinejs"></script>
        </head>
        <body hx-boost="true">
        <main>
            <header>
                <h1>
                    <all-caps>contacts.app</all-caps>
                    <sub-title>A Demo Contacts Application</sub-title>
                </h1>
            </header>
            {{ Flashes.Get().Select(ToFlash).Join() }}
            {{ x }}
            <br>
            <br>
        </main>
        <script src="/_framework/aspnetcore-browser-refresh.js"></script>
        </body>
        </html>
        """;


    public static string HxmlLayout(this string x, string? header = null) => $$"""
<doc xmlns="https://hyperview.org/hyperview">
  <screen>
    <styles></styles>
    <body style="body" safe-area="true">
      <header style="header">
      {{ header ?? """
        <text style="header-title">Contact.app</text>
"""   }}
      </header>

      <view style="main">
        {{ x }}
      </view>
    </body>
  </screen>
</doc>
""";

    private static string ToFlash(string message) => $$"""
        <div class="flash">{{ message }}</div>
        """;

}

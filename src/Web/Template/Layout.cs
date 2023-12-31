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
            <link rel="stylesheet" href="/static/missing.css">
            <link rel="stylesheet" href="/static/site.css">
            <script src="/static/js/htmx-1.8.0.js"></script>
            <script src="/static/js/_hyperscript-0.9.7.js"></script>
            <script src="/static/js/rsjs-menu.js" type="module"></script>
            <script defer src="https://unpkg.com/alpinejs@3.13.3/dist/cdn.min.js"></script>
        </head>
        <body hx-boost="true">
        <main>
            <header>
                <h1>
                    <all-caps>contacts.app</all-caps>
                    <sub-title>A Demo Contacts Application</sub-title>
                </h1>
            </header>

            {{Flashes.Get().Select(ToFlash).ToHtml()}}

            {{x}}
        </main>
        <script src="/_framework/aspnetcore-browser-refresh.js"></script>
        </body>
        </html>
        """;

    private static string ToFlash(string message) => $$"""
        <div class="flash">{{message}}</div>
        """;

}

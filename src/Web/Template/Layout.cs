namespace Reim.Htmx.Web.Template;

public static class Flashes {
    private static List<string> s_list = [];
    public static string[] Get() {
        var arr = s_list.ToArray();
        s_list = [];
        return arr;
    }
    public static void Add(string flash) { s_list.Add(flash); }

    public static string Html() => s_list.Select(x => $$"""
        <div class="flash">{{ x }}</div>
        """).Join();

    public static string Hxml() => s_list.Select(x => $$"""
  <behavior 
    xmlns:message="https://hypermedia.systems/hyperview/message"
    trigger="load"
    action="show-message"
    message:text="{{ x }}"
  />
""").Join();

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
            {{ Flashes.Html() }}
            {{ x }}
            <br>
            <br>
        </main>
        <script src="/_framework/aspnetcore-browser-refresh.js"></script>
        </body>
        </html>
        """;


}

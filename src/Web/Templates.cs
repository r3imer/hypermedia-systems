namespace Reim.Htmx.Web;

public static class Templates {
    public static string ToHtml(this string[] x) => string.Join("\n", x);
    public static string ToHtml(this IEnumerable<string> x) => string.Join("\n", x);
    public static IResult AsHtml(this string x) => Results.Content(x, "text/html; charset=utf-8");

    public static string ToHtml(this ContactsRequest x) => $"""
        <form action="/contacts" method="get" class="py-4 tool-bar">
          <label for="search">Search Term</label>
          <input id="search" type="search" name="q" value="{x.q}" class="block w-full rounded-md border-0 py-1.5 pl-7 pr-20 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"/>
          <input type="submit" value="Search" />
        </form>
        <table class="border-collapse border border-slate-500 bg-slate-800 text-sm shadow-sm">
          <thead class="bg-slate-50 dark:bg-slate-700">
            <tr>
              <th class="border border-slate-600 font-semibold p-4 text-slate-200 text-left">First</th>
              <th class="border border-slate-600 font-semibold p-4 text-slate-200 text-left">First</th>
              <th class="border border-slate-600 font-semibold p-4 text-slate-200 text-left">Last</th>
              <th class="border border-slate-600 font-semibold p-4 text-slate-200 text-left">Phone</th>
              <th class="border border-slate-600 font-semibold p-4 text-slate-200 text-left">Email</th>
              <th class="border border-slate-600 font-semibold p-4 text-slate-200 text-left"></th>
            </tr>
          </thead>
          <tbody>
            {x.contacts.Select(ToHtml).ToHtml()}
          </tbody>
        </table>
        <p>
            <a href="/contacts/new">Add Contact</a>
        </p>
        """;

    public static string ToHtml(this Contact x) => $"""
        <tr>
          <td class="border border-slate-700 p-2 text-slate-400">{x.first}</td>
          <td class="border border-slate-700 p-2 text-slate-400">{x.last}</td>
          <td class="border border-slate-700 p-2 text-slate-400">{x.phone}</td>
          <td class="border border-slate-700 p-2 text-slate-400">{x.email}</td>
          <td class="border border-slate-700 p-2 text-slate-400">
              <a href="/contacts/{x.id}/edit">Edit</a>
              <a href="/contacts/{x.id}">View</a>
          </td>
        </tr>
        """;

    //public static string ToIndex()

    public static string ToLayout(this string x) => $"""
        <!doctype html>
        <html lang="en">
        <head>
            <title>Contact App</title>
            <link rel="stylesheet" href="https://the.missing.style/v0.2.0/missing.min.css">
            <link rel="stylesheet" href="/static/site.css">
            <script src="/static/js/htmx-1.8.0.js"></script>
            <script src="/static/js/_hyperscript-0.9.7.js"></script>
            <script src="/static/js/rsjs-menu.js" type="module"></script>
            <script defer src="https://unpkg.com/alpinejs@3/dist/cdn.min.js"></script>
            <script src="https://cdn.tailwindcss.com"></script>
        </head>
        <body hx-boost="true" class="antialiased text-slate-400 bg-slate-900">
        <main class="font-sans">
            <header>
                <h1 class="text-base font-semibold leading-7 uppercase">contacts.app</h1>
                <h2 class="text-base font-semibold leading-7">A Demo Contacts Application</h2>
            </header>
            {x}
        </main>
        </body>
        </html>
        """;
}

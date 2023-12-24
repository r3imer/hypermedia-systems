namespace Reim.Htmx.Web.Template;

public static partial class Template {

    public static string ToHtml(this string[] x) => string.Join("\n", x);
    public static string ToHtml(this IEnumerable<string> x) => string.Join("\n", x);
    public static IResult AsHtml(this string x) => Results.Content(x, "text/html; charset=utf-8");

}

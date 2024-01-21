namespace Reim.Htmx.Web.Template;

public static partial class Template {

    public static string Join(this string[] x) => string.Join(Environment.NewLine, x);
    public static string Join(this IEnumerable<string> x) => string.Join(Environment.NewLine, x);

    //public static ContentHttpResult AsHtml(this string x) {
    //    return TypedResults.Content(x, "text/html; charset=utf-8");
    //}
    public static IResult AsHtml(this string? x) {
        return Results.Content(x, "text/html; charset=utf-8");
    }
    public static IResult AsHxml(this string? x) {
        return Results.Content(x, "application/vnd.hyperview+xml");
    }

}

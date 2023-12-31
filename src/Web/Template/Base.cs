using Microsoft.AspNetCore.Http.HttpResults;

namespace Reim.Htmx.Web.Template;

public static partial class Template {

    public static string ToHtml(this string[] x) => string.Join("\n", x);
    public static string ToHtml(this IEnumerable<string> x) => string.Join("\n", x);

    //public static ContentHttpResult AsHtml(this string x) {
    //    return TypedResults.Content(x, "text/html; charset=utf-8");
    //}
    public static IResult AsHtml(this string? x) {
        return Results.Content(x, "text/html; charset=utf-8");
    }

}

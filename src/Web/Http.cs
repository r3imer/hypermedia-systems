using Microsoft.Net.Http.Headers;
using System.Net.Mime;

namespace Reim.Http;

public static class Http {
    public static void Response303(this HttpContext x, string url) {
        x.Response.Headers[HeaderNames.Location] = url;
        x.Response.StatusCode = StatusCodes.Status303SeeOther;
    }

    public static async Task Response200Html(
        this HttpContext x,
        string html,
        CancellationToken cancel = default
    ) {
        x.Response.StatusCode = StatusCodes.Status200OK;
        x.Response.ContentType = MediaTypeNames.Text.Html;
        await x.Response.WriteAsync(html, cancel);
    }

    public static async Task Response200Json<T>(
        this HttpContext x,
        T type,
        CancellationToken cancel = default
    ) {
        x.Response.StatusCode = StatusCodes.Status200OK;
        await x.Response.WriteAsJsonAsync(type, cancel);
    }
}

//public class Test0 : ControllerBase {

//    public void Testing() {
//        string location = this.Url.Action("MyAction", new { resourceId = 0 });
//        this.Response.Headers.Add(HeaderNames.Location, location);
//        this.StatusCode(StatusCodes.Status303SeeOther);
//    }
//}

//public class Test1 : Controller {

//    public void Testing() {
//        string location = this.Url.Action("MyAction", new { resourceId = 0 });
//        this.Response.Headers.Add(HeaderNames.Location, location);
//        this.StatusCode(StatusCodes.Status303SeeOther);
//    }
//}




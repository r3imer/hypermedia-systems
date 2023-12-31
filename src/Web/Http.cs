using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Reim.Http;

public static class Http {
    public static HttpResponse Response303(this HttpContext x, string url) {
        x.Response.Headers[HeaderNames.Location] = url;
        x.Response.StatusCode = StatusCodes.Status303SeeOther;
        return x.Response;
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




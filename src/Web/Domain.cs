namespace Reim.Htmx.Web;

public static class Domain {

    public static Contact To(this ContactDto x) => new() {
        email = x.email,
        first = x.first_name,
        last = x.last_name,
        phone = x.phone,
    };
}

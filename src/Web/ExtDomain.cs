namespace Reim.Htmx.Web;

public static class ExtDomain {
    public static ContactDto ToDto(this ContactForm x, int? id)
        => new(x.first_name, x.last_name, x.phone, x.email, id);
    public static ContactDto ToDto(this Contact x)
        => new(x.first, x.last, x.phone, x.email, x.id);
}

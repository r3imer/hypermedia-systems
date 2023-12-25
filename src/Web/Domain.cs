namespace Reim.Htmx.Web;

public static class Domain {

    public static Contact To(this ContactDto x) => new() {
        email = x.email,
        first = x.first_name,
        last = x.last_name,
        phone = x.phone,
    };

    public static bool Update(this Contact old, ContactDto dto) {
        old.email = dto.email;
        old.phone = dto.phone;
        old.first = dto.first_name;
        old.last = dto.last_name;
        return true;
    }
}

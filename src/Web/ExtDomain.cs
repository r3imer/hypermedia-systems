using System.Text.Json;

namespace Reim.Htmx.Web;

public static class Const {
    public const int PAGE_SIZE = 10;
}

public static class ExtDomain {
    private static readonly JsonSerializerOptions _opts = new() { WriteIndented = true };

    public static ContactDto ToDto(this ContactForm x, int? id)
        => new(x.first_name, x.last_name, x.phone, x.email, id);
    public static ContactDto ToDto(this Contact x)
        => new(x.first, x.last, x.phone, x.email, x.id);
    public static byte[] ToJsonBytes<T>(this T x)
        => JsonSerializer.SerializeToUtf8Bytes(x, _opts);
}

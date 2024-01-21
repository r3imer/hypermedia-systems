using Reim.Htmx.Archiver;

namespace Reim.Htmx.Web.Template;

public static partial class Hxml {

    public static string HxmlIndex(this Contacts a, IArchiver? b = null) => $$"""
<form>
    <text-field name="q" value="" placeholder="Search..." style="search-field" />
    <list id="contacts-list">
        {{ a.HxmlRows() }}
    </list>
</form>
""";

    public static string HxmlRows(this Contacts a) => """
<items xmlns="https://hyperview.org/hyperview">
""" + a.arr.Select(b => $$"""
    <item key="{{ b.id }}" style="contact-item">
      <text style="contact-item-label">
        {{ b switch {
        { first: string first } => $"{first} {b.last}",
        { phone: string phone } => phone,
        { email: string email } => email,
        _ => "",
        }}}
      </text>
    </item>
""").Join() + """
</items>
""";

}

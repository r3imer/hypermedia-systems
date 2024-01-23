using Reim.Htmx.Archiver;

namespace Reim.Htmx.Web.Template;

public static partial class Hxml {

    public static string HxmlIndex(this Contacts a, IArchiver? b = null) => $$"""
<form style="contacts-form">
    <text-field name="q" value="" placeholder="Search..." style="search-field">
      <behavior trigger="change"
                action="replace-inner"
                target="contacts-list"
                href="/mobile/contacts?rows_only=true"
                verb="get"/>
    </text-field>
    <list id="contacts-list"
          trigger="refresh"
          action="replace-inner"
          target="contacts-list"
          href="/mobile/contacts?rows_only=true"
          verb="get"
          >
        {{ a.HxmlRows() }}
    </list>
</form>
""";

    public static string HxmlRows(this Contacts a) => $$"""
<items xmlns="https://hyperview.org/hyperview">
  {{ a.arr.Select(b => $$"""
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
""").Join()
  }}
  {{ a.arr.Length switch {
        >0 => $$"""
    <item key="load-more"
          style="Spinner"
          action="replace"
          trigger="visible"
          href="/mobile/contacts?rows_only=true&page={{ a.q.page + 1 }}&q={{ a.q.q }}"
          verb="get"
    >
      <spinner />
    </item>
""",
        _ => "",
  }}}
</items>
""";

}

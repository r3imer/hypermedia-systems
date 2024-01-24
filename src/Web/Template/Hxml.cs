using Reim.Htmx.Archiver;
using Reim.Std.Domain;

namespace Reim.Htmx.Web.Template;

public static partial class HxmlTemplates {

    public static HxmlLayout HxmlIndex(this Contacts a, IArchiver? b = null) => new($$"""
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
""");

    public static Hxml HxmlRows(this Contacts a) => new($$"""
<items xmlns="https://hyperview.org/hyperview">
  {{ a.arr.Select(contact => $$"""
    <item key="{{ contact.id }}" style="contact-item">
      <behavior trigger="press" action="push" href="/mobile/contacts/{{ contact.id }}" />
      <text style="contact-item-label">
        {{ contact switch {
        { first: string first } => $"{first} {contact.last}",
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
""");


    public static HxmlLayout HxmlShow(this ContactDto contact) => new(
        header: $$"""
        <text style="header-button">
          <behavior trigger="press" action="back" />
          Back
        </text>
        <text style="header-button">
          <behavior trigger="press" action="reload" href="/mobile/contacts/{{contact.id}}/edit" />
          Edit
        </text>
        """,
        content: $$"""
        <view style="details">
          <text style="contact-name">{{ contact.first_name }} {{ contact.last_name }}</text>

          <view style="contact-section">
            <text style="contact-section-label">Phone</text>
            <text style="contact-section-info">{{contact.phone}}</text>
          </view>

          <view style="contact-section">
            <text style="contact-section-label">Email</text>
            <text style="contact-section-info">{{contact.email}}</text>
          </view>
        </view>
        """);

    public static HxmlLayout HxmlEdit(this ContactDto contact, Errors? errors, bool saved) => new(
        header: $$"""
        <text style="header-button">
          <behavior trigger="press" action="back" href="#" />
          Back
        </text>
        """,
        content: $$"""
        <form>
          <view id="form-fields">
            {{ contact.HxmlFields(errors, saved) }}
          </view>

          <view style="button">
            <behavior
              trigger="press"
              action="replace-inner"
              target="form-fields"
              href="/mobile/contacts/{{contact.id}}/edit"
              verb="post"
            />
            <text style="button-label">Save</text>
          </view>
          <view style="button">
            <behavior
              trigger="press"
              action="reload"
              href="/mobile/contacts/{{contact.id}}"
            />
            <text style="button-label">Cancel</text>
          </view>
        </form>
        """);

    public static string HxmlFields(this ContactDto contact, Errors? errors, bool saved) => $$"""
<view style="edit-group" xmlns="https://hyperview.org/hyperview">
  {{( saved ? $$"""
    <behavior
      trigger="load"
      action="reload"
      href="/mobile/contacts/{{contact.id}}"
    />
""" : "" )}}
  <view style="edit-field">
    <text-field name="first_name" placeholder="First name" value="{{ contact.first_name }}" />
    <text style="edit-field-error">{{ errors?["first_name"] }}</text>
  </view>

  <view style="edit-field">
    <text-field name="last_name" placeholder="Last name" value="{{ contact.last_name }}" />
    <text style="edit-field-error">{{ errors?["last_name"] }}</text>
  </view>

  <view style="edit-field">
    <text-field name="email" placeholder="Email" value="{{ contact.email }}" />
    <text style="edit-field-error">{{ errors?["email"] }}</text>
  </view>

  <view style="edit-field">
    <text-field name="phone" placeholder="Phone" value="{{ contact.phone }}" />
    <text style="edit-field-error">{{ errors?["phone"] }}</text>
  </view>

</view>
""";

}

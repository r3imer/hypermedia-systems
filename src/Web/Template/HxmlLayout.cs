namespace Reim.Htmx.Web.Template;

public static partial class Hxml {

    public static string HxmlLayout(this string x, string? header = null) => $$"""
<doc xmlns="https://hyperview.org/hyperview">
  <screen>
    <styles>
      <style id="header" flexDirection="row" justifyContent="space-between" alignItems="center" borderBottomColor="#ccc" borderBottomWidth="1" paddingLeft="24" paddingRight="24" paddingVertical="16" backgroundColor="white" />
      <style id="header-title" fontSize="16" color="black" fontWeight="500" />
      <style id="header-button" fontSize="16" color="blue" />

      <style id="body" flex="1" />
      <style id="main" flex="1" backgroundColor="#eee" />

      <style id="search-field" paddingHorizontal="24" paddingVertical="8" borderBottomWidth="1" borderBottomColor="#ddd" backgroundColor="#eee" />

      <style id="contact-item" borderBottomColor="#ddd" borderBottomWidth="1" paddingLeft="24" paddingRight="24" paddingVertical="16" backgroundColor="white" />
      <style id="contact-item-label" fontWeight="500" />
      <style id="load-more-item" paddingVertical="16" />

      <style id="contact-name" fontSize="24" textAlign="center" marginVertical="32" fontWeight="500" />
      <style id="contact-section" margin="8" backgroundColor="white" borderRadius="8" padding="8" marginHorizontal="24" />
      <style id="contact-section-label" fontSize="12" color="#aaa" marginBottom="4" />
      <style id="contact-section-info" fontSize="14" fontWeight="500" color="blue" />

      <style id="edit-group" marginVertical="32" />
      <style id="edit-field" borderBottomWidth="1" borderColor="#ddd" paddingHorizontal="24" paddingVertical="16" backgroundColor="white" />
      <style id="edit-field-error" color="red" fontSize="12" marginTop="4" />

      <style id="button" borderBottomWidth="1" borderColor="#ddd" paddingHorizontal="24" paddingVertical="16" backgroundColor="white" />
      <style id="button-label" color="blue" fontWeight="500" />
      <style id="button-label-delete" color="red" />
    </styles>
    <body style="body" safe-area="true">
      <header style="header">
      {{ header ?? """
        <text style="header-title">Contacts</text>
"""   }}
      </header>
      <view style="main">
        {{ x }}
      </view>
    </body>
  </screen>
</doc>
""";

}

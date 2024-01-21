namespace Reim.Htmx.Web.Template;

public static partial class Hxml {

    public static string HxmlLayout(this string x, string? header = null) => $$"""
<doc xmlns="https://hyperview.org/hyperview">
  <screen>
    <styles>
      <style id="body" backgroundColor="white" flex="1" paddingTop="48" />
      <style id="" />
      <style
        id="contact-item"
        alignItems="center"
        borderBottomColor="#eee"
        borderBottomWidth="1"
        flex="1"
        flexDirection="row"
        height="48"
        justifyContent="space-between"
        paddingLeft="24"
        paddingRight="24"
      />
      <style id="contact-item-label" fontSize="18" />
    </styles>
    <body style="body" safe-area="true">
      <header style="header">
      {{ header ?? """
        <text style="header-title">Contact.app</text>
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

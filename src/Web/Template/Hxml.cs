namespace Reim.Htmx.Web.Template;

public static class Hxml {
    public static string HalloWorld() => $$"""
<doc xmlns="https://hyperview.org/hyperview">
  <screen>
    <styles>
      <style class="body" flex="1" flexDirection="column" />
      <style class="header" borderBottomWidth="1" borderBottomColor="#ccc" />
      <style class="main" margin="24" />
      <style class="h1" fontSize="32" />
      <style class="info" color="blue" />
    </styles>

    <body style="body">
      <header style="header">
        <text style="info">My first app</text>
      </header>
      <view style="main">
        <text style="h1 info">Hello World!</text>
      </view>
    </body>
  </screen>
</doc>
""";
}

using Reim.Htmx.Archiver;

namespace Reim.Htmx.Web.Template;

public static partial class Template {

    public static string HtmlArchiver(this IArchiver a) {
        int p = (int) (a.progress() * 100);
        return $$"""
            <div id="archive-ui" hx-target="this" hx-swap="outerHTML">
                {{ a.status() switch {
                    Status.Waiting => $$"""
                        <button hx-post="/contacts/archive">
                            Download Contact Archive
                        </button>
                        """,
                    Status.Running => $$"""
                        <div hx-get="/contacts/archive" hx-trigger="load delay:500ms">
                            Creating Archive...
                            <div class="progress" >
                                <div id="archive-progress" class="progress-bar" role="progressbar"
                                     aria-valuenow="{{ p }}"
                                     style="width:{{ p }}%"></div>
                            </div>
                        </div>
                        """,
                    Status.Complete => """
                        <a hx-boost="false" href="/contacts/archive/file"
                           _="on load click() me">
                            Archive Ready!  Click here to download. &downarrow;
                        </a>
                        <button hx-delete="/contacts/archive">Clear Download</button>
                        """,
                }}}
            </div>
            """;
    }


    public static string HxmlIndex(this Contacts a, IArchiver? b = null) => $$"""
<form>
    <text-field name="q" value="" placeholder="Search..." style="search-field" />
    <list id="contacts-list">
        {{ a.HxmlRows() }}
    </list>
</form>
""";

    public static string HtmlIndex(this Contacts a, IArchiver b) => $$"""
        <p>{{ b.HtmlArchiver() }}</p>
        <form action="/contacts" method="get" class="tool-bar">
            <label for="search">Search Term</label>
            <input id="search" type="search" name="q" value="{{ a.q.q }}"
                   _="on keydown[altKey and code is 'KeyS'] from the window me.focus()"
                   hx-get="/contacts"
                   hx-target="tbody"
                   hx-select="tbody tr"
                   hx-push-url="true"
                   hx-indicator="#spinner-search"
                   hx-trigger="keyup delay:300ms changed"/>
            <input type="submit" value="Search" />
            <img style="height: 26px" id="spinner-search" class="htmx-indicator" src="/static/img/tail-spin.svg"/>
        </form>
        <form x-data="{ selected: [] }">
            <template x-if="selected.length > 0">
                <div class="box info tool-bar flxed top">
                    <slot x-text="selected.length"></slot>
                    contacts selected
                    
                    <button type="button" class="bad bg color border"
                            @click="confirm(`Delete ${selected.length} contacts?`) &&
                            htmx.ajax('DELETE', '/contacts', { source: $root, target: document.body })">
                        Delete
                    </button>
                    <hr aria-orientation="vertical">
                    <button type="button" @click="selected = []">Cancel</button> 
                </div>
            </template>
            <table>
                <thead>
                    <tr>
                        <th></th>
                        <th>First</th>
                        <th>Last</th>
                        <th>Phone</th>
                        <th>Email</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {{ a.HtmlRows() }}
                </tbody>
            </table>
            <button hx-delete="/contacts"
                    hx-confirm="Are you sure you want to delete these contacts?"
                    hx-target="body">
                Delete Selected Contacts
            </button>
        </form>
        <p>
            <a href="/contacts/new">Add Contact</a>
            <span hx-get="/contacts/count" hx-trigger="load">
                <img style="height: 20px" class="my-indicator" src="/static/img/tail-spin.svg"/>
            </span>
        </p>
        """;

    public static string HtmlRows(this Contacts a) => a.arr.Select(b => $$"""
        <tr>
            <td><input type="checkbox" name="selected_contact_ids" value="{{ b.id }}" x-model="selected"></td>
            <td>{{ b.first }}</td>
            <td>{{ b.last }}</td>
            <td>{{ b.phone }}</td>
            <td>{{ b.email }}</td>
            <td>
                <div data-overflow-menu>
                    <button type="button" aria-haspopup="menu"
                            aria-controls="contact-menu-{{ b.id }}">
                        Options
                    </button>
                    <div role="menu" hidden id="contact-menu-{{ b.id }}">
                        <a role="menuitem" href="/contacts/{{ b.id }}/edit">Edit</a>
                        <a role="menuitem" href="/contacts/{{ b.id }}">View</a>
                        <a role="menuitem" href="#"
                           hx-delete="/contacts/{{ b.id }}"
                           hx-confirm="Are you sure you want to delete this contact?"
                           hx-swap="outerHTML swap:1s"
                           hx-target="closest tr">Delete</a>
                    </div>
                </div>
            </td>
        </tr>
        """).Join() + ( a.arr.Length == Const.PAGE_SIZE ? $$"""
            <tr>
                <td colspan="5" style="text-align: center">
                    <button hx-target="closest tr"
                            hx-swap="outerHTML"
                            hx-select="tbody > tr"
                            hx-get="/contacts?page={{ a.q.page + 1 }}&q={{ a.q.q }}">
                        Load More
                        <img style="height: 20px" class="my-indicator" src="/static/img/tail-spin.svg"/>
                    </button>
                </td>
            </tr>
        """ : "");

    public static string HxmlRows(this Contacts a) => """
<items xmlns="https://hyperview.org/hyperview">
""" + a.arr.Select(b => $$"""
    <item key="{{ b.id }}" style="contact-item"> (3)
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

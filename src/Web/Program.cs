using Reim.Htmx.Web;
using System.Text.Json;
using Microsoft.AspNetCore.HttpLogging;
using NLog.Web;

ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddNLog("nlog.config"));
ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
logger.LogInformation("init main");

var bldr = WebApplication.CreateBuilder(args);
bldr.Logging.ClearProviders();
bldr.Host.UseNLog();

var serv = bldr.Services;

serv.AddAntiforgery();

//serv.AddSingleton<DB>();
serv.AddSingleton<ContactsRepo>(_
    => new(JsonSerializer.Deserialize<Contact[]>(
        File.ReadAllText("contacts2.json")) ?? []
    )
);

serv.AddHttpLogging(opts => {
    opts.CombineLogs = true;
    opts.RequestHeaders.Add("HX-Boosted");
    opts.RequestHeaders.Add("HX-Current-URL");
    opts.RequestHeaders.Add("HX-History-Restore-Request");
    opts.RequestHeaders.Add("HX-Prompt");
    opts.RequestHeaders.Add("HX-Request");
    opts.RequestHeaders.Add("HX-Target");
    opts.RequestHeaders.Add("HX-Trigger-Name");
    opts.RequestHeaders.Add("HX-Trigger");
    opts.LoggingFields =
        HttpLoggingFields.Duration 
        | HttpLoggingFields.ResponseStatusCode
        | HttpLoggingFields.ResponseHeaders
        | HttpLoggingFields.RequestHeaders
        ;
});

var app = bldr.Build();
var log = app.Services.GetService<ILogger<Program>>()!;

//app.UseMiddleware<CatchExceptions>();
app.UseStaticFiles();
app.UseHttpLogging();
app.UseAntiforgery();

app.Use((ctxt, next) => {
    //log.
    var r = next(ctxt);
    return r;
});

app.MapGet("/", () => Results.Redirect("/contacts"));

app.MapGroup("/contacts")
    .DisableAntiforgery()
    .MapWebEndpoints()
    ;

try {
    app.Run();
} catch (Exception ex) {
    logger.LogError(ex, "Stopped program because of exception");
    throw;
} finally {
    NLog.LogManager.Shutdown();
}

using Microsoft.AspNetCore.HttpLogging;
using NLog.Web;
using Reim.Htmx.Archiver;
using Reim.Htmx.Web;
using System.Text.Json;

ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddNLog("nlog.config"));
ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
logger.LogInformation("init main");

var bldr = WebApplication.CreateBuilder(args);
bldr.Logging.ClearProviders();
bldr.Host.UseNLog();

var serv = bldr.Services;

var AllowOrigins = "AllowOrigins";
serv.AddCors(opts => {
    opts.AddPolicy(name: AllowOrigins, policy => {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

serv.AddAntiforgery();

//serv.AddSingleton<DB>();
serv.AddSingleton<ContactsRepo>(_
    => new(JsonSerializer.Deserialize<Contact[]>(
        File.ReadAllText("contacts2.json")) ?? []
    )
);
serv.AddSingleton<IArchiver, FakeTimeArchiver>();
//serv.AddSingleton<IArchiver, FakeCountArchiver>();

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
        | HttpLoggingFields.ResponseBody
        ;
});

var app = bldr.Build();
var log = app.Services.GetService<ILogger<Program>>()!;

//app.Use(async (ctxt, next) => {
//    await next(ctxt);
//    if (ctxt.Response.StatusCode is 204) {
//        log.LogInformation("---204---");
//    }
//});

//app.UseMiddleware<CatchExceptions>();
app.UseStaticFiles();
app.UseHttpLogging();
app.UseAntiforgery();
app.UseCors();

app.MapGet("/", () => Results.Redirect("/contacts"));

app.MapGroup("/mobile/contacts")
    .RequireCors(AllowOrigins)
    .DisableAntiforgery()
    .MapMobileEndpoints()
    ;

app.MapGroup("/contacts")
    .DisableAntiforgery()
    .MapWebEndpoints()
    ;

app.MapGroup("api/v1/contacts")
    .MapApiEndpointsV1()
    ;

try {
    app.Run();
} catch (Exception ex) {
    logger.LogError(ex, "Stopped program because of exception");
    throw;
} finally {
    NLog.LogManager.Shutdown();
}

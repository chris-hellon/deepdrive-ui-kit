using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using DeepDrive.UiKit.Demo.Components;
using DeepDrive.UiKit.Demo.Widgets;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// --- DeepDrive UI Kit demo wiring ---
// In-memory sample data + FluentValidation for the form models…
builder.Services.AddSingleton<WidgetStore>();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// …and EasyApi so ICall<TRequest,TResult> dispatches to the handlers in this assembly.
builder.Services
    .AddEasyApi()
    .WithContract(typeof(Program).Assembly)
    .WithServer();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

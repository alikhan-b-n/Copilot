using Copilot.Admin;
using Copilot.Admin.Data.Apis;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Copilot.Admin.Data.Services;
using Copilot.WhatsApp.Api.Interfaces;
using Copilot.WhatsApp.Api.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Builder;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

var url = builder.Configuration["WebApiUrl"] 
          ?? throw new ArgumentException("Cannot find WebApiUrl in the Configuration file");

builder.Services.AddScoped(
    sp => new HttpClient
    {
        BaseAddress = new Uri(url)
    });

#region Apis

builder.Services.AddScoped<IFlowSellApi, FlowSellApi>();
builder.Services.AddScoped<WhatsAppApi>();
builder.Services.AddScoped<AltegioCompanyApi>();
builder.Services.AddScoped<UserApi>();
builder.Services.AddScoped<GptModelApi>();
builder.Services.AddScoped<ChatBotApi>();
builder.Services.AddScoped<DialoguesApi>();
builder.Services.AddScoped<PluginApi>();

#endregion

#region Authentication

builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

builder.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

#endregion

builder.Services.AddAntDesign();

var app = builder.Build();

await app.RunAsync();
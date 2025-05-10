using System.Text.Json.Serialization;
using Copilot.Ai;
using Copilot.Api.Extentions;
using Copilot.Api.Hubs;
using Copilot.ChatBotManagement;
using Copilot.Infrastructure;
using Copilot.Infrastructure.Entities;
using Copilot.MessageHandling;
using Copilot.WhatsApp.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMessageHandlingServices(builder.Configuration)
    .AddWhatsAppApi(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddChatBotManagement(builder.Configuration)
    .AddCopilotAi(builder.Configuration);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwagger();

builder.Services.AddSignalR();

builder.Services
    .AddIdentity()
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "AllowAll",
        configurePolicy: policyBuilder =>
        {
            policyBuilder.AllowAnyHeader();
            policyBuilder.AllowAnyMethod();
            policyBuilder.SetIsOriginAllowed(_ => true);
            policyBuilder.AllowCredentials();
        });
});

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

app.MapGroup("/identity")
    .MapIdentityApi<User>();

app.MapHub<TestChatHub>("chathub");

app.MapControllers();
app.MapGet("/", (IHttpContextAccessor httpContextAccessor) =>
    $"Hey! You can find docs here: {httpContextAccessor.HttpContext!.Request.Host}/swagger");

app.Run();
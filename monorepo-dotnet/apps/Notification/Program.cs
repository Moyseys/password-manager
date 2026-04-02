using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Notification.Interfaces;
using Notification.Options;
using Notification.Services;
using SimpleMq.Config;
using SimpleMq.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions<EmailSettingsOptions>()
    .Bind(builder.Configuration.GetSection(nameof(EmailSettingsOptions)))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<EmailSettingsOptions>>().Value
);
builder.Services.AddScoped<IEmailService, EmailService>();

var mqConfig = new NotificationMqConfig();

builder.Services.AddSimpleMQ(
    builder.Configuration,
    exchangeConfig: mqConfig,
    queueConfig: mqConfig,
    bindConfig: mqConfig
);

var app = builder.Build();
await app.RunAsync();
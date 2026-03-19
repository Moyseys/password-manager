using Microsoft.Extensions.Hosting;
using SimpleMq.Config;
using SimpleMq.Extensions;

var builder = Host.CreateApplicationBuilder(args);
var mqConfig = new NotificationMqConfig();

builder.Services.AddSimpleMQ(
    builder.Configuration,
    exchangeConfig: mqConfig,
    queueConfig: mqConfig,
    bindConfig: mqConfig
);

var app = builder.Build();
await app.RunAsync();
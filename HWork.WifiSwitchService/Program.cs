using HWork.WifiSwitchService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<WifiSwitchackgroundService>();

var host = builder.Build();
host.Run();

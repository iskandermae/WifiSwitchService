using HWork.WifiSwitchService;

internal class Program {
    private static void Main(string[] args) {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddWindowsService(options =>
        {
            options.ServiceName = "Better WIFI Service";
        });
        builder.Services.AddHostedService<WifiSwitchackgroundService>();
        var host = builder.Build();
        host.Run();
    }
}